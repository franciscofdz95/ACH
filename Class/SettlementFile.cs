using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;
using PaymentXP.BusinessObjects.Logging;


namespace AchSystem
{
    public class SettlementFile:NachaFile 
    {

        public SettlementFile() { }

        public SettlementFile(Bank bank)
        {
            this.Bank = bank;
            this.TotalLines += 2;
        }

        public SettlementFile(ILogging Log) 
        {
            this.NachFileLog = Log;
        }

        public override NachaBatch CreateBatch()
        {
            return new SettlementBatch (this);
        }


        public override NachaBatch CreateBatch(ILogging log)
        {
            return new SettlementBatch(this, log );
        }

    }
}
