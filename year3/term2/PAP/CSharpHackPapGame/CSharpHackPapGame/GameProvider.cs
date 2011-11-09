using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CSharpHackPapGame
{
    public class DeadPoint
    {
        private Point p;

        public Point P
        {
            get { return p; }
            set { p = value; }
        }

        private int timeOut = 20;

        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }

        public DeadPoint(Point point)
        {
            p = new Point(point.X, point.Y);
        }
    }

    public class GameProvider
    {
        List<Mosquito> list1;
        List<Mosquito> list2;
        List<Mosquito> list3;

        List<DeadPoint> bloodPlaces;

        Bitmap gameImage;
        Bitmap mosquitoImage;
        Bitmap bloodImage;

        Random rand = new Random(DateTime.Now.Millisecond);

        int currCount;

        public int CurrCount
        {
            get { return currCount; }            
        }

        int delta = 40;
        
        public GameProvider(int pictureWidth, int pictureHeight, Image MosquitoImage, Image BloodImage)
        {
            list1 = new List<Mosquito>();
            list2 = new List<Mosquito>();
            list3 = new List<Mosquito>();

            gameImage = new Bitmap(pictureWidth, pictureHeight);

            mosquitoImage = new Bitmap(MosquitoImage);
            bloodImage = new Bitmap(BloodImage);

            bloodPlaces = new List<DeadPoint>();            
        }

        public bool HitPoint(Point p)
        {
            bool state = false;
            int i = 0;            
            while(i < list1.Count)
            {
                if (list1[i].IsOnDelta(p, delta))
                {
                    //list1.RemoveAt(i);
                    --currCount;
                    bloodPlaces.Add(new DeadPoint(new Point(p.X + 30, p.Y + 28)));
                    state = true;
                }
                ++i;
            }
            list1.RemoveAll(mosq => mosq.IsOnDelta(p, delta));

            i = 0;
            while (i < list2.Count)
            {
                if (list2[i].IsOnDelta(p, delta))
                {
                    //list2.RemoveAt(i);
                    --currCount;
                    bloodPlaces.Add(new DeadPoint(new Point(p.X + 30, p.Y + 28)));
                    state = true;
                }
                ++i;
            }
            list2.RemoveAll(mosq => mosq.IsOnDelta(p, delta));

            i = 0;
            while (i < list3.Count)
            {
                if (list3[i].IsOnDelta(p, delta))
                {
                    //list3.RemoveAt(i);
                    --currCount;
                    bloodPlaces.Add(new DeadPoint(new Point(p.X + 30, p.Y + 28)));
                    state = true;
                }
                ++i;
            }
            list3.RemoveAll(mosq => mosq.IsOnDelta(p, delta));

            return state;
        }

        public void RefreshDeadPoints()
        {
            for (int i = 0; i < bloodPlaces.Count; ++i)
                --bloodPlaces[i].TimeOut;
            int howMany = bloodPlaces.RemoveAll(place => place.TimeOut <= 0);
        }

        public void RefreshLists(int width, int height)
        {
            int i = 0;
            for (i = 0; i < list1.Count; ++i)
                list1[i].ProvideIteration(width, height);

            for (i = 0; i < list2.Count; ++i)
                list2[i].ProvideIteration(width, height);

            for (i = 0; i < list3.Count; ++i)
                list3[i].ProvideIteration(width, height);
        }

        private void UpdatePicture()
        {
            Graphics gc = Graphics.FromImage(gameImage);
            gc.Clear(Color.White);
            int i = 0;
            //drawing dead points
            for (i = 0; i < bloodPlaces.Count; ++i)
                gc.DrawImage(bloodImage, bloodPlaces[i].P);

            DrawMosquitos(ref gc, ref list1);
            DrawMosquitos(ref gc, ref list2);
            DrawMosquitos(ref gc, ref list3);
        }

        private void DrawMosquitos(ref Graphics gc, ref List<Mosquito> list)
        {
            for (int i = 0; i < list.Count; ++i)
                gc.DrawImage(mosquitoImage, new Rectangle(list[i].CurrPoint, 
                    new Size(mosquitoImage.Width + rand.Next(10), mosquitoImage.Height + rand.Next(10))));
        }

        public Image GetGameImage()
        {
            UpdatePicture();
            return gameImage;
        }

        public void AddMosquito1(Point p)
        {            
            Mosquito temp = new Mosquito(p);
            temp.DeltaX = rand.Next(3, 7) * Math.Sign((rand.Next() - rand.Next()));
            temp.DeltaY = rand.Next(3, 7) * Math.Sign((rand.Next() - rand.Next()));
            list1.Add(temp);
            ++currCount;
        }

        public void AddMosquito2(Point p)
        {
            Mosquito temp = new Mosquito(p);
            temp.DeltaX = rand.Next(6, 12) * Math.Sign((rand.Next() - rand.Next()));
            temp.DeltaY = rand.Next(6, 12) * Math.Sign((rand.Next() - rand.Next()));
            list2.Add(temp);
            ++currCount;
        }

        public void AddMosquito3(Point p)
        {
            Mosquito temp = new Mosquito(p);
            temp.DeltaX = rand.Next(12, 14) * Math.Sign((rand.Next() - rand.Next()));
            temp.DeltaY = rand.Next(12, 14) * Math.Sign((rand.Next() - rand.Next()));
            list3.Add(temp);
            ++currCount;
        }
    }
}
