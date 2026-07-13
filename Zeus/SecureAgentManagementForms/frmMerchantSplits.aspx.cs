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
using System.Drawing;
using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Linq;

public partial class frmMerchantSplits : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkMerchantSplits")).CssClass = "active";


        Page.Master.Page.Form.DefaultButton = btnSearch.UniqueID;

        if (!this.IsPostBack)
        {
            //DM-3882 ALamadrid - Get page sizes to display in combobox
            foreach (var data in System.Enum.GetNames(typeof(CommonUtility.PageEnum.PageSizeEnum)))
            {
                cboPageSize.Items.Add(((int)System.Enum.Parse(typeof(CommonUtility.PageEnum.PageSizeEnum), data)).ToString());
            }

            //this.Search(true);
            ShowFirstRecord();

        }
    }

    //public void Search(bool IsOnLoad)
    //{
    //    return;
    //    Hashtable prms = new Hashtable();
    //    prms.Add("@AgentUID", UserSessions.CurrentAgent.AgentUID);
    //    DataMerchantApp data = DataAccess.DataMerchantAppDao;
    //    DataSet ds = null;

    //    ds = data.GetMerchantApps(prms);
    //    DataView dv = ds.Tables[0].DefaultView;
    //    grd.DataSource = dv;
    //    grd.DataBind();

    //    //Select the fist row if row count is > 0
    //    if (grd.Rows.Count > 0)
    //    {
    //        grd.SelectedIndex = 0;
    //        this.UID = grd.DataKeys[0].Values["MerchantAppUID"].ToString();
    //        lblDBA.Text = grd.Rows[0].Cells[2].Text;
    //        lblID.Text = ((LinkButton)grd.Rows[0].FindControl("lbtnMerchantID")).Text;
    //        this.FormShow(this.UID);
    //    }
    //    else
    //        btnEdit.Visible = false;

    //    lblRecords.Visible = (grd.Rows.Count == 0);
    //}


    public override void FormShow(string ID)
    {
        //PXP-8404: Code changes: Start
        if (!String.IsNullOrWhiteSpace(ID))
        {
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID", ID);
            DataSet ds = DataAccess.DataAgentDao.GetAgentContracts(prms);
            grdContracts.DataSource = ds;
            grdContracts.DataBind();
        }
        //PXP-8404: Code changes: End
        lblError.Text = string.Empty;
        foreach (GridViewRow grdRow in grdContracts.Rows)
        {
            ((TextBox)grdRow.Cells[2].FindControl("txtRate")).Enabled = btnSave.Enabled;
        }
        if (grdContracts.Rows.Count == 0)
        {
            btnEdit.Visible = false;
        }
        else
            btnEdit.Visible = true;         //PXP-18974 Bug Fix Change
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
            return false;
        try
        {
            this.UpdateAgentContract();
            this.FormShow(this.UID);
            this.EditMode = false;
            this.ToggleButtons();
            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
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
        string message = string.Empty;

        if (grdContracts.Rows.Count > 0)
        {
            decimal total = 0;
            foreach (GridViewRow row in grdContracts.Rows)
            {
                if (((TextBox)row.Cells[2].FindControl("txtRate")).Text == string.Empty)
                    ((TextBox)row.Cells[2].FindControl("txtRate")).Text = "0.0";
                total += Convert.ToDecimal(((TextBox)row.Cells[2].FindControl("txtRate")).Text.Replace('%', ' ').Trim());
            }

            if (total != 100)
                message += " Payout percent must equal 100%. ";
        }
        if (message == string.Empty)
            return true;
        else
        {
            lblError.Text = message;
            return false;
        }
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !btnEdit.Enabled;
        btnSave.Enabled = !btnSave.Enabled;
        btnCancel.Enabled = !btnCancel.Enabled;

        //MasterPageAgent master = (MasterPageAgent)this.Master;
        //master.Menu.Enabled = !master.Menu.Enabled;

        foreach (GridViewRow grdRow in grdContracts.Rows)
        {
            ((TextBox)grdRow.Cells[2].FindControl("txtRate")).Enabled = btnSave.Enabled;
        }

        pnlGrid.Enabled = !btnSave.Enabled;
    }

    public bool UpdateAgentContract()
    {
        try
        {
            //get de old values
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID", this.UID);
            DataSet ds = DataAccess.DataAgentDao.GetAgentContracts(prms);
            var dataRows = ds.Tables[0].AsEnumerable();

            DataAgent data = DataAccess.DataAgentDao;
            AgentPayout payout = null;
            AgentPayout clone = null;

            foreach (GridViewRow row in grdContracts.Rows)
            {
                payout = new AgentPayout();
                payout.AgentID = UserSessions.CurrentAgent.AgentID;
                payout.ZID = Convert.ToInt32(lblID.Text);
                payout.DbaID = Convert.ToInt32(row.Cells[0].Text);
                payout.UID = grdContracts.DataKeys[row.RowIndex].Values["UID"].ToString();
                payout.ID = Convert.ToInt32(row.Cells[0].Text);
                payout.Rate = DataLayer.Decimal2Field(((TextBox)row.Cells[2].FindControl("txtRate")).Text.Replace('%', ' ').Trim());

                User user = UserSessions.CurrentUser;
                payout.UserUpdated = user.UserName;

                data.UpdateAgentContract(payout);

                //DM-1240 added for log
                var mRow = dataRows
                        .Where(p => p["UID"].ToString() == payout.UID)
                        .First();

                clone = new AgentPayout();
                clone.UID = mRow["UID"].ToString();
                clone.AgentID = UserSessions.CurrentAgent.AgentID;
                clone.ZID = Convert.ToInt32(lblID.Text);
                clone.ID = Convert.ToInt32(mRow["ID"]);
                clone.Dba = mRow["DBA"].ToString();
                clone.Rate = DataLayer.Decimal2Field(mRow["Rate"]);
                //payout.UserUpdated = user.UserName;

                FormHandler.LogFormChanges(UserSessions.CurrentAgent.AgentDBA, payout.UID, Convert.ToInt32(payout.ID), clone, payout, "frmMerchantSplits");
            }
            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
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
                this.FormSave();
                break;

            case "Refresh":
                this.FormShow(this.UID);
                break;

            case "Cancel":
                this.FormCancel();
                break;

            case "Close":
                break;

            case "Edit":
                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow grdRow = null;

        if (e.CommandSource is LinkButton)
        {
            if (grd != null)
            {
                grdRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                this.UID = grd.DataKeys[grdRow.RowIndex].Values["MerchantAppUID"].ToString();
                grd.SelectedIndex = grdRow.RowIndex;
            }
        }
        else
            return;

        switch (e.CommandName)
        {
            case "Select":
                this.FormShow(this.UID);
                lblDBA.Text = grdRow.Cells[2].Text;
                lblID.Text = ((LinkButton)grdRow.FindControl("lbtnMerchantID")).Text;
                grd.SelectedIndex = grdRow.RowIndex;
                break;
        }
    }


    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        grd.DataBind();


        ShowFirstRecord();

    }

    private void ShowFirstRecord()
    {
        if (grd.Rows.Count > 0)
        {
            // every time the page pages for the grid, we select the first customer on the grid.
            grd.SelectedIndex = 0;

            this.UID = grd.DataKeys[0].Values["MerchantAppUID"].ToString();
            this.FormShow(this.UID);


            lblDBA.Text = grd.SelectedRow.Cells[2].Text;
            //DM-1240 fixed to show the first ZID when loading
            lblID.Text = ((LinkButton)grd.SelectedRow.FindControl("lbtnMerchantID")).Text;

        }
        else
        {
            lblDBA.Text = "N/A";
            lblID.Text = "N/A";
            grdContracts.Visible = false;
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;

        ShowFirstRecord();
        //Search(false);
        //DataTable dataTable = ((DataView)grd.DataSource).Table;
        //if (dataTable != null)
        //{
        //    DataView dataView = new DataView(dataTable);
        //    if (CurrentExp != e.SortExpression)
        //        CurrentSort = SortDirection.Ascending;
        //    dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
        //    grd.DataSource = dataView;
        //    grd.DataBind();
        //    CurrentExp = e.SortExpression;
        //}
    }

    //DM-3882 ALamadrid - Sort table by column
    private int ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        int newSortDirection;
        switch (sortDirection)
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

    public SortDirection CurrentSort
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

    public string CurrentExp
    {
        get
        {
            if (ViewState["sortExp"] == null)
            {
                return "ID";
            }
            return ViewState["sortExp"].ToString();
        }
        set { ViewState["sortExp"] = value; }
    }

    protected void odsMerchants_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        prms.Add("@AgentUID", UserSessions.CurrentAgent.AgentUID);

        if (BusinessDBAName.Text != string.Empty)
            prms.Add("@BusinessDBAName", BusinessDBAName.Text);

        if (MerchantID.Text != string.Empty)
            prms.Add("@ID", MerchantID.Text);

        
        prms.Add("@PageSize", this.PageSize);

        prms.Add("@CurrentPage", this.CurrentPage);
        
        
        grd.PageSize = this.PageSize;
        grd.PageIndex = this.CurrentPage - 1;

        //DM-3882 ALamadrid - Pagination and sort; 
        if (this.SortOrder == string.Empty)
            prms["@SortOrder"] = "BUSINESSDBANAME";
        else
            prms["@SortOrder"] = this.SortOrder;

        prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grd.ID;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetMerchantAppsPagingRowCount(prms, 0, 0, this.grd.ID).ToString();
   
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grd.PageSize = this.PageSize;

        grd.DataBind();
        ShowFirstRecord();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        BusinessDBAName.Text = "";
        MerchantID.Text = "";
    }

    decimal payout = 0.0m;

    protected void grdContracts_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                payout = 0.0M;
                break;
            case DataControlRowType.DataRow:
                //TextBox lblPayout = (TextBox)e.Row.FindControl("txtRate");
                var txtRate = DataBinder.Eval(e.Row.DataItem, "Rate").ToString();

                payout += txtRate.Equals(string.Empty) ? 0.00M : Convert.ToDecimal(txtRate.Replace('%', ' ').Trim());
                break;

            case DataControlRowType.Footer:
                e.Row.Cells[2].Text = payout.ToString("0.00");//"c");
                break;
            default:
                break;
        }
    }

    //DM-3882 ALamadrid - Page size selection
    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grd.PageSize = this.PageSize;
        grd.DataBind();
        ShowFirstRecord();
    }
}
