from pyFEM.geometry import Point, Edge


class ConditionsItem(object):
    def __init__(self, alpha, lambda_, u_environment):
        self.alpha = alpha
        self.lambda_ = lambda_
        self.u_environment = u_environment

    @staticmethod
    def copy(cond):
        return ConditionsItem(cond.alpha, cond.lambda_, cond.u_environment)

class BoundaryConditions(object):
    def __init__(self, conditions_list, diameter, subdivide_n=0):
        self.conditions = []
        self.points_to_condindex = {}
        self.diameter = diameter

        for key, value in conditions_list:
            p_start, p_end = key
            points = Edge.split(p_start, p_end, diameter)
            if points[-1] != p_end:
                points.append(Point.copy(p_end))

            self.conditions.append(ConditionsItem.copy(value))

            for i in range(len(points) - 1):
                p1_hash = hash(points[i])
                p2_hash = hash(points[i + 1])
                arr = tuple(sorted([p1_hash, p2_hash]))
                self.points_to_condindex[arr] = len(self.conditions) - 1

    def __getitem__(self, index):
        return self.conditions[index]

class CompleteTaskConditions(object):
    def __init__(self, a, b, d, function, conditions):
        self.a = a
        self.b = b
        self.d = d
        self.f = function

        self.boundary_conditions = conditions
