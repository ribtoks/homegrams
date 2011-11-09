using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHP
{
    public class SolverResults
    {
        private Dictionary<int, double> priorityDictionary;
        private double consistencyIndex;

        public Dictionary<int, double> PriorityDictionary
        {
            get { return priorityDictionary; }
        }

        public double ConsistencyIndex
        {
            get { return consistencyIndex; }
        }

        public SolverResults(Dictionary<int, double> newPriorityDictionary, double newConsistencyIndex)
        {
            priorityDictionary = newPriorityDictionary;
            consistencyIndex = newConsistencyIndex;
        }
    }
}
