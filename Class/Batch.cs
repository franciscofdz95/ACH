using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public class Batch
    {
        int m_AchID = 0;
        long m_BatchID = 0;
        double m_Debit = 0;
        double m_Credit = 0;
        int m_DebitCount = 0;
        int m_CreditCount = 0;
        double m_OverDailyAmountLimitCount = 0;
        double m_OverItemAmount = 0;
        string m_TransBase = string.Empty;

        public Batch(int AchID,long batchid,double debit, double credit,int debitcount, int creditcount,double overdailylimit,double overitemlimit)
        {
            m_AchID = AchID;
            m_BatchID = batchid;
            m_Debit = debit;
            m_Credit = credit;
            m_DebitCount = debitcount;
            m_CreditCount = creditcount;
            m_OverDailyAmountLimitCount = overdailylimit;
            m_OverItemAmount = overitemlimit;
        }
        public override string ToString()
        {
            return m_BatchID.ToString();
        }
        public int AchID
        {
            get { return m_AchID; }
            set { m_AchID = value; }
        }
        public long BatchID
        {
            get { return m_BatchID; }
            set { m_BatchID = value; }
        }
        public double Debit
        {
            get { return m_Debit; }
            set { m_Debit = value; }
        }
        public double Credit
        {
            get { return m_Credit; }
            set { m_Credit = value; }
        }
        public int DebitCount
        {
            get { return m_DebitCount; }
            set { m_DebitCount = value; }
        }
        public int CreditCount
        {
            get { return m_CreditCount; }
            set { m_CreditCount = value; }
        }
        public double OverDailyAmountLimitCount
        {
            get { return m_OverDailyAmountLimitCount; }
            set { m_OverDailyAmountLimitCount = value; }
        }
        public double OverItemAmount
        {
            get { return m_OverItemAmount; }
            set { m_OverItemAmount = value; }
        }
        public string TransBase
        {
            get { return m_TransBase; }
            set { m_TransBase = value; }
        }
        public bool PostBatch()
        {
            return true;
        }

    }
}
