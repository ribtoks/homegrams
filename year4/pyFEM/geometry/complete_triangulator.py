from collections import defaultdict

from pyFEM.geometry import Point, Edge, Polygon, PolygonDecomposer, MonotonePolygonTriangulator
import pyFEM.symbols as sym


# TODO (TS): move into some common module
def uniq(sorted_seq):
    res = [sorted_seq[0]]
    for i in range(1, len(sorted_seq)):
        if sorted_seq[i] != sorted_seq[i - 1]:
            res.append(sorted_seq[i])
    return res

class Triangulator(object):
    def __init__(self):
        self.initial_polygon = None
        self.diameter = None
        self.triangles = None
        self.points_to_triangles = None
        self.points_to_points = None
        self.bound_points = None
        self.triangles_hash = None
        self.vertices_hash = None

    def triangulate(self, input_polygon, diameter):
        self.initial_polygon = Polygon.copy(input_polygon.points)
        decomposer = PolygonDecomposer()
        polygon = Polygon.copy(input_polygon.points)
        polygons = decomposer.decompose(polygon)
        self.bound_points = {}
        temp_triangulator = MonotonePolygonTriangulator(diameter, Polygon.copy(input_polygon.points))
        temp_triangulator.precalculate_polygon()

        self.bounds_polygon = temp_triangulator.precalculated_polygon
        
        self.bound_points = dict(self.bound_points, **temp_triangulator.get_bound_points())
        self.triangles = []
        for pol in polygons:
            triangulator = MonotonePolygonTriangulator(diameter, pol)
            self.triangles.extend(triangulator.triangulate())
        self.calculate_hashes()
        return self.triangles

    def smooth(self):
        smoothed_triangles = [Polygon.copy(tr.points) for tr in self.triangles]
        for key, value in self.points_to_points.items():
            if key in self.bound_points: continue
            new_point = Polygon.get_center(value)
            for i in self.points_to_triangles[key]:
                smoothed_triangles[i].replace_point(key, new_point)
        self.triangles = smoothed_triangles
        self.calculate_hashes()
        return self.triangles

    def correct_all_triangles(self):
        for tr in self.triangles:
            a, b, c = [tr.points[i] for i in [0, 1, 2]]
            if c.classify(a, b) is not sym.right: continue
            tr.points[0], tr.points[1] = tr.points[1], tr.points[0]

    def enumerate_triangles(self):
        vertex_index = min(range(len(self.initial_polygon.points)),
                           key=self.initial_polygon.get_angle)
        queue = []
        marked = {}
        for tr in self.triangles:
            for p in tr.all_points:
                marked[p] = False

        curr_point = self.initial_polygon.points[vertex_index]
        queue.append(curr_point)
        marked[curr_point] = True

        curr_number = 0
        self.vertices_hash = {}
        self.vertices_hash[curr_point] = curr_number
        curr_number += 1

        while queue:
            point = queue.pop()
            for p in self.points_to_points[point]:
                if marked[p]: continue
                marked[p] = True
                queue.append(p)
                self.vertices_hash[p] = curr_number
                curr_number += 1
                
        return self.vertices_hash

    def subdivide_triangles(self, point_count=2):
        for tr in self.triangles:
            tr.subdivide(point_count)

        self.bounds_polygon.subdivide(point_count)
        for p in self.bounds_polygon.all_points:
            self.bound_points[p] = True
    
    def calculate_hashes(self):
        self.points_to_points = defaultdict(list)
        self.points_to_triangles = defaultdict(list)

        for index in range(len(self.triangles)):
            triangle = self.triangles[index]
            all_points = triangle.all_points
            for point in triangle.points:
                self.points_to_triangles[point].append(index)
            for i, p in enumerate(all_points):
                p_prev = all_points[i - 1]
                p_next = all_points[(i + 1) % len(all_points)]
                self.points_to_points[p].append(p_prev)
                self.points_to_points[p].append(p_next)
                self.points_to_points[p].sort()
                self.points_to_points[p] = uniq(self.points_to_points[p])

