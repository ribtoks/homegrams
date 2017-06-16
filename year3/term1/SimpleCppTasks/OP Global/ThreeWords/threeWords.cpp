#include <iostream>
#include <conio.h>
#include <string>
using namespace std;

int main()
{
	int arr1[256];
	int arr2[256];
	int arr3[256];
	memset(arr1, 0, 256*sizeof(int));
	memset(arr2, 0, 256*sizeof(int));
	memset(arr3, 0, 256*sizeof(int));
	
	string s1, s2, s3;
	cout << "Please, enter three strings:" << endl;
	cin >> s1 >> s2 >> s3;

	//initializing
	int i = 0;
	for (i = 0; i < s1.length(); ++i)
		++arr1[s1[i]];

	for (i = 0; i < s2.length(); ++i)
		++arr2[s2[i]];

	for (i = 0; i < s3.length(); ++i)
		++arr3[s3[i]];

	cout << "For three string there are next union chars:" << endl;
	//solving
	for (i = 0; i < 256; ++i)
	{
		if (arr1[i] > 0  &&  arr2[i] > 0  &&  arr3[i] > 0)
		{
			char ch = i;
			cout << ch << " ";
		}
	}
	cout << '\b';
	_getch();
	return 0;
}