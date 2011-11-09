using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SLAR
{
    public class DiagonalMatrix : System.Collections.IEnumerable, StandartMatrixInterface, IOMatrixInterface, ConsoleMatrixInterface
    {
        protected int dimension;
        protected double[] diagonal;

        #region Конструктори

        public DiagonalMatrix()
        {
            diagonal = null;
            dimension = 0;
        }

        public DiagonalMatrix(int Dimension)
        {
            dimension = Dimension;
            diagonal = new double[dimension];
        }

        public DiagonalMatrix(double[] Diagonal)
        {
            dimension = Diagonal.Length;
            diagonal = (double[])Diagonal.Clone();
        }

        public DiagonalMatrix(DiagonalMatrix from)
            : this(from.diagonal)
        {
        }

        #endregion

        //занулити елементи матриці
        public void NullMe()
        {
            for (int i = 0; i < dimension; ++i)
                diagonal[i] = 0;
        }

        public DiagonalMatrix CopyOfMe()
        {
            DiagonalMatrix ret = new DiagonalMatrix(this);
            return ret;
        }

        public int Width
        {
            get { return dimension; }
        }

        public int Height
        {
            get { return dimension; }
        }

        //простий індексатор
        public double this[int h, int w]
        {
            get
            {
                if (h >= 0 & h < dimension & w >= 0 & w < dimension)
                {
                    if (h == w)
                        return diagonal[h];
                    else
                        return 0;
                }
                else
                    throw new Exception("Indices out of Matrix dimensions!");
            }
            set
            {
                if (h >= 0 & h < dimension & w >= 0 & w < dimension)
                {
                    if (h == w)
                        diagonal[h] = value;
                    else
                        throw new Exception("Can not change non-diagonal elements! Use usual matrix.");
                }                 
                else
                    throw new Exception("Indices out of Matrix dimensions!");
            }
        }

        //рядковий індексатор
        public double[] this[int index]
        {
            get
            {
                if (index >= 0 & index < dimension)
                {
                    double[] line = new double[dimension];
                    line[index] = diagonal[index];
                    return line;
                }
                else
                    throw new Exception("Index out of Matrix dimensions!");
            }
            set
            {
                if (index >= 0 & index < dimension & value.Length == dimension)
                {
                    CheckLine(value, index);
                    diagonal[index] = value[index];
                }
                else
                    throw new Exception("Index out of Matrix dimensions!");
            }
        }

        private void CheckLine(double[] line, int index)
        {
            for (int i = 0; i < line.Length; ++i)
            {
                if (line[i] != 0)
                    if (i != index)
                        throw new NotSupportedException("Can not change non-diagonal elements! Use usual matrix.");
            }
        }

        #region Прохід по матриці
        public System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < dimension; ++i)
                yield return diagonal[i];                
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public static DiagonalMatrix Get_E(int N)
        {
            DiagonalMatrix E = new DiagonalMatrix(N);
            for (int i = 0; i < N; ++i)
                E.diagonal[i] = 1;
            return E;
        }

        //повертає окремі рвані масиви рядків
        public double[][] GetRows()
        {
            double[][] M = new double[dimension][];
            for (int i = 0; i < dimension; ++i)
            {
                M[i] = new double[dimension];
                M[i][i] = diagonal[i];
            }
            return M;
        }

        //повертає масив колонок
        public double[][] GetColumns()
        {
            double[][] M = new double[dimension][];
            int i = 0;
            for (; i < dimension; ++i)
            {
                M[i] = new double[dimension];
                M[i][i] = diagonal[i];
            }
            return M;
        }

        #region Операції
        //множення матриці на вектор
        public double[] MulVector(double[] vector)
        {
            if (dimension != vector.Length)
                throw new Exception("Can not multiply matrix and vector! Different sizes!");
            double[] result = new double[dimension];
            for (int i = 0; i < dimension; ++i)
            {
                result[i] = diagonal[i] * vector[i];
            }
            return result;
        }

        public void MulDouble(double mulWhat)
        {
            for (int i = 0; i < dimension; ++i)
                diagonal[i] *= mulWhat;
        }

        public void AddDouble(double addWhat)
        {
            for (int i = 0; i < dimension; ++i)
                diagonal[i] += addWhat;
        }

        //повертає слід матриці
        public double Track()
        {
            double track = 0;
            for (int i = 0; i < dimension; ++i)
                track += diagonal[i];
            return track;
        }
        #endregion

        #region Зчитування

        public void ConsoleRead()
        {
            throw new NotSupportedException("Operation is not defined!");
        }

        public void ConsoleReadWithParsing()
        {
            throw new NotSupportedException("Operation is not defined!");
        }

        //зчитування рядка матриці із стрічки-рівняння
        public double ReadRow(string rowString, int RowIndex)
        {
            throw new NotSupportedException("Operation is not defined!");
        }

        //зчитування всіх рядків
        public void ReadAllRows(string[] rows)
        {
            throw new NotSupportedException("Operation is not defined!");
        }

        //зчитування матриці з потоку
        public void ReadFromStreamWithParsing(TextReader tr)
        {
            throw new NotSupportedException("Operation is not defined!");
        }

        //зчитування матриці з потоку
        public void ReadFromStream(TextReader tr)
        {
            throw new NotSupportedException("Operation is not defined!");
        }

        #endregion

        #region Запис

        //друкування матриці
        public void Print()
        {
            WriteToStream(Console.Out);
        }

        public override string ToString()
        {
            string Result = "";
            for (int i = 0; i < dimension; ++i)
            {
                for (int j = 0; j < dimension; ++j)
                {
                    if (i != j)
                        Result += "0 ";
                    else
                        Result += diagonal[j] + " ";
                }
                Result += "\r\n";
            }
            return Result;
        }

        public void WriteToStream(TextWriter tw)
        {
            tw.WriteLine(this.ToString());
        }

        #endregion
    }
}
