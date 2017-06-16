using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using LongInt;
using RSAlib;

namespace RSADemoControl
{
    public partial class RSADemoControl : UserControl
    {
        private Dictionary<int, BitLength> bitLengths;
        private RSAKeyGenerator keyGen;

        ULongIntB encoded;
        ULongIntB decoded;

        public RSADemoControl()
        {
            InitializeComponent();

            this.bitLengths = new Dictionary<int, BitLength>();
            Array values = Enum.GetValues(typeof(BitLength));
            int key = 0;
            foreach (object obj in values)
            {
                this.comboBox1.Items.Add(obj);
                this.bitLengths.Add(key, (BitLength)obj);
                key++;
            }


            keyGen = new RSAKeyGenerator((ulong)DateTime.Now.Millisecond);

        }

        public override string ToString()
        {
            return "RSA Demo";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("No bit length is selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            this.panel1.Enabled = false;

            this.label4.Text = "Please, wait while generating prime numbers...";
            this.label4.Update();
            keyGen.GenerateRSAKey(this.bitLengths[this.comboBox1.SelectedIndex]);
            this.label4.Text = "Prime numbers are generated!";

            this.panel1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox1.Text.Length == 0)
                {
                    MessageBox.Show("No message is entered...", "Error");
                }
                else
                {
                    ULongIntB number = new ULongIntB(this.textBox1.Text);
                    this.encoded = RSAEncoder.Encode(keyGen.GetPublicKey(), number);
                    this.textBox2.Text = this.encoded.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occured",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.textBox2.Text.Length == 0)
                {
                    MessageBox.Show("No encoded message is entered...", "Error");
                }
                else
                {
                    ULongIntB number = new ULongIntB(this.textBox2.Text);
                    this.decoded = RSADecoder.Decode(keyGen.GetPrivateKey(), number);
                    this.textBox1.Text = this.decoded.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error occured",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
