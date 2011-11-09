function y = GetEulerMethodResult(func, exactFunc, x0, u0, h, N)

u = u0;
x = x0;

% main loop
for i=1:N + 1
	
	u = u + h*func(x, u)
	
	x
	exactValue = exactFunc(x)
	x = x + h;
	disp('------------')
end

y = u;

end