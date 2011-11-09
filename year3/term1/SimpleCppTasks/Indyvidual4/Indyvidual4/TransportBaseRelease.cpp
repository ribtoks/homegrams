#include <iostream>
#include <string>
#include "TransportBaseClass.h"
using namespace std;

int TransportBase::Speed() const
{
	return speed;
}

int TransportBase::Price() const
{
	return price;
}

int TransportBase::HoistingCapacity() const
{
	return hoisting_capacity;
}

TransportType TransportBase::Type() const
{
	return type;
}

string TransportBase::TypeToString() const
{
	if (type == TransportType::Land)
		return "Land";
	if (type == TransportType::Water)
		return "Water";
	return "Unknown";
}

ostream& operator<<(ostream& stream, const TransportBase& transport)
{
	stream << endl << "Printing transport properties:" << endl;
	stream << "Type: " << transport.TypeToString() << endl;
	stream << "Hoisting capacity: " << transport.hoisting_capacity << endl;
	stream << "Speed: " << transport.speed << endl;		
	stream << "Price: " << transport.price << endl << endl;
	return stream;
}

istream& operator>>(istream& stream, TransportBase& transport)
{
	stream >> transport.hoisting_capacity >> transport.speed >> transport.price;
	return stream;
}

bool operator<(const TransportBase& TrBase1, const TransportBase& TrBase2)
{
	return (TrBase1.Price() < TrBase2.Price());
}

TransportBase& TransportBase::operator=(const TransportBase& trBase)
{
	hoisting_capacity = trBase.hoisting_capacity;
	speed = trBase.speed;
	price = trBase.price;
	type = trBase.type;
	return *this;
}