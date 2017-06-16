function AllPolynomCalculations

unknownYear = 0;

x = 1900:10:1970;

y(1) = 75994575;
y(2) = 91972266;
y(3) = 105710620;
y(4) = 122755046;
y(5) = 131669275;
y(6) = 150697361;
y(7) = 179323175;
y(8) = 203235297;


%1980
%226547298

point = 1980;

unknownYear = InterpolateByLagrange(y, x, point)