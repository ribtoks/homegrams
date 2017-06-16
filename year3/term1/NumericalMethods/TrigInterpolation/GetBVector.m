function y = GetBVector(n, func)
resultVector(1) = 0;
ySum = 0;
	for k=1:n - 1
		ySum = 0;
		for  j=0:2*n - 1
			ySum = ySum +  func(pi*j / n)*sin(pi*k* j / n);
		end
		resultVector(k) = ySum / n;
	end
y = resultVector;
end