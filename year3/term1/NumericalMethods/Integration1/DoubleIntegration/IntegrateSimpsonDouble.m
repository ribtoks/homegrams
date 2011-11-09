function y = IntegrateSimpsonDouble(func, a, b, func1, func2, n)

h = (b - a) / n;

integralSum = 0;
for i=1:n-1
	x = a + i*h;
	integralSum = integralSum +IntegrateSimpsonDoubleInPoint(func, x, func1(x), func2(x), n) * ( 2 + 2*(mod(i, 2)) );
end
integralSum = integralSum + (IntegrateSimpsonDoubleInPoint(func, a, func1(a), func2(a), n) + IntegrateSimpsonDoubleInPoint(func, b, func1(b), func2(b), n));
y = integralSum*h/3;
end

function f = IntegrateSimpsonDoubleInPoint(func, xPoint, fA, fB, n)

h1 = (fB - fA) / n;

integralSum = 0;
for j=1:n-1
	y1 = fA + j*h1;
	integralSum = integralSum + func(xPoint, y1) * ( 2 + 2*(mod(j, 2)) );
end

integralSum = integralSum + (func(xPoint, fA) + func(xPoint, fB));
f = integralSum*h1/3;

end