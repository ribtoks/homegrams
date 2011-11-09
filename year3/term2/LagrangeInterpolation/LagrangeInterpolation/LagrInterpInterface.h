#pragma once
#include <vector>
using namespace std;

typedef double (*UsualFunction)(double);


double InterpolateByLagrange(UsualFunction func, const vector<double>& points, double x);

vector<double> GetChebyshPoints(int count);

vector<double> GetEquidistantPoints(int count, double a, double b);

double RungeFunction(double x);

vector<double> ApplyFunction(UsualFunction func, const vector<double>& points);