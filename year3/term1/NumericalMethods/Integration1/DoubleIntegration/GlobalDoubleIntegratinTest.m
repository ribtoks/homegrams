function GlobalDoubleIntegratinTest

a = input('Enter left bound of interval >: ')
b = input('Enter right bound of interval >: ')

epsilon = input('Enter epsilon >: ')

integral = TestDoubleIntegration(@UserTwoVarFunction, @IntegrateSimpsonDouble, a, b, @UserFunc1, @UserFunc2, epsilon);
%exactIntegral = UserFunctionExact(b) - UserFunctionExact(a);

%if (abs(integral - exactIntegral) < epsilon)
	%disp("All is ok")
	disp(integral)
	%disp(exactIntegral)
%end

end