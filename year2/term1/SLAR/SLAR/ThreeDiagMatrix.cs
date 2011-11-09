using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SLAR
{
    public class ThreeDiagMatrix : System.Collections.IEnumerable, StandartMatrixInterface, IOMatrixInterface, ConsoleMatrixInterface
    {
        //розмірність
        int dimension;
        //і три головні діагоналі
        protected double[] mainDiagonal;
        protected double[] lowerDiagonal;
        protected double[] upperDiagonal;

        #region Конструктори

        public ThreeDiagMatrix()
        {
            dimension = 0;
            mainDiagonal = null;
            lowerDiagonal = null;
            upperDiagonal = null;
        }

        public ThreeDiagMatrix(int Dimension)
        {
            dimension = Dimension;
            mainDiagonal = new double[dimension];
            lowerDiagonal = new double[dimension - 1];
            upperDiagonal = new double[dimension - 1];
        }

        public ThreeDiagMatrix(double[] MainDiagonal, double[] LowerDiagonal, double[] UpperDiagonal)
        {
            if (LowerDiagonal.Length == UpperDiagonal.Length)
            {
                if (LowerDiagonal.Length + 1 == MainDiagonal.Length)
                {
                    dimension = MainDiagonal.Length;
                    mainDiagonal = (double[])MainDiagonal.Clone();
                    lowerDiagonal = (double[])LowerDiagonal.Clone();
                    upperDiagonal = (double[])UpperDiagonal.Clone();
                }
                else
                    throw new Exception("Wrong size of input vectors!");
            }
            else
                throw new Exception("Wrong size of input vectors!");
        }

        public ThreeDiagMatrix(ThreeDiagMatrix from)
            : this(from.mainDiagonal, from.lowerDiagonal, from.upperDiagonal)
        {
        }

        public ThreeDiagMatrix(Matrix A)
        {
            if (A.Height != A.Width)
                throw new Exception("Can not create three diagonal matrix from not-square matrix!");
            dimension = A.Height;

            mainDiagonal = new double[dimension];
            lowerDiagonal = new double[dimension - 1];
            upperDiagonal = new double[dimension - 1]; 
           
            int N = dimension;
            mainDiagonal[0] = A[0, 0];
            upperDiagonal[0] = A[0, 1];
            lowerDiagonal[dimension - 2] = A[N - 1, N - 2];
            mainDiagonal[dimension - 1] = A[N - 1, N - 1];
            //головний цикл обрахунків
            for (int i = 1; i < N - 1; ++i)
            {
                lowerDiagonal[i - 1] = A[i, i - 1];
                mainDiagonal[i] = A[i, i];
                upperDiagonal[i] = A[i, i + 1];
            } //loop
        }

        #endregion

        //занулити елементи матриці
        public void NullMe()
        {
            int i = 0;
            for (; i < dimension; ++i)
            {
                mainDiagonal[i] = 0;                
            }
            for (i = 0; i < dimension - 1; ++i)
            {
                lowerDiagonal[i] = 0;
                upperDiagonal[i] = 0;
            }
        }

        public ThreeDiagMatrix CopyOfMe()
        {
            ThreeDiagMatrix ret = new ThreeDiagMatrix(this);
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
                        return mainDiagonal[h];
                    if (h - 1 == w)
                        return lowerDiagonal[w];
                    if (w - 1 == h)
                        return upperDiagonal[h];
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
                        mainDiagonal[h] = value;
                    else
                        if (h - 1 == w)
                            lowerDiagonal[w] = value;
                        else
                            if (w - 1 == h)
                                upperDiagonal[h] = value;
                            else
                            {
                                if (value != 0)
                                    throw new Exception("Can not change non-diagonal elements! Use usual matrix.");
                            }
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
                    if (index == 0)
                    {
                        line[0] = mainDiagonal[0];
                        line[1] = upperDiagonal[0];
                        return line;
                    }
                    if (index == dimension - 1)
                    {
                        line[dimension - 2] = lowerDiagonal[lowerDiagonal.Length - 1];
                        line[dimension - 1] = mainDiagonal[dimension - 1];
                        return line;
                    }
                    line[index] = mainDiagonal[index];
                    line[index + 1] = upperDiagonal[index];
                    line[index - 1] = lowerDiagonal[index - 1];
                    return line;
                }
                else
                    throw new Exception("Index out of Matrix dimensions!");
            }
            set
            {
                if (index >= 0 & index < dimension & value.Length == dimension)
                {                    
                    mainDiagonal[index] = value[index];
                    if (index == 0)
                    {
                        upperDiagonal[0] = value[1];
                    }
                    else
                        if (index == dimension - 1)
                        {
                            lowerDiagonal[lowerDiagonal.Length - 1] = value[dimension - 2];
                        }
                        else
                        {
                            upperDiagonal[index] = value[index + 1];
                            lowerDiagonal[index - 1] = value[index - 1];
                        }
                      
                }
                else
                    throw new Exception("Index out of Matrix dimensions!");
            }
        }
         

        #region Прохід по матриці
        public System.Collections.IEnumerator GetEnumerator()
        {
            yield return mainDiagonal[0];
            yield return upperDiagonal[0];
            for (int i = 1; i < dimension - 1; ++i)
            {
                yield return lowerDiagonal[i - 1];
                yield return mainDiagonal[i];
                yield return upperDiagonal[i];
            }
            yield return lowerDiagonal[lowerDiagonal.Length - 1];
            yield return mainDiagonal[mainDiagonal.Length - 1];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public static ThreeDiagMatrix Get_E(int N)
        {
            ThreeDiagMatrix E = new ThreeDiagMatrix(N);
            for (int i = 0; i < N; ++i)
                E.mainDiagonal[i] = 1;
            return E;
        }

        //повертає окремі рвані масиви рядків
        public double[][] GetRows()
        {
            return ToMatrix().GetRows();
        }

        //повертає масив колонок
        public double[][] GetColumns()
        {
            return ToMatrix().GetColumns();
        }

        public Matrix ToMatrix()
        {
            Matrix A = new Matrix(dimension);
            int N = dimension;
            A[0, 0] = mainDiagonal[0];
            A[0, 1] = upperDiagonal[0];
            A[N - 1, N - 2] = lowerDiagonal[lowerDiagonal.Length - 1];
            A[N - 1, N - 1] = mainDiagonal[mainDiagonal.Length - 1];
            //головний цикл обрахунків
            for (int i = 1; i < N - 1; ++i)
            {
                A[i, i - 1] = lowerDiagonal[i - 1];
                A[i, i] = mainDiagonal[i];
                A[i, i + 1] = upperDiagonal[i];
            } //loop
            return A;
        }

        public void BuildFromMatrix(Matrix A)
        {
            int N = A.Height;
            mainDiagonal = new double[N];
            lowerDiagonal = new double[N - 1];
            upperDiagonal = new double[N - 1];

            mainDiagonal[0] = A[0, 0];
            upperDiagonal[0] = A[0, 1];
            lowerDiagonal[lowerDiagonal.Length - 1] = A[N - 1, N - 2];
            mainDiagonal[mainDiagonal.Length - 1] = A[N - 1, N - 1];
            //головний цикл обрахунків
            for (int i = 1; i < N - 1; ++i)
            {
                lowerDiagonal[i - 1] = A[i, i - 1];
                mainDiagonal[i] = A[i, i];
                upperDiagonal[i] = A[i, i + 1];
            } //loop            
        }

        #region Операції

        //множення матриці на вектор
        public double[] MulVector(double[] vector)
        {
            return ToMatrix().MulVector(vector);
        }

        public void MulDouble(double mulWhat)
        {
            int i = 0;
            for (; i < dimension; ++i)
            {
                mainDiagonal[i] *= mulWhat;                
            }

            for (i = 0; i < dimension - 1; ++i)
            {
                upperDiagonal[i] *= mulWhat;
                lowerDiagonal[i] *= mulWhat;
            }
        }

        public void AddDouble(double addWhat)
        {
            int i = 0;
            for (; i < dimension; ++i)
            {
                mainDiagonal[i] += addWhat;
            }

            for (i = 0; i < dimension - 1; ++i)
            {
                upperDiagonal[i] += addWhat;
                lowerDiagonal[i] += addWhat;
            }
        }

        //повертає слід матриці
        public double Track()
        {
            double track = 0;
            for (int i = 0; i < dimension; ++i)
                track += mainDiagonal[i];
            return track;
        }

        #endregion

        #region Зчитування
        public void ConsoleRead()
        {
            Matrix A = new Matrix(dimension);
            A.ConsoleRead();
            BuildFromMatrix(A);
            A = null;
        }

        public void ConsoleReadWithParsing()
        {
            Matrix A = new Matrix(dimension);
            A.ConsoleReadWithParsing();
            BuildFromMatrix(A);
            A = null;
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
            Matrix A = new Matrix(dimension);
            A.ReadFromStreamWithParsing(tr);
            BuildFromMatrix(A);
            A = null;
        }

        //зчитування матриці з потоку
        public void ReadFromStream(TextReader tr)
        {
            Matrix A = new Matrix(dimension);
            A.ReadFromStream(tr);
            BuildFromMatrix(A);
            A = null;
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
            return ToMatrix().ToString();
        }

        public void WriteToStream(TextWriter tw)
        {
            tw.WriteLine(this.ToString());
        }

        #endregion

    }
}
