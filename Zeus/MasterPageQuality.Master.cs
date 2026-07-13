using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPageQuality : frmBaseMaster
{
    public enum eMasterSideMenu : int
    {
        NotSet,
        AgentAllocations,
        AgentAllocationsDetail,
        ApplicationErrors,
        TicketErrors
    }
    public void SetStatusBarText(string msg)
    {
        wucTopMenu1.StatusBarText = msg;
    }

    protected void Page_Load(eMasterSideMenu eM)
    {
        switch (eM)
        {
            case eMasterSideMenu.AgentAllocations:
                break;
            case eMasterSideMenu.AgentAllocationsDetail:
                break;
            case eMasterSideMenu.ApplicationErrors:
                wucTopMenu1.StatusBarText = "Application Errors";
                lnkApplicationErrors.CssClass = "active";
                break;
            case eMasterSideMenu.TicketErrors:
                wucTopMenu1.StatusBarText = "Ticket Errors";
                lnkTicketErrors.CssClass = "active";
                break;
            default:
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

    public void AddMessageSuccess(string msg)
    {
        this.WucMessage1.AddMessageSuccess(msg);
    }

}
