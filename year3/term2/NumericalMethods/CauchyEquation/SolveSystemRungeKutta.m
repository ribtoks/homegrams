function y = SolveSystemRungeKutta(func1, func2, u1_0, u2_0, h, N)

				% x = x0;
  u1Last = u1_0;
  u2Last = u2_0;
  
				% main loop
  for i=1:N
    
    k1 = func1(u1Last, u2Last);
    k2 = func1(u1Last + h*k1/2, u2Last + h*k1/2);
    k3 = func1(u1Last + h*k2/2, u2Last + h*k2/2);
    k4 = func1(u1Last + h*k3, u2Last + h*k3);
    
    u1Curr = u1Last + h*(k1 + 2*k2 + 2*k3 + k4) / 6;
    
    
    k1 = func2(u1Last, u2Last);
    k2 = func2(u1Last + h*k1/2, u2Last + h*k1/2);
    k3 = func2(u1Last + h*k2/2, u2Last + h*k2/2);
    k4 = func2(u1Last + h*k3, u2Last + h*k3);
    
    u2Curr = u2Last + h*(k1 + 2*k2 + 2*k3 + k4) / 6;
    
				% exactValue = exactFunc(x)
				% x = x + h;
    
    u1Last = u1Curr
    u2Last = u2Curr
    
    disp('------------')
  end
    
    y(1) = u1Curr;
    y(2) = u2Curr;

end
