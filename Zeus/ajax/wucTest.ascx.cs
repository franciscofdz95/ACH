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

public partial class ajax_wucTest : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void SetParams(object o)
    {
        HttpRequest req = (HttpRequest)o;
        string MerchantAppUID = req["UserName"].ToString();

        Label1.Text = MerchantAppUID;
    }

    public void ForcePreRender()
    {
        this.Page_Load(null, null);


    }
}
