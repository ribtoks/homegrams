#pragma once
typedef unsigned int uint;
//class for DataTime
class Time
{
private:
	//usual data
	uint hour;
	uint minute;
	uint second;

	bool is24hour;

//delimiter in string time
	char delimiter;	

public:
	//constructors

	//usual constructor
	Time() 
		: hour(0), minute(0), second(0), delimiter(':'), is24hour(false)
	{
	}
	//given time in time
	Time(int Hour, int Minute, int Second) 
		: hour(Hour), minute(Minute), second(Second), delimiter(':'), is24hour(false)
	{
		if (!IsTimeCorrect())
			MakeCorrectness();
	}

	//given time in time
	Time(int Hour, int Minute, int Second, char Delimiter) 
		: hour(Hour), minute(Minute), second(Second), delimiter(Delimiter), is24hour(false)
	{
		if (!IsTimeCorrect())
			MakeCorrectness();
	}

	//given time in time
	Time(int Hour, int Minute, int Second, char Delimiter, bool Is24Hour) 
		: hour(Hour), minute(Minute), second(Second), delimiter(Delimiter), is24hour(Is24Hour)
	{
		if (!IsTimeCorrect())
			MakeCorrectness();
	}

	//given time in string
	Time(const char* TimeinString);
	Time(const Time& t);

	//operator overloading
	Time& operator=(const Time& time);

	Time& operator+=(const Time& time);

	std::string ToString() const;

	inline uint GetHour() const { return hour;};
	inline uint GetMinute() const { return minute;};
	inline uint GetSecond() const { return second;};
	inline char GetDelimiter() const { return delimiter;};

	//if the time in class has correct structure
	bool IsTimeCorrect();

	//makes Time in class correct
	void MakeCorrectness();

	inline void SetHours24(bool is24) { is24hour = is24;};
	inline bool GetHours24() const { return is24hour;};

	inline void SetDelimiter(char delim) { delimiter = delim;};
};

const Time operator+(const Time& t1, const Time&t2);
bool operator==(const Time& t1, const Time&t2);
bool operator<(const Time& t1, const Time&t2);

std::ostream& operator<<(std::ostream& stream, const Time& t);
std::istream& operator>>(std::istream& stream, Time&t);