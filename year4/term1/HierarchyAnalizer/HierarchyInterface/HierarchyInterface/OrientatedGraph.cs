using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HierarchyInterface
{
    /*
     * [a] -> b -> c -> d -> null
     * [b] -> k -> l -> null
     * [c] -> null
     * [d] -> m -> l -> null
     * 
     * 
     *              a
     *      b       c       d
     *    k     l               m
    */

    /// <summary>
    /// Class, that represents orientated graph
    /// </summary>
    /// <typeparam name="Q">Type of node identifier</typeparam>
    /// <typeparam name="T">Type of element holded in node</typeparam>
    class OrientatedGraph<Q, T>
        where T : class, ICloneable, new()
        where Q : IComparable<Q>
    {
        #region Class Data

        public class GraphCell
        {
            public Q Apex { get; set; }
            public GraphCell Next { get; set; }

            public GraphCell(Q cellApex, GraphCell nextCell)
            {
                Apex = cellApex;
                Next = nextCell;
            }
        }

        private Dictionary<Q, GraphCell> table = new Dictionary<Q, GraphCell>();
        private Dictionary<Q, T> apexData = new Dictionary<Q, T>();

        #endregion

        private bool deleteEdge(Q a, Q b)
        {
            GraphCell cl = table[a].Next;
            if (cl == null)
                return false;

            if (cl.Apex.CompareTo(b) == 0)
            {
                table[a].Next = cl.Next;
                apexData.Remove(b);
                return true;
            }

            GraphCell cl_next = cl.Next;
            while (true)
            {
                if (cl_next == null)
                    return false;
                if (cl_next.Apex.CompareTo(b) == 0)
                    break;
                cl = cl_next;
                cl_next = cl.Next;
            }

            cl.Next = cl_next.Next;
            apexData.Remove(b);
            return true;
        }

        public void AddEdge(Q a, Q b, T data)
        {
            GraphCell cl = table[a];
            GraphCell newCell = new GraphCell(b, null);
            newCell.Next = cl.Next;
            cl.Next = newCell;
        }

        public void AddApex(Q a, T data)
        {
            apexData.Add(a, (T)data.Clone());
            table.Add(a, new GraphCell(a, null));
        }

        public bool RemoveEdge(Q a, Q b)
        {
            return deleteEdge(a, b);
        }

        public Dictionary<Q, GraphCell> Apices
        {
            get { return table; }
        }

        public Dictionary<Q, T> Data
        {
            get { return apexData; }
            set { apexData = value; }
        }
    }
}
