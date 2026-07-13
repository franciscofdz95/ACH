using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;


namespace AchSystem
{
    public class SettlementAddenda
    {
        string m_RecordType = string.Empty;
        string m_TypeCode = string.Empty;
        string m_PaymentInfo = string.Empty;
        int m_AddendaSeqNumber = 0;
        long m_EntrySeqNumber = 0;

        SettlementDetail  m_Detail = null;

        public SettlementAddenda(SettlementDetail detail)
        {
            this.Detail = detail;
            this.RecordType = "7";
            this.TypeCode = "05";
        }

        public SettlementDetail Detail
        {
            get { return m_Detail; }
            set { m_Detail = value; }
        }

        public string RecordType
        { 
            get { return m_RecordType; }
            set { m_RecordType = value; }
        }

        public string TypeCode
        {
            get { return m_TypeCode; }
            set { m_TypeCode = value; }
        }

        public string PaymentInfo
        {
            get { return m_PaymentInfo; }
            set { m_PaymentInfo = value; }
        }

        public int AddendaSeqNumber
        {
            get { return m_AddendaSeqNumber; }
            set { m_AddendaSeqNumber = value; }
        }

        public long EntrySeqNumber
        {
            get { return m_EntrySeqNumber; }
            set { m_EntrySeqNumber = value; }
        }

        public string GetFileAddenda()
        {
            return DataLayer.FilePadString(this.RecordType, 1) +
                   DataLayer.FilePadString(this.TypeCode, 2) +
                   DataLayer.FilePadString(this.PaymentInfo , 80) +
                   DataLayer.FilePadNumber(this.AddendaSeqNumber , 4) +
                   DataLayer.FilePadNumber(this.EntrySeqNumber , 7);
        }
    }
}
