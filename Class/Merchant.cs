using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public class Merchant
    {
        long m_MerchantID = 0;
        long m_AchID = 0;
        string m_MerchantName = string.Empty;
        string m_Email = string.Empty;
        ArrayList m_Batches;
        string m_MerchantAppUID = string.Empty;

        public Merchant(long AchID, string MerchantName)
        {
            m_Batches = new ArrayList();

            m_AchID = AchID;
            m_MerchantName = MerchantName;
        }
        public Merchant(long AchID, long MerchantID,string MerchantName)
        {
            m_Batches = new ArrayList();

            m_AchID = AchID;
            m_MerchantID = MerchantID;
            m_MerchantName = MerchantName;
        }
        public override string ToString()
        {
            return m_MerchantName;
        }
        public ArrayList Batches
        {
            get { return m_Batches; }
            set { m_Batches = value; }
        }
        public long MerchantID
        {
            get { return m_MerchantID; }
            set { m_MerchantID = value; }
        }
        public long AchID
        {
            get { return m_AchID; }
            set { m_AchID = value; }
        }
        public string MerchantName
        {
            get { return m_MerchantName; }
            set { m_MerchantName = value; }
        }
        public string Email
        {
            get { return m_Email; }
            set { m_Email = value; }
        }
        public string MerchantAppUID
        {
            get { return m_MerchantAppUID; }
            set { m_MerchantAppUID = value; }
        }

        public int UploadID { get; set; }
        
    }


}
