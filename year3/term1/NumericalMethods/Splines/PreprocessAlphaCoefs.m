function alpha = PreprocessAlphaCoefs(y, a1, b1, h)

	%length(y) == n + 1

	kapa1 = -0.5;
	m1 = 1.5*y(1) + h*a1/2;
	
	kapa2 = -2;
	m2 = 3*y(length(y)) + h*b1;

	alphaCoef(1) = kapa1;
	for i=1:length(y)
		alphaCoef(i + 1) = -1/(4 + alphaCoef(i));
	end
	
	betaCoef(1) = m1;
	
	for i=1:length(y)
		betaCoef(i + 1) = ( 6*y(i) - betaCoef(i) ) / (4 + alphaCoef(i));
	end
	
	lastAlpha = ( m2 + kapa2*betaCoef(length(betaCoef)) ) / (1 - kapa2*alphaCoef(length(alphaCoef)));
	
	i = length(y);
	alpha(length(y) + 1) = lastAlpha;
	
	while (i > 0)
		alpha(i) = alpha(i + 1)*alphaCoef(i + 1) + betaCoef(i + 1);
		i = i - 1;
	end
end