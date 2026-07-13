using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

public partial class wucEmailList : System.Web.UI.UserControl
{
    public string UID
    {
        get { return Convert.ToString(ViewState["UID"]); }
        set { ViewState["UID"] = Convert.ToString(value); }
    }

    public string Module
    {
        get { return Convert.ToString(ViewState["Module"]); }
        set { ViewState["Module"] = Convert.ToString(value); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

        }
    }
}
