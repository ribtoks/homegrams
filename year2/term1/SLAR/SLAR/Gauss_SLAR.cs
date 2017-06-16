using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SLAR
{
    public class Gauss_SLAR : basic_SLAR
    {
        #region Дані

        //зовсім додаткові дані
        protected double _determinant; //визначник головної матриці
        protected int _rank; //ранг головної матриці
        protected Matrix Trianguled_A; //головна, зведена до трикутної
        protected double[] Trianguled_b; //стовпець, що при зведенні до трикутної...
        protected bool isTrianguled; //чи матриця вже зведена до трикутного вигляду

        #endregion

        #region Конструктори

        public Gauss_SLAR()
            : base()
        {
            Trianguled_A = null;
            Trianguled_b = null;
            isTrianguled = false;
            _determinant = Double.NaN;
            _rank = 0;
        }

        public Gauss_SLAR(int equation_number, int variable_number)
            : base(equation_number, variable_number)
        {
            isTrianguled = false;
        }

        public Gauss_SLAR(int Dimension)
            : this(Dimension, Dimension)
        {
        }

        public Gauss_SLAR(Matrix matrixA, double[] vectorB, bool AlreadyTrianguled)
            : base(matrixA, vectorB)
        {
            if (AlreadyTrianguled == true)
            {
                Trianguled_A = new Matrix(A);
                Trianguled_b = (double[])b.Clone();
            }
            isTrianguled = AlreadyTrianguled;
        }

        public Gauss_SLAR(Matrix matrixA, double[] vectorB)
            : base(matrixA, vectorB)
        {            
            isTrianguled = false;
        }

        public Gauss_SLAR(Gauss_SLAR from)
            : base(from)
        {
            if (from.isTrianguled == true)
            {
                isTrianguled = true;
                Trianguled_A = new Matrix(from.Trianguled_A);
                Trianguled_b = (double[])from.Trianguled_b.Clone();
                try
                {
                    _rank = from._rank;
                    _determinant = from._determinant;
                }
                catch
                {
                }
            }
        }

        #endregion

        public void ReSetSLAR(Matrix matrixA, double[] vectorB, bool AlreadyTrianguled)
        {
            ReSetSLAR(matrixA, vectorB);
            isTrianguled = AlreadyTrianguled;
            if (AlreadyTrianguled == true)
            {
                Trianguled_A = new Matrix(matrixA);
                Trianguled_b = (double[])b.Clone();
            }
            else
            {
                Trianguled_A = null;
                Trianguled_b = new double[0];
            }
            _determinant = 0;
            _rank = 0;
        }

        //подивитися ранг
        public int Rank
        {
            get { return _rank; }
        }
        //та визначник
        public double Determinant
        {
            get { return _determinant; }
        }

        //змінити лише вектор
        public void ChangeVectorB(double[] newVectorB, bool Already_Trianguled)
        {
            b = (double[])newVectorB.Clone();
            if (Already_Trianguled == true)
            {
                Trianguled_b = (double[])newVectorB.Clone();
            }
        }

        public override void PrintResults()
        {
            if (_determinant != 0)
            {
                base.PrintResults();
                Console.WriteLine("Determinant == " + _determinant);
            }
            else
            {
#if DEBUG
                Console.WriteLine("There're no solution to this SLAR. Determinant == 0.");
                return;
#endif
                throw new Exception("Can not find solution! Input Matrix is degenerate!");
            }
        }

        #region Розв'язування

        protected void AllToTrianguled()
        {
            if (isTrianguled == true)
                return;

            bool swapped = false;

            Trianguled_A = new Matrix(A);
            Trianguled_b = (double[])b.Clone();

            int i = 0, k = 0, j = 0;
            int rank = 0;

            //власне зведення до трикутної матриці
            for (; i < Trianguled_A.Width; ++i)
            {
                if (Math.Abs(Trianguled_A[i, i]) < epsilon)
                {
                    k = i + 1;
                    if (k < Trianguled_A.Height)
                        while (k < Trianguled_A.Height & Trianguled_A[k, i] == 0)
                        {
                            ++k;
                            if (k == Trianguled_A.Height)
                            {
                                rank = i - 1;
                                _rank = rank;
                                _determinant = 0;
                                return;
                            }
                        }
                    else
                    {
                        rank = i - 1;
                        _rank = rank;
                        _determinant = 0;
                        return;
                    }
                    swapped = !swapped;
                    Trianguled_A.SwapRows(i, k);
                    //і свап знов елементів вектора - стовбця                    
                    double temp2 = Trianguled_b[i];
                    Trianguled_b[i] = Trianguled_b[k];
                    Trianguled_b[k] = temp2;
                }//if   == 0

                //реалізація вибору в стовпці максимального елемента за модулем
                int mod_index = i;
                for (int ind = i + 1; ind < Trianguled_A.Height; ++ind)
                {
                    if (Math.Abs(Trianguled_A[ind, i]) > Math.Abs(Trianguled_A[mod_index, i]))
                        mod_index = ind;
                }

                //якщо максимальний елемент < Epsilon, то вихід
                if (Math.Abs(Trianguled_A[mod_index, i]) < epsilon)
                {
                    rank = i - 1;
                    _rank = rank;
                    _determinant = 0;
                    return;
                }

                //свап цих векторів
                if (mod_index > i)
                {
                    swapped = !swapped;
                    Trianguled_A.SwapRows(i, mod_index);
                    //і свап знов елементів вектора - стовбця
                    double temp_ = Trianguled_b[i];
                    Trianguled_b[i] = Trianguled_b[mod_index];
                    Trianguled_b[mod_index] = temp_;
                }

                //real calculating
                for (k = i + 1; k < Trianguled_A.Height; ++k)
                {
                    double factor = Trianguled_A[k, i] / Trianguled_A[i, i];
                    for (j = i + 1; j < Trianguled_A.Width; ++j)
                        Trianguled_A[k, j] -= (factor * Trianguled_A[i, j]);
                    Trianguled_A[k, i] = 0;
                    Trianguled_b[k] -= (factor * Trianguled_b[i]);
                }
            }//main for

            if (Trianguled_A.Height > Trianguled_A.Width)
                rank = Trianguled_A.Width;
            else
                rank = Trianguled_A.Height;
            _rank = rank;

            //заодно підрахуємо визначник
            _determinant = 1.0;
            for (i = 0; i < Trianguled_A.Width; ++i)
                _determinant *= Trianguled_A[i, i];
            if (swapped)
                _determinant *= -1;
            isTrianguled = true;
        }

        protected void SolveByColumnGauss()
        {
            AllToTrianguled();
            x = new double[this.Trianguled_b.Length];
            int i = 0, j = 0;
            //власне обрахунок коренів
            for (i = Trianguled_A.Height - 1; i >= 0; --i)
            {
                double temp_sum = 0;
                for (j = i + 1; j < Trianguled_A.Width; ++j)
                    temp_sum += (x[j] * Trianguled_A[i, j]);

                double coef = Trianguled_b[i];
                double div = Trianguled_A[i, i];
                double curr_x = (coef - temp_sum) / div;
                if (Math.Abs(curr_x) <= epsilon)
                    curr_x = 0;
                x[i] = curr_x;
            }//end of calculating x
        }

        public override void SolveSLAR()
        {
            try
            {
                SolveByColumnGauss();
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch
            {
#if DEBUG
                throw;
#endif
            }
        }

        #endregion

        #region DEBUG

        //друкує матрицю, зведену до трикутного вигляду і вектор - стовпець
        public void PrintTrianguled()
        {
            if (Trianguled_A != null)
            {
                for (int i = 0; i < Trianguled_A.Height; ++i)
                {
                    for (int j = 0; j < Trianguled_A.Width; ++j)
                    {
                        Console.Write(Trianguled_A[i, j] + " ");
                    }
                    Console.WriteLine(" | " + Trianguled_b[i]);
                }
            }
        }

        #endregion

        #region Зчитування

        public override void ReSetSLAR(object obj)
        {
            Gauss_SLAR gs_slar = (Gauss_SLAR)obj;
            if (gs_slar == null)
                throw new NotSupportedException("Can not unbox object to Gauss_SLAR!");
            ReSetSLAR(gs_slar.A, gs_slar.b, gs_slar.isTrianguled);
            gs_slar = null;
        }

        #endregion

        #region Запис

        public override void WriteResultsToStream(TextWriter tw)
        {
            base.WriteResultsToStream(tw);            
            tw.WriteLine("Determinant == " + _determinant);
            tw.WriteLine();
        }

        public virtual void WriteTrianguledToStream(TextWriter tw)
        {
            if (Trianguled_A != null)
            {
                for (int i = 0; i < Trianguled_A.Height; ++i)
                {
                    for (int j = 0; j < Trianguled_A.Width; ++j)
                    {
                        tw.Write(Trianguled_A[i, j] + " ");
                    }
                    tw.WriteLine(" | " + Trianguled_b[i]);
                }
            }
        }

        public override void WriteInputToStream(TextWriter tw)
        {
            base.WriteInputToStream(tw);
        }

        #endregion
    }
}
