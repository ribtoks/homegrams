function main

  format('long', 'g');

  a = 0.5;
  b = 1;

  N = 1000;

  pFunc = @(x) 1/x;
  qFunc = @(x) 1/(x*x);
  fFunc = @(x) -2/(x^3);

  exactFunc = @(x) log(x)/x;

%  pFunc = @(x) 1/x;
%  qFunc = @(x) 0;
%  fFunc = @(x) 1/x;

%  exactFunc = @(x) x - 2.5*log(x)/log(2);

%  pFunc = @(x) 2*x;
%  qFunc = @(x) 1;
%  fFunc = @(x) 2*(x*x + 1)*cos(x);

%  exactFunc = @(x) x*sin(x);

  alpha =-2*log(2);
  beta = 0;%0.5*sin(0.5);

  s0 = 20;

  x = (b + a)/2;

  epsilon = 0.00001;

  u = SolveEquation(x, pFunc, qFunc, fFunc, a, b, N, alpha, beta, s0, epsilon);

  disp("----------------------");

  u'

  exactValue = exactFunc(x)
  
end
