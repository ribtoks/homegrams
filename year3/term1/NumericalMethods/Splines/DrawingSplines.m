function DrawingSplines

a = input('Enter left bound of interval >: ')
b = input('Enter right bound of interval >: ')

if a >= b
	errRecord = MException('Wrong interval!');
	throw(errRecord);
end

n = input('Enter number of points for generation >: ')
h = (b - a) / n;
x = a:h:b;

yB1(1) = 0;
yB3(1) = 0;

for i=1:length(x)
	yB1(i) = B1(x(i));
	yB3(i) = B3(x(i));
end
	
plot(x, yB1, x, yB3)