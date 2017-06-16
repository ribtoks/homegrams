#include <iostream>
#include <cstdlib>
#include <sstream>
#include <string>
#include <iomanip>
#include "classTime.h"

using namespace std;

Time::Time(const char *TimeInString)
{
	int wasRead = sscanf(TimeInString, "%d%*c%d%*c%d", &hour, &minute, &second);
	if (wasRead < 3)
		throw "Time in Line has no all parameters!";
}

Time::Time(const Time &t)
{
	hour = t.hour;
	minute = t.minute;
	second = t.second;
	is24hour = t.is24hour;
}

const Time operator +(const Time &t1, const Time& t2)
{
	Time time(t1.GetHour() + t2.GetHour(), t1.GetMinute() + t2.GetMinute(), t1.GetSecond() + t2.GetSecond());
	if (!time.IsTimeCorrect())
		time.MakeCorrectness();
	return time;
}

bool operator <(const Time& t1, const Time& t2)
{
	if (t1.GetHour() < t2.GetHour())
		return true;
	if (t1.GetHour() > t2.GetHour())
		return false;
	if (t1.GetMinute() < t2.GetMinute())
		return true;
	if (t1.GetMinute() > t2.GetMinute())
		return false;
	if (t1.GetSecond() < t2.GetSecond())
		return true;
	return false;
}

bool operator ==(const Time& t1, const Time& t2)
{
	return (t1.GetHour() == t2.GetHour()  &&  t1.GetMinute() == t2.GetMinute()  &&  t1.GetSecond() == t2.GetSecond());
}

bool Time::IsTimeCorrect()
{
	if (second > 59  ||  second < 0)
		return false;
	if (minute > 59  ||  minute < 0)
		return false;
	if (is24hour)
		if (hour > 23  ||  hour < 0)
			return false;
	if (hour < 0)
		return false;
	return true;
}

void Time::MakeCorrectness()
{
	/*
	if (second < 0)
		throw "Seconds must be positive!";
	if (minute < 0)
		throw "Minutes must be positive!";
	if (hour < 0)
		throw "Hours must be positive!";
	*/
	if (second > 59)
	{
		second %= 60;
		++minute;
	}

	if (minute > 59)
	{
		minute %= 60;
		++hour;
	}

	if (is24hour)
		if (hour > 23)
			throw "Hours must be less than 23!";
}

Time& Time::operator =(const Time &time)
{
	hour = time.hour;
	minute = time.minute;
	second = time.second;
	is24hour = time.is24hour;
	SetDelimiter(time.GetDelimiter());
	return *this;
}

Time& Time::operator +=(const Time &time)
{
	hour += time.hour;
	minute += time.minute;
	second += time.second;
	return *this;
}

string Time::ToString() const
{
	stringstream ss;
	ss << hour << delimiter << minute << delimiter << second;
	return string(ss.str());
}

ostream& operator<< (ostream& stream, const Time& t)
{
	char del = t.GetDelimiter();
	stream << t.GetHour() << del << t.GetMinute() << del << t.GetSecond();
	return stream;
}

std::istream& operator>>(std::istream& stream, Time &t)
{
	string s;
	stream >> s;
	Time time(s.c_str());
	t = time;
	return stream;
}