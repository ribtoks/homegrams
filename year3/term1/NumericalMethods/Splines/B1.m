function y = B1(x)
	absx = abs(x);
	yTemp = 0;
	if (absx < 1)
		yTemp = 1 - absx;
	end
	
	y = yTemp;	
end