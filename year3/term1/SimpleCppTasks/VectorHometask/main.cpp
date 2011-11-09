/* 
 * File:   main.cpp
 * Author: taras
 *
 * Created on October 13, 2009, 1:47 PM
 */

#include <iostream>
#include <vector>
#include <algorithm>
#include <iterator>
#include <ctime>
using namespace std;

struct RandIntGenerator
{
private:
    int low;
    int high;

public:
    RandIntGenerator(int l, int h)
            :low(l), high(h)
            {
            }
    int operator()()const
    {
        return low + (rand()%((high - low) + 1));
    }
};

int main()
{
    srand(time(0));

    int min = 0, max = 0;
    int n = 0;
    cout << "Enter number of elements in vector >: ";
    cin >> n;

    cout << endl << "Enter minimum possible value in vector >: ";
    cin >> min;

    cout << endl << "Enter maximum possible value in vector >: ";
    cin >> max;

    vector<int> v(n);
    generate(v.begin(), v.end(), RandIntGenerator(min, max));

    copy(v.begin(), v.end(), ostream_iterator<int>(cout, " "));

    cout << "Doh" << endl;
    return (0);
}

