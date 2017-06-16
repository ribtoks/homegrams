function y = ChordAndTangentsSolution(func, funcDerivative, funcDerivative2, a, b, epsilon)

	fFa = funcDerivative(a)*funcDerivative2(a);
	fFb = funcDerivative(b)*funcDerivative2(b);
	
	xLowerCurr = a;
	xUpperCurr = b;
	
if (fFa < 0 || fFb < 0)
	while (abs(xUpperCurr - xLowerCurr) >= epsilon)
		xLowerNext = greatLowerFuncChord(xLowerCurr, func, funcDerivative);
		xUpperNext = greatUpperFuncChord(xLowerCurr, xUpperCurr, func);
		
		xLowerCurr = xLowerNext
		xUpperCurr = xUpperNext
		
		disp('------')
		
	endwhile
	
else
	
	while (abs(xUpperCurr - xLowerCurr) >= epsilon)
		xLowerNext = greatLowerFuncTangent(xLowerCurr, xUpperCurr, func);
		xUpperNext = greatUpperFuncTangent(xLowerCurr, func, funcDerivative);
		
		xLowerCurr = xLowerNext
		xUpperCurr = xUpperNext
	
		disp('------')
	endwhile
	
endif
	
	y = (xLowerCurr + xUpperCurr) / 2;
end

function z = greatLowerFuncChord(xLowerPrev, func, funcDerivative)
	z = xLowerPrev - func(xLowerPrev) / funcDerivative(xLowerPrev);
end

function k = greatUpperFuncChord(xLowerPrev, xUpperPrev, func)
	k = xUpperPrev - func(xUpperPrev)*(xUpperPrev - xLowerPrev) / (func(xUpperPrev) - func(xLowerPrev));
end



function w = greatLowerFuncTangent(xLowerPrev, xUpperPrev, func)
	w = xLowerPrev - func(xLowerPrev)*(xUpperPrev - xLowerPrev) / (func(xUpperPrev) - func(xLowerPrev));	
end

function v = greatUpperFuncTangent(xUpperPrev, func, funcDerivative)
	v = xUpperPrev - func(xUpperPrev) / funcDerivative(xUpperPrev);
end