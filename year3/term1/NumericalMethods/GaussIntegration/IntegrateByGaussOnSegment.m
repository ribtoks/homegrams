function y = IntegrateByGaussOnSegment(func, a, b, weights, points, n)

h = (b - a) / (n - 1);
x = a:h:b;

integralSum = 0;
for i=1:length(x) - 1
	integralSum = integralSum + IntegrateByGauss(func, x(i), x(i + 1), weights, points);
end

y = integralSum;
end