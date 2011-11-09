using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EDiary
{
    public partial class AddStringForm : Form
    {
        private string line = "";
        private bool save;

        public string Line
        {
            get { return line; }
        }

        public AddStringForm(string headerHext)
        {
            InitializeComponent();
            this.Text = "Enter " + headerHext + ":";
        }

        public AddStringForm(string headerHext, string editString)
        {
            InitializeComponent();
            this.Text = "Enter " + headerHext + ":";
            this.line = editString;
            this.stringBox.Text = editString;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            save = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            save = false;
            this.Close();
        }

        private void AddStringForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!save)
                return;

            if (this.stringBox.Text.Length == 0)
            {
                MessageBox.Show("Please, enter some text!", "No text entered",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                this.stringBox.Focus();
                return;
            }
            this.line = this.stringBox.Text;
        }
    }
}
