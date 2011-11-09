#include <iostream>
#include <limits>
#include <conio.h>
using namespace std;

int main()
{
	int n = 0;
	cout << "Please, enter dimension (odd) of matrix :> ";
	cin >> n;
	
	int i = 0, j = 0;
	int **matr = new int* [n];
	for (i = 0; i < n; ++i)
	{
		matr[i] = new int[n];
		memset(matr[i], 0, n*sizeof(int));
	}

	cout << "Please, enter all elements of matrix:" << endl;
	for (i = 0; i < n; ++i)
		for (j = 0; j < n; ++j)
			cin >> matr[i][j];

	int maximumEl = numeric_limits<int>::min();
	int iMax = -1;
	for (i = 0; i < n; ++i)
	{
		if (matr[i][i] > maximumEl)
		{
			maximumEl = matr[i][i];
			iMax = i;
		}
	}
	
	bool wasSmth = false;
	for (i = 0; i < n; ++i)
	{
		if (matr[i][n - i - 1] > maximumEl)
		{
			maximumEl = matr[i][n - i - 1];
			iMax = i;
			wasSmth = true;
		}
	}
	
	cout << "Maximum diagonal element is " << maximumEl << " at position: [" << iMax << ", " << (wasSmth?(n - iMax - 1):(iMax)) << "]" << endl << endl;
	
	int index2 = wasSmth?(n - iMax - 1):(iMax);

	int temp = matr[iMax][index2];
	matr[iMax][index2] = matr[n >> 1][n >> 1];
	matr[n >> 1][n >> 1] = temp;

	for (i = 0; i < n; ++i)
	{
		for (j = 0; j < n; ++j)
		{
			cout << matr[i][j] << " ";
		}
		cout << endl;
	}

	_getch();
	for (i = 0; i < n; ++i)
		delete[] matr[i];
	delete[] matr;
	return 0;
}