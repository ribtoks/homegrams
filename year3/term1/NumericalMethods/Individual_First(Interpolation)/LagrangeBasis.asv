function L = LagrangeBasis(points, x, xIndex)
%returns lagrange basis in point x
mulResult = 1;
	for i=1:length(points)
		if i ~= xIndex
			mulResult = ((x - points(i)) / (points(xIndex) - points(i))) * mulResult;
        end
    end
L = mulResult;
end