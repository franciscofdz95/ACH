using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class MasterPageAccounting : frmBaseMaster
{
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
