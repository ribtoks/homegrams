function y = IntegrateTrapeze(func, a, b, n)

h = (b - a) / n;

integralSum = 0;
for i=1:n-1
	x = a + i*h;
	integralSum = integralSum + func(x);
end
integralSum = integralSum + (func(a) + func(b))/2;
y = integralSum*h;
end