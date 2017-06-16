using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using LongInt;
using LongInt.Math;
using LongInt.Math.Special;

namespace eXtendedGCD
{
    public partial class eXtendedGCDControl : UserControl
    {
        public eXtendedGCDControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SLongIntB number1 = new SLongIntB(this.textBox1.Text);
                SLongIntB number2 = new SLongIntB(this.textBox2.Text);

                SLongIntB U = null;
                SLongIntB V = null;

                SLongIntB gcd = CryptoMath.eXtendedGCD(number1, number2, out U, out V);

                if (gcd != number1 * U + number2 * V)
                {
                    this.label7.Text = "Error!";
                    this.label7.ForeColor = Color.Red;
                }
                else
                {
                    this.label7.ForeColor = Color.ForestGreen;
                    this.label7.Text = "All ok!";
                }

                this.textBox3.Text = gcd.ToString();
                this.textBox4.Text = U.ToString();
                this.textBox5.Text = V.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override string ToString()
        {
            return "eXtended GCD Calculation";
        }
    }
}
