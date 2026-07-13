using System;
using System.Collections.Generic;
using System.Text;
using Nmc.Ach.Dal;


namespace BatchFileLoader
{
    public class SettlementFile:NachaFile 
    {

        public SettlementFile()
        {
            this.TotalLines += 2;
        }

        public override NachaBatch CreateBatch()
        {
            return new SettlementBatch (this);
        }

    }
}
