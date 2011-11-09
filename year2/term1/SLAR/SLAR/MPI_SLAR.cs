using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SLAR
{
    //базовий клас для ітераційних методів
    public class MPI_SLAR : basic_SLAR
    {
        #region Дані

        //ітераційна матриця
        protected Matrix C;
        //ітераційний вектор
        protected double[] d;

        protected bool isRandomStartVector = true;

        protected SLAR_Diagnostics slar_d;

        #endregion

        #region Конструктори

        public MPI_SLAR()
            : base()
        {
        }
       
        public MPI_SLAR(int Dimension)
            : base(Dimension)
        {
            C = new Matrix(Dimension, Dimension);            
            d = new double[Dimension];
            slar_d = new SLAR_Diagnostics();
        }

        public MPI_SLAR(MPI_SLAR mpi_slar)
            : base(mpi_slar)
        {
            C = new Matrix(mpi_slar.C);
            d = (double[])mpi_slar.d.Clone();
            slar_d = new SLAR_Diagnostics();
        }

        public MPI_SLAR(Matrix matrixA, double[] vectorB)
            : base(matrixA, vectorB)
        {
        }

        #endregion

        public bool IsRandomStartVector
        {
            get { return isRandomStartVector; }
            set { isRandomStartVector = value; }
        }

        public void Set_C(Matrix newC)
        {
            C = new Matrix(newC);
        }

        public void Set_d(double[] new_d)
        {
            d = (double[])new_d.Clone();
        }

        public override void SolveSLAR()
        {            
            int n = C.Width;
            double[] x_temp = null;
            if (isRandomStartVector == true)
                x_temp = VectorOperations.GetRandomVector(0, n, n);
            else
                x_temp = VectorOperations.Get_Unit_Vector(n);
            double[] temp = new double[n];
            //початкове наближення
            do
            {
                temp = C.MulVector(x_temp);
                x_temp = VectorOperations.Add(temp, d);
            } while (ContinueIterations(ref x_temp, ref temp));            
            x = (double[])x_temp.Clone();
        }

        public void Print_C()
        {
            C.Print();
        }

        public void Print_d()
        {
            VectorOperations.PrintVector(d);
        }

        //перевірка, чи в матриці 'С' є діагональна перевага
        public bool HasDiagonalAdvantage()
        {
            bool Have = true;
            for (int i = 0; i < A.Height; ++i)
            {
                double module_sum = 0;
                for (int j = 0; j < A.Width; ++j)
                {
                    if (j != i)
                        module_sum += Math.Abs(A[i, j]);
                }
                if (module_sum >= Math.Abs(A[i, i]))
                {
                    Have = false;
                    break;
                }
            }
            return Have;
        }

        public int GetIterationsMade()
        {
            return slar_d.IterationNumber;
        }

        public override void PrintResults()
        {
            Console.WriteLine("Vector X:" + VectorOperations.VectorToString(x));
            Console.WriteLine("Made " + slar_d.IterationNumber + " iteration(s).");             
        }

        public override void WriteResultsToStream(TextWriter tw)
        {
            base.WriteResultsToStream(tw);
            tw.WriteLine("Made " + slar_d.IterationNumber + " iteration(s).");
            tw.WriteLine();
        }

        public virtual void Write_CToStream(TextWriter tw)
        {
            tw.WriteLine(C.ToString());            
        }

        public virtual void Write_dToStream(TextWriter tw)
        {
            tw.WriteLine(VectorOperations.VectorToString(d));
        }

        public override void ReadSLAR()
        {
            base.ReadSLAR();
            Console.WriteLine("Please, enter Epsilon, that you want(default is 0,00000001):");
            string str = Console.ReadLine();
            double eps = 0.00000001;
            if (str.Length != 0)
                eps = Double.Parse(str);
            epsilon = eps;
        }

        public override void ReSetSLAR(object obj)
        {
            MPI_SLAR mpi_slar = (MPI_SLAR)obj;
            if (mpi_slar == null)
                throw new NotSupportedException("Can not unbox object to MPI_SLAR!");
            ReSetSLAR(mpi_slar.A, mpi_slar.b);
            C = mpi_slar.C.CopyOfMe();
            d = (double[])mpi_slar.d.Clone();
            slar_d = new SLAR_Diagnostics(mpi_slar.slar_d);
            isRandomStartVector = mpi_slar.isRandomStartVector;
            mpi_slar = null;
        }

        //метод, що визначає, чи продовжувати цикл ітерації
        protected virtual bool ContinueIterations(ref double[] vectorPrev, ref double[] vectorCurr)
        {
            return (! VectorOperations.IsEqualByEpsilon(vectorPrev, vectorCurr, epsilon));
        }
    }
}
