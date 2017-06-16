using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HierarchyClasses.MatrixClasses
{
    [Serializable]
    public class Matrix : System.Collections.IEnumerable, StandartMatrixInterface, IOMatrixInterface, ICloneable
    {
        protected double[][] mArray;
        protected int dimWidth;
        protected int dimHeight;

        #region Constructors

        public Matrix()
        {
            mArray = null;
            dimHeight = 0;
            dimWidth = 0;
        }

        public Matrix(int width, int height)
        {
            dimHeight = height;
            dimWidth = width;
            mArray = new double[height][];
            for (int i = 0; i < height; ++i)
                mArray[i] = new double[width];
        }

        public Matrix(int dimension)
            : this(dimension, dimension)
        {
        }

        public Matrix(Matrix copyFrom)
        {
            dimHeight = copyFrom.dimHeight;
            dimWidth = copyFrom.dimWidth;
            //для ссилок не працюватиме!!!
            mArray = new double[dimHeight][];
            for (int k = 0; k < dimHeight; ++k)
                mArray[k] = new double[copyFrom.mArray[k].Length];

            for (int i = 0; i < mArray.Length; ++i)
                for (int j = 0; j < copyFrom.mArray[i].Length; ++j)
                    mArray[i][j] = copyFrom.mArray[i][j];
        }

        public Matrix(double[][] data, bool areColumns)
        {
            if (areColumns == true)
            {
                dimHeight = data[0].Length;
                dimWidth = data.Length;
                mArray = new double[dimHeight][];
                int i = 0;
                for (; i < dimHeight; ++i)
                    mArray[i] = new double[dimWidth];
                for (i = 0; i < dimHeight; ++i)
                    for (int j = 0; j < dimWidth; ++j)
                        mArray[i][j] = data[j][i];
            }
            else
            {
                dimHeight = data.Length;
                dimWidth = data[0].Length;
                mArray = new double[data.Length][];
                for (int i = 0; i < data.Length; ++i)
                    mArray[i] = (double[])data[i].Clone();
            }
        }

        #endregion

        /// <summary>
        /// Assigns zero to all elements
        /// </summary>
        public void ZeroMe()
        {
            for (int i = 0; i < dimHeight; ++i)
                for (int j = 0; j < dimWidth; ++j)
                    mArray[i][j] = 0;
        }

        public int Width
        {
            get { return dimWidth; }
        }

        public int Height
        {
            get { return dimHeight; }
        }

        public double this[int h, int w]
        {
            get
            {
                if (h >= 0 & h < dimHeight & w >= 0 & w < dimWidth)
                    return mArray[h][w];
                else
                    return double.NaN;
                    //throw new Exception("Indices out of Matrix dimensions!");
            }
            set
            {
                if (h >= 0 & h < dimHeight & w >= 0 & w < dimWidth)
                    mArray[h][w] = value;
                else
                    throw new Exception("Indices out of Matrix dimensions!");
            }
        }

        //рядковий індексатор
        public double[] this[int index]
        {
            get
            {
                if (index >= 0 & index < dimHeight)
                {
                    return mArray[index];
                }
                else
                    return null;
                    //throw new Exception("Index out of Matrix dimensions!");
            }
            set
            {
                if (index >= 0 & index < dimHeight & value.Length == dimWidth)
                {
                    mArray[index] = value;
                }
                else
                    throw new Exception("Index out of Matrix dimensions!");
            }
        }

        #region Matrix foreach implementations
        public System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < dimHeight; ++i)
                for (int j = 0; j < dimWidth; ++j)
                {
                    yield return mArray[i][j];
                }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public static Matrix Get_E(int N)
        {
            Matrix E = new Matrix(N, N);
            for (int i = 0; i < N; ++i)
                E.mArray[i][i] = 1;
            return E;
        }

        //повертає окремі рвані масиви рядків
        public double[][] GetRows()
        {
            double[][] M = new double[mArray.Length][];
            for (int i = 0; i < mArray.Length; ++i)
                M[i] = (double[])mArray[i].Clone();
            return M;
        }

        /// <summary>
        /// Get array of columns
        /// </summary>
        /// <returns></returns>
        public double[][] GetColumns()
        {
            double[][] M = new double[mArray[0].Length][];
            int i = 0;
            for (; i < mArray[0].Length; ++i)
                M[i] = new double[mArray.Length];
            for (int j = 0; j < dimWidth; ++j)
                for (i = 0; i < dimHeight; ++i)
                    M[j][i] = mArray[i][j];
            return M;
        }

        #region Operators

        public static Matrix operator +(Matrix op1, Matrix op2)
        {
            return MatrixOperations.Add(op1, op2);
        }

        public static Matrix operator -(Matrix op1, Matrix op2)
        {
            return MatrixOperations.Subtract(op1, op2);
        }

        public static Matrix operator +(Matrix op, double d)
        {
            return MatrixOperations.Add(op, d);
        }

        public static Matrix operator *(Matrix A, Matrix B)
        {
            return MatrixOperations.Product(A, B);
        }

        public static Matrix operator *(Matrix op, double d)
        {
            Matrix m = (Matrix)op.Clone();
            m.MulDouble(d);
            return m;
        }

        #endregion

        #region Operations

        /// <summary>
        /// Multiply matrix on vector
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public double[] MulVector(double[] vector)
        {
            if (dimWidth != vector.Length)
                throw new Exception("Can not multiply matrix and vector! Different sizes!");
            double[] result = new double[dimHeight];
            for (int i = 0; i < dimHeight; ++i)
            {
                result[i] = VectorOperations.ScalarProduct(vector, mArray[i]);
            }
            return result;
        }

        /// <summary>
        /// Multiply matrix on scalar value
        /// </summary>
        /// <param name="mulWhat"></param>
        public void MulDouble(double mulWhat)
        {
            for (int i = 0; i < dimHeight; ++i)
                for (int j = 0; j < dimWidth; ++j)
                {
                    mArray[i][j] *= mulWhat;
                }
        }

        /// <summary>
        /// Adds scalar to matrix
        /// </summary>
        /// <param name="addWhat"></param>
        public void AddDouble(double addWhat)
        {
            for (int i = 0; i < dimHeight; ++i)
                for (int j = 0; j < dimWidth; ++j)
                {
                    mArray[i][j] += addWhat;
                }
        }

        /// <summary>
        /// Swaps rows with specified indices
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void SwapRows(int i, int j)
        {
            for (int k = 0; k < dimWidth; ++k)
            {
                double temp = mArray[i][k];
                mArray[i][k] = mArray[j][k];
                mArray[j][k] = temp;
            }
        }

        /// <summary>
        /// Swaps columns with specified indices
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public void SwapColumns(int i, int j)
        {
            for (int k = 0; k < dimHeight; ++k)
            {
                double temp = mArray[k][i];
                mArray[k][i] = mArray[k][j];
                mArray[k][j] = temp;
            }
        }

        /// <summary>
        /// Get sum of diagonal elements
        /// </summary>
        /// <returns></returns>
        public double Track()
        {
            double track = 0;
            for (int i = 0; i < dimHeight; ++i)
                track += mArray[i][i];
            return track;
        }

        #endregion

        #region Reading matrix code

        /// <summary>
        /// Reading of matrix row from string
        /// </summary>
        /// <param name="rowString"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public double ReadRow(string rowString, int rowIndex)
        {
            double d;
            mArray[rowIndex] = EquationParser.Parse(rowString, dimWidth, out d);
            return d;
        }

        /// <summary>
        /// Parsing matrix from array of strings
        /// </summary>
        /// <param name="rows"></param>
        public void ReadAllRows(string[] rows)
        {
            double d;
            for (int i = 0; i < rows.Length; ++i)
            {
                mArray[i] = EquationParser.Parse(rows[i], dimWidth, out d);
            }
        }

        /// <summary>
        /// Reading matrix from stream like equations
        /// </summary>
        /// <param name="tr"></param>
        public void ReadFromStreamWithParsing(TextReader tr)
        {
            for (int i = 0; i < dimHeight; ++i)
            {
                string s = tr.ReadLine();
                double d;
                mArray[i] = EquationParser.Parse(s, dimWidth, out d);
            }
        }

        /// <summary>
        /// Reading matrix from stream
        /// </summary>
        /// <param name="tr"></param>
        public void ReadFromStream(TextReader tr)
        {
            for (int i = 0; i < dimHeight; ++i)
            {
                string s = tr.ReadLine();
                string[] numbers = s.Split(' ');
                for (int j = 0; j < numbers.Length; ++j)
                    mArray[i][j] = Double.Parse(numbers[j]);
            }
        }
        #endregion

        #region Matrix writing
        
        public void Print()
        {
            WriteToStream(Console.Out);
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < dimHeight; ++i)
            {
                for (int j = 0; j < dimWidth; ++j)
                {
                    result += mArray[i][j].ToString() + " "; //Math.Round(mArray[i][j],MidpointRounding.AwayFromZero).ToString() + " ";
                }
                result += Environment.NewLine;
            }
            return result;
        }

        public void WriteToStream(TextWriter tw)
        {
            tw.WriteLine(this.ToString());
        }

        #endregion

        public object Clone()
        {
            return new Matrix(this);
        }
    }
}
