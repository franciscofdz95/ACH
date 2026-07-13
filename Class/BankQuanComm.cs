using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;

namespace AchSystem
{
    class BankQuanComm: Bank 
    {
        public BankQuanComm()
		{
		}

        public BankQuanComm(string bank, int bankid)
            : base(bank, bankid){}

        public override bool CreateBatch(Merchant merchant)
        {
            Console.WriteLine(this.BankName + "," + merchant.MerchantName + ", CreateBatch");
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
    }
}
