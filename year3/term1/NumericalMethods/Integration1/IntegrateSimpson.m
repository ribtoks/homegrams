function y = IntegrateSimpson(func, a, b, n)

h = (b - a) / n;

integralSum = 0;
for i=1:n-1
	x = a + i*h;
	integralSum = integralSum + func(x)*( 2 + 2*(mod(i, 2)) );
end
integralSum = integralSum + (func(a) + func(b));
y = integralSum*h/3;
end