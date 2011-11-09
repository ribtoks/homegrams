using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EDiary
{
    public partial class ManagePersonsForm : Form
    {
        public ManagePersonsForm()
        {
            InitializeComponent();

            this.listBox1.DataSource = Staff.PersonList.Persons;
        }

        private void addPersonButton_Click(object sender, EventArgs e)
        {
            AddPersonForm addPersonForm = new AddPersonForm();
            if (addPersonForm.ShowDialog() == DialogResult.OK)
            {
                Staff.PersonList.Persons.Add(new PersonInfo(addPersonForm.Person));

                // update DataSource
                this.listBox1.DataSource = null;
                this.listBox1.DataSource = Staff.PersonList.Persons;
            }
        }

        private void editPersonButton_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Items.Count == 0)
            {
                MessageBox.Show("Please, create some persons before editing.", "No item to edit",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please, select some item first.", "No item is selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int index = this.listBox1.SelectedIndex;

            AddPersonForm editPerson = new AddPersonForm(Staff.PersonList.Persons[index]);
            
            if (editPerson.ShowDialog() == DialogResult.OK)
            {
                Staff.PersonList.Persons[index] = new PersonInfo(editPerson.Person);

                this.listBox1.DataSource = null;
                this.listBox1.DataSource = Staff.PersonList.Persons;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.Items.Count == 0)
            {
                MessageBox.Show("Please, create some persons before deleting them.", "No item to delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // move selection after deletion
            if (this.listBox1.SelectedIndex != -1)
            {
                int index = this.listBox1.SelectedIndex;
                Staff.PersonList.Persons.RemoveAt(index);

                // update DataSource
                this.listBox1.DataSource = null;
                this.listBox1.DataSource = Staff.PersonList.Persons;

                // move the selection
                if (this.listBox1.Items.Count > 0)
                {
                    if (index < this.listBox1.Items.Count)
                        this.listBox1.SelectedIndex = index;
                    else
                        this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
                }
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
