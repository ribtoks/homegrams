using System;
using System.Collections.Generic;
using System.Text;

namespace SLAR
{
    public static class VectorOperations
    {        
        //отримати одиничний вектор вказаної довжини
        public static double[] Get_Unit_Vector(int size)
        {
            double[] v = new double[size];
            for (int i = 0; i < size; ++i)
                v[i] = 1;
            return v;
        }

        //надрукувати вектор
        public static void PrintVector(double[] vector)
        {
            Console.Write("Vector: ( ");
            foreach (double element in vector)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine(")T");
            Console.WriteLine();
        }

        //перевірка з вказаною точністю
        public static bool IsEqualByEpsilon(double[] vect1, double[] vect2, double epsilon)
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

        public static double[] GetReversedVector(double[] vector)
        {
            double[] d = new double[vector.Length];
            for (int i = 0; i < vector.Length; ++i)
                d[i] = vector[vector.Length - i - 1];
            return d;
        }

        //повертає вектор з рендомними числами, що лежать в [min, max]
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
      
        //повертає вектор з рендомними числами, що лежать в [min, max]
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

        //перековбасити вектор окремо від трикутної матриці
        public static double[] GetTrianguledVector(double[] source, TriangulingProtocol TrPt)
        {
            double[] res = (double[])source.Clone();
            TriangulingProtocol tp = new TriangulingProtocol(TrPt);
            int last_swap = 0;
            int used_factor = 0;
            for (int i = 0; i < source.Length; ++i)
            {
                //while-version
                if (last_swap < tp.MadeSwaps.Count)                    
                    while (tp.MadeSwaps[last_swap].iteration == i)
                    {
                        VectorOperations.SwapElements(ref res, tp.MadeSwaps[last_swap].x, tp.MadeSwaps[last_swap].y);
                        ++last_swap;
                        if (last_swap == tp.MadeSwaps.Count)
                            break;
                    }
                for (int k = i + 1; k < res.Length; ++k)
                {
                    res[k] -= tp.factors[used_factor] * res[i];
                    ++used_factor;
                }
            }
            return res;
        }

        #region Операції

        //просто скалярний добуток двох векторів
        public static double ScalarProduct(double[] vect1, double[] vect2)
        {
            if (vect1.Length != vect2.Length)
                throw new Exception("Vectors have different size!");
            double res = 0;
            for (int i = 0; i < vect1.Length; ++i)
                res += vect1[i] * vect2[i];
            return res;
        }

        //різниця двох векторів
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

        //cума двох векторів
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

        //cума двох векторів
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

        public static void SwapElements(ref double[] vector, int i, int j)
        {
            double temp = vector[i];
            vector[i] = vector[j];
            vector[j] = temp;
        }

        public static double[] MulDouble(double[] vector, double mulWhat)
        {
            double[] res = (double[])vector.Clone();
            for (int i = 0; i < res.Length; ++i)
                res[i] *= mulWhat;
            return res;
        }

        public static void MulDouble(ref double[] vector, double mulWhat)
        {
            for (int i = 0; i < vector.Length; ++i)
                vector[i] *= mulWhat;
        }

        public static double[] AddDouble(double[] vector, double addWhat)
        {
            double[] res = (double[])vector.Clone();
            for (int i = 0; i < res.Length; ++i)
                res[i] += addWhat;
            return res;
        }

        public static void AddDouble(ref double[] vector, double addWhat)
        {
            for (int i = 0; i < vector.Length; ++i)
                vector[i] += addWhat;
        }

        #endregion

        #region Норми

        //сума квадратів елементів вектора
        public static double Norm(double[] vector)
        {
            double norma = 0;
            for (int i = 0; i < vector.Length; ++i)
                norma += vector[i] * vector[i];
            return norma;
        }

        //максимальний по модулю елемент
        public static double NormMax(double[] vector)
        {
            int curr = 0;
            for (int i = 1; i < vector.Length; ++i)
                if (Math.Abs(vector[i]) > Math.Abs(vector[curr]))
                    curr = i;
            return Math.Abs(vector[curr]);
        }

        //сума модулів елементів
        public static double NormSum(double[] vector)
        {
            double res = 0;
            for (int i = 0; i < vector.Length; ++i)
            {
                res += Math.Abs(vector[i]);
            }
            return res;
        }

        //корінь суми квадратів
        public static double NormSqrt(double[] vector)
        {
            return Math.Sqrt(Norm(vector));
        }

        public static double ModuleElementsSum(double[] vector)
        {
            double sum = 0;
            foreach (double d in vector)
            {
                sum += Math.Abs(d);
            }
            return sum;
        }

        #endregion
    }
}
