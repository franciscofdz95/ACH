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

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

public partial class MasterPageReports : frmBaseMaster
{
    public int ErrorCount()
    {
        return this.WucMessage1.ErrorCount();
    }

    public void AddMessageError(string msg)
    {
        this.WucMessage1.AddMessageError(msg);
    }

    public void AddMessageStatus(string msg)
    {
        this.WucMessage1.AddMessageStatus(msg);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        if (!this.IsPostBack)
        {
            FormHandler.SetSecurity(this.Page.Master);
        }
    }
}
