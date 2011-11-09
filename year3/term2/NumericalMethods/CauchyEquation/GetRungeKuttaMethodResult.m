function y = GetRungeKuttaMethodResult(func, exactFunc, x0, u0, h, N)

u = u0;
x = x0;

% main loop
for i=1:N + 1
	
	k1 = func(x, u);
	k2 = func(x + h/2, u + h*k1/2);
	k3 = func(x + h/2, u + h*k2/2);
	k4 = func(x + h, u + h*k3);
	
	u = u + h*(k1 + 2*k2 + 2*k3 + k4) / 6
	
	x
	exactValue = exactFunc(x)
	x = x + h;
	disp('------------')
end

y = u;

end