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

public partial class wucTradeReference : System.Web.UI.UserControl
{

    public string SetTitle
    {
        get { return lblTitle.Text; }
        set { lblTitle.Text = value; }
    }

    public Panel pnlTR
    {
        get { return pnl; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //Added Trade country code
    protected void Page_PreRender(object sender, EventArgs e)
    {
        PhoneNumberCountryCodeDisplay.ReadOnly = true;
    }

    override protected void OnInit(EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //Added Trade phone country code
            LookupTableHandler.LoadCountryCallingCodes(PhoneNumberCountryCode);            
        }
    }


}
