using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AHP;
using HierarchyClasses.MatrixClasses;
using HierarchyClasses;

namespace AHP_Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            double[][] data = new double[5][];
            data[0] = new double[] { 1, 3, 0.2, 1 / 6.0, 1 / 8.0 };
            data[1] = new double[] { 1 / 3.0, 1, 1 / 6.0, 1 / 8.0, 1 / 9.0 };
            data[2] = new double[] { 5, 6, 1, 1 / 3.0, 1 / 5.0 };
            data[3] = new double[] { 6, 8, 3, 1, 1 / 3.0 };
            data[4] = new double[] { 8, 9, 5, 3, 1 };
            Matrix Q11 = new Matrix(data, false);

            data[0] = new double[] { 1, 1, 1, 1, 1 / 9.0 };
            data[1] = new double[] { 1, 1, 1, 1, 1 / 9.0 };
            data[2] = new double[] { 1, 1, 1, 1, 1 / 9.0 };
            data[3] = new double[] { 1, 1, 1, 1, 1 / 9.0 };
            data[4] = new double[] { 9, 9, 9, 9, 1 };
            Matrix Q21 = new Matrix(data, false);

            data[0] = new double[] { 1, 1 / 2.0, 1 / 5.0, 1 / 3.0, 1 / 9.0 };
            data[1] = new double[] { 2, 1, 1 / 6.0, 1 / 2.0, 1 / 9.0 };
            data[2] = new double[] { 5, 6, 1, 2, 1 / 5.0 };
            data[3] = new double[] { 3, 2, 1 / 2.0, 1, 1 / 9.0 };
            data[4] = new double[] { 9, 9, 5, 9, 1 };
            Matrix Q22 = new Matrix(data, false);

            data[0] = new double[] { 1, 1, 3, 2, 1 / 7.0 };
            data[1] = new double[] { 1, 1, 3, 2, 1 / 7.0 };
            data[2] = new double[] { 1 / 3.0, 1 / 3.0, 1, 1 / 2.0, 1 / 9.0 };
            data[3] = new double[] { 1 / 2.0, 1 / 2.0, 2, 1, 1 / 8.0 };
            data[4] = new double[] { 7, 7, 9, 8, 1 };
            Matrix Q23 = new Matrix(data, false);

            data[0] = new double[] { 1, 4, 1, 7, 1 / 2.0 };
            data[1] = new double[] { 1 / 4.0, 1, 1 / 3.0, 5, 1 / 5.0 };
            data[2] = new double[] { 1, 3, 1, 7, 1 / 2.0 };
            data[3] = new double[] { 1 / 7.0, 1 / 5.0, 1 / 7.0, 1, 1 / 9.0 };
            data[4] = new double[] { 2, 5, 2, 9, 1 };
            Matrix Q24 = new Matrix(data, false);

            data[0] = new double[] { 1, 1 / 3.0, 3, 5, 1 / 3.0 };
            data[1] = new double[] { 3, 1, 5, 7, 1 };
            data[2] = new double[] { 1 / 3.0, 1 / 5.0, 1, 3, 1 / 5.0 };
            data[3] = new double[] { 1 / 5.0, 1 / 7.0, 1 / 3.0, 1, 1 / 7.0 };
            data[4] = new double[] { 3, 1, 5, 7, 1 };
            Matrix Q25 = new Matrix(data, false);

            OrientatedGraph<int, Matrix> hierarcyGraph = new OrientatedGraph<int, Matrix>();
            hierarcyGraph.AddApex(1, Q11);
            hierarcyGraph.AddApex(2, Q21);
            hierarcyGraph.AddApex(3, Q22);
            hierarcyGraph.AddApex(4, Q23);
            hierarcyGraph.AddApex(5, Q24);
            hierarcyGraph.AddApex(6, Q25);
            hierarcyGraph.AddApex(7, new Matrix());
            hierarcyGraph.AddApex(8, new Matrix());
            hierarcyGraph.AddApex(9, new Matrix());
            hierarcyGraph.AddApex(10, new Matrix());
            hierarcyGraph.AddApex(11, new Matrix());

            hierarcyGraph.AddEdge(1, 2);
            hierarcyGraph.AddEdge(1, 3);
            hierarcyGraph.AddEdge(1, 4);
            hierarcyGraph.AddEdge(1, 5);
            hierarcyGraph.AddEdge(1, 6);

            hierarcyGraph.AddEdge(2, 7);
            hierarcyGraph.AddEdge(2, 8);
            hierarcyGraph.AddEdge(2, 9);
            hierarcyGraph.AddEdge(2, 10);
            hierarcyGraph.AddEdge(2, 11);

            hierarcyGraph.AddEdge(3, 7);
            hierarcyGraph.AddEdge(3, 8);
            hierarcyGraph.AddEdge(3, 9);
            hierarcyGraph.AddEdge(3, 10);
            hierarcyGraph.AddEdge(3, 11);

            hierarcyGraph.AddEdge(4, 7);
            hierarcyGraph.AddEdge(4, 8);
            hierarcyGraph.AddEdge(4, 9);
            hierarcyGraph.AddEdge(4, 10);
            hierarcyGraph.AddEdge(4, 11);

            hierarcyGraph.AddEdge(5, 7);
            hierarcyGraph.AddEdge(5, 8);
            hierarcyGraph.AddEdge(5, 9);
            hierarcyGraph.AddEdge(5, 10);
            hierarcyGraph.AddEdge(5, 11);

            hierarcyGraph.AddEdge(6, 7);
            hierarcyGraph.AddEdge(6, 8);
            hierarcyGraph.AddEdge(6, 9);
            hierarcyGraph.AddEdge(6, 10);
            hierarcyGraph.AddEdge(6, 11);

            SolverResults results = AHPSolver.Solve(hierarcyGraph);
            Dictionary<int, double> priority = results.PriorityDictionary;
            foreach (KeyValuePair<int, double> pair in priority)
                Console.WriteLine("Node {0} has priority {1}", pair.Key, pair.Value);
            Console.WriteLine("\nConsistensy index is - {0}", results.ConsistencyIndex);

            Console.ReadLine();
        }
    }
}
