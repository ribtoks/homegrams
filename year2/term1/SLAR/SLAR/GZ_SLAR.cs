using System;
using System.Collections.Generic;
using System.Text;

namespace SLAR
{
    //клас система рівнянь, що розв'язує систему ітераційним методом Гауса-Зейделя
    public class GZ_SLAR : MPI_SLAR
    {
        protected bool jacobiView = true;

        protected double convergenceConstant;

        #region Конструктори

        public GZ_SLAR()
            : base()
        {
        }

        public GZ_SLAR(int Dimension)
            : base(Dimension)
        {
        }

        public GZ_SLAR(GZ_SLAR gz_slar)
            : base(gz_slar)
        {
            jacobiView = gz_slar.jacobiView;
            convergenceConstant = gz_slar.convergenceConstant;
        }

        public GZ_SLAR(Matrix matrixA, double[] vectorB)
            : base(matrixA, vectorB)
        {
        }

        #endregion


        public override void ReadSLAR()
        {
            base.ReadSLAR();
            if (HasDiagonalAdvantage() == false)
            {
                Console.WriteLine();
//#if DEBUG
                Console.WriteLine("This matrix has not diagonal advantage. Wrong answer is possible.");
                Console.WriteLine();               
//#endif
//                throw new Exception("This matrix has not diagonal advantage. Wrong answer is possible.");                
            }
            Console.WriteLine("If you want to make SLAR like in Jacobi method, press 't'.\r\nElse, press any other key.");
            Console.Write(">:");
            char ch = Convert.ToChar(Console.ReadLine());
            if (ch == 't' | ch == 'T')
                jacobiView = true;
            FindConvergenceConstant();               
        }

        public override void ReSetSLAR(object obj)
        {
            GZ_SLAR gz = (GZ_SLAR)obj;
            if (gz == null)
                throw new NotSupportedException("Can not unbox object to GZ_SLAR!");
            ReSetSLAR(gz.A, gz.b);
            C = gz.C.CopyOfMe();
            d = (double[])gz.d.Clone();
            slar_d = new SLAR_Diagnostics(gz.slar_d);
            isRandomStartVector = gz.isRandomStartVector;
            jacobiView = gz.jacobiView;
            convergenceConstant = gz.convergenceConstant;
            gz = null;
        }

        public Matrix GetL()
        {
            Matrix L = new Matrix(A.Width, A.Height);
            for (int i = 0; i < A.Height; ++i)
            {
                for (int j = 0; j < A.Width; ++j)
                {
                    if (i > j)
                        L[i, j] = A[i, j];
                }
            }
            return L;
        }

        public Matrix GetD()
        {
            Matrix D = new Matrix(A.Width, A.Height);
            for (int i = 0; i < A.Height; ++i)
                D[i, i] = A[i, i];
            return D;
        }

        public Matrix GetR()
        {
            Matrix R = new Matrix(A.Width, A.Height);
            for (int i = 0; i < A.Height; ++i)
            {
                for (int j = 0; j < A.Width; ++j)
                {
                    if (j > i)
                        R[i, j] = A[i, j];
                }
            }
            return R;
        }

        //розв'язок методом Гауса-Зейделя
        public override void SolveSLAR()
        {            
            if (jacobiView == true)
                try
                {
                    SetAllByJacobi();                 
                }
                catch(Exception e)
                {
#if DEBUG
                    Console.WriteLine(e.Message);
                    return;
#endif
                    throw;
                }

            int n = x.Length;
            double[] x_prev = null;
            if (isRandomStartVector == true)
            {
                x_prev = VectorOperations.GetRandomVector(n);
                while (VectorOperations.IsEqualByEpsilon(x_prev, new double[n], Double.Epsilon))
                    x_prev = VectorOperations.GetRandomVector(n);
            }
            else
            {
                x_prev = VectorOperations.Get_Unit_Vector(n);
                x_prev[0] = 0;
            }
            double[] x_curr = new double[n];
            int iterations = 0;

#if DEBUG
            Console.WriteLine(VectorOperations.VectorToString(x_prev));
#endif
            

            if (VectorOperations.IsEqualByEpsilon(A.MulVector(x_prev), b, this.epsilon))
            {
                x = (double[])x_prev.Clone();
                slar_d.IterationNumber = iterations;
                return;
            }
            //main loop
//            while (ContinueIterations(x_prev, x_curr))
            do
            {                
                ++iterations;
                int j = 0;
                //changing elements snippet
                for (int i = 0; i < n; ++i)
                {
                    double sum_of_curr = 0;
                    for (j = 0; j < i; ++j)
                    {
                        sum_of_curr += C[i, j] * x_curr[j];
                    }

                    double sum_of_prev = 0;
                    for (j = i; j < n; ++j)
                    {
                        sum_of_prev += C[i, j] * x_prev[j];
                    }
                    x_curr[i] = sum_of_prev + sum_of_curr + d[i];
                    //\\changing elements snippet
                }
            } while (ContinueIterations(ref x_prev, ref x_curr));
            x = (double[])x_curr.Clone();
            slar_d.IterationNumber = iterations;
        }
       
        public void SetAllByJacobi()
        {
            Matrix LD = MatrixOperations.Add(GetL(), GetD());
            Matrix LD_1 = MatrixOperations.FindReservedMatrix(LD, epsilon);

            if (LD_1 == null)
                throw new Exception("Can not find reserved Matrix. Input is degenerated.");

            d = LD_1.MulVector(b);

            LD_1.MulDouble(-1);
            C = MatrixOperations.Product(LD_1, GetR());          
        }

        protected bool IsConvergence(double[] x_prev, double[] x_next)
        {
            double fraction = VectorOperations.NormSqrt(VectorOperations.Subtract(x_next, x_prev)) / VectorOperations.NormSqrt(x_prev);
            return (fraction <= convergenceConstant);
        }

        protected void FindConvergenceConstant()
        {
            Matrix A_1 = MatrixOperations.FindReservedMatrix(A, epsilon);
            Matrix LD = MatrixOperations.Add(GetL(), GetD());
            double norm_ld = Math.Sqrt(MatrixOperations.Norm(LD));
            double norm_a_1 = Math.Sqrt(MatrixOperations.Norm(A_1));
            convergenceConstant = 1 / (norm_ld * norm_a_1);
            convergenceConstant *= (epsilon / (epsilon + 1));
        }

        protected override bool ContinueIterations(ref double[] vectorPrev, ref double[] vectorCurr)
        {
            bool is_true = (!IsConvergence(vectorPrev, vectorCurr));
            vectorPrev = (double[])vectorCurr.Clone();
            return is_true;
        }
    }
}
