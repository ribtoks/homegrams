function y = CalculatePolynom(x, a, n)

% calculates approximation polynom using coeficients 'a'
% power ''n' and point 'x'

if n > length(a)
	n = length(a);
end

if n < 1
    n = 1;
end

resultSum = 0;

% just a simple sum

for i=1:n
	resultSum = resultSum + (a(i) * (x^(i - 1)));
end

y = resultSum;

end