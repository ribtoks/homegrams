using System;
using System.Collections.Generic;
using System.Text;

namespace EDiary
{
    public class DiaryEntryItem : System.Windows.Forms.ListViewItem
    {
        private DiaryEntry innerEntry;

        public DiaryEntryItem(DiaryEntry from)
        {
            innerEntry = new DiaryEntry(from);

            base.Text = innerEntry.MyEvent.ShortDescription;
            base.SubItems.Add(innerEntry.MyEvent.Date.ToShortDateString() + " - "
                + innerEntry.MyEvent.Date.ToShortTimeString() + " - " + innerEntry.MyEvent.Duration.ToString() + " hour(s)");

            base.SubItems.Add(innerEntry.MyEvent.Place);
            base.SubItems.Add(innerEntry.Person.ToString());
        }

        public DiaryEntry Entry
        {
            get { return innerEntry; }
        }
    }
}
