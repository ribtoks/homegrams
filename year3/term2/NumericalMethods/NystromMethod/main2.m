function main()

  f_func = @(x) exp(x);
  K_func = @(x,y) x*exp(x*y);

  n = 3;
  
  a = 0;
  b = 1;

  format('long', 'g');

  a_coefs(1) = 1.0/6.0;
  a_coefs(2) = 4.0/6.0;
  a_coefs(3) = 1.0/6.0;

  points(1) = a;
  points(2) = (a+b)/2.0;
  points(3) = b;
  
  vect = SolveNystrom(K_func, f_func, a, b, n, a_coefs, points)
  
end
