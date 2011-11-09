function y = SolveGeneralProblem(p_func, q_func, f_func, a, b, N, alpha0, beta0, gamma0,  alpha1, beta1, gamma1, driveIndex)
  h = (b - a)/(N + 1);
  % x should be [0..N + 1]
  % and in our case would be
  % in [1..N+2]
  x = a:h:b;
  h2 = h*h;

  kapa1 = -beta0/(h*alpha0 - beta0);
  m1 = gamma0*h/(h*alpha0 - beta0);

  kapa2 = beta1/(h*alpha1 + beta1);
  m2 = gamma1*h/(h*alpha1 + beta1);

  for i=2:N + 1
    xk = x(i);
    a_coefs(i) = -0.5*(1 + h*p_func(xk)/2);
    b_coefs(i) = 1 + h2*q_func(xk)/2;
    c_coefs(i) = -0.5*(1 - h*p_func(xk)/2);
  end

  a_coefs(1) = 0;
  b_coefs(1) = 1;
  c_coefs(1) = -kapa1;

  a_coefs(N + 2) = -kapa2;
  b_coefs(N + 2) = 1;
  c_coefs(N + 2) = 0;

  for i=2:N + 1
    f_coefs(i) = h2*f_func( x(i) )/2;
  end

  f_coefs(1) = m1;
  f_coefs(N + 2) = m2;

%  f_coefs(1) = f_coefs(1)*h2/2;
%  f_coefs(N + 2) = f_coefs(N + 2)*h2/2;

  alpha_coefs = CalculateAlpha(a_coefs, b_coefs, c_coefs, kapa1);
  beta_coefs = CalculateBeta(a_coefs, b_coefs, alpha_coefs, f_coefs, m1);

  ksi_coefs = CalculateKsi(a_coefs, b_coefs, c_coefs, kapa2, N + 2);
  eta_coefs = CalculateEta(b_coefs, c_coefs, f_coefs, ksi_coefs, m2, N + 2);


  u = zeros(1, N + 2);
  
  if (driveIndex == 1)
    u(1) = (f_coefs(1)*alpha_coefs(2) + c_coefs(2)*beta_coefs(2))/(c_coefs(2) +
			b_coefs(1)*alpha_coefs(2));
  end
    
  if (driveIndex == N + 2)
    u(N + 2) = (f_coefs(N + 2) - beta_coefs(N + 2)*a_coefs(N + 2))/(b_coefs(N + 2) + 
		a_coefs(N + 2)*alpha_coefs(N + 2));
  end

  if ((driveIndex > 1) && (driveIndex < N + 2))
    i = driveIndex;
    u(i) = (beta_coefs(i + 1) + eta_coefs(i)*alpha_coefs(i +
		1))/(1 - alpha_coefs(i + 1)*ksi_coefs(i));
  end


  i = driveIndex - 1;
  while (i >= 1)
    u(i) = u(i + 1)*alpha_coefs(i + 1) + beta_coefs(i + 1);
    i = i - 1;
  end

  for i=driveIndex:N + 1
    u(i + 1) = ksi_coefs(i)*u(i) + eta_coefs(i);
  end

  y = u;
  
end


function alpha = CalculateAlpha(a_coefs, b_coefs, c_coefs, kapa1)
  alpha(1) = kapa1;

  index = length(a_coefs) - 1;
  for i = 1:index
    alpha(i + 1) = -c_coefs(i)/(b_coefs(i) + alpha(i)*a_coefs(i));
  end

end

function beta = CalculateBeta(a_coefs, b_coefs, alpha_coefs, f_coefs, m1)
  beta(1) = m1;

  index = length(a_coefs) - 1;

  for i = 1:index
    beta(i + 1) = (f_coefs(i) - beta(i)*a_coefs(i)) / (b_coefs(i) + 
			alpha_coefs(i)*a_coefs(i));
  end
end

function ksi = CalculateKsi(a_coefs, b_coefs, c_coefs, kapa2, N)
  ksi(N) = kapa2;

  for i = N - 1:(-1):1
    ksi(i) = -a_coefs(i + 1)/(b_coefs(i + 1) + ksi(i + 1)*c_coefs(i + 1));
  end
end

function eta = CalculateEta(b_coefs, c_coefs, f_coefs, ksi_coefs, m2, N)
  eta(N) = m2;

  for i = N - 1:(-1):1
    eta(i) = (f_coefs(i + 1) - c_coefs(i + 1)*eta(i + 1)) / (b_coefs(i + 1) + 
		ksi_coefs(i + 1)*c_coefs(i + 1));
  end
end
