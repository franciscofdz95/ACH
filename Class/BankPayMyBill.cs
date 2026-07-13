using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using Nmc.Ach.Dal;

namespace AchSystem
{
	/// <summary>
	/// Summary description for BankPayMyBill.
	/// </summary>
	public class BankPayMyBill:Bank
	{
		public BankPayMyBill(){}

        public BankPayMyBill(string bank, int bankid)
            : base(bank, bankid){}


        public override bool CreateBatch(Merchant merchant)
        {
            return true;
        }//CreateBatch

        public override bool CreateJournal(Batch batch)
        {
            return true;
        }//CreateJournal

        public override bool JournalReleaseHold()
        {
            return true;
        }//JournalReleaseHold 


        public override bool CreateSettlementFile(DateTime dateSubmitted)
        {
            return true;
        }//CreateSettlementFile

 


	}
}
