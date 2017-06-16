using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EDiary
{
    public partial class ManageStringListForm : Form
    {
        private string listType;
        private List<string> lines = new List<string>();

        public List<string> Lines
        {
            get { return lines; }
            set { lines = value; }
        }

        public ManageStringListForm(string listName)
        {
            InitializeComponent();
            this.listType = listName;
            this.Text = "Manage " + listType + " list";
        }

        public ManageStringListForm(string listName, List<string> initialLines)
        {
            InitializeComponent();
            this.listType = listName;
            this.Text = "Manage " + listType + " list";
            lines = new List<string>(initialLines);

            foreach (string line in initialLines)
                this.stringsList.Items.Add(line);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddStringForm addStringForm = new AddStringForm(listType);

            // if user entered a line, add it to List<> and to listBox
            if (addStringForm.ShowDialog() == DialogResult.OK)
            {
                lines.Add(addStringForm.Line);
                this.stringsList.Items.Add(addStringForm.Line);
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (this.stringsList.Items.Count == 0)
            {
                MessageBox.Show("Please, enter some lines before editing.", "No item to edit",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.stringsList.SelectedIndex == -1)
            {
                MessageBox.Show("Please, select some item first.", "No item is selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int index = this.stringsList.SelectedIndex;

            AddStringForm addStringForm = new AddStringForm(listType, lines[index]);

            // if user changed line, save it in list
            if (addStringForm.ShowDialog() == DialogResult.OK)
            {
                lines[index] = addStringForm.Line;
                this.stringsList.Items[index] = addStringForm.Line;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (this.stringsList.Items.Count == 0)
            {
                MessageBox.Show("Please, enter some lines before deleting them.", "No item to delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // move selection after deletion
            if (this.stringsList.SelectedIndex != -1)
            {
                int index = this.stringsList.SelectedIndex;

                this.lines.RemoveAt(index);
                this.stringsList.Items.RemoveAt(index);

                if (this.stringsList.Items.Count > 0)
                {
                    if (index < this.stringsList.Items.Count)
                        this.stringsList.SelectedIndex = index;
                    else
                        this.stringsList.SelectedIndex = this.stringsList.Items.Count - 1;
                }
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
