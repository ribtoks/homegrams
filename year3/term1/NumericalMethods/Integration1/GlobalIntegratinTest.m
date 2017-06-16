function GlobalIntegratinTest

a = input('Enter left bound of interval >: ')
b = input('Enter right bound of interval >: ')

epsilon = input('Enter epsilon >: ')

integral = TestIntegration(@UserFunction, @IntegrateSimpson, a, b, epsilon);
exactIntegral = UserFunctionExact(b) - UserFunctionExact(a);

if (abs(integral - exactIntegral) < epsilon)
	disp("All is ok")
	disp(integral)
	disp(exactIntegral)
end

end