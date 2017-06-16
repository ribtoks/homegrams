#include <iostream>
#include <string>
#include "WaterTransport.h"
using namespace std;

int WaterTransport::CrewNumber() const
{
	return crewNumber;
}

ostream& operator<<(ostream& stream, const WaterTransport& transport)
{
	stream << endl << "Printing transport properties:" << endl;
	stream << "Type: " << transport.TypeToString() << endl;
	stream << "Hoisting capacity: " << transport.hoisting_capacity << endl;
	stream << "Speed: " << transport.speed << endl;		
	stream << "Price: " << transport.price << endl;
	stream << "Crew Number: " << transport.crewNumber << endl << endl;
	return stream;
}

istream& operator>>(istream& stream, WaterTransport& transport)
{
	stream >> transport.hoisting_capacity >> transport.speed >> transport.price >> transport.crewNumber;
	return stream;
}

bool operator<(const WaterTransport& TrBase1, const WaterTransport& TrBase2)
{
	return (TrBase1.Price() < TrBase2.Price());
}