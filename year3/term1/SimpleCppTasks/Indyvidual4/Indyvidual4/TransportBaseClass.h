#pragma once
//class with 4 private fields
//that represent the parameters
//of some kind of transport

enum TransportType {Unknown, Water, Land};

class TransportBase
{
protected:
	//вантажопідйомність
	int hoisting_capacity;

	//how many it costs
	int price;

	//speed of current transport
	int speed;

	//type of transport
	TransportType type;

	//get the string eqvivalent to enum field name
	virtual std::string TypeToString() const;

public:

	///constructors
	TransportBase()
		: hoisting_capacity(0), price(0), speed(0), type(TransportType::Unknown)
	{
	}

	TransportBase(int HoistingCapacity, int Price, int Speed)
		: hoisting_capacity(HoistingCapacity), price(Price), speed(Speed), type(TransportType::Unknown)
	{
	}

	TransportBase(const TransportBase& from)
		: hoisting_capacity(from.hoisting_capacity), price(from.price), speed(from.speed), type(from.type)
	{
	}
	///end of constructors

	TransportBase& operator=(const TransportBase& trBase);

	//"properties"
	int Speed() const;
	int Price() const;
	int HoistingCapacity() const;
	TransportType Type() const;

	//friends of this class
	friend std::ostream& operator<<(std::ostream& stream, const TransportBase& transport);
	friend std::istream& operator>>(std::istream& stream, TransportBase& transport);
};

bool operator<(const TransportBase& TrBase1, const TransportBase& TrBase2);