function vector = GetChebyshPoints(n)
resultVector(1) = 0;
	for i=1:n
		resultVector(i) = cos((2*i - 1)*pi / (2 * n));
    end
vector = resultVector;
end