using System;
using System.Collections.Generic;
using System.Text;

namespace SLAR
{
    //ітераційний метод спряжених градієнтів для симетричних додатно-визначених матриць
    public class JGrad_SLAR : MPI_SLAR
    {        
        #region Конструктори

        public JGrad_SLAR()
            : base()
        {
            C = null;
            d = null;
        }

        public JGrad_SLAR(int Dimension)
            : base(Dimension)
        {
            C = null;
            d = null;
        }

        public JGrad_SLAR(JGrad_SLAR jg_slar)
            : base(jg_slar)
        {
            C = null;
            d = null;
        }

        public JGrad_SLAR(Matrix matrixA, double[] vectorB)
            : base(matrixA, vectorB)
        {
            C = null;
            d = null;
        }

        #endregion

        public override void ReadSLAR()
        {
            base.ReadSLAR();
        }

        public override void ReSetSLAR(object obj)
        {
            JGrad_SLAR jg = (JGrad_SLAR)obj;
            if (jg == null)
                throw new NotSupportedException("Can not unbox object to Joined Gradients SLAR!");
            ReSetSLAR(jg.A, jg.b);
            C = jg.C.CopyOfMe();
            d = (double[])jg.d.Clone();
            slar_d = new SLAR_Diagnostics(jg.slar_d);
            isRandomStartVector = jg.isRandomStartVector;
            jg = null;
        }

        public override void SolveSLAR()
        {
            int n = x.Length;
            double[] x_curr = null;
            if (isRandomStartVector == true)
            {
                x_curr = VectorOperations.GetRandomVector(n);
                while (VectorOperations.IsEqualByEpsilon(x_curr, new double[n], Double.Epsilon))
                    x_curr = VectorOperations.GetRandomVector(n);
            }
            else
                x_curr = VectorOperations.Get_Unit_Vector(n);
            double[] x_next = new double[n];
            int iterations = 0;

            double[] r_curr = VectorOperations.Subtract(b, A.MulVector(x_curr));
            double[] r_next = new double[n];

            double[] p_curr = (double[])r_curr.Clone();
            double[] p_next = new double[n];

            double alpha = 0;
            double beta = 0;
            
            double[] nullParam = null;

            while (ContinueIterations(ref nullParam, ref r_curr))
            {
                double[] Ap = A.MulVector(p_curr);
                alpha = VectorOperations.ScalarProduct(r_curr, p_curr) / VectorOperations.ScalarProduct(p_curr, Ap);
                
                if (Double.IsNaN(alpha))
                    throw new Exception("Attempt of division by zero! Probably was entered not symmetrical matrix!");

                x_next = VectorOperations.Add(x_curr, VectorOperations.MulDouble(p_curr, alpha));
                r_next = VectorOperations.Add(r_curr, VectorOperations.MulDouble(Ap, (-1) * alpha));
                
                //checking convergence
                if (VectorOperations.NormSum(r_next) <= epsilon)
                    break;

                beta = VectorOperations.ScalarProduct(r_next, Ap) / VectorOperations.ScalarProduct(p_curr, Ap);
                p_next = VectorOperations.Add(r_next, VectorOperations.MulDouble(p_curr, (-1) * beta));

                //going to next step
                p_curr = (double[])p_next.Clone();
                r_curr = (double[])r_next.Clone();
                x_curr = (double[])x_next.Clone();
                ++iterations;
            }
            if (VectorOperations.NormSum(x_next) < epsilon)
                x = (double[])x_curr.Clone();
            else
                x = (double[])x_next.Clone();            
            slar_d.IterationNumber = iterations;
        }

        protected override bool ContinueIterations(ref double[] vectorPrev, ref double[] vectorCurr)
        {
            return (VectorOperations.NormSum(vectorCurr) > epsilon);
        }
    }
}
