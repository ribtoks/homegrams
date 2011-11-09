using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HierarchyClasses
{
    /*
     * [a] -> b -> c -> d -> null
     * [b] -> k -> l -> null
     * [c] -> null
     * [d] -> m -> null
     * 
     * 
     *              a
     *      b       c       d
     *    k   l               m
    */

    /// <summary>
    /// Class, that represents an orientated graph
    /// </summary>
    /// <typeparam name="Q">Type of node identifier</typeparam>
    /// <typeparam name="T">Type of element holded in node</typeparam>
    [Serializable]
    public class OrientatedGraph<Q, T>
        where T : class, ICloneable, new()
        where Q : IComparable<Q>
    {
        #region Class Data
        [Serializable]
        public class GraphCell
        {
            public Q Apex { get; set; }
            public GraphCell Next { get; set; }

            public GraphCell(Q cellApex, GraphCell nextCell)
            {
                Apex = cellApex;
                Next = nextCell;
            }

            /// <summary>
            /// Returns count subnodes
            /// </summary>
            public int Count
            {
                get
                {
                    int count = 0;
                    GraphCell cell = this.Next;
                    while (cell != null)
                    {
                        cell = cell.Next;
                        ++count;
                    }
                    return count;
                }
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

        /// <summary>
        /// Creates orientated edge in graph. An O(1) operation.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void AddEdge(Q a, Q b)
        {
            GraphCell cl = table[a];
            GraphCell newCell = new GraphCell(b, null);
            newCell.Next = cl.Next;
            cl.Next = newCell;
        }

        /// <summary>
        /// Adds apex with its data to graph
        /// </summary>
        /// <param name="a"></param>
        /// <param name="data"></param>
        public void AddApex(Q a, T data)
        {
            apexData.Add(a, (T)data.Clone());
            table.Add(a, new GraphCell(a, null));
        }

        /// <summary>
        /// Deletes specified apex from graph with 
        /// all connections to it from other apices.
        /// An O(n + m) operation.
        /// </summary>
        /// <param name="a">Apex to delete</param>
        public void DeleteApex(Q a)
        {
            table.Remove(a);
            apexData.Remove(a);

            // now remove all references on this element in graph
            foreach (var keyvalue in table)
            {
                // delete edge if exists
                deleteEdge(keyvalue.Key, a);
            }
        }

        /// <summary>
        /// Deletes edge and creates new clear data of apex if it has no childs
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True if deletion succeeded, otherwise false</returns>
        public bool RemoveEdge(Q a, Q b)
        {
            bool result = deleteEdge(a, b);
            if (table[a].Next == null)
                apexData[a] = new T();
            return result;
        }

        /// <summary>
        /// Checks if such edge exists in graph
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True, if edge exists, otherwise false</returns>
        public bool EdgeExists(Q a, Q b)
        {
            GraphCell cell = table[a].Next;

            while (cell != null)
            {
                if (cell.Apex.CompareTo(b) == 0)
                    return true;
                cell = cell.Next;
            }
            return false;
        }

        /// <summary>
        /// Just deletes edge. An O(n) operation.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True if deletion succeeded, otherwise false</returns>
        public bool DeleteEdge(Q a, Q b)
        {
            return deleteEdge(a, b);
        }

        /// <summary>
        /// Gets apices of current graph
        /// </summary>
        public Dictionary<Q, GraphCell> Apices
        {
            get { return table; }
        }

        /// <summary>
        /// Gets data of appropriate apex
        /// </summary>
        public Dictionary<Q, T> Data
        {
            get { return apexData; }
            set { apexData = value; }
        }
    }
}
