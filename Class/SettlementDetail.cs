using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nmc.Ach.Dal;


namespace AchSystem
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
            this.DiscretionaryData = "  ";
            this.AddendaRecordIndicator = "0";
        }

        public override string GetFileDetail()
        {
            switch (this.Batch.Secc)
            { 
                case "WEB":
                    if (this.RecurID != 0)
                        this.DiscretionaryData = "R ";
                    else
                        this.DiscretionaryData = "S ";

                    break;
                case "RCK":
                    this.IdentificationNumber = this.CheckNumber;
                    break;
            }

/* NCAL */
            if (this.Batch.AchFile.Bank.BankID == (int)PaymentXP.BusinessObjects.AchBankInfo.FIFTH_THIRD) //53rd
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
                       this.Batch.AchFile.ImmediateDestination.Substring(1, 8) + DataLayer.FilePadNumber(this.TraceNumber, 7);
                //"24207175" + DataLayer.FilePadNumber (this.TraceNumber,7);
            }
            else if (this.Batch.AchFile.Bank.BankID == (int)PaymentXP.BusinessObjects.AchBankInfo.NCAL) //ncal
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
                       this.Batch.AchFile.ImmediateDestination.Substring(1, 8) + DataLayer.FilePadNumber(this.TraceNumber, 7);
            }
            else if (this.Batch.AchFile.Bank.BankID == (int)PaymentXP.BusinessObjects.AchBankInfo.FDR) //FDR
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
                       "12203936" + DataLayer.FilePadNumber(this.TraceNumber, 7);
            }
            else if (this.Batch.AchFile.Bank.BankID == (int)PaymentXP.BusinessObjects.AchBankInfo.GOLDMAN_SACHS) //GOLDMAN SACHS
            {
                return DataLayer.FilePadString(this.RecordType, 1) +
                       DataLayer.FilePadString(this.TransType, 2) +
                       DataLayer.FilePadString(this.TransRoute.Substring(0, 8), 8) +
                       DataLayer.FilePadString(this.TransRoute.Substring(TransRoute.Length - 1), 1) +
                       DataLayer.FilePadString(this.AccountNo, 17) +
                       DataLayer.FilePadAmount(this.Amount, 10) +
                       DataLayer.FilePadString(this.IdentificationNumber, 15) + // Field 7
                       // Secc CCD, PPD, WEB and TEL
                       DataLayer.FilePadString(this.AccountName, 22) + // Field 8
                       DataLayer.FilePadString(this.DiscretionaryData, 2) + // Field 9
                       DataLayer.FilePadString(this.AddendaRecordIndicator, 1) +// Field 10                      
                       DataLayer.FilePadString("02601507", 8) + DataLayer.FilePadNumber(this.TraceNumber, 7); // Field 11
            }
            else
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
                       (string.IsNullOrWhiteSpace(this.Batch.AchFile.Immediate_Origin) ? string.Empty : this.Batch.AchFile.Immediate_Origin.Substring(1, 8)) + DataLayer.FilePadNumber(this.TraceNumber, 7);
            }
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
