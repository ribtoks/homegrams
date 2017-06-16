using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLAR;

namespace ProperValueProblem
{
    class Forward_SP : ProperValueBasic
    {
        #region Дані
        //найменше власне значення
        protected double lambda_max;

        //власний вектор, відповідний цьому власному значенню
        protected double[] proper_vector_max;
        #endregion

        #region Конструктори

        public Forward_SP()
            : base()
        {
            lambda_max = Double.NaN;
            proper_vector_max = null;
        }

        public Forward_SP(int Dimension)
            : base(Dimension)
        {
            lambda_max = Double.NaN;
            proper_vector_max = new double[Dimension];
        }

        public Forward_SP(int Dimension, double Epsilon)
            : base(Dimension, Epsilon)
        {
            lambda_max = Double.NaN;
            proper_vector_max = new double[Dimension];
        }

        public Forward_SP(Matrix A, double Epsilon)
            : base(A, Epsilon)
        {
            lambda_max = Double.NaN;
            proper_vector_max = new double[A.Width];
        }

        #endregion

        public override void SolveProperProblem()
        {
            isRandomStartVector = true;

            int n = A.Width;
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
            while (true)
            {
                y = A.MulVector(x);

                s = VectorOperations.ScalarProduct(y, y);
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
            lambda_max = lambda_curr;
            proper_vector_max = (double[])x.Clone();
        }

        public override void PrintResults()
        {
            Console.WriteLine("Maximal proper value is " + lambda_max);
            Console.WriteLine();
            Console.WriteLine("Proper vector:");
            VectorOperations.PrintVector(proper_vector_max);
            Console.WriteLine("Made " + slar_d.IterationNumber + " iteration(s).");
            Console.WriteLine();
        }

        public override void ReadProblem()
        {
            base.ReadProblem();
        }
    }
}
