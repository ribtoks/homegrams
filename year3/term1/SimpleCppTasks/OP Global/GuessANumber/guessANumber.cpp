#include <iostream>
#include <limits>
#include <cmath>
#include <ctime>
#include <string>
#include <conio.h>
using namespace std;

int random(int max)
{
	return rand() % max;
}

int GetRandomNumber(int lengthOfNumber)
{
	srand(time(0));

	if (lengthOfNumber > 9 || lengthOfNumber < 1)
		return -1;
	int returnNumber = 0;
	returnNumber = random(9) + 1;
	for (int i = 1; i < lengthOfNumber; ++i)
	{
		int temp = random(10);
		returnNumber *= 10;
		returnNumber += temp;
	}	
	return returnNumber;
}

int GetLength(int Number)
{
	int counter = 0;
	while (Number)
	{
		Number /= 10;
		++counter;
	}
	return counter;
}

void PasteNumbers(int* NumbersArray, int Number)
{
	while (Number)
	{
		++NumbersArray[Number%10];
		Number /= 10;
	}
}

bool IterationModulation(int NumberToGuess, int LengthNumberToGuess, int TryNumber)
{
	if (GetLength(TryNumber) != LengthNumberToGuess)
	{
		cout << "Your word must have the same length as looking for." << endl;
		return false;
	}
	if (NumberToGuess == TryNumber)
		return true;
	//initializing
	int numbers[10], numbersGuessing[10];
	memset(numbers, 0, 10*sizeof(int));
	memset(numbersGuessing, 0, 10*sizeof(int));
	PasteNumbers(numbers, NumberToGuess);
	PasteNumbers(numbersGuessing, TryNumber);

	cout << "Going from right to left:" << endl;
	int i = 0;
	while (NumberToGuess)
	{
		int a = NumberToGuess % 10;
		int b = TryNumber % 10;
		if (a == b)
		{
			cout << "Correct digit at position " << i << endl;
			--numbers[a];
			--numbersGuessing[a];
		}
		NumberToGuess /= 10;
		TryNumber /= 10;
		++i;
	}
	cout << "Also we have:" << endl;
	int counter = 0;
	for (int i = 0; i < 10; ++i)
	{
		if (numbers[i] > 0 && numbersGuessing[i] > 0)
			cout << ((numbersGuessing[i] > numbers[i]) ? (numbers[i]) : (numbersGuessing[i])) << " digits " << i << " at the wrong place;" << endl;
	}
	return false;
}

int main()
{
	int lengthOfANumber = 0;
	cout << "Enter the length of number to guess: ([1, 9] == length)" << endl;
	
	cin >> lengthOfANumber;
	int NumberToGuess = GetRandomNumber(lengthOfANumber);

	cout << "Game started." << endl;
	int tryNumber;
	while(19)
	{
		cout << "Enter Next probable number :> ";
		cin >> tryNumber;
		cout << endl;
		bool tempResult = IterationModulation(NumberToGuess, lengthOfANumber, tryNumber);
		
		if (tempResult)
			break;

		cout << endl << "If you want to exit, press 'e' :> ";
		string s;
		cin >> s;
		if (s == "e" || s == "E")
		{
			cout << "Will exit now." << endl;
			return 0;
		}
		cout << endl << "----------------" << endl;
	}
	cout << "Congratulations, you guessed the number!" << endl;
	_getch();
	return 0;
}