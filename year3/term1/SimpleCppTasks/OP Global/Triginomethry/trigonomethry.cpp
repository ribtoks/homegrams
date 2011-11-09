#include <iostream>
#include <limits>
#include <conio.h>
#include <cmath>
using namespace std;


void WriteSomething(double& sin, double& cos, double& log, double& What)
{

	if (What == sin)
	{
		cout << "sin(x) == " << sin << endl;
		sin = -numeric_limits<double>::max();
	}
	else
		if (What == cos)
		{
			cout << "cos(x) == " << cos << endl;
			cos = -numeric_limits<double>::max();
		}
		else
		{
			cout << "Log(x) == " << log << endl;
			log = -numeric_limits<double>::max();;
		}
}

int main()
{
	double x;
	cout << "Please, enter value (double): ";
	cin >> x;
	cout << "Results:" << endl;

	double sinX;
	double cosX;
	double logX;

	if (false)
	{
		cout << "WARNING! Sin() and Cos() can not take current argument!" << endl;
		if (x <= 0)
		{
			cout << "WARNING! Loh() must take positive argument!" << endl;
			cout << "Nothing can be calculated!" << endl;
			_getch();
			return 0;
		}
		else
		{
			cout << "Log() == " << log(x) << endl;
			_getch();
			return 0;
		}
	}
	else
	{
		sinX = sin(x);
		cosX = cos(x);
	}

	if (x <= 0)
	{
		cout << "WARNING! Loh() must take positive argument!" << endl;
		if (sinX > cosX)		
			cout << "sin(x) == " << sinX << endl << "cos(x) == " << cosX << endl;
		else
			cout << "cos(x) == " << cosX << endl << "sin(x) == " << sinX << endl;
		_getch();
		return 0;
	}
	else
		logX = log(x);
	
	double tempMax = ((sinX > cosX && sinX > logX) ? (sinX) : ((cosX > logX) ? cosX : logX));	
	WriteSomething(sinX, cosX, logX, tempMax);

	tempMax = ((sinX > cosX && sinX > logX) ? (sinX) : ((cosX > logX) ? cosX : logX));	
	WriteSomething(sinX, cosX, logX, tempMax);

	tempMax = ((sinX > cosX && sinX > logX) ? (sinX) : ((cosX > logX) ? cosX : logX));	
	WriteSomething(sinX, cosX, logX, tempMax);

	_getch();
	return 0;
}