function y = TestDoubleIntegration(func, integrationFunc, a, b, func1, func2, epsilon)


n = 1;

I1 = integrationFunc(func, a, b, func1, func2, n);
I2 = integrationFunc(func, a, b, func1, func2, 2*n);

while (  abs(I1 - I2) > epsilon )
	n = n*2;
	I1 = integrationFunc(func, a, b, func1, func2, n);
	I2 = integrationFunc(func, a, b, func1, func2, 2*n);
end

y = I2;
n

end