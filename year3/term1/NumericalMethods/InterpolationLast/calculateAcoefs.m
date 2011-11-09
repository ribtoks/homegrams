function y = calculateAcoefs(Points, Values, count)

A = zeros(count);

% generate our matrix

for i=1:count
    for j=1:count
        B(i, j) = 0;        
        for k=1:length(Points)
            B(i, j) = B(i, j) + (Points(k) ^ (i + j - 2));
        end
	end	
end

% generate our vector 'b'

b(1) = 0;
for i = 1:count
    tempSum = 0;
    for j=1:length(Points)
        tempSum = tempSum + ((Points(j)^(i - 1))*Values(j));
    end
    
    b(i) = tempSum;
end

% solve system (A*y == b) using matlab hack

y = B \ b';

end