function y = LagrangeBasis(points, x, xIndex)
%returns lagrange basis in point x
mul = 1;
	for i=1:length(points)
		if i ~= xIndex
			mul = mul*((x - points(i)) / (points(xIndex) - points(i)));
        		end
    end
y = mul;
end