using System;
using System.Collections.Generic;
using System.Text;

namespace EDiary.Staff
{
    /// <summary>
    /// Class - container for
    /// all persons in company
    /// </summary>
    public static class PersonList
    {
        public static List<PersonInfo> Persons;

        static PersonList()
        {
            // create this list in static constructor
            Persons = new List<PersonInfo>();
        }
    }
}
