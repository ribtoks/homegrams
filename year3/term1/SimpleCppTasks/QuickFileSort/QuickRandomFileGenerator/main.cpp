// project created on 4/26/2009 at 10:32 AM
#include <iostream>
#include <ctime>
#include <cstdio>
#include <cstdlib>
#include <limits>
using namespace std;

int MyRand()
{
	__int64 a = 0;
	int b = rand();
	__int64 sign = 1;
	int c = rand()%4;
	if (c == 3)
		sign = -1;
	a = b;
	a = a*55347;
	a %= numeric_limits<int>::max();
	a *= sign;
	return (int)a;
}

//const int MAX_INTS = 2*1000*930; //1 860 0000

int main (int argc, char *argv[])
{
	FILE* file;
	file = fopen("test.txt", "w");
	srand(time(0));

	int number = 0;
	cout << "Please, enter number of int's :> ";
	cin >> number;
	
	int next = rand();
	for (int i = 0; i < number; ++i)
	{
		next = MyRand();
		fprintf(file, "%d ", next);
	}
//	next = rand();
//	fprintf(file, "%d", next);
	fclose(file);
	return 0;
}

/*
	if (fopen_s(&test, "test.txt", "rb") == 0)
	{
		int a = ProcessSorting(test);
		fclose(test);
		ProcessMerging(a);
	}
	*/