function y = InterpolateByLagrange(func, interpPoints, point)
%this function finds approximation of @func() in point "point"
%interpPoints - array of points (N)
sum = 0;
resultSum(1) = 0;
	for i=1:length(interpPoints)
		resultSum(i) = func(interpPoints(i)) * LagrangeBasis(interpPoints, point, i);
        sum = sum + resultSum(i);
    end
y = sum;
end