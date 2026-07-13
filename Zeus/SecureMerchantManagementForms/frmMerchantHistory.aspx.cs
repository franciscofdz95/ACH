using System;
using System.Data;
using System.Data.SqlClient;
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
using PaymentXP.Facade;
using ZeusWeb.Extensions;

public partial class frmMerchantHistory : frmBaseDataEntry
{

    public string vsChangeHistoryFieldID
    {
        get { return CommonUtility.Util.if_i(ViewState["vsChangeHistoryFieldID"], -1).ToString(); }
        set { ViewState["vsChangeHistoryFieldID"] = value; }
    }


    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentMerchantApp != null) 
            base.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        WucBusinessInfo1.pnlInfo.Enabled = false;
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.History);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "History");
            }

            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            this.FormShow(this.UID);

            if (UserSessions.CurrentUser.IsBank)
            {
                pnlACHHistory.Visible = false;
            }

            // PXP-6926 Move 'Change History' section from 'Linked Accounts' page to 'History' page
            // Fady Massoud 10/08/2018
            DropDownList ddl = (DropDownList)grdChange.HeaderRow.FindControl("ddlChangeType");

            LookupTableHandler.LoadChangeHistoryFields(ddl, true, ChangeHistoryFields.ChangeHistoryFieldSource.Merchant);
            ddl.SelectedValue = this.vsChangeHistoryFieldID;

            //Code added for PXP-8431 by koshlendra start                
            DropDownList ddlRlRId = (DropDownList)grdRelationshipsChangeHistory.HeaderRow.FindControl("ddlRelationShipsRecordId");
            LookupTableHandler.LoadRelationShipsRecordIds(ddlRlRId, true, UserSessions.CurrentMerchantApp.MerchantAppUID.ToString());
            ddlRlRId.SelectedValue = this.vsRelationshipsRecordID;
            //Code added for PXP-8431 by koshlendra end

            //Code added for PXP-8430 by koshlendra start 
            DropDownList ddlequip = ((DropDownList)grdEquipmentsChangeHistory.HeaderRow.FindControl("ddlEquipmentsRecordId"));
            LookupTableHandler.LoadEquipmentsRecordIds(ddlequip, true, UserSessions.CurrentMerchantApp.MerchantAppUID.ToString());
            ddlequip.SelectedValue = this.vsEquipmentsRecordID;

            //Code added for PXP-8430 by koshlendra end 
        }
    }


    public override void FormShow(string ID)
    {
        
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

        FormBinding.BindObjectToControls(agreement, WucBusinessInfo1);
        
        //check to see if the account is ACH only and get the ach status in case if it is or else the cc status
        
        string m_StatusUID = agreement.StatusUID.ToUpper();

        if (WucBusinessInfo1.isACHonly && UserSessions.ActiveAchMerchant != null)
        {
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
        }

        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);

        LoadCreditCardApplicationStatusHistory(true);

        // PXP-7623 RThakur
        LoadDocumentsChangeHistory(true);

        // PXP-7623
        if (agreement.AchID > 0)
        {
            LoadACHApplicationStatusHistory(true);
        }

        this.LoadMerchantMIDHistory(true);


        //Code added for PXP-8431 by koshlendra start
        LoadRelationshipsChangeHistory(true);
        //Code added for PXP-8431 by koshlendra end   
        //Code added for PXP-8430 by koshlendra start
          LoadEquipmentsChangeHistory(true);
        //Code added for PXP-8430 by koshlendra end 

        //DM-2363
        LoadRDRChangesHistory(true);
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

    #region ChangeHistory

    // START PXP-6926 Move 'Change History' section from 'Linked Accounts' page to 'History' page
    // Fady Massoud 10/08/2018

    protected void odsChangeHistory_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter app = new SearchParameter();



        prms.Add("@ZID", Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));

        int ChangeHistoryFieldID = CommonUtility.Util.if_i(this.vsChangeHistoryFieldID, 0);
        if (ChangeHistoryFieldID != 0)
            prms.Add("@ChangeHistoryFieldID", ChangeHistoryFieldID);

        FormBinding.BindControlsToObject(app, Panel1);

        prms.Add("@SortOrder", this.SortOrder);
        prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));
        this.SearchParameters = app;

        this.grdChange.PageSize = Convert.ToInt32(cboCHPageSize.SelectedItem.Value);
        prms.Add("@PageSize", this.grdChange.PageSize);

        prms.Add("@CurrentPage", grdChange.PageIndex + 1);


        //grdChange.PageSize = this.PageSize;
        //grdChange.PageIndex = this.CurrentPage - 1;

        e.InputParameters[0] = prms;
        //e.InputParameters[3] = this.grdChange.ID;
        lblCHRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetChangeHistoryPagingCount(prms, 0, 0).ToString();
    }

    public void LoadMerchantMIDHistory(bool IsOnLoad)
    {
        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, Panel1);
        }

        grdChange.DataBind();
    }

    protected void ddlChangeType_SelectedIndexChanged(object sender, EventArgs e)
    {

        DropDownList ddl = (DropDownList)sender;

        this.vsChangeHistoryFieldID = CommonUtility.Util.if_i(ddl.SelectedValue, -1).ToString();
        grdChange.PageIndex = 0;
        this.LoadMerchantMIDHistory(false);
    }

    protected void ddlChangeType_PreRender(object sender, EventArgs e)
    {
        DropDownList ddl = ((DropDownList)grdChange.HeaderRow.FindControl("ddlChangeType"));

        LookupTableHandler.LoadChangeHistoryFields(ddl, true, ChangeHistoryFields.ChangeHistoryFieldSource.Merchant);
        ddl.SelectedValue = this.vsChangeHistoryFieldID;
    }


    protected void grdChange_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ((Label)e.Row.Cells[1].FindControl("lblNameHeader")).Text = "Value";
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string value = DataBinder.Eval(e.Row.DataItem, "NewValue").ToString();

            ((Label)e.Row.Cells[1].FindControl("lblNewValue")).Text = value;
            e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text);


        }

    }
    // Fady Massoud 10/08/2018
    // END PXP-6926 Move 'Change History' section from 'Linked Accounts' page to 'History' page

    protected void cboCHPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdChange.PageIndex = 0;
        this.grdChange.PageSize = Convert.ToInt32(cboCHPageSize.SelectedItem.Value);
        this.LoadMerchantMIDHistory(false);
    }


    protected void grdChangeHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdChange.PageIndex = e.NewPageIndex;
        this.LoadMerchantMIDHistory(false);
    }

    protected void grd_ChangeHistorySorting(object sender, GridViewSortEventArgs e)
    {
        grdChange.PageIndex = 0;
        //this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.LoadMerchantMIDHistory(false);
    }


    #endregion ChangeHistory

    #region CreditCardApplicationStatusHistory

    protected void odsCC_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter app = new SearchParameter();

        prms.Add("@MerchantAppUID", this.UID);

        FormBinding.BindControlsToObject(app, Panel1);

        prms.Add("@SortOrder", this.SortOrder);
        prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));
        this.SearchParameters = app;

        this.grd.PageSize = Convert.ToInt32(cboCCPageSize.SelectedItem.Value);
        prms.Add("@PageSize", this.grd.PageSize);

        prms.Add("@CurrentPage", grd.PageIndex + 1);


        e.InputParameters[0] = prms;
        //e.InputParameters[3] = this.grdChange.ID;
        lblCCRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetSearchStatusHistoryCount(prms, 0, 0).ToString();
    }

    private void LoadCreditCardApplicationStatusHistory(bool IsOnLoad)
    {
        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, Panel1);
        }

        grd.DataBind();
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                e.Row.Cells[1].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[1].Text);
                break;
        }
    }

    protected void cboCCPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grd.PageIndex = 0;
        this.grd.PageSize = Convert.ToInt32(cboCCPageSize.SelectedItem.Value);
        LoadCreditCardApplicationStatusHistory(false);
    }

    protected void grdCC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd.PageIndex = e.NewPageIndex;
        LoadCreditCardApplicationStatusHistory(false);
    }
    protected void grd_CCSorting(object sender, GridViewSortEventArgs e)
    {
        grd.PageIndex = 0;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        LoadCreditCardApplicationStatusHistory(false);
    }

    #endregion CreditCardApplicationStatusHistory


    #region ACHApplicationStatusHistory
    protected void odsACHH_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        SearchParameter app = new SearchParameter();

        prms.Add("@merchantID", Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));

        FormBinding.BindControlsToObject(app, Panel1);

        prms.Add("@SortOrder", this.SortOrder);
        prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));
        this.SearchParameters = app;

        this.grd2.PageSize = Convert.ToInt32(cboACHHPageSize.SelectedItem.Value);
        prms.Add("@PageSize", this.grd2.PageSize);

        prms.Add("@CurrentPage", grd2.PageIndex + 1);


        e.InputParameters[0] = prms;
        //e.InputParameters[3] = this.grdChange.ID;
        Label1.Visible = !(grd2.Rows.Count > 0);
        lblACHHRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetACHSearchStatusHistoryCount(prms, 0, 0).ToString();
    }
    private void LoadACHApplicationStatusHistory(bool IsOnLoad)
    {
        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, Panel1);
        }

        grd2.DataBind();
    }

    protected void grd2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                e.Row.Cells[1].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[1].Text);
                break;
        }
    }

    protected void cboACHHPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grd2.PageIndex = 0;
        this.grd2.PageSize = Convert.ToInt32(cboACHHPageSize.SelectedItem.Value);
        LoadACHApplicationStatusHistory(false);
    }

    protected void grdACHH_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd2.PageIndex = e.NewPageIndex;
        LoadACHApplicationStatusHistory(false);
    }

    protected void grd_ACHHSorting(object sender, GridViewSortEventArgs e)
    {
        grd2.PageIndex = 0;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        LoadACHApplicationStatusHistory(false);
    }
    #endregion ACHApplicationStatusHistory

    #region DocumentsChangeHistory

    protected void odsDocH_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter app = new SearchParameter();

        prms.Add("@MerchantAppUID", this.UID);

        FormBinding.BindControlsToObject(app, Panel1);

        prms.Add("@SortOrder", this.SortOrder);
        prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));
        this.SearchParameters = app;

        this.grd3.PageSize = Convert.ToInt32(cboDocHPageSize.SelectedItem.Value);
        prms.Add("@PageSize", this.grd3.PageSize);

        prms.Add("@CurrentPage", grd3.PageIndex + 1);


        e.InputParameters[0] = prms;
        //e.InputParameters[3] = this.grdChange.ID;
        lblDocumentsStatus.Visible = !(grd3.Rows.Count > 0);
        lblDocHRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetSearchDocumentsChangeHistoryCount(prms, 0, 0).ToString();
    }

    private void LoadDocumentsChangeHistory(bool IsOnLoad)
    {
        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, Panel1);
        }

        grd3.DataBind();
    }

    //PXP-7623 RThakur
    protected void grd3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                e.Row.Cells[3].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[3].Text);
                break;
        }
    }

    protected void cboDocHPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grd3.PageIndex = 0;
        this.grd3.PageSize = Convert.ToInt32(cboDocHPageSize.SelectedItem.Value);
        LoadDocumentsChangeHistory(false);
    }

    protected void grdDocH_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd3.PageIndex = e.NewPageIndex;
        LoadDocumentsChangeHistory(false);
    }

    protected void grd_DocHSorting(object sender, GridViewSortEventArgs e)
    {
        grd3.PageIndex = 0;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        LoadDocumentsChangeHistory(false);
    }

    #endregion DocumentsChangeHistory



    #region RelationshipsChangeHistory

    protected void odsRelationshipsChangeHistory_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        SearchParameter app = new SearchParameter();

        prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID.ToString());

        int relationshipsRecordID = CommonUtility.Util.if_i(this.vsRelationshipsRecordID, 0);

        if (relationshipsRecordID != 0)
        {
            prms.Add("@RelationshipRecordID", relationshipsRecordID);
        }

        FormBinding.BindControlsToObject(app, Panel1);

        prms.Add("@SortOrder", this.SortOrder);
        prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));
        this.SearchParameters = app;

        this.grdRelationshipsChangeHistory.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        prms.Add("@PageSize", this.grdRelationshipsChangeHistory.PageSize);

        prms.Add("@CurrentPage", grdRelationshipsChangeHistory.PageIndex + 1);


        e.InputParameters[0] = prms;
        //e.InputParameters[3] = this.grdChange.ID;
        lblRelationshipsHRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetSelectRelationshipsChangeHistoryCount(prms, 0, 0).ToString();
    }
    //Code added for PXP-8431by koshlendra start

    public string vsRelationshipsRecordID
    {
        get { return CommonUtility.Util.if_i(ViewState["vsRelationshipsRecordID"], -1).ToString(); }
        set { ViewState["vsRelationshipsRecordID"] = value; }
    }
    private void LoadRelationshipsChangeHistory(bool IsOnLoad)
    {
        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, Panel1);
        }

        grdRelationshipsChangeHistory.DataBind();
    }

    protected void ddlRelationShipsRecordId_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = ((DropDownList)grdRelationshipsChangeHistory.HeaderRow.FindControl("ddlRelationShipsRecordId"));
        this.vsRelationshipsRecordID = CommonUtility.Util.if_i(ddl.SelectedValue, -1).ToString();

        this.LoadRelationshipsChangeHistory(false);
    }

    protected void ddlRelationShipsRecordId_PreRender(object sender, EventArgs e)
    {
        DropDownList ddl = ((DropDownList)grdRelationshipsChangeHistory.HeaderRow.FindControl("ddlRelationShipsRecordId"));
        LookupTableHandler.LoadRelationShipsRecordIds(ddl, true, UserSessions.CurrentMerchantApp.MerchantAppUID.ToString());
        ddl.SelectedValue = this.vsRelationshipsRecordID;
    }

    protected void grdRelationshipsChangeHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ((Label)e.Row.Cells[1].FindControl("lblNewNameHeader")).Text = "Value";
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string value = DataBinder.Eval(e.Row.DataItem, "NewValue").ToString();

            ((Label)e.Row.Cells[1].FindControl("lblNewValue")).Text = value;
            e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text);

        }

    }
    protected void grdRelationshipsChangeHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdRelationshipsChangeHistory.PageIndex = e.NewPageIndex;
        LoadRelationshipsChangeHistory(false);
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdRelationshipsChangeHistory.PageIndex = 0;
        this.grdRelationshipsChangeHistory.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        LoadRelationshipsChangeHistory(false);

    }
    //Code added for PXP-8431by koshlendra end

    protected void grd_RelationshipsChangeHistorySorting(object sender, GridViewSortEventArgs e)
    {
        grd.PageIndex = 0;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        LoadRelationshipsChangeHistory(false);
    }
    #endregion RelationshipsChangeHistory


    #region EquipmentsChangeHistory
    protected void odsEquipmentsChangeHistory_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        SearchParameter app = new SearchParameter();

        prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID.ToString());

        int equipmentsRecordID = CommonUtility.Util.if_i(this.vsEquipmentsRecordID, 0);

        if (equipmentsRecordID != 0)
        {
            prms.Add("@EquipmentRecordID", equipmentsRecordID);
        }

        FormBinding.BindControlsToObject(app, Panel1);

        prms.Add("@SortOrder", this.SortOrder);
        prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));
        this.SearchParameters = app;

        this.grdEquipmentsChangeHistory.PageSize = Convert.ToInt32(cboEHPageSize.SelectedItem.Value);
        prms.Add("@PageSize", this.grdEquipmentsChangeHistory.PageSize);

        prms.Add("@CurrentPage", grdEquipmentsChangeHistory.PageIndex + 1);


        e.InputParameters[0] = prms;
        //e.InputParameters[3] = this.grdChange.ID;
        lblEquipmentHRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetSelectEquipmentsChangeHistoryCount(prms, 0, 0).ToString();
    }

    //Code added for PXP-8430 by koshlendra start
    public string vsEquipmentsRecordID
    {
        get { return CommonUtility.Util.if_i(ViewState["vsEquipmentsRecordID"], -1).ToString(); }
        set { ViewState["vsEquipmentsRecordID"] = value; }
    }

    private void LoadEquipmentsChangeHistory(bool IsOnLoad)
    {
        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, Panel1);
        }

        grdEquipmentsChangeHistory.DataBind();
    }

    protected void ddlEquipmentsRecordId_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = ((DropDownList)grdEquipmentsChangeHistory.HeaderRow.FindControl("ddlEquipmentsRecordId"));
        this.vsEquipmentsRecordID = CommonUtility.Util.if_i(ddl.SelectedValue, -1).ToString();

        this.LoadEquipmentsChangeHistory(false);
    }

    protected void ddlEquipmentsRecordId_PreRender(object sender, EventArgs e)
    {
        DropDownList ddl = ((DropDownList)grdEquipmentsChangeHistory.HeaderRow.FindControl("ddlEquipmentsRecordId"));
        LookupTableHandler.LoadEquipmentsRecordIds(ddl, true, UserSessions.CurrentMerchantApp.MerchantAppUID.ToString());
        ddl.SelectedValue = this.vsEquipmentsRecordID;
    }

    protected void grdEquipmentsChangeHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            ((Label)e.Row.Cells[1].FindControl("lblNewNameHeader")).Text = "Value";
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string value = DataBinder.Eval(e.Row.DataItem, "NewValue").ToString();

            ((Label)e.Row.Cells[1].FindControl("lblNewValue")).Text = value;
            e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text);

        }

    }
    protected void grdEquipmentsChangeHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdEquipmentsChangeHistory.PageIndex = e.NewPageIndex;
        LoadEquipmentsChangeHistory(false);
    }

    protected void cboEHPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdEquipmentsChangeHistory.PageIndex = 0;
        this.grdEquipmentsChangeHistory.PageSize = Convert.ToInt32(cboEHPageSize.SelectedItem.Value);
        LoadEquipmentsChangeHistory(false);

    }
    //Code added for PXP-8430by koshlendra end

    protected void grd_EquipmentsChangeHistorySorting(object sender, GridViewSortEventArgs e)
    {
        grdEquipmentsChangeHistory.PageIndex = 0;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        LoadEquipmentsChangeHistory(false);
    }
    #endregion EquipmentsChangeHistory


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

    #region RDR Producto Rule History
    //DM-2363 
    private int _FilterIdSelected
    {
        get { return CommonUtility.Util.if_i(ViewState["filterIdSelected"], 0); }
        set { ViewState["filterIdSelected"] = value; }
    }

    protected void LoadRDRChangesHistory(bool IsOnLoad)
    {
        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, Panel1);
        }

        RdrProductRulesGrid.DataBind();
    }

    protected void RdrPageSizeCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
        RdrProductRulesGrid.PageIndex = 0;
        RdrProductRulesGrid.PageSize = Convert.ToInt32(RdrPageSizeCombo.SelectedItem.Value);
        LoadRDRChangesHistory(false);
    }

    protected void RdrProductRulesGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RdrProductRulesGrid.PageIndex = e.NewPageIndex;
        LoadRDRChangesHistory(false);
    }

    protected void odsRdrProductRulesChangeHistory_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        SearchParameter app = new SearchParameter();

        prms.Add("@MerchantId", UserSessions.CurrentMerchantApp.ID);
        prms.Add("@FilterId", _FilterIdSelected);

        FormBinding.BindControlsToObject(app, Panel1);
        this.SearchParameters = app;

        prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));
        RdrProductRulesGrid.PageSize = Convert.ToInt32(RdrPageSizeCombo.SelectedItem.Value);
        prms.Add("@PageSize", RdrProductRulesGrid.PageSize);
        prms.Add("@PageIndex", RdrProductRulesGrid.PageIndex);


        e.InputParameters[0] = prms;
        lblRuleChangeHistoryRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetProductRuleHistoryCount(prms, 0, 0).ToString();
    }

    protected void RdrProductFilterCol_PreRender(object sender, EventArgs e)
    {
        DropDownList ddl = ((DropDownList)RdrProductRulesGrid.HeaderRow.FindControl("RdrProductFilterCol"));
        var lsItems = DataRDR.SelectProductRuleHistoryFilter(UserSessions.CurrentMerchantApp.ID);

        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("All", "0"));
        foreach (var p in lsItems)
        {
            ddl.Items.Add(new ListItem(p.Value, p.Key));
        }
        ddl.SelectedValue = CommonUtility.Util.if_i(_FilterIdSelected, 0).ToString();
    }

    protected void RdrProductFilterCol_SelectedIndexChanged(object sender, EventArgs e)
    {
        _FilterIdSelected = CommonUtility.Util.if_i(((DropDownList)sender).SelectedItem.Value, 0);
        RdrProductRulesGrid.PageIndex = 0;
        LoadRDRChangesHistory(false);
    }
    //DM-2363 
    protected void RdrProductRulesGridSorting(object sender, GridViewSortEventArgs e)
    {
        RdrProductRulesGrid.PageIndex = 0;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        LoadRDRChangesHistory(false);
    }
    #endregion
}
