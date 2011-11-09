using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;

namespace SLAR
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("          Simple Help" + Environment.NewLine);
            Console.WriteLine("Format of input Matrix and vector:");
            Console.WriteLine("Size1 Size2 / Size - <width and height / width == height == Size>");
            Console.WriteLine("c/f - <coeficients/full> equations");
            Console.WriteLine("a11 a12 b1");
            Console.WriteLine("a21 a22 b2");
            Console.WriteLine();
            SLARSolver ss = new SLARSolver();
            try
            {
/*                FileStream fs = new FileStream("Equations.txt", FileMode.Open);
                TextReader console = Console.In;

                TextReader in_file = new StreamReader(fs);
                Console.SetIn(in_file);
                */
                ss.FillSLAR();
                ss.SolveForConsole();
                ss.PrintMatrix();
                ss.PrintResults();
                ss.PrintTime();
                Console.WriteLine();
                ss.PrintProductForDebug(); //добуток матриці на вектор-розв'язок
//                Console.SetIn(console);
            }
            catch(Exception ex)
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Error occured!");
                Console.WriteLine("--------------");
                Console.WriteLine(ex.Message);
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine(Environment.NewLine + "Program will close now!");
            }
            Console.ReadLine();
            
            #region Непотрібний код, який шкода викинути
            /* 
             * 
             * /*            Matrix A = new Matrix(5, 5);
            A.ReadFromStream(Console.In);
            int r = 0;
            TriangulingProtocol tp = MatrixOperations.ToTrianguled(A, ref A, out r, true, 0.00000001);
            A.Print();
            MatrixOperations.RestoreTrianguled(A, tp).Print();
            Console.ReadLine();
 
            LU_SLAR lu = new LU_SLAR();
            lu.ReadSLAR();
            lu.SolveSLAR();
            lu.PrintResults();
            Console.WriteLine(lu.SolutionMatches());
            Console.ReadLine();
   
             * 
             * int n = 4;
            basic_SLAR bs = new basic_SLAR(n);
            Matrix A = new Matrix(n, n);
            double[] b = new double[n];
            bs.ReadSLAR();
            
            A.Read();
           
            int r = 0;
            TriangulingProtocol tp = MatrixOperations.ToTrianguled(A, ref A, out r, true, 0.000000001);
            A.Print();
            bs.SolveSLAR();
            bs.PrintTrianguled();

            string str = Console.ReadLine();
            string[] numbers = str.Split(' ');
            for (int i = 0; i < n; ++i)
                b[i] = Double.Parse(numbers[i]);

            b = VectorOperations.GetTrianguledVector(b, tp, true);
            VectorOperations.PrintVector(b);
            Console.ReadLine();
            /*            SLARSolver ss = new SLARSolver();
                        ss.FillSLAR();
                        ss.Solve();
                        ss.PrintMatrix();
                        ss.PrintResults();
                        ss.PrintProductForDebug(); //добуток матриці на вектор-розв'язок
                        Console.ReadLine();
             * 
                        Matrix A = new Matrix(2, 2);
                        Console.ReadLine();
1 2 3 12 5
1 3 4 5 6
2 112 6 7 8
12 43 56 78 99

1 2 3 12
1 3 4 5
2 112 6 7
12 43 56 78
             */

            /* 
            JGrad_SLAR jg_slar = new JGrad_SLAR(3);
            
//            GZ_SLAR gz_slar = new GZ_SLAR(4);
            jg_slar.IsRandomStartVector = true;
            jg_slar.Epsilon = 0.000001;
            jg_slar.ReadSLAR();
//            gz_slar.SetAllByJacobi();
            jg_slar.SolveSLAR();
            jg_slar.PrintResults();           
//            Console.WriteLine(jg_slar.HasDiagonalAdvantage());
            Console.ReadLine();
*/
            /*
                                    Matrix A = new Matrix(3, 3);
                                    A.Read();
                                    int r = 0;
                                    TriangulingProtocol tp = MatrixOperations.ToTrianguled(A, ref A, out r);
                                    MatrixOperations.RestoreTrianguled(A, tp).Print();
                                    Console.ReadLine();
                         */
            /*            Matrix A = new Matrix(3, 3);
                        A.Read();
                        Matrix LR = new Matrix(A);
                        LR[0, 0] = 0;
                        LR[1, 1] = 0;
                        LR[2, 2] = 0;
                        Matrix D = new Matrix(3, 3);
                        D[0, 0] = A[0, 0];
                        D[1, 1] = A[1, 1];
                        D[2, 2] = A[2, 2];
                        D = MatrixOperations.FindReservedMatrix(D);
                        Matrix Res = MatrixOperations.Product(D, LR);
                        Console.WriteLine(Res.ToString());
                        Console.WriteLine(MatrixOperations.Product(MatrixOperations.GetTranspose(Res), Res).ToString());
                        Console.WriteLine(MatrixOperations.NormColumns(Res) + " columns");
                        Console.WriteLine(MatrixOperations.NormRows(Res) + " rows");
                        Console.ReadLine();
             */
            /*            Matrix A = new Matrix(3, 3);
                        A.Read();
                        Matrix ld = new Matrix(A);
                        ld[0, 1] = 0;
                        ld[0, 2] = 0;
                        ld[1, 2] = 0;
                        ld = MatrixOperations.FindReservedMatrix(ld);
            /*            Matrix t = new Matrix(3, 3);
                        t[0, 0] = 1 / 3;
                        t[1, 1] = 1 / 4;
                        t[2, 0] = -1 / 12;
                        t[2, 1] = -1 / 8;
                        t[2, 2] = 1 / 4;
                        MatrixOperations.ToTrianguled(t, ref t);
                        Matrix R = new Matrix(3, 3);
                        R[0, 1] = A[0, 1];
                        R[0, 2] = A[0, 2];
                        R[1, 2] = A[1, 2];
                        Console.WriteLine(MatrixOperations.Product(ld, R).ToString());
                        Console.ReadLine();
               */
            /*
                        Matrix A = new Matrix(3, 3);
                        A.Read();
                        Matrix A_1 = MatrixOperations.FindReservedMatrix(A, true);
                        MatrixOperations.ToTrianguled(A, ref A, false, 0.0000001);
                        Console.WriteLine(MatrixOperations.Product(A, A_1, 0.00000001).ToString());
                        Console.ReadLine();
                        */
            /*
            LU_SLAR lu;
            int Number = 0;            
            Console.WriteLine("Enter number of equations:");
            Number = Convert.ToInt32(Console.ReadLine());            
            lu = new LU_SLAR(Number);
            lu.ReadSLAR();
            lu.SolveSLAR();
            lu.PrintResults();
            Console.ReadLine();
            */
            #endregion
        }
    }
}