using System;
using System.Collections.Generic;
using System.Text;

namespace SLAR
{
    public struct SLAR_Diagnostics
    {
        private int iterationNumber;

        public int IterationNumber
        {
            get { return iterationNumber; }
            set { iterationNumber = value; }
        }
        
        public SLAR_Diagnostics(SLAR_Diagnostics sd)
        {
            iterationNumber = sd.iterationNumber;
        }
    }
}
