#include "stdafx.h"
#include "LagrInterpInterface.h"
#include <vector>
#include <cmath>
using namespace std;

#define pi 3.1415926535897932384626433832795

double LagrangeBasisCalc(const vector<double>& points, double point, double pointIndex)
{
	double mulResult = 1;

	for (int i = 0; i < points.size(); ++i)
		if (i != pointIndex)
			mulResult *= ((point - points[i]) / (points[pointIndex] - points[i]));

	return mulResult;
}

double InterpolateByLagrange(UsualFunction func, const vector<double>& points, double x)
{
	double sum = 0;

	for (int i = 0; i < points.size(); ++i)
		sum += func(points[i]) * LagrangeBasisCalc(points, x, i);

	return sum;
}

vector<double> GetEquidistantPoints(int count, double a, double b)
{
	if (a >= b)
		throw "Left bound is not less than right bound!";

	//--count;
	vector<double> points;

	double h = (b - a) / count;
	for (int i = 0; i <= count; ++i)
		points.push_back(a + h*i);

	return points;
}

vector<double> GetChebyshPoints(int count)
{
	vector<double> points;

	for (int i = 1; i <= count; ++i)
		points.push_back( cos((2.0*i - 1.0)*pi / (2*count)) );

	return points;
}

double RungeFunction(double x)
{
	return 1 / (1.0 + 25.0*x*x);
}

vector<double> ApplyFunction(UsualFunction func, const vector<double>& points)
{
	vector<double> v;
	for (int i = 0; i < points.size(); ++i)
		v.push_back(func(points[i]));

	return v;
}