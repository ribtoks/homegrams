using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HierarchyClasses.MatrixClasses
{
    public static class VectorOperations
    {
        /// <summary>
        /// Creates vector with "1" elements
        /// </summary>
        /// <param name="size"></param>
        /// <returns>Vector with "1" elements and length "size"</returns>
        public static double[] Get_Unit_Vector(int size)
        {
            double[] v = new double[size];
            for (int i = 0; i < size; ++i)
                v[i] = 1;
            return v;
        }

        //перевірка з вказаною точністю
        public static bool AreEqualByEpsilon(double[] vect1, double[] vect2, double epsilon)
        {
            bool isEqual = true;
            if (vect1.Length != vect2.Length)
            {
                throw new Exception("Vectors have different size! Can not compare!");
            }
            for (int i = 0; i < vect1.Length; ++i)
                if (Math.Abs(vect1[i] - vect2[i]) > epsilon)
                {
                    isEqual = false;
                    break;
                }
            return isEqual;
        }

        /// <summary>
        /// Creates string from specified vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>String representation of vector</returns>
        public static string VectorToString(double[] vector)
        {
            string result = "( ";
            foreach (double element in vector)
            {
                result += element + " ";
            }
            result += ")T";
            return result;
        }

        /// <summary>
        /// Reversed input vector and returns copy
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double[] GetReversedVector(double[] vector)
        {
            double[] d = new double[vector.Length];
            for (int i = 0; i < vector.Length; ++i)
                d[i] = vector[vector.Length - i - 1];
            return d;
        }

        /// <summary>
        /// Creates vector with random elements, that are in rage min..max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="VectorLength"></param>
        /// <returns></returns>
        public static double[] GetRandomVector(int min, int max, int VectorLength)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            double[] res = new double[VectorLength];
            for (int i = 0; i < res.Length; ++i)
            {
                res[i] = r.Next(min, max);
            }
            return res;
        }

        /// <summary>
        /// Create vector with random elements from 0 to 1
        /// </summary>
        /// <param name="VectorLength"></param>
        /// <returns></returns>
        public static double[] GetRandomVector(int VectorLength)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            double[] res = new double[VectorLength];
            for (int i = 0; i < res.Length; ++i)
            {
                res[i] = r.NextDouble();
            }
            return res;
        }

        #region Operations

        /// <summary>
        /// Calculates scalar product of specified vectors
        /// </summary>
        /// <param name="vect1"></param>
        /// <param name="vect2"></param>
        /// <returns></returns>
        public static double ScalarProduct(double[] vect1, double[] vect2)
        {
            if (vect1.Length != vect2.Length)
                throw new Exception("Vectors have different size!");
            double res = 0;
            for (int i = 0; i < vect1.Length; ++i)
                res += vect1[i] * vect2[i];
            return res;
        }

        /// <summary>
        /// Calculates difference of two vectors
        /// </summary>
        /// <param name="vect1"></param>
        /// <param name="vect2"></param>
        /// <returns></returns>
        public static double[] Subtract(double[] vect1, double[] vect2)
        {
            if (vect1.Length != vect2.Length)
                return null;
            //throw new Exception("Different size of subtracting vectors!");

            double[] res = new double[vect1.Length];
            for (int i = 0; i < vect1.Length; ++i)
            {
                res[i] = vect1[i] - vect2[i];
            }
            return res;
        }

        /// <summary>
        /// Calculates sum of two vectors
        /// </summary>
        /// <param name="vect1"></param>
        /// <param name="vect2"></param>
        /// <returns></returns>
        public static double[] Add(double[] vect1, double[] vect2)
        {
            if (vect1.Length != vect2.Length)
                return null;
            //throw new Exception("Different size of subtracting vectors!");

            double[] res = new double[vect1.Length];
            for (int i = 0; i < vect1.Length; ++i)
            {
                res[i] = vect1[i] + vect2[i];
            }
            return res;
        }

        /// <summary>
        /// Calculates sum of two vectors and assigns it to first vector (a += b)
        /// </summary>
        /// <param name="vect1"></param>
        /// <param name="vect2"></param>
        public static void Add(ref double[] vect1, double[] vect2)
        {
            if (vect1.Length != vect2.Length)
                throw new Exception("Different size of operanding vectors!");
            //throw new Exception("Different size of subtracting vectors!");

            for (int i = 0; i < vect1.Length; ++i)
            {
                vect1[i] += vect2[i];
            }
        }

        /// <summary>
        /// Swaps elements in vector on specified positions
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public static void SwapElements(ref double[] vector, int i, int j)
        {
            double temp = vector[i];
            vector[i] = vector[j];
            vector[j] = temp;
        }

        /// <summary>
        /// Multiplies specified vector on double number
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="mulWhat"></param>
        /// <returns></returns>
        public static double[] MulDouble(double[] vector, double mulWhat)
        {
            double[] res = (double[])vector.Clone();
            for (int i = 0; i < res.Length; ++i)
                res[i] *= mulWhat;
            return res;
        }

        /// <summary>
        /// Multiplies specified vector on double number and save result in it (a *= const)
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="mulWhat"></param>
        public static void MulDouble(ref double[] vector, double mulWhat)
        {
            for (int i = 0; i < vector.Length; ++i)
                vector[i] *= mulWhat;
        }

        /// <summary>
        /// Adds scalar to vector elements
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="addWhat"></param>
        /// <returns></returns>
        public static double[] AddDouble(double[] vector, double addWhat)
        {
            double[] res = (double[])vector.Clone();
            for (int i = 0; i < res.Length; ++i)
                res[i] += addWhat;
            return res;
        }

        /// <summary>
        /// Adds scalar to vector elements and save result in it (a += const)
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="addWhat"></param>
        public static void AddDouble(ref double[] vector, double addWhat)
        {
            for (int i = 0; i < vector.Length; ++i)
                vector[i] += addWhat;
        }

        #endregion

        #region Norms

        /// <summary>
        /// Sum of squares of vector elements
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double Norm(double[] vector)
        {
            double norma = 0;
            for (int i = 0; i < vector.Length; ++i)
                norma += vector[i] * vector[i];
            return norma;
        }

        /// <summary>
        /// Maximum absolute element of vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double NormMax(double[] vector)
        {
            int curr = 0;
            for (int i = 1; i < vector.Length; ++i)
                if (Math.Abs(vector[i]) > Math.Abs(vector[curr]))
                    curr = i;
            return Math.Abs(vector[curr]);
        }

        /// <summary>
        /// Sum of absolute values of vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double NormSum(double[] vector)
        {
            double res = 0;
            for (int i = 0; i < vector.Length; ++i)
            {
                res += Math.Abs(vector[i]);
            }
            return res;
        }

        /// <summary>
        /// Square root of squares sum of elements
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double NormSqrt(double[] vector)
        {
            return Math.Sqrt(Norm(vector));
        }

        #endregion
    }
}
