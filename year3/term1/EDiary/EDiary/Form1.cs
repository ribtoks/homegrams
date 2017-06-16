using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace EDiary
{
    public partial class Form1 : Form
    {
        // path to folder, where application 
        // is situated in
        string currentFolderPath;

        // path to binary file with all data inside
        string todosFilePath;
        bool loadedFound = false;
        List<DiaryEntry> loadedItems;
        List<int> itemsToHighlight = new List<int>();

        public Form1()
        {
            InitializeComponent();
            //this.rectangleShape1.FillColor = Color.FromArgb(0x99, 0xcc, 0xff);
            this.panel1.BackColor = Color.FromArgb(20, Color.LightBlue);

            LoadOptions();
            LoadToDoList();
            MyDiaryList.UpdateItems();
            ReloadItems();
        }

        /// <summary>
        /// Loads diary entries from their storage
        /// </summary>
        private void LoadToDoList()
        {
            currentFolderPath = Application.StartupPath;
            string todosDirPath = currentFolderPath + Path.DirectorySeparatorChar + "Events" + Path.DirectorySeparatorChar;

            if (!Directory.Exists(todosDirPath))
                Directory.CreateDirectory(todosDirPath);

            todosFilePath = todosDirPath + global::EDiary.Properties.Settings.Default.EventsFileName + 
                global::EDiary.Properties.Settings.Default.EventsFileExtension;

            if (!File.Exists(todosFilePath))
                MyDiaryList.SaveEmptyList(todosFilePath);

            MyDiaryList.LoadDiary(todosFilePath);
        }

        /// <summary>
        /// Reloads options from windows user configuratin files
        /// </summary>
        private void LoadOptions ()
        {
        	try
			{
        		global::EDiary.Properties.Settings.Default.Reload ();
        		MyDiaryList.DaysToAdd = global::EDiary.Properties.Settings.Default.DaysToAdd;
        		MyDiaryList.RemoveYesterdayBusiness = global::EDiary.Properties.Settings.Default.RemoveOldBusiness;
        	}
			catch
			{
			}
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // a little hack with RectangleShape Control
            //this.dollarDrawer1.BringToFront();
            //this.Update();
        }

        // close button
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // minimize button
        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //show options button
        private void button4_Click(object sender, EventArgs e)
        {
            OptionsForm options = new OptionsForm();
            options.ShowDialog();

            MyDiaryList.UpdateItems();
            ReloadItems();
        }

        private void addEntryButton_Click(object sender, EventArgs e)
        {
            if (loadedFound)
            {
                if (MessageBox.Show("Return to events list?", "EDiary Question",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                ReloadItems();
            }

            AddEntryForm addEntryForm = new AddEntryForm();
            if (addEntryForm.ShowDialog() == DialogResult.OK)
            {
                MyDiaryList.AddEntry(addEntryForm.DEntry);
                ReloadItems();
            }
        }

        private void deleteEntryButton_Click(object sender, EventArgs e)
        {
            if (this.diaryEntries.Items.Count == 0)
            {
                MessageBox.Show("Please, add some entries before deleting them.", "No item to delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.diaryEntries.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Please, select some entry first.", "No item to delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Delete this item?", "Delete question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int index = this.diaryEntries.SelectedIndices[0];
            DiaryEntryItem tempEntryItem = (DiaryEntryItem)this.diaryEntries.Items[index];

            MyDiaryList.DeleteEntry(tempEntryItem.Entry.MyEvent.Date);

            if (!loadedFound)
                ReloadItems();
            else
            {
                loadedItems.RemoveAll(item => (DateTime.Compare(item.MyEvent.Date, tempEntryItem.Entry.MyEvent.Date) == 0));
                // and update them in listView
                this.diaryEntries.Items.Clear();
                foreach (DiaryEntry de in loadedItems)
                    this.diaryEntries.Items.Add(new DiaryEntryItem(de));
            }
        }

        private void editEntryButton_Click(object sender, EventArgs e)
        {
            if (this.diaryEntries.Items.Count == 0)
            {
                MessageBox.Show("Please, add some entries before editing them.", "No item to edit",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.diaryEntries.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Please, select some entry first.", "No item to edit",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } 
            
            int index = this.diaryEntries.SelectedIndices[0];
            DiaryEntryItem tempEntryItem = (DiaryEntryItem)this.diaryEntries.Items[index];

            AddEntryForm editEntry = new AddEntryForm(tempEntryItem.Entry);

            // if item was edited, than we should refresh all items
            if (editEntry.ShowDialog() == DialogResult.OK)
            {
                MyDiaryList.RefreshValue(tempEntryItem.Entry.MyEvent.Date, editEntry.DEntry);

                if (DateTime.Compare(tempEntryItem.Entry.MyEvent.Date, editEntry.DEntry.MyEvent.Date) == 0)
                {
                    this.diaryEntries.Items[index] = new DiaryEntryItem(editEntry.DEntry);
                    return;
                }

                if (!loadedFound)
                    ReloadItems();
                else
                {
                    index = loadedItems.FindIndex(li => (DateTime.Compare(li.MyEvent.Date, tempEntryItem.Entry.MyEvent.Date) == 0));
                    loadedItems[index] = new DiaryEntry(editEntry.DEntry);

                    // and update them in listView
                    this.diaryEntries.Items.Clear();
                    foreach (DiaryEntry de in loadedItems)
                        this.diaryEntries.Items.Add(new DiaryEntryItem(de));
                }
            }
        }

        /// <summary>
        /// Updates states of all items in listView
        /// </summary>
        private void UpdateEntriesState()
        {
            itemsToHighlight.Clear();
            for (int i = 0; i < this.diaryEntries.Items.Count; ++i)
            {
                DiaryEntryItem tempItem = (DiaryEntryItem)this.diaryEntries.Items[i];

                TimeSpan ts = tempItem.Entry.MyEvent.Date - DateTime.Now;                
                if (Math.Abs(ts.Days) <= global::EDiary.Properties.Settings.Default.DaysBeforeNotify)
                {
                    tempItem.ImageIndex = 0;
                    itemsToHighlight.Add(i);
                }

                if (MyDiaryList.IsEventBad(tempItem.Entry.MyEvent.Date))
                {
                    tempItem.BackColor = Color.Ivory;
                    tempItem.ImageIndex = 1;

                    itemsToHighlight.Remove(i);
                }
            }
        }

        private void ReloadItems()
        {
            this.diaryEntries.Items.Clear();

            this.loadedItems = MyDiaryList.GetFirstN(global::EDiary.Properties.Settings.Default.FirstN);

            // update here first N events
            foreach (DiaryEntry dE in loadedItems)
                this.diaryEntries.Items.Add(new DiaryEntryItem(dE));

            UpdateEntriesState();

            loadedFound = false;
            this.timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReloadItems();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AboutEDiaryForm about = new AboutEDiaryForm();
            about.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MyDiaryList.SaveDiary(todosFilePath);
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            FindEventsForm findForm = new FindEventsForm(loadedItems);
            findForm.ShowDialog();

            if (findForm.Canceled)
                return;

            loadedItems = findForm.FoundEntries;

            this.diaryEntries.Items.Clear();
            foreach (DiaryEntry de in loadedItems)
                this.diaryEntries.Items.Add(new DiaryEntryItem(de));

            loadedFound = true;
            this.timer1.Enabled = false;
        }

        public void RoundRectangle(Graphics gc, float left, float top, float width, float height, float radius)
        {
            GraphicsPath rectPath = new GraphicsPath();

            rectPath.AddLine(left + radius, top, left + width - (radius * 2), top);
            rectPath.AddArc(left + width - (radius * 2), top, radius * 2, radius * 2, 270, 90);
            rectPath.AddLine(left + width, top + radius, left + width, top + height - (radius * 2));
            rectPath.AddArc(left + width - (radius * 2), top + height - (radius * 2), radius * 2, radius * 2, 0, 90); // Corner
            rectPath.AddLine(left + width - (radius * 2), top + height, left + radius, top + height);
            rectPath.AddArc(left, top + height - (radius * 2), radius * 2, radius * 2, 90, 90);
            rectPath.AddLine(left, top + height - (radius * 2), left, top + radius);
            rectPath.AddArc(left, top, radius * 2, radius * 2, 180, 90);
            rectPath.CloseFigure();

            gc.TextRenderingHint = TextRenderingHint.AntiAlias;
            gc.InterpolationMode = InterpolationMode.HighQualityBilinear;
            gc.PixelOffsetMode = PixelOffsetMode.HighQuality;
            gc.SmoothingMode = SmoothingMode.HighQuality;

            // generate linear gradient brush
            Brush brush = new LinearGradientBrush(new Point((int)width / 2, 0), new Point((int)width / 2, (int)(height + top)),
                Color.FromArgb(0x99, 0xcc, 0xff), Color.RoyalBlue);

            gc.FillPath(brush, rectPath);

            Pen pen = new Pen(Brushes.MediumTurquoise, 1.5f);

            gc.DrawPath(pen, rectPath);

            rectPath.Dispose();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics gc = e.Graphics;
            //draw round rectangle
            RoundRectangle(gc, 10, 10, this.panel2.Width - 20, this.panel2.Height - 20, 20);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.panel2.Refresh();
            this.dollarDrawer1.BringToFront();
            this.dollarDrawer1.Refresh();
        }

        private void editItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.editEntryButton.PerformClick();
        }

        private void deleteItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.deleteEntryButton.PerformClick();
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.addEntryButton.PerformClick();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadItems();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (int i in itemsToHighlight)
            {
                if (this.diaryEntries.Items[i].BackColor == Color.White)
                    this.diaryEntries.Items[i].BackColor = Color.WhiteSmoke;
                else
                    this.diaryEntries.Items[i].BackColor = Color.White;
            }
            this.diaryEntries.Refresh();
        }
    }
}
