using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;

using LetterFrequency;

namespace LetterFrequencyCounter
{
    public partial class LetterFrequencyControl : UserControl, ILanguageChangable
    {
        LetterCounter lcounter = new LetterCounter();
        Language currentLanguage = Language.English;

        public LetterFrequencyControl()
        {
            InitializeComponent();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox5.Text = File.ReadAllText(this.openFileDialog1.FileName);
                this.groupBox1.Enabled = false;
            }
        }

        public void ChangeLanguage(Language newLanguage)
        {
            // do nothing - we have own radiogroup
        }

        private void button9_Click(object sender, EventArgs e)
        {
            lcounter.CalculateFrequency(this.textBox5.Text, currentLanguage);
            this.groupBox1.Enabled = true;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked)
                currentLanguage = Language.English;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
                currentLanguage = Language.Ukrainian;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked)
                currentLanguage = Language.Russian;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            StatisticsForm sf = new StatisticsForm(lcounter, currentLanguage);
            sf.ShowDialog();
        }

        public override string ToString()
        {
            return "Letter frequency counter";
        }
    }
}
