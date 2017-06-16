#include <iostream>
#include <string>
#include <conio.h>
using namespace std;

typedef int MatrixType;

MatrixType** ReadMatrix(int& Out_MatrHeight, int& Out_MatrWidth)
{
	cout << endl << "Enter matrix dimension: " << endl;
	int dimension;
	cin >> dimension;
	if (dimension%2 == 0)
	{
		cout << "Wrong matrix size!" << endl;
		_getch();
	}
	Out_MatrWidth = dimension;
	Out_MatrHeight = dimension;

	MatrixType** Matr = new MatrixType* [dimension];
	for (int i = 0; i < dimension; ++i)
	{
		Matr[i] = new MatrixType[dimension];
		memset(Matr[i], 0, sizeof(MatrixType)*dimension);
	}
	cout << "Now enter elements of matrix:" << endl;
	for (int i = 0; i < dimension; ++i)
		for (int j = 0; j < dimension; ++j)
			cin >> Matr[i][j];
	return Matr;
}

void PrintMatrix(const MatrixType** Matr, int MatrHeight, int MatrWidth)
{
	cout << endl << "Printing matrix:" << endl;
	cout << "--------begin--------" << endl;
	for (int i = 0; i < MatrHeight; ++i)
	{
		for (int j = 0; j < MatrWidth; ++j)
			cout << Matr[i][j] << " ";
		cout << '\b' << endl;
	}
	cout << "--------end----------" << endl;
}

//deletes matrix
void DeleteMatrix(MatrixType** Matr, int height)
{
	if (Matr == 0)
		return;
	for (int i = 0; i < height; ++i)
		delete[] Matr[i];
	delete[] Matr;
}
//do same work as operator=
void AssignMatrix(MatrixType** assignWhat, const MatrixType** assignTo, int Height, int Width)
{
	for (int i = 0; i < Height; ++i)
		for (int j = 0; j < Width; ++j)
			assignWhat[i][j] = assignTo[i][j];
}
//do same work as Copy Constructor
MatrixType** CopyMatrix(const MatrixType** From, int FromHeight, int FromWidth)
{
	MatrixType** arr = new MatrixType* [FromHeight];
	for (int i = 0; i < FromHeight; ++i)
	{
		arr[i] = new MatrixType[FromWidth];
		for (int j = 0; j < FromWidth; ++j)
			arr[i][j] = From[i][j];
	}
	return arr;
}
//transposes the matrix
void GetTransponedMatrix(MatrixType** Matr, int MatrHeight, int MatrWidth)
{
	MatrixType** arr = new MatrixType* [MatrHeight];
	for (int i = 0; i < MatrHeight; ++i)
	{
		arr[i] = new MatrixType[MatrWidth];
	}
	
	for (int i = 0; i < MatrHeight; ++i)
		for (int j = 0; j < MatrWidth; ++j)
			arr[i][j] = Matr[j][i];	

	AssignMatrix(Matr, (const MatrixType**)arr, MatrHeight, MatrWidth);
	DeleteMatrix(arr, MatrHeight);
}
//swaps two rows of a matrix
void SwapRows(MatrixType** Matr, int DimWidth, int i, int j)
{
	for (int k = 0; k < DimWidth; ++k)
	{
		MatrixType temp = Matr[i][k];
		Matr[i][k] = Matr[j][k];
		Matr[j][k] = temp;
	}
}

//swaps two columns of a matrix
void SwapColumns(MatrixType** Matr, int DimHeight, int i, int j)
{
	for (int k = 0; k < DimHeight; ++k)
	{
		MatrixType temp = Matr[k][i];
		Matr[k][i] = Matr[k][j];
		Matr[k][j] = temp;
	}
}
//reflects matrix horisontaly
void ReflectHorisontal(MatrixType** Matr, int height, int width)
{
	for (int i = 0; i < (height >> 1); ++i)
		SwapRows(Matr, width, i, height - i - 1);
}
//reflects matrix verticaly
void ReflectVertical(MatrixType** Matr, int height, int width)
{
	for (int i = 0; i < (width >> 1); ++i)
		SwapColumns(Matr, height, i, width - i - 1);
}

MatrixType** GetUpperDiagonalQuater(const MatrixType** From, int FromHeight, int FromWidth)
{
	//initializing
	MatrixType** arr = new MatrixType* [FromHeight];
	int i = 0;
	for (i = 0; i < FromHeight; ++i)
	{
		arr[i] = new MatrixType[FromWidth];
		memset(arr[i], 0, FromWidth*sizeof(MatrixType));
	}
	//copy data
	for (i = 0; i < (FromHeight >> 1); ++i)
		for (int j = i + 1; j < FromWidth - (i + 1); ++j)
			arr[i][j] = From[i][j];
	return arr;
}

MatrixType** GetLeftDiagonalQuater(const MatrixType** From, int FromHeight, int FromWidth)
{
	//initializing
	MatrixType** arr = new MatrixType* [FromHeight];
	int i = 0;
	for (i = 0; i < FromHeight; ++i)
	{
		arr[i] = new MatrixType[FromWidth];
		memset(arr[i], 0, FromWidth*sizeof(MatrixType));
	}
	//copy data
	for (i = 0; i < (FromWidth >> 1); ++i)
		for (int j = i + 1; j < FromHeight - (i + 1); ++j)
			arr[j][i] = From[j][i];
	return arr;
}

MatrixType** GetRightDiagonalQuater(const MatrixType** From, int FromHeight, int FromWidth)
{
	MatrixType **Clone = 0;
	Clone = CopyMatrix(From, FromHeight, FromWidth);
	ReflectVertical(Clone, FromHeight, FromWidth);
	MatrixType** right = GetLeftDiagonalQuater((const MatrixType**)Clone, FromHeight, FromWidth);	
	ReflectVertical(right, FromHeight, FromWidth);
	DeleteMatrix(Clone, FromHeight);
	return right;
}

MatrixType** GetDownDiagonalQuater(const MatrixType** From, int FromHeight, int FromWidth)
{
	MatrixType **Clone = 0;
	Clone = CopyMatrix(From, FromHeight, FromWidth);
	ReflectHorisontal(Clone, FromHeight, FromWidth);	
	MatrixType** down = GetUpperDiagonalQuater((const MatrixType**)Clone, FromHeight, FromWidth);	
	ReflectHorisontal(down, FromHeight, FromWidth);
	DeleteMatrix(Clone, FromHeight);
	return down;
}

MatrixType** AddMatrixes(MatrixType** A1, MatrixType** A2, int height, int width)
{
	MatrixType** arr = new MatrixType* [height];
	int i = 0, j = 0;
	for (i = 0; i < height; ++i)
	{
		arr[i] = new MatrixType[width];
	}
	for (i = 0; i < height; ++i)
		for (j = 0; j < width; ++j)
			arr[i][j] = A1[i][j] + A2[i][j];
	return arr;
}

MatrixType** AddMatrixes(const MatrixType*** MatrixArray, int MatrixArraySize, int height, int width)
{
	MatrixType** arr = new MatrixType* [height];
	int i = 0, j = 0;
	for (i = 0; i < height; ++i)
	{
		arr[i] = new MatrixType[width];
		memset(arr[i], 0, width*sizeof(MatrixType));
	}

	for (i = 0; i < height; ++i)
		for (j = 0; j < width; ++j)
			for (int k = 0; k < MatrixArraySize; ++k)
				arr[i][j] += MatrixArray[k][i][j];
	return arr;
}

MatrixType** GetDiagonals(const MatrixType** From, int FromHeight, int FromWidth)
{
	MatrixType** arr = new MatrixType* [FromHeight];
	int i = 0;
	for (i = 0; i < FromHeight; ++i)
	{
		arr[i] = new MatrixType[FromWidth];
		memset(arr[i], 0, FromWidth*sizeof(MatrixType));
	}

	for (i = 0; i < FromHeight; ++i)
		arr[i][i] = From[i][i];

	for (i = 0; i < FromWidth; ++i)
		arr[i][FromWidth - i - 1] = From[i][FromWidth - i - 1];

	return arr;
}

int main()
{
	cout << "Matrix Operations";
	MatrixType** matr = 0;
	int height = 0, width = 0;
	matr = ReadMatrix(height, width);
	PrintMatrix((const MatrixType**)matr, height, width);
	//left quater
	MatrixType** left = 0;
	left = GetLeftDiagonalQuater((const MatrixType**)matr, height, width);
	//right quater
	MatrixType** right = 0;
	right = GetRightDiagonalQuater((const MatrixType**)matr, height, width);
	//Up quater
	MatrixType** up = 0;
	up = GetUpperDiagonalQuater((const MatrixType**)matr, height, width);
	//Down quater
	MatrixType** down = 0;
	down = GetDownDiagonalQuater((const MatrixType**)matr, height, width);
	//diagonals
	MatrixType** diagonals = 0;
	diagonals = GetDiagonals((const MatrixType**)matr, height, width);

	MatrixType** left_cpy = 0;
	MatrixType** right_cpy = 0;
	MatrixType** up_cpy = 0;
	MatrixType** down_cpy = 0;

	//looping...
	int whatChange1 = 0, whatChange2 = 0;
	
	MatrixType*** Array = new MatrixType**[5];
	memset(Array, 0, 5*sizeof(MatrixType**));
	Array[0] = diagonals;
	MatrixType** result = 0;
	while (19)
	{		
		//copies for manipulatings...
		left_cpy = CopyMatrix((const MatrixType**) left, height, width);
		right_cpy = CopyMatrix((const MatrixType**) right, height, width);
		up_cpy = CopyMatrix((const MatrixType**) up, height, width);
		down_cpy = CopyMatrix((const MatrixType**) down, height, width);
		
		cout << endl << "Please, eneter two numbers which quaters must be changed" << endl;
		cin >> whatChange1 >> whatChange2;
		if (whatChange1 == whatChange2)
		{
			cout << "Error occured..." << endl;
			continue;
		}
		if (whatChange1 % 2 == whatChange2 % 2)
		{
			if (whatChange1 % 2 == 0)
			{
				ReflectVertical(right_cpy, height, width);
				ReflectVertical(left_cpy, height, width);
			}
			else
			{
				ReflectHorisontal(up_cpy, height, width);
				ReflectHorisontal(down_cpy, height, width);				
			}
		}
		else
		{
			if (whatChange1 + whatChange2 == 5)
			{
				if (whatChange1 == 1  ||  whatChange1 == 4)
				{
					GetTransponedMatrix(up_cpy, height, width);
					GetTransponedMatrix(left_cpy, height, width);
				}
				else
				{
					GetTransponedMatrix(down_cpy, height, width);
					GetTransponedMatrix(right_cpy, height, width);
				}
			}
			else
			{
				if (whatChange1 == 1 || whatChange1 == 2)
				{
					GetTransponedMatrix(up_cpy, height, width);
					ReflectVertical(up_cpy, height, width);
					GetTransponedMatrix(right_cpy, height, width);					
					ReflectHorisontal(right_cpy, height, width);
				}
				else
				{
					GetTransponedMatrix(left_cpy, height, width);
					ReflectHorisontal(left_cpy, height, width);
					GetTransponedMatrix(down_cpy, height, width);
					ReflectVertical(down_cpy, height, width);
				}
			}
		}
		Array[0] = diagonals;
		Array[1] = left_cpy;
		Array[2] = right_cpy;
		Array[3] = up_cpy;
		Array[4] = down_cpy;
		result = AddMatrixes((const MatrixType***) Array, 5, height, width);				
		PrintMatrix((const MatrixType**)result, height, width);
		cout << endl << "If you want to exit, press 'e' :> ";
		string s;
		cin >> s;
		if (s == "e" || s == "E")
		{
			cout << "Will exit now." << endl;
			DeleteMatrix(left, height);
			DeleteMatrix(right, height);
			DeleteMatrix(up, height);
			DeleteMatrix(down, height);
			DeleteMatrix(left_cpy, height);
			DeleteMatrix(right_cpy, height);
			DeleteMatrix(up_cpy, height);
			DeleteMatrix(down_cpy, height);

			DeleteMatrix(matr, height);
			delete[] Array;
			_getch();
			return 0;
		}
	}

	DeleteMatrix(left, height);
	DeleteMatrix(right, height);
	DeleteMatrix(up, height);
	DeleteMatrix(down, height);
	DeleteMatrix(left_cpy, height);
	DeleteMatrix(right_cpy, height);
	DeleteMatrix(up_cpy, height);
	DeleteMatrix(down_cpy, height);

	DeleteMatrix(matr, height);
	delete[] Array;
	_getch();
	return 0;
}