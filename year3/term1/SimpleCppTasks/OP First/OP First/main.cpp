#include <iostream>
#include <string>
#include <vector>
#include <conio.h>
using namespace std;

class MyDictionary
{
	friend istream& operator>>(istream &stream, MyDictionary &dict);
	friend ostream& operator<<(ostream &stream, const MyDictionary &dict);

private:
	string *sentence;
	int wordsNumber;
	string wholeSentence;

public:
	
	//constructors

	MyDictionary()
	{
		sentence = 0;
		wordsNumber = 0;
		wholeSentence = "";
	}

	MyDictionary(string Sentence)
	{
	}

	MyDictionary(int WordsNumber)
	{
		sentence = new string[WordsNumber];		
		wordsNumber = WordsNumber;
	}

	MyDictionary(const MyDictionary& from)
	{
		sentence = new string[from.wordsNumber];
		wordsNumber = from.wordsNumber;
		wholeSentence = from.wholeSentence;
	}
	//end of constructors

	~MyDictionary()
	{
		delete[] sentence;
	}

	int GetWordsNumber()
	{
		return wordsNumber;
	}

	bool Replace(string wordToReplace, string replaceWith)
	{
		for (int i = 0; i < wordsNumber; ++i)
			if (sentence[i] == wordToReplace)
			{
				sentence[i] = replaceWith;
				return true;
			}

		return false;
	}

	bool Replace(int IndexToReplace,  string replaceWith)
	{
		if (IndexToReplace >= 0  &&  IndexToReplace < wordsNumber)
		{
			sentence[IndexToReplace] = replaceWith;
			return true;
		}
		return false;
	}

	int Find(string FindWhat)
	{
		for (int i = 0; i < wordsNumber; ++i)
			if (sentence[i] == FindWhat)
			{
				return i;
			}
		return -1;
	}

	string GetWord(int Index)
	{
		return sentence[Index];
	}

	bool IsEqual(int Index, string word)
	{
		return (sentence[Index] == word);
	}
};

istream& operator>>(istream &stream, MyDictionary &dict)
{
	vector<string> vect;
	string curr = "";
	string res = "";
	while (int next = stream.get())
	{
		if (next == 13 || next == 10)
		{
			vect.push_back(curr);
			res += curr;
			break;
		}
		if (next == 32)
		{
			vect.push_back(curr + " ");
			res += curr + " ";
			curr = "";
		}
		else
			curr += (char)next;
	}
	dict.sentence = new string[vect.size()];
	for (int i = 0; i < vect.size(); ++i)
	{
		dict.sentence[i] = vect[i];
	}
	dict.wordsNumber = vect.size();
	dict.wholeSentence = res;

	return stream;
}

bool AreWordsEqual(MyDictionary &d1, MyDictionary &d2)
{
	if (d1.GetWordsNumber() != d2.GetWordsNumber())
		return false;
	for (int i = 0; i < d1.GetWordsNumber(); ++i)
	{
		if (!d1.IsEqual(i, d2.GetWord(i)))
			return false;
	}
	return true;
}

bool HaveWordsEqual(MyDictionary &d1, MyDictionary &d2)
{
	for (int i = 0; i < d1.GetWordsNumber(); ++i)
	{
		int index = d2.Find(d1.GetWord(i));
		if (index != -1)
			return true;
	}
	return false;
}

ostream& operator<<(ostream &stream, const MyDictionary &dict)
{
	stream << dict.wholeSentence;
	return stream;
}


int main()
{
	MyDictionary d1, d2;
	cin >> d1 >> d2;
	if (d1.GetWordsNumber() < 3  ||  d2.GetWordsNumber() < 3)
	{
		cout << "Was read other sentence!" << endl;
	}

	if (!HaveWordsEqual(d1, d2))
	{
		cout << endl << d1 << endl << d2 << endl;
	}
	else
	{
		cout << endl << "Equal words:" << endl;
		//typing of all equal words
		for (int i = 0; i < d1.GetWordsNumber(); ++i)
		{
			string s = d1.GetWord(i);
			int index = d2.Find(s);
			if (index != -1)
				cout << s << endl;
		}
	}

	int wait = _getch();
	return 0;
}