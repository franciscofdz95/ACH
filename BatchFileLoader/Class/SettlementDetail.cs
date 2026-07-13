using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;


namespace BatchFileLoader
{
    public class SettlementDetail : NachaDetail
    {
        List<SettlementAddenda> m_Addendas = new List<SettlementAddenda>();

        public List<SettlementAddenda> Addendas
        {
            get { return m_Addendas; }
            set { m_Addendas = value; }
        }

        public SettlementDetail(NachaBatch batch)
        {
            this.Batch = batch;
            this.RecordType = "6";
            this.DescretionaryData = "  ";
            this.AddendaRecordIndicator = "0";
        }

        public override string GetFileDetail()
        {
            switch (this.Batch.Secc)
            { 
                case "WEB":
                    this.DescretionaryData = "S ";
                    break;
                case "RCK":
                    this.IdentificationNumber = this.CheckNumber;
                    break;
            }

            
            return DataLayer.FilePadString( this.RecordType,1) +
                   DataLayer.FilePadString(this.TransType,2) +
                   DataLayer.FilePadString(this.TransRoute,9) +
                   DataLayer.FilePadString(this.AccountNo,17) +
                   DataLayer.FilePadAmount(this.Amount, 10) +
                   DataLayer.FilePadString(this.IdentificationNumber,15) +
                   DataLayer.FilePadString(this.AccountName,22) +
                   DataLayer.FilePadString(this.DescretionaryData,2) +
                   DataLayer.FilePadString(this.AddendaRecordIndicator,1) +
                   DataLayer.FilePadNumber (this.TraceNumber,15);
        }

        public override object CreateAddenda()
        {
            return new SettlementAddenda(this);
        }

        public override void AddAddenda(object addenda)
        {
            this.Addendas.Add((SettlementAddenda)addenda);
        }
    }
}
