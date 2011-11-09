using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EDiary
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();

            // reload all program settings
            global::EDiary.Properties.Settings.Default.Reload();

            // and save it values to form
            this.firstNUpDown.Value = global::EDiary.Properties.Settings.Default.FirstN;
            this.radioButton1.Checked = global::EDiary.Properties.Settings.Default.RemoveOldBusiness;
            this.radioButton2.Checked = !this.radioButton1.Checked;
            this.daysIncUpDown.Value = (int)global::EDiary.Properties.Settings.Default.DaysToAdd;
            this.daysBeforeNotifyUpDown.Value = global::EDiary.Properties.Settings.Default.DaysBeforeNotify;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.panel2.Enabled = this.radioButton2.Checked;
        }

        private void discardButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            global::EDiary.Properties.Settings.Default.FirstN = (int)this.firstNUpDown.Value;
            global::EDiary.Properties.Settings.Default.RemoveOldBusiness = this.radioButton1.Checked;
            global::EDiary.Properties.Settings.Default.DaysToAdd = (double)this.daysIncUpDown.Value;
            global::EDiary.Properties.Settings.Default.DaysBeforeNotify = (int)this.daysBeforeNotifyUpDown.Value;

            MyDiaryList.DaysToAdd = global::EDiary.Properties.Settings.Default.DaysToAdd;
            MyDiaryList.RemoveYesterdayBusiness = global::EDiary.Properties.Settings.Default.RemoveOldBusiness;

            // save all changes now
            global::EDiary.Properties.Settings.Default.Save();

            this.Close();
        }
    }
}
