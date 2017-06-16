#include <iostream>
#include <conio.h>
using namespace std;

int main()
{
	int n;
	cout << "Enter number of elements: ";
	cin >> n;
	int i = 0;
	int temp;
	int mean_temp = 0;
	cout << "Please, enter " << n << " elements:" << endl;
	while (i < n)
	{
		cin >> temp;
		mean_temp += temp;
		++i;
	}
	int real_mean = mean_temp / n;
	double double_mean = mean_temp;
	double_mean /= (n + 0.0);
	cout << "(int) Mean equals " << real_mean << endl;
	cout << "(double) Mean equals " << double_mean << endl;
	_getch();
	return 0;
}