function ProcessSplines

a = input('Enter left bound of interval >: ')
b = input('Enter right bound of interval >: ')

if a >= b
	errRecord = MException('Wrong interval!');
	throw(errRecord);
end

n = input('Enter number of points for generation >: ')
h = (b - a) / n;
x = a:h:b;

y(1) = 0;

ySpecial(1) = 0;
xSpecial = a:0.001:b;

	for i=1:length(xSpecial)
		ySpecial(i) = OwnFunction(xSpecial(i));
	end
	
	for i=1:length(x)
		y(i) = OwnFunction(x(i));
	end
	
linearSpline(1) = 0;

for i=1:length(x)
	linearSpline(i) = s1(x(i), y, a, h);
end

alpha = PreprocessAlphaCoefs(y, OwnFunctionDerivative(a), OwnFunctionDerivative(b), h);

cubicSpline(1) = 0;

xForCubic = a:0.01:b;
for i=1:length(xForCubic)
	cubicSpline(i) = s3(xForCubic(i), a, h, alpha);
end

subplot(1, 2, 1);
plot(xSpecial, ySpecial, 'b', x, linearSpline, 'r')

subplot(1, 2, 2);
plot(xSpecial, ySpecial, 'b', xForCubic, cubicSpline, 'r')