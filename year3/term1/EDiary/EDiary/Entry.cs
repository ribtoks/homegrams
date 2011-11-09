using System;
using System.Collections.Generic;
using System.Text;

namespace EDiary
{
    [Serializable()]
    public enum Appointment { Swabber, StaffManager, Developer, ProjectManager, CoDirector, Director, God }

    [Serializable()]
    public class PersonInfo
    {
        #region Private data

        // person name
        string name;
        // person surname
        string surname;  
      
        // person assignment in organization
        Appointment appointment;

        // contact phone
        List<string> phoneNumbers;
        // contact e-mail
        List<string> emails;

        #endregion

        #region Constructors

        public PersonInfo()
        {
            name = "Nobody";
            surname = "Noone";
            appointment = Appointment.Swabber;
            phoneNumbers = new List<string>();
            emails = new List<string>();
        }

        public PersonInfo(string pName, string pSurname, Appointment pAssignment,
            List<string> pPhoneNumbers, List<string> pEmails)
        {
            name = pName;
            surname = pSurname;
            appointment = pAssignment;
            phoneNumbers = new List<string>(pPhoneNumbers);
            emails = new List<string>(pEmails);
        }

        public PersonInfo(PersonInfo from)
            :this(from.name, from.surname, from.appointment, from.phoneNumbers, from.emails)
        {
        }

        #endregion

        #region Properties

        public List<string> Emails
        {
            get { return emails; }
            set { emails = value; }
        }

        public List<string> PhoneNumbers
        {
            get { return phoneNumbers; }
            set { phoneNumbers = value; }
        }

        public Appointment Appointment
        {
            get { return appointment; }
            set { appointment = value; }
        }

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        public override string ToString()
        {
            return name + " " + surname + " (" + appointment.ToString() + ")";
        }
    }

    [Serializable()]
    public class EventInfo
    {
        #region Inner data

        // time of event
        DateTime date;

        // duration of event in hours
        int duration;

        // description of place, where event would take place
        string place;

        // short description of event
        string shortDescription;

        #endregion

        #region Properties

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public string Place
        {
            get { return place; }
            set { place = value; }
        }

        public string ShortDescription
        {
            get { return shortDescription; }
            set { shortDescription = value; }
        }

        #endregion

        #region Constructors

        public EventInfo()
        {
            date = new DateTime();
            duration = -1;
            place = "";
            shortDescription = "no description";
        }

        public EventInfo(DateTime fDate, int fDuration, string fPlace, string fShortDescription)
        {
            date = fDate;
            duration = fDuration;
            place = fPlace;
            shortDescription = fShortDescription;
        }

        public EventInfo(EventInfo from)
            :this(from.date, from.duration, from.place, from.shortDescription)
        {
        }

        #endregion

        public override string ToString()
        {
            return date.ToShortDateString() + " " + shortDescription;
        }
    }

    [Serializable()]
    public class DiaryEntry
    {
        private PersonInfo person;
        private EventInfo myEvent;

        public DiaryEntry()
        {
            person = new PersonInfo();
            myEvent = new EventInfo();
        }

        public DiaryEntry(PersonInfo p, EventInfo e)
        {
            person = new PersonInfo(p);
            myEvent = new EventInfo(e);
        }

        public DiaryEntry(DiaryEntry from)
            :this(from.person, from.myEvent)
        {
        }

        public PersonInfo Person
        {
            get { return person; }
            set { person = value; }
        }

        public EventInfo MyEvent
        {
            get { return myEvent; }
            set { myEvent = value; }
        }
    }
}
