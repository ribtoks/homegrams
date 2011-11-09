function y = SolveSystemEuler(func1, func2, u1_0, u2_0, h, x0, N)

	x = x0;
	u1Last = u1_0;
	u2Last = u2_0;
	
	% main loop
	for i=1:N
		u1Curr = u1Last + h * func1(u1Last, u2Last);
		u2Curr = u2Last + h * func2(u1Last, u2Last);
	
		% exactValue = exactFunc(x)
		% x = x + h;
		
		u1Last = u1Curr
		u2Last = u2Curr
		
		disp('-------')
	end
	
	y(0) = u1Curr;
	y(1) = u2Curr;

end