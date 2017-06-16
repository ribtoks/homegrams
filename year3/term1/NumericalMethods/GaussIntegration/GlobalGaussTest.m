function GlobalGaussTest

% -------------------------------------------

points3(1) = -0.774597;
points3(2) = 0;
points3(3) = 0.774597;

weights3(1) = 5/9;
weights3(2) = 8/9;
weights3(3) = 5/9;

% -------------------------------------------

points4(1) = -0.861136;
points4(2) = -0.339981;
points4(3) = 0.339981;
points4(4) = 0.861136;

weights4(1) = 0.347855;
weights4(2) = 0.652145;
weights4(3) = 0.652145;
weights4(4) = 0.347855;

% ------------------------------------------

points5(1) = -0.906180;
points5(2) = -0.538469;
points5(3) = 0;
points5(4) = 0.538469;
points5(5) = 0.906180;

weights5(1) = 0.236927;
weights5(2) = 0.478629;
weights5(3) = 0.568889;
weights5(4) = 0.478629;
weights5(5) = 0.236927;

% -----------------------------------------

n = input("Enter number of points >: ")
if (n < 3 || n > 5)
	error("Wrong n!")
end

a = input("Enter left bound of interval >: ")
b = input("Enter right bound of interval >: ")

epsilon = input("Enter epsilon >: ")

if (n == 3)
	integral = TestGauss(@UserFunction, a, b, epsilon, weights3, points3);
end

if (n == 4)
	integral = TestGauss(@UserFunction, a, b, epsilon, weights4, points4);
end

if (n == 5)
	integral = TestGauss(@UserFunction, a, b, epsilon, weights5, points5);
end

exactIntegral = UserFunctionExact(b) - UserFunctionExact(a);

format('long', 'g');
%if (abs(integral - exactIntegral) < epsilon)
%	disp("All is ok")
	disp(integral)
	disp(exactIntegral)
%end

end