using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SLAR
{
    //клас для реалізації LU-розкладу(decomposition)
    public class LU_SLAR : basic_SLAR
    {
        //квадратна додаткова матриці
        protected Matrix LU;
        //додатковий вектор для полегшення обчислення
        protected double[] y;

        #region Конструктори
        public LU_SLAR()
        {
            LU = new Matrix();
            y = new double[0];
        }

        public LU_SLAR(int Dimension)
            : base(Dimension)
        {
            LU = new Matrix(Dimension, Dimension);            
            y = new double[Dimension];
        }

        public LU_SLAR(Matrix matrixA, double[] vectorB)
            : base(matrixA, vectorB)
        {            
            LU = new Matrix(matrixA.Height);
            y = new double[matrixA.Height];
        }

        public LU_SLAR(LU_SLAR lu_slar)
            : base(lu_slar)
        {
            LU = new Matrix(lu_slar.LU);
            y = (double[])lu_slar.y.Clone();
        }

        #endregion

        public override void SolveSLAR()
        {
            //спочатку обчислюємо матриці L та U
            double sum;
            int i = 0;
            int j;
            int k;
            for (; i < A.Height; ++i)
            {
                for (j = 0; j < A.Width; ++j)
                {
                    if (i <= j)
                    {
                        //обчислення суми
                        sum = 0;
                        for (k = 0; k <= i - 1; ++k)
                            sum += LU[i, k] * LU[k, j];
                        LU[i, j] = A[i, j] - sum;
                    }
                    else
                    {
                        //обчислення суми
                        sum = 0;
                        for (k = 0; k <= j - 1; ++k)
                            sum += LU[i, k] * LU[k, j];
                        LU[i, j] = (A[i, j] - sum) / LU[j, j];
                    }
                }
            }

            //тепер обчислюємо додатковий вектор 'y'
            for (i = 0; i < A.Height; ++i)
            {
                sum = 0;
                for (k = 0; k <= i - 1; ++k)
                    sum += LU[i, k] * y[k];
                y[i] = b[i] - sum;
            }

            //тепер власне вектор 'x'
            for (i = A.Height - 1; i >= 0; --i)
            {
                sum = 0;
                for (k = i + 1; k < A.Height; ++k)
                    sum += LU[i, k] * x[k];
                x[i] = (y[i] - sum) / LU[i, i];
            }
/*
            //попутньо визначими детермінант
            _determinant = 1.0;
            for (j = 0; j < A.Height; ++j)
                _determinant *= LU[j, j];
            
            //ну і ранг
            _rank = A.Height;            
*/
        }

        public override void PrintResults()
        {
            int i = 0;
            int j = 0;
            base.PrintResults();
//            LU.Print();     
            //вивід матриці L(витягнення з LU)
            Console.WriteLine();
            Console.WriteLine("Matrix L:");
            for (; i < LU.Height; ++i)
            {
                for (j = 0; j < LU.Width; ++j)
                {
                    if (i > j)
                        Console.Write(LU[i, j] + " ");
                    else
                        if (i == j)
                            Console.Write("1 ");
                        else
                            Console.Write("0 ");
                }
                Console.WriteLine();
            }
            //вивід матриці U(витягнення з LU)
            Console.WriteLine();
            Console.WriteLine("Matrix U:");
            for (i = 0; i < LU.Height; ++i)
            {
                for (j = 0; j < LU.Width; ++j)
                {
                    if (j >= i)
                        Console.Write(LU[i, j] + " ");
                    else
                        Console.Write("0 ");
                }
                Console.WriteLine();
            }            
        }
        
        //отримання матриці L
        public Matrix GetL()
        {
            Matrix L = new Matrix(LU.Width, LU.Width);
            for (int i = 0; i < LU.Height; ++i)
            {
                for (int j = 0; j < LU.Width; ++j)
                {
                    if (i > j)
                        L[i, j] = LU[i, j];
                    else
                        if (i == j)
                            L[i, j] = 1;
                        else
                            L[i, j] = 0;
                }                
            }
            return L;
        }

        //отримання матриці U
        public Matrix GetU()
        {
            Matrix U = new Matrix(LU.Width, LU.Width);
            for (int i = 0; i < LU.Height; ++i)
            {
                for (int j = 0; j < LU.Width; ++j)
                {
                    if (j >= i)
                        U[i, j] = LU[i, j];
                    else
                        U[i, j] = 0;
                }
            }
            return U;
        }

        public override void ReSetSLAR(object obj)
        {
            LU_SLAR lu = (LU_SLAR)obj;
            ReSetSLAR(lu.A, lu.b);
            LU = lu.LU.CopyOfMe();
            y = (double[])lu.y.Clone();
            lu = null;
        }

        public virtual void WriteLToStream(TextWriter tw)
        {
            tw.WriteLine(GetL().ToString());
        }

        public virtual void WriteUToStream(TextWriter tw)
        {
            tw.WriteLine(GetU().ToString());
        }
    }
}
