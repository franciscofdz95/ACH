using System;
using System.Collections.Generic;
using System.Text;

namespace AchSystem
{
    public class User
    {
        private int m_UserID;
        private string m_LoginID = string.Empty;
        private string m_FirstName = string.Empty;
        private string m_LastName = string.Empty;
        private bool m_IsAdmin = false;
        private bool m_ReadOnly = false;
        private bool m_ProcessModule = false;
        private bool m_ReportModule = false;
        private bool m_UtilityModule = false;
        private bool m_LookUpModule = false;
        private bool m_AdminModule = false;
        private bool m_SearchModule = false;
        private bool m_RiskModule = false;
        

        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }

        public string LoginID
        {
            get { return m_LoginID; }
            set { m_LoginID = value; }
        }

        public string FirstName
        {
            get { return m_FirstName; }
            set { m_FirstName = value; }
        }

        public string LastName
        {
            get { return m_LastName; }
            set { m_LastName = value; }
        }

        public bool IsAdmin
        {
            get { return m_IsAdmin; }
            set { m_IsAdmin = value; }
        }

        public bool ReadOnly
        {
            get { return m_ReadOnly; }
            set { m_ReadOnly = value; }
        }

        public bool ProcessModule
        {
            get { return m_ProcessModule; }
            set { m_ProcessModule = value; }
        }

        public bool ReportModule
        {
            get { return m_ReportModule; }
            set { m_ReportModule = value; }
        }

        public bool UtilityModule
        {
            get { return m_UtilityModule; }
            set { m_UtilityModule = value; }
        }

        public bool LookUpModule
        {
            get { return m_LookUpModule; }
            set { m_LookUpModule = value; }
        }
        public bool AdminModule
        {
            get { return m_AdminModule; }
            set { m_AdminModule = value; }
        }

        public bool RiskModule
        {
            get { return m_RiskModule; }
            set { m_RiskModule = value; }
        }
        public bool SearchModule
        {
            get { return m_SearchModule; }
            set { m_SearchModule = value; }
        }

    }
}
