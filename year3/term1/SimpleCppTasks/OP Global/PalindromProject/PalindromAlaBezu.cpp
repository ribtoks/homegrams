#include <iostream>
#include <string>
#include <conio.h>
using namespace std;

//returns length of palindrom
int FindOddPalindrome(string& s, int pos)
{
	int length = 0;
	int i, j;
	for (i = pos + 1, j = pos - 1; (i < s.length() & j >= 0); ++i, --j)
	{
		if (s[i] == s[j])
			++length;
		else
			break;
	}
	if (length > 0)
		return 2*length + 1;
	else
		return 0;
}

//returns length of palindrom
//pos - position of start of palindrom  abb == 1
int FindEvenPalindrome(string& s, int pos)
{
	int i, j;
	int length = 0;
	if (s[pos + 1] == s[pos])
	{
		++length;	
		for (i = pos + 2, j = pos - 1; (i < s.length() & j >= 0); ++i, --j)
		{
			if (s[i] == s[j])
				++length;
			else
				break;
		}
	}
	return length*2;
}

bool HasLinePalindrome(string& s, int& out_length, int& out_pos, bool& out_is_odd)
{
	int max_length = 0;
	int length = 0;
	for (int i = 0; i < s.length(); ++i)
	{
		length = FindOddPalindrome(s, i);
		if (length > max_length)
		{
			max_length = length;
			out_is_odd = true;
			out_pos = i;
			out_length = length;
		}
		length = FindEvenPalindrome(s, i);
		if (length > max_length)
		{
			max_length = length;
			out_is_odd = false;
			out_pos = i;
			out_length = length;
		}
	}
	if (max_length == 0)
		return false;
	else
		return true;
}



string BuildBackPalindrome(string s, int pos)
{
	string result(s);
	for (int i = pos; i < s.length(); ++i)
		result = s[i] + result;
	return result;
}

string BuildFrontPalindrome(string s, int pos)
{
	string result(s);
	for (int i = pos; i >= 0; --i)
		result += s[i];
	return result;
}

string BuildWholePalindrome(string s)
{
	return BuildBackPalindrome(s, 1);
}

bool CanBuild(string& s, int length, int pos, bool IsOdd)
{
	//if end of palindrom lies at the ends of word
	int halfLength = 0;
	if (IsOdd)
	{
		halfLength = (length - 1) >> 1;
		if (pos - halfLength == 0)
			return true;
		if (pos + halfLength == s.length() - 1)
			return true;
		return false;
	}
	else
	{
		halfLength = length >> 1;		
		if (pos - halfLength + 1 == 0)
			return true;
		if (pos + halfLength == s.length() - 1)
			return true;
		return false;
	}
}

int main()
{
	string s;
	//enter line - palindrom-alpha
	cout << "Please, enter a line (probable palindrome)." << endl << ":>";
	cin >> s;
	int length = 0, pos = -1;
	bool isOdd = false;
	//find probable palindrome
	if (HasLinePalindrome(s, length, pos, isOdd))
	{
		//if we can build palindrome only adding symbols
		if (CanBuild(s, length, pos, isOdd))
		{
			int halfLength = 0;
			if (isOdd)
			{
				halfLength = (length - 1) >> 1;
				//if palindrome starts at the beginning of the word
				if (pos - halfLength == 0)
				{
					s = BuildBackPalindrome(s, pos + halfLength + 1);
				}
				else
					//if palindrome ends at the end of the word
					if (pos + halfLength == s.length() - 1)
					{
						s = BuildFrontPalindrome(s, pos - halfLength - 1);
					}
			}
			else
			{
				halfLength = length >> 1;	
				//if palindrome starts at the beginning of the word
				if (pos - halfLength + 1 == 0)
				{
					s = BuildBackPalindrome(s, pos + halfLength + 1);
				}
				else
					//if palindrome ends at the end of the word
					if (pos + halfLength == s.length() - 1)
					{
						s = BuildFrontPalindrome(s, pos - halfLength);
					}
			}
		}
		else
			s = BuildWholePalindrome(s);
	}
	else
	{
		s = BuildWholePalindrome(s);
	}
	cout << "The shortest palindrome is " << s << endl;
	_getch();
	return 0;
}