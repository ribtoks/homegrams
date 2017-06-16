#!/usr/bin/env python2

import numpy as np
import scipy.linalg
from mpl_toolkits.mplot3d import art3d, axes3d
import matplotlib.pyplot as plt

from pyFEM.geometry import Point, Polygon, Triangulator
from pyFEM.conditions import ConditionsItem, BoundaryConditions, CompleteTaskConditions
from pyFEM.matrices import generate_matrix

from pyFEM.geometry import PolygonDecomposer

if __name__ == '__main__':
    subdivide_n = 1

    p1 = Point(0, 0)
    p2 = Point(0, 3)
    p3 = Point(6, 3)
    p4 = Point(6, 0)
    polygon = Polygon.copy([p1, p2, p3, p4])
    bounds_list = [([p1, p2], ConditionsItem(0, 1, 0)),
                   ([p2, p3], ConditionsItem(10000, 1, 0)),
                   ([p3, p4], ConditionsItem(0, 1, 0)),
                   ([p4, p1], ConditionsItem(10000, 1, 100))]

    p1 = Point(0, 0)
    p2 = Point(0, 5)
    p3 = Point(5, 0)
    polygon = Polygon.copy([p1, p2, p3])
    bounds_list = [([p1, p2], ConditionsItem(1000, 1, 100)),
                   ([p2, p3], ConditionsItem(1000, 1, 100)),
                   ([p3, p1], ConditionsItem(1, 1, 0))]
    

    x_limits = (min([p.x for p in polygon]), max(p.x for p in polygon))
    y_limits = (min([p.y for p in polygon]), max(p.y for p in polygon))
    diameter = 0.6
    a = -8
    b = a
    d = 0
    f = lambda x: 1.0
    
    boundary_conditions = BoundaryConditions(bounds_list, diameter, subdivide_n)
    conditions = CompleteTaskConditions(a, b, d, f, boundary_conditions)

    triangulator = Triangulator()
    triangulator.triangulate(polygon, diameter)
    triangulator.smooth()
    triangulator.subdivide_triangles(subdivide_n)
    triangulator.calculate_hashes()
    nums = triangulator.enumerate_triangles()

    m_matrix, f_vector = generate_matrix(triangulator, conditions, subdivide_n)

    print(m_matrix, f_vector)
                        
    u = scipy.linalg.solve(m_matrix, f_vector)
    print([min(u), max(u)])


    plot_triangles = []
    z_colors = []
    if subdivide_n == 0:
        for triangle in triangulator.triangles:
            cur_tri = []
            for p in triangle.all_points:
                idx = nums[p]
                u_v = u[idx]
                cur_tri.append([p.x, p.y, u_v])
            plot_triangles.append(np.array(cur_tri, dtype=np.float64))
            z_colors.append(sum([p[2] for p in cur_tri]) / 3.0)
    elif subdivide_n == 1:
        print(triangulator.triangles[0].all_points)
        for triangle in triangulator.triangles:
            for i in range(4):
                if i == 3:
                    cur_tri = [triangle.all_points[j] for j in [0, 2, 4]]
                else:
                    cur_tri = [triangle.all_points[((2 * i) + j) % 6] for j in range(3)]
                new_tri = np.array([[p.x, p.y, 0.0] for p in cur_tri], dtype=np.float64)
                for i, p in enumerate(cur_tri):
                    idx = nums[p]
                    u_v = u[idx]
                    new_tri[i][2] = u_v
                plot_triangles.append(new_tri)
                z_colors.append(sum([p[2] for p in new_tri]) / 3.0)


    tri_col = art3d.Poly3DCollection(plot_triangles)
    tri_col.set_array(np.array(z_colors, dtype=np.float64))
    fig = plt.figure()
    ax = axes3d.Axes3D(fig)
    ax.add_collection(tri_col)
    ax.set_xlim3d(*x_limits)
    ax.set_ylim3d(*y_limits)
    ax.set_zlim3d(np.min(u), np.max(u))
    plt.show()
