function DrawStupid

x = -1:0.1:1;
for i=1:length(x)
	y(i) = exp(x(i));
	z(i) = -x(i)/2;
end

plot (x, y, x, z)

end