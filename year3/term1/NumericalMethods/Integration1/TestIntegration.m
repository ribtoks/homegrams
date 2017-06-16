function y = TestIntegration(func, integrationFunc, a, b, epsilon)


n = 2;

I1 = integrationFunc(func, a, b, n);
I2 = integrationFunc(func, a, b, 2*n);

while (  abs(I1 - I2) > epsilon )	
	n = n*2;
	I1 = integrationFunc(func, a, b, n);
	I2 = integrationFunc(func, a, b, 2*n);
end

y = I2;
n

end