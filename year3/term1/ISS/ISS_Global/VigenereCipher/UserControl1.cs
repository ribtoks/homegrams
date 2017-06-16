using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;

using LetterFrequency;

namespace VigenereCipher
{
    public partial class VigenereCipherControl : UserControl, ILanguageChangable
    {
        Language currentLanguage = Language.English;
        char[][] currentVigTable;

        public VigenereCipherControl()
        {
            InitializeComponent();
        }

        public void ChangeLanguage(Language newLanguage)
        {
            currentLanguage = newLanguage;
            UpdateDataGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string key = GetKeyWithNeededLength(this.textBox3.Text, this.textBox1.Text.Length);

            StringBuilder sb = new StringBuilder(this.textBox1.Text.Length);
            sb.Append(this.textBox1.Text.ToLower());

            for (int i = 0; i < this.textBox1.Text.Length; ++i)
            {
                if (char.IsLetter(this.textBox1.Text[i]))
                {
                    int firstIndex = 0;
                    int secondIndex = 0;

                    try
                    {
                        firstIndex = Languages.Chars[currentLanguage][key[i]];
                        secondIndex = Languages.Chars[currentLanguage][char.ToLower(this.textBox1.Text[i])];
                    }
                    catch
                    {
                        MessageBox.Show("Wrong language! Please, check current language.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    sb[i] = currentVigTable[firstIndex][secondIndex];
                }
            }

            this.textBox2.Text = sb.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string key = GetKeyWithNeededLength(this.textBox3.Text, this.textBox2.Text.Length);

            StringBuilder sb = new StringBuilder(this.textBox2.Text.Length);
            sb.Append(this.textBox2.Text.ToLower());

            int languageLength = Languages.Strings[currentLanguage].Length;

            for (int i = 0; i < this.textBox2.Text.Length; ++i)
            {
                if (char.IsLetter(this.textBox2.Text[i]))
                {
                    int firstIndex = 0;
                    int secondIndex = 0;
                    try
                    {
                        firstIndex = Languages.Chars[currentLanguage][key[i]];
                        secondIndex = Languages.Chars[currentLanguage][char.ToLower(this.textBox2.Text[i])];
                    }
                    catch
                    {
                        MessageBox.Show("Wrong language! Please, check current language.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }

                    int index = secondIndex - firstIndex;
                    if (index < 0)
                        index += languageLength;

                    sb[i] = Languages.Strings[currentLanguage][index];
                }
            }

            this.textBox1.Text = sb.ToString();
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (this.textBox3.Text == "Key")
            {
                this.textBox3.Text = "";
                this.textBox3.ForeColor = Color.Black;
                this.textBox3.Font = new Font(this.textBox3.Font, FontStyle.Regular);
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (this.textBox3.Text.Length == 0)
            {
                this.textBox3.Text = "Key";
                this.textBox3.ForeColor = Color.DarkGray;
                this.textBox3.Font = new Font(this.textBox3.Font, FontStyle.Bold);
            }
        }

        private void UpdateDataGridView()
        {
            char[][] vigenereTable = GetVigenereTable(currentLanguage);

            DataTable table = new DataTable();

            for (int i = 0; i < vigenereTable.Length; ++i)
                table.Columns.Add(Languages.Strings[currentLanguage][i].ToString());

            for (int i = 0; i < vigenereTable.Length; ++i)
                table.LoadDataRow(
                    Array.ConvertAll<char, object>(vigenereTable[i],
                    delegate(char c)
                    {
                        return (object)c;
                    }), true);

            this.dataGridView1.RowHeadersWidth = 30;
            this.dataGridView1.DataSource = table;

            for (int i = 0; i < this.dataGridView1.Columns.Count; ++i)
            {
                this.dataGridView1.Columns[i].Width = 20;
                this.dataGridView1.Rows[i].HeaderCell.Value = (object)Languages.Strings[currentLanguage][i].ToString();
            }

            currentVigTable = vigenereTable;
        }

        private string GetKeyWithNeededLength(string key, int length)
        {
            if (key.Length > length)
                key.Remove(length);

            int count = (int)(length / key.Length);

            StringBuilder sb = new StringBuilder(key.Length * (count + 1));
            for (int i = 0; i <= count; ++i)
                sb.Append(key);

            return sb.ToString().Substring(0, length);
        }

        public static char[][] GetVigenereTable(Language language)
        {
            char[][] vigTable = new char[Languages.Strings[language].Length][];

            int languageLength = Languages.Strings[language].Length;

            for (int i = 0; i < Languages.Strings[language].Length; ++i)
                vigTable[i] = Array.ConvertAll<char, char>(Languages.Strings[language].ToCharArray(), delegate(char c)
                {
                    return Languages.Strings[language][(Languages.Chars[language][c] + i) % languageLength];
                });

            return vigTable;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                this.textBox1.Text = File.ReadAllText(this.openFileDialog1.FileName);
        }

        public override string ToString()
        {
            return "Vigenere cipher";
        }
    }
}
