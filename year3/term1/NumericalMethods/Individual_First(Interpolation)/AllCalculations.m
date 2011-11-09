function AllCalculations

a = input('Enter left bound of interval >: ')
b = input('Enter right bound of interval >: ')

if a >= b
	errRecord = MException('Wrong interval!');
	throw(errRecord);
end

n = input('Enter number of points for generation>: ')
h = (b - a) / n;
x = a:h:b;

pointsNumber = input('Enter number of points for interpolation (both Equidistant and Chebysh) >: ');

equidistantPoints = GetEquidistantPoints(a, b, pointsNumber);
chebyshPoints = GetChebyshPoints(pointsNumber);

usualX = a:0.001:b;

	for i = 1:length(usualX)
		usualY(i) = RungeFunction(usualX(i));
	end

	for i = 1:length(x)
		yEquidistant(i) = InterpolateByLagrange(@RungeFunction, equidistantPoints, x(i));
		yChebysh(i) =  InterpolateByLagrange(@RungeFunction, chebyshPoints, x(i));
    end

yourpoint = input('Enter your point >: ')
yRunge = RungeFunction(yourpoint)
yEq = InterpolateByLagrange(@RungeFunction, equidistantPoints, yourpoint)
yCheb = InterpolateByLagrange(@RungeFunction, chebyshPoints, yourpoint)

    
subplot(1, 2, 1);
plot(usualX, usualY, 'b');
hold on
plot(x, yEquidistant, 'g');
subplot(1, 2, 2);
plot(usualX, usualY,'b');
hold on
plot(x, yChebysh, 'g');