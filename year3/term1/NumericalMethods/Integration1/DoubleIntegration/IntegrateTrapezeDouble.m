function y = IntegrateTrapezeDouble(func, a, b, func1, func2, n)

h = (b - a) / n;

integralSum = 0;
for i=1:n-1
	x = a + i*h;
	integralSum = integralSum +IntegrateTrapezeDoubleInPoint(func, x, func1(x), func2(x), n);
end
integralSum = integralSum + (IntegrateTrapezeDoubleInPoint(func, a, func1(a), func2(a), a) + IntegrateTrapezeDoubleInPoint(func, b, func1(b), func2(b), n))/2;
y = integralSum*h;
end

function f = IntegrateTrapezeDoubleInPoint(func, xPoint, fA, fB, n)

h = (fB - fA) / n;

integralSum = 0;
for i=1:n-1
	y = fA + i*h;
	integralSum = integralSum + func(xPoint, y);
end

integralSum = integralSum + (func(xPoint, fA) + func(xPoint, fB))/2;
f = integralSum*h;

end