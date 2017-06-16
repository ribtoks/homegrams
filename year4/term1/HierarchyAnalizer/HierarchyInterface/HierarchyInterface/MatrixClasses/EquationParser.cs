using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HierarchyInterface.MatrixClasses
{
    public static class EquationParser
    {
        //парсання стрічки з рівнянням на основі скінченного автомата. Повертає вектор коефіцієнтів
        public static double[] Parse(string equation, int max_power_of_x, out double b_i)
        {
            char[] Equation = equation.ToCharArray();
            //коефіцієнт вектора
            double coef;
            double[] result = new double[max_power_of_x];
            double b_out = 0;
            //спочатку занулюємо всі коефіцієнти
            for (int i = 0; i < max_power_of_x; ++i)
                result[i] = 0;

            int state = 0, curr = 0;
            int index = 0; //індекс 'x'
            char ch; //черговий символ
            StringBuilder temp_coef = new StringBuilder();
            StringBuilder temp_index = new StringBuilder();

            while (curr != Equation.Length)
            {
                ch = Equation[curr];
                switch (state)
                {
                    case 0:
                        switch (ch)
                        {
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                            case ',':
                            case '-':
                                temp_coef.Append(ch);
                                state = 0;
                                break;
                            case '*':
                                state = 1;
                                break;
                            case 'x':
                                state = 2;
                                break;

                            default:
                                throw new Exception("Wrong symbol at position " + curr + "!");
                        }
                        break;

                    case 1:
                        switch (ch)
                        {
                            case 'x':
                                state = 2;
                                break;
                            default:
                                throw new Exception("Wrong symbol at position " + curr + "!");
                        }
                        break;

                    case 2:
                        switch (ch)
                        {
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                            case ',':
                                temp_index.Append(ch);
                                state = 2;
                                break;

                            case '+':
                            case '-':
                                {
                                    index = Int32.Parse(temp_index.ToString());

                                    if (temp_coef.ToString() == "" | temp_coef.ToString() == "-")
                                        temp_coef.Append('1');
                                    coef = Double.Parse(temp_coef.ToString());
                                    result[index - 1] = coef;

                                    //тепер занулення використаних елементів
                                    index = 0;
                                    temp_coef.Remove(0, temp_coef.ToString().Length);
                                    temp_index.Remove(0, temp_index.ToString().Length);

                                    //тепер щойно використання case '-':
                                    if (ch == '-')
                                        temp_coef.Append(ch);
                                    state = 3;
                                }
                                break;

                            case '=':
                                index = Int32.Parse(temp_index.ToString());
                                if (temp_coef.ToString() == "" | temp_coef.ToString() == "-")
                                    temp_coef.Append('1');
                                coef = Double.Parse(temp_coef.ToString());
                                result[index - 1] = coef;

                                StringBuilder last = new StringBuilder();
                                for (int i = curr + 1; i < Equation.Length; ++i)
                                    last.Append(Equation[i]);
                                b_out = Double.Parse(last.ToString());
                                curr = Equation.Length - 1;
                                break;

                            default:
                                throw new Exception("Wrong symbol at position " + curr + "!");
                        }
                        break;

                    case 3:
                        switch (ch)
                        {
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                            case ',':
                                temp_coef.Append(ch);
                                state = 3;
                                break;

                            case '*':
                                state = 1;
                                break;

                            case 'x':
                                state = 2;
                                break;

                            default:
                                throw new Exception("Wrong symbol at position " + curr + "!");
                        }
                        break;
                } //Main switch end;
                ++curr;
            } //while end
            b_i = b_out;
            return result;
        }
    }
}
