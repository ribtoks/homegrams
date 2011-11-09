function y = SolveSystemRungeKutta(func1, func2, u1_0, u2_0, a, b, N)
  u1Last = u1_0;
  u2Last = u2_0;

  h = (b - a)/N;
  x = a;

  for i=1:N

    k1 = func1(x, u1Last, u2Last);
    k2 = func1(x, u1Last + h*k1/2, u2Last + h*k1/2);
    k3 = func1(x, u1Last + h*k2/2, u2Last + h*k2/2);
    k4 = func1(x, u1Last + h*k3, u2Last + h*k3);
    
    u1Curr = u1Last + h*(k1 + 2*k2 + 2*k3 + k4) / 6;
    
    
    k1 = func2(x, u1Last, u2Last);
    k2 = func2(x, u1Last + h*k1/2, u2Last + h*k1/2);
    k3 = func2(x, u1Last + h*k2/2, u2Last + h*k2/2);
    k4 = func2(x, u1Last + h*k3, u2Last + h*k3);
    
    u2Curr = u2Last + h*(k1 + 2*k2 + 2*k3 + k4) / 6;
    

    u1Last = u1Curr;
    u2Last = u2Curr;

    x = x + h;
  end

    y(1) = u1Last;
    y(2) = u2Last;
end
