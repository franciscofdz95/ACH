using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using System.Collections;
using PaymentXP.Facade;
using System.IO;
using System.Configuration;
using CommonUtility;
using PaymentXP.BusinessObjects.Tickets;

public partial class SecureMerchantManagementForms_frmCompliance : frmBaseDataEntry
{


    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;

        if (UserSessions.CurrentMerchantApp != null)
            base.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

    }

    public TimeZones TimeZoneID
    {
        get { return (TimeZones)ViewState["TimeZoneID"]; }
        set { ViewState["TimeZoneID"] = value; }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.WebsiteReview);
            this.TimeZoneID = UserSessions.CurrentUser.TimeZone;
            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Compliance");
            }

            //DM-704
            /*
            string ticket_uid = CommonUtility.Util.if_s(Request.QueryString["TicketUID"]);

            if (CommonUtility.Util.IsValidGuid(ticket_uid))
            {
                // a ticket was passed in, so we load the WSCompliance object and display it in the tab.
                Hashtable prms = new Hashtable();
                prms.Add("@TicketUID", ticket_uid);
                UserSessions.CurrentWSCompliance = DataWSCompliance.SelectWSCompliance(prms, UserSessions.CurrentUser.TimeZone);

                WebTab1.SelectedIndex = 1;
            }
            else
            {
                WebTab1.SelectedIndex = 0;
            }

            if (Request.QueryString["ComplianceCancelled"] != null)
            {
                wucMessage2.AddMessageStatus("Review Cancelled.");
            }
            */
            this.FormShow(null);

        }


        //DM-704
        /*
        // hide the "Checklist Tab" if a review is not selected.
        WebTab1.Tabs[1].Enabled = (UserSessions.CurrentWSCompliance == null) ? false : true;

        //eluxa: disable the checklist tab if there is no current compliance review or if the 
        //current merchant app id does not match the merchant id assigned to the current compliance
        WebTab1.Tabs[1].Enabled = (UserSessions.CurrentWSCompliance == null || (UserSessions.CurrentMerchantApp != null && int.Parse(UserSessions.CurrentMerchantApp.ID) != UserSessions.CurrentWSCompliance.MerchantID)) ? false : true;

        // when an item in the gridview is clicked, let this page know about it.
        wucWSComplianceGrid1.GridViewCommand += new UserControls_wucWSComplianceGrid.GridViewRowCommandHandler(wucWSComplianceGrid1_GridViewCommand);

        wucWSComplianceGrid2.GridViewCommand += new UserControls_wucWSComplianceGrid.GridViewRowCommandHandler(wucWSComplianceGrid1_GridViewCommand);

        wucWSComplianceEdit1.coolevent += new UserControls_wucWSComplianceEdit.MyEventHandler(wucWSComplianceEdit1_coolevent);

        wucWSComplianceEdit1.emailclick += new UserControls_wucWSComplianceEdit.MyEmailClick(wucWSComplianceEdit1_emailclick);
        */
    }

    protected void wucWSComplianceEdit1_emailclick(string pTo, string pFrom, string pCC, string pSubject, List<string> liFiles, string pBody, string pdf_location, string pdf_cleanname)
    {

        tbTo.Text = pTo;
        tbFrom.Text = pFrom;
        tbCC.Text = pCC;
        tbSubject.Text = pSubject;
        tbBody.Text = pBody;
        tbBCC.Text = UserSessions.CurrentUser.Email;
        hidPDFLocation.Value = pdf_location;
        hidPDFCleanName.Value = pdf_cleanname;


        phA.Controls.Clear();

        int counter = 1;
        foreach (string fname in liFiles)
        {
            Label lFile = new Label();
            lFile.Text = string.Format("{0}) {1}", counter, fname);
            lFile.Style.Add("display", "block");
            phA.Controls.Add(lFile);

            counter++;
        }

        WebDialogWindow3.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    protected void WebDialogWindow3_StateChanged(object sender, Infragistics.Web.UI.LayoutControls.DialogWindowStateChangedEventArgs e)
    {
        if (e.NewState == Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden)
        {
            if (!string.IsNullOrEmpty(hidPDFLocation.Value) && File.Exists(hidPDFLocation.Value))
            {
                File.Delete(hidPDFLocation.Value);
            }

        }

    }



    protected void wucWSComplianceGrid1_GridViewCommand(object sender, string TicketUID, string WSComplianceID)
    {
        if (CommonUtility.Util.IsValidGuid(TicketUID))
        {
            WebTab1.SelectedIndex = 1;
            WebTab1.Tabs[1].Enabled = true;

            Response.Redirect(string.Format("~/SecureMerchantManagementForms/frmCompliance.aspx?TicketUID={0}&MerchantAppUID={1}&WSComplianceID={2}",
                TicketUID,
                UserSessions.CurrentMerchantApp.MerchantAppUID,
                WSComplianceID)
                );
        }
    }

    protected void wucWSComplianceEdit1_coolevent(object sender, string e)
    {


        litResult.Text = e;

        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }



    public override void FormShow(string ID)
    {
        // Disable the editablity of the controls. they're for read only on this page.
        wucBusinessInfo1.pnlInfo.Enabled = false;

        if (UserSessions.CurrentMerchantApp != null)
        {
            FormBinding.BindObjectToControls(UserSessions.CurrentMerchantApp, wucBusinessInfo1);


            if (WSComplianceFacade.GetPendingReviewCount(CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0)) == 0)
            {
                // new pending reviews, display button
                wucWSComplianceGrid1.Visible = false;
                pnlButton.Visible = true;
            }
            else
            {
                wucWSComplianceGrid1.Visible = true;
                pnlButton.Visible = false;
                Hashtable prms1 = new Hashtable();
                prms1.Add("@MerchantID", Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));
                prms1.Add("@StatusUID", Constants.COMPLIANCE_TBD);
                wucWSComplianceGrid1.SetDataSource(prms1, 10);
            }

            Hashtable prms2 = new Hashtable();
            prms2.Add("@MerchantID", Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));
            prms2.Add("@StatusUIDList", string.Format("{0},{1},{2},{3},{4},{5}",
                Constants.COMPLIANCE_NONCOMPLIANT,
                Constants.COMPLIANCE_COMPLIANT,
                Constants.COMPLIANCE_WEBSITEINACTIVE,
                Constants.COMPLIANCE_CANCELLED,
                Constants.COMPLIANCE_NOECOMMERCE,
                Constants.COMPLIANCE_REVIEWCOMPLETE));

            wucWSComplianceGrid2.SetDataSource(prms2, 10);
        }
        else
        {
            WebTab1.Tabs[0].Enabled = false;
        }

        MerchantApp app = UserSessions.CurrentMerchantApp;
        wucBusinessInfo1.LoadOffice(app);


    }

    public override void FormClear()
    {
        throw new NotImplementedException();
    }

    public override bool FormSave()
    {
        throw new NotImplementedException();
    }

    public override void FormNew()
    {
        throw new NotImplementedException();
    }

    public override bool FormDelete()
    {
        throw new NotImplementedException();
    }

    public override bool FormDataCheck()
    {
        throw new NotImplementedException();
    }

    public override void FormCancel()
    {
        throw new NotImplementedException();
    }

    public override void ToggleButtons()
    {
        throw new NotImplementedException();
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        // insert ticket.

        Ticket ticket = new Ticket();

        ticket.MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
        ticket.AgentUID = UserSessions.CurrentMerchantApp.AgentUID;
        ticket.StatusID = Ticket.TICKET_ASSIGNED;
        ticket.UserID = UserSessions.CurrentUser.UID;
        ticket.DepartmentID = "15"; // Compliance. let's not hard code this.
        ticket.CategoryID = "963"; // "website review" lets not hard code this
        ticket.ParentID = "962"; // "website review"
        ticket.Problem = string.Format("Website Review Request for ZID: {0}, DBA: {1}", UserSessions.CurrentMerchantApp.ID, UserSessions.CurrentMerchantApp.BusinessDBAName);
        ticket.DateCreated = DateTime.Now;
        ticket.DateModified = DateTime.Now;
        ticket.UserCreated = UserSessions.CurrentUser.UserName;
        ticket.UserModified = UserSessions.CurrentUser.UserName;
        ticket.Priority = "Low";
        ticket.TicketSource = "i"; // "i" is for internal
        ticket.UserCreatedUserUID = UserSessions.CurrentUser.UID;
        ticket.AttentionReq = true;
        ticket.OfficeID = UserSessions.CurrentMerchantApp.Office.GetHashCode();

        DataTicket.GetInstance().InsertTicket(ticket, TimeZoneID);

        if (ticket.TicketUID != "-1")
        {
            // good ticket, save it in the session.
            UserSessions.CurrentTicket = ticket;

            // send out apppriate emails!!
            PaymentXP.Facade.TicketNotification.NewTicketCreated(ticket.TicketUID, false, false);
            ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for Website Review Request: Ticket ID : {0} .", ticket.TicketUID);


            // insert into WSCompliance Table,
            Hashtable prms_ws = new Hashtable();
            prms_ws.Add("@TicketID", CommonUtility.Util.if_i(ticket.TicketID, 0));
            prms_ws.Add("@RequestType", (int)eWSComplianceRequestType.UserInitiated);
            int ws_compliance_id = DataWSCompliance.InsertWSCompliance(prms_ws);

            wucMessage2.AddMessageStatus("New website compliance ticket created");

            // the idea is that we want to 

            // find pending condition
            IList<UWConditions> liUWC = DataMerchantApp.GetInstance().GetUWConditionsList(UserSessions.CurrentMerchantApp.MerchantAppUID);
            foreach (UWConditions obj in liUWC)
            {
                if (obj.ConditionID == PaymentXP.BusinessObjects.Constants.COMPLIANCE_PENDING_ID && obj.ReceivedInfo == false)
                {
                    // set it to satisified.
                    obj.ARDate = DateTime.Now;
                    obj.ReceivedInfo = true;
                    obj.MerchantAppsUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                    obj.Comments = "The pending condition was cleared because a new Website Compliance Review was initiated";

                    DataConditions.UpdtateConditions(obj, UserSessions.CurrentUser.UserName);

                    break;
                }
            }

            //PXP-2955
            FormHandler.InsertMerchantNotesForTicket(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.UserName, ticket);

            //Response.Redirect("~/SecureMerchantManagementForms/frmCompliance.aspx?TicketID=" + CommonUtility.Util.if_i(ticket.TicketID, 0).ToString());
            Response.Redirect(string.Format("~/SecureMerchantManagementForms/frmCompliance.aspx?MerchantAppUID={0}&TicketUID={1}&WSComplianceID={2}"
                    , UserSessions.CurrentMerchantApp.MerchantAppUID
                    , ticket.TicketUID
                    , ws_compliance_id.ToString()
                    ));
        }
        else
        {
            wucMessage2.AddMessageError("ERROR: could not create website compliance ticket.");
            Response.Redirect(string.Format("~/SecureMerchantManagementForms/frmCompliance.aspx?MerchantAppUID={0}", UserSessions.CurrentMerchantApp.MerchantAppUID));
        }



    }

    protected void btnComplianceSubmit_Click(object sender, EventArgs e)
    {

        if (UserSessions.CurrentWSCompliance.StatusUID.ToUpper() == Constants.COMPLIANCE_TBD)
        {
            bool b = wucWSComplianceEdit1.FormSubmit();

            if (b == true)
            {
                wucMessage2.AddMessageStatus("Success. Website Submitted");
                Response.Redirect(string.Format("~/SecureMerchantManagementForms/frmCompliance.aspx?MerchantAppUID={0}", UserSessions.CurrentMerchantApp.MerchantAppUID));
            }
        }
        else
        {
            wucMessage2.AddMessageError("Website already saved");
            Response.Redirect(string.Format("~/SecureMerchantManagementForms/frmCompliance.aspx?MerchantAppUID={0}", UserSessions.CurrentMerchantApp.MerchantAppUID));
        }

        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void btnComplianceCancel_Click(object sender, EventArgs e)
    {
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected bool email_check()
    {
        return true;
    }

    // send email button
    protected void Button1_Click(object sender, EventArgs e)
    {
        // validate the form here.
        if (this.email_check())
        {

            string subject = tbSubject.Text.Trim();
            string to = tbTo.Text.Trim();
            string from = tbFrom.Text.Trim();
            string body = tbBody.Text.Trim();
            string cc = tbCC.Text.Trim();
            string bcc = tbBCC.Text.Trim();

            Hashtable htAttachments = new Hashtable();

            if (File.Exists(hidPDFLocation.Value))
            {
                // attach the compliance doc
                byte[] bytes = System.IO.File.ReadAllBytes(hidPDFLocation.Value);
                htAttachments.Add(hidPDFCleanName.Value, bytes);

                // attach all uploaded docs.
                Hashtable prms = new Hashtable();
                prms.Add("@PrimaryKeyID", UserSessions.CurrentTicket.TicketID);
                prms.Add("@MDocSourceID", (int)MDoc.eMDocSourceID.Tickets);
                List<MDoc> li = DataDocuments.GetInstance().GetMDocuments(prms);

                if (li != null)
                {
                    foreach (MDoc m in li)
                    {

                        ZeusWeb.MDocWS.FileUpload objFU = new ZeusWeb.MDocWS.FileUpload();
                        objFU.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];
                        ZeusWeb.MDocWS.MDoc mf = objFU.GetFile(m.DocID, m.MerchantID, 0, 0);
                        if (mf != null)
                        {
                            htAttachments.Add(mf.OrigName, mf.FileBinary);
                        }
                    }
                }

                // send off the email
                bool perform = CommonUtility.Email.SendEmail(subject, body, body, from, to, cc, bcc, htAttachments);

                if (perform)
                    ZeusWeb.Logging.EmailLog.InfoFormat("Successfully sent Compliance doc in email to: {0}", to);
                else
                    ZeusWeb.Logging.EmailLog.InfoFormat("Error while Sending Compliance doc in email to: {0}", to);

                // cleanup
                if (File.Exists(hidPDFLocation.Value))
                {
                    File.Delete(hidPDFLocation.Value);
                }

                WebDialogWindow3.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
            }

        }
    }

}
