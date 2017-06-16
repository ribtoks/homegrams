using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MethodTesturer
{
    class Program
    {
        static void Main(string[] args)
        {
            MethodTesturer mt = new MethodTesturer();
            Console.WriteLine("Started...");
            mt.StartTesturing("g|f|Equations.txt");
            Console.WriteLine(mt.GetStatistics());
            Console.ReadLine();
        }
    }
}
