function y = GetEulerMethodResult(func, exactFunc, x0, u0, a, b, N)

u = u0;
x = x0;
h = (b - a)/N;

% main loop
for i=1:N
	u = u + h*func(x, u);
	x = x + h;
end

y = u;	

end
