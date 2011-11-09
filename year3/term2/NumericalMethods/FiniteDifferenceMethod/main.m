function main()

  p_func = @(x) 1;
  q_func = @(x) 2;
  f_func = @(x) 3*exp(x);

  a = 0;
  b = 1;
  N = 200;

%  format long

  driveIndex = floor((N + 2)/2);

  c2 = -1.5*(1 + 3*exp(2))/(1 + 5*exp(3));
  c1 = 4.5*exp(2) + 5*c2*exp(3);

  u_real = @(x) 1.5*exp(x) + c1*exp(-x) + c2*exp(2*x);

  x=a:(b - a)/(N + 1):b;

  for i=1:length(x)
    y(i) = u_real( x(i) );
  end

  alpha0 = 1;
  beta0 = 0;
  gamma0 = 0;

  alpha1 = 1;
  beta1 = 2;
  gamma1 = 0;

  [y', SolveGeneralProblem(p_func, q_func, f_func, a, b, N, alpha0, beta0, gamma0, alpha1, beta1, gamma1, driveIndex)']

end
