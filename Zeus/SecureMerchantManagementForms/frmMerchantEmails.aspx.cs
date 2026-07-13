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
using Infragistics.WebUI.WebDataInput;
using Infragistics.WebUI.WebHtmlEditor;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;

public partial class frmMerchantEmails : frmBaseDataEntry
{

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;
    }

    private int MerchantID
    {
        get
        {
            if (ViewState["MerchantID"] == null)
                return -1;
            else
                return Convert.ToInt32(ViewState["MerchantID"]);
        }
        set { ViewState["MerchantID"] = Convert.ToInt32(value); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        WucBusinessInfo1.pnlInfo.Enabled = false;

        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Emails);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Emails");
            }

            //btnEmail.Attributes.Add("onclick", "return ShowEmail()");
            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

            int merchantId;
            int.TryParse(UserSessions.CurrentMerchantApp.ID, out merchantId);
            this.MerchantID = merchantId;

            this.FormShow(this.UID);
            this.LoadEmails();
        }
    }



    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();  
        MerchantApp agreement = facade.GetMerchantAppZeus(ID);

        FormBinding.BindObjectToControls(agreement, pnlDetail);

        WucEmail1.AgentEmail = agreement.AgentEmail;
        WucEmail1.MerchantEmail = agreement.BusinessEmailAddress;

        //check to see if the account is ACH only and get the ach status in case if it is or else the cc status
        string m_StatusUID = agreement.StatusUID.ToUpper();

        if (WucBusinessInfo1.isACHonly && UserSessions.ActiveAchMerchant != null)
        {
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
        }

        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);
       
      
        WucBusinessInfo1.LoadOffice(agreement);
       
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
        return true;
    }

    public override void FormCancel()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void ToggleButtons()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.LoadEmails();
    }

    protected void grdEmail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                break;
            case DataControlRowType.DataRow:

                e.Row.Cells[2].Attributes.Add("title", Server.HtmlDecode(e.Row.Cells[2].Text));
                e.Row.Cells[2].Text = "<div style='width:270px;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;'><NOBR>" + Server.HtmlDecode(e.Row.Cells[2].Text) + "</NOBR></div>";


                e.Row.Cells[7].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[7].Text);

                break;

            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
    }

    //This method finds a string between the mentioned substrings.
    private string Between(string src, string findfrom, string findto)
    {
        int start = src.IndexOf(findfrom);
        int to = src.IndexOf(findto, start + findfrom.Length);
        if (start < 0 || to < 0) return "";
        string s = src.Substring(
                       start,
                       to + findfrom.Length);
        return s;
    }

    
    protected void grdEmail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataCommunication data = DataAccess.DataCommunicationDao;
        GridViewRow grdRow = null;
        if (e.CommandSource is LinkButton)
        {
            switch (e.CommandName)
            {
                case "View":
                    grdRow = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer);
                    Communication comm = data.GetCommunication(grdEmail.DataKeys[grdRow.RowIndex].Values["CommunicationID"].ToString());
                    FormBinding.BindObjectToControls(comm, WucEmail1);

                    WebHtmlEditor editor = (WebHtmlEditor)WucEmail1.FindControl("txtHTMLBody");
                    string temp = Between(comm.HTMLBody,"<style","style>");

                    editor.Text = string.IsNullOrEmpty(temp) == true ? comm.HTMLBody : comm.HTMLBody.Replace(temp, " ");
                    Label lb1 = (Label)WucEmail1.FindControl("lblMessage");
                    lb1.Text = "";

                    Label lbl1 = (Label)WucEmail1.FindControl("lblError");
                    lbl1.Text = "";
                    WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                    break;
            }
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        LoadEmails();
    }

    private int ConvertSortDirectionToSql(SortDirection direction)
    {
        int newSortDirection;

        switch (direction)
        {
            case SortDirection.Descending:
                newSortDirection = 1;
                this.SortDirectionSearch = SortDirection.Descending;
                break;

            default:
                newSortDirection = 0;
                this.SortDirectionSearch = SortDirection.Ascending;
                break;
        }
        return newSortDirection;
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        LoadEmails();
    }

    protected void odsEmails_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        DataCommunication data = DataAccess.DataCommunicationDao;
        Hashtable prms = new Hashtable();

        prms.Add("@MerchantAppUID", this.UID);
        prms.Add("@MerchantID", this.MerchantID);
        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);
        grdEmail.PageSize = this.PageSize;
        grdEmail.PageIndex = this.CurrentPage - 1;

        if (this.SortOrder == string.Empty)
            prms["@SortOrder"] = "OutboxID";
        else
            prms["@SortOrder"] = this.SortOrder;

        prms["@SortDirection"] = SortDirection.Descending; //this.ConvertSortDirectionToSql(this.SortDirectionSearch);


        e.InputParameters[0] = prms;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetCommunicationsPagingRowCount(prms, 0, 0, this.grdEmail.ID).ToString();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        LoadEmails();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (rdExport.SelectedValue.Equals("1"))
        {
            this.PageSize = 5000;
            this.CurrentPage = 1;
        }
        else
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = grdEmail.PageIndex + 1;
        }

        LoadEmails();
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        FormHandler.Export2Excel("MerchantEmails.xls", grdEmail);
    }


    private void LoadEmails()
    {
        if (this.MerchantID == 0)
            return;

        grdEmail.DataBind();
        lblEmail.Visible = (grdEmail.Rows.Count == 0);
        pnlRecords.Visible = (grdEmail.Rows.Count > 0);
    }
}
