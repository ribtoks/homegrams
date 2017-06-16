using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SLAR
{
    //реалізація стандартної системи рівнянь [A*x == b]
    public class basic_SLAR
    {
        #region Дані
        //основні дані
        protected Matrix A; //матриця коефіцієнтів
        protected double[] b; //відомий вектор
        protected double[] x; //невідомий вектор
        
        //додаткові дані
        protected double epsilon; //точність, врахована при обчисленнях       

        #endregion

        #region Конструктори
        public basic_SLAR()
        {
            A = new Matrix();
            b = null;
            x = null;            
            epsilon = Double.Epsilon;
        }

        public basic_SLAR(int equation_number, int variable_number)
        {
            A = new Matrix(variable_number, equation_number);                      
            b = new double[equation_number];
            x = new double[variable_number];            
            epsilon = 0.00000001; //[10]^-8            
        }

        public basic_SLAR(int Dimension)
            : this(Dimension, Dimension)
        {
        }

        //конструктор з готової матриці
        public basic_SLAR(Matrix matrixA, double[] vectorB)
        {
            A = new Matrix(matrixA);
            b = (double[])vectorB.Clone();
            x = new double[b.Length];
            epsilon = 0.00000001; //[10]^-8            
        }

        //конструктор з іншої basic_SLAR
        public basic_SLAR(basic_SLAR from)
        {
            A = new Matrix(from.A);
            b = (double[])from.b.Clone();
            x = new double[b.Length];
            epsilon = from.epsilon;            
            x = (double[])from.x.Clone();
        }
        #endregion

        //перевизначити СЛАР
        public void ReSetSLAR(Matrix matrixA, double[] vectorB)
        {
            A = new Matrix(matrixA);
            b = (double[])vectorB.Clone();
            x = new double[b.Length];
            epsilon = 0.00000001; //[10]^-8            
        }

        //можна встановити точність, яку хочеш
        public double Epsilon
        {
            get { return epsilon; }
            set { epsilon = value; }
        }        

        //повертає вектор - розв'язок
        public double[] vectorX
        {
            get { return x; }
        }

        //змінити лише вектор
        public void ChangeVectorB(double[] newVectorB)
        {
            b = (double[])newVectorB.Clone();            
        }

        //просто друкує вхідну матрицю
        public virtual void PrintCurrent()
        {
            if (A != null)
            {
                for (int i = 0; i < A.Height; ++i)
                {
                    for (int j = 0; j < A.Width; ++j)
                    {
                        Console.Write(A[i, j] + " ");
                    }
                    Console.WriteLine(" | " + b[i]);
                }
            }
        }
        
        //функція для перевизначання у класах-наслідниках
        public virtual void PrintResults()
        {
            if (x != null)
                Console.WriteLine("Vector X: " + VectorOperations.VectorToString(x));
        }

        #region Розв'язування

        //функція для перевизначання у класах-наслідниках
        public virtual void SolveSLAR()
        {
            throw new NotSupportedException("Can not invoke SolveSLAR for basicSLAR!");
        }

        #endregion

        #region DEBUG

        //чи розв'язок правильний
        public virtual bool SolutionMatches()
        {
            double[] res = A.MulVector(x);
            bool state = true;
            for (int i = 0; i < A.Height; ++i)
            {
                if (Math.Abs(res[i] - b[i]) > epsilon)
                {
                    state = false;
                    break;
                }
            }
            return state;
        }        

        //повертає добуток матриці на вектор-роз'язок
        public virtual double[] GetProduct()
        {
            return A.MulVector(x);
        }

        #endregion

        #region Зчитування

        public virtual void ReadSLAR()
        {
            SLARReader sr = new SLARReader();
            Console.WriteLine("Reading of SLAR. Enter 'c' if you want to enter input data from console. If you want to get it from file, enter 'f'.");
            Console.Write(">:");
            char ch = Convert.ToChar(Console.ReadLine());
            switch (ch)
            {
                case 'c':
                case 'C':
                    sr.ReadSLAR(Console.In, this.GetType());
                    break;
                case 'f':
                case 'F':
                    Console.WriteLine("Please, enter FilePath.");
                    Console.Write(">:");
                    string FileName = Console.ReadLine();
                    FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    TextReader tr = new StreamReader(fs);
                    sr.ReadSLAR(tr, this.GetType());
                    break;
            }
            ReSetSLAR(sr.Obj);
        }

        public virtual void ReSetSLAR(object obj)
        {
            basic_SLAR bs = (basic_SLAR)obj;
            if (bs == null)
                throw new NotSupportedException("Can not unbox object to basic_SLAR!");
            ReSetSLAR(bs.A, bs.b);
            bs = null;
        }


        #endregion

        #region Запис

        public virtual void WriteResultsToStream(TextWriter tw)
        {
            tw.WriteLine(VectorOperations.VectorToString(x));
            tw.WriteLine();
        }        

        public virtual void WriteInputToStream(TextWriter tw)
        {
            if (A != null)
            {
                for (int i = 0; i < A.Height; ++i)
                {
                    for (int j = 0; j < A.Width; ++j)
                    {
                        tw.Write(A[i, j] + " ");
                    }
                    tw.WriteLine(" | " + b[i]);
                }
            }
        }

        #endregion
    }
}
