#include <iostream>
#include <conio.h>
using namespace std;

int main()
{
	int n = 0, m = 0;
	cout << "Please, enter height of matrix :> ";
	cin >> n;
	cout << "Please, enter width of matrix :> ";
	cin >> m;

	int i = 0, j = 0;
	int **matr = new int* [n];
	for (i = 0; i < n; ++i)
	{
		matr[i] = new int[m];
		memset(matr[i], 0, m*sizeof(int));
	}
	
	int dirX[] = {1, 0, -1, 0};
	int dirY[] = {0, 1, 0, -1};
	int currPos = 0;

	i = 0, j = 0;
	int way = 0;
	while (way < n*m)
	{
		while (matr[i][j] == 0)
		{			
			if (i < n  &&  i >= 0  &&  j < m  &&  j >= 0)
			{
				++way;
				matr[i][j] = way;
				i += dirY[currPos];
				j += dirX[currPos];				
			}
			else
				break;
			if (i >= n  ||  i < 0  ||  j >= m  ||  j < 0)
				break;
		}

		if (i >= n  ||  i < 0  ||  j >= m  ||  j < 0  ||  matr[i][j] != 0)
		{
			i -= dirY[currPos];
			j -= dirX[currPos];
		}

		currPos += 1;
		currPos %= 4;

		i += dirY[currPos];
		j += dirX[currPos];
	}
	for (i = 0; i < n; ++i)
	{
		for (j = 0; j < m; ++j)
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