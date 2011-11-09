using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using LongInt;
using LongInt.Math;

namespace BinaryPowerModule
{
    public partial class BinaryPowerControl : UserControl
    {
        public BinaryPowerControl()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "Binary Power Module";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SLongIntD number = new SLongIntD(this.textBox1.Text);

                ulong value = (ulong)this.numericUpDown1.Value;

                SLongIntD powerValue = LongMath.Power(number, value);

                this.textBox2.Text = powerValue.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
