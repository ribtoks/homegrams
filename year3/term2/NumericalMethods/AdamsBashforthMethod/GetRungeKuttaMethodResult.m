function y = GetRungeKuttaMethodResult(func, exactFunc, x0, u0, a, b, N)

u = u0;
x = x0;
h = (b -a)/N;

% main loop
for i=1:N
	
	k1 = func(x, u);
	k2 = func(x + h/2, u + h*k1/2);
	k3 = func(x + h/2, u + h*k2/2);
	k4 = func(x + h, u + h*k3);
	
	u = u + h*(k1 + 2*k2 + 2*k3 + k4) / 6;
	
	x = x + h;
end

y = u;	

end
