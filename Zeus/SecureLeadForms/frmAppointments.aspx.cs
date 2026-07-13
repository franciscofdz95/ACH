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
using System.Collections.Generic;

public partial class frmAppointments : frmBaseSearch
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        ((HyperLink)this.Master.FindControl("lnkAppointments")).CssClass = "active";

        if (!this.IsPostBack)
        {
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Appointments Search";        
        }        
    }
}
