function y = UserFunction2Exact(x)

	c = log(sqrt(2) + 1) / pi;
	
	if (tan(pi*x/2) > 0)
		y = 2*atan(exp(-pi*x*x/2 + c*pi))/pi;
	else
		y = -2*atan(exp(-pi*x*x/2 + c*pi))/pi;
end