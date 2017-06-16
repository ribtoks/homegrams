function y=InterpolateWithTrigPolynom(x, n)

a = GetAVector(n, @Func);
b = GetBVector(n, @Func);

ySum = a(1)/2;

for i=2:length(a)
	ySum = ySum + a(i)*cos((i - 1)*x);
end

for i=1:length(b)
	ySum = ySum + b(i)*sin(i*x);
end

y = ySum;
end