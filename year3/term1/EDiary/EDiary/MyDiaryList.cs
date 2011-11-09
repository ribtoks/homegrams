using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace EDiary
{
    public static class MyDiaryList
    {
        #region Static data

        private static SortedList<DateTime, DiaryEntry> toDoList;

        // indicates if to-do list is already loaded
        private static bool loadedList;

        // indicates, if program should deleted old to-do items
        public static bool RemoveYesterdayBusiness;

        // if program shouldn't delete old items
        // incrementValue saves DateTime constant
        // which should be added to old business
        public static double DaysToAdd;

        #endregion

        static MyDiaryList()
        {
            toDoList = new SortedList<DateTime, DiaryEntry>();
            loadedList = false;
        }

        #region Load-Save region

        public static void SaveEmptyList(string fullFilePath)
        {
            try
            {
                using (Stream stream = File.Open(fullFilePath, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, new SortedList<DateTime, DiaryEntry>());
                }
            }
            catch (IOException)
            {
            }
        }

        public static void LoadDiary(string fullFilePath)
        {
            try
            {
                using (Stream stream = File.Open(fullFilePath, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    toDoList = (SortedList<DateTime, DiaryEntry>)bin.Deserialize(stream);
                }

                loadedList = true;

                // and reread all persons in list
                Staff.PersonList.Persons.Clear();
                foreach (KeyValuePair<DateTime, DiaryEntry> entry in toDoList)
                {
                    if (!Staff.PersonList.Persons.Exists(p => p.ToString() == entry.Value.Person.ToString()))
                        Staff.PersonList.Persons.Add(entry.Value.Person);
                }
            }
            catch (Exception)
            {
                if (MessageBox.Show("Cannot load Events Log File. It can be corrupted." +
                    Environment.NewLine + "Create new file?", "Critical error",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                    Application.Exit();

                SaveEmptyList(fullFilePath);
                LoadDiary(fullFilePath);
            }
        }

        public static void SaveDiary(string fullFilePath)
        {
            // if any list wasn't loaded than we have no to save it
            if (loadedList == false)
                return;

            try
            {
                using (Stream stream = File.Open(fullFilePath, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, toDoList);
                }
            }
            catch (IOException)
            {
            }
        }

        #endregion

        #region Actions region

        /// <summary>
        /// Adds entry to diary
        /// </summary>
        /// <param name="entryToAdd">Object of DiaryEntry</param>
        public static void AddEntry(DiaryEntry entryToAdd)
        {
            toDoList.Add(entryToAdd.MyEvent.Date, entryToAdd);
        }

        /// <summary>
        /// Checks if specified date already
        /// exits in diary list
        /// </summary>
        /// <param name="date">Date to check</param>
        /// <returns>True, if date exits, otherwise false</returns>
        public static bool DateExitst(DateTime date)
        {
            return toDoList.ContainsKey(date);
        }

        /// <summary>
        /// Deletes specified entry from diary list
        /// </summary>
        /// <param name="eventTime">Date of event, that should be removed</param>
        /// <returns>True, if removal succeded, otherwise false</returns>
        public static bool DeleteEntry(DateTime eventTime)
        {
            return toDoList.Remove(eventTime);
        }

        /// <summary>
        /// Refresh some value in list
        /// </summary>
        /// <param name="oldDate">Date of old Value</param>
        /// <param name="entryToRefresh">Entry, that should be refreshed</param>
        public static void RefreshValue(DateTime oldDate, DiaryEntry entryToRefresh)
        {
            if (DateTime.Compare(oldDate, entryToRefresh.MyEvent.Date) == 0)
                toDoList[oldDate] = new DiaryEntry(entryToRefresh);
            else
            {
                toDoList.Remove(oldDate);
                toDoList.Add(entryToRefresh.MyEvent.Date, entryToRefresh);
            }
        }

        /// <summary>
        /// Gets first N events from diary list
        /// </summary>
        /// <param name="N">Number of events to take</param>
        /// <returns></returns>
        public static List<DiaryEntry> GetFirstN(int N)
        {
            if (!loadedList)
                throw new InvalidOperationException("Attempt to view not loaded list.");

            if (N <= 0)
                throw new ArgumentException("Attempt to take not positive number of elements.");

            // reserve space for N entries
            List<DiaryEntry> firstN = new List<DiaryEntry>(N);

            int i = 0;

            // loop through sorted by date values
            foreach (KeyValuePair<DateTime, DiaryEntry> entry in toDoList)
            {
                firstN.Add(entry.Value);
                ++i;

                if (i >= N)
                    break;
            }

            return firstN;
        }

        public static bool IsEventBad(DateTime eventTime)
        {
            if (!loadedList)
                throw new InvalidOperationException("Cannot process events, that are not loaded!");

            if (!toDoList.ContainsKey(eventTime))
                throw new InvalidOperationException("Cannot find date specified...");

            foreach (KeyValuePair<DateTime, DiaryEntry> entry in toDoList)
            {
                // skip different persons
                if (entry.Value.Person.ToString() != toDoList[eventTime].Person.ToString())
                    continue;

                // skip my event time
                if (DateTime.Compare(entry.Value.MyEvent.Date, eventTime) == 0)
                    continue;

                // if some event belongs lies in my event bounds
                if (DateTime.Compare(entry.Value.MyEvent.Date, eventTime) > 0 &&
                    DateTime.Compare(entry.Value.MyEvent.Date, toDoList[eventTime].MyEvent.Date.AddHours((double)toDoList[eventTime].MyEvent.Duration)) <= 0)
                    return true;

                DateTime incrCurrTime = entry.Value.MyEvent.Date.AddHours((double)entry.Value.MyEvent.Duration);

                // if my event bounds lies in some event bounds
                if (DateTime.Compare(incrCurrTime, eventTime) >= 0 &&
                    DateTime.Compare(incrCurrTime, toDoList[eventTime].MyEvent.Date.AddHours((double)toDoList[eventTime].MyEvent.Duration)) <= 0)
                    return true;

                // if current event covers my event
                if (DateTime.Compare(entry.Value.MyEvent.Date, eventTime) < 0 &&
                    DateTime.Compare(incrCurrTime, toDoList[eventTime].MyEvent.Date.AddHours((double)toDoList[eventTime].MyEvent.Duration)) >= 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Removes old entries, or shifts them to future
        /// </summary>
        public static void UpdateItems()
        {
            if (!loadedList)
                throw new InvalidOperationException("Attempt to view not loaded list.");

            List<DateTime> oldItems = new List<DateTime>();

            foreach (KeyValuePair<DateTime, DiaryEntry> entry in toDoList)
            {
                // if date of event is old, than save 
                // it in order to process it later
                if (DateTime.Compare(entry.Key, DateTime.Now) < 0)
                    oldItems.Add(entry.Key);
            }

            foreach (DateTime time in oldItems)
            {
                // copy whole object
                DiaryEntry tempEntry = new DiaryEntry(toDoList[time].Person, toDoList[time].MyEvent);

                toDoList.Remove(time);

                // if not just remove - increment this
                // date on specified number of days
                if (!RemoveYesterdayBusiness)
                {
                    // make a huge copy of current 'time' object
                    tempEntry.MyEvent.Date = time.AddDays(DaysToAdd);

                    // paste it again to list
                    toDoList.Add(tempEntry.MyEvent.Date, tempEntry);
                }
            }
        }

        #endregion

        #region Search region

        /// <summary>
        /// Searches specified substring 
        /// in all fiesds of entries
        /// </summary>
        /// <param name="substring">Searching parameter</param>
        /// <returns>List of found objects</returns>
        public static List<DiaryEntry> Search(string substring)
        {
            if (!loadedList)
                throw new InvalidOperationException("Cannot search in not loaded list.");

            List<DiaryEntry> entriesFound = new List<DiaryEntry>();

            foreach (KeyValuePair<DateTime, DiaryEntry> entry in toDoList)
            {
                if (entry.Value.MyEvent.Place.Contains(substring) ||
                    entry.Value.MyEvent.ShortDescription.Contains(substring) ||
                    entry.Value.Person.ToString().Contains(substring))
                    entriesFound.Add(new DiaryEntry(entry.Value));
            }

            return entriesFound;
        }

        public static List<DiaryEntry> Search(string substring, List<DiaryEntry> lookIn)
        {
            if (!loadedList)
                throw new InvalidOperationException("Cannot search in not loaded list.");

            List<DiaryEntry> entriesFound = new List<DiaryEntry>();

            foreach (DiaryEntry entry in lookIn)
            {
                if (entry.MyEvent.Place.Contains(substring) ||
                    entry.MyEvent.ShortDescription.Contains(substring) ||
                    entry.Person.ToString().Contains(substring))
                    entriesFound.Add(new DiaryEntry(entry));
            }

            return entriesFound;
        }

        public static List<DiaryEntry> SearchPerson(PersonInfo pi)
        {
            if (!loadedList)
                throw new InvalidOperationException("Cannot search in not loaded list.");

            List<DiaryEntry> entriesFound = new List<DiaryEntry>();

            foreach (KeyValuePair<DateTime, DiaryEntry> entry in toDoList)
                if (entry.Value.Person.ToString() == pi.ToString())
                    entriesFound.Add(new DiaryEntry(entry.Value));

            return entriesFound;
        }

        public static List<DiaryEntry> SearchPerson(PersonInfo pi, List<DiaryEntry> lookIn)
        {
            if (!loadedList)
                throw new InvalidOperationException("Cannot search in not loaded list.");

            List<DiaryEntry> entriesFound = new List<DiaryEntry>();

            foreach (DiaryEntry entry in lookIn)
                if (entry.Person.ToString() == pi.ToString())
                    entriesFound.Add(new DiaryEntry(entry));

            return entriesFound;
        }

        public static List<DiaryEntry> SearchAppointment(Appointment searchWhat)
        {
            if (!loadedList)
                throw new InvalidOperationException("Cannot search in not loaded list.");

            List<DiaryEntry> entriesFound = new List<DiaryEntry>();

            foreach (KeyValuePair<DateTime, DiaryEntry> entry in toDoList)
                if (entry.Value.Person.Appointment == searchWhat)
                    entriesFound.Add(new DiaryEntry(entry.Value));

            return entriesFound;
        }

        public static List<DiaryEntry> SearchAppointment(Appointment searchWhat, List<DiaryEntry> lookIn)
        {
            if (!loadedList)
                throw new InvalidOperationException("Cannot search in not loaded list.");

            List<DiaryEntry> entriesFound = new List<DiaryEntry>();

            foreach (DiaryEntry entry in lookIn)
                if (entry.Person.Appointment == searchWhat)
                    entriesFound.Add(new DiaryEntry(entry));

            return entriesFound;
        }

        public static List<DiaryEntry> SearchDate(DateTime date, int sign)
        {
            if (!loadedList)
                throw new InvalidOperationException("Cannot search in not loaded list.");

            List<DiaryEntry> entriesFound = new List<DiaryEntry>();

            foreach (KeyValuePair<DateTime, DiaryEntry> entry in toDoList)
            {
                DateTime tempDate1 = new DateTime(entry.Value.MyEvent.Date.Year, 
                    entry.Value.MyEvent.Date.Month, entry.Value.MyEvent.Date.Day);

                DateTime tempDate2 = new DateTime(date.Year, date.Month, date.Day);

                if (Math.Sign((tempDate1 - tempDate2).Days) == Math.Sign(sign))
                    entriesFound.Add(new DiaryEntry(entry.Value));
            }

            return entriesFound;
        }

        public static List<DiaryEntry> SearchDate(DateTime date, int sign, List<DiaryEntry> lookIn)
        {
            if (!loadedList)
                throw new InvalidOperationException("Cannot search in not loaded list.");

            List<DiaryEntry> entriesFound = new List<DiaryEntry>();

            foreach (DiaryEntry entry in lookIn)
            {
                DateTime tempDate1 = new DateTime(entry.MyEvent.Date.Year,
                    entry.MyEvent.Date.Month, entry.MyEvent.Date.Day);

                DateTime tempDate2 = new DateTime(date.Year, date.Month, date.Day);

                if (Math.Sign((tempDate1 - tempDate2).Days) == Math.Sign(sign))
                    entriesFound.Add(new DiaryEntry(entry));
            }

            return entriesFound;
        }

        #endregion
    }
}