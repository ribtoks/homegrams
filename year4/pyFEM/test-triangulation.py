#!/usr/bin/env python2

import matplotlib.pyplot as plt
from matplotlib import collections as mcoll

from pyFEM.geometry import Point, Polygon, Triangulator


if __name__ == '__main__':
    poly = Polygon.copy([Point(0, 0), Point(0, 4), Point(4, 4), Point(4, 0)])
    diam = 1.0

    triangulator = Triangulator()
    triangulator.triangulate(poly, diam)
    triangulator.smooth()
    triangulator.subdivide_triangles(1)
    triangulator.calculate_hashes()
    nums = triangulator.enumerate_triangles()
    
    triangles = []
    all_points_x = []
    all_points_y = []
    text = []
    for i, tri in enumerate(triangulator.triangles):
        triangles.append(tuple([(p.x, p.y) for p in tri.points]))
        all_points_x.extend([p.x for p in tri.all_points])
        all_points_y.extend([p.y for p in tri.all_points])
        text.extend([str(nums[p]) for p in tri.all_points])
        print("triangle {0}: {1}".format(i, [nums[p] for p in tri.all_points]))
    # print("bound points: {0}".format([nums[pt]
    #                                   for pt, bound
    #                                   in triangulator.bound_points.items()
    #                                   if bound]))
    print("bound_points: {0}".format(triangulator.bound_points.keys()))
        
    poly_coll = mcoll.PolyCollection(triangles, facecolors=("lime",))
    fig = plt.figure()
    p = fig.add_subplot(1, 1, 1)
    p.add_collection(poly_coll)
    p.plot(all_points_x, all_points_y, 'o', color='silver')
    for i, t in enumerate(text):
        p.text(all_points_x[i], all_points_y[i], t)
    plt.show()
