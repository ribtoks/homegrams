import math

from pyFEM.geometry import Point
from pyFEM import symbols as sym


class Edge(object):
    def __init__(self, *args):
        if len(args) == 0:
            self.origin = Point(0, 0)
            self.destination = Point(0, 0)
        elif len(args) == 1:
            e = args[0]
            self.origin = Point(e.origin.x, e.origin.y)
            self.destination = Point(e.destination.x, e.destination.y)
        elif len(args) == 2:
            self.origin, self.destination = [Point(p.x, p.y) for p in args]
        else:
            raise ValueError("Unexpected number of arguments")

    def __eq__(self, other):
        return (self.origin == other.origin) and (self.destination == other.destination)

    def length(self):
        return (destination - origin).length()

    def rotate(self):
        """Rotate self 90 degrees CW around the center"""
        m = (self.origin + self.destination) * 0.5
        v = self.destination - self.origin
        n = Point(v.y, -v.x)
        self.origin = m - n * 0.5
        self.destination = m + n * 0.5

    def flip(self):
        self.rotate()
        self.rotate()

    def intersect(self, other):
        raise NotImplementedError("Edge.intersect stub")

    @staticmethod
    def angle(edge1, edge2):
        raise NotImplementedError("Edge.angle stub")

    @staticmethod
    def angle_points(point1, point2, point3):
        """Return the absolute value of the angle in radians
        between [point1, point2] and [point2, point3]"""
        # use law of cosines
        a = (point2 - point1).length()
        b = (point3 - point2).length()
        c = (point3 - point1).length()

        value = ((a ** 2) + (b ** 2) - (c ** 2)) / (2.0 * a * b)
        if abs(value) > 1.0:
            value = 1.0 * (value / abs(value))
        return abs(math.acos(value))

    @staticmethod
    def angle_points_orient(point_next, point, point_prev):
        angle = Edge.angle_points(point_prev, point, point_next)
        if point_next.classify(point_prev, point) == sym.left:
            angle = (math.pi * 2.0) - angle
        return angle
    
    @staticmethod
    def split(point1, point2, diameter):
        # TODO: implement `split' in terms of point count instead of diameter
        l = (point2 - point1).length()
        n = int(math.ceil(l / diameter))
        step_v = (point2 - point1) * (1.0 / n)
        return [point1 + (step_v * i) for i in range(n)]

    @staticmethod
    def subdivide(point1, point2, point_count):
        step_v = (point2 - point1) * (1.0 / (point_count + 1))
        return [point1 + (step_v * i) for i in range(1, point_count + 1)]

    @staticmethod
    def subdivide_with_bounds(point1, point2, point_count):
        res = [point1]
        res.extend(Edge.subdivide(point1, point2, point_count))
        res.append(point2)
        return res
        
  
