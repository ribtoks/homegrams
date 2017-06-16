using System;
using System.Collections.Generic;
using System.Text;

namespace SLAR
{
    //клас для методу ортогоналізації
    public class Ortho_SLAR : basic_SLAR
    {
        #region Конструктори

        public Ortho_SLAR()
            : base()
        {
        }

        public Ortho_SLAR(int equation_number, int variable_number)
            : base(equation_number, variable_number)
        {
        }

        public Ortho_SLAR(int Dimension)
            : base(Dimension)
        {
        }

        public Ortho_SLAR(Matrix matrixA, double[] vectorB)
            : base(matrixA, vectorB)
        {
        }

        public Ortho_SLAR(Ortho_SLAR from)
            : base(from)
        {
        }

        #endregion

        public override void SolveSLAR()
        {
            int i = 0;
            int N = b.Length;
            //initializing
            double[] alpha = new double[N];
            double[][] xVectors = new double[N][];
            for (; i < b.Length; ++i)
                xVectors[i] = new double[N];
            double[][] ECols = Matrix.Get_E(N).GetColumns();
            double[] lambda = new double[N];
            //end of initializing
            //...
            //calculating x(index)-vectors
            xVectors[0] = (double[])ECols[0].Clone();
            //main loop            
            for (i = 1; i < N; ++i)
            {
                for (int j = 0; j < i; ++j)
                {
                    lambda[j] = -(VectorOperations.ScalarProduct(ECols[i], A.MulVector(xVectors[j]))) / (VectorOperations.ScalarProduct(xVectors[j], A.MulVector(xVectors[j])));
                }

                xVectors[i] = (double[])ECols[i].Clone();
                for (int k = 0; k < i; ++k)
                {
                    VectorOperations.Add(ref xVectors[i], VectorOperations.MulDouble(xVectors[k], lambda[k]));
                }
            }

            //now calculating alpha
            for (i = 0; i < N; ++i)
                alpha[i] = VectorOperations.ScalarProduct(b, xVectors[i]) / VectorOperations.ScalarProduct(A.MulVector(xVectors[i]), xVectors[i]);
            
            x = new double[N];
            //now calculating result
            for (i = 0; i < N; ++i)
                VectorOperations.Add(ref x, VectorOperations.MulDouble(xVectors[i], alpha[i]));
        }

        public override void ReSetSLAR(object obj)
        {
            Ortho_SLAR or = (Ortho_SLAR)obj;
            ReSetSLAR(or.A, or.b);
            or = null;
        }
    }
}
