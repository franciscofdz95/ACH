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
using PaymentXP.Facade;

public partial class frmTicketPopup : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        this.UID = CommonUtility.Util.if_s(Request.QueryString["TicketUID"], null);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!IsPostBack)
        {
            if (Request.QueryString["Adding"] != null)
                this.ticket1.Adding = Convert.ToBoolean(Request.QueryString["Adding"].ToString());

            if (Request.QueryString["RequestOrigin"] != null)
                this.ticket1.RequestOrigin = Request.QueryString["RequestOrigin"].ToString();

            if (Request.QueryString["TicketUID"] != null)
                this.ticket1.UID = Request.QueryString["TicketUID"].ToString();

            //            this.ticket1.CloseBtn.Visible = true;


            if (UserSessions.CurrentTicket != null)
            {
                this.Page.Title = string.Format("TicketID: {0} - DBA: {1} - ZID: {2}", UserSessions.CurrentTicket.TicketID, UserSessions.CurrentTicket.BusinessDBAName, UserSessions.CurrentTicket.ZID);
            }

            if (CommonUtility.Util.IsValidGuid(Request.QueryString["TicketUID"]))
            {
                string tuid = Request.QueryString["TicketUID"].ToUpper();

                TicketNotification.InsertTicketAccessHistory(new Guid(UserSessions.CurrentUser.UID), new Guid(tuid));

                //UserApplication.AddViewLogTicket(tuid, UserSessions.CurrentUser.UserName);
            }

        }
    }



    public override void FormShow(string ID)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormNew()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDataCheck()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormCancel()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void ToggleButtons()
    {
        throw new Exception("The method or operation is not implemented.");
    }
}
