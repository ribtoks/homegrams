#include <iostream>
#include <iomanip>
#include <string>
#include <conio.h>
#include "classTime.h"
using namespace std;

template<class T> void QuickSort(T* arr, int first, int last)
{
	int left = first;
	int right = last;
	T v = arr[(left + right) >> 1];
	while (left <= right)
	{
		while (arr[left] < v) ++left;
		while (v < arr[right]) --right;
		if (left <= right)
		{
			T temp = arr[left];
			arr[left] = arr[right];
			arr[right] = temp;
			++left; -- right;
		}
	}
	if (left < last)
		QuickSort(arr, left, last);
	if (right > first)
		QuickSort(arr, first, right);
}

template<class Q> void CoutArray(Q *it, int start, int count)
{
	for (int i = start; i < count; i++)
		cout << it[i] << " ";
	cout << '\b' << endl;
}

int main()
{
	int n, i = 0;
	Time* arr = 0;
	try
	{
		cout << "Enter number of elements in array :> ";
		cin >> n;
		if (n == 0)
			throw "No elements!";
		arr = new Time[n];
		cout << "Enter " << n << " elements of Time:" << endl;
		for (i = 0; i < n; ++i)
			cin >> arr[i];
		cout << endl;		
		QuickSort(arr, 0, n - 1);
		cout << "Sorted:" << endl;
		CoutArray(arr, 0, n);
		cout << "Sum of maximum and minimum elements is " << arr[0] + arr[n - 1] << endl << endl;
		cout << "Enter a time please :> ";
		Time t;
		cin	>> t;
		cout << "Equal to this are: ";
		for (i = 0; i < n; ++i)
			if (arr[i] == t)
				cout << i << " ";
		cout << endl;
	}
	catch (const char* str)
	{
		cout << "Error occured!" << endl << str << endl;
	}
	delete[] arr;
	cout << "The end." << endl;
	_getch();
	return 0;
}