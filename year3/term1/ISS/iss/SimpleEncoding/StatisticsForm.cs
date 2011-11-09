using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LetterFrequency;

namespace SimpleEncoding
{
    public partial class StatisticsForm : Form
    {
        LetterCounter counter;
        Language currentLanguage;

        public StatisticsForm(LetterCounter from, Language language)
        {
            InitializeComponent();

            this.dataGridView1.DataSource = null;
            
            counter = from;
            
            this.dataGridView1.DataSource = counter.GetLettersPersentage(language);
            this.dataGridView1.Columns["Key"].Width = 20;

            switch (language)
            {
                case Language.English:
                    this.radioButton1.Checked = true;
                    break;

                case Language.Ukrainian:
                    this.radioButton2.Checked = true;
                    break;
                case Language.Russian:
                    this.radioButton3.Checked = true;
                    break;
            }

            currentLanguage = language;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked)
            {
                this.dataGridView1.DataSource = counter.GetLettersPersentage(Language.English);
                this.dataGridView1.Columns["Key"].Width = 30;
                currentLanguage = Language.English;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked)
            {
                this.dataGridView1.DataSource = counter.GetLettersPersentage(Language.Ukrainian);
                this.dataGridView1.Columns["Key"].Width = 30;
                currentLanguage = Language.Ukrainian;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton3.Checked)
            {
                this.dataGridView1.DataSource = counter.GetLettersPersentage(Language.Russian);
                this.dataGridView1.Columns["Key"].Width = 30;
                currentLanguage = Language.Russian;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.FileName = currentLanguage.ToString() + "Frequency" + DateTime.Now.Millisecond;
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = this.saveFileDialog1.FileName;
                if (!filePath.EndsWith(".lft"))
                    filePath += ".lft";

                counter.SaveLangFreqTableToFile(filePath, currentLanguage);
            }
        }
    }
}
