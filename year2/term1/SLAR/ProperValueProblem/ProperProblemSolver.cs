using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SLAR;

namespace ProperValueProblem
{
    class ProperProblemSolver
    {
        //ссилка на базовий (для власних значень) клас
        protected ProperValueBasic basicPr;

        //час, витрачений для розрахунків(у секундах)
        double timeElapsed;

        public double TimeElapsed
        {
            get { return timeElapsed; }
        }


        public ProperProblemSolver()
        {
            basicPr = new ProperValueBasic();            
        }

        public virtual void FillSolver()
        {
            Console.WriteLine("Select method type to solve with:");
            Console.WriteLine("1. 'b' for Back Iteration method.");
            Console.WriteLine("2. 'f' for Forward Iteration method.");
            Console.WriteLine("3. 'j' for Jacobi Turns method.");
            Console.Write(">:");
            char ch = Convert.ToChar(Console.ReadLine());
            int Number = 0;
            switch (ch)
            {
                case 'b':
                case 'B':
                    Console.WriteLine("Enter dimension of Matrix:");
                    Number = Convert.ToInt32(Console.ReadLine());
                    basicPr = new BackIterations_SP(Number);
                    break;
                case 'f':
                case 'F':
                    Console.WriteLine("Enter dimension of Matrix:");
                    Number = Convert.ToInt32(Console.ReadLine());
                    basicPr = new Forward_SP(Number);
                    break;
                case 'j':
                case 'J':
                    Console.WriteLine("Enter dimension of Matrix:");
                    Number = Convert.ToInt32(Console.ReadLine());
                    basicPr = new JacobiTurns(Number);
                    break;
            }
            basicPr.ReadProblem();
        }

        public virtual void FillSolver(string format)
        {
            string[] directives = format.Split('|');
            char ch = Convert.ToChar(directives[0]);
            int Number = 0;
            switch (ch)
            {
                case 'b':
                case 'B':                    
                    Number = Convert.ToInt32(directives[1]);
                    basicPr = new BackIterations_SP(Number);
                    break;
                case 'f':
                case 'F':
                    Number = Convert.ToInt32(directives[1]);
                    basicPr = new Forward_SP(Number);
                    break;
                case 'j':
                case 'J':
                    Number = Convert.ToInt32(directives[1]);
                    basicPr = new JacobiTurns(Number);
                    break;
            }
            TextReader tr = ChooseTextReader(directives);
            Matrix A = new Matrix(Number);
            if (directives[directives.Length - 2] == "c" | directives[directives.Length - 2] == "C")
                A.ReadFromStream(tr);
            else
                if (directives[directives.Length - 2] == "f" | directives[directives.Length - 2] == "F")
                    A.ReadFromStreamWithParsing(tr);
                else
                    throw new NotSupportedException("Can not read matrix!");
            double eps = 0.00000001;
            if (directives[directives.Length - 1].Length != 0)
                eps = Convert.ToDouble(directives[directives.Length - 1]);
            basicPr.SetAll(eps, A);              
        }

        protected virtual TextReader ChooseTextReader(string[] parameters)
        {
            char ch = Convert.ToChar(parameters[2]);
            switch (ch)
            {
                case 'c':
                case 'C':
                    return Console.In;
                case 'f':
                case 'F':
                    string FileName = parameters[3];
                    FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    TextReader tr = new StreamReader(fs);
                    return tr;
                default:
                    throw new NotSupportedException("Such way of reading doesn't exist!");
            }
        }


        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        extern static short QueryPerformanceCounter(ref long x);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        extern static short QueryPerformanceFrequency(ref long x);

        public void SolveProblem()
        {           
            long ctrl1 = 0, ctrl2 = 0, freq = 0;
            if (QueryPerformanceCounter(ref ctrl1) != 0)
            {
                basicPr.SolveProperProblem();
                QueryPerformanceCounter(ref ctrl2);
                QueryPerformanceFrequency(ref freq);
                timeElapsed = (ctrl2 - ctrl1) * 1.0 / freq;
            }
            else
            {
                basicPr.SolveProperProblem();
            }
            
        }

        public void ConsoleSolveProblem()
        {
            Console.WriteLine();
            Console.WriteLine("Solving process started...");
            Console.WriteLine();

            SolveProblem();

            Console.WriteLine();
            Console.WriteLine("Problem is solved.");
            Console.WriteLine();
        }

        public void PrintResults()
        {
            basicPr.PrintResults();
        }

        public void PrintTime()
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Time Elapsed == " + TimeElapsed + " seconds.");
            Console.WriteLine("--------------------------------------------");
        }
    }
}
