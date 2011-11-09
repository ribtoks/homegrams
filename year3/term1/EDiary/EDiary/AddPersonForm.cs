using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EDiary
{
    public partial class AddPersonForm : Form
    {
        private PersonInfo person = new PersonInfo();
        private List<string> emailsList = new List<string>();
        private List<string> phoneNumbersList = new List<string>();
        private bool save;

        public PersonInfo Person
        {
            get { return person; }
        }

        public AddPersonForm()
        {
            InitializeComponent();

            string[] names = Enum.GetNames(typeof(Appointment));

            foreach (string name in names)
                this.comboBox1.Items.Add(name);

            this.comboBox1.SelectedIndex = 0;
        }

        public AddPersonForm(PersonInfo from)
        {
            InitializeComponent();

            person = new PersonInfo(from);

            string[] names = Enum.GetNames(typeof(Appointment));
            int index = Array.FindIndex(names, t => t == from.Appointment.ToString());

            foreach (string name in names)
                this.comboBox1.Items.Add(name);

            this.comboBox1.SelectedIndex = index;

            this.nameTextBox.Text = from.Name;
            this.surnameTextBox.Text = from.Surname;

            emailsList = new List<string>(from.Emails);
            phoneNumbersList = new List<string>(from.PhoneNumbers);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            save = false;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            save = true;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ManageStringListForm manageEmails = new ManageStringListForm("e-mail", emailsList);
            manageEmails.ShowDialog();

            // save changes if so
            emailsList = new List<string>(manageEmails.Lines);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ManageStringListForm managePhones = new ManageStringListForm("phone number", phoneNumbersList);
            managePhones.ShowDialog();

            // save changes if so
            phoneNumbersList = new List<string>(managePhones.Lines);
        }

        private void AddPersonForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!save)
                return;

            if (this.nameTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please, enter some name.", "Error: no name entered",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                this.nameTextBox.Focus();
                return;
            }

            if (this.surnameTextBox.Text.Length == 0)
            {
                MessageBox.Show("Please, enter persons' surname.", "Error: no surname entered",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                this.surnameTextBox.Focus();
                return;
            }

            if (this.comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please, enter some appointment.", "Error: no appointment selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                this.comboBox1.Focus();
                return;
            }

            // create new PersonInfo from our data
            person = new PersonInfo(this.nameTextBox.Text, this.surnameTextBox.Text,
                (Appointment)this.comboBox1.SelectedIndex, phoneNumbersList, emailsList);
        }
    }
}
