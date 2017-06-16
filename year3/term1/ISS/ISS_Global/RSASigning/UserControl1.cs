using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using LongInt;
using RSAlib;

namespace RSASigning
{
    public partial class UserControl1 : UserControl
    {
        RSAKeyGenerator keyGen = new RSAKeyGenerator((ulong)DateTime.Now.Millisecond);
        List<ULongIntB> encodedText;

        public UserControl1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            keyGen.GenerateRSAKey(BitLength.Small);
            this.panel1.Enabled = true;
        }
    }
}
