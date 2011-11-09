function AllCalculations

x = 0:0.01:2*pi;
y = GetUsualVector(x, @Func);

%first generate all nodes

x2 = GetNodes(2)
y2Interpolated = GetInterpolatedVector(x2, 2);

x4 = GetNodes(4)
y4Interpolated = GetInterpolatedVector(x4, 4);

x8 = GetNodes(8)
y8Interpolated = GetInterpolatedVector(x8, 8);

subplot(1, 3, 1);
plot(x, y, x2, y2Interpolated);

subplot(1, 3, 2);
plot(x, y, x4, y4Interpolated);

subplot(1, 3, 3);
plot(x, y, x8, y8Interpolated);