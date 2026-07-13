using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;


public partial class wucLeadInfo : System.Web.UI.UserControl
{
    public Control_wucAgentSelector AgentSelector
    {
        get
        {
            return this.wucAgentSelector;
        }
    }

    public string BusinessDBA
    {
        get
        {
            return this.DBAName.Text;
        }
    }

    public string MerchantUID
    {
        get
        {
            return this.MerchantAppUID.Text;
        }
    }

    private int CurrentPage
    {
        get
        {
            if (ViewState["CurrentPage"] == null)
                return 1;
            else
                return Convert.ToInt32(ViewState["CurrentPage"]);
        }
        set 
        { 
            ViewState["CurrentPage"] = value; 
        }
    }

    private int PageSize
    {
        get
        {
            if (ViewState["PageSize"] == null)
                return 10;
            else
                return Convert.ToInt32(ViewState["PageSize"]);
        }
        set 
        { 
            ViewState["PageSize"] = value; 
        }
    }

    private Hashtable m_Prms
    {
        get
        {
            if (ViewState["m_Prms"] == null)
                return null;
            else
                return (Hashtable)ViewState["m_Prms"];
        }
        set 
        { 
            ViewState["m_Prms"] = value; 
        }
    }

    private string SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
                return "";

            return ViewState["SortOrder"].ToString();
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }

    private SortDirection SortDirectionSearch
    {
        get
        {
            if (ViewState["SortDirectionSearch"] == null)
                return SortDirection.Ascending;

            return (SortDirection)ViewState["SortDirectionSearch"];
        }
        set
        {
            ViewState["SortDirectionSearch"] = value;
        }
    }

    private bool Assigned
    {
        get
        {
            if (ViewState["Assigned"] == null)
                return false;

            return (bool)ViewState["Assigned"];
        }
        set
        {
            ViewState["Assigned"] = value;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadLeadStatus(StatusID, false);
            LookupTableHandler.LoadLeadSources(SourceID, false);
            LookupTableHandler.LoadLeadFollowupStatus(FollowupStatusID, false);
            LookupTableHandler.LoadLeadTypes(LeadTypeUID, false);
            LookupTableHandler.LoadLeadReps(AssignedUserID, false);
            LookupTableHandler.LoadCountries(Country);
            LookupTableHandler.LoadLeadOrigin(OriginID, false);
            LookupTableHandler.LoadCurrencyCodes(Currency, false);
            LookupTableHandler.LoadLeadClosureCodes(ClosureCode, false);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void FormNew(bool editMode)
    {
        User user = UserSessions.CurrentUser;

        wucAgentSelector.m_AgentUID = user.AgentUID;
        wucAgentSelector.m_AgentID = user.AgentID.ToString();
        wucAgentSelector.m_AgentDBA = user.AgentDBA;

        ListHandler.ListFindItem(SourceID, "27d3ad71-b6e4-47c8-9117-a5f6df118e20");

        Country.Text = "US";
        this.Province.Attributes.Add("Style", "display:none");
        this.State.Attributes.Add("Style", "display:inline");

        this.Province.Attributes.Add("Style", "display:none");
        this.State.Attributes.Add("Style", "display:inline");

        FormHandler.SetControlEditMode(pnlLeadInfo, editMode);

        wucAgentSelector.btnSelect.Enabled = editMode;

        if (user.IsAgent)
            UserCreated.Text = user.AgentDBA + " (" + user.AgentID + ")";
        else
            UserCreated.Text = user.UserName;
        DateCreated.Text = DateTime.Now.ToString();

        LeadID.ReadOnly = true;
        this.Assigned = false;
    }

    public void FormShow(Lead lead, bool editMode)
    {
        FormBinding.BindObjectToControls(lead, pnlLeadInfo);
        FormHandler.SetControlEditMode(pnlLeadInfo, editMode);

        wucAgentSelector.btnSelect.Enabled = editMode;

        User user = UserSessions.CurrentUser;
        if (user.IsAgent && lead.UserCreated.ToUpper() == user.UserName.ToUpper())
            UserCreated.Text = UserSessions.CurrentUser.AgentDBA + " (" + UserSessions.CurrentUser.AgentID + ")";

        LeadID.ReadOnly = true;

        //PhoneNumber.Value = CommonUtility.Util.GetNumbersFromString(lead.PhoneNumber);
        FaxNumber.Value = CommonUtility.Util.GetNumbersFromString(lead.FaxNumber);

        hypZID.Text = lead.CountofZIDs.ToString();
        hypZID.Enabled = (lead.CountofZIDs > 0);

        DateCreated.Text = WebUtil.ConvertToUserDateTimeSettings(lead.DateCreated.ToString());

        this.Assigned = lead.AssignedUserID > 0;

        if (lead.Country != null)
        {
            if (lead.Country.Trim().ToUpper().Equals("US") || lead.Country.Trim().ToUpper().Equals(""))
            {
                this.Province.Attributes.Add("Style", "display:none");
                this.State.Attributes.Add("Style", "display:inline");
            }
            else
            {
                this.Province.Attributes.Add("Style", "display:inline");
                this.State.Attributes.Add("Style", "display:none");
                Province.Text = lead.State == "-1" ? string.Empty : lead.State;
            }
        }

        FormHandler.SetControlEditMode(pnlRecords, true);
    }

    public void FormClear()
    {
        FormHandler.ClearAllControls(pnlLeadInfo);
    }

    public string FormDataCheck()
    {
        string msg = string.Empty;

        if (this.StatusID.SelectedValue == Constants.LEADSTATUS_ASSIGNED.ToLower() && this.AssignedUserID.SelectedIndex == 0)
        {
            msg += "Please assign a user to lead";
        }

        //If the Status is not workable the user has to select the closure code.
        if (this.StatusID.SelectedValue.ToUpper().Equals(Constants.LEADSTATUS_NOTINTERESTED.ToUpper()) && this.ClosureCode.SelectedValue.Equals("-1"))
        {
            msg += "\n Please select a Closure Code";
        }

        return msg;
    }

    protected void hypZID_Click(object sender, EventArgs e)
    {
        Search(true);
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.Search(false);
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.Search(false);
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grd.PageSize = this.PageSize;
        this.Search(false);
    }

    protected void odsMerchants_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        if (!string.IsNullOrWhiteSpace(this.LeadID.Text))
            prms.Add("@LeadsID", this.LeadID.Text);

        if (prms.Count > 0)
        {

            prms.Add("@PageSize", this.PageSize);

            prms.Add("@CurrentPage", this.CurrentPage);
            grd.PageSize = this.PageSize;
            grd.PageIndex = this.CurrentPage - 1;

            if (this.SortOrder == string.Empty)
                prms["@SortOrder"] = "ID";
            else
                prms["@SortOrder"] = this.SortOrder;

            prms["@SortDirection"] = SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch);

            e.InputParameters[0] = prms;
            e.InputParameters[3] = this.grd.ID;
            lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetMerchantAppsPagingRowCount(prms, 0, 0, this.grd.ID).ToString();
        }
        else
        {
            // we do this so that we don't call the SP with zero paramters. it's very slow otherwise.

            //prms.Add("@ID", -1);
            e.Cancel = true;
        }

    }

    private void Search(bool IsOnLoad)
    {
        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        grd.DataBind();

    }

    protected void AssignedUserID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!this.Assigned)
        {
            this.StatusID.SelectedValue = Constants.LEADSTATUS_ASSIGNED.ToLower();
            this.Assigned = true;
        }
    }
}
