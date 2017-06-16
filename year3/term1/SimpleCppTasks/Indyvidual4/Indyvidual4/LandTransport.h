#pragma once
#include "TransportBaseClass.h"

class LandTransport : public TransportBase
{
private:
	bool hasEngine;

public:
	///constructor Area
	LandTransport()
		: TransportBase(0, 0, 0), hasEngine(false)
	{
		type = TransportType::Land;
	}

	LandTransport(int HoistingCapacity, int Price, int Speed, bool HasEngine)
		: TransportBase(HoistingCapacity, Price, Speed), hasEngine(HasEngine)
	{
		type = TransportType::Land;
	}

	LandTransport(const LandTransport& from)
		:TransportBase(from.hoisting_capacity, from.price, from.speed), hasEngine(from.hasEngine)		
	{
		type = TransportType::Land;
	}
	///end of constructor Area

	bool HasEngine() const;

	//friends of this class
	friend std::ostream& operator<<(std::ostream& stream, const LandTransport& transport);
	friend std::istream& operator>>(std::istream& stream, LandTransport& transport);
};

bool operator<(const LandTransport& TrBase1, const LandTransport& TrBase2);