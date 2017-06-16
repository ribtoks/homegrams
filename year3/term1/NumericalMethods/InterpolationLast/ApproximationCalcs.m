function ApproximationCalcs

n = input('Enter power of polynom >: ')

if n < 1 || n > 8
	n = 8;
end

x = 1900:10:1970;

y(1) = 75994575;
y(2) = 91972266;
y(3) = 105710620;
y(4) = 122755046;
y(5) = 131669275;
y(6) = 150697361;
y(7) = 179323175;
y(8) = 203235298;

a = calculateAcoefs(x, y, n);

point = 1980;

% calculate popularity using approximation

unknownPeopleCount = CalculatePolynom(point, a, n)