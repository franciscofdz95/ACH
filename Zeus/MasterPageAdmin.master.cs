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

public partial class MasterPageAdmin : frmBaseMaster
{
    public enum eMasterSideMenu : int
    {
        NotSet,
        Users,
        ManageRoles,
        LookupTables
    }

    public void SideMenuSelect(eMasterSideMenu eM)
    {
        
        switch (eM)
        {
            case eMasterSideMenu.Users:
                wucTopMenu1.StatusBarText = "Users";
                lnkAddUsers.CssClass = "active";
                break;

            case eMasterSideMenu.ManageRoles:
                wucTopMenu1.StatusBarText = "Manage Roles";
                lnkGeneralInfo.CssClass = "active";
                break;

            case eMasterSideMenu.LookupTables:
                wucTopMenu1.StatusBarText = "Lookup Tables";
                lnkLookupTables.CssClass = "active";
                break;

        }

    }

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
