#include <iostream>
#include <conio.h>
#include <cmath>
using namespace std;

int main()
{
	double a = 0., b = .0, c = 0.0;
	cout << "Please, enter 3 coefficients:" << endl;
	cin >> a >> b >> c;
	double D = b*b - ((a*c) * 4.0);
	if (D < 0)
	{
		cout << "We have no equation solutions!" << endl;
	}
	else
	{
		double epsilon = 0.00000000001;
		if (fabs(D) < epsilon)
		{
			cout << "x == " << -b/(2.0*a) << endl;
		}
		else
		{
			double d = sqrt(D);
			cout << "x1 == " << (-b + d)/(2.0*a) << endl;
			cout << "x2 == " << (-b - d)/(2.0*a) << endl;
		}
	}
	_getch();
	return 0;
}