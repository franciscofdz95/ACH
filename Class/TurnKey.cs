using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AchSystem
{
    public class TurnKey
    {
        decimal m_BatchNetAmount = 0;
        decimal m_TransactionTotal = 0;
        decimal m_TurnKeySplitTotal = 0; //turn key split
        decimal m_DistributorSplitTotal = 0; //distributor split
        string m_DistributorABA = "";
        string m_DistributorDDA = "";
        string m_TurnKey_ABA = "";
        string m_TurnKey_DDA = "";

        public decimal BatchNetAmount { get { return m_BatchNetAmount;} set{ m_BatchNetAmount = value;} }
        public decimal TransactionTotal { get { return m_TransactionTotal; } set { m_TransactionTotal = value; } }
        public decimal TurnKeySplitTotal { get { return m_TurnKeySplitTotal; } set { m_TurnKeySplitTotal = value; } }
        public decimal DistributorSplitTotal { get { return m_DistributorSplitTotal; } set { m_DistributorSplitTotal = value; } }

        public string DistributorABA { get { return m_DistributorABA; } set { m_DistributorABA = value; } }
        public string DistributorDDA { get { return m_DistributorDDA; } set { m_DistributorDDA = value; } }
        public string TurnKey_ABA { get { return m_TurnKey_ABA; } set { m_TurnKey_ABA = value; } }
        public string TurnKey_DDA { get { return m_TurnKey_DDA; } set { m_TurnKey_DDA = value; } }
    }
}
