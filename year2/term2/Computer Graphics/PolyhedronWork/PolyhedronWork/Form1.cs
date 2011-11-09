using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace PolyhedronWork
{
    public enum RunTimeMode { NewFigureCreation, FigureModifying, None };
    public enum ViewMode { Fixed, Aligned }

    public partial class Form1 : Form
    {
        #region Дані
        private Graphics gc = null;
        private Image coordAxes = null;
        private Point screenCenter;
        private int scaler = 25;

        private int offsetConst = 5;

        private int currCellCol;
        private int currCellRow;

        RunTimeMode CurrentMode = RunTimeMode.None;
        RunTimeMode LastMode = RunTimeMode.None;

        ViewMode viewMode = ViewMode.Aligned;

        //початкова фігура, що була задана у dataGrivView
        GeomFigure initialFigure;

        //фігура, що була модифікована
        GeomFigure lastModifiedFigure;

        //фігура, що відображає останні модифікації
        GeomFigure currentFigure;

        List<MyPoint> points;

        SimpleMatrix mulMatrix;
        bool useLast = true;
        #endregion


        public Form1()
        {
            InitializeComponent();            

            gc = splitContainer1.Panel2.CreateGraphics();
            gc.Clip = new Region(this.splitContainer1.Panel2.ClientRectangle);
            screenCenter.X = ((int)(gc.ClipBounds.Width)) >> 1;
            screenCenter.Y = ((int)(gc.ClipBounds.Height)) >> 1;

            //initializing figures
            initialFigure = new GeomFigure();
            lastModifiedFigure = new GeomFigure();
            currentFigure = new GeomFigure();
            //set scaling factor
            initialFigure.ScalingFactor = lastModifiedFigure.ScalingFactor = currentFigure.ScalingFactor = scaler;
            //generate all necessary objects
            GenerateCoordAxes();
            GenerateFiguresProperties();

            this.dataGridView1.DataError += new DataGridViewDataErrorEventHandler(dataGridView1_DataError);

            points = new List<MyPoint>();
        }

        void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// Generates private Image of axes of coordinates
        /// </summary>
        private void GenerateCoordAxes()
        {
            coordAxes = new Bitmap(splitContainer1.Panel2.Width, this.splitContainer1.Panel2.Height);
            Graphics coordGc = Graphics.FromImage(coordAxes);
            coordGc.Clear(Color.White);
            screenCenter.X = this.splitContainer1.Panel2.Width >> 1;
            screenCenter.Y = this.splitContainer1.Panel2.Height >> 1;
            Pen pen = new Pen(Color.Black);
            pen.Width = 1.5f;

            int xLength = this.splitContainer1.Panel2.Width - (offsetConst << 1);
            int yLength = this.splitContainer1.Panel2.Height - (offsetConst << 1);

            coordGc.DrawLine(pen, new Point(offsetConst, screenCenter.Y), new Point(this.splitContainer1.Panel2.Width - offsetConst, screenCenter.Y));
            coordGc.DrawLine(pen, new Point(screenCenter.X, offsetConst), new Point(screenCenter.X, this.splitContainer1.Panel2.Height - offsetConst));

            //signing axes of coordinates
            coordGc.DrawString("X", new Font(FontFamily.GenericMonospace, 10), Brushes.DarkBlue, 
                new Point(this.splitContainer1.Panel2.Width - (offsetConst << 2), screenCenter.Y - (offsetConst << 2)));
            coordGc.DrawString("Y", new Font(FontFamily.GenericMonospace, 10), Brushes.DarkBlue,
                new Point(screenCenter.X + offsetConst, offsetConst));

            //drawing mini-lines
            int i = 0;
            Point p1 = new Point(0, screenCenter.Y - (offsetConst >> 1));
            Point p2 = new Point(0, screenCenter.Y + (offsetConst >> 1));
            for (i = screenCenter.X + scaler; i < xLength; i += scaler)
            {
                p1.X = p2.X = i;
                coordGc.DrawLine(pen, p1, p2);
            }

            for (i = screenCenter.X - scaler; i >= offsetConst; i -= scaler)
            {
                p1.X = p2.X = i;
                coordGc.DrawLine(pen, p1, p2);
            }

            p1 = new Point(screenCenter.X - (offsetConst >> 1), 0);
            p2 = new Point(screenCenter.X + (offsetConst >> 1), 0);
            for (i = screenCenter.Y + scaler; i < yLength; i += scaler)
            {
                p1.Y = p2.Y = i;
                coordGc.DrawLine(pen, p1, p2);
            }

            for (i = screenCenter.Y - scaler; i >= offsetConst; i -= scaler)
            {
                p1.Y = p2.Y = i;
                coordGc.DrawLine(pen, p1, p2);
            }
        }

        private void DrawCoordAxes(ref Graphics gr)
        {
            gr.DrawImage(coordAxes, new Point(0, 0));
        }

        private void GenerateFiguresProperties()
        {
            initialFigure.OwnPen.Brush = Brushes.Silver;
            initialFigure.OwnPen.DashStyle = DashStyle.Dash;
            initialFigure.OwnPen.Width = 1f;

            initialFigure.ScalingFactor = scaler;

            lastModifiedFigure.OwnPen.Brush = Brushes.RoyalBlue;
            lastModifiedFigure.OwnPen.DashStyle = DashStyle.Dash;
            lastModifiedFigure.OwnPen.Width = 2f;

            lastModifiedFigure.ScalingFactor = scaler;

            currentFigure.OwnPen.Brush = Brushes.DarkBlue;
            currentFigure.OwnPen.DashStyle = DashStyle.Solid;
            currentFigure.OwnPen.Width = 2f;

            currentFigure.ScalingFactor = scaler;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics grf = e.Graphics;
            grf.CompositingQuality = CompositingQuality.HighQuality;
            grf.InterpolationMode = InterpolationMode.Bicubic;
            grf.SmoothingMode = SmoothingMode.AntiAlias;
            //draw axes of coordinates
            DrawCoordAxes(ref grf);
            if (CurrentMode == RunTimeMode.FigureModifying)
            {
                initialFigure.Draw(ref grf, offsetConst);
                lastModifiedFigure.Draw(ref grf, offsetConst);
                currentFigure.Draw(ref grf, offsetConst);
            }
            else
                if (CurrentMode == RunTimeMode.NewFigureCreation)
                {
                    foreach (MyPoint p in points)
                    {
                        p.Draw(new Point(screenCenter.X - offsetConst, screenCenter.Y - offsetConst), scaler, ref grf, Pens.RoyalBlue);
                    }
                }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //initialize structures
            points.Clear();
            this.groupBox1.Enabled = true;
            LastMode = CurrentMode;
            CurrentMode = RunTimeMode.NewFigureCreation;
            this.button4.Enabled = false;

            this.dataGridView1.DataSource = null;
            points.Add(new MyPoint());
            this.dataGridView1.DataSource = points;
            UpdateAxes(sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (points.Count < 2)
            {
                MessageBox.Show("Must be at least 2 points!", "Polyhendron work", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            LastMode = CurrentMode;
            CurrentMode = RunTimeMode.FigureModifying;
            this.groupBox1.Enabled = false;
            this.button4.Enabled = true;

            initialFigure = new GeomFigure(points, screenCenter);
            lastModifiedFigure = new GeomFigure(points, screenCenter);
            currentFigure = new GeomFigure(points, screenCenter);

            GenerateFiguresProperties();
            UpdateAxes(sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CurrentMode = LastMode;
            this.groupBox1.Enabled = false;
            this.button4.Enabled = true;
        }

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            if (viewMode == ViewMode.Aligned)
            {
                GenerateCoordAxes();
                if (gc == null)
                    return;
                gc = this.splitContainer1.Panel2.CreateGraphics();

                gc.Clip = new Region(this.splitContainer1.Panel2.ClientRectangle);
                screenCenter.X = ((int)(gc.ClipBounds.Width)) >> 1;
                screenCenter.Y = ((int)(gc.ClipBounds.Height)) >> 1;

                initialFigure.ScreenCenterCoords = new Point(screenCenter.X, screenCenter.Y);
                lastModifiedFigure.ScreenCenterCoords = new Point(screenCenter.X, screenCenter.Y);
                currentFigure.ScreenCenterCoords = new Point(screenCenter.X, screenCenter.Y);

                UpdateAxes(sender);
            }
        }
/*
        private void button5_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            points.Add(new MyPoint());
//            this.splitContainer1_Panel2_Paint(sender, new PaintEventArgs(gc, gc.ClipBounds as RectangleF));
            this.dataGridView1.DataSource = points;
        }
*/
        private void UpdateAxes(object sender)
        {
            PaintEventArgs pea = new PaintEventArgs(gc, splitContainer1.Panel2.ClientRectangle);
            this.splitContainer1_Panel2_Paint(sender, pea);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateAxes(sender);
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currCellCol = e.ColumnIndex;
                currCellRow = e.RowIndex;

                Point p = new Point(e.X, e.Y);

                p.X += this.dataGridView1.RowHeadersWidth;
                p.Y += this.dataGridView1.ColumnHeadersHeight;

                p.X += this.dataGridView1.Location.X + this.groupBox1.Location.X + this.tabPage1.Left;
                p.Y += this.dataGridView1.Location.Y + this.groupBox1.Location.Y + this.tabPage1.Top;

                int cellWidth = (this.dataGridView1.Columns.GetColumnsWidth(DataGridViewElementStates.None) / this.dataGridView1.Columns.Count);

                p.Y += currCellRow * this.dataGridView1.RowTemplate.Height;
                p.X += currCellCol * cellWidth;

                p = this.PointToScreen(p);
                this.contextMenuStrip1.Show(p, ToolStripDropDownDirection.BelowRight);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            points.Add(new MyPoint());
            this.dataGridView1.DataSource = points;
            UpdateAxes(sender);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.RowCount == 1)
                return;
            this.dataGridView1.DataSource = null;
            if (points.Count > 0)
                points.RemoveAt(currCellRow);
            this.dataGridView1.DataSource = points;
            UpdateAxes(sender);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked)
                this.panel1.Enabled = true;
            else
                this.panel1.Enabled = false;
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 3)
            {
                this.dataGridView2.Rows.Clear();
                mulMatrix = SimpleMatrix.Get_E(3);                
                this.dataGridView2.Rows.Add(3);
                for (int i = 0; i < 3; ++i)
                    this.dataGridView2.Rows[i].Cells[i].Value = (object)1.0;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //initialize matrix
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                {
                    if (this.dataGridView2.Rows[i].Cells[j].Value != null)
                        mulMatrix[i, j] = Convert.ToDouble(this.dataGridView2.Rows[i].Cells[j].Value.ToString());
                    else
                        mulMatrix[i, j] = 0;
                }

            if (initialFigure == null)
                return;

            if (useLast)
            {
                lastModifiedFigure = new GeomFigure(currentFigure);
                currentFigure.MulMatrix(mulMatrix);
            }
            else
            {
                initialFigure.MulMatrix(mulMatrix);
                lastModifiedFigure = new GeomFigure(initialFigure);
                currentFigure = new GeomFigure(initialFigure);
            }

            GenerateFiguresProperties();
            UpdateAxes(sender);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateAxes(sender);
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                double d = 0;
                bool b = Double.TryParse(this.dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out d);
                if (!b)
                {
                    MessageBox.Show("Error occured while parsing value in " + e.ToString());
                    this.dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (object)0;
                }
            }
            catch
            {
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
                useLast = true;
            else
                useLast = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox2.Text.Length == 0  ||  this.textBox2.Text == "-")
                return;
            double d = 0;
            bool b = Double.TryParse(this.textBox2.Text, out d);
            if (!b)
            {
                MessageBox.Show("Error occured while parsing value in " + e.ToString());
                this.textBox2.Text = "0";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox3.Text.Length == 0 || this.textBox3.Text == "-")
                return;
            double d = 0;
            bool b = Double.TryParse(this.textBox3.Text, out d);
            if (!b)
            {
                MessageBox.Show("Error occured while parsing value in " + e.ToString());
                this.textBox3.Text = "0";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length == 0 || this.textBox1.Text == "-")
                return;

            double d = 0;
            bool b = Double.TryParse(this.textBox1.Text, out d);
            if (!b)
            {
                MessageBox.Show("Error occured while parsing value in " + e.ToString());
                this.textBox1.Text = "90";
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox7.Text.Length == 0)
                return;
            double d = 0;
            bool b = Double.TryParse(this.textBox7.Text, out d);
            if (!b)
            {
                MessageBox.Show("Error occured while parsing value in " + e.ToString());
                this.textBox7.Text = "1,0";
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox6.Text.Length == 0)
                return;
            double d = 0;
            bool b = Double.TryParse(this.textBox6.Text, out d);
            if (!b)
            {
                MessageBox.Show("Error occured while parsing value in " + e.ToString());
                this.textBox6.Text = "1,0";
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox5.Text.Length == 0 || this.textBox5.Text == "-")
                return;
            double d = 0;
            bool b = Double.TryParse(this.textBox5.Text, out d);
            if (!b)
            {
                MessageBox.Show("Error occured while parsing value in " + e.ToString());
                this.textBox5.Text = "0";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox4.Text.Length == 0 || this.textBox4.Text == "-")
                return;
            double d = 0;
            bool b = Double.TryParse(this.textBox4.Text, out d);
            if (!b)
            {
                MessageBox.Show("Error occured while parsing value in " + e.ToString());
                this.textBox4.Text = "0";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Length == 0)
            {                
                MessageBox.Show("There is no angle!");
                this.textBox1.Focus();
                return;
            }

            if (initialFigure == null)
                return;

            if (useLast)
            {
                lastModifiedFigure = new GeomFigure(currentFigure);
                //not relatively to point
                if (this.checkBox2.Checked == false)
                    currentFigure.Rotate(Convert.ToDouble(this.textBox1.Text), this.checkBox1.Checked);
                else
                {
                    if (this.textBox2.Text.Length == 0 || this.textBox3.Text.Length == 0)
                    {
                        MessageBox.Show("There is no Point Coordinates!");
                        this.textBox2.Focus();
                        return;
                    }
                    currentFigure.RotateRelativelyToPoint(new MyPoint(Convert.ToDouble(this.textBox2.Text), Convert.ToDouble(this.textBox3.Text)),
                        Convert.ToDouble(this.textBox1.Text), this.checkBox1.Checked);
                }
            }
            else
            {
                //not relatively to point
                if (this.checkBox2.Checked == false)
                    initialFigure.Rotate(Convert.ToDouble(this.textBox1.Text), this.checkBox1.Checked);
                else
                    initialFigure.RotateRelativelyToPoint(new MyPoint(Convert.ToDouble(this.textBox2.Text), Convert.ToDouble(this.textBox3.Text)),
                        Convert.ToDouble(this.textBox1.Text), this.checkBox1.Checked);
                lastModifiedFigure = new GeomFigure(initialFigure);
                currentFigure = new GeomFigure(initialFigure);
            }

            GenerateFiguresProperties();
            UpdateAxes(sender);
        }

        //scales the figure
        private void button6_Click(object sender, EventArgs e)
        {
            if (initialFigure == null)
                return;

            if (this.textBox7.Text.Length == 0 || this.textBox6.Text.Length == 0)
            {
                MessageBox.Show("There is no Scale factor!");
                this.textBox7.Focus();
                return;
            }

            if (useLast)
            {
                lastModifiedFigure = new GeomFigure(currentFigure);
                currentFigure.Scale(Convert.ToDouble(this.textBox7.Text), Convert.ToDouble(this.textBox6.Text));
            }
            else
            {
                initialFigure.Scale(Convert.ToDouble(this.textBox7.Text), Convert.ToDouble(this.textBox6.Text));
                lastModifiedFigure = new GeomFigure(initialFigure);
                currentFigure = new GeomFigure(initialFigure);
            }
            GenerateFiguresProperties();

            UpdateAxes(sender);
        }

        //shifts the figure
        private void button7_Click(object sender, EventArgs e)
        {
            if (initialFigure == null)
                return;

            if (this.textBox5.Text.Length == 0 || this.textBox4.Text.Length == 0)
            {
                MessageBox.Show("There is no Scale factor!");
                this.textBox5.Focus();
                return;
            }

            if (useLast)
            {
                lastModifiedFigure = new GeomFigure(currentFigure);
                currentFigure.MoveOnDelta(Convert.ToDouble(this.textBox5.Text), Convert.ToDouble(this.textBox4.Text));
            }
            else
            {
                initialFigure.MoveOnDelta(Convert.ToDouble(this.textBox5.Text), Convert.ToDouble(this.textBox4.Text));
                lastModifiedFigure = new GeomFigure(initialFigure);
                currentFigure = new GeomFigure(initialFigure);
            }

            GenerateFiguresProperties();
            UpdateAxes(sender);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            scaler = trackBar1.Value * 5;
            //update all
            initialFigure.ScalingFactor = scaler;
            lastModifiedFigure.ScalingFactor = scaler;
            currentFigure.ScalingFactor = scaler;
            GenerateCoordAxes();
            UpdateAxes(sender);
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            if (viewMode == ViewMode.Aligned)            
                viewMode = ViewMode.Fixed;
            else
                viewMode = ViewMode.Aligned;
            UpdateAxes(sender);
        }
    }
}
