function y = CalculatePolynomOfVector(GeneralXValues, Points, Values, Power)

a = calculateAcoefs(Points, Values, Power);

for i=1:length(GeneralXValues)
	vector(i) = CalculatePolynom(GeneralXValues(i), a, Power);
end

y = vector;

end