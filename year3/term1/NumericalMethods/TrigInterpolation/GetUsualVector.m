function y = GetUsualVector(x, func)
res(1) = 0;
for i=1:length(x)
	res(i) = func(x(i));
end
y = res;
end