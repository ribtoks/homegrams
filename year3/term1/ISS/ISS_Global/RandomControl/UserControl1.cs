using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using LongInt;

namespace RandomControl
{
    public partial class RandomControl : UserControl
    {
        RandomLong rand = new RandomLong((ulong)DateTime.Now.Millisecond);

        public RandomControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SLongIntB n = new SLongIntB(this.textBox1.Text);
                this.textBox2.Text = this.rand.Next(n).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occured", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SLongIntB p = new SLongIntB(this.textBox3.Text);
                SLongIntB q = new SLongIntB(this.textBox4.Text);
                this.textBox5.Text = this.rand.Next(p, q).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occured",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public override string ToString()
        {
            return "Random demo";
        }
    }
}
