"""Point class"""

from numbers import Number
import math

from pyFEM.config import eps
from pyFEM import symbols


class Point(object):
    def __init__(self, x, y):
        self.x = float(x)
        self.y = float(y)

    @staticmethod
    def copy(p):
        return Point(p.x, p.y)

    def __add__(self, other):
        return Point(self.x + other.x, self.y + other.y)

    def __sub__(self, other):
        return Point(self.x - other.x, self.y - other.y)

    def __mul__(self, other):
        if isinstance(other, Point):
            raise RuntimeError("Please use Point.dot() for dot product")
        elif isinstance(other, Number):
            return Point(self.x * other, self.y * other)
        else:
            raise TypeError("Don't know how to multiply point with {0}".format(type(other)))
            

    def length(self):
        return math.sqrt((self.x ** 2) + (self.y ** 2))

    def __getitem__(self, i):
        if i == 0:
            return self.x
        elif i == 1:
            return self.y
        else:
            raise IndexError("Only 0 and 1 are supported for 2D Point")

    def __setitem__(self, i, value):
        if i == 0:
            self.x = value
        elif i == 1:
            self.y = value
        else:
            raise IndexError("Only 0 and 1 are supported for 2D Point")

    def __eq__(self, other):
        return (abs(self.x - other.x) < eps) and (abs(self.y - other.y) < eps)
        
    def __lt__(self, other):
        return (self.x + eps < other.x) or \
            ((abs(self.x - other.x) < eps) and (self.y + eps < other.y))

    def __gt__(self, other):
        return (self.x > other.x + eps) or \
            ((abs(self.x - other.x) < eps) and (self.y > other.y + eps))

    def __cmp__(self, other):
        if self == other:
            return 0
        elif self > other:
            return 1
        else:
            return -1

    def __hash__(self):
        a = 100000000
        return hash(math.ceil(self.x * a) * 13.0 + math.ceil(self.y * a) * 57.0)

    def classify(self, p0, p1):
        """Classify the position of self w/r/t the edge [p0, p1]"""
        p2 = self
        a = p1 - p0
        b = p2 - p0
        sa = (a.x * b.y) - (b.x * a.y)
        if sa > 0:
            return symbols.left
        elif sa < 0:
            return symbols.right
        elif (a.x * b.x < 0) or (a.y * b.y < 0):
            return symbols.behind
        elif a.length() < b.length():
            return symbols.beyond
        elif p0 == p2:
            return symbols.origin
        elif p2 == p1:
            return symbols.destination
        else:
            return symbols.between

    def dot(self, other):
        return self.x * other.x + self.y * other.y
        
    def distance(self, p0, p1):
        """Distance from self to the edge [p0, p1]"""
        edge_l = (p1 - p0).length()
        det = (p0.y - p1.y) * self.x + (p1.x - p0.x) * self.y + ((p0.x * p1.y) - (p1.x * p0.y))
        return abs(det / edge_l)

    @staticmethod
    def orientation(p0, p1, p2):
        """Orientation between vectors [p0, p1] and [p0, p2] via cross-product"""
        a = p1 - p0
        b = p2 - p0
        sa = (a.x * b.y) - (b.x * a.y)
        if sa > 0:
            # CCW, positive
            return 1
        elif sa < 0:
            # CW, negative
            return -1
        else:
            # colinear
            return 0

    def __repr__(self):
        return "pt({0}, {1})".format(self.x, self.y)
