using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

using LetterFrequency;

namespace SimpleEncoding
{
    public partial class Form1 : Form
    {
        #region Form data

        CharEncoder charEncoder;

        LetterCounter lcounter = new LetterCounter();
        Language currentLanguage = Language.English;

        private const int KeyColumnWidth = 30;
        private const int ValueColumnWidth = 50;

        private Language currentDecodingLanguage = Language.English;
        private EncodingType currentDecodingType = EncodingType.Cycle;

        private List<KeyValuePair<char, double>> sourceFrequencyTable;
        private List<KeyValuePair<char, double>> destinationFrequencyTable;

        private Dictionary<EncodingType, TextDecodingDelegate> textDecoder;
        private LetterCounter decodedLetterCounter;

        // frequency table, that is read from file 
        // when frequency analysis is going
        private List<KeyValuePair<char, double>> readFrequency = null;

        #endregion

        private void UpdateDataGridView()
        {
            charEncoder.CurrentLanguage = currentLanguage;
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = charEncoder.EncodingListMap;

            this.dataGridView1.Columns["Key"].Width = KeyColumnWidth;
            this.dataGridView1.Columns["Value"].Width = ValueColumnWidth;
        }

        public Form1()
        {
            InitializeComponent();

            charEncoder = new CharEncoder(Language.English, EncodingType.Cycle);
            this.dataGridView1.DataSource = charEncoder.EncodingListMap;
            this.dataGridView1.Columns["Key"].Width = KeyColumnWidth;
            this.dataGridView1.Columns["Value"].Width = ValueColumnWidth;

            textDecoder = new Dictionary<EncodingType, TextDecodingDelegate>(3);
            textDecoder.Add(EncodingType.Cycle, TextDecoder.DecodeCycledText);
            textDecoder.Add(EncodingType.Shunting, TextDecoder.DecodeShuntedText);
            textDecoder.Add(EncodingType.Rijndael, TextDecoder.DecodeRijndaeledText);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = File.ReadAllText(this.openFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.FileName = charEncoder.CurrentLanguage.ToString() + "Text" + DateTime.Now.Millisecond;
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = this.saveFileDialog1.FileName;

                if (!filePath.EndsWith(".txt"))
                    filePath += ".txt";

                //File.WriteAllBytes(filePath, Encoding.UTF8.GetBytes(this.textBox2.Text));
                File.WriteAllText(filePath, this.textBox2.Text);

                MessageBox.Show("File was saved.", "Simple Encoding", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = this.textBox1.Text.Replace(Environment.NewLine, "");
            StringBuilder encoded = new StringBuilder();

            //start encoding
            for (int i = 0; i < text.Length; ++i)
            {
                if (!char.IsWhiteSpace(text[i]))
                {
                    char tempChar = text[i];
                    if (char.IsLetter(text[i]))
                        encoded.Append(charEncoder.EncodingMap[char.ToLower(text[i])]);
                    else
                        encoded.Append(text[i]);
                }
            }

            this.textBox2.Text = encoded.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                sourceFrequencyTable = Languages.ReadFrequencyFromFile(openFileDialog1.FileName);
                this.dataGridView2.DataSource = sourceFrequencyTable;
                //this.splitContainer2.Panel2.Enabled = true;
                this.button7.Enabled = true;

                this.dataGridView2.Columns["Key"].Width = KeyColumnWidth;
                this.dataGridView2.Columns["Value"].Width = ValueColumnWidth;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.textBox4.Text = textDecoder[currentDecodingType].Invoke(this.textBox3.Text, currentDecodingLanguage);
            this.button7.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            decodedLetterCounter = new LetterCounter();            
            decodedLetterCounter.CalculateFrequency(this.textBox4.Text, currentDecodingLanguage);
            destinationFrequencyTable = decodedLetterCounter.GetLettersPersentage(currentDecodingLanguage);

            this.dataGridView3.DataSource = destinationFrequencyTable;
            this.dataGridView3.Columns["Key"].Width = KeyColumnWidth;
            this.dataGridView3.Columns["Value"].Width = ValueColumnWidth;

            //check tables
            bool tablesEqual = true;

            if (destinationFrequencyTable.Count != sourceFrequencyTable.Count)
                tablesEqual = false;

            if (tablesEqual)
                for (int i = 0; i < destinationFrequencyTable.Count; ++i)
                {
                    if (destinationFrequencyTable[i].Key != sourceFrequencyTable[i].Key)
                    {
                        tablesEqual = false;
                        break;
                    }

                    if (destinationFrequencyTable[i].Value != sourceFrequencyTable[i].Value)
                    {
                        tablesEqual = false;
                        break;
                    }
                }

            if (!tablesEqual)
                MessageBox.Show("Wrong encoding! Frequency Tables are not equal!", "Decoding",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show("Congratulations! Your decoding is perfect.", "Decoding", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox3.Text = File.ReadAllText(this.openFileDialog1.FileName, Encoding.UTF8);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox5.Text = File.ReadAllText(this.openFileDialog1.FileName);
                this.groupBox1.Enabled = false;
            }
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

        private void exitNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nextTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex < this.tabControl1.TabCount - 1)
                ++this.tabControl1.SelectedIndex;
        }

        private void previousTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex > 0)
                --this.tabControl1.SelectedIndex;
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
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
        
        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.russianToolStripMenuItem.Checked = false;
            this.ukrainianToolStripMenuItem.Checked = false;

            currentLanguage = Language.English;
            if (this.tabControl1.SelectedIndex == 1)
                UpdateDataGridView();
        }

        private void ukrainianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.englishToolStripMenuItem.CheckState = CheckState.Unchecked;
            this.russianToolStripMenuItem.CheckState = CheckState.Unchecked;

            currentLanguage = Language.Ukrainian;
            if (this.tabControl1.SelectedIndex == 1)
                UpdateDataGridView();
        }

        private void russianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.englishToolStripMenuItem.Checked = false;
            this.ukrainianToolStripMenuItem.Checked = false;

            currentLanguage = Language.Russian;
            if (this.tabControl1.SelectedIndex == 1)
                UpdateDataGridView();
        }

        private void shiftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.rjindaelToolStripMenuItem.Checked = false;
            this.shuntingToolStripMenuItem.Checked = false;

            charEncoder.CurrentEncoding = EncodingType.Cycle;
            if (this.tabControl1.SelectedIndex == 1)
                UpdateDataGridView();
        }

        private void shuntingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.rjindaelToolStripMenuItem.Checked = false;
            this.shiftToolStripMenuItem.Checked = false;

            charEncoder.CurrentEncoding = EncodingType.Shunting;
            if (this.tabControl1.SelectedIndex == 1)
                UpdateDataGridView();
        }

        private void rjindaelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.shuntingToolStripMenuItem.Checked = false;
            this.shiftToolStripMenuItem.Checked = false;

            charEncoder.CurrentEncoding = EncodingType.Rijndael;

            if (this.tabControl1.SelectedIndex == 1)
                UpdateDataGridView();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                this.textBox6.Text = File.ReadAllText(this.openFileDialog1.FileName);
        }
    }
}
