using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Nmc.Ach.Dal;

namespace AchSystem
{
    class BankNct : Bank
    {
        public BankNct(string bank, int bankid)
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

 
    }
}
