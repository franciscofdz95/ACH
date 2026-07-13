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

public partial class wucTradeVerification : System.Web.UI.UserControl
{

    public string SetFullName
    {
        get { return FullName.Text; }
        set { FullName.Text = value; }
    }

    public string SetCreditScore
    {
        get { return CreditScore.Text; }
        set { CreditScore.Text = value; }
    }

    public string SetTitle
    {
        get { return lblTitle.Text; }
        set { lblTitle.Text = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
