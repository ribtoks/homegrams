using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SLAR;

namespace MethodTesturer
{
    public class MethodTesturer
    {
        #region Дані
        //головна складова
        SLARSolver ss;
        //кількість випробувань
        int testNumber = 10;        
        //потік для виконання
        Thread testuringThreads;
        //statistics data       
        int ok_num;
        int wa_num;
        int tl_num;

        //пороговий час
        double min_time;

        #endregion

        public MethodTesturer()            
        {
            ss = new SLARSolver();
        }

        public int TestNumber
        {
            get { return testNumber; }
            set { testNumber = value; }
        }

        public void StartTesturing(string format)
        {
            for (int i = 0; i < testNumber; ++i)
            {
            }
        }

        private void Run()
        {
            lock (this)
            {
                ss.Solve();
            }
        }

        private void SetSLAR(string format)
        {
            ss.FillSLAR(format);
        }

        public string GetStatistics()
        {
            return "WA == " + wa_num.ToString() + " TL == " + tl_num.ToString() + " OK == " + ok_num.ToString();
        }
    }
}
