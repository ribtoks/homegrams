function vector = GetEquidistantPoints(a, b, n)
%returns "n" equidistant points from interval [a, b]
h = (b - a) / n;
resVect = a:h:b;
vector = resVect;
end