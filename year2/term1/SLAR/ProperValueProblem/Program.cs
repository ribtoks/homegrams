using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SLAR;

namespace ProperValueProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ProperProblemSolver pps = new ProperProblemSolver();
                pps.FillSolver();
                pps.SolveProblem();
                pps.PrintResults();
                pps.PrintTime();
            }
            catch (Exception ex)
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Error occured!");
                Console.WriteLine("--------------");
                Console.WriteLine(ex.Message);
                Console.WriteLine("--------------------------------------------------");
            }            
            Console.ReadLine();
        }
    }
}
