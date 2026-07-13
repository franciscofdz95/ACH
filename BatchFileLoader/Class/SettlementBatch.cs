using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;


namespace BatchFileLoader
{
    public class SettlementBatch: NachaBatch 
    {
        public SettlementBatch() { }

        public SettlementBatch(NachaFile file)
        {
            this.AchFile = file;
            this.RecordTypeHeader = "5";
            this.ServiceClassCode = "200";
            this.SettlementDate = "   ";
            this.OriginatorStatusCode = "1";

            this.RecordTypeFooter = "8";
        }

        public override void AddDetail(NachaDetail detail)
        {
            //Calculate batch variables
            if (detail.TransType.Substring(1, 1) == "7")
            {
                this.TotalDebit += detail.Amount;
                this.AchFile.TotalDebit += detail.Amount;
            }
            else
            {
                this.TotalCredit += detail.Amount;
                this.AchFile.TotalCredit += detail.Amount;
            }

            this.EntryAddendaCount++;
            this.EntryHash += Convert.ToInt32(detail.TransRoute.Substring(0, 8));

            //Calculate file variables
            this.AchFile.TotalLines++;
            this.AchFile.EntryAddendaCount++;
            this.AchFile.EntryHash += Convert.ToInt32(detail.TransRoute.Substring(0, 8));

            SettlementDetail sd = (SettlementDetail)detail;
            if (sd.Addendas.Count > 0)
            {
                detail.AddendaRecordIndicator = "1";
                this.AchFile.TotalLines++;
                this.EntryAddendaCount++;
                this.AchFile.EntryAddendaCount++;
            }
            
            this.Details.Add(detail);
        }

        public override NachaDetail CreateDetail()
        {
            return new SettlementDetail(this);
        }

        public override string GetBatchHeader()
        {
            return DataLayer.FilePadString(this.RecordTypeHeader,1)  + 
                   DataLayer.FilePadString(this.ServiceClassCode,3) + 
                   DataLayer.FilePadString(this.CompanyName,16) +
                   DataLayer.FilePadString(this.DiscretionaryData,20) + 
                   DataLayer.FilePadString(this.CompanyIdentification,10) + 
                   DataLayer.FilePadString(this.Secc,3) +
                   DataLayer.FilePadString(this.CompanyDescription,10) + 
                   DataLayer.FilePadString(this.CompanyDescriptiveDate,6) + 
                   DataLayer.FilePadString(this.EffectiveEntryDate,6) +
                   DataLayer.FilePadString(this.SettlementDate,3) + 
                   DataLayer.FilePadString(this.OriginatorStatusCode,1) + 
                   DataLayer.FilePadString(this.OriginatingDFI,8) + 
                   DataLayer.FilePadNumber(this.BatchNumber,7);
        }

        public override string GetBatchFooter()
        {
            return DataLayer.FilePadString(this.RecordTypeFooter, 1) +
                   DataLayer.FilePadString(this.ServiceClassCode, 3) +
                   DataLayer.FilePadNumber(this.EntryAddendaCount, 6) +
                   DataLayer.FilePadNumber(this.EntryHash, 10) +
                   DataLayer.FilePadAmount(this.TotalDebit, 12) +
                   DataLayer.FilePadAmount(this.TotalCredit, 12) +
                   DataLayer.FilePadString(this.CompanyIdentification, 10) +
                   DataLayer.FilePadString(this.MessageAuthCode, 19) +
                   DataLayer.FilePadString(this.Reserved, 6) +
                   DataLayer.FilePadString(this.OriginatingDFI, 8) +
                   DataLayer.FilePadNumber(this.BatchNumber, 7);
        }

        public override string GetBatchOffset()
        {
            decimal BatchNetAmount = this.TotalDebit - this.TotalCredit;
            string TransType = string.Empty;

            if (BatchNetAmount > 0)
                TransType = "22";
            else
                TransType = "27";

            return string.Empty;
            //return DataLayer.FilePadString("6", 1) +
            //       DataLayer.FilePadString(TransType, 2) +
            //       DataLayer.FilePadString(this.AchFile.Bank.OriginatingTransRoute, 9) +
            //       DataLayer.FilePadString(this.AchFile.Bank.OriginatingAccountNo, 17) +
            //       DataLayer.FilePadAmount(Math.Abs(BatchNetAmount), 10) +
            //       DataLayer.FilePadString(this.AchFile.Bank.Symbol + " BATCH OFFSET", 15) +
            //       DataLayer.FilePadString(this.CompanyDescription, 22) +
            //       DataLayer.FilePadString("  ", 2) +
            //       DataLayer.FilePadString("0", 1) +
            //       DataLayer.FilePadNumber(this.BatchOffsetTraceNumber , 15) ;
        }

        
    }
}
