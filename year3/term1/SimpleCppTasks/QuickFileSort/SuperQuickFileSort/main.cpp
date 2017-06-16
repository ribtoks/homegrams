// project created on 4/26/2009 at 10:31 AM
#include <iostream>
#include <cstdio>
#include <cstdlib>
#include <ctime>
using namespace std;

const int MAX_QUARTER_SIZE = 400000;
int Array[MAX_QUARTER_SIZE];

int main()
{
	memset(Array, 0, MAX_QUARTER_SIZE*sizeof(int));
	FILE* test = fopen("test.txt", "r");
	if (test == 0)
	{
		cout << "Can not open test.txt";
		return 0;
	}
	cout << "Now program starts. Press Enter to start." << endl;
	getchar();
	clock_t start = 0, finish = 0;
	start = clock();


	//start
	int i = 0, next = 0;
	while (!feof(test))
	{
		fscanf(test, "%d ", &next);
		++Array[next];
		++i;
	}
	fclose(test);

	FILE* resultFile = fopen("ResultFileSuperQuick.txt", "w");
	i = 0;
	while (i < MAX_QUARTER_SIZE)
	{
		if (Array[i])
			while (Array[i])
			{
				fprintf(resultFile, "%d ", i);
				--Array[i];
			}
		++i;
	}
	fclose(resultFile);
	//end
	
	
	finish = clock();
	cout << "Program worked for " << ((double)(finish - start) / CLOCKS_PER_SEC) << " seconds" << endl << "Press enter to exit...";
	getchar();
	return 0;
}