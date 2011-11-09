using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HierarchyClasses.MatrixClasses
{
    public static class MatrixOperations
    {
        /// <summary>
        /// Returns Transposed Matrix
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static Matrix GetTranspose(Matrix from)
        {
            Matrix temp = new Matrix(from.Width, from.Height);
            for (int i = 0; i < from.Height; ++i)
                for (int j = 0; j < from.Width; ++j)
                    temp[i, j] = from[j, i];
            return temp;
        }

        /// <summary>
        /// Builds matrix from columns
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static Matrix BuildFromColumns(double[][] columns)
        {
            Matrix col = new Matrix(columns, true);
            return col;
        }

        /// <summary>
        /// Replace all elements, which absolute values are less epsilon, to zero
        /// </summary>
        /// <param name="A"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static Matrix Epsilomalize(Matrix A, double epsilon)
        {
            Matrix B = (Matrix)A.Clone();
            for (int i = 0; i < B.Height; ++i)
                for (int j = 0; j < B.Width; ++j)
                    if (Math.Abs(B[i, j]) < epsilon)
                        B[i, j] = 0;
            return B;
        }

        /// <summary>
        /// Find element with maximum absolute value
        /// </summary>
        /// <param name="A"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Turn all diagonal elements to zero and return copy
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        public static Matrix GetV_Matrix(Matrix A)
        {
            Matrix B = new Matrix(A);
            for (int i = 0; i < B.Height; ++i)
                B[i, i] = 0;
            return B;
        }

        /// <summary>
        /// Turn all diagonal elements to zero and make changes in place
        /// </summary>
        /// <param name="A"></param>
        public static void GetV_Matrix(ref Matrix A)
        {
            for (int i = 0; i < A.Height; ++i)
                A[i, i] = 0;
        }

        #region Norm

        //рядкова норма A(INFINITY)
        public static double NormRows(Matrix A)
        {
            double[] vect = new double[A.Height];
            for (int i = 0; i < vect.Length; ++i)
                vect[i] = VectorOperations.NormSum(A[i]);
            return VectorOperations.NormMax(vect);
        }

        //стовпцева норма A1
        public static double NormColumns(Matrix A)
        {
            double[][] Columns = A.GetColumns();
            double[] vect = new double[A.Width];
            for (int i = 0; i < vect.Length; ++i)
                vect[i] = VectorOperations.NormSum(Columns[i]);
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

        #endregion

        #region Operations

        //множення двох матриць(A*B)
        /// <summary>
        /// Finds matrix product
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
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

        /// <summary>
        /// All elements, less epsilon, will be zeros
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="Epsilon"></param>
        /// <returns></returns>
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

        public static Matrix Add(Matrix A, double d)
        {
            Matrix m = (Matrix)A.Clone();
            m.AddDouble(d);
            return m;
        }

        #endregion
    }
}
