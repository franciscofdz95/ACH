using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;
using PaymentXP.BusinessObjects.Logging;

namespace AchSystem
{
    public class ReturnBatch : NachaBatch 
    {
        private ILogging ReturnBatchLog = new Log();


        public ReturnBatch() { }

        public ReturnBatch(NachaFile file)
        {
            this.AchFile = file;
            this.RecordTypeHeader = "5";
            this.ServiceClassCode = "200";
            this.SettlementDate = "   ";
            this.OriginatorStatusCode = "1";

            this.RecordTypeFooter = "8";
        }

        public ReturnBatch(NachaFile file, ILogging log)
        {
            this.AchFile = file;
            this.RecordTypeHeader = "5";
            this.ServiceClassCode = "200";
            this.SettlementDate = "   ";
            this.OriginatorStatusCode = "1";
            this.RecordTypeFooter = "8";

            this.ReturnBatchLog = log;
        }

        public override void AddDetail(NachaDetail detail)
        {
            //Calculate batch variables
            if (detail.TransType.Substring(1, 1) == "6")
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

            if (detail.TransRoute.Length == 9)
                this.EntryHash = this.EntryHash + Convert.ToInt64(detail.TransRoute.Substring(0, 8));

            //Calculate file variables
            this.AchFile.TotalLines++;
            this.AchFile.EntryAddendaCount++;
            this.AchFile.EntryHash += Convert.ToInt64(detail.TransRoute.Substring(0, 8));
                        
            this.Details.Add(detail);
        }

        public override NachaDetail CreateDetail()
        {
            return new ReturnDetail(this);
        }

        public override string GetBatchHeader()
        {
            return DataLayer.FilePadString(this.RecordTypeHeader, 1) +
                   DataLayer.FilePadString(this.ServiceClassCode, 3) +
                   DataLayer.FilePadString(this.CompanyName, 16) +
                   DataLayer.FilePadString(this.DiscretionaryData, 20) +
                   DataLayer.FilePadString(this.CompanyIdentification, 10) +
                   DataLayer.FilePadString(this.Secc, 3) +
                   DataLayer.FilePadString(this.CompanyDescription, 10) +
                   DataLayer.FilePadString(this.CompanyDescriptiveDate, 6) +
                   DataLayer.FilePadString(this.EntyDate, 6) +
                   DataLayer.FilePadString(this.SettlementDate, 3) +
                   DataLayer.FilePadString(this.OriginatorStatusCode, 1) +
                   DataLayer.FilePadString(this.OriginatingDFI, 8) +
                   DataLayer.FilePadNumber(this.BatchNumber, 7);
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
    }
}
