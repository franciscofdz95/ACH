using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;


namespace BatchFileLoader
{
    public abstract class NachaFile
    {
        //Header
        string m_RecordTypeHeader = "1";
        string m_PriorityCode = "01";
        string m_ImmediateDestination = string.Empty; 
        string m_Immediate_Origin = string.Empty; 
        string m_FileCreationDate = string.Empty; 
        string m_FileCreationTime = string.Empty;
        string m_FileID = "1";
        string m_RecordSize = "094";
        int m_BlockingFactor = 10;
        string m_FormatCode = "1";
        string m_DestinationName = string.Empty; 
        string m_OriginName = string.Empty; 
        string m_ReferenceCode = string.Empty; 

        //Footer
        string m_RecordTypeFooter = "9";
        int m_BatchCount = 0;
        int m_BlockCount = 0;
        int m_EntryAddendaCount;
        int m_EntryHash = 0;
        decimal m_TotalDebit = 0;
        decimal m_TotalCredit = 0;
        string m_Reserved = string.Empty;
        int m_TotalLines = 0;

        List<NachaBatch> m_Batches = new List<NachaBatch>();

        public NachaFile()
        {
            m_TotalLines += 2;
        }


        public List<NachaBatch> Batches
        {
            get { return m_Batches; }
            set { m_Batches = value; }
        }

        public int TotalLines
        {
            get { return m_TotalLines; }
            set { m_TotalLines = value; }
        }

        public string RecordTypeHeader
        {
            get { return m_RecordTypeHeader; }
            set { m_RecordTypeHeader = value; }
        }

        public string RecordTypeFooter
        {
            get { return m_RecordTypeFooter; }
            set { m_RecordTypeFooter = value; }
        }

        public string PriorityCode
        {
            get { return m_PriorityCode; }
            set { m_PriorityCode = value; }
        }

        public string ImmediateDestination
        {
            get { return m_ImmediateDestination; }
            set { m_ImmediateDestination = value; }
        }

        public string Immediate_Origin
        {
            get { return m_Immediate_Origin; }
            set { m_Immediate_Origin = value; }
        }

        public string FileCreationDate
        {
            get { return m_FileCreationDate; }
            set { m_FileCreationDate = value; }
        }

        public string FileCreationTime
        {
            get { return m_FileCreationTime; }
            set { m_FileCreationTime = value; }
        }

        public string FileID
        {
            get { return m_FileID; }
            set { m_FileID = value; }
        }

        public string RecordSize
        {
            get { return m_RecordSize; }
            set { m_RecordSize = value; }
        }

        public int BlockingFactor
        {
            get { return m_BlockingFactor; }
            set { m_BlockingFactor = value; }
        }

        public string FormatCode
        {
            get { return m_FormatCode; }
            set { m_FormatCode = value; }
        }

        public string DestinationName
        {
            get { return m_DestinationName; }
            set { m_DestinationName = value; }
        }

        public string OriginName
        {
            get { return m_OriginName; }
            set { m_OriginName = value; }
        }

        public string ReferenceCode
        {
            get { return m_ReferenceCode; }
            set { m_ReferenceCode = value; }
        }

        public int BatchCount
        {
            get { return m_BatchCount; }
            set { m_BatchCount = value; }
        }

        public int BlockCount
        {
            get { return m_BlockCount; }
            set { m_BlockCount = value; }
        }

        public int EntryAddendaCount
        {
            get { return m_EntryAddendaCount; }
            set { m_EntryAddendaCount = value; }
        }

        public int EntryHash
        {
            get { return m_EntryHash; }
            set { m_EntryHash = value; }
        }

        public decimal TotalDebit
        {
            get { return m_TotalDebit; }
            set { m_TotalDebit = value; }
        }

        public decimal TotalCredit
        {
            get { return m_TotalCredit; }
            set { m_TotalCredit = value; }
        }

        public string Reserved
        {
            get { return m_Reserved; }
            set { m_Reserved = value; }
        }


        public void AddBatch(NachaBatch batch)
        {
            this.BatchCount += 1;
            this.TotalLines += 2;

            batch.BatchNumber = this.BatchCount;
            this.Batches.Add(batch);
        }

        public abstract NachaBatch CreateBatch();


        public string GetFileHeader()
        {
            return DataLayer.FilePadString(this.m_RecordTypeHeader,1) + 
                   DataLayer.FilePadString(this.PriorityCode,2) + 
                   DataLayer.FilePadString(this.ImmediateDestination,10) +
                   DataLayer.FilePadString(this.Immediate_Origin,10) + 
                   DataLayer.FilePadString(this.FileCreationDate,6) +
                   DataLayer.FilePadString(this.FileCreationTime , 4) + 
                   DataLayer.FilePadString(this.FileID,1) +
                   DataLayer.FilePadString(this.RecordSize,3) + 
                   DataLayer.FilePadNumber(this.BlockingFactor,2) + 
                   DataLayer.FilePadString(this.FormatCode,1) +
                   DataLayer.FilePadString(this.DestinationName,23) + 
                   DataLayer.FilePadString(this.OriginName,23) + 
                   DataLayer.FilePadString(this.ReferenceCode,8); 
        }

        public string GetFileFooter()
        {
            this.BlockCount = this.TotalLines / 10;

            if (this.TotalLines % 10 != 0)
                this.BlockCount++;

            return DataLayer.FilePadString(this.RecordTypeFooter,1) + 
                    DataLayer.FilePadNumber(this.BatchCount,6) + 
                    DataLayer.FilePadNumber(this.BlockCount,6)  +
                    DataLayer.FilePadNumber(this.EntryAddendaCount,8)  + 
                    DataLayer.FilePadNumber(this.EntryHash,10) +
                    DataLayer.FilePadAmount(this.TotalDebit, 12) +
                    DataLayer.FilePadAmount(this.TotalCredit, 12) + 
                    DataLayer.FilePadString(this.Reserved,39);
        }

    }
}
