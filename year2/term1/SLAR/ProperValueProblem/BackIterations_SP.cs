using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLAR;

namespace ProperValueProblem
{
    class BackIterations_SP : ProperValueBasic
    {
        #region Дані
        //найменше власне значення
        protected double lambda_min;

        //власний вектор, відповідний цьому власному значенню
        protected double[] proper_vector_min;
        #endregion

        #region Конструктори

        public BackIterations_SP()
            : base()
        {
            lambda_min = Double.NaN;
            proper_vector_min = null;
        }

        public BackIterations_SP(int Dimension)
            : base(Dimension)
        {
            lambda_min = Double.NaN;
            proper_vector_min = new double[Dimension];
        }

        public BackIterations_SP(int Dimension, double Epsilon)
            : base(Dimension, Epsilon)
        {
            lambda_min = Double.NaN;
            proper_vector_min = new double[Dimension];
        }

        public BackIterations_SP(Matrix A, double Epsilon)
            : base(A, Epsilon)
        {
            lambda_min = Double.NaN;
            proper_vector_min = new double[A.Width];
        }

        #endregion

        public override void SolveProperProblem()
        {
            isRandomStartVector = true;
            int n = A.Width;
            Matrix TriangA = null;
            int rang = 0;
            TriangulingProtocol tp = MatrixOperations.ToTrianguled(A, ref TriangA, out rang, true, Epsilon);
            //initialization
            double[] y = null;

            if (isRandomStartVector == true)
            {
                y = VectorOperations.GetRandomVector(n);                
            }
            else
            {
                y = VectorOperations.Get_Unit_Vector(n);
                y[0] = 0;
            }
            

            double s = VectorOperations.ScalarProduct(y, y);
            
            if (s == 0)
            {
                y = VectorOperations.Get_Unit_Vector(n);
                y[0] = 0;
                s = VectorOperations.ScalarProduct(y, y);
            }

            double t = 0;            
            double[] x = VectorOperations.MulDouble(y, 1 / Math.Sqrt(s));

            double lambda_prev = y[0];
            double lambda_curr = 0;

            int iterations = 0;
            //end of initialization

            Gauss_SLAR bSLAR = new Gauss_SLAR(TriangA, x, true);
            bSLAR.Epsilon = this.epsilon;
            while (true)
            {
                double[] x_temp = VectorOperations.GetTrianguledVector(x, tp);
                bSLAR.ChangeVectorB(x_temp, true);
                //solving [A*y(k) == x(k - 1)];
                bSLAR.SolveSLAR();

#if DEBUG
                if (!bSLAR.SolutionMatches())
                    throw new Exception("Solution doesn't match!");
#endif
                
                y = (double[])bSLAR.vectorX.Clone();

                s = VectorOperations.Norm(y);
                t = VectorOperations.ScalarProduct(y, x);
                x = VectorOperations.MulDouble(y, 1 / Math.Sqrt(s));                
                lambda_curr = s / t;

                if (Math.Abs(lambda_curr - lambda_prev) < epsilon)
                    break;

                //<going to next step> snippet                
                lambda_prev = lambda_curr;
                ++iterations;                
            }
            slar_d.IterationNumber = iterations;
            lambda_min = 1 / lambda_curr;
            proper_vector_min = (double[])x.Clone();
        }

        public override void PrintResults()
        {
            Console.WriteLine("Minimal proper value is " + lambda_min);
            Console.WriteLine();
            Console.WriteLine("Proper vector:");
            VectorOperations.PrintVector(proper_vector_min);
            Console.WriteLine("Made " + slar_d.IterationNumber + " iteration(s).");
            Console.WriteLine();
        }

        public override void ReadProblem()
        {
            base.ReadProblem();
        }
    }
}
