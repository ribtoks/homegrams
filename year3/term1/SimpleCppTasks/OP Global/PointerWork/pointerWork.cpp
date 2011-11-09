#include <iostream>
#include <conio.h>
using namespace std;

int main()
{
	int n;
	cout << "Please, enter array dimension :> ";
	cin >> n;
	int *arr = new int[n];
	cout << "Please, enter " << n << " elements of array:" << endl;
	for (int i = 0; i < n; ++i)
	{
		cin >> arr[i];
	}
	
	int m = 0;
	cout << "Please, enter misterious number M :> ";	
	cin >> m;

	for (int i = 0; i < n; ++i)
		for (int j = 0; j < n; ++j)
		{
			if (arr[i] + arr[j] == m)
			{
				cout << "Numbers of that elements: " << i << " and " << j;
				delete[] arr;
				_getch();
				return 0;
			}
		}
	cout << "There are no such elements!";
	delete[] arr;
	_getch();
	return 0;
}