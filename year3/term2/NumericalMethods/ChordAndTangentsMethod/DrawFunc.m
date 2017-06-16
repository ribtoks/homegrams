function DrawFunc(func, a, b)

x = a:0.1:b;
for i=1:length(x)
	y(i) = func(x(i));
end

plot(x,y)

end