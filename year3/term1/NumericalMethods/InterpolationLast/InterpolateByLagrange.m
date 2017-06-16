function y = InterpolateByLagrange(funcPoints, interpPoints, point)
%this function finds approximation of @func() in point "point"
%interpPoints - array of points (N)
resultSum = 0;
	for i=1:length(interpPoints)
		resultSum = resultSum + (funcPoints(i) * LagrangeBasis(interpPoints, point, i));
	end
y = resultSum;
end