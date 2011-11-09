using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SLAR
{
    public class SLARSolver
    {
        //ссилка на базовий клас
        protected basic_SLAR SLAR;
        //засіб для читання з потоків
        protected SLARReader slarReader;
        //засіб для запису у потік
        protected SLARWriter slarWriter;
        //час, витрачений для розрахунків(у секундах)
        protected double timeElapsed;

        //простий конструктор
        public SLARSolver()
        {
            SLAR = new basic_SLAR();
            slarReader = new SLARReader();
            slarWriter = new SLARWriter();
        }       

        protected void GetNumbers(out int EquationNumber, out int VariableNumber)
        {
            Console.WriteLine("Enter number of equations:");
            EquationNumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter number of variables:");
            VariableNumber = Convert.ToInt32(Console.ReadLine());
        }

        public virtual void FillSLAR()
        {
            Console.WriteLine("Select SLAR type to solve:");
            Console.WriteLine("1. 'g' for Gauss method.");
            Console.WriteLine("2. 's' for SQRT method.");
            Console.WriteLine("3. 'o' for Orthogonalization method.");
            Console.WriteLine("4. 'l' for LU method.");
            Console.WriteLine("5. 'd' for Driving method.");
            Console.WriteLine("6. 'z' for Gauss-Zeidel method.");
            Console.WriteLine("7. 'j' for Joined gradients method.");
            Console.WriteLine("8. 'm' for minimal deviation method.");
            Console.Write(Environment.NewLine + ">:");
            char ch = Convert.ToChar(Console.ReadLine());
            TextReader tr = null;
            switch (ch)
            {
                case 'g':
                case 'G':
                    tr = SelectReading();
                    Console.WriteLine("Expecting of entering SLAR:");
                    SLAR = (Gauss_SLAR)slarReader.ReadSLAR(tr, typeof(Gauss_SLAR));
                    break;
                case 's':
                case 'S':
                    tr = SelectReading();
                    Console.WriteLine("Expecting of entering SLAR:");
                    SLAR = (sqrt_SLAR)slarReader.ReadSLAR(tr, typeof(sqrt_SLAR));
                    break;
                case 'd':
                case 'D':
                    Console.WriteLine("Please, enter driving index:");
                    int drIndex = Convert.ToInt32(Console.ReadLine());
                    SLAR = new DriveAway_SLAR(DriveHelper.CalculateCoefficients, drIndex);
                    break;
                case 'l':
                case 'L':
                    tr = SelectReading();
                    Console.WriteLine("Expecting of entering SLAR:");
                    SLAR = (LU_SLAR)slarReader.ReadSLAR(tr, typeof(LU_SLAR));
                    break;
                case 'z':
                case 'Z':
                    tr = SelectReading();
                    Console.WriteLine("Expecting of entering SLAR:");
                    SLAR = (GZ_SLAR)slarReader.ReadSLAR(tr, typeof(GZ_SLAR));
                    break;
                case 'j':
                case 'J':
                    tr = SelectReading();
                    Console.WriteLine("Expecting of entering SLAR:");
                    SLAR = (JGrad_SLAR)slarReader.ReadSLAR(tr, typeof(JGrad_SLAR));
                    break;
                case 'o':
                case 'O':
                    tr = SelectReading();
                    Console.WriteLine("Expecting of entering SLAR:");
                    SLAR = (Ortho_SLAR)slarReader.ReadSLAR(tr, typeof(Ortho_SLAR));
                    break;
                case 'm':
                case 'M':
                    tr = SelectReading();
                    Console.WriteLine("Expecting of entering SLAR:");
                    SLAR = (MinDeviation_SLAR)slarReader.ReadSLAR(tr, typeof(MinDeviation_SLAR));
                    break;
                default:
                    throw new Exception("There no such type of method!");
            }
        }

        public virtual void FillSLAR(string format)
        {
            string[] directives = format.Split('|');
            char ch = Convert.ToChar(directives[0]);
            TextReader tr = null;
            switch (ch)
            {
                case 'g':
                case 'G':
                    tr = SelectParameterReading(directives);                    
                    SLAR = (Gauss_SLAR)slarReader.ReadSLAR(tr, typeof(Gauss_SLAR));
                    break;
                case 's':
                case 'S':
                    tr = SelectParameterReading(directives);                    
                    SLAR = (sqrt_SLAR)slarReader.ReadSLAR(tr, typeof(sqrt_SLAR));
                    break;
                case 'd':
                case 'D':
                    int drIndex = Convert.ToInt32(directives[1]);
                    SLAR = new DriveAway_SLAR(DriveHelper.CalculateCoefficients, drIndex);
                    break;
                case 'l':
                case 'L':
                    tr = SelectParameterReading(directives);                    
                    SLAR = (LU_SLAR)slarReader.ReadSLAR(tr, typeof(LU_SLAR));
                    break;
                case 'z':
                case 'Z':
                    tr = SelectParameterReading(directives);                    
                    SLAR = (GZ_SLAR)slarReader.ReadSLAR(tr, typeof(GZ_SLAR));
                    break;
                case 'j':
                case 'J':
                    tr = SelectParameterReading(directives);                    
                    SLAR = (JGrad_SLAR)slarReader.ReadSLAR(tr, typeof(JGrad_SLAR));
                    break;
                case 'o':
                case 'O':
                    tr = SelectParameterReading(directives);                    
                    SLAR = (Ortho_SLAR)slarReader.ReadSLAR(tr, typeof(Ortho_SLAR));
                    break;
                case 'm':
                case 'M':
                    tr = SelectParameterReading(directives);                    
                    SLAR = (MinDeviation_SLAR)slarReader.ReadSLAR(tr, typeof(MinDeviation_SLAR));
                    break;
                default:
                    throw new Exception("There no such type of method!");
            }
        }

        protected virtual TextReader SelectParameterReading(string[] parameters)
        {            
            char ch = Convert.ToChar(parameters[1]);
            switch (ch)
            {
                case 'c':
                case 'C':
                    return Console.In;
                case 'f':
                case 'F':                    
                    string FileName = parameters[2];
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

        public virtual void Solve()
        {            
            long ctrl1 = 0, ctrl2 = 0, freq = 0;
            if (QueryPerformanceCounter(ref ctrl1) != 0)
            {
                SLAR.SolveSLAR();
                QueryPerformanceCounter(ref ctrl2);
                QueryPerformanceFrequency(ref freq);
                timeElapsed = (ctrl2 - ctrl1) * 1.0 / freq;
            }
            else
            {
                SLAR.SolveSLAR();
            }            
        }

        public virtual void SolveForConsole()
        {
            Console.WriteLine();
            Console.WriteLine("Solving process started...");
            Console.WriteLine();

            Solve();

            Console.WriteLine();
            Console.WriteLine("SLAR is solved.");
            Console.WriteLine();
        }

        public void PrintResults()
        {
            SLAR.PrintResults();
            Console.WriteLine();
            Console.WriteLine("Solution Matches == " + SLAR.SolutionMatches());            
        }

        public void PrintTime()
        {
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Time Elapsed == " + TimeElapsed + " seconds.");
            Console.WriteLine("--------------------------------------------");
        }

        public void PrintMatrix()
        {
            Console.WriteLine("Print input coefficient Matrix? (y/n)");
            char ch = Convert.ToChar(Console.ReadLine());
            switch (ch)
            {
                case 'y':
                case 'Y':                    
                    SLAR.PrintCurrent();
                    break;
            }
            Console.WriteLine();
            if (SLAR is Gauss_SLAR)
            {
                Console.WriteLine("Print trianguled coefficient Matrix? (y/n)");
                ch = Convert.ToChar(Console.ReadLine());
                switch (ch)
                {
                    case 'y':
                    case 'Y':
                        (SLAR as Gauss_SLAR).PrintTrianguled();
                        break;
                }
                Console.WriteLine();
            }
        }

        public void PrintProductForDebug()
        {
            VectorOperations.PrintVector(SLAR.GetProduct());
        }

        protected TextReader SelectReading()
        {
            Console.WriteLine("Reading of SLAR. Enter 'c' if you want to enter input data from console. If you want to get it from file, enter 'f'.");
            Console.Write(">:");
            char ch = Convert.ToChar(Console.ReadLine());
            switch (ch)
            {
                case 'c':
                case 'C':
                    return Console.In;                    
                case 'f':
                case 'F':
                    Console.WriteLine("Please, enter FilePath.");
                    Console.Write(">:");
                    string FileName = Console.ReadLine();
                    FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    TextReader tr = new StreamReader(fs);
                    return tr;                    
                default:
                    throw new NotSupportedException("Such way of reading doesn't exist!");
            }
        }

        #region Інформація про виконання

        public double TimeElapsed
        {
            get { return timeElapsed; }
        }

        public bool SolutionMatches
        {
            get { return SLAR.SolutionMatches(); }
        }

        public double[] Product_Ax
        {
            get { return SLAR.GetProduct(); }
        }

        public double[] ResultX
        {
            get { return SLAR.vectorX; }
        }

        #endregion
    }
}
