using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SLAR
{
    //розв'язання системи рівнянь з симетричною матрицею методом квадратних коренів
    public class sqrt_SLAR : basic_SLAR
    {
        protected Matrix U; //головна матриця у методі квадратних коренів

        #region Конструктори
        public sqrt_SLAR()
            : base()
        {
        }

        public sqrt_SLAR(int equation_number, int variable_number)
            : base(equation_number, variable_number)
        {
        }

        public sqrt_SLAR(int Dimension)
            : base(Dimension)
        {
        }

        public sqrt_SLAR(Matrix matrixA, double[] vectorB)
            : base(matrixA, vectorB)
        {            
        }

        public sqrt_SLAR(sqrt_SLAR from)
            : base(from)
        {
            U = new Matrix(from.U);
        }
        #endregion

        protected void CalculateMatrixU()
        {
            U = new Matrix(A.Width, A.Width);
            double temp_sum = 0;
            double tmp = 0;
            int i = 0, j = 0, k = 0;

            for (; i < A.Width; ++i)
            {
                for (j = 0; j < A.Width; ++j)
                {
                    if (j < i)
                    {
                        U[i, j] = 0;
                        continue;
                    }

                    if (i == j)
                    {
                        temp_sum = 0;
                        for (k = 0; k < i; ++k)
                            temp_sum += (U[k, i]) * (U[k, i]);
                        tmp = (A[i, j]) - temp_sum;
                        if (tmp < 0)
                        {
//                            _rank = i;
                            throw new Exception("Can not find root of negative value!");
                        }
                        U[i, j] = Math.Sqrt(tmp);
                        continue;
                    }

                    if (j > i)
                    {
                        temp_sum = 0;
                        for (k = 0; k < i; ++k)
                            temp_sum += U[k, i] * U[k, j];
                        tmp = ((A[i, j]) - temp_sum) / U[i, i];
                        U[i, j] = tmp;
                    }
                } //inner loop
            }//main loop

/*            //обчислимо детермінант
            _determinant = 1.0;
            for (j = 0; j < A.Width; ++j)
            {
                tmp = U[j, j];
                _determinant *= (tmp * tmp);                
            }
            _rank = A.Height;

            if (_determinant == 0)
            {
                _rank = A.Height - 1;
                return;
            }
*/
        }

        protected void CalculateVectorX()
        {
            int i = 0, k = 0;
            double temp_sum = 0;
            //другий етап - обчислення тимчасового вектору /y/
            double[] y = new double[A.Width];
            for (i = 0; i < A.Width; ++i)
            {
                temp_sum = 0;
                for (k = 0; k < i; ++k)
                    temp_sum += U[k, i] * y[k];
                y[i] = (b[i] - temp_sum) / U[i, i];
            }

            //третій етап - обчислення розв'язку
            for (i = A.Width - 1; i >= 0; --i)
            {
                temp_sum = 0;
                for (k = i + 1; k < A.Width; ++k)
                    temp_sum += U[i, k] * (x[k]);
                double res = (y[i] - temp_sum) / U[i, i];
                if (Math.Abs(res) < epsilon)
                    res = 0;
                x[i] = res;
            }
        }

        public override void SolveSLAR()
        {
            CalculateMatrixU();
            CalculateVectorX();
        }

        public void PrintMatrixU()
        {
            U.Print();
        }

        public override void ReSetSLAR(object obj)
        {
            sqrt_SLAR sqrt_slar = (sqrt_SLAR)obj;
            ReSetSLAR(sqrt_slar.A, sqrt_slar.b);
            U = sqrt_slar.U.CopyOfMe();
            sqrt_slar = null;
        }

        public virtual void WriteUToStream(TextWriter tw)
        {
            tw.WriteLine(U.ToString());
        }
    }
}
