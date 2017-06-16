using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EDiary
{
    public partial class AddEntryForm : Form
    {
        private int maxDescriptionLength = 50;
        private int maxPlaceDescLength = 30;
        private DiaryEntry entry = new DiaryEntry();
        private bool save;

        public DiaryEntry DEntry
        {
            get { return entry; }
        }

        public AddEntryForm()
        {
            InitializeComponent();

            this.dateTimePicker1.MinDate = DateTime.Today;
            this.dateTimePicker2.MinDate = DateTime.Now;

            UpdateComboBox();
        }

        public AddEntryForm(DiaryEntry from)
        {
            InitializeComponent();

            this.dateTimePicker1.MinDate = DateTime.Today;
            this.dateTimePicker2.MinDate = DateTime.Now;
            entry = new DiaryEntry(from);

            // check if dates are up-to-date
            if (DateTime.Compare(from.MyEvent.Date, DateTime.Now) > 0)
            {
                this.dateTimePicker1.Value = from.MyEvent.Date;
                this.dateTimePicker1.Value = from.MyEvent.Date;
            }
            else
            {
                this.dateTimePicker1.Value = DateTime.Today;
                this.dateTimePicker2.Value = DateTime.Now;
            }

            UpdateComboBox();

            this.descriptionTextBox.Text = from.MyEvent.ShortDescription;
            this.placeDescTextBox.Text = from.MyEvent.Place;

            this.personComboBox.SelectedIndex = Array.FindIndex(EDiary.Staff.PersonList.Persons.ToArray(), t => t.ToString() == from.Person.ToString());
        }

        private void UpdateComboBox()
        {
            this.personComboBox.Items.Clear();

            // fill comboBox with persons
            foreach (PersonInfo p in EDiary.Staff.PersonList.Persons)
                this.personComboBox.Items.Add(p);

            this.personComboBox.Items.Add("Add new...");
            this.personComboBox.Items.Add("Manage...");
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (this.descriptionTextBox.Text.Length > maxDescriptionLength)
            {
                errorProvider1.SetIconAlignment((Control)sender, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError((Control)sender, "Length of short description should be less than " + maxDescriptionLength + " symbols.");
                e.Cancel = true;
            }
            else
                errorProvider1.SetError((Control)sender, "");
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (this.placeDescTextBox.Text.Length > maxPlaceDescLength)
            {
                errorProvider1.SetIconAlignment((Control)sender, ErrorIconAlignment.MiddleLeft);
                errorProvider1.SetError((Control)sender, "Length of place description should be less than " + maxPlaceDescLength + " symbols.");
                e.Cancel = true;
            }
            else
                errorProvider1.SetError((Control)sender, "");
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            save = true;
            // exit form
            this.Close();
        }

        private void personComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // paranoiac hack
            if (this.personComboBox.SelectedIndex == -1)
                return;

            // if "Add new person" item selected
            if (this.personComboBox.SelectedIndex == this.personComboBox.Items.Count - 2)
            {
                AddPersonForm addPersonForm = new AddPersonForm();
                if (addPersonForm.ShowDialog() == DialogResult.OK)
                {
                    // save new PersonInfo
                    EDiary.Staff.PersonList.Persons.Insert(0, new PersonInfo(addPersonForm.Person));
                    this.personComboBox.Items.Insert(0, Staff.PersonList.Persons[0]);
                }

                this.personComboBox.SelectedIndex = 0;

                return;
            }

            // if "Manage Person list" item selected
            if (this.personComboBox.SelectedIndex == this.personComboBox.Items.Count - 1)
            {
                ManagePersonsForm managePersons = new ManagePersonsForm();
                managePersons.ShowDialog();

                UpdateComboBox();
            }
        }

        private void discardButton_Click(object sender, EventArgs e)
        {
            save = false;
            this.Close();
        }

        private void AddEntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!save)
                return;

            if (this.descriptionTextBox.Text.Length > maxDescriptionLength)
            {
                MessageBox.Show("Event description must have less than " + maxDescriptionLength + " symbols!",
                    "Too long description", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                this.descriptionTextBox.Focus();
                return;
            }

            if (this.descriptionTextBox.Text.Length == 0)
            {
                MessageBox.Show("No event description is entered.", "Too short description", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                this.descriptionTextBox.Focus();
                return;
            }

            if (this.placeDescTextBox.Text.Length > maxPlaceDescLength)
            {
                MessageBox.Show("Place description must have less than " + maxPlaceDescLength + " symbols!",
                    "Too long description", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                this.placeDescTextBox.Focus();
                return;
            }

            if (this.placeDescTextBox.Text.Length == 0)
            {
                MessageBox.Show("No place description is entered", "Too short description", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                this.placeDescTextBox.Focus();
                return;
            }

            int index = this.personComboBox.SelectedIndex;
            int count = this.personComboBox.Items.Count;

            if ((index == -1) || (index == count - 1) || (index == count - 2))
            {
                MessageBox.Show("Please, select some person for this event.", "No person selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                this.personComboBox.Focus();
                return;
            }

            // now all is ok and we can save all data, entered by user


            // save event data first
            entry.MyEvent.Date = new DateTime(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month, this.dateTimePicker1.Value.Day,
                this.dateTimePicker2.Value.Hour, this.dateTimePicker2.Value.Minute, this.dateTimePicker2.Value.Second);

            entry.MyEvent.Duration = (int)this.numericUpDown1.Value;
            entry.MyEvent.Place = this.placeDescTextBox.Text;
            entry.MyEvent.ShortDescription = this.descriptionTextBox.Text;

            // save reference to selected person
            entry.Person = Staff.PersonList.Persons[this.personComboBox.SelectedIndex];
        }
    }
}
