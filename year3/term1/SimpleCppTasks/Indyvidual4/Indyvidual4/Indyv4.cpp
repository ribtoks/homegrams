#include <iostream>
#include <conio.h>
#include "TransportBaseClass.h"
#include "WaterTransport.h"
#include "LandTransport.h"
using namespace std;

template<class T>
void QuickSort(T* items, int first, int last)
{
	int left = first;
	int right = last;
	T v = items[(right + left) >> 1];
	while (left <= right)
	{
		while (items[left] < v) ++left;
		while (v < items[right]) --right;
		if (left <= right)
		{
			T temp = items[left];
			items[left] = items[right];
			items[right] = temp;
			--right;
			++left;
		}
	}
	if (left < last)
		QuickSort(items, left, last);
	if (first < right)
		QuickSort(items, first, right);
}

void QuickSort(TransportBase** items, int first, int last)
{
	int left = first;
	int right = last;
	TransportBase v = *items[(right + left) >> 1];
	while (left <= right)
	{
		while (*items[left] < v) ++left;
		while (v < *items[right]) --right;
		if (left <= right)
		{
			TransportBase temp = *items[left];
			*items[left] = *items[right];
			*items[right] = temp;
			--right;
			++left;
		}
	}
	if (left < last)
		QuickSort(items, left, last);
	if (first < right)
		QuickSort(items, first, right);
}

template<class T>
void OutArray(T* Array, int SizeOfArray, ostream& stream)
{
	for (int i = 0; i < SizeOfArray; ++i)
		cout << Array[i] << " ";
	cout << endl;
}

int main()
{
	int n = 0;
	cout << "Please, enter number of elements :> ";
	cin >> n;
	TransportBase** arr = new TransportBase*[n];
	int i = 0;
	TransportBase temp;
	cout << "Now enter elements of array:" << endl;
	for (i = 0; i < n / 3; ++i)
	{
		cin >> temp;
		arr[i] = new TransportBase(temp);
	}
	
	for (i = n / 3; i < (2 * n) / 3; ++i)
	{
		cin >> temp;
		arr[i] = new LandTransport(temp.HoistingCapacity(), temp.Price(), temp.Speed(), i % 2);
	}
	
	for (i = (2 * n) / 3; i < n; ++i)
	{
		cin >> temp;
		arr[i] = new WaterTransport(temp.HoistingCapacity(), temp.Price(), temp.Speed(), i * 2);
	}

	QuickSort(arr, 0, n - 1);

	LandTransport *landTransport = 0;
	WaterTransport *waterTransport = 0;
	for (i = 0; i < n; ++i)
	{
		waterTransport = const_cast<WaterTransport*>(dynamic_cast<WaterTransport*>(arr[i]));
		landTransport = const_cast<LandTransport*>(dynamic_cast<LandTransport*>(arr[i]));

		if (waterTransport != 0)
			cout << (*waterTransport);
		else
			if (landTransport != 0)
				cout << (*landTransport);
			else
				cout << (*arr[i]);
		landTransport = 0;
		waterTransport = 0;
	}

	_getch();
	return 0;
}