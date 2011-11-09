function y = IntegrateRectangularsDouble(func, a, b, func1, func2, n)

h = (b - a) / n;

integralSum = 0;
for i=0:n-1
	x = a + i*h + (h/2);
	integralSum = integralSum + IntegrateRectangularsDoubleInPoint(func, x, func1(x), func2(x), n);
end
y = integralSum*h;
end

function f = IntegrateRectangularsDoubleInPoint(func, xPoint, fA, fB, n)

h = (fB - fA) / n;

integralSum = 0;
for i=0:n-1
	y1 = fA + i*h + (h/2);
	integralSum = integralSum + func(xPoint, y1);
end

f = integralSum*h;

end