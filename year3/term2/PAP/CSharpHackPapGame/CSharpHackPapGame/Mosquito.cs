using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CSharpHackPapGame
{
    public class Mosquito
    {
        int deltaX;
        int deltaY;
        Point currPoint;

        public Mosquito(Point location)
        {
            currPoint = new Point(location.X, location.Y);            
        }

        public int DeltaX
        {
            get { return deltaX; }
            set { deltaX = value; }
        }

        public int DeltaY
        {
            get { return deltaY; }
            set { deltaY = value; }
        }

        public Point CurrPoint
        {
            get { return currPoint; }
            set { currPoint = value; }
        }

        public void ProvideIteration(int width, int height)
        {
            if ((currPoint.X + deltaX) < 0)
                deltaX *= -1;
            if ((currPoint.Y + deltaY) < 0)
                deltaY *= -1;

            currPoint.X = Math.Abs(currPoint.X + deltaX) % width;
            currPoint.Y = Math.Abs(currPoint.Y + deltaY) % height;
        }

        public bool IsOnDelta(Point p, int delta)
        {
            if (Math.Abs(currPoint.X - 30 - p.X) < delta && Math.Abs(currPoint.Y - 28 - p.Y) < delta)
                return true;
            return false;
        }
    }
}
