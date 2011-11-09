using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSharpHackPapGame
{
    public partial class Form1 : Form
    {
        GameProvider gameProvider;
        Bitmap zapperImage;
        Point currCursor;
        Bitmap currGameImage;

        bool isNowSmallZapper = false;

        Point lastMouse;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gameProvider = new GameProvider(this.Width, this.Height, Image.FromFile(Application.StartupPath + "/Mosquito.png"),
                Image.FromFile(Application.StartupPath + "/bloodItem.png"));
            currGameImage = new Bitmap(gameProvider.GetGameImage());

            zapperImage = new Bitmap(Application.StartupPath + "/Zapper.png");

            Random rand = new Random(DateTime.Now.Millisecond);
            gameProvider.AddMosquito1(new Point(rand.Next(this.Width), rand.Next(this.Height)));
            for (int i = 0; i < 3; ++i)
            {
                gameProvider.AddMosquito2(new Point(rand.Next(this.Width), rand.Next(this.Height)));
                gameProvider.AddMosquito3(new Point(rand.Next(this.Width), rand.Next(this.Height)));
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameProvider != null)
            {
                Graphics gc = e.Graphics;
                gc.DrawImage(gameProvider.GetGameImage(), new Point());
            }
        }

        private void DrawZapper(ref Graphics gc)
        {
            if (!isNowSmallZapper)
            {
                gc.DrawImage(zapperImage, currCursor);
            }
            else
                gc.DrawImage(zapperImage, new Rectangle(currCursor, new Size(zapperImage.Width - 20, zapperImage.Height - 20)));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (gameProvider.CurrCount > 0)
            {
                gameProvider.RefreshLists(this.Width, this.Height);
                Graphics gc = this.CreateGraphics();
                currGameImage = new Bitmap(gameProvider.GetGameImage());
                gc.DrawImage(currGameImage, new Point());
                DrawZapper(ref gc);
            }
            else
            {
                bool time1 = timer1.Enabled;
                bool time2 = timer2.Enabled;
                bool time3 = timer3.Enabled;
                bool time4 = timer4.Enabled;

                timer1.Enabled = false;
                timer2.Enabled = false;
                timer3.Enabled = false;
                timer4.Enabled = false;
                
                if (MessageBox.Show("You is a winner!!! Game over." + Environment.NewLine + "Continue?", "MegaGame",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Random rand = new Random(DateTime.Now.Millisecond);
                    int i = 0;
                    for (i = 0; i < rand.Next(4); ++i)
                        gameProvider.AddMosquito1(new Point(rand.Next(this.Width), rand.Next(this.Height)));

                    for (i = 0; i < rand.Next(5); ++i)
                        gameProvider.AddMosquito2(new Point(rand.Next(this.Width), rand.Next(this.Height)));

                    for (i = 0; i < rand.Next(5); ++i)
                        gameProvider.AddMosquito3(new Point(rand.Next(this.Width), rand.Next(this.Height)));

                    timer1.Enabled = time1;
                    timer2.Enabled = time2;
                    timer3.Enabled = time3;
                    timer4.Enabled = time4;
                }
                else
                {
                    this.BackColor = Color.GhostWhite;
                    gameProvider = null;
                }
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (gameProvider == null)
                return;

            lastMouse = new Point(currCursor.X, currCursor.Y);
            currCursor = new Point(e.X - 40, e.Y - 50);
            if (Math.Abs(currCursor.X - lastMouse.X) > 5 || Math.Abs(currCursor.Y - lastMouse.Y) > 5)
            {
                Graphics gc = this.CreateGraphics();
                gc.DrawImage(currGameImage, new Point());
                DrawZapper(ref gc);
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameProvider == null)
                return;

            gameProvider.HitPoint(currCursor);
            isNowSmallZapper = true;
            this.timer4.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            gameProvider.RefreshDeadPoints();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            int i = 0;
            for (i = 0; i < rand.Next(4); ++i)
                gameProvider.AddMosquito1(new Point(rand.Next(this.Width), rand.Next(this.Height)));

            for (i = 0; i < rand.Next(5); ++i)
                gameProvider.AddMosquito2(new Point(rand.Next(this.Width), rand.Next(this.Height)));

            for (i = 0; i < rand.Next(5); ++i)
                gameProvider.AddMosquito3(new Point(rand.Next(this.Width), rand.Next(this.Height)));
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            isNowSmallZapper = false;
            this.timer4.Enabled = false;
        }
    }
}
