using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections;
using System.Text;
using Nmc.Ach.Dal;
using PaymentXP.BusinessObjects.Logging;

namespace AchSystem
{
    public class BankFDR : Bank
    {

        public BankFDR(string bank, int bankid)
            : base(bank, bankid)
        
        {
            
        }

        public BankFDR(string bank, int bankid, ILogging log)
            : base(bank, bankid)
        {
            this.BankLog = log;
        }

        public override bool CreateBatch(Merchant merchant)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool CreateJournal(Batch batch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool JournalReleaseHold()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void CreateBatchOffsetDetailRecord(NachaBatch batch, ref int DetailCount)
        {
            decimal BatchNetAmount = batch.TotalDebit - batch.TotalCredit;

            if (BatchNetAmount == 0) //Offset is not required
                return;

            NachaDetail detail = null;

            detail = batch.CreateDetail();



            if (BatchNetAmount > 0)
                detail.TransType = "22";
            else if (BatchNetAmount < 0)
                detail.TransType = "27";
            else
                detail.TransType = "28";


            detail.AccountName = "OFFSET - DLY ADV";
            detail.AccountNo = "2549980";
            detail.TransRoute = "122039360";
            detail.Amount = Math.Abs(BatchNetAmount);
            detail.TraceNumber = ++DetailCount;
            detail.IdentificationNumber = "871301000700";
            detail.DiscretionaryData = "99";
            batch.AddDetail(detail);

        }

        public virtual NachaDetail GetBatchOffsetDetailRecord(NachaBatch batch, ref int DetailCount)
        {
            decimal BatchNetAmount = batch.TotalDebit - batch.TotalCredit;

            if (BatchNetAmount == 0) //Offset is not required
                return null;

            NachaDetail detail = null;

            detail = batch.CreateDetail();



            if (BatchNetAmount > 0)
                detail.TransType = "22";
            else if (BatchNetAmount < 0)
                detail.TransType = "27";
            else
                detail.TransType = "28";


            detail.AccountName = "OFFSET - DLY ADV";
            detail.AccountNo = "2549980";
            detail.TransRoute = "122039360";
            detail.Amount = Math.Abs(BatchNetAmount);
            detail.TraceNumber = ++DetailCount;
            detail.IdentificationNumber = "871301000700";
            detail.DiscretionaryData = "99";
            batch.AddDetail(detail);

            return detail;

        }
        
    }
}
