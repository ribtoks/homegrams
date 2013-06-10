#include <iostream>
#include <map>
#include <string>
#include <cstdio>
using namespace std;

enum Direction {Right,Left,Straight};

class CurrState {
public:
	int cstate;
	CurrState(int st) { CurrState::cstate=st ; }
};

class Stemp {
protected:
	map<char, int> state;
	map<char, char>letter;
	map<char, char> direct;
public:
	void SetNextState(char curr, int st) 
	{
		state.insert(pair<char, int>(curr, st));
	}

	void SetNextLetter(char curr, char res) 
	{
		letter.insert(pair<char, char>(curr, res));
	}

	void SetNextDir(char ch, char d)
	{
		direct.insert(pair<char, char>(ch, d));
	}

	int NextState(char ch)
	{
		map<char,int>::iterator p;
		p = state.find(ch);
		if(p != state.end())
	    return p->second;
		else{
			throw 1;//State error::not defined
			return 0;
		}
	}
	char NextLetter(char ch) 
	{
		map<char, char>::iterator c;
		c = letter.find(ch);
		if(c != letter.end())
		return c->second;
		else{
			throw 2;//letter error :: not defined
	return '^';
		}
	}
	
	char GetDir(char ch) 
	{
		map<char,char>::iterator cs;
		cs = direct.find(ch);
		if(cs != direct.end())
		return cs->second;
		else{
			throw 3;//direction error :: not defined
		return 'S';
	}
	}

};

class Command {
public:
	int q_curr, q_res;
	char ch_curr, ch_res;
	bool error;
	char d;
	
	Command() 
	{
		q_curr = 0;
		q_res = 0;
		ch_curr = ' ';
		ch_res = ' ';
		d = 'S';
		error = false;
	}

	Command(string s) 
	{
		error = false;
		d = 'S';
		const char *a = 0;
		a = s.data();
		//char ch;
		int y = sscanf(a, "q%d %c->%c%cq%d", &q_curr, &ch_curr, &ch_res, &d, &q_res);
	if(y != 5)
		error = true;
	if(d != 'R' && d != 'L' && d != 'S')
		error = true;
	}
	void Parse ( string s ) 
	{
		error = false;
		d = 'S';
		const char *a = 0;
		a = s.data();
		//char ch;
		int y = sscanf(a, "q%d %c->%c%cq%d", &q_curr, &ch_curr, &ch_res, &d, &q_res);
	if(y != 5)
		error = true;
	if(d != 'R' && d != 'L' && d != 'S')
		error = true;
	
	}
};

class WorkLine : virtual public CurrState
{		
	public:
		string line;
		int curr_num;
		WorkLine():CurrState(1) {
			line = "";
			curr_num = 0;
		}
		WorkLine(string s):CurrState(1) {
			line = s;
			for(int i = 0; i < 21; i++)
			   line.insert(0, "^");
            for(int i = 0; i < 21; i++)
				line.insert(line.length(), "^");
			curr_num = 21;
		}

		void MakeLine(string s){
			line.clear();
			line.insert(0, s);
			for(int i = 0; i < 40; i++)
			   line.insert(0, "^");
            for(int i = 0; i < 40; i++)
				line.insert(line.length(), "^");
			curr_num = 40;
			cstate = 1;
		}

		void MakeCommand(Stemp *st) {
		   int i = 0;
		   char dir;
		   try
		   {
			   dir = st->GetDir(line[curr_num]);
			   cstate = st->NextState(line[curr_num]);
		   }
		   catch(int f)
		   {
			   throw f;
		   }
			switch(dir) {
				case 'R':
					if(curr_num == line.length())
					{
						for(int j = 0; j < 21; j++)
							line.insert(line.length(), "^");
					}
					if(curr_num == line.length() - 1)
					{
						for(int j = 0; j < 21; j++)
							line.insert(line.length() - 1, "^");
					}
					i = 1;
					break;
				case 'L':
					if(curr_num == 0){
						for(int j = 0; j < 21; j++)
							line.insert(0, "^");						
						curr_num = 21;
					}
					i = -1;
					break;
			}

			line[curr_num] = st->NextLetter(line[curr_num]);			
			curr_num += i;
		}
};
			



			
			




