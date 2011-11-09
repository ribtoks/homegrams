#pragma once
#include "TransportBaseClass.h"

class WaterTransport : public TransportBase
{
protected:
	//number of people in water transport team
	int crewNumber;

public:
	///constructor Area
	WaterTransport()
		: TransportBase(0, 0, 0), crewNumber(0)
	{
		type = TransportType::Water;
	}

	WaterTransport(int HoistingCapacity, int Price, int Speed, int CrewNumber)
		: TransportBase(HoistingCapacity, Price, Speed), crewNumber(CrewNumber)
	{
		type = TransportType::Water;
	}

	WaterTransport(const WaterTransport& from)
		:TransportBase(from.hoisting_capacity, from.price, from.speed), crewNumber(from.crewNumber)		
	{
		type = TransportType::Water;
	}
	///end of constructor Area

	int CrewNumber() const;

	//friends of this class
	friend std::ostream& operator<<(std::ostream& stream, const WaterTransport& transport);
	friend std::istream& operator>>(std::istream& stream, WaterTransport& transport);
};

bool operator<(const WaterTransport& TrBase1, const WaterTransport& TrBase2);