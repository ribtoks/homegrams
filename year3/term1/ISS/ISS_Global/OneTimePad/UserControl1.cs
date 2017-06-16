using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;

using LetterFrequency;

namespace OneTimePad
{
    public partial class OneTimePadControl : UserControl, ILanguageChangable
    {
        Language currentLanguage;
        string key;
        string encodedString;

        public OneTimePadControl()
        {
            InitializeComponent();
        }

        public void ChangeLanguage(Language newLanguage)
        {
            currentLanguage = newLanguage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                this.textBox1.Text = File.ReadAllText(this.openFileDialog1.FileName);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel2Collapsed = false;
            this.splitContainer1.Panel1Collapsed = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                encodedString = File.ReadAllText(this.openFileDialog1.FileName);
                this.textBox2.Text = encodedString;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Panel1Collapsed = false;
            this.splitContainer1.Panel2Collapsed = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            key = GetKey(this.textBox1.Text.Length);
            this.textBox3.Text = key;
            this.button4.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                File.WriteAllText(this.saveFileDialog1.FileName, this.textBox3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.textBox3.Text.Length != this.textBox1.Text.Length)
            {
                if (this.textBox3.Text.Length > this.textBox1.Text.Length)
                    key = this.textBox3.Text.Substring(0, this.textBox1.Text.Length);
                else
                    key = this.textBox3.Text + GetKey(this.textBox1.Text.Length - this.textBox3.Text.Length);
            }
            // now key is ready

            StringBuilder text = new StringBuilder(this.textBox1.Text);
            string textToEncode = this.textBox1.Text;

            for (int i = 0; i < this.textBox1.Text.Length; ++i)
                text[i] = (char)(textToEncode[i] ^ key[i]);

            encodedString = text.ToString();
            this.textBox2.Text = text.ToString();
            this.textBox4.Text = key;

            this.splitContainer1.Panel2Collapsed = false;
            this.splitContainer1.Panel1Collapsed = true;

            if (MessageBox.Show("Save encoded text into file?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                    File.WriteAllText(this.saveFileDialog1.FileName, encodedString);
            }
        }

        private string GetKey(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            Random rand = new Random(DateTime.Now.Millisecond);

            int langLettersCount = Languages.Strings[currentLanguage].Length;

            for (int i = 0; i < length; ++i)
                sb.Append(Languages.Strings[currentLanguage][rand.Next(langLettersCount)]);

            return sb.ToString();
        }

        public override string ToString()
        {
            return "One-time Pad";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                this.textBox4.Text = File.ReadAllText(this.openFileDialog1.FileName);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (this.textBox4.Text.Length == 0)
            {
                MessageBox.Show("No key entered.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            try
            {
                key = this.textBox4.Text;
                StringBuilder text = new StringBuilder(encodedString);
                string textToDecode = encodedString;

                for (int i = 0; i < textToDecode.Length; ++i)
                    text[i] = (char)(textToDecode[i] ^ key[i]);

                this.textBox1.Text = text.ToString();
                
                this.splitContainer1.Panel1Collapsed = false;
                this.splitContainer1.Panel2Collapsed = true;

                this.textBox3.Text = key;
            }
            catch
            {
                MessageBox.Show("Something wrong with key or input text...", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                this.textBox2.SelectAll();
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                this.textBox4.SelectAll();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                this.textBox3.SelectAll();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                this.textBox1.SelectAll();
        }
    }
}
