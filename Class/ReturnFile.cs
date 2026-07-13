using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;
using PaymentXP.BusinessObjects.Logging;


namespace AchSystem
{
    public class ReturnFile: NachaFile 
    {

        public ReturnFile() { }

        public ReturnFile(Bank bank)
        {
            this.Bank = bank;
            this.TotalLines += 2;
        }

        public override NachaBatch CreateBatch()
        {
            return new ReturnBatch  (this);
        }

        public override NachaBatch CreateBatch(ILogging log)
        {
            return new ReturnBatch(this, log);
        }
    }
}
