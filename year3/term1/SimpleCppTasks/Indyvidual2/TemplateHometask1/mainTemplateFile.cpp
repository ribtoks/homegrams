#include <iostream>
#include <fstream>
#include <string>
#include <conio.h>
using namespace std;

template<class T> void AscendingQuickSort(T* arr, int first, int last)
{
	int left = first;
	int right = last;
	T v = arr[(left + right) >> 1];
	while (left <= right)
	{
		while (arr[left] < v) ++left;
		while (arr[right] > v) --right;
		if (left <= right)
		{
			T temp = arr[right];
			arr[right] = arr[left];
			arr[left] = temp;
			++left; --right;
		}
	}
	if (left < last)
		AscendingQuickSort(arr, left, last);
	if (right > first)
		AscendingQuickSort(arr, first, right);
}

template<class T> void DescendingQuickSort(T* arr, int first, int last)
{
	int left = first;
	int right = last;
	T v = arr[(left + right) >> 1];
	while (left <= right)
	{
		while (arr[left] > v) ++left;
		while (arr[right] < v) --right;
		if (left <= right)
		{
			T temp = arr[right];
			arr[right] = arr[left];
			arr[left] = temp;
			++left;
			--right;
		}
	}
	if (left < last)
		DescendingQuickSort(arr, left, last);
	if (right > first)
		DescendingQuickSort(arr, first, right);
}

template<class Q> void OutArray(Q* items, int start, int count, ostream& stream)
{
	for (int i = start; i < count; i++)
		stream << items[i] << " ";
	stream << '\b' << endl;
}

//returns nsize of array
template<class Class> int ReadArray(Class* items, int currSize, ifstream& input)
{
	if (!input)
		return -1;
	int currPos = 0;
	while (!input.eof())
	{
		Class item = 0;
		input >> item;
		items[currPos] = item;
		if (typeid(item) == typeid(char))
			if (item == 0)
				continue;
		++currPos;
//		if (currPos == currSize)
//			currSize = ExtendArray(items, currSize);
	}
	return currPos;
}
//returns new size
template<class T> int ExtendArray(T* items, int currSize)
{
	int newSize = (currSize << 1) + 1;
	T* newItems  = new T[newSize];
	memset(newItems, 0, sizeof(T)*newSize);
	for (int i = 0; i < currSize; ++i)
		newItems[i] = items[i];
	delete[] items;
	items = 0;
	items = new T[newSize];
	memset(items, 0, sizeof(T)*newSize);

	for (int i = 0; i < newSize; ++i)
		items[i] = newItems[i];
	delete[] newItems;
	newItems = 0;
	return newSize;
}

int main()
{
	cout << "Please, enter path to file :> ";
	string s;
	cin >> s;
	ifstream input(s.c_str());
	if (!input)
	{
		cout << "Can not open file!" << endl;
		_getch();
	}
	
	string st = "";
	bool isInt = false;
	input >> st;
	if (st == "int")
		isInt = true;

	if (isInt)
	{
		int currSize = 5000;
		int* arr = new int[currSize];
		cout << endl;
		int size = ReadArray(arr, currSize, input);
		OutArray(arr, 0, size, cout);
		cout << endl;
		DescendingQuickSort(arr, 0, (size >> 1) - 1);
		OutArray(arr, 0, size, cout);		
		AscendingQuickSort(arr, (size >> 1), size - 1);
		cout << endl;
		OutArray(arr, 0, size, cout);
		delete[] arr;
	}
	else
	{
		int currSize = 5000;
		char* arr = new char[currSize];
		cout << endl;
		int size = ReadArray(arr, currSize, input);
		OutArray(arr, 0, size, cout);
		cout << endl;
		DescendingQuickSort(arr, 0, (size >> 1) - 1);
		OutArray(arr, 0, size, cout);
		cout << endl;
		AscendingQuickSort(arr, (size >> 1), size - 1);		
		OutArray(arr, 0, size, cout);
		delete[] arr;
	}
	input.close();
	_getch();
	return 0;
}