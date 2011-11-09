function Main

epsilon = 0.0000001;

format('long', 'g');

a = input("Enter left bound of interval >: ")
b = input("Enter right bound of interval >: ")

func = @Function2;
funcDerivative = @Function2Derivative;
funcDerivative2 = @Function2Derivative2;

solution = ChordAndTangentsSolution(func, funcDerivative, funcDerivative2, a, b, epsilon)
func(solution)

end