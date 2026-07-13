using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;

namespace AchSystem
{
    class AchListItem
    {
        private string m_ItemValue = string.Empty;
        private string m_ItemText = string.Empty;

        public AchListItem(string strValue, string strText)
        {
            m_ItemValue = strValue;
            m_ItemText = strText;
        }

        public override string ToString()
        {
            return m_ItemText;
        }

        public string ItemValue
        {
            get { return m_ItemValue; }
            set { m_ItemValue = value; }
        }

        public string ItemText
        {
            get { return m_ItemText; }
            set { m_ItemText = value; }
        }
    }

}
