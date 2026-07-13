using System;
using System.Collections.Generic;
using System.Text;

namespace FileMonitoring
{
    class Merchant
    {
        private string m_MerchantID;
        private string m_FtpPath;
        private string m_ArchivePath;
        private string m_CompletePath;

        public Merchant() { }

        public Merchant(string MerchID, string FtpPath, string ArchivePath)
        {
            m_MerchantID = MerchID;
            m_FtpPath = FtpPath;
            m_ArchivePath = ArchivePath;
        }


        public string MerchantID
        {
            get { return m_MerchantID; }
            set { m_MerchantID = value; }
        }

        public string FtpPath
        {
            get { return m_FtpPath; }
            set { m_FtpPath = value; }
        }

        public string ArchivePath
        {
            get { return m_ArchivePath; }
            set { m_ArchivePath = value; }
        }

        public string CompletePath
        {
            get { return m_ArchivePath + @"Complete\"; }
            set { m_CompletePath = value; }
        }
        
    }
}
