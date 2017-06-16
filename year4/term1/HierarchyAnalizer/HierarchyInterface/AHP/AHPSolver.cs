using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HierarchyClasses.MatrixClasses;
using HierarchyClasses;

namespace AHP
{
    public static class AHPSolver
    {
        #region Private methods and constants

        private static Dictionary<int, double> standartConsistencyIndex;

        private static Dictionary<int, double> StandartConsistencyIndex
        {
            get
            {
                if (standartConsistencyIndex == null)
                {
                    standartConsistencyIndex = new Dictionary<int, double>()
                    {
                        {1, 0},
                        {2, 0},
                        {3, 0.58},
                        {4, 0.9},
                        {5, 1.12},
                        {6, 1.24},
                        {7, 1.32},
                        {8, 1.41},
                        {9, 1.45},
                        {10, 1.49}
                    };
                }
                return standartConsistencyIndex;
            }
        }

        private static double[] CalculateLocalPriorities(Matrix M)
        {
            double totalSum = 0;
            double[] result = new double[M.Height];
            for (int i = 0; i < M.Height; i++)
            {
                double product = 1;
                for (int j = 0; j < M.Width; j++)
                    product *= M[i, j];
                result[i] = Math.Pow(product, 1 / (double)M.Width);
                totalSum += result[i];
            }
            result = VectorOperations.MulDouble(result, 1 / totalSum);
            return result;
        }

        private static double CalculateLocalConsistensyIndex(Matrix M, double[] localPriorities)
        {
            double consistensyIndex = 0;
            int n = M.Width;
            for (int j = 0; j < M.Width; j++)
            {
                double columnSum = 0;
                for (int i = 0; i < M.Height; i++)
                    columnSum += M[i, j];
                consistensyIndex += columnSum * localPriorities[j];
            }
            consistensyIndex -= n;
            consistensyIndex /= (double)(n - 1);
            return consistensyIndex;
        }

        private static OrientatedGraph<int, Matrix>.GraphCell GetParentNode(OrientatedGraph<int, Matrix> hierarcyGraph, Dictionary<int, double[]> priorityVectorData, OrientatedGraph<int, Matrix>.GraphCell currentNode)
        {
            foreach (var node in hierarcyGraph.Apices)
            {
                if (priorityVectorData.Keys.Contains<int>(node.Key))
                    continue;
                var cell = node.Value.Next;
                bool isChild = false;
                while (cell != null)
                {
                    if (cell.Apex == currentNode.Apex)
                    {
                        isChild = true;
                        break;
                    }
                    cell = cell.Next;
                }
                if (isChild)
                    return node.Value;
            }
            return null;
        }

        #endregion

        public static SolverResults Solve(OrientatedGraph<int, Matrix> hierarcyGraph)
        {
            //Find the top of tree
            var top = new KeyValuePair<int, OrientatedGraph<int, Matrix>.GraphCell>();
            foreach (var node1 in hierarcyGraph.Apices)
            {
                if (node1.Value.Next == null)
                    continue;
                bool isTop = true;
                foreach (var node2 in hierarcyGraph.Apices)
                {
                    if (node2.Value.Next == null)
                        continue;
                    if (node2.Value.Next == node1.Value)
                    {
                        isTop = false;
                        break;
                    }
                }
                if (isTop)
                {
                    top = node1;
                    break;
                }
            }

            //Determine one leave
            int leave = top.Key;
            foreach (var node in hierarcyGraph.Apices)
                if (node.Value.Next == null)
                {
                    leave = node.Key;
                    break;
                }

            Dictionary<int, double[]> priorityVectorData = new Dictionary<int, double[]>();
            Dictionary<int, double> consistensyIndexData = new Dictionary<int, double>();

            //Calculate leaves parents
            foreach (var node in hierarcyGraph.Apices)
            {
                OrientatedGraph<int, Matrix>.GraphCell cell = node.Value.Next;
                bool hasLeave = false;
                while (cell != null)
                {
                    if (cell.Apex == leave)
                    {
                        hasLeave = true;
                        break;
                    }
                    cell = cell.Next;
                }

                if (!hasLeave)
                    continue;

                priorityVectorData.Add(node.Key, CalculateLocalPriorities(hierarcyGraph.Data[node.Key]));
                consistensyIndexData.Add(node.Key, CalculateLocalConsistensyIndex(hierarcyGraph.Data[node.Key], priorityVectorData[node.Key]));
            }

            //Calculate others
            while (!priorityVectorData.Keys.Contains<int>(top.Key))
            {
                foreach (var node in hierarcyGraph.Apices)
                {
                    if (priorityVectorData.Keys.Contains<int>(node.Key))
                        continue;
                    if (node.Value.Next == null)
                        continue;
                    OrientatedGraph<int, Matrix>.GraphCell cell = node.Value.Next;
                    bool isAllChildsCalculated = true;
                    while (cell != null)
                    {
                        if (!priorityVectorData.Keys.Contains<int>(cell.Apex))
                        {
                            isAllChildsCalculated = false;
                            break;
                        }
                        cell = cell.Next;
                    }
                    if (isAllChildsCalculated)
                    {
                        double[] localPriorities = CalculateLocalPriorities(hierarcyGraph.Data[node.Key]);
                        double[] consistensyIndexVector = new double[localPriorities.Length];
                        cell = node.Value.Next;
                        int height = 1;
                        if (!priorityVectorData.Keys.Contains<int>(cell.Apex))
                            height = CalculateLocalPriorities(hierarcyGraph.Data[cell.Apex]).Length;
                        else
                            height = priorityVectorData[cell.Apex].Length;

                        Matrix M = new Matrix(localPriorities.Length, height);
                        int j = localPriorities.Length - 1;
                        while (cell != null)
                        {
                            double[] temp;
                            if (!priorityVectorData.Keys.Contains<int>(cell.Apex))
                                temp = CalculateLocalPriorities(hierarcyGraph.Data[cell.Apex]);
                            else
                                temp = priorityVectorData[cell.Apex];

                            for (int i = 0; i < height; i++)
                                M[i, j] = temp[i];
                            consistensyIndexVector[j] = consistensyIndexData[cell.Apex];
                            cell = cell.Next;
                            j--;
                        }
                        priorityVectorData.Add(node.Key, M.MulVector(localPriorities));
                        consistensyIndexData.Add(node.Key, VectorOperations.ScalarProduct(localPriorities, consistensyIndexVector));
                    }
                }
            }

            //Get leaves list
            List<int> leavesIDList = new List<int>();
            foreach (var node in hierarcyGraph.Apices)
            {
                OrientatedGraph<int, Matrix>.GraphCell cell = node.Value.Next;
                if (cell == null)
                    leavesIDList.Add(node.Key);
            }


            double[] globalPriorities = priorityVectorData[top.Key];
            Dictionary<int, double> result = new Dictionary<int, double>();
            for (int i = 0; i < globalPriorities.Length; i++)
                result.Add(leavesIDList[i], globalPriorities[i]);

            return new SolverResults(result, consistensyIndexData[top.Key] / StandartConsistencyIndex[leavesIDList.Count]);
        }
    }
}
