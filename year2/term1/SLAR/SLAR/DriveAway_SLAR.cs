using System;
using System.Collections.Generic;
using System.Text;

namespace SLAR
{
    //делегат на функцію, яка обрахує всі коефіцієнти
    public delegate void FillingCoefficients(out double[] lower_diagonal, out double[] central_diagonal,
        out double[] upper_diagonal, out double[] vectorF);

    //розв'язання системи рівнянь з тридіагональною матрицею методом зустрічних прогонок  [A*x == f]
    public class DriveAway_SLAR : basic_SLAR
    {
        #region Дані
        protected ThreeDiagMatrix A3;
        protected double[] f; //вектор-функція        
        
        protected double[] alpha; //вектор допоміжних коефіціентів 1 для правої прогонки
        protected double[] beta; //вектор допоміжних коефіціентів 2 для правої прогонки
        
        protected double[] ksi; //вектор допоміжних коефіціентів 1 для лівої прогонки
        protected double[] eta; //вектор допоміжних коефіціентів 2 для лівої прогонки

        protected int driveIndex; //індекс для прогонки [0 .. n - 1]        

//        FillingCoefficients filler;//делегат для заповнення коефіцієнтами
        #endregion        

        #region Конструктори
        public DriveAway_SLAR()
            : base()
        {           
        }

        //конструктор із параметрами - три діагоналі та вектор f та індекс для прогонки
        public DriveAway_SLAR(double[] lower_diagonal, double[] central_diagonal, 
            double[] upper_diagonal, double[] vectorF, int drivingIndex)
            : base(central_diagonal.Length, central_diagonal.Length)
        {
            A = null;
            int N = central_diagonal.Length;
            if (lower_diagonal.Length != central_diagonal.Length - 1 | upper_diagonal.Length != central_diagonal.Length  - 1
                | vectorF.Length != central_diagonal.Length)
                throw new Exception("Wrong diagonal size!");
            if (drivingIndex >= N)
                throw new Exception("Wrong driving element index!");

            f = (double[])vectorF.Clone(); //копіюємо члени функціонального ряду
            for (int i = 1; i < f.Length - 1; ++i)
                f[i] *= -1;

            driveIndex = drivingIndex;

            alpha = new double[N - 1];
            beta = new double[N - 1];

            ksi = new double[N - 1];
            eta = new double[N - 1];

            A3 = new ThreeDiagMatrix(central_diagonal, lower_diagonal, upper_diagonal);                        
            //головний цикл обрахунків
            for (int i = 1; i < N - 1; ++i)
            {
                A3[i, i] = -A3[i, i];                
            } //loop
            b = f;
        }
        
        public DriveAway_SLAR(FillingCoefficients FillDelegate, int drivingIndex)
        {
            A = null;
            double[] lower_diagonal;
            double[] upper_diagonal;
            double[] central_diagonal;
            double[] vectorF;
            FillDelegate(out lower_diagonal, out central_diagonal, out upper_diagonal, out vectorF);

            int N = central_diagonal.Length;
            
            //реалізація конструктора базового класу
            
            b = new double[N];
            x = new double[N];
            epsilon = 0.00000001; //[10]^-8            
            //\реалізація конструктора базового класу

            
            if (lower_diagonal.Length != central_diagonal.Length - 1 | upper_diagonal.Length != central_diagonal.Length - 1
                | vectorF.Length != central_diagonal.Length)
                throw new Exception("Wrong diagonal size!");
            if (drivingIndex >= N)
                throw new Exception("Wrong driving element index!");

            f = (double[])vectorF.Clone(); //копіюємо члени функціонального ряду
            for (int i = 1; i < f.Length - 1; ++i)
                f[i] *= -1;

            driveIndex = drivingIndex;

            alpha = new double[N - 1];
            beta = new double[N - 1];

            ksi = new double[N - 1];
            eta = new double[N - 1];

            A3 = new ThreeDiagMatrix(central_diagonal, lower_diagonal, upper_diagonal);
            //головний цикл обрахунків
            for (int i = 1; i < N - 1; ++i)
            {
                A3[i, i] = -A3[i, i];
            } //loop
            b = f;
        }

        public DriveAway_SLAR(DriveAway_SLAR dr_slar)
            : base(dr_slar)
        {
            A = null;
            A3 = new ThreeDiagMatrix(dr_slar.A3);
            f = (double[])dr_slar.f.Clone();
            alpha = (double[])dr_slar.alpha.Clone();
            beta = (double[])dr_slar.beta.Clone();
            ksi = (double[])dr_slar.ksi.Clone();
            eta = (double[])dr_slar.eta.Clone();
            driveIndex = dr_slar.driveIndex;
        }

        #endregion

        //перезаповнення за делегатом
        public void ReFillSLAR(FillingCoefficients FillDelegate)
        {
            double[] lower_diagonal;
            double[] upper_diagonal;
            double[] central_diagonal;
            double[] vectorF;
            FillDelegate(out lower_diagonal, out central_diagonal, out upper_diagonal, out vectorF);

            int N = central_diagonal.Length;

            //реалізація конструктора базового класу            
            b = new double[N];
            x = new double[N];
            epsilon = 0.00000001; //[10]^-8            
            //\реалізація конструктора базового класу


            if (lower_diagonal.Length != central_diagonal.Length - 1 | upper_diagonal.Length != central_diagonal.Length - 1
                | vectorF.Length != central_diagonal.Length)
                throw new Exception("Wrong diagonal size!");
            
            f = (double[])vectorF.Clone(); //копіюємо члени функціонального ряду
            for (int i = 1; i < f.Length - 1; ++i)
                f[i] *= -1;            

            alpha = new double[N - 1];
            beta = new double[N - 1];

            ksi = new double[N - 1];
            eta = new double[N - 1];

            A3 = new ThreeDiagMatrix(central_diagonal, lower_diagonal, upper_diagonal);
            //головний цикл обрахунків
            for (int i = 1; i < N - 1; ++i)
            {
                A3[i, i] = -A3[i, i];
            } //loop
            b = f;
        }

        //зміна індексу для прогонки
        public int DriveIndex
        {
            get
            {
                return driveIndex;
            }
            set
            {
                driveIndex = value;
            }
        }

        //тут не потрібно зчитувати слар
        public override void ReadSLAR()
        {
            throw new NotSupportedException("Can not invoke ReadSLAR for DriveAway_SLAR");
        }

        protected void AdjustLine(int LineIndex, int CorrectIndex)
        {
            if (A3[LineIndex, CorrectIndex] != 0)
            {
                double tmp = A3[LineIndex, CorrectIndex];
                for (int j = 0; j < A3.Height; ++j)
                    A3[LineIndex, j] /= tmp;
                f[LineIndex] /= tmp;
            }
            else
                throw new Exception("Attempt of Division by Zero!");
        }

        //перевірка (і виправлення в разі потребі), чи перші елементи є одиницями
        protected void MakeCorrectness()
        {
            if (A3[0, 0] != 1)
                AdjustLine(0, 0);
            if (A3[A3.Height - 1, A3.Width - 1] != 1)
                AdjustLine(A3.Height - 1, A3.Width - 1);
            b = (double[])f.Clone();
        }

        #region Необхідні коефіцієнти
        protected double getM1()
        {
            return f[0];
        }

        protected double getM2()
        {
            return f[f.Length - 1];
        }

        protected double getKapa1()
        {
            return -A3[0, 1];
        }

        protected double getKapa2()
        {
            return -A3[A3.Height - 1, A3.Width - 2];
        }

        protected double a_c(int i)
        {
            return A3[i + 1, i];
        }

        protected double b_c(int i)
        {
            return A3[i + 1, i + 2];
        }

        protected double c_c(int i)
        {
            return -A3[i + 1, i + 1];
        }

        protected double f_c(int i)
        {
            if (i >= f.Length - 1)
                throw new Exception("Wrong vector F index!");
            return -f[i + 1];
        }
        #endregion

        #region Обчислення допоміжних коефіцієнтів
        protected void CalculateAlpha()
        {
            alpha[0] = getKapa1(); //це капа1
            int tmp_index = driveIndex + 1;
            if (tmp_index > A3.Height - 1)
                --tmp_index;
            for (int i = 0; (i + 1) < tmp_index; ++i)
            {
                alpha[i + 1] = b_c(i) / (c_c(i) - alpha[i] * a_c(i));
            }
        }

        protected void CalculateBeta()
        {
            beta[0] = getM1(); //це м'ю1
            int tmp_index = driveIndex + 1;
            if (tmp_index > A3.Height - 1)
                --tmp_index;
            for (int i = 0; (i + 1) < tmp_index; ++i)
            {
                beta[i + 1] = (a_c(i) * beta[i] + f_c(i)) / (c_c(i) - alpha[i] * a_c(i));
            }
        }

        protected void CalculateKsi()
        {
            ksi[ksi.Length - 1] = getKapa2(); //це капа2
            for (int i = ksi.Length - 2; i >= driveIndex; --i)
            {
                ksi[i] = a_c(i) / (c_c(i) - ksi[i + 1] * b_c(i));
            }
        }

        protected void CalculateEta()
        {
            eta[eta.Length - 1] = getM2(); //це м'ю2
            for (int i = eta.Length - 2; i >= driveIndex; --i)
            {
                eta[i] = (b_c(i) * eta[i + 1] + f_c(i)) / (c_c(i) - ksi[i + 1] * b_c(i));
            }
        }

        protected void UpdateCoefficients()
        {
            CalculateAlpha();
            CalculateBeta();
            CalculateKsi();
            CalculateEta();
        }
        #endregion

        //перевизначений метод для будування розв'язку
        public override void SolveSLAR()
        {            
            if (driveIndex == A3.Height - 1)
            {
                driveIndex = x.Length - 1;
            }
            MakeCorrectness();                                    
            UpdateCoefficients(); 
            //знайдемо початковий x
            if (driveIndex == 0) // вибрали ліву прогонку
                x[driveIndex] = (beta[driveIndex] + alpha[driveIndex] * eta[driveIndex]) / 
                    (1 - alpha[driveIndex] * ksi[driveIndex]);
            else
                if (driveIndex >= A3.Height - 1) //вибрали праву прогонку
                {
                    driveIndex = x.Length - 1;
                    x[driveIndex] = (beta[driveIndex - 1] * getKapa2() + getM2()) /
                        (1 - alpha[driveIndex - 1] * getKapa2());
                }
                else //Х десь всередині
                {                    
                    x[driveIndex] = (beta[driveIndex] + alpha[driveIndex] * eta[driveIndex]) /
                        (1 - alpha[driveIndex] * ksi[driveIndex]);
                }
            //обчислимо тепер всі [x]
            int i = driveIndex - 1;
            //частина правою прогонкою
            for (; i >= 0; --i)
            {
                x[i] = alpha[i] * x[i + 1] + beta[i];
            }
            //а частина - лівою
            for (i = driveIndex; i < x.Length - 1; ++i)
            {
                x[i + 1] = ksi[i] * x[i] + eta[i];
            }
        }

        public override void PrintResults()
        {
            Console.Write("Vector X: ( ");
            foreach (double _x in x)
            {
                Console.Write(_x + " ");
            }
            Console.WriteLine(")T");            
        }

        public override void PrintCurrent()
        {
            if (A3 != null)
            {
                for (int i = 0; i < A3.Height; ++i)
                {
                    for (int j = 0; j < A3.Width; ++j)
                    {
                        Console.Write(A3[i, j] + " ");
                    }
                    Console.WriteLine(" | " + b[i]);
                }
            }
        }

        public override bool SolutionMatches()
        {
            double[] res = A3.MulVector(x);
            bool state = true;
            for (int i = 0; i < A3.Height; ++i)
            {
                if (Math.Abs(res[i] - b[i]) > epsilon)
                {
                    state = false;
                    break;
                }
            }
            return state;
        }

        public override double[] GetProduct()
        {
            return A3.MulVector(x);
        }
    }

    static class DriveHelper
    {
        public static void CalculateCoefficients(out double[] temp_a, out double[] temp_c,
            out double[] temp_b, out double[] temp_f)
        {
            int n = 15; // 7
            int i = 0;
            //ініціалізація f для [n == 7]
            temp_f = new double[n];
            for (; i < n - 1; ++i)
            {
                temp_f[i] = (i + 1) * (i + 1) + 14 * (i + 1) - 1;
            }
            temp_f[n - 1] = 600;//202;//600

            //ініціалізація a
            temp_a = new double[n - 1];
            for (i = 0; i < temp_a.Length; ++i)
            {
                temp_a[i] = -(1 + (i + 1));
            }

            //ініціалізація b
            temp_b = new double[n - 1];
            for (i = 0; i < temp_b.Length; ++i)
            {
                temp_b[i] = (i + 1);
            }

            //ініціалізація c
            temp_c = new double[n];
            for (i = 0; i < n; ++i)
            {
                temp_c[i] = 15 + (i + 1);
            }
        }
    }
}
