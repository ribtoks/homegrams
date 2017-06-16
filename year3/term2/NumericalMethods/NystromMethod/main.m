function main()

  points3(1) = -0.774597;
  points3(2) = 0;
  points3(3) = 0.774597;
  
  weights3(1) = 5/9;
  weights3(2) = 8/9;
  weights3(3) = 5/9;


  f_func = @(x) exp(-x) - 0.5 + 0.5*exp(-x-1);
  phi_exact = @(x) exp(-x);
  K_func = @(x,y) (x + 1)*exp(-x*y)/2;

  n = 10;
  a = 0;
  b = 1;

  h = (b - a) / (n - 1);

%  points(1) = a;
%  a_coefs(1) = h/2;
  
%  for i=2:n-1
%    a_coefs(i) = h;
%    points(i) = points(i - 1) + h;
%  end

%  points(n) = points(n - 1) + h;
%  a_coefs(n) = h/2;

 bPa = (b + a)/2;
 bMa = (b - a)/2;

 currIndex = 1;
 
 for i = 1 : n - 1

   x_a = a + (i - 1)*h;
   x_b = x_a + h;

   bPa = (x_b + x_a)/2;
   bMa = (x_b - x_a)/2;

   
   a_coefs(currIndex) = weights3(1)*bMa;
   a_coefs(currIndex + 1) = weights3(2)*bMa;
   a_coefs(currIndex + 2) = weights3(3)*bMa;

   points(currIndex) = bMa * points3(1) + bPa;
   points(currIndex + 1) = bMa * points3(2) + bPa;
   points(currIndex + 2) = bMa * points3(3) + bPa;

   currIndex = currIndex + 3;

 end

 

  for i = 1:length(points)
    exact(i) = phi_exact(points(i));
  end
  
  vect = SolveNystrom(K_func, f_func, a, b, n, a_coefs, points);

  for i=1:length(vect)
    delta(i) = abs(vect(i) - exact(i));
  end
  
  [vect, exact', delta']
  
end
