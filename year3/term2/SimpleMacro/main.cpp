#include <iostream>
#include <string>
#include <sstream>
#include <fstream>
#include <map>
using namespace std;

void ReplaceString(string& line, const string& pattern, const string& what)
{
   int findIndex = line.find(pattern);

   while (findIndex != string::npos)
   {
      line.erase(findIndex, pattern.length());
      line.insert(findIndex, what);

      findIndex = line.find(pattern, findIndex);
   }
}

int main()
{ 
   string line, macrosfilename;

   cout << "Enter macros filename: ";
   cin >> macrosfilename;

   cout << endl;

   cout << "Enter filename: ";
   cin >> line;

   map<string, string> mapper;

   ifstream macrosfile(macrosfilename.c_str());
   
   string macros;

   while (!macrosfile.eof())
   {
      getline(macrosfile, macros);
      cout << macros << endl;
      if (macros.length() > 8)
	 if (macros.substr(0, 8) == "$mymacro")      
	  {
	     stringstream ss(macros);
	     string name, value;
	     string s;

	     ss >> s;
	     ss >> name >> value;

	     mapper[name] = value;
	     cout << name << " " << value << endl;
	  }
   }
   macrosfile.close();


   ifstream file(line.c_str());

   char c;
   string filecontents;

   while (file.get(c))
      filecontents += c;
   file.close();

   map<string, string>::iterator it;

   for (it = mapper.begin(); it != mapper.end(); ++it)
      ReplaceString(filecontents, it->first, it->second);

   cout << filecontents << endl;
   remove(line.c_str());
   ofstream out(line.c_str());

   out << filecontents;
   out.close();
   
   return 0;
}
