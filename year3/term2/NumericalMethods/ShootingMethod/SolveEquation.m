function y = SolveEquation(x0, pFunc, qFunc, fFunc, a, b, N, alpha, beta, s0, epsilon)

  s = s0
  
  func1 = @(x, y1, y2) y2;
  func2 = @(x, y1, y2) fFunc(x) - pFunc(x)*y2 + qFunc(x)*y1;

  v_func1 = @(x, y1, y2) y2;
  v_func2 = @(x, y1, y2) qFunc(x)*y1 - pFunc(x)*y2;


  % solve first system

  y1_0 = alpha;
  y2_0 = s;

  u = SolveSystemRungeKutta(func1, func2, y1_0, y2_0, a, b, N);

  y(1) = u(1);
  i = 2;

  while (abs(u(1) - beta) > epsilon)

    % solve second system
    y1_0 = 0;
    y2_0 = 1;

    v = SolveSystemRungeKutta(v_func1, v_func2, y1_0, y2_0, a, b, N);

    s = s - (u(1) - beta)/v(1)


    % solve first system again

    y1_0 = alpha;
    y2_0 = s;

    u = SolveSystemRungeKutta(func1, func2, y1_0, y2_0, a, b, N);

    y(i) = u(1);
    i = i + 1;
    
  end

  % now we know s

  func1 = @(x, y1, y2) y2;
  func2 = @(x, y1, y2) fFunc(x) - pFunc(x)*y2 + qFunc(x)*y1;

  u = SolveSystemRungeKutta(func1, func2, alpha, s, a, x0, N);
  y(i) = u(1);

end
