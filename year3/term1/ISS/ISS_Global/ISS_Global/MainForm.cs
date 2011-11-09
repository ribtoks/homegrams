using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using LetterFrequency;

namespace ISS_Global
{
    public partial class MainForm : Form
    {
        #region Private data

        List<UserControl> controls;
        Dictionary<string, UserControl> dictControls;
        UserControl currentControl;

        private readonly int defaultWidth;
        private readonly int defaultHeight;

        private readonly int defaultPanelWidth;
        private readonly int defaultPanelHeight;

        private bool loadedSmth = false;

        Language currentLanguage = Language.English;

        #endregion

        public MainForm()
        {
            InitializeComponent();
            controls = new List<UserControl>();
            dictControls = new Dictionary<string, UserControl>();

            defaultHeight = this.Height;
            defaultWidth = this.Width;

            defaultPanelHeight = this.panel1.Height;
            defaultPanelWidth = this.panel1.Width;
        }

        private void updateListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.allFoundToolStripMenuItem.Enabled = true;

            try
            {
                UpdateModulesList();
                UpdateMenuModuleItems();
            }
            catch
            {
                MessageBox.Show("Problem occured while updating modules list.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                MessageBox.Show(controls.Count + " modules were found. Saved in \"Modules -> All found ->\"", "Info", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateModulesList()
        {
            controls.Clear();

            // get './Modules/' directory
            DirectoryInfo dir = new DirectoryInfo("." + Path.DirectorySeparatorChar + "Modules" + Path.DirectorySeparatorChar);

            foreach (FileInfo fi in dir.GetFiles("*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(fi.FullName);
                Type[] types = assembly.GetTypes();

                foreach(Type t in types)
                {
                    if (t.BaseType == typeof(UserControl))
                    {
                        controls.Add((UserControl)Activator.CreateInstance(t));
                        dictControls.Add(controls[controls.Count - 1].ToString(), controls[controls.Count - 1]);
                    }
                }
            }
        }

        /// <summary>
        /// Updates menu items due to new list of user controls
        /// </summary>
        private void UpdateMenuModuleItems()
        {
            this.allFoundToolStripMenuItem.DropDownItems.Clear();

            for (int i = 0; i < controls.Count; ++i)
                this.allFoundToolStripMenuItem.DropDownItems.Add(controls[i].ToString(), null, delegate(object sender, EventArgs e)
                {
                    PlaceModuleOnPanel(dictControls[sender.ToString()]);
                });
        }

        /// <summary>
        /// Loads selected user control to panel
        /// </summary>
        /// <param name="uc"></param>
        private void PlaceModuleOnPanel(UserControl uc)
        {
            this.Text = "Information Systems Security - " + uc.ToString();

            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(uc);

            Control panelControl = this.panel1.Controls[0];

            panelControl.Left = 10;
            panelControl.Top = 10;

            this.panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            this.loadedSmth = true;
            this.unloadCurrentToolStripMenuItem.Enabled = true;

            currentControl = uc;
            UpdateLanguageInControl();
        }

        private void UpdateLanguageInControl()
        {
            if (loadedSmth)
            {
                if (currentControl is ILanguageChangable)
                    (currentControl as ILanguageChangable).ChangeLanguage(currentLanguage);
            }
        }

        private void unloadCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentControl = null;
            loadedSmth = false;
            this.panel1.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.AutoSizeMode = AutoSizeMode.GrowOnly;
            this.panel1.Controls.Clear();
            this.Size = new Size(defaultWidth, defaultHeight);
            this.panel1.Size = new Size(defaultPanelWidth, defaultPanelHeight);
            this.unloadCurrentToolStripMenuItem.Enabled = false;
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentLanguage = Language.English;

            this.ukrainianToolStripMenuItem.Checked = false;
            this.russianToolStripMenuItem.Checked = false;

            UpdateLanguageInControl();
        }

        private void ukrainianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentLanguage = Language.Ukrainian;

            this.englishToolStripMenuItem.Checked = false;
            this.russianToolStripMenuItem.Checked = false;

            UpdateLanguageInControl();
        }

        private void russianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentLanguage = Language.Russian;

            this.ukrainianToolStripMenuItem.Checked = false;
            this.englishToolStripMenuItem.Checked = false;

            UpdateLanguageInControl();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }
    }
}
