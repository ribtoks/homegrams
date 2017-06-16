function Func = s1(x, y, a, h)

	tempSum = 0;
	xk = a;
	
	for i=1:length(y)
		tempSum = tempSum + y(i) * B1( (x - xk)/h );
		xk = xk + h;
	end
	Func = tempSum;
end