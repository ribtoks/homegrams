#include <iostream>
#include <string>
#include <conio.h>
using namespace std;

int main()
{
	string s = "";
	//reading string
	while (int next = cin.get())
	{
		if (next == 13 || next == 10)
			break;
		s += char(next);
	}
	int counter = -1;
	int firstSeqPos = string::npos;
	int firstSeqLength = 0;
	int i = 0;
	while (i < s.length())
	{
		if (s[i] >= '0' && s[i] <= '9')
		{
			if (firstSeqPos == string::npos)
			{
				firstSeqPos = i;
				counter = 0;
				while (s[i] >= '0'  &&  s[i] <= '9')
				{
					++i;
					++firstSeqLength;
				}
				continue;
			}
			int tempPos = firstSeqPos;
			while (s[i] == s[tempPos]  &&  (s[i] >= '0' && s[i] <= '9'))
			{
				++i;
				++tempPos;
			}
			if ((s[i] < '0'  ||  s[i] > '9')  &&  (s[tempPos] < '0'  ||  s[tempPos] > '9'))
				++counter;
		}
		++i;
	}
	cout << endl;
	if (counter > 0)
		cout << "There are " << counter << " group(s) of digits like first." << endl;
	else
		if (counter == -1)
			cout << "There no digits groups...";
		else
			if (counter == 0)
				cout << "Threre no groups like first one...";
	_getch();
	return 0;
}