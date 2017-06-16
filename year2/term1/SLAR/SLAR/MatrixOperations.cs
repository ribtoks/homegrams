using System;
using System.Collections.Generic;
using System.Text;

namespace SLAR
{
    public static class MatrixOperations
    {        
        //відновити по протоколу вихідну матрицю
        public static Matrix RestoreTrianguled(Matrix A, TriangulingProtocol TrPt)
        {
            //робимо копію
            TriangulingProtocol tp = new TriangulingProtocol(TrPt);
           
            Matrix restored = new Matrix(A);
            int last = tp.factors.Length - 1;
            int last_swap = tp.MadeSwaps.Count - 1;
            for (int i = A.Height - 2; i >= 0; --i)
            {
                for (int k = A.Height - 1; k >= i + 1; --k)
                {
                    for (int j = A.Width - 1; j >= i + 1; --j)
                    {
                        restored[k, j] += tp.factors[last] * restored[i, j];
                    }
                    if (last >= 0)
                        restored[k, i] = tp.factors[last] * restored[i, i];
                    --last;                    
                }
                if (last_swap >= 0)
                {
                    while (i == tp.MadeSwaps[last_swap].iteration)
                    {
                        restored.SwapRows(tp.MadeSwaps[last_swap].x, tp.MadeSwaps[last_swap].y);
                        tp.MadeSwaps.RemoveAt(last_swap);
                        --last_swap;
                        if (last_swap == -1)
                            break;
                    }
                }
            }
            return restored;
        }

        //транспонування матриці
        public static Matrix GetTranspose(Matrix from)
        {
            Matrix temp = new Matrix(from.Width, from.Height);
            for (int i = 0; i < from.Height; ++i)
                for (int j = 0; j < from.Width; ++j)
                    temp[i, j] = from[j, i];
            return temp;
        }

        //повернути матрицю на основі колонок
        public static Matrix BuildFromColumns(double[][] columns)
        {
            Matrix col = new Matrix(columns, true);
            return col;
        }        

        //повертає обернену матрицю до вхідної, але, можливо, зведену до трикутної
        public static Matrix FindReservedMatrix(Matrix Source, bool ReturnTrianguled, double epsilon)
        {
            Matrix Reserved = null;
            Matrix TrSource = null;
            Matrix E = Matrix.Get_E(Source.Width);
            double[][] E_Columns = E.GetColumns(); //просто візьмемо колонки в цілях оптимізації
            //одночасне зведення колонок і матириці до трикутного вигляду
            int rang = 0;
            if (ReturnTrianguled == false)
                rang = MatrixOperations.ToTrianguled(Source, ref TrSource, ref E_Columns, true, epsilon);
            else
                rang = MatrixOperations.ToTrianguled(Source, ref TrSource, true, epsilon);

            if (rang < Source.Height - 1)
                return Reserved;          

            //масив колонок - результати обчислень
            double[][] Columns = new double[Source.Width][];

            Gauss_SLAR slar1 = new Gauss_SLAR(Source.Height, Source.Width);
            slar1.ReSetSLAR(TrSource, E_Columns[0], true);
            for (int i = 0; i < Source.Width; ++i)
            {
                //перевизначення вектора-стовпця в системі
                slar1.ChangeVectorB(E_Columns[i], true);
//                slar1.ReSetSLAR(Source, E_Columns[i], false);
                slar1.SolveSLAR();
                //забираємо результат
                Columns[i] = (double[])slar1.vectorX.Clone();
            }
            Reserved = new Matrix(Columns, true);
            return Reserved;
        }

        //повертає обернену матрицю до вхідної
        public static Matrix FindReservedMatrix(Matrix Source, double epsilon)
        {
            Matrix Reserved = null;
            Matrix TrSource = null;
            Matrix E = Matrix.Get_E(Source.Width);
            double[][] E_Columns = E.GetColumns(); //просто візьмемо колонки в цілях оптимізації
            //одночасне зведення колонок і матириці до трикутного вигляду
            int rang = MatrixOperations.ToTrianguled(Source, ref TrSource, ref E_Columns, true, epsilon);
            if (rang < Source.Height - 1)
                return Reserved;

            //масив колонок - результати обчислень
            double[][] Columns = new double[Source.Width][];

            Gauss_SLAR slar1 = new Gauss_SLAR(Source.Height, Source.Width);
            slar1.ReSetSLAR(TrSource, E_Columns[0], true);
            for (int i = 0; i < Source.Width; ++i)
            {
                //перевизначення вектора-стовпця в системі
                slar1.ChangeVectorB(E_Columns[i], true);
                //                slar1.ReSetSLAR(Source, E_Columns[i], false);
                slar1.SolveSLAR();
                //забираємо результат
                Columns[i] = (double[])slar1.vectorX.Clone();
            }
            Reserved = new Matrix(Columns, true);
            return Reserved;
        }

        //close to zero elements turn to zero
        public static Matrix Epsilomalize(Matrix A, double epsilon)
        {
            Matrix B = A.CopyOfMe();
            for (int i = 0; i < B.Height; ++i)
                for (int j = 0; j < B.Width; ++j)
                    if (Math.Abs(B[i, j]) < epsilon)
                        B[i, j] = 0;
            return B;
        }

        public static double MaxModuleElement(Matrix A, out int i, out int j)
        {
            int k = 0, m = 0;
            double max = A[k, m];
            for (int x = 0; x < A.Height; ++x)
                for (int y = 0; y < A.Width; ++y)
                    if (Math.Abs(A[x, y]) > Math.Abs(max))
                    {
                        k = x;
                        m = y;
                        max = A[x, y];
                    }
            i = k;
            j = m;
            return max;
        }

        //отримати матрицю з вилученими діагональними елементами
        public static Matrix GetV_Matrix(Matrix A)
        {
            Matrix B = new Matrix(A);
            for (int i = 0; i < B.Height; ++i)
                B[i, i] = 0;
            return B;
        }

        //отримати матрицю з вилученими діагональними елементами
        public static void GetV_Matrix(ref Matrix A)
        {            
            for (int i = 0; i < A.Height; ++i)
                A[i, i] = 0;            
        }

        #region Зведення до трикутного вигляду

        //повертає ранг матриці за звичайним Гаусом
        public static int ToTrianguled(Matrix Source, ref Matrix Target, bool ByColumns, double epsilon)
        {
            Matrix tmp = new Matrix(Source);
            int i = 0, k = 0, j = 0;
            int rank = 0;

            //власне зведення до трикутної матриці
            for (; i < tmp.Width; ++i)
            {
                if (Math.Abs(tmp[i, i]) < epsilon)
                {
                    k = i + 1;
                    if (k < tmp.Height)
                        while ((Math.Abs(tmp[k, i]) < epsilon))
                        {
                            ++k;
                            if (k >= tmp.Height)
                            {
                                rank = i - 1;
                                Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
                                return rank;
                            }
                        }
                    else
                    {
                        rank = i - 1;
                        Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
                        return rank;
                    }
                    //свап векторів
                    tmp.SwapRows(i, k);

                }//if   == 0

                if (ByColumns == true)
                {
                    //реалізація вибору в стовпці максимального елемента за модулем
                    int mod_index = i;
                    for (int ind = i + 1; ind < tmp.Height; ++ind)
                    {
                        if (Math.Abs(tmp[ind, i]) > Math.Abs(tmp[mod_index, i]))
                            mod_index = ind;
                    }

                    //якщо максимальний елемент < Epsilon, то вихід
                    if (Math.Abs(tmp[mod_index, i]) < epsilon)
                    {
                        rank = i - 1;
                        Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
                        return rank;
                    }

                    //свап цих векторів
                    if (mod_index > i)
                    {
                        tmp.SwapRows(i, mod_index);
                    }
                }//if by column Gauss

                //real calculating
                for (k = i + 1; k < tmp.Height; ++k)
                {
                    for (j = i + 1; j < tmp.Width; ++j)
                    {
                        tmp[k, j] -= tmp[i, j] * tmp[k, i] / tmp[i, i];
                    }
                    tmp[k, i] = 0;
                }
            }//main for
            if (tmp.Height > tmp.Width)
                rank = tmp.Width;
            else
                rank = tmp.Height;

            Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
            return rank;
        }

        //разом з системою векторів-стовпців - спеціально для обернених матриць
        public static int ToTrianguled(Matrix Source, ref Matrix Target, ref double[][] vectorColumns, bool ByColumns, double epsilon)
        {
            Matrix tmp = new Matrix(Source);
            int i = 0, k = 0, j = 0, m = 0;
            int rank = 0;

            //власне зведення до трикутної матриці
            for (; i < tmp.Width; ++i)
            {
                if (Math.Abs(tmp[i, i]) < epsilon)
                {
                    k = i + 1;
                    if (k < tmp.Height)
                        while (Math.Abs(tmp[k, i]) < epsilon)
                        {
                            ++k;
                            if (k >= tmp.Height)
                            {
                                rank = i - 1;
                                Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
                                return rank;
                            }
                        }
                    else
                    {
                        rank = i - 1;
                        Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
                        return rank;
                    }
                    //свап векторів
                    tmp.SwapRows(i, k);
                    //і свап знов елементів вектора - стовбця у всіх стовбців
                    for (m = 0; m < vectorColumns.Length; ++m)
                    {
                        double temp = vectorColumns[m][i];
                        vectorColumns[m][i] = vectorColumns[m][k];
                        vectorColumns[m][k] = temp;
                    }

                }//if   == 0

                if (ByColumns == true)
                {
                    //реалізація вибору в стовпці максимального елемента за модулем
                    int mod_index = i;
                    for (int ind = i + 1; ind < tmp.Height; ++ind)
                    {
                        if (Math.Abs(tmp[ind, i]) > Math.Abs(tmp[mod_index, i]))
                            mod_index = ind;
                    }

                    //якщо максимальний елемент < Epsilon, то вихід
                    if (Math.Abs(tmp[mod_index, i]) < epsilon)
                    {
                        rank = i - 1;
                        Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
                        return rank;
                    }

                    //свап цих векторів
                    if (mod_index > i)
                    {
                        tmp.SwapRows(i, mod_index);
                        //і свап елементів вектора - стовбця у всіх стовбців
                        for (m = 0; m < vectorColumns.Length; ++m)
                        {
                            double temp = vectorColumns[m][i];
                            vectorColumns[m][i] = vectorColumns[m][mod_index];
                            vectorColumns[m][mod_index] = temp;
                        }
                    }
                }//if by column Gauss                
                //real calculating
                for (k = i + 1; k < tmp.Height; ++k)
                {
                    double factor = tmp[k, i] / tmp[i, i];
                    for (j = i + 1; j < tmp.Width; ++j)
                        tmp[k, j] -= tmp[i, j] * factor;
                    tmp[k, i] = 0;

                    for (m = 0; m < vectorColumns.Length; ++m)
                    {
                        vectorColumns[m][k] -= (vectorColumns[m][i] * factor);
                    }
                }
            }//main for
            if (tmp.Height > tmp.Width)
                rank = tmp.Width;
            else
                rank = tmp.Height;

            Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
            return rank;
        }

        //з історією
        public static TriangulingProtocol ToTrianguled(Matrix Source, ref Matrix Target, out int rank, bool ByColumns, double epsilon)
        {
            TriangulingProtocol tp = new TriangulingProtocol(Source.Width);

            Matrix tmp = new Matrix(Source);
            int i = 0, k = 0, j = 0;
            int used = 0;

            //власне зведення до трикутної матриці
            for (; i < tmp.Width; ++i)
            {
                if (Math.Abs(tmp[i, i]) < epsilon)
                {
                    k = i + 1;
                    if (k < tmp.Height)
                        while (Math.Abs(tmp[k, i]) < epsilon)
                        {
                            ++k;
                            if (k >= tmp.Height)
                            {
                                rank = i - 1;
                                Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
                                return tp;
                            }
                        }
                    else
                    {
                        rank = i - 1;
                        Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
                        return tp;
                    }
                    tmp.SwapRows(i, k);
                    tp.MadeSwaps.Add(new SwapEl(i, k, i));

                }//if   == 0

                if (ByColumns == true)
                {
                    //реалізація вибору в стовпці максимального елемента за модулем
                    int mod_index = i;
                    for (int ind = i + 1; ind < tmp.Height; ++ind)
                    {
                        if (Math.Abs(tmp[ind, i]) > Math.Abs(tmp[mod_index, i]))
                            mod_index = ind;
                    }

                    //якщо максимальний елемент < Epsilon, то вихід
                    if (Math.Abs(tmp[mod_index, i]) < epsilon)
                    {
                        rank = i - 1;
                        Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
                        return tp;
                    }

                    //свап цих векторів
                    if (mod_index > i)
                    {
                        tmp.SwapRows(i, mod_index);
                        tp.MadeSwaps.Add(new SwapEl(i, mod_index, i));
                    }
                }//if by column Gauss

                //real calculating
                for (k = i + 1; k < tmp.Height; ++k)
                {
                    for (j = i + 1; j < tmp.Width; ++j)
                        tmp[k, j] -= tmp[i, j] * tmp[k, i] / tmp[i, i];

                    tp.factors[used] = tmp[k, i] / tmp[i, i];
                    ++used;

                    tmp[k, i] = 0;
                }
            }//main for
            if (tmp.Height > tmp.Width)
                rank = tmp.Width;
            else
                rank = tmp.Height;

            Target = Epsilomalize(tmp.CopyOfMe(), epsilon);
            return tp;
        }

        #endregion

        #region Норми

        //рядкова норма A(INFINITY)
        public static double NormRows(Matrix A)
        {
            double[] vect = new double[A.Height];
            for (int i = 0; i < vect.Length; ++i)
                vect[i] = VectorOperations.ModuleElementsSum(A[i]);
            return VectorOperations.NormMax(vect);
        }

        //стовпцева норма A1
        public static double NormColumns(Matrix A)
        {
            double[][] Columns = A.GetColumns();
            double[] vect = new double[A.Width];
            for (int i = 0; i < vect.Length; ++i)
                vect[i] = VectorOperations.ModuleElementsSum(Columns[i]);
            return VectorOperations.NormMax(vect);
        }

        //Frobenius Norm
        public static double Norm(Matrix A)
        {
            double res = 0;
            for (int i = 0; i < A.Height; ++i)
            {
                res += VectorOperations.Norm(A[i]);
            }
            return res;
        }

        //число обумовленості матриці, що використовує рядкові норми
        public static double CondRows(Matrix A, double epsilon)
        {
            Matrix Reserved_A = MatrixOperations.FindReservedMatrix(A, epsilon);
            return NormRows(A) * NormRows(Reserved_A);
        }

        //число обумовленості матриці, що використовує стовпцеві норми
        public static double CondColumns(Matrix A, double epsilon)
        {
            Matrix Reserved_A = MatrixOperations.FindReservedMatrix(A, epsilon);
            return NormColumns(A) * NormColumns(Reserved_A);
        }

        #endregion

        #region Арифметичні операції

        //множення двох матриць(A*B)
        public static Matrix Product(Matrix A, Matrix B)
        {
            Matrix C = null;
            if (A.Width != B.Height)
                return C;
            C = new Matrix(B.Width, A.Height);
            for (int i = 0; i < A.Height; ++i)
            {
                for (int j = 0; j < B.Width; ++j)
                {
                    double temp_sum = 0;
                    for (int k = 0; k < A.Width; ++k)
                        temp_sum += A[i, k] * B[k, j];
                    C[i, j] = temp_sum;
                }
            }
            return C;
        }

        //множення двох матриць(A*B)
        public static Matrix Product(Matrix A, Matrix B, double Epsilon)
        {
            Matrix C = null;
            if (A.Width != B.Height)
                return C;
            C = new Matrix(B.Width, A.Height);
            for (int i = 0; i < A.Height; ++i)
            {
                for (int j = 0; j < B.Width; ++j)
                {
                    double temp_sum = 0;
                    for (int k = 0; k < A.Width; ++k)
                        temp_sum += A[i, k] * B[k, j];
                    if (temp_sum < Epsilon)
                        temp_sum = 0;
                    C[i, j] = temp_sum;
                }
            }
            return C;
        }

        //різниця двох матриць(A - B)
        public static Matrix Subtract(Matrix A, Matrix B)
        {
            Matrix C = null;
            if (A.Width != B.Width | A.Height != B.Height)
                return C;
            C = new Matrix(A.Width, A.Height);
            for (int i = 0; i < A.Height; ++i)
                for (int j = 0; j < A.Width; ++j)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }

        //сума двох матриць(A + B)
        public static Matrix Add(Matrix A, Matrix B)
        {
            Matrix C = null;
            if (A.Width != B.Width | A.Height != B.Height)
                return C;
            C = new Matrix(A.Width, A.Height);
            for (int i = 0; i < A.Height; ++i)
                for (int j = 0; j < A.Width; ++j)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }

        #endregion
    }
}
