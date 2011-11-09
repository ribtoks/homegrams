using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLAR;
using System.IO;

namespace ProperValueProblem
{
    class ProperValueBasic
    {
        #region Дані
        
        //матриця коефіцієнтів
        protected Matrix A;
        
        //точність обчислень
        protected double epsilon;

        protected bool isRandomStartVector = false;        

        protected SLAR_Diagnostics slar_d;

        #endregion

        #region Конструктори
        public ProperValueBasic()
        {
            A = null;
            epsilon = 0.0000001;
        }

        public ProperValueBasic(int Dimension)
        {
            A = new Matrix(Dimension, Dimension);
            epsilon = 0.00000001;
        }

        public ProperValueBasic(int Dimension, double Epsilon)
        {
            A = new Matrix(Dimension, Dimension);
            epsilon = Epsilon;
        }

        public ProperValueBasic(Matrix From, double Epsilon)
        {
            A = new Matrix(From);
            epsilon = Epsilon;
        }
        #endregion

        protected double Epsilon
        {
            get { return epsilon; }
            set { epsilon = value; }
        }

        protected bool IsRandomStartVector
        {
            get { return isRandomStartVector; }
            set { isRandomStartVector = value; }
        }

        //метод для знаходження власних значень
        public virtual void SolveProperProblem()
        {
            throw new NotSupportedException("Can not invoke SolveProperProblem for ProperValueBasic!");
        }

        //показ результатів
        public virtual void PrintResults()
        {
            throw new NotSupportedException("Can not invoke PrintResults for ProperValueBasic!");
        }

        public virtual void ReadProblem()
        {
            Console.WriteLine("Please, enter Epsilon, that you want(default is 0,00000001):");
            string str = Console.ReadLine();
            double eps = 0.00000001;
            if (str.Length != 0)
                eps = Double.Parse(str);
            epsilon = eps;
            Console.WriteLine();
            //now reading matrix from some stream
            Console.WriteLine("Reading of Matrix. Enter 'c' if you want to enter input data from console. If you want to get it from file, enter 'f'.");
            Console.Write(">:");
            char ch = Convert.ToChar(Console.ReadLine());
            switch (ch)
            {
                case 'c':
                case 'C':
                    Console.WriteLine("Expecting of matrix...");
                    A.ReadFromStream(Console.In);
                    break;
                case 'f':
                case 'F':
                    Console.WriteLine("Please, enter FilePath.");
                    Console.Write(">:");
                    string FileName = Console.ReadLine();
                    FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    TextReader tr = new StreamReader(fs);
                    A.ReadFromStream(tr);
                    break;
            }
        }

        public virtual void SetAll(double Eps, Matrix mA)
        {
            epsilon = Eps;
            A = new Matrix(A);
        }
    }
}
    
