#include <iostream>
#include <iomanip>
#include <string>
#include "LandTransport.h"
using namespace std;

bool LandTransport::HasEngine() const
{
	return hasEngine;
}

ostream& operator<<(ostream& stream, const LandTransport& transport)
{
	stream << endl << "Printing transport properties:" << endl;
	stream << "Type: " << transport.TypeToString() << endl;
	stream << "Hoisting capacity: " << transport.hoisting_capacity << endl;
	stream << "Speed: " << transport.speed << endl;		
	stream << "Price: " << transport.price << endl;
	stream << "Has engine : " << boolalpha << transport.hasEngine << endl << endl;
	return stream;
}

istream& operator>>(istream& stream, LandTransport& transport)
{
	stream >> transport.hoisting_capacity >> transport.speed >> transport.price;
	return stream;
}

bool operator<(const LandTransport& TrBase1, const LandTransport& TrBase2)
{
	return (TrBase1.Price() < TrBase2.Price());
}