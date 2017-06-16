function y = SolveAdamsBashforth(func, x0, N, a, b, u0, u1, u2, u3)

h = (b - a)/N;
hdiv24 = h/24;
x = x0;

u0_last = u0;
u1_last = u1;
u2_last = u2;
u3_last = u3;

y(1) = u0;
y(2) = u1;
y(3) = u2;
y(4) = u3;

j = 5;

for i = 0 : N - 4
	u =  u3_last + hdiv24*(55*func(x + 3*h, u3_last) - 59*func(x + 2*h, u2_last) + 37*func(x + h, u1_last) - 9*func(x, u0_last));
	
	u0_last = u1_last;
	u1_last = u2_last;
	u2_last = u3_last;
	u3_last = u;
	x = x + h;
	
	y(j) = u;
	j = j + 1;
end


end
