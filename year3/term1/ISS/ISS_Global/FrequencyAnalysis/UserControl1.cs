using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;

using LetterFrequency;

namespace FrequencyAnalysis
{
    public partial class FrequencyAnalysisControl : UserControl, ILanguageChangable
    {
        private Language currentLanguage;

        // frequency table, that is read from file 
        // when frequency analysis is going
        private List<KeyValuePair<char, double>> readFrequency = null;

        public FrequencyAnalysisControl()
        {
            InitializeComponent();
        }

        public void ChangeLanguage(Language newLanguage)
        {
            currentLanguage = newLanguage;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                this.textBox6.Text = File.ReadAllText(this.openFileDialog1.FileName);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.label11.Text = Path.GetFileName(this.openFileDialog1.FileName);
                try
                {
                    readFrequency = (List<KeyValuePair<char, double>>)Languages.ReadFrequencyFromFile(this.openFileDialog1.FileName);
                }
                catch
                {
                    MessageBox.Show("Can not read frequencies from file.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.button14.Enabled = true;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                File.WriteAllText(this.saveFileDialog1.FileName, this.textBox7.Text);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            LetterCounter tempCounter = new LetterCounter();
            tempCounter.CalculateFrequency(this.textBox6.Text, currentLanguage);
            List<KeyValuePair<char, double>> persantege = tempCounter.GetLettersPersentage(currentLanguage);

            Dictionary<char, char> transformDictionary = new Dictionary<char, char>();

            if (persantege.Count != readFrequency.Count)
            {
                MessageBox.Show("Wrong frequency table. Load table of current language.", "Fatal error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < persantege.Count; ++i)
                transformDictionary.Add(persantege[i].Key, readFrequency[i].Key);

            StringBuilder sb = new StringBuilder(this.textBox6.Text.Length);

            for (int i = 0; i < this.textBox6.Text.Length; ++i)
            {
                if (transformDictionary.ContainsKey(this.textBox6.Text[i]))
                    sb.Append(transformDictionary[this.textBox6.Text[i]]);
                else
                    sb.Append(this.textBox6.Text[i]);
            }

            this.textBox7.Text = sb.ToString();
        }

        public override string ToString()
        {
            return "Frequency analysis";
        }
    }
}
