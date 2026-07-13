using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace BatchFileLoader
{
    public interface iData
    {
        DataSet Search(ArrayList prms);
        SqlDataReader Select(ArrayList prms);
        int Delete(ArrayList prms);
        int Update(ArrayList prms);
        long Insert(ArrayList prms);

    }
}
