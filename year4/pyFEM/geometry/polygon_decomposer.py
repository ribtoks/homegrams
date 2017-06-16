from pyFEM.geometry import Point, Edge, Polygon
from pyFEM import symbols as sym

def is_vertex_convex(p):
    a = p.neighbor(sym.counterclockwise)
    b = p.curr_point
    c = p.neighbor(sym.clockwise)
    return (c.classify(a, b) is sym.right)

def point_in_triangle(p, a, b, c):
    tri = [a, b, c]
    res = all([p.classify(tri[i - 1], tri[i]) != sym.left for i in range(len(tri))])
    return res

def find_intruding_vertex(polygon):
    a = polygon.neighbor(sym.counterclockwise)
    b = polygon.curr_point
    c = polygon.advance(sym.clockwise)

    d = None
    bestD = -1.0
    ca = Edge(c, a)
    v = polygon.move_next()

    while v != a:
        if point_in_triangle(v, a, b, c):
            dist = v.distance(ca.origin, ca.destination)
            if dist > bestD:
                bestD = dist
                d = Point.copy(v)
        v = polygon.move_next()
    polygon.curr_point = b
    return d

def get_distances(polygon):
    distances = []
    for i in range(len(polygon)):
        polygon.curr_index = i
        if not is_vertex_convex(polygon): continue
        d = find_intruding_vertex(polygon)
        if d is not None:
            distances.append([(d - polygon.curr_point).length(), polygon.curr_point, d])
    return sorted(distances, cmp=lambda a, b: cmp(a[0], b[0]))


class PolygonDecomposer(object):
    def __init__(self):
        self.polygons = []

    def decompose(self, polygon):
        self.polygons = []
        self.inner_decompose(polygon)
        return self.polygons

    def inner_decompose(self, polygon):
        distances = get_distances(polygon)
        if not distances:
            self.polygons.append(Polygon.copy(polygon.points))
            return

        element = distances[0]
        point1 = element[1]
        point2 = element[2]

        new_polygon = polygon.split_points(point1, point2)

        self.inner_decompose(new_polygon)
        self.inner_decompose(polygon)
