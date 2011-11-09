using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using LongInt;
using LongInt.Math.Special;

namespace JacobiSymbolControl
{
    public partial class JacobiSymbolControl : UserControl
    {
        public JacobiSymbolControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SLongIntB A = new SLongIntB(this.textBox1.Text);
            SLongIntB B = new SLongIntB(this.textBox2.Text);
            this.textBox3.Text = CryptoMath.JacobiSymbol(A, B).ToString();
        }

        public override string ToString()
        {
            return "Jacobi symbol demo";
        }
    }
}
