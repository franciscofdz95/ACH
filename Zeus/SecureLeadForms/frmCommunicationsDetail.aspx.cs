using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Reflection;
using System.Net.Mail;

using Infragistics.WebUI.WebDataInput;
using Infragistics.WebUI.WebControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

public partial class frmCommunicationsDetail : frmBaseDataEntry
{
   


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Email Detail";

            //this.UIDName = "CommunicationID";
            //this.CurrentObject = null;
           
            FormHandler.SetSecurity(this.Page);
            this.Adding = Convert.ToBoolean(Request["Adding"].ToString());
            lstType.SelectedIndex = 0;

            this.DeleteSystemGenerateFiles();

            if (this.Adding)
            {
                this.FormNew();
                if (Request["DBAName"] != null)
                    DBAName.Text = Request["DBAName"].ToString();

                if (Request["LeadID"] != null)
                    LeadID.Text = Request["LeadID"].ToString();

                if (Request["Email"] != null)
                    To.Text = Request["Email"].ToString();

            }
            else
            {
                if (Request["CommunicationID"] != null)
                {
                    this.UID = Request["CommunicationID"].ToString();
                    //LookupTableHandler.LoadAttachments(lstAttachments, false, this.UID);
                    FormShow(this.UID);
                }
            }
        }
    }

    public override void FormShow(string ID)
    {
        DataCommunication data = DataAccess.DataCommunicationDao;
        Communication comm = data.GetCommunication(ID);
        FormBinding.BindObjectToControls(comm, pnlDetail);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);

        if (TimeSent.Text.Contains("1/1/0001"))
            TimeSent.Text = "";

        txtHTMLBody.Text = comm.HTMLBody;

        if (comm.IsEmail)
            lstType.SelectedIndex = 0;
        else
            lstType.SelectedIndex = 1;

        this.SetType();

        lblMessage.Text = string.Empty;
        lblError.Text = string.Empty;

        //this.CurrentObject = comm;
        UserSessions.CurrentCommunication = comm;

        if (comm != null && comm.Attachments != null)
        {
            //foreach (Attachments item in comm.Attachments)
            //{
            //    if (item.Checked)
            //    {
            //        ListItem lstItem = lstAttachments.Items.FindByValue(item.AttachmentID);
            //        lstItem.Selected = true;
            //    }
            //}
        }
    }

    public override void FormClear()
    {
        To.Text = string.Empty;
        Cc.Text = string.Empty;
        Bcc.Text = string.Empty;
        Subject.Text = string.Empty;
        txtHTMLBody.Text = string.Empty;
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
            return false;

        try
        {
            Communication comm;
            if (this.Adding)
            {
                comm = new Communication();
            }
            else
            {
                //comm = (Communication)this.CurrentObject;
                comm = UserSessions.CurrentCommunication;
            }

            FormBinding.BindControlsToObject(comm, pnlDetail);
            comm.LeadID = LeadID.Text;
            //UserSessions.CurrentLeadUID = LeadID.Text;
            comm.HTMLBody = txtHTMLBody.TextXhtml;
            comm.Body = txtHTMLBody.TextPlain;
            DataCommunication data = DataAccess.DataCommunicationDao;

            comm.IsEmail = (lstType.SelectedIndex == 0);

            User user = UserSessions.CurrentUser;

            if (!this.Adding)
            {   
                comm.UserUpdated = user.UserName;
                data.UpdateCommunication(comm);
            }
            else
            {
                comm.UserCreated = user.UserName; 
                data.InsertCommunication(comm);
                if (comm.CommunicationID != "-1")
                {
                    this.UID = comm.CommunicationID;
                    this.Adding = false;
                }

            }
            this.UpdateAttachments();

            this.EditMode = false;
            this.ToggleButtons();

            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    public override void ToggleButtons()
    {
        btnSend.Enabled = !btnSend.Enabled;
        btnEdit.Enabled = !btnEdit.Enabled;
        btnAdd.Enabled = !btnAdd.Enabled;
        btnSave.Enabled = !btnSave.Enabled;
        btnRefresh.Enabled = !btnRefresh.Enabled;
        btnCancel.Enabled = !btnCancel.Enabled;
        btnClose.Enabled = !btnClose.Enabled;


        MasterPageSales master = (MasterPageSales)this.Master;
        //master.Menu.Enabled = !master.Menu.Enabled;
    }

    public override bool FormDataCheck()
    {
        string message = string.Empty;

        if (lstType.SelectedIndex == 1)
            if (To.Text.Length != 11)
                message += "Fax number format is incorrect. Correct format example:(19491234566)";

        if (lstType.SelectedIndex == 1)
        {
            bool ItemSelected = false;

            foreach (ListItem item in lstAttachments.Items)
            {
                if (item.Selected)
                {
                    ItemSelected = true;
                    break;
                }

            }
            if (!ItemSelected)
                message += "Faxing requires an attachment. Please select an attachment.";
        }

        lblError.Text = message;

        if (message == string.Empty)
            return true;
        else
            return false;
    }

    public override void FormNew()
    {
        this.FormClear();
        this.Adding = true;
        this.EditMode = true;
        //LeadID.Text = UserSessions.CurrentLeadUID;
        //DBAName.Text = UserSessions.CurrentLeadDBA;
        ToggleButtons();
        FormHandler.SetControlEditMode(pnlDetail, EditMode);
    }

    public override bool FormDelete()
    {
        return false;
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
    }

    public bool UpdateAttachments()
    {
        try
        {
            DataCommunication data = DataAccess.DataCommunicationDao;
            foreach (ListItem item in lstAttachments.Items)
            {
                data.UpdateOutboxAttachment(this.UID, item.Value, item.Selected);
            }
            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    //protected void btnStart_Click(object sender, ImageClickEventArgs e)
    //{
    //    FormHandler.RecordNav(RecordNavigation.First, this, (DataView)UserSessions.SearchResultsDataView2, true);
    //}

    //protected void btnPrevious_Click(object sender, ImageClickEventArgs e)
    //{
    //    FormHandler.RecordNav(RecordNavigation.Previous, this, (DataView)UserSessions.SearchResultsDataView2, true);
    //}

    //protected void btnNext_Click(object sender, ImageClickEventArgs e)
    //{
    //    FormHandler.RecordNav(RecordNavigation.Next, this, (DataView)UserSessions.SearchResultsDataView2, true);
    //}

    //protected void btnEnd_Click(object sender, ImageClickEventArgs e)
    //{
    //    FormHandler.RecordNav(RecordNavigation.Last, this, (DataView)UserSessions.SearchResultsDataView2, true);
    //}

    protected void btnSave_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (this.FormSave())
        {
            this.FormShow(this.UID);
        }
    }

    protected void btnClose_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.CloseForm();
    }

    protected void btnDelete_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (this.FormDelete())
            Response.Redirect("frmLeads.aspx");
    }

    protected void btnRefresh_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormShow(this.UID);
    }

    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormNew();
    }

    protected void btnEdit_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.ToggleButtons();
    }

    protected void btnCancel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (this.UID.Equals(string.Empty))
        {
            // keep an eye on this, it might give you problems.
            Response.Redirect("frmCommunications.aspx");
        }
        else
            this.FormCancel();
    }

    private void CloseForm()
    {
        //this.ErrorDisplay.Text = FormHandler.LogFormChanged(this.CurrentObject, this.DetailPanel);
        lblError.Text = FormHandler.LogFormChanged(UserSessions.CurrentCommunication, pnlDetail);

        if (lblError.Text != string.Empty)
            return;

        string url;

        if (Request.QueryString["PostBackURL"] == null)
            url = string.Empty;
        else
            url = Request.QueryString["PostBackURL"].ToString();

        if (!url.Equals(string.Empty))
            Response.Redirect(url);
        else
            Response.Redirect("frmCommunications.aspx");
    }

    private void DeleteSystemGenerateFiles()
    {
        User user = UserSessions.CurrentUser;

        //Clean up system generated files
        string[] files;
        files = Directory.GetFiles(Server.MapPath("~/PDF/"));
        string FilePrefix = string.Empty;
        foreach (string file in files)
        {
            FileInfo fi = new FileInfo(file);
            int pos = fi.Name.IndexOf(" - ");

            if (pos > 0)
            {
                FilePrefix = fi.Name.Substring(0, fi.Name.IndexOf(" - "));
                if (FilePrefix == user.UserName)
                {
                    try
                    {
                        File.Delete(file.ToString());
                    }
                    //catch (Exception exc)
                    catch
                    {
                    }
                }
            }
        }
    }

    private bool SendEmail(string smtpServer, string subject, string[] recipients, string body, string[] cc, string[] bcc, string sender, string sender_displayName, ArrayList attachments)
    {
        bool result = true;

        try
        {
            MailMessage message = new MailMessage();
            message.Priority = MailPriority.Normal;
            message.IsBodyHtml = true;

            // Recipients
            foreach (string recipient in recipients)
                message.To.Add(recipient);

            // Body
            message.Body = body;

            // Subject
            message.Subject = subject;

            // From
            if (sender == null || sender == string.Empty)
                message.From = new MailAddress("support@nationalmerchant.com", "Merchant Support");
            else
            {
                message.From = new MailAddress(sender);
            }

            // Carbon copy
            if (cc != null)
                foreach (string carboncopy in cc)
                    message.CC.Add(carboncopy);

            // Blind copy
            if (bcc != null)
                foreach (string blindcopy in bcc)
                    message.Bcc.Add(blindcopy);

            // Attachments
            if (attachments != null)
                foreach (object attachment in attachments)
                    message.Attachments.Add(new Attachment(attachment.ToString()));

            //SmtpClient client = new SmtpClient("192.168.1.6");
            SmtpClient client = new SmtpClient(smtpServer);
            client.UseDefaultCredentials = true;
            client.Send(message);

            client = null;
            message = null;
        }
        //catch (Exception ex)
        catch
        {
            result = false;
            //log.WriteLog(ex.Message + "\r\n" + ex.StackTrace);
        }
        return result;
    }

    private void SetType()
    {

    }

    protected void lstType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.SetType();
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;
        string url = string.Empty;

        switch (btn.Text)
        {
            case "Add":
                this.FormNew();
                break;

            case "Save":
                if (this.FormSave())
                {
                    url = "~/SecureLeadForms/frmCommunicationsDetail.aspx?Adding=false";
                    url += "&CommunicationUID=" + this.UID;
                    url += "&PostBackURL=" + Request.QueryString["PostBackURL"].ToString();
                    Response.Redirect(url);
                }
                break;

            case "Refresh":
                this.FormShow(this.UID);
                break;

            case "Cancel":
                if (this.UID.Equals(string.Empty))
                {
                    Response.Redirect("frmCommunications.aspx?");
                }
                else
                    this.FormCancel();
                break;

            case "Close":
                this.CloseForm();
                break;

            case "Create ACH Profile":
                //MerchantApp app = (MerchantApp)this.CurrentObject;
                MerchantApp app = UserSessions.CurrentMerchantApp;
                Response.Redirect("~/SecureACHForms/MerchantACH.aspx?Adding=true&MerchantID=" + app.ID);
                break;

            case "View ACH Profile":
                //MerchantApp app1 = (MerchantApp)this.CurrentObject;
                MerchantApp app1 = UserSessions.CurrentMerchantApp;

                url = "MerchantACH.aspx?";
                url += "MerchantAppUID=" + this.UID;
                url += "&MerchantID=" + app1.ID;

                Response.Redirect("~/SecureACHForms/" + url);
                break;

            case "Delete":
                if (this.FormDelete())
                    Response.Redirect("frmLeads.aspx");
                break;

            case "Upload Document":
                Response.Redirect("~/DocumentUpload/frmDocumentUpload.aspx?MerchantAppUID=" + this.UID);
                break;

            case "PDF":
                break;

            case "Edit":
                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();
                break;

            case "Send":
                DataCommunication data = DataAccess.DataCommunicationDao;
                data.UpdateCommunicationTimeSent(this.UID, DateTime.Now);
                this.FormShow(this.UID);
                break;

            //case "<<":
            //    FormHandler.RecordNav(RecordNavigation.First, this, UserSessions.SearchResultsDataView, false);
            //    break;

            //case "<":
            //    FormHandler.RecordNav(RecordNavigation.Previous, this, UserSessions.SearchResultsDataView, false);
            //    break;

            //case ">":
            //    FormHandler.RecordNav(RecordNavigation.Next, this, UserSessions.SearchResultsDataView, false);
            //    break;

            //case ">>":
            //    FormHandler.RecordNav(RecordNavigation.Last, this, UserSessions.SearchResultsDataView, false);
            //    break;
        }
    }
}
