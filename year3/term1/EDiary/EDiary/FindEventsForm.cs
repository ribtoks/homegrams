using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EDiary
{
    public partial class FindEventsForm : Form
    {
        private List<DiaryEntry> entriesFound;
        private List<DiaryEntry> lookIn;
        private bool canceled = false;

        public List<DiaryEntry> FoundEntries
        {
            get { return entriesFound; }
        }

        public bool Canceled
        {
            get { return canceled; }
        }

        private FindEventsForm()
        {
            InitializeComponent();

            string[] names = Enum.GetNames(typeof(Appointment));

            foreach (string name in names)
                this.appComboBox.Items.Add(name);

            this.appComboBox.SelectedIndex = 0;
            this.personComboBox.DataSource = Staff.PersonList.Persons;
            this.dateTimePicker1.MinDate = DateTime.Today;

            entriesFound = new List<DiaryEntry>();
        }

        public FindEventsForm(List<DiaryEntry> loadedItems)
            :this()
        {
            lookIn = loadedItems;
        }

        private void findByDateRB_CheckedChanged(object sender, EventArgs e)
        {
            this.panel2.Enabled = this.findByDateRB.Checked;

            this.textBox1.Enabled = !this.findByDateRB.Checked;
            this.appComboBox.Enabled = !this.findByDateRB.Checked;
            this.personComboBox.Enabled = !this.findByDateRB.Checked;
        }

        private void findByTextRB_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox1.Enabled = this.findByTextRB.Checked;

            this.panel2.Enabled = !this.findByTextRB.Checked;
            this.appComboBox.Enabled = !this.findByTextRB.Checked;
            this.personComboBox.Enabled = !this.findByTextRB.Checked;
        }

        private void findByAppRB_CheckedChanged(object sender, EventArgs e)
        {
            this.appComboBox.Enabled = this.findByAppRB.Checked;

            this.textBox1.Enabled = !this.findByAppRB.Checked;
            this.panel2.Enabled = !this.findByAppRB.Checked;
            this.personComboBox.Enabled = !this.findByAppRB.Checked;
        }

        private void findByPersonRB_CheckedChanged(object sender, EventArgs e)
        {
            this.personComboBox.Enabled = this.findByPersonRB.Checked;

            this.textBox1.Enabled = !this.findByPersonRB.Checked;
            this.panel2.Enabled = !this.findByPersonRB.Checked;
            this.appComboBox.Enabled = !this.findByPersonRB.Checked;
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            // now find whick search tool was selected
            // and run search using MyDiaryList class


            if (this.findByTextRB.Checked)
            {
                if (this.textBox1.Text.Length == 0)
                {
                    MessageBox.Show("No text is entered!", "Search error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.textBox1.Focus();
                    return;
                }

                // search this text though all fields of all events
                if (this.searchAllRB.Checked)
                    entriesFound = MyDiaryList.Search(this.textBox1.Text);
                else
                    entriesFound = MyDiaryList.Search(this.textBox1.Text, lookIn);

                this.Close();
                return;
            }

            if (this.findByDateRB.Checked)
            {
                int sign = 0;

                if (this.lessThanRB.Checked)
                    sign = -1;

                if (this.greaterThanRB.Checked)
                    sign = 1;

                // search this text though all fields of all events
                if (this.searchAllRB.Checked)
                    entriesFound = MyDiaryList.SearchDate(this.dateTimePicker1.Value, sign);
                else
                    entriesFound = MyDiaryList.SearchDate(this.dateTimePicker1.Value, sign, lookIn);

                this.Close();
                return;
            }

            if (this.findByPersonRB.Checked)
            {
                if (this.personComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("No person is selected!", "Search error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.personComboBox.Focus();
                    return;
                }

                // search this text though all fields of all events
                if (this.searchAllRB.Checked)
                    entriesFound = MyDiaryList.SearchPerson(Staff.PersonList.Persons[this.personComboBox.SelectedIndex]);
                else
                    entriesFound = MyDiaryList.SearchPerson(Staff.PersonList.Persons[this.personComboBox.SelectedIndex], lookIn);

                this.Close();
                return;
            }

            if (this.findByAppRB.Checked)
            {
                if (this.appComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("No appointment is selected!", "Search error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.appComboBox.Focus();
                    return;
                }

                // search this text though all fields of all events
                if (this.searchAllRB.Checked)
                    entriesFound = MyDiaryList.SearchAppointment((Appointment)this.appComboBox.SelectedIndex);
                else
                    entriesFound = MyDiaryList.SearchAppointment((Appointment)this.appComboBox.SelectedIndex, lookIn);

                this.Close();
                return;
            }

            // ??
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            canceled = true;
            this.Close();
        }
    }
}
