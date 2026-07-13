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
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using System.Linq;


public partial class wucAlertsnCategories : System.Web.UI.UserControl
{
    
    public bool EditMode
    {
        get
        {
            if (ViewState["EditMode"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["EditMode"]);
        }
        set { ViewState["EditMode"] = value; }

    }

    public eControlContactType UserAlertType
    {
        get
        {
            return (eControlContactType)ViewState["UserAlertType"];
        }
        private set
        {
            ViewState["UserAlertType"] = value;
        }
    }

    private SortDirection CurrentSort
    {
        get
        {
            if (ViewState["sortDir"] == null)
            {
                return SortDirection.Ascending;
            }
            return (SortDirection)ViewState["sortDir"];
        }
        set { ViewState["sortDir"] = value; }
    }

    private string CurrentExp
    {
        get
        {
            if (ViewState["sortExp"] == null)
            {
                return "CategoryID";
            }
            return ViewState["sortExp"].ToString();
        }
        set { ViewState["sortExp"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.pnlAlertContacts.UpdateContacts += new wucAlertContacts.UpdateContactAlerts(pnlAlertContacts_UpdateContacts);
      
    }

    protected void grdCat_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                DataSet ds1 = (DataSet)grdCat.DataSource;
                DataRowView dr = (DataRowView)e.Row.DataItem;
                DataView dv = dr.CreateChildView(ds1.Tables[1].ParentRelations["header"]);
                dv.RowFilter = "Name <> ' '";
                ((GridView)e.Row.FindControl("gvAlerts")).DataSource = dv;
                ((GridView)e.Row.FindControl("gvAlerts")).DataBind();
                ((HtmlImage)e.Row.FindControl("img1")).Attributes.Add("onclick", "CollapseExpand('" + ((HtmlGenericControl)e.Row.FindControl("div1")).ClientID + "',null,'" + ((HtmlImage)e.Row.FindControl("img1")).ClientID + "')");

                break;
            default:
                break;
        }
    }

    protected void gvAlerts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                ((CheckBox)e.Row.FindControl("Ischecked")).Checked = CommonUtility.Util.if_b(DataBinder.Eval(e.Row.DataItem, "Checked"), false);
                ((CheckBox)e.Row.FindControl("Ischecked")).Enabled = EditMode;

                LinkButton lnkEmailTo = (LinkButton)e.Row.FindControl("lnkEmailTo");
                lnkEmailTo.CommandArgument = string.Format("{0},{1}", DataBinder.Eval(e.Row.DataItem, "AlertID"), this.UserAlertType == eControlContactType.Agent ? UserSessions.CurrentAgent.AgentID.ToString() : UserSessions.CurrentMerchantApp.ID);
                lnkEmailTo.Enabled = this.EditMode;

                e.Row.Cells[3].Style.Add("border-right-width", "0px");
                e.Row.Cells[4].Style.Add("border-left-width", "0px");
                break;

            case DataControlRowType.Header:
                e.Row.Cells[3].Style.Add("border-right-width", "0px");
                e.Row.Cells[4].Style.Add("border-left-width", "0px");
                break;

            default:
                break;
        }
    }

    protected void gvAlerts_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;

        if (e.CommandSource is LinkButton)
            lnk = (LinkButton)e.CommandSource;
        else
            return;

        string[] str = e.CommandArgument.ToString().Split(new char[] { ',' });

        string alertId = str[0];
        string Id = str[1];

        GridViewRow row = (GridViewRow)(lnk.NamingContainer);
        string alertContacts = ((HiddenField)row.FindControl("hdnUpdateContacts")).Value;

        pnlAlertContacts.SetDataSource(this.UserAlertType, CommonUtility.Util.if_i(Id, 0), CommonUtility.Util.if_i(alertId, 0), 4, 10, alertContacts);

        this.dlgContacts.Header.CaptionText = row.Cells[0].Text + " Email Contact Notifications";
        this.dlgContacts.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;

        return;
    }

    private void LoadLanguageOptions(eControlContactType userType)
    {
        int languageID = userType == eControlContactType.Agent? UserSessions.CurrentAgent.LanguageID: UserSessions.CurrentMerchantApp.LanguageID;

        this.AlertLanguage.SelectedValue = languageID.ToString();

        this.AlertLanguage.Enabled = EditMode;
    }

    public void LoadGrid(eControlContactType userType)
    {
        LoadLanguageOptions(userType);

        Hashtable prms = new Hashtable();
        this.UserAlertType = userType;

        if (userType == eControlContactType.Agent)
        {
            // TOL: uncomment this when ready. commenting out just for now
            //this.pnlAlertContacts.AlertUserId = UserSessions.CurrentAgent.AgentID;
            prms.Add("@AgentID", UserSessions.CurrentAgent.AgentID);
            prms.Add("@PortalID", Constants.AGENT_PORTAL);

            //fixes bug with agent alerts where user clicks on edit enables/disables alert
            //and clicks save. the bug was that the enable/disable was not being saved for the
            //agent alert
            this.pnlAlertContacts.AlertUserId = UserSessions.CurrentAgent.AgentID;

            DataSet ds = DataAccess.DataAgentDao.GetAgentAlerts(prms);
            grdCat.DataSource = ds;
            ds.Relations.Add("header", ds.Tables[0].Columns["CategoryID"], ds.Tables[1].Columns["CategoryID"]);
            grdCat.DataBind();
        }
        else if (userType == eControlContactType.Merchant)
        {
            // TOL: uncomment this when ready. commenting out just for now
            //this.pnlAlertContacts.AlertUserId = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
            prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
            prms.Add("@PortalIDList", string.Format("{0},{1},{2}", Constants.MERCHANT_PORTAL, Constants.ZEUS_PORTAL, Constants.PAYMENTXP_PORTAL));

            //fixes bug with merchant alerts where user clicks on edit enables/disables alert
            //and clicks save. the bug was that the enable/disable was not being saved for the
            //merchant alert
            this.pnlAlertContacts.AlertUserId = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);

            DataSet ds = DataAccess.DataAgentDao.GetMerchantAlerts(prms);
            grdCat.DataSource = ds;
            ds.Relations.Add("header", ds.Tables[0].Columns["CategoryID"], ds.Tables[1].Columns["CategoryID"]);
            grdCat.DataBind();
        }

        if (grdCat.Rows.Count == 0)
        {
            pnl1.Visible = false;
            noData.Visible = true;
            pnlDetail.Visible = false;
        }
        else
        {
            noData.Visible = false;
            pnlDetail.Visible = true;
            pnl1.Visible = true;
        }
    }

    public bool FormSave(eControlContactType userType)
    {
        if (userType == eControlContactType.Agent)
            return this.UpdateAgentAlerts();
        else if (userType == eControlContactType.Merchant)

            return this.UpdateMerchantAlerts();

        return false;
    }

    private bool UpdateAgentAlerts()
    {
        DataAccess.DataMeritusAlertsDao.UpdateLanguageSelection(this.pnlAlertContacts.AlertUserId, Convert.ToInt32(this.AlertLanguage.SelectedValue), (int)eControlContactType.Agent);
        UserSessions.CurrentAgent.LanguageID = Convert.ToInt32(this.AlertLanguage.SelectedValue);

        foreach (GridViewRow row in grdCat.Rows)
        {
            GridView grd = (GridView)row.FindControl("gvAlerts");

            foreach (GridViewRow grdRow in grd.Rows)
            {
                int alertId = CommonUtility.Util.if_i(grd.DataKeys[grdRow.RowIndex].Values["AlertID"].ToString(), 0);

                if (alertId > 0)
                    UpdateAgentAlertConfiguration(grdRow, alertId);
            }
        }

        return true;
    }

    private void UpdateAgentAlertConfiguration(GridViewRow grdRow, int alertId)
    {
        string updateIds = ((HiddenField)grdRow.FindControl("hdnUpdateContacts")).Value.Trim().Replace(";", ",");

        bool enabled = ((CheckBox)grdRow.FindControl("Ischecked")).Checked;

        DataAccess.DataAgentDao.UpdateAgentContactAlerts(alertId, this.pnlAlertContacts.AlertUserId, updateIds, enabled);
        }

    private bool UpdateMerchantAlerts()
    {
        DataAccess.DataMeritusAlertsDao.UpdateLanguageSelection(this.pnlAlertContacts.AlertUserId, Convert.ToInt32(this.AlertLanguage.SelectedValue), (int)eControlContactType.Merchant);
        UserSessions.CurrentMerchantApp.LanguageID = Convert.ToInt32(this.AlertLanguage.SelectedValue);

        foreach (GridViewRow row in grdCat.Rows)
        {
            GridView grd = (GridView)row.FindControl("gvAlerts");

            foreach (GridViewRow grdRow in grd.Rows)
            {
                int alertId = CommonUtility.Util.if_i(grd.DataKeys[grdRow.RowIndex].Values["AlertID"].ToString(), 0);

                if (alertId > 0)
                    UpdateMerchantAlertConfiguration(grdRow, alertId);
            }
        }

        return true;
    }

    private void UpdateMerchantAlertConfiguration(GridViewRow grdRow, int alertId)
    {
        string updateIds = ((HiddenField)grdRow.FindControl("hdnUpdateContacts")).Value.Trim().Replace(";", ",");

        bool enabled = ((CheckBox)grdRow.FindControl("Ischecked")).Checked;

        DataAccess.DataMerchantAppDao.UpdateMerchantContactAlerts(alertId, this.pnlAlertContacts.AlertUserId, updateIds, enabled);

    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        LoadGrid(this.UserAlertType);

        DataTable dataTable = ((DataSet)grdCat.DataSource).Tables[0];
        DataTable dt2 = ((DataSet)grdCat.DataSource).Tables[1];

        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            DataView dv2 = new DataView(dt2);

            if (CurrentExp != e.SortExpression)
                CurrentSort = SortDirection.Ascending;

            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
            DataSet ds = new DataSet();
            ds.Tables.Add(dataView.ToTable());
            ds.Tables.Add(dv2.ToTable());
            ds.Relations.Add("header", ds.Tables[0].Columns["CategoryID"], ds.Tables[1].Columns["CategoryID"]);
            grdCat.DataSource = ds;
            grdCat.DataBind();
            CurrentExp = e.SortExpression;
        }
    }

    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;

        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "ASC";
                CurrentSort = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                newSortDirection = "DESC";
                CurrentSort = SortDirection.Ascending;
                break;
        }

        return newSortDirection;
    }

    protected void pnlAlertContacts_UpdateContacts(int alertId, List<int> currentList, List<string> newList, string email, bool applyAll)
    {
        dlgContacts.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;

        foreach (GridViewRow row in grdCat.Rows)
        {
            GridView grd = (GridView)row.FindControl("gvAlerts");

            foreach (GridViewRow grdRow in grd.Rows)
            {
                if (alertId == CommonUtility.Util.if_i(grd.DataKeys[grdRow.RowIndex].Values["AlertID"].ToString(), 0) || applyAll)
                {
                    Label emails = (Label)grdRow.FindControl("lblEmailTo");
                    emails.Text = email;

                    HiddenField contactEmailIds = (HiddenField)grdRow.FindControl("hdnUpdateContacts");
                    contactEmailIds.Value = string.Join(";", newList.ToArray());

                    if(!applyAll)
                        return;
                }
            }
        }
    }

    public bool ValidateConfiguration()
    {
        foreach (GridViewRow row in grdCat.Rows)
        {
            GridView grd = (GridView)row.FindControl("gvAlerts");

            foreach (GridViewRow grdRow in grd.Rows)
            {
                List<string> updateIds = new List<string>(((HiddenField)grdRow.FindControl("hdnUpdateContacts")).Value.Trim().Split(';'));
                bool enabled = ((CheckBox)grdRow.FindControl("Ischecked")).Checked;

                //if merchant alert is enabled and the first contact email id is not a number or equals 0 then
                //we have an invalid merchant alert configuration
                if (enabled && CommonUtility.Util.if_i(updateIds[0], 0) == 0)
                {
                    this.lblError.Text = string.Format("Please assign a contact email address for {0} Alert", grdRow.Cells[0].Text);
                    return false;
                }
            }
        }

        return true;
    }
}
