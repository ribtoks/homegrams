function y = TestGauss(func, a, b, epsilon, weights, points)

n = 2;

I1 = IntegrateByGaussOnSegment(func, a, b, weights, points, n);
I2 = IntegrateByGaussOnSegment(func, a, b, weights, points, 2*n);

while (  abs(I1 - I2) > epsilon )	
	n = n*2;
	I1 = IntegrateByGaussOnSegment(func, a, b, weights, points, n);
	I2 = IntegrateByGaussOnSegment(func, a, b, weights, points, 2*n);
end

y = I2;
n

end