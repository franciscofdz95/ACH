using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;


namespace AchSystem
{
    public class ReturnAddenda
    {
        string m_RecordType = string.Empty;
        string m_TypeCode = string.Empty;
        string m_ReturnReasonCode = string.Empty;
        long m_OrigTraceNumber = 0;
        string m_DateOfDeath = string.Empty;
        string m_OrigRDFI = string.Empty;
        string m_AddendaInfo = string.Empty;
        long m_TraceNumber = 0;

        ReturnDetail m_Detail = null;

        public ReturnAddenda(ReturnDetail detail)
        {
            this.Detail = detail;
            this.RecordType = "7";
            this.TypeCode = "99";
        }

        public ReturnDetail Detail
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

        public string ReturnReasonCode
        {
            get { return m_ReturnReasonCode; }
            set { m_ReturnReasonCode = value; }
        }

        public long OrigTraceNumber
        {
            get { return m_OrigTraceNumber; }
            set { m_OrigTraceNumber = value; }
        }

        public string DateOfDeath
        {
            get { return m_DateOfDeath; }
            set { m_DateOfDeath = value; }
        }

        public string OrigRDFI
        {
            get { return m_OrigRDFI; }
            set { m_OrigRDFI = value; }
        }

        public string AddendaInfo
        {
            get { return m_AddendaInfo; }
            set { m_AddendaInfo = value; }
        }

        public long TraceNumber
        {
            get { return m_TraceNumber; }
            set { m_TraceNumber = value; }
        }

        public string GetFileAddenda()
        {
            return DataLayer.FilePadString(this.RecordType, 1) +
                   DataLayer.FilePadString(this.TypeCode , 2) +
                   DataLayer.FilePadString(this.ReturnReasonCode , 3) +
                   DataLayer.FilePadNumber(this.OrigTraceNumber, 15) +
                   DataLayer.FilePadString(this.DateOfDeath, 6) +
                   DataLayer.FilePadString(this.OrigRDFI, 8) +
                   DataLayer.FilePadString(this.AddendaInfo, 44) +
                   DataLayer.FilePadNumber(this.TraceNumber, 15);
        }
    }
}
