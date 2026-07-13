using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

/// <summary>
/// Summary description for Enum
/// </summary>

public enum RecordNavigation
{
    First,
    Previous,
    Next,
    Last
}

public enum CONDITIONS
{
    CREDITUNDERWRITING = 1,
    RELATIONSHIPMANAGEMENT,
    APPPROCESSING
}

public enum eButtonSet : int
{
    Search = 1,
    Edit = 2,
    ReadOnly = 3,
    Add = 4
}

public enum eDaysHoldType : int
{
    NotSet = 0,
    CalendarDays = 1,
    BusinessDays = 2
}

public enum BillingTypeEnum
{
    Select = -1,
    Annual = 0,
    Annual_PCI = 1,
    PCI_NAF = 2
}


public class Enum
{
    public Enum()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}

public enum Platform
{
    [Description("")]
    NoPlatform = 0,

    [Description("fdms")]
    FirstDataNashville,

    [Description("pxgatewy")]
    PaysafeProcessingPxP,

    [Description("psfconty")]
    PaysafeContinuity

}

public enum NMIMerchantStatus
{
    [Description("")]
    no_status = 0,
    active,
    closed,
    restricted,
    deleted
}
