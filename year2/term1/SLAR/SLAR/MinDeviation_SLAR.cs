using System;
using System.Collections.Generic;
using System.Text;

namespace SLAR
{
    public class MinDeviation_SLAR : MPI_SLAR
    {
        #region Конструктори

        public MinDeviation_SLAR()
            : base()
        {
        }

        public MinDeviation_SLAR(int Dimension)
            : base(Dimension)
        {
        }

        public MinDeviation_SLAR(MinDeviation_SLAR md_slar)
            : base(md_slar)
        {
        }

        public MinDeviation_SLAR(Matrix matrixA, double[] vectorB)
            : base(matrixA, vectorB)
        {
        }

        #endregion

        public override void ReadSLAR()
        {
            base.ReadSLAR();
        }

        public override void SolveSLAR()
        {
            //черговий відхил
            double[] r = null;
            //коефіціент для варіаційної ітерації
            double tau = 0;
            //other initialization
            int n = x.Length;
            double[] x_prev = null;
            if (isRandomStartVector == true)
            {
                x_prev = VectorOperations.GetRandomVector(n);
                while (VectorOperations.IsEqualByEpsilon(x_prev, new double[n], epsilon))
                    x_prev = VectorOperations.GetRandomVector(n);
            }
            else
            {
                x_prev = VectorOperations.Get_Unit_Vector(n);
                x_prev[0] = 0;
            }
            double[] x_curr = new double[n];
            int iterations = 0;
            do
            {
                r = VectorOperations.Subtract(A.MulVector(x_prev), b);
                double sq = VectorOperations.NormSqrt(A.MulVector(r));
                tau = VectorOperations.ScalarProduct(A.MulVector(r), r) / (sq * sq);

                x_curr = VectorOperations.Subtract(x_prev, VectorOperations.MulDouble(r, tau));
                ++iterations;
            } while (ContinueIterations(ref x_prev, ref x_curr));
            x = (double[])x_curr.Clone();
            slar_d.IterationNumber = iterations;
        }

        public override void ReSetSLAR(object obj)
        {
            MinDeviation_SLAR md = (MinDeviation_SLAR)obj;
            if (md == null)
                throw new NotSupportedException("Can not unbox object to GZ_SLAR!");
            ReSetSLAR(md.A, md.b);
            C = md.C.CopyOfMe();
            d = (double[])md.d.Clone();
            slar_d = new SLAR_Diagnostics(md.slar_d);
            isRandomStartVector = md.isRandomStartVector;
            md = null;
        }

        protected override bool ContinueIterations(ref double[] vectorPrev, ref double[] vectorCurr)
        {
            bool is_true = (!VectorOperations.IsEqualByEpsilon(vectorPrev, vectorCurr, epsilon));
            vectorPrev = (double[])vectorCurr.Clone();
            return is_true;
        }
    }
}
