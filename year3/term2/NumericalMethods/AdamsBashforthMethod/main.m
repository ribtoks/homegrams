function main

a = 1;
b = 2;
N = 10;
x0 = 1;
u0 = -2;
h = (b - a)/N;

format('long', 'g');

%yEuler = GetEulerMethodResult(@f_func, @u_exact, x0, u0, a, b, N);
%yEuler'

%yRungeKutta = GetRungeKuttaMethodResult(@f_func, @u_exact, x0, u0, a, b, N);

%yRungeKutta'

u1(1) = u_exact(x0 + h);
u2(1) = u_exact(x0 + 2*h);
u3(1) = u_exact(x0 + 3*h);

u1(2) = GetEulerMethodResult(@f_func, @u_exact, x0, u0, x0, x0 + h, N);
u2(2) = GetEulerMethodResult(@f_func, @u_exact, x0, u0, x0, x0 + 2*h, N);
u3(2) = GetEulerMethodResult(@f_func, @u_exact, x0, u0, x0, x0 + 3*h, N);

u1(3) = GetRungeKuttaMethodResult(@f_func, @u_exact, x0, u0, x0, x0 + h, N);
u2(3) = GetRungeKuttaMethodResult(@f_func, @u_exact, x0, u0, x0, x0 + 2*h, N);
u3(3) = GetRungeKuttaMethodResult(@f_func, @u_exact, x0, u0, x0, x0 + 3*h, N);

yAdamsExact = SolveAdamsBashforth(@f_func, x0, N, a, b, u0, u1(1), u2(1), u3(1))';
yAdamsEuler = SolveAdamsBashforth(@f_func, x0, N, a, b, u0, u1(2), u2(2), u3(2))';
yAdamsRunge = SolveAdamsBashforth(@f_func, x0, N, a, b, u0, u1(3), u2(3), u3(3))';

deltaExact = abs(yAdamsExact(length(yAdamsExact)) - u_exact(2))
deltaEuler = abs(yAdamsEuler(length(yAdamsEuler)) - u_exact(2))
deltaRunge = abs(yAdamsRunge(length(yAdamsRunge)) - u_exact(2))

[yAdamsExact yAdamsEuler, yAdamsRunge]

u_exact(b)

end
