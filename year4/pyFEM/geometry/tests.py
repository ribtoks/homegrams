#!/usr/bin/env python2

import unittest
import math

from pyFEM.geometry import Point, Edge
from pyFEM.config import eps

class TestPointSanity(unittest.TestCase):
    def test_arithmetic(self):
        p1 = Point(1, 2)
        p2 = Point(-5, 6)
        self.assertEqual(p1 + p2, Point(1 - 5, 2 + 6))
        self.assertEqual(p1 - p2, Point(1 + 5, 2 - 6))
        self.assertEqual(p1 * 5.6, Point(1 * 5.6, 2 * 5.6))

    def test_advanced(self):
        x1 = 1
        y1 = 2
        p1 = Point(x1, y1)
        p2 = Point(x1 + 4, y1 + 4)
        p3 = Point(x1 + 4, y1)
        self.assertAlmostEqual(p3.distance(p1, p2), math.sqrt((2 ** 2) + (2 ** 2)), delta=eps)
        self.assertEqual(Point.orientation(p1, p2, p3), -1)
        self.assertEqual(Point.orientation(p1, p2, p1 + Point(1, 1)), 0)

class TestEdgeSanity(unittest.TestCase):
    def test_angles(self):
        p2 = Point(1, 2)
        p1 = p2 + Point(4, 4)
        p3 = p2 + Point(-3, 3)
        self.assertAlmostEqual(Edge.angle_points(p1, p2, p3), math.pi / 2, delta=eps)

    def test_split(self):
        n = 5
        p1 = Point(1, 2)
        p2 = p1 + Point(n, n)
        s = Edge.split(p1, p2, 1.0)
        self.assertEqual(s, [p1 + (Point(1, 1) * i) for i in range(n)])
        
if __name__ == '__main__':
    unittest.main()
