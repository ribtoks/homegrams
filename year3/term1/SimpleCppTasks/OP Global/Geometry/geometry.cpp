#include <iostream>
#include <conio.h>
#include <cmath>
using namespace std;

double Distance(double x1, double y1, double x2, double y2)
{
	return sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));
}

int main()
{
	double Ax, Ay, Bx, By, Cx, Cy;
	cout << "Point A: ";
	cin >> Ax >> Ay;
	cout << "Point B: ";
	cin >> Bx >> By;
	cout << "Point C: ";
	cin >> Cx >> Cy;

	double dAB = Distance(Ax, Ay, Bx, By);
	double dAC = Distance(Ax, Ay, Cx, Cy);

	if (dAB < dAC)
		cout << "Point B is nearer to A." << endl;
	else
		if (dAB > dAC)
			cout << "Point C is nearer to A." << endl;
		else
			cout << "They have equal distance." << endl;
	_getch();
	return 0;
}