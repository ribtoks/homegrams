using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using LetterFrequency;

namespace LetterFrequencyCalculation
{
    public partial class Form1 : Form
    {
        LetterCounter lcounter = new LetterCounter();
        Language currentLanguage = Language.English;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = File.ReadAllText(this.openFileDialog1.FileName);
                this.groupBox1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lcounter.CalculateFrequency(this.textBox1.Text, currentLanguage);
            this.groupBox1.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            StatisticsForm sf = new StatisticsForm(lcounter, currentLanguage);
            sf.ShowDialog();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                currentLanguage = Language.English;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                currentLanguage = Language.Ukrainian;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                currentLanguage = Language.Russian;
        }
    }
}
