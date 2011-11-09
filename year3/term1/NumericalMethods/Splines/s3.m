function Func = s3(x, a, h, alpha)

	tempSum = 0;
	xk = a;
	
	for i=1:length(alpha)
		tempSum = tempSum + alpha(i) * B3( (x - xk)/h );
		xk = xk + h;
	end
	Func = tempSum;
end