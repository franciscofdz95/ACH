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

public partial class MasterPageHome : frmBaseMaster
{
    public enum eMasterSideMenu : int
    {
        NotSet,
        Home,
        ApplicationProcessing,
        CreditUnderwriting,
        ApplicationBoarding,
        Deployment,
        ClientServices,
        Retention,
        OnlineApps,
        DepartmentTickets
    }

    public void SideMenuSelect(eMasterSideMenu eM)
    {
        switch (eM)
        {
            case eMasterSideMenu.Home:
                wucTopMenu1.StatusBarText = "Home";
                lnkHomePage.CssClass = "active";
                break;

            case eMasterSideMenu.ApplicationProcessing:
                wucTopMenu1.StatusBarText = "Queue: Sales Support";
                lnkQueueAP.CssClass = "active";
                break;

            case eMasterSideMenu.CreditUnderwriting:
                wucTopMenu1.StatusBarText = "Queue: Credit Underwriting";
                lnkQueueCU.CssClass = "active";
                break;

            case eMasterSideMenu.ApplicationBoarding:
                wucTopMenu1.StatusBarText = "Queue: Operations";
                lnkQueueDE.CssClass = "active";
                break;

            case eMasterSideMenu.Deployment:
                wucTopMenu1.StatusBarText = "Queue: Deployment";
                lnkQueueDP.CssClass = "active";
                break;

            case eMasterSideMenu.ClientServices:
                wucTopMenu1.StatusBarText = "Queue: Merchant Support";
                lnkQueueCS.CssClass = "active";
                break;
            case eMasterSideMenu.Retention:
                wucTopMenu1.StatusBarText = "Queue: Retention";
                lnkQueueRT.CssClass = "active";
                break;
            case eMasterSideMenu.OnlineApps:
                wucTopMenu1.StatusBarText = "OnlineApps";
                lnkQueueOA.CssClass = "active";
                break;

           case eMasterSideMenu.DepartmentTickets:
                wucTopMenu1.StatusBarText = "Department Tickets Summary";
                lnkDeptTickets.CssClass = "active";
                break;
                
        }

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
