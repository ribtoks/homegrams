function y = GetAVector(n, func)
resultVector(1) = 0;
ySum = 0;
	for k=0:n
		ySum = 0;
		for  j=0:2*n - 1
			ySum = ySum +  func(pi*j / n)*cos(pi*k* j / n);
		end
		resultVector(k + 1) = ySum / n;
	end
y = resultVector;
end