using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLAR;

namespace ProperValueProblem
{
    class JacobiTurns : ProperValueBasic
    {
        #region Дані

        //масив колонок власних векторів
        protected double[][] ProperVectors;
        //масив власний значень
        protected double[] lambda;

        #endregion

        #region Конструктори

        public JacobiTurns()
            : base()
        {
            ProperVectors = null;
            lambda = null;
        }

        public JacobiTurns(int Dimension)
            : base(Dimension)
        {
            ProperVectors = new double[Dimension][];
            for (int i = 0; i < Dimension; ++i)
                ProperVectors[i] = new double[Dimension];
            lambda = new double[Dimension];
        }

        public JacobiTurns(int Dimension, double Epsilon)
            : base(Dimension, Epsilon)
        {
            ProperVectors = new double[Dimension][];
            for (int i = 0; i < Dimension; ++i)
                ProperVectors[i] = new double[Dimension];
            lambda = new double[Dimension];
        }

        public JacobiTurns(Matrix A, double Epsilon)
            : base(A, Epsilon)
        {
            int Dimension = A.Width;
            ProperVectors = new double[Dimension][];
            for (int i = 0; i < Dimension; ++i)
                ProperVectors[i] = new double[Dimension];
            lambda = new double[Dimension];
        }

        #endregion

        public override void SolveProperProblem()
        {           
            Matrix Prev = A.CopyOfMe();
            Matrix Next = null;            

            double p, q, d, r, c, s;
            //main elenemt of transformed matrix - maxModule element
            double mainElement;
            //matrix of proper vectors(columns)
            Matrix T = Matrix.Get_E(A.Width);
            //indices of maxModule element
            int i, j;
            while (Math.Sqrt(MatrixOperations.Norm(MatrixOperations.GetV_Matrix(Prev))) >= epsilon)
            {
                mainElement = Math.Abs(MatrixOperations.MaxModuleElement(Prev, out i, out j));
                p = 2 * mainElement;
                q = Prev[i, i] - Prev[j, j];
                d = Math.Sqrt(p * p + q * q);
                //if q != 0
                if (Math.Abs(q) >= epsilon)
                {
                    r = Math.Abs(q) / (2 * d);
                    c = Math.Sqrt(0.5 + r);
                    if (Math.Abs(p * epsilon) < Math.Abs(q))
                    {
                        s = Math.Abs(p) * Math.Sign(p * q) / (2 * c * d);
                    }
                    else
                    {
                        s = Math.Sqrt(0.5 - r) * Math.Sign(p * q);
                    }
                }
                else
                {
                    //q == 0
                    s = Math.Sqrt(2) / 2.0;
                    c = s;
                }
                //now calculating the matrix elements
                Next = Prev.CopyOfMe();
                Next[i, i] = c * c * Prev[i, i] + s * s * Prev[j, j] + 2 * s * c * Prev[i, j];
                Next[j, j] = s * s * Prev[i, i] + c * c * Prev[j, j] - 2 * s * c * Prev[i, j];
                double temp_res = (c * c - s * s) * Prev[i, j] + c * s * (Prev[j, j] - Prev[i, i]);
#if DEBUG
                Next[i, j] = temp_res;// 0;
                Next[j, i] = temp_res;// 0;
#else
                Next[i, j] = 0;
                Next[j, i] = 0;
#endif

                for (int m = 0; m < A.Height; ++m)
                    if (m != i & m != j)
                    {
                        double temp1 = c * Prev[m, i] + s * Prev[m, j];
                        double temp2 = -s * Prev[m, i] + c * Prev[m, j];
                        Next[i, m] = Next[m, i] = temp1;
                        Next[j, m] = Next[m, j] = temp2;
                    }                
                //now continue generating of matrix T
                Matrix Temp = Matrix.Get_E(A.Width);
                Temp[i, i] = c;
                Temp[j, j] = c;
                Temp[i, j] = -s;
                Temp[j, i] = s;
                T = MatrixOperations.Product(T, Temp);

                //going to next step
                Prev = MatrixOperations.Epsilomalize(Next, epsilon);
//                Prev = Next.CopyOfMe();
            }

            //return result
            ProperVectors = T.GetColumns();
            for (i = 0; i < Prev.Height; ++i)
                lambda[i] = Prev[i, i];
        }

        public override void ReadProblem()
        {
            base.ReadProblem();
        }

        public override void PrintResults()
        {           
            for (int i = 0; i < lambda.Length; ++i)
            {
                Console.WriteLine("Proper value #" + (i + 1) + " : " + lambda[i]);
                Console.WriteLine("Proper vector #" + (i + 1) + " : " + VectorOperations.VectorToString(ProperVectors[i]));
                Console.WriteLine();
            }                    
        }
    }
}
