import math

from pyFEM.geometry import Point, Edge
from pyFEM import symbols


class Polygon(object):
    def __init__(self):
        self.points = []
        self.curr_index = -1
        self.inner_points = {}

    @staticmethod
    def copy(points):
        res = Polygon()
        res.curr_index = 0
        for p in points:
            res.append(Point.copy(p))
        return res

    @staticmethod
    def get_center(points):
        n = float(len(points))
        avg_x = sum([p.x for p in points]) / n
        avg_y = sum([p.y for p in points]) / n
        return Point(avg_x, avg_y)

    def __len__(self):
        return len(self.points)

    def concat(self, points_arr):
        raise NotImplementedError("Use extend() instead.")
    
    def extend(self, points_arr):
        self.points.extend(points_arr)

    def append(self, p):
        if not self.points:
            self.curr_index = 0
        self.points.append(p)

    def insert_point(self, index, point):
        self.points.insert(index, point)

    def edge(self):
        return Edge(self.curr_point, self.next_point)
    
    @property
    def curr_point(self):
        return self.points[self.curr_index] 

    @curr_point.setter
    def curr_point(self, value):
        try:
            self.curr_index = self.points.index(value)
        except ValueError:
            self.curr_index = None
        return self.curr_point

    def _next_idx(self, n=1):
        return (self.curr_index + n) % len(self.points)

    def _prev_idx(self, n=1):
        return (self.curr_index - n) % len(self.points)
    
    @property
    def next_point(self):
        raise NotImplementedError("next_point stub")

    @property
    def prev_point(self):
        raise NotImplementedError("prev_point stub")

    def move_next(self):
        self.curr_index = self._next_idx()
        return self.curr_point

    def move_prev(self):
        self.curr_index = self._prev_idx()
        return self.curr_point

    def next(self, n=1):
        return self.points[self._next_idx(n)]

    def prev(self, n=1):
        return self.points[self._prev_idx(n)]
    
    def neighbor(self, rotation):
        if rotation is symbols.clockwise:
            return self.next()
        elif rotation is symbols.counterclockwise:
            return self.prev()
        else:
            raise ValueError("Unrecognized rotation: clockwise or counterclockwise expected")

    def advance(self, rotation):
        if rotation is symbols.clockwise:
            return self.move_next()
        elif rotation is symbols.counterclockwise:
            return self.move_prev()
        else:
            raise ValueError("Unrecognized rotation: clockwise or counterclockwise expected")

    def get_angle(self, index):
        index %= len(self.points)
        
        cur_p = self.points[index]
        prev_p = self.points[(index - 1) % len(self.points)]
        next_p = self.points[(index + 1) % len(self.points)]

        angle = Edge.angle_points(prev_p, cur_p, next_p)
        if next_p.classify(prev_p, cur_p) is symbols.left:
            angle = (math.pi * 2.0) - angle
        return angle

    def remove_current(self):
        if not self.points:
            return
        elif len(self.points) == 1:
            self.points = []
            self.curr_index = None
        else:
            del self.points[self.curr_index]
            self.move_prev()

    def get_centroid(self):
        return Polygon.get_center(self.points)
        
    def __iter__(self):
        return self.points.__iter__()

    def replace_point(self, point, new_point):
        i = self.points.index(point)
        self.points[i] = Point.copy(new_point)
        
    def replace_current(self, point):
        self.replace_point(self.curr_point, point)

    def split(self, point):
        if not self.points:
            return None
        idx1 = self.curr_index
        idx2 = self.points.index(point)
        
        tmp_cur = Point.copy(self.curr_point)

        p1 = Point.copy(self.curr_point)
        p2 = Point.copy(point)

        if idx2 < idx1:
            idx1, idx2 = idx2, idx1
            p1, p2 = p2, p1

        self.points.insert(idx1, p1)

        poly2 = Polygon.copy(self.points[idx1:(idx2+1)]) # crude implementation of slice!
        del self.points[idx1:(idx2+1)]
        poly2.curr_point = tmp_cur
        self.curr_point = point

        return poly2

    def has_point(self, p):
        c = 0
        x = p.x
        y = p.y
        for i in range(len(self.points)):
            cp = self.points[i]
            pp = self.points[i - 1]
            if (((cp.y <= y and y < pp.y) or (pp.y <= y and y < cp.y)) and 
                (x > (pp.x - cp.x) * (y - cp.y) / (pp.y - cp.y) + cp.x)):
                c = 1 - c
        return (c == 1)

    def split_points(self, point1, point2):
        try:
            idx1 = self.points.index(point1)
            idx2 = self.points.index(point2)
        except ValueError:
            return None
        return self.split_indices(idx1, idx2)

    def split_indices(self, idx1, idx2):
        idx1 %= len(self.points)
        idx2 %= len(self.points)

        p1 = Point.copy(self.points[idx1])
        p2 = Point.copy(self.points[idx2])

        if idx2 < idx1:
            idx1, idx2 = idx2, idx1
            p1, p2 = p2, p1

        self.points.insert(idx1, p1)
        idx1 += 1
        idx2 += 1
        self.points.insert(idx2, p2)
        
        res = Polygon.copy(self.points[idx1:(idx2+1)])
        del self.points[idx1:(idx2+1)]
        return res

    def __repr__(self):
        return repr(self.points)
        
    def subdivide(self, n=0):
        if n == 0: return
        for i in range(len(self.points)):
            prev_p = self.points[i - 1]
            cur_p = self.points[i]
            arr = tuple(sorted([hash(prev_p), hash(cur_p)]))
            step_v = (cur_p - prev_p) * (1.0 / (n + 1))
            self.inner_points[arr] = [prev_p + (step_v * j) for j in range(1, n+1)]

    @property
    def all_points(self):
        res = []
        for i in range(len(self.points)):
            prev_p = self.points[i - 1]
            cur_p = self.points[i]
            arr = tuple(sorted([hash(prev_p), hash(cur_p)]))
            if arr in self.inner_points:
                res.extend(self.inner_points[arr])
            res.append(cur_p)
        return res
