import numpy as np
import operator
import math
from sympy import abc
from itertools import izip
from operator import mul

from pyFEM.geometry import Point, Edge, Polygon, Triangulator


def solve_phi_integrals(expr, bounds=False):
    p = expr.as_poly()
    syms = p.symbols
    res = 0

    base_syms = [abc.i, abc.j, abc.m]

    for coeff, monome in izip(p.coeffs, p.monoms):
        pow_hash = { abc.i: 0, abc.j: 0, abc.m: 0 }
        for sym, pow_ in izip(syms, monome):
            pow_hash[sym] = pow_
        a_b_c = [pow_hash[sym] for sym in base_syms]
        if bounds:
            sum_fact = math.factorial(sum(a_b_c) + 1)
        else:
            sum_fact = math.factorial(sum(a_b_c) + 2)
        res += coeff * reduce(mul, [math.factorial(x) for x in a_b_c]) / float(sum_fact)
        
    return res

def generate_matrix(triangulator, conditions, subdivide_n=0):
    triangulator.correct_all_triangles()
    triangles = triangulator.triangles
    points_numbers = triangulator.vertices_hash
    bounds_hash = triangulator.bound_points
    n = len(triangulator.points_to_points)

    m_matrix = np.matrix(np.zeros((n, n), dtype=np.float64))
    f_vector = np.zeros(n, dtype=np.float64)

    sz = 3 * (subdivide_n + 1)

    for tr in triangles:
        matrices, vector = get_matrices_and_vector(tr, bounds_hash, conditions, subdivide_n)
        p_numbers = [points_numbers[p] for p in tr.all_points]
        for i in range(sz):
            f_vector[p_numbers[i]] += vector[i][0]
        for m in matrices:
            for i in range(sz):
                for j in range(sz):
                    m_matrix[p_numbers[i],p_numbers[j]] += m[i,j]
    return m_matrix, f_vector

def get_matrices_and_vector(triangle, bounds_hash, conditions, subdivide_n=0):
    bconditions = conditions.boundary_conditions
    a = conditions.a
    b = conditions.b
    d = conditions.d
    func = conditions.f
    sz = 6
    
    matrices = []
    
    if a != 0:
        matrices.append(get_k(triangle, a))
    # if a != b:
    #     matrices.append(get_k_simplified(triangle, b - a))
    if d != 0:
        matrices.append(get_d_matrix(triangle, d))
        
    fi = map(func, triangle.all_points)
    fi_vector = np.matrix(fi)
    md = get_d_matrix(triangle, 1)
    f_vector = md * fi_vector.transpose()
    
    has_no_bound_point = True
    
    tr_bound_points = []
    tr_points = triangle.points
    
    tr_segments = []
    tr_segments.append([tr_points[0], tr_points[1], 2])
    tr_segments.append([tr_points[1], tr_points[2], 0])
    tr_segments.append([tr_points[2], tr_points[0], 1])

    bound_segments = []

    for i in range(len(tr_segments)):
        segment = tr_segments[i]
        arr = tuple(sorted([hash(segment[0]), hash(segment[1])]))
        if arr in bconditions.points_to_condindex:
            # subsegments = Edge.subdivide_with_bounds(segment[0], segment[1], subdivide_n)
            # for i in range(len(subsegments) - 1):
            #     p_cur = subsegments[i]
            #     p_next = subsegments[i + 1]
            #     bound_segments.append([p_cur, p_next, segment[2]])
            bound_segments.append(segment)
            has_no_bound_point = False

    if has_no_bound_point:
        return matrices, f_vector

    condindex_hash = bconditions.points_to_condindex

    for segment in bound_segments:
        arr = tuple([hash(segment[i]) for i in range(2)])
        unbound_point_index = segment[2]
        conditions_index = condindex_hash[tuple(sorted(arr))]
        ci = bconditions[conditions_index]
        coef1 = -ci.alpha / ci.lambda_
        coef2 = ci.u_environment * ci.alpha / ci.lambda_

        matrices.append(get_bounds_k(triangle, unbound_point_index, coef1))
        
        f_temp = get_bounds_k_simplified(triangle, unbound_point_index, coef2)
        for i in range(sz):
            f_vector[i][0] -= f_temp[i]

    return matrices, f_vector

def get_d_matrix(triangle, coef):
    psi_i = abc.i * (2 * abc.i - 1)
    psi_j = abc.j * (2 * abc.j - 1)
    psi_m = abc.m * (2 * abc.m - 1)

    psi_k = 4 * abc.i * abc.j
    psi_l = 4 * abc.j * abc.m
    psi_n = 4 * abc.i * abc.m

    arr = [psi_i, psi_k, psi_j, psi_l, psi_m, psi_n]

    sz = 6
    d_matrix = np.zeros((sz, sz), dtype=np.float64)
    
    delta = 2.0 * triangle_square(triangle.points)

    for i in range(sz):
        for j in range(i, sz):
            polynome = arr[i] * arr[j]
            d_matrix[i, j] = d_matrix[j, i] = delta * coef * solve_phi_integrals(polynome)
    
    return d_matrix

def get_bounds_k(triangle, unbound_point_index, coef):
    points = triangle.points
    sz = 6

    line_points = [Point.copy(points[(unbound_point_index + i + 1) % 3])
                   for i in range(2)]
    if unbound_point_index == 0:
        line_indices = [2, 3, 4]
    elif unbound_point_index == 1:
        line_indices = [0, 4, 5]
    elif unbound_point_index == 2:
        line_indices = [0, 1, 2]
    line_length = (line_points[1] - line_points[0]).length()

    # XXX: copy-paste
    psi_i = abc.i * (2 * abc.i - 1)
    psi_j = abc.j * (2 * abc.j - 1)
    psi_m = abc.m * (2 * abc.m - 1)

    psi_k = 4 * abc.i * abc.j
    psi_l = 4 * abc.j * abc.m
    psi_n = 4 * abc.i * abc.m

    arr = [psi_i, psi_k, psi_j, psi_l, psi_m, psi_n]

    k_matrix = np.zeros((sz, sz), dtype=np.float64)
    for i in line_indices:
        for j in line_indices:
            if j < i: continue
            polynome = arr[i] * arr[j]
            sum_ = solve_phi_integrals(polynome, True)
            k_matrix[i, j] = k_matrix[j, i] = coef * line_length * sum_
    
    return k_matrix
    
    
    
def get_bounds_k_simplified(triangle, unbound_point_index, coef):
    # XXX: copy-paste
    points = triangle.points
    sz = 6

    line_points = [Point.copy(points[(unbound_point_index + i + 1) % 3])
                   for i in range(2)]
    if unbound_point_index == 0:
        line_indices = [2, 3, 4]
    elif unbound_point_index == 1:
        line_indices = [0, 4, 5]
    elif unbound_point_index == 2:
        line_indices = [0, 1, 2]
    line_length = (line_points[1] - line_points[0]).length()
    
    psi_i = abc.i * (2 * abc.i - 1)
    psi_j = abc.j * (2 * abc.j - 1)
    psi_m = abc.m * (2 * abc.m - 1)

    psi_k = 4 * abc.i * abc.j
    psi_l = 4 * abc.j * abc.m
    psi_n = 4 * abc.i * abc.m

    arr = [psi_i, psi_k, psi_j, psi_l, psi_m, psi_n]

    ks_vector = np.zeros(sz, dtype=np.float64)
    
    for i in line_indices:
        sum_ = solve_phi_integrals(arr[i], True)
        ks_vector[i] = line_length * coef * sum_

    return ks_vector
        
    
def get_k(triangle, coef):
    delta = 2.0 * triangle_square(triangle.points)
    if delta == 0:
        raise ValueError("something went wrong")   # what
    ai = [get_a(triangle.points, i) for i in range(3)]
    bi = [get_b(triangle.points, i) for i in range(3)]
    ci = [get_c(triangle.points, i) for i in range(3)]

    a = -(bi[1] * bi[2] + ci[1] * ci[2])
    b = -(bi[0] * bi[2] + ci[0] * ci[2])
    c = -(bi[0] * bi[1] + ci[0] * ci[1])
    d = a + b + c

    k_matrix = np.matrix([[3*(b+c), c, b, -4*c, 0, -4*b],
                          [0, 3*(c+a), a, -4*c, -4*a, 0],
                          [0, 0, 3*(a+b), 0, -4*a, -4*b],
                          [0, 0, 0, 8*d, -8*b, -8*a],
                          [0, 0, 0, 0, 8*d, -8*c],
                          [0, 0, 0, 0, 0, 8*d]], dtype=np.float64)

    sz = 6
    for i in range(sz):
        for j in range(i + 1, sz):
            k_matrix[j, i] = k_matrix[i, j]

    for i in range(sz):
        for j in range(sz):
            k_matrix[i, j] *= coef / (delta * 6.0)
    
    return k_matrix

def get_k_simplified(triangle, coef):
    raise RuntimeError("Don't use")
    ks_matrix = np.matrix(np.zeros((3, 3), dtype=np.float64))
    delta = 2.0 * triangle_square(triangle.points)
                          
    ai = [get_a(triangle.points, i) for i in range(3)]
    bi = [get_b(triangle.points, i) for i in range(3)]
    ci = [get_c(triangle.points, i) for i in range(3)]

    for i in range(3):
        val = ci[i] ** 2
        ks_matrix[i,i] = val * coef / (2.0 * delta)

    for i in range(3):
        for j in range(i + 1, 3):
            val = ci[i] * ci[j]
            ks_matrix[i,j] = ks_matrix[j,i] = val * coef / (2.0 * delta)

    return ks_matrix

def triangle_square(points):
    sides = [(points[i - 1] - points[i]).length() for i in range(3)]
    p = sum(sides) / 2.0
    return math.sqrt(p * reduce(operator.mul, [p - s for s in sides], 1))

def get_a(points, i):
    i1 = (i + 1) % len(points)
    i2 = (i + 2) % len(points)
    return points[i1].x * points[i2].y - points[i1].y * points[i2].x

def get_b(points, i):
    i1 = (i + 1) % len(points)
    i2 = (i + 2) % len(points)
    return points[i1].y - points[i2].y

def get_c(points, i):
    i1 = (i + 1) % len(points)
    i2 = (i + 2) % len(points)
    return points[i2].x - points[i1].x
