function DrawPolynoms

x = 1900:10:1970;

y(1) = 75994575;
y(2) = 91972266;
y(3) = 105710620;
y(4) = 122755046;
y(5) = 131669275;
y(6) = 150697361;
y(7) = 179323175;
y(8) = 203235298;

greatestYear = 3000;

points = -greatestYear:10:greatestYear;

yLast = CalculatePolynomOfVector(points, x, y, 8);

yPreLast = CalculatePolynomOfVector(points, x, y, 7);

yPrePreLast = CalculatePolynomOfVector(points, x, y, 6);

plot(points, yLast, points, yPreLast, points, yPrePreLast);