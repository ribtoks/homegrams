// project created on 4/26/2009 at 10:31 AM
#include <iostream>
#include <cstdio>
#include <cstdlib>
#include <ctime>
using namespace std;

const char* namesArray[] = {"tempFile1.txt", "tempFile2.txt", "tempFile3.txt", "tempFile4.txt", "tempFile5.txt", "tempFile6.txt", "tempFile7.txt", "tempFile8.txt",
							"tempFile9.txt", "tempFile10.txt", "tempFile11.txt", "tempFile12.txt", "tempFile13.txt", "tempFile14.txt", "tempFile15.txt", "tempFile16.txt",
							"tempFile17.txt", "tempFile18.txt", "tempFile19.txt", "tempFile20.txt", "tempFile21.txt", "tempFile22.txt"};

const int MAX_QUARTER_SIZE = 400001;

const int MAX_FILES_NUMBER = 15;

int *Array;

int* Arrays[MAX_FILES_NUMBER];

int ArraysSizes[MAX_FILES_NUMBER];

int ArraysLastUsed[MAX_FILES_NUMBER];

FILE* FileArray[MAX_FILES_NUMBER];
bool IsUsed[MAX_FILES_NUMBER];
int arr[MAX_FILES_NUMBER];

int QuickSortFirst = 0, QuickSortLast = 0, QuickSortTemp = 0;

//simple quick sort
void QuickSort()
{
	int tempFL = 0;
	int left = QuickSortFirst;
	int right = QuickSortLast;
	int v = Array[(left + right) >> 1];
	QuickSortTemp = 0;
	while (left <= right)
	{
		while (Array[left] < v) ++left;
		while (Array[right] > v) --right;
		if (left <= right)
		{
			QuickSortTemp = Array[left];
			Array[left] = Array[right];
			Array[right] = QuickSortTemp;
			++left; --right;
		}
	}
	if (right > QuickSortFirst)
	{
		tempFL = QuickSortLast;
		QuickSortLast = right;
		QuickSort();
		QuickSortLast = tempFL;
	}
	if (left < QuickSortLast)
	{
		tempFL = QuickSortFirst;
		QuickSortFirst = left;
		QuickSort();
		QuickSortFirst = tempFL;
	}
}

//file should be opened before this func
//returns the number of opened files
int ProcessSorting(FILE* file)
{
	int j = 0, i = 0, next = 0;	
	FILE* tempFile;
	while (!feof(file))
	{
		i = 0;		
		while (i < MAX_QUARTER_SIZE  &&  !feof(file))
		{
			fscanf(file, "%d ", &next);
			Array[i] = next;
			++i;
		}
		QuickSortFirst = 0;
		QuickSortLast = i - 1;
		QuickSort();
		
		tempFile = fopen(namesArray[j], "wb");	
		//writing array to file
		fwrite(Array, sizeof(int), i, tempFile);
		fclose(tempFile);
		++j;
	}
	fclose(file);
	delete[] Array;
	return j;
}

void ProcessMerging(int FilesNumber)
{
	int i = 0, temp = 0;
	int MAX_ARRAYS_LENGTH = MAX_QUARTER_SIZE / FilesNumber;
	bool empty = false;	
	FILE* resultFile = fopen("ResultFile.txt", "r+");
	for (i = 0; i < FilesNumber; ++i)
	{
		Arrays[i] = new int[MAX_ARRAYS_LENGTH];
		FileArray[i] = fopen(namesArray[i], "rb");
		ArraysSizes[i] = fread(Arrays[i], sizeof(int), MAX_ARRAYS_LENGTH, FileArray[i]);
		ArraysLastUsed[i] = 0;
	}

	//start
	for (i = 0; i < FilesNumber; ++i)
		if (!feof(FileArray[i]))
		{
			arr[i] = Arrays[i][ ArraysLastUsed[i] ];
			++ArraysLastUsed[i];
			IsUsed[i] = false;			
		}
		else
			IsUsed[i] = true;

	int first = 0, temp2 = 0;
	int m = 0, min = 0, j = 0;
	while (!empty)
	{
		first = 0;
		empty = true;
		while ((first < FilesNumber)  &&  empty)
			if (IsUsed[first])
				++first;
			else
				empty = false;
		if (!empty)
		{
			m = first;
			min = arr[m];
			for (j = first + 1; j < FilesNumber; ++j)
				if ((!IsUsed[j])  &&  (arr[j] < min))
				{
					m = j;
					min = arr[m];
				}
			temp2 = arr[m];
			fprintf(resultFile, "%d ", temp2);
			if (ArraysLastUsed[m] < ArraysSizes[m])
			{
				arr[m] = Arrays[m][ArraysLastUsed[m]];
				++ArraysLastUsed[m];
			}
			else
				if (!feof(FileArray[m]))
				{
					ArraysSizes[m] = fread(Arrays[m], sizeof(int), MAX_ARRAYS_LENGTH, FileArray[m]);
					ArraysLastUsed[m] = 0;
					arr[m] = Arrays[m][ArraysLastUsed[m]];
					++ArraysLastUsed[m];
				}
				else
					IsUsed[m] = true;
		}
	}

	//end
	fclose(resultFile);
}

int main ()
{
	FILE* test = fopen("test.txt", "r");
	if (test == 0)
	{
		cout << "Can not open test.txt";
		return 0;
	}
	FILE* resultFile = fopen("ResultFile.txt", "w");
	fclose(resultFile);

	Array = new int[MAX_QUARTER_SIZE];
	clock_t start = 0, finish = 0;
	cout << "Now program starts. Press Enter to start." << endl;
	getchar();
	start = clock();

	int a = ProcessSorting(test);
	ProcessMerging(a);
	finish = clock();
	cout << "Program worked for " << ((double)(finish - start) / CLOCKS_PER_SEC) << " seconds." << endl << "Press Enter to exit...";	

	for (int i = 0; i < a; ++i)
	{
		fclose(FileArray[i]);
		delete[] Arrays[i];
		remove(namesArray[i]);
	}

	getchar();
	return 0;
}
