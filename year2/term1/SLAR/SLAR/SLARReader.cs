using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace SLAR
{
    public class SLARReader
    {
        public SLARReader()
        {
            obj = new object();
        }

        private object obj;

        public object Obj
        {
            get { return obj; }
            set { obj = value; }
        }
        
        // Summary:
        //     Reads a SLAR of type TypeOfSLAR from TextReader
        //
        // Parameters:
        //   tr:
        //     A stream, where the SLAR is written
        //
        //   TypeOfSLAR:
        //     Type of SLAR that you want to get from this method
        //     
        // Returns:
        //     Object then reffers the requested SLAR
        //
        // Exceptions:
        //   System.NotSupportedException:
        //     Unknown type of input data in stream
        public object ReadSLAR(TextReader tr, Type TypeOfSLAR)
        {
            string FirstLine = tr.ReadLine();
            string[] numbers = FirstLine.Split(' ');
            int h = 0;
            int w = 0;
            int i = 0;

            object temp = new object();

            //temp SLAR to refill current
//            basic_SLAR bs = new basic_SLAR();
            //if we have only one(two equal) dimension
            if (numbers.Length == 1)
            {
                w = Int32.Parse(numbers[0]);
                h = w;
            }
            else
            {
                h = Int32.Parse(numbers[0]);
                w = Int32.Parse(numbers[1]);
            }
            //reading type of input data
            string KindOf = tr.ReadLine();
            //reading the input data
            string[] lines = new string[h];
            for (i = 0; i < h; ++i)
            {
                string s = tr.ReadLine();
                lines[i] = (string)s.Clone();
            }

            //pattern for new object
            Matrix A = new Matrix();
            double[] b = new double[0];

            //choose type of parsing
            switch (KindOf)
            {
                //Coeficients
                case "c":
                case "C":
                    //reading coefs
                    ReadCoefs(lines, out A, out b);
                    break;
                //Equations
                case "e":
                case "E":
                    //reading wholeequations
                    ReadEquations(lines, out A, out b);
                    break;
                default:
                    throw new NotSupportedException("Such type of parsing doesn't exist!");
            }            
            ConstructorInfo[] ci = TypeOfSLAR.GetConstructors();
            //find the looked for - constructor
            for (i = 0; i < ci.Length; ++i)
            {
                ParameterInfo[] pi = ci[i].GetParameters();
                //looking for constructor (Matrix, double[])
                if (pi.Length == 2)
                    if (pi[0].ParameterType == typeof(Matrix) & pi[1].ParameterType == typeof(double[]))
                    {
                        //now we found...
                        object[] parameters = new object[2];
                        parameters[0] = A;
                        parameters[1] = b;
                        obj = ci[i].Invoke(parameters);
                        temp = ci[i].Invoke(parameters); // for return
                        //done everething! break...
                        break;
                    }                    
            }
            return temp;
        }

        //parsing lines with coefs...
        protected void ReadCoefs(string[] Lines, out Matrix A, out double[] b)
        {
            //out data preparing...
            Matrix Temp = new Matrix(Lines.Length);
            double[] d = new double[Lines.Length];
            //calculating            
            for (int i = 0; i < Lines.Length; ++i)
            {
                string s = Lines[i];
                string[] numbers = s.Split(' ');
                for (int j = 0; j < numbers.Length - 1; ++j)
                    Temp[i, j] = Double.Parse(numbers[j]);
                d[i] = Double.Parse(numbers[numbers.Length - 1]);
            }
            A = Temp;
            b = d;
        }

        //parsing lines with equations...
        protected void ReadEquations(string[] Lines, out Matrix A, out double[] b)
        {
            //out data preparing...
            Matrix Temp = new Matrix(Lines.Length);
            double[] d = new double[Lines.Length];
            int n = Lines.Length;
            //calculating
            for (int i = 0; i < Lines.Length; ++i)
            {
                double t;
                Temp[i] = EquationParser.Parse(Lines[i], n, out t);
                d[i] = t;
            }
            A = Temp;
            b = d;
        }
    }
}
