using System;
using System.Collections.Generic;
using System.Text;


namespace Nmc.Ach.Dal
{
    public class DataBaseClass
    {
        private string m_KeyColumnName = string.Empty;

        public string KeyColumnName
        {
            get { return m_KeyColumnName; }
            set { m_KeyColumnName = value; }
        }

    }
}
