using System;
using System.Collections.Generic;
using System.Text;

namespace BatchFileLoader
{
    public interface iTransaction
    {
        long SaveTransaction();
    }
}
