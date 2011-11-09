function y = IntegrateByGauss(func, a, b, weights, points)

bPa = (b + a)/2;
bMa = (b - a)/2;

integralSum = 0;
for i=1:length(points)
	integralSum = integralSum + weights(i)*func(bMa * points(i) + bPa);
end

y = bMa * integralSum;
end