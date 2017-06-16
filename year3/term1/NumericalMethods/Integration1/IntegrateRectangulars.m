function y = IntegrateRectangulars(func, a, b, n)

h = (b - a) / n;

integralSum = 0;
for i=0:n-1
	x = a + i*h + (h/2);
	integralSum = integralSum + func(x);
end
y = integralSum*h;
end