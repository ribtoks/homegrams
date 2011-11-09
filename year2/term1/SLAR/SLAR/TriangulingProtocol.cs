using System;
using System.Collections.Generic;
using System.Text;

namespace SLAR
{
    public struct SwapEl
    {
        public int x;
        public int y;
        public int iteration;

        public SwapEl(int n1, int n2, int iter)
        {
            x = n1;
            y = n2;
            iteration = iter;
        }
    }


    public class TriangulingProtocol
    {
        internal List<SwapEl> MadeSwaps;
        internal double[] factors;

        public TriangulingProtocol(int Dimension)
        {
            MadeSwaps = new List<SwapEl>(0);
            factors = new double[(Dimension * (Dimension - 1)) >> 1];
        }

        public TriangulingProtocol(TriangulingProtocol tp)
        {
            MadeSwaps = new List<SwapEl>(tp.MadeSwaps);
            factors = (double[])tp.factors.Clone();
        }
    }
}
