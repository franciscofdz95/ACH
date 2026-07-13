using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public class Journal
    {
        long m_JournalID = 0;

        public long JournalID
        {
            get { return m_JournalID; }
            set { m_JournalID = value; }
        }

        public bool PostJournal() 
        { 
            return true; 
        }

    }
}
