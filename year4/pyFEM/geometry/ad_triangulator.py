from collections import defaultdict
import math

from pyFEM.geometry import Point, Edge, Polygon
from pyFEM import symbols as sym


def uniq(sorted_seq):
    res = [sorted_seq[0]]
    for i in range(1, len(sorted_seq)):
        if sorted_seq[i] != sorted_seq[i - 1]:
            res.append(sorted_seq[i])
    return res

class MonotonePolygonTriangulator(object):
    """Class for triangulating monotone polygons (with no intruding vertices)"""
    def __init__(self, diameter, polygon):
        self.diameter = diameter
        self.polygon = polygon
        self.triangles = []
        
        self.precalculated_polygon = None
        self.points_to_triangles = None
        self.points_to_points = None
        self.bound_points = None
        self.bounds_hasn = None

    def triangulate(self):
        self.triangles = []
        self.curr_polygon = self.precalculate_polygon()
        self.curr_angles = self.precalculate_angles(self.curr_polygon)
        self.points_to_points = defaultdict(list)
        self.points_to_triangles = defaultdict(list)
        
        self.inner_triangulate()
        
        return self.triangles

    def precalculate_polygon(self):
        diameter = self.diameter
        self.bounds_hash = {}
        my_polygon = Polygon()
        for i in range(len(self.polygon)):
            point1 = self.polygon.points[i - 1]
            point2 = self.polygon.points[i]
            my_polygon.extend(Edge.split(point1, point2, diameter))
        self.precalculated_polygon = Polygon.copy(my_polygon.points)
        return my_polygon

    def get_bound_points(self, subdivide_n=0):
        self.bound_points = {}

        precalculated_copy = Polygon.copy(self.precalculated_polygon.points)
        precalculated_copy.subdivide(subdivide_n)
        
        for p in precalculated_copy:
            self.bound_points[p] = True
        return self.bound_points

    def precalculate_angles(self, polygon):
        angles = {}
        for i in range(len(polygon)):
            angles[polygon.points[i]] = polygon.get_angle(i)
        return sorted(angles.items(), cmp=lambda a, b: cmp(a[1], b[1]))

    def inner_find_point(self, p1, p2, p3, s2, s3, s, angle):
        aa = s3*s3 + s*s - 2*s3*s*math.cos(angle/2.0)
        bb = s2*s2 + s*s - 2*s2*s*math.cos(angle/2.0)

        k = s*s - aa + (p2.x**2 - p1.x**2) + (p2.y**2 - p1.y**2)
        l = aa - bb + (p3.x**2 - p2.x**2) + (p3.y**2 - p2.y**2)

        m = 2.0*(p2.x - p1.x)
        n = 2.0*(p2.y - p1.y)
        p = 2.0*(p3.x - p2.x)
        q = 2.0*(p3.y - p2.y)

        det = n*p - q*m
        x_new = (l*n - k*q)/det
        y_new = (k*p - l*m)/det

        return Point(x_new, y_new)

    def process_polygon4(self, polygon):
        angles = [polygon.get_angle(i) for i in range(len(polygon.points))]
        tr = None
        if angles[0] + angles[2] >= angles[1] + angles[3]:
            tr = polygon.split_indices(0, 2)
        else:
            tr = polygon.split_indices(1, 3)

        self.triangles.append(tr)
        self.save_triangle_points(tr, len(self.triangles) - 1)

        self.triangles.append(polygon) 
        self.save_triangle_points(polygon, len(self.triangles) - 1)

        return polygon

    def find_index(self, angle, point, values):
        left, right = 0, len(values) - 1
        while left <= right:
            m = (left + right) // 2
            if values[m][1] == angle: break
            if angle > values[m][1]:
                left = m + 1
            else:
                right = m - 1
        if left <= right:
            return [True, m]
        else:
            return [False, left]

    def find_by_angle(self, angle, point, values):
        index = self.find_index(angle, point, values)
        while (values[index[1] - 1][1] == angle) and (index[1] > 0):
            index[1] -= 1
        return index

    def find_by_point(self, angle, point, values):
        try:
            index = [x[0] for x in values].index(point)
        except ValueError:
            return [False, None]
        return [True, index]

    def delete_vertex(self, angle, point, values):
        index = self.find_by_point(angle, point, values)
        if not index[0]:
            raise ValueError("Vertex to delete not found")
        del values[index[1]]

    def change_alpha(self, point, alpha, new_alpha, values):
        index = self.find_by_point(alpha, point, values)
        if not index[0]:
            raise ValueError("Alpha to change not found")
        if values[index[1]][1] != alpha:
            raise ValueError("Found bad value")

        del values[index[1]]
        new_index = self.find_by_angle(new_alpha, point, values)
        values.insert(new_index[1], [point, new_alpha])
        return True

    def insert_alpha_point(self, alpha, point, values):
        new_index = self.find_by_angle(alpha, point, values)
        values.insert(new_index[1], [point, alpha])

    def cut_off_vertex(self, index, alpha):
        new_angle = Edge.angle_points_orient(self.curr_polygon.next(),
                                             self.curr_polygon.prev(),
                                             self.curr_polygon.prev(2))
        alpha1 = self.curr_polygon.get_angle(index - 1)
        self.change_alpha(self.curr_polygon.prev(), alpha1, new_angle, self.curr_angles)
        new_angle = Edge.angle_points_orient(self.curr_polygon.next(2),
                                             self.curr_polygon.next(),
                                             self.curr_polygon.prev())
        alpha2 = self.curr_polygon.get_angle(index + 1)
        self.change_alpha(self.curr_polygon.next(), alpha2, new_angle, self.curr_angles)
        new_polygon = self.curr_polygon.split_indices(index - 1, index + 1)
        if len(self.curr_polygon) == 3:
            self.curr_polygon, new_polygon = new_polygon, self.curr_polygon

        self.triangles.append(new_polygon)
        self.save_triangle_points(new_polygon, len(self.triangles) - 1)

    def save_triangle_points(self, triangle, index):
        for p in triangle.points:
            self.points_to_triangles[p].append(index)

        for p in triangle.points:
            for inn_p in triangle.points:
                if inn_p == p: continue
                self.points_to_points[p].append(inn_p)
            self.points_to_points[p].sort()
            self.points_to_points[p] = uniq(self.points_to_points[p])

    def process_case2(self, angle):
        p1 = self.curr_polygon.curr_point
        p2 = self.curr_polygon.prev()
        p3 = self.curr_polygon.next()

        s2 = (p3 - p1).length()
        s3 = (p2 - p1).length()
        s1 = (self.curr_polygon.next(2) - self.curr_polygon.next()).length()
        s4 = (self.curr_polygon.prev() - self.curr_polygon.prev(2)).length()

        s = (s1 + (2 * s2) + (2 * s3) + s4) / 6.0
        p = self.inner_find_point(p1, p2, p3, s2, s3, s, angle)
        if self.curr_polygon.has_point(p):
            return p
        raise RuntimeError("doh")

    def process_case3(self, angle):
        l1 = Edge.length(self.curr_polygon.next(), self.curr_polygon.curr_point)
        l2 = Edge.length(self.curr_polygon.curr_point, self.curr_polygon.prev())

        p2 = self.curr_polygon.curr_point
        p1 = self.curr_polygon.prev()
        a = l2

        index = self.curr_polygon.curr_index

        if l1 < l2:
            p1 = self.curr_polygon.curr_point
            p2 = self.curr_polygon.next()
            a = l1
            index += 1

            p_middle = Point.new((p1.x + p2.x)/2.0, (p1.y + p2.y)/2.0)
    
        m = 2.0*(p2.x - p1.x)
        n = 2.0*(p2.y - p1.y)
        k = ((p2.x**2) + (p2.y**2)) - ((p1.x**2) + (p1.y**2))

        p = p2.y - p1.y
        q = p1.x - p2.x
        l = p1.x*p2.y - p2.x*p1.y + Math.sqrt(3.0)*a*a/2.0

        det = n*p - q*m
        x_new = (l*n - k*q)/det
        y_new = (k*p - l*m)/det

        p = Point.new(x_new, y_new)

        if self.curr_polygon.has_point(p):
            return [p, index]

        # find symmetric point
        p_new = Point(2.0*p.x - p_middle.x, 2.0*p.y - p_middle.y)
        if p_new in self.curr_polygon:
            return [p_new, index] 

        raise RuntimeError("doh doh doh 3")

    def inner_triangulate(self):
        count = 0

        if len(self.curr_polygon) < 3:
            return self.curr_polygon 

        if len(self.curr_polygon) == 3:
            self.triangles.append(self.curr_polygon)
            return self.curr_polygon

        if len(self.curr_polygon) == 4:
            return self.process_polygon4(self.curr_polygon)

# main loop
        while self.curr_angles:
            # first stupid/simple situations
            if len(self.curr_polygon) == 4:
                self.process_polygon4(self.curr_polygon)
                break

            if len(self.curr_polygon) == 3:
                self.triangles.append(self.curr_polygon)
                self.save_triangle_points(self.curr_polygon, len(self.triangles) - 1)
                break

            if len(self.curr_polygon) < 3:
                break 

            # and now sophisticated stuff

            # at first delete element with smalest angle
            element = self.curr_angles[0]
            point = element[0]
            angle = element[1]

            del self.curr_angles[0]
            self.curr_polygon.curr_point = point

            # if it is just a cutoff - do it
            # and go to next loop iteration
            if angle < math.pi/2.0:
                #p "upper cutoff"
                self.cut_off_vertex(self.curr_polygon.curr_index, angle)
                continue

            a1 = (math.pi / 2.0)
            a2 = (5.0*math.pi / 6.0)
            a3 = 2.0*math.pi

            bad_point = False
            used_elements = []

            while True: # while bad_point
                if 0 <= angle <= a1:
                     #p "inner cutoff"
                    bad_point = False
                    self.cut_off_vertex(self.curr_polygon.curr_index, angle)
                elif a1 <= angle <= a2:
                    #p 'case 2'
                    p = self.process_case2(angle)
                    if not p:
                        used_elements.append([Point(point.x, point.y), angle])
                        bad_point = True
                    bad_point = False
                    # if all is ok - than we will insert
                    # a new point - need to add first elements
                    if used_elements:
                        self.curr_angles = used_elements + self.curr_angles
                        used_elements = []

                    p1 = self.curr_polygon.curr_point
                    p2 = self.curr_polygon.prev()
                    p3 = self.curr_polygon.next()

                    tr1 = Polygon.copy([p1, p3, p])
                    tr2 = Polygon.copy([p1, p, p2])

                    self.triangles.append(tr1)
                    self.save_triangle_points(tr1, len(self.triangles) - 1)

                    self.triangles.append(tr2)
                    self.save_triangle_points(tr2, len(self.triangles) - 1)

                    angle1 = self.curr_polygon.get_angle(self.curr_polygon.curr_index - 1)
                    angle2 = self.curr_polygon.get_angle(self.curr_polygon.curr_index + 1)

                    self.curr_polygon.replace_current(p)

                    # change angle of previous point
                    new_angle = self.curr_polygon.get_angle(self.curr_polygon.curr_index - 1)
                    self.change_alpha(self.curr_polygon.prev(), angle1, new_angle, self.curr_angles)

                    # add angle of next point
                    new_angle = self.curr_polygon.get_angle(self.curr_polygon.curr_index + 1)
                    self.change_alpha(self.curr_polygon.next(), angle2, new_angle, self.curr_angles)

                    # add angle of current point
                    new_angle = self.curr_polygon.get_angle(self.curr_polygon.curr_index)
                    self.insert_alpha_point(new_angle, self.curr_polygon.curr_point, self.curr_angles)
                elif a2 <= angle <= a3:
                    return
                    # print('case 3')
                    # arr = self.process_case3(angle)
                    # if not arr:
                    #     used_elements.append([Point(point.x, point.y), angle])
                    #     bad_point = True
                    # else:
                    #     bad_point = False            

                    # if used_elements:
                    #     self.curr_angles = used_elements + self.curr_angles
                    #     used_elements = []

                    # p = arr[0]
                    # index = arr[1]

                    # self.curr_polygon.curr_index = index
                    # p2 = self.curr_polygon.curr_point
                    # p1 = self.curr_polygon.prev

                    # tr = Polygon.copy([p1, p2, p])
                    # self.triangles.append(tr)
                    # self.save_triangle_points(tr, len(self.triangles) - 1)

                    # self.curr_polygon.insert_point(index, p)

                    # # insert angle of current vertex (a new vertex)
                    # new_angle = self.curr_polygon.get_angle(index)
                    # self.insert_alpha_point(new_angle, p, self.curr_angles)

                    # new_angle = self.curr_polygon.get_angle(index - 1)
                    # self.change_alpha(self.curr_polygon.prev, angle, new_angle, self.curr_angles)

                    # new_angle = self.curr_polygon.get_angle(index - 2)
                    # self.change_alpha(self.curr_polygon.prevprev, angle, new_angle, self.curr_angles)


                if len(self.curr_angles) <= 4:
                    break

                # break if self.curr_angles.size <= 4

                        #p self.curr_angles
                        #p self.curr_polygon

                if bad_point:
                    element = self.curr_angles[0]
                    point = element[0]
                    angle = element[1]

                    del self.curr_angles[0]

                    self.curr_polygon.curr_point = point

                    print('move next')
                else:
                    if used_elements:
                        self.curr_angles = used_elements + self.curr_angles
                        used_elements.clear

                if not bad_point:
                    break




