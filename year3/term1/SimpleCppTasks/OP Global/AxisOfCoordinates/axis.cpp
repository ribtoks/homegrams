#include <iostream>
#include <conio.h>
using namespace std;

int main()
{
	double x, y;
	cout << "Enter coordinates of Point: ";
	cin >> x >> y;
	if (x == 0  &&  y == 0)
		cout << "It is a center of coordinates." << endl;
	else
		if (x == 0)
			cout << "It is X axis." << endl;
		else
			if (y == 0)
				cout << "It is Y axis." << endl;
			else
			{
				if (x > 0  &&  y > 0)
					cout << "It is 1-st quarter." << endl;
				else
					if (x < 0  &&  y < 0)
						cout << "It is 3-d quarter." << endl;
					else
						if (x > 0  &&  y < 0)
							cout << "It is 4-th quarter." << endl;
						else
							if (x < 0  &&  y > 0)
								cout << "It is 2-nd quarter." << endl;
			}
	_getch();
	return 0;
}