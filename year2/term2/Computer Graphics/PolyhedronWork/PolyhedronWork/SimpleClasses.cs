using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PolyhedronWork
{
    public static class MyPointStaticData
    {
        public static double Epsilon = 0.00000001;
    }

    public class MyPoint
    {
        #region Дані
        //coordinates
        private double x;
        private double y;
        #endregion

        //for data grid view
        public double X
        {
            get { return x; }
            set { x = value; }
        }
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        #region Конструктори
        public MyPoint()
        {
            x = 0;
            y = 0;
        }

        public MyPoint(int X, int Y)
        {
            x = (double)X;
            y = (double)Y;
        }

        public MyPoint(double X, double Y)
        {
            x = X;
            y = Y;
        }

        public MyPoint(MyPoint p)
            : this()
        {
            x = p.x;
            y = p.y;
        }
        #endregion        

        public Point GetDrawingPoint(Point CenterScreenCoords, double scalingFactor)
        {
            double MaxInt = Int32.MaxValue / 100;
            double tempX = x * scalingFactor;
            if (Math.Abs(tempX) > MaxInt)
                tempX = MaxInt * Math.Sign(tempX);
            double tempY = y * scalingFactor;
            if (Math.Abs(tempY) > MaxInt)
                tempY = MaxInt * Math.Sign(tempY);
            return new Point(CenterScreenCoords.X + (int)(tempX), CenterScreenCoords.Y - (int)(tempY));
        }

        public Point GetDrawingPoint(Point CenterScreenCoords, double scalingFactor, int offset)
        {
            double MaxInt = Int32.MaxValue / 100;
            double tempX = x * scalingFactor;
            if (Math.Abs(tempX) > MaxInt)
                tempX = MaxInt * Math.Sign(tempX);
            double tempY = y * scalingFactor;
            if (Math.Abs(tempY) > MaxInt)
                tempY = MaxInt * Math.Sign(tempY);
            return new Point(CenterScreenCoords.X + (int)(tempX) - offset, CenterScreenCoords.Y - (int)(tempY) - offset);
        }

        public void Draw(Point CenterScreenCoords, double scalingFactor, ref Graphics gc, Pen pen)
        {
            Rectangle rect = new Rectangle(GetDrawingPoint(CenterScreenCoords, scalingFactor), new Size(8, 8));
            gc.FillEllipse(pen.Brush, rect);
        }

        public void Draw(Point CenterScreenCoords, double scalingFactor, ref Graphics gc, Pen pen, int offset)
        {
            try
            {                
                Rectangle rect = new Rectangle(GetDrawingPoint(CenterScreenCoords, scalingFactor, offset), new Size(8, 8));
                gc.FillEllipse(pen.Brush, rect);
            }
            catch(OverflowException e)
            {
                System.Windows.Forms.MessageBox.Show("Overflow exception occured! Great loss of precision!", "Polyhendron work", 
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// rotation of point
        /// </summary>
        /// <param name="angle">Value of angle</param>
        /// <param name="IsVeer">Rotate by clockwise</param>
        public void Rotate(double angle, bool IsClockwise)
        {            
            if (IsClockwise)
                angle *= -1;
            //initializing structure
            double radians = Math.PI * (angle / 180.0);
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);
            SimpleMatrix rotateMatrix = new SimpleMatrix(2, 2);
            rotateMatrix[0][0] = cos;
            rotateMatrix[0][1] = -sin;
            rotateMatrix[1][0] = sin;
            rotateMatrix[1][1] = cos;
            //and multiply
            MulSimpleMatrix(rotateMatrix);
        }

        private void MulSimpleMatrix(SimpleMatrix sm)
        {
            double[] vect = new double[2];
            vect[0] = x;
            vect[1] = y;
            double[] newCoords = (double[])sm.MulVector(vect);
            x = newCoords[0]; // (newCoords[0] < MyPointStaticData.Epsilon) ? (MyPointStaticData.Epsilon * Math.Sign(newCoords[0])) : newCoords[0];
            y = newCoords[1]; // (newCoords[1] < MyPointStaticData.Epsilon) ? (MyPointStaticData.Epsilon * Math.Sign(newCoords[1])) : newCoords[0];
        }

        /// <summary>
        /// Scales points on known factors
        /// </summary>
        /// <param name="xFactor">Scale factor of X-coordinate</param>
        /// <param name="yFactor">Scale factor of Y-coordinate</param>
        public void Scale(double xFactor, double yFactor)
        {
            //initialize structure
            SimpleMatrix scaleMatrix = new SimpleMatrix(2, 2);
            scaleMatrix[0][0] = xFactor;
            scaleMatrix[0][1] = 0;
            scaleMatrix[1][0] = 0;
            scaleMatrix[1][1] = yFactor;
            //and multiply
            MulSimpleMatrix(scaleMatrix);
        }

        public void MoveOnDelta(double deltaX, double deltaY)
        {
            x += deltaX;
            y += deltaY;
        }

        public void MulMatrix(SimpleMatrix Matrix)
        {
            double[] vector = new double[3];
            vector[0] = x;
            vector[1] = y;
            vector[2] = 1;
            Matrix = SimpleMatrix.GetTranspose(Matrix);
            double[] result = Matrix.MulVector(vector);
            x = result[0];
            y = result[1];
        }
        
        /// <summary>
        /// Rotates current point relatively to fixed point
        /// </summary>
        /// <param name="p">Fixed point</param>
        /// <param name="angle">Value of angle</param>
        /// <param name="IsClockwise">Rotate by clockwise</param>
        public void RotateRelativelyToPoint(MyPoint p, double angle, bool IsClockwise)
        {
            x -= p.x;
            y -= p.y;
            Rotate(angle, IsClockwise);
            x += p.x;
            y += p.y;
        }
    }

    public class GeomFigure
    {
        #region Дані

        //apices of a figure
        private List<MyPoint> apices;

        private Pen ownPen;
        private Point ScreenCenter;

        public Point ScreenCenterCoords
        {
            get { return ScreenCenter; }
            set { ScreenCenter = value; }
        }
        private double scalingFactor = 1.0;

        #endregion

        private List<MyPoint> CloneList(List<MyPoint> from)
        {
            List<MyPoint> list = new List<MyPoint>(from.Count);
            for (int i = 0; i < from.Count; ++i)
                list.Add(new MyPoint(from[i]));
            return list;
        }

        public List<MyPoint> Apices
        {
            get { return apices; }
            set { apices = value; }
        }

        #region Конструктори

        public GeomFigure()
        {
            apices = new List<MyPoint>();
            ownPen = new Pen(Brushes.White);
        }

        public GeomFigure(int n, Point ScreenCenterCoords)
        {
            apices = new List<MyPoint>(n);
            ScreenCenter = new Point(ScreenCenterCoords.X, ScreenCenterCoords.Y);
            ownPen = new Pen(Brushes.White);
        }

        public GeomFigure(List<MyPoint> Apices, Point ScreenCenterCoords)
        {
            apices = CloneList(Apices);
            ScreenCenter = new Point(ScreenCenterCoords.X, ScreenCenterCoords.Y);
            ownPen = new Pen(Brushes.White);
        }

        public GeomFigure(GeomFigure fig)
        {
            apices = CloneList(fig.apices);      
            ScreenCenter = new Point(fig.ScreenCenter.X, fig.ScreenCenter.Y);
            ownPen = new Pen(fig.ownPen.Brush, fig.ownPen.Width);
        }
        #endregion

        public Pen OwnPen
        {
            get { return ownPen; }
            set { ownPen = value; }
        }

        public double ScalingFactor
        {
            get { return scalingFactor; }
            set { scalingFactor = value; }
        }

        /// <summary>
        /// Joines all Points together
        /// </summary>
        /// <param name="gc">Graphics to use</param>
        /// <param name="ApicesOffset">Offset oa apices of figure</param>
        public void Draw(ref Graphics gc, int ApicesOffset)
        {
            //Draw lines
            int i = 0;
            for (; i < apices.Count - 1; ++i)
                gc.DrawLine(ownPen, apices[i].GetDrawingPoint(ScreenCenter, scalingFactor), apices[i + 1].GetDrawingPoint(ScreenCenter, scalingFactor));
            gc.DrawLine(ownPen, apices[0].GetDrawingPoint(ScreenCenter, scalingFactor), apices[apices.Count - 1].GetDrawingPoint(ScreenCenter, scalingFactor));
            //draw all points
            for (i = 0; i < apices.Count; ++i)
                apices[i].Draw(ScreenCenter, scalingFactor, ref gc, ownPen, ApicesOffset);
        }

        /// <summary>
        /// Rotation of all point of figure
        /// </summary>
        /// <param name="angle">Value of angle</param>
        /// <param name="IsClockwise">Rotate by clockwise</param>
        public void Rotate(double angle, bool IsClockwise)
        {
            for (int i = 0; i < apices.Count; ++i)
                apices[i].Rotate(angle, IsClockwise);
        }

        /// <summary>
        /// Scale all point of figure
        /// </summary>
        /// <param name="xFactor">Scale factor of X-coordinate</param>
        /// <param name="yFactor">Scale factor of Y-coordinate</param>
        public void Scale(double xFactor, double yFactor)
        {
            for (int i = 0; i < apices.Count; ++i)
                apices[i].Scale(xFactor, yFactor);
        }

        /// <summary>
        /// Moves figure on fixed distance
        /// </summary>
        /// <param name="deltaX">X-delta</param>
        /// <param name="delatY">Y-delta</param>
        public void MoveOnDelta(double deltaX, double delatY)
        {
            for (int i = 0; i < apices.Count; ++i)
                apices[i].MoveOnDelta(deltaX, delatY);
        }

        public void MulMatrix(SimpleMatrix Matrix)
        {
            for (int i = 0; i < apices.Count; ++i)
                apices[i].MulMatrix(Matrix);
        }

        /// <summary>
        /// Rotates figure relatively to fixed point
        /// </summary>
        /// <param name="p">Fixed point</param>
        /// <param name="angle">Value of angle</param>
        /// <param name="IsClockwise">Rotate by clockwise</param>
        public void RotateRelativelyToPoint(MyPoint p, double angle, bool IsClockwise)
        {
            for (int i = 0; i < apices.Count; ++i)
                apices[i].RotateRelativelyToPoint(p, angle, IsClockwise);
        }


    }

    public class SimpleMatrix : System.Collections.IEnumerable
    {
        #region Дані

        private double[][] MArray; //у зубчастого масива швидший доступ до елементів і повільніший конструктор

        private int DimWidth;
        private int DimHeight;

        #endregion

        #region Конструктори
        
        public SimpleMatrix()
        {
            MArray = null;
            DimHeight = 0;
            DimWidth = 0;
        }
       
        public SimpleMatrix(int width, int height)
        {
            DimHeight = height;
            DimWidth = width;
            MArray = new double[height][];
            for (int i = 0; i < height; ++i)
                MArray[i] = new double[width];            
        }

        public SimpleMatrix(int Dimension)
            : this(Dimension, Dimension)
        {
        }

        public SimpleMatrix(SimpleMatrix CopyFrom)
        {
            DimHeight = CopyFrom.DimHeight;
            DimWidth = CopyFrom.DimWidth;
            //для ссилок не працюватиме!!!
            MArray = new double[DimHeight][];
            for (int k = 0; k < DimHeight; ++k)
                MArray[k] = new double[CopyFrom.MArray[k].Length];

            for (int i = 0; i < MArray.Length; ++i)
                for (int j = 0; j < CopyFrom.MArray[i].Length; ++j)
                    MArray[i][j] = CopyFrom.MArray[i][j];
        }

        public SimpleMatrix(double[][] Data, bool IsColumns)
        {
            if (IsColumns == true)
            {
                DimHeight = Data[0].Length;
                DimWidth = Data.Length;
                MArray = new double[DimHeight][];
                int i = 0;
                for (; i < DimHeight; ++i)
                    MArray[i] = new double[DimWidth];
                for (i = 0; i < DimHeight; ++i)
                    for (int j = 0; j < DimWidth; ++j)
                        MArray[i][j] = Data[j][i];
            }
            else
            {
                DimHeight = Data.Length;
                DimWidth = Data[0].Length;
                MArray = new double[Data.Length][];
                for (int i = 0; i < Data.Length; ++i)
                    MArray[i] = (double[])Data[i].Clone();
            }            
        }        

        #endregion
        /*
        public double[][] MatrixArray
        {
            get { return MArray; }
            set { MArray = value; }
        }
        */
        //занулити елементи матриці
        public void NullMe()
        {
            for (int i = 0; i < DimHeight; ++i)
                for (int j = 0; j < DimWidth; ++j)
                    MArray[i][j] = 0;
        }

        public SimpleMatrix CopyOfMe()
        {
            SimpleMatrix ret = new SimpleMatrix(this);
            return ret;
        }

        [Browsable(false)]
        public int Width
        {
            get { return DimWidth; }
        }
        [Browsable(false)]
        public int Height
        {
            get { return DimHeight; }
        }

        //простий індексатор
        public double this[int h, int w]
        {
            get
            {
                if (h >= 0 & h < DimHeight & w >= 0 & w < DimWidth)
                    return MArray[h][w];
                else
                    throw new Exception("Indices out of Matrix dimensions!");
            }
            set
            {
                if (h >= 0 & h < DimHeight & w >= 0 & w < DimWidth)
                    MArray[h][w] = value;
                else
                    throw new Exception("Indices out of Matrix dimensions!");
            }
        }

        //рядковий індексатор
        public double[] this[int index]
        {
            get
            {
                if (index >= 0 & index < DimHeight)
                {
                    return MArray[index];
                }
                else
                    throw new Exception("Index out of Matrix dimensions!");
            }
            set
            {
                if (index >= 0 & index < DimHeight & value.Length == DimWidth)
                {
                    MArray[index] = value;
                }
                else
                    throw new Exception("Index out of Matrix dimensions!");
            }
        }

        #region Прохід по матриці
        public System.Collections.IEnumerator GetEnumerator()
        {
            for (int i = 0; i < DimHeight; ++i)
                for (int j = 0; j < DimWidth; ++j)
                {
                    yield return MArray[i][j];
                }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion        

        public static double ScalarProduct(double[] vect1, double[] vect2)
        {
            if (vect1.Length != vect2.Length)
                throw new Exception("Vectors have different size!");
            double res = 0;
            for (int i = 0; i < vect1.Length; ++i)
                res += vect1[i] * vect2[i];
            return res;
        }

        //множення матриці на вектор
        public double[] MulVector(double[] vector)
        {
            if (DimWidth != vector.Length)
                throw new Exception("Can not multiply matrix and vector! Different sizes!");
            double[] result = new double[DimHeight];
            for (int i = 0; i < DimHeight; ++i)
            {
                result[i] = ScalarProduct(vector, MArray[i]);
            }
            return result;
        }

        //множення двох матриць(A*B)
        public static SimpleMatrix Product(SimpleMatrix A, SimpleMatrix B)
        {
            SimpleMatrix C = null;
            if (A.Width != B.Height)
                return C;
            C = new SimpleMatrix(B.Width, A.Height);
            for (int i = 0; i < A.Height; ++i)
            {
                for (int j = 0; j < B.Width; ++j)
                {
                    double temp_sum = 0;
                    for (int k = 0; k < A.Width; ++k)
                        temp_sum += A[i, k] * B[k, j];
                    C[i, j] = temp_sum;
                }
            }
            return C;
        }

        public static SimpleMatrix Get_E(int N)
        {
            SimpleMatrix E = new SimpleMatrix(N, N);
            for (int i = 0; i < N; ++i)
                E.MArray[i][i] = 1;
            return E;
        }

        //транспонування матриці
        public static SimpleMatrix GetTranspose(SimpleMatrix from)
        {
            SimpleMatrix temp = new SimpleMatrix(from.Width, from.Height);
            for (int i = 0; i < from.Height; ++i)
                for (int j = 0; j < from.Width; ++j)
                    temp[i, j] = from[j, i];
            return temp;
        }
    }
}