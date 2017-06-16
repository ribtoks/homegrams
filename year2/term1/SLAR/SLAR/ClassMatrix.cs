using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

namespace SLAR
{
    public class Matrix : System.Collections.IEnumerable, StandartMatrixInterface, IOMatrixInterface, ConsoleMatrixInterface
    {
        protected double[][] MArray; //у зубчастого масива швидший доступ до елементів і повільніший конструктор
        protected int DimWidth;
        protected int DimHeight;
        
        #region Конструктори
        
        public Matrix()
        {
            MArray = null;
            DimHeight = 0;
            DimWidth = 0;
        }
       
        public Matrix(int width, int height)
        {
            DimHeight = height;
            DimWidth = width;
            MArray = new double[height][];
            for (int i = 0; i < height; ++i)
                MArray[i] = new double[width];            
        }

        public Matrix(int Dimension)
            : this(Dimension, Dimension)
        {
        }

        public Matrix(Matrix CopyFrom)
        {
            DimHeight = CopyFrom.DimHeight;
            DimWidth = CopyFrom.DimWidth;
            //для ссилок не працюватиме!!!
            MArray = new double[DimHeight][];
            for (int k = 0; k < DimHeight; ++k)
                MArray[k] = new double[CopyFrom.MArray[k].Length];

            for (int i = 0; i < MArray.Length; ++i)
                for (int j = 0; j < CopyFrom.MArray[i].Length; ++j)
                    MArray[i][j] = CopyFrom.MArray[i][j];
        }

        public Matrix(double[][] Data, bool IsColumns)
        {
            if (IsColumns == true)
            {
                DimHeight = Data[0].Length;
                DimWidth = Data.Length;
                MArray = new double[DimHeight][];
                int i = 0;
                for (; i < DimHeight; ++i)
                    MArray[i] = new double[DimWidth];
                for (i = 0; i < DimHeight; ++i)
                    for (int j = 0; j < DimWidth; ++j)
                        MArray[i][j] = Data[j][i];
            }
            else
            {
                DimHeight = Data.Length;
                DimWidth = Data[0].Length;
                MArray = new double[Data.Length][];
                for (int i = 0; i < Data.Length; ++i)
                    MArray[i] = (double[])Data[i].Clone();
            }            
        }        

        #endregion

        //занулити елементи матриці
        public void NullMe()
        {
            for (int i = 0; i < DimHeight; ++i)
                for (int j = 0; j < DimWidth; ++j)
                    MArray[i][j] = 0;
        }

        public Matrix CopyOfMe()
        {
            Matrix ret = new Matrix(this);
            return ret;
        }

        public int Width
        {
            get { return DimWidth; }
        }

        public int Height
        {
            get { return DimHeight; }
        }

        //простий індексатор
        public double this[int h, int w]
        {
            get
            {
                if (h >= 0 & h < DimHeight & w >= 0 & w < DimWidth)
                    return MArray[h][w];
                else
                    throw new Exception("Indices out of Matrix dimensions!");
            }
            set
            {
                if (h >= 0 & h < DimHeight & w >= 0 & w < DimWidth)
                    MArray[h][w] = value;
                else
                    throw new Exception("Indices out of Matrix dimensions!");
            }
        }

        //рядковий індексатор
        public double[] this[int index]
        {
            get
            {
                if (index >= 0 & index < DimHeight)
                {
                    return MArray[index];
                }
                else
                    throw new Exception("Index out of Matrix dimensions!");
            }
            set
            {
                if (index >= 0 & index < DimHeight & value.Length == DimWidth)
                {
                    MArray[index] = value;                    
                }
                else
                    throw new Exception("Index out of Matrix dimensions!");
            }
        }

        #region Прохід по матриці
        public System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < DimHeight; ++i)
                for (int j = 0; j < DimWidth; ++j)
                {
                    yield return MArray[i][j];
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
                E.MArray[i][i] = 1;
            return E;
        }
                
        //повертає окремі рвані масиви рядків
        public double[][] GetRows()
        {
            double[][] M = new double[MArray.Length][];
            for (int i = 0; i < MArray.Length; ++i)
                M[i] = (double[])MArray[i].Clone();
            return M;
        }

        //повертає масив колонок
        public double[][] GetColumns()
        {
            double[][] M = new double[MArray[0].Length][];
            int i = 0;
            for (; i < MArray[0].Length; ++i)
                M[i] = new double[MArray.Length];
            for (int j = 0; j < DimWidth; ++j)
                for (i = 0; i < DimHeight; ++i)
                    M[j][i] = MArray[i][j];
            return M;
        }                

        #region Операції

        //множення матриці на вектор
        public double[] MulVector(double[] vector)
        {
            if (DimWidth != vector.Length)
                throw new Exception("Can not multiply matrix and vector! Different sizes!");
            double[] result = new double[DimHeight];
            for (int i = 0; i < DimHeight; ++i)
            {
                result[i] = VectorOperations.ScalarProduct(vector, MArray[i]);
            }
            return result;
        }

        public void MulDouble(double mulWhat)
        {
            for (int i = 0; i < DimHeight; ++i)
                for (int j = 0; j < DimWidth; ++j)
                {
                    MArray[i][j] *= mulWhat;
                }
        }

        public void AddDouble(double addWhat)
        {
            for (int i = 0; i < DimHeight; ++i)
                for (int j = 0; j < DimWidth; ++j)
                {
                    MArray[i][j] += addWhat;
                }
        }

        //обмін рядів
        public void SwapRows(int i, int j)
        {
            for (int k = 0; k < DimWidth; ++k)
            {
                double temp = MArray[i][k];
                MArray[i][k] = MArray[j][k];
                MArray[j][k] = temp;
            }
        }

        //обмін стовпців
        public void SwapColumns(int i, int j)
        {
            for (int k = 0; k < DimHeight; ++k)
            {
                double temp = MArray[k][i];
                MArray[k][i] = MArray[k][j];
                MArray[k][j] = temp;
            }
        }

        //повертає слід матриці
        public double Track()
        {
            double track = 0;
            for (int i = 0; i < DimHeight; ++i)
                track += MArray[i][i];
            return track;
        }

        #endregion

        #region Зчитування матриці
        public void ConsoleRead()
        {
            Console.WriteLine("Please, enter elements of " + DimHeight + " * " + DimWidth + " double matrix(ex. 3,141596):");
            ReadFromStream(Console.In);
            Console.WriteLine("Reading complete.");
        }

        public void ConsoleReadWithParsing()
        {
            Console.WriteLine("Please, enter " + DimHeight + " equations:");
            ReadFromStreamWithParsing(Console.In);
            Console.WriteLine("Reading complete.");
        } 
        
        //зчитування рядка матриці із стрічки-рівняння
        public double ReadRow(string rowString, int RowIndex)
        {
            double d;
            MArray[RowIndex] = EquationParser.Parse(rowString, DimWidth, out d);
            return d;
        }

        //зчитування всіх рядків
        public void ReadAllRows(string[] rows)
        {
            double d;
            for (int i = 0; i < rows.Length; ++i)
            {                
                MArray[i] = EquationParser.Parse(rows[i], DimWidth, out d);
            }
        }

        //зчитування матриці з потоку
        public void ReadFromStreamWithParsing(TextReader tr)
        {
            for (int i = 0; i < DimHeight; ++i)
            {
                string s = tr.ReadLine();
                double d;
                MArray[i] = EquationParser.Parse(s, DimWidth, out d);
            }
        }

        //зчитування матриці з потоку
        public void ReadFromStream(TextReader tr)
        {
            for (int i = 0; i < DimHeight; ++i)
            {
                string s = tr.ReadLine();
                string[] numbers = s.Split(' ');
                for (int j = 0; j < numbers.Length; ++j)
                    MArray[i][j] = Double.Parse(numbers[j]);
            }
        }
        #endregion

        #region Запис матриці
        //друкування матриці
        public void Print()
        {
            WriteToStream(Console.Out);
        }

        public override string ToString()
        {
            string Result = "";
            for (int i = 0; i < DimHeight; ++i)
            {
                for (int j = 0; j < DimWidth; ++j)
                {
                    Result += MArray[i][j] + " ";
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
