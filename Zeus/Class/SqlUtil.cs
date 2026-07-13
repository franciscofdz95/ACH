using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for SqlUtil
/// </summary>
public class SqlUtil
{
    public SqlUtil()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static int ConvertSortDirectionToSql(SortDirection direction)
    {
        int newSortDirection;
        switch (direction)
        {
            case SortDirection.Descending:
                newSortDirection = 1;
                
                break;

            default:
                newSortDirection = 0;
                
                break;
        }
        return newSortDirection;
    }
}
