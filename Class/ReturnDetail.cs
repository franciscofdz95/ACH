using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;


namespace AchSystem
{
    public class ReturnDetail: NachaDetail 
    {
        List<ReturnAddenda> m_Addendas = new List<ReturnAddenda>();

        public List<ReturnAddenda> Addendas
        {
            get { return m_Addendas; }
            set { m_Addendas = value; }
        }

    
        public ReturnDetail( NachaBatch  batch)
        {
            this.Batch = batch;
            this.RecordType = "6";
            this.DiscretionaryData = "  ";
            this.AddendaRecordIndicator = "0";

        }

        public override string GetFileDetail()
        {
            return DataLayer.FilePadString(this.RecordType, 1) +
                   DataLayer.FilePadString(this.TransType, 2) +
                   DataLayer.FilePadString(this.TransRoute, 9) +
                   DataLayer.FilePadString(this.AccountNo, 17) +
                   DataLayer.FilePadAmount(this.Amount, 10) +
                   DataLayer.FilePadString(this.IdentificationNumber, 15) +
                   DataLayer.FilePadString(this.AccountName, 22) +
                   DataLayer.FilePadString(this.DiscretionaryData, 2) +
                   DataLayer.FilePadString(this.AddendaRecordIndicator, 1) +
                   DataLayer.FilePadNumber(this.TraceNumber, 15);
        }

        public override object CreateAddenda()
        {
            return new ReturnAddenda(this);
        }

        public override void AddAddenda(object addenda)
        {
            this.Addendas.Add((ReturnAddenda)addenda);
        }

    }
}
