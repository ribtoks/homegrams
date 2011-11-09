function y = B3(x)
	absx = abs(x);
	yTemp = 0;
	
	if (absx <= 1)
		yTemp = ((2 - absx)^3) - 4*((1 - absx)^3);
	end
	
	if  ((absx >= 1) && (absx <= 2))
		yTemp = (2 - absx)^3;
	end
	
	y = yTemp / 6;
end