function GeneralCalculations

	N  = input('Please, enter N :> ')
	a = 1;
	u0 = -2;
	
	b = input('Please, enter b :> ')
	h = (b - a) / N;
	
	format('long', 'g');
	
	func = @UserFunction1;
	funcExact = @UserFunction1Exact;
	
	%f = GetEulerMethodResult(func, funcExact, a, u0, h, N)
	
	
	%disp('----------------------------------------------------------------------------------------------------')
	
%	u0 = 0.75
	
	%f = GetRungeKuttaMethodResult(func, funcExact, a, u0, h, N)
	
	%disp('----------------------------------------------------------------------------------------------------')
	
	%f2 = SolveSystemRungeKutta(@Function1_U1U2, @Function2_U1U2, 0, 4, h, N);
	%f2(1)
	%f2(2)
	
	f1 = SolveSystemEuler(@Function1_U1U2, @Function2_U1U2, 0, 4, h, 0, N);
	
	disp('-----')
	
	x = 0.5;
	y1 = -exp(x) + exp(5*x)
	y2 = exp(x) + 3*exp(5*x)
end