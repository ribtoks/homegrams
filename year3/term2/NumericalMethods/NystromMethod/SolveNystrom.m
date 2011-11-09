function y = SolveNystrom(K, f, a, b, n, a_coefs, points)

  N = length(points);

  matr = zeros(N, N);
  h = (b - a)/ (n - 1);

  
  for i = 1:N
    for k = 1:N
      matr(i, k) = -a_coefs(k)*K(points(i), points(k));
    end

    matr(i, i) = matr(i, i) + 1;
    func(i) = f(points(i));
  end

  y = inv(matr)*func';
  
end
