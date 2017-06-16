function y = GetInterpolatedVector(x, n)

resultVector(1) = 0;

	for i=1:length(x)
		resultVector(i) = InterpolateWithTrigPolynom(x(i), n);
	end

y = resultVector;
end