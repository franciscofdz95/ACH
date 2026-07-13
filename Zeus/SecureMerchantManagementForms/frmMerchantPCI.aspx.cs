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
using System.Text;
using System.Collections.Generic;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Drawing;
using Infragistics.WebUI.WebDataInput;


public partial class frmMerchantPCI : frmBaseDataEntry
{
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
        WebUtil.SetUserSpecificDisplayMode(RequestDate);
        WebUtil.SetUserSpecificDisplayMode(CompletedDate);
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.PCI);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "PCI");
            }

            LookupTableHandler.MerchantAppStatus(StatusUID, false, "PCI");
            LookupTableHandler.LoadPCIVendors(VendorID, false);

            FormShow("");
        }
    }



    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

        FormBinding.BindObjectToControls(agreement, WucBusinessInfo1);
        FormHandler.SetControlEditMode(WucBusinessInfo1, false);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        grd.Enabled = !this.EditMode;
        VendorID.Enabled = false;

        BindGrid();

        WucBusinessInfo1.LoadOffice(agreement);
        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message    
        if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (this.btnSave.Enabled && !this.EditMode) || (!this.btnCancel.Enabled && !this.EditMode))
        {
            MasterPageMerchant master = (MasterPageMerchant)this.Master;
            master.UpdateNotification("");
            MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
        }  
        /******** End of PXP-2206 **************/
        
    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlDetail);
    }

    public override bool FormSave()
    {
        bool perform = false;
        Hashtable prms = new Hashtable();
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        int rows = 0;

        if (!this.FormDataCheck())
            return false;

        //save merchant
        MerchantApp agreement = null;
        agreement = (MerchantApp)UserSessions.CurrentMerchantApp;

        prms.Add("@MerchantID", agreement.ID);
        prms.Add("@VendorID", VendorID.SelectedValue);
        prms.Add("@StatusUID", StatusUID.SelectedValue);
        prms.Add("@RequestDate", Convert.ToDateTime(RequestDate.Value));
        if (!string.IsNullOrEmpty(CompletedDate.Text))
            prms.Add("@CompletedDate", DataLayer.Field2Date(CompletedDate.Value));

        if (Adding)
        {
            prms.Add("@MerchantAppUID", agreement.MerchantAppUID);
            rows = data.InsertMerchantPCI(prms);
            if (rows > 0)
                this.UID = string.Empty;
        }
        else
        {
            if (string.IsNullOrEmpty(this.UID))
            {
                this.Master.AddMessageError("Please select a record in the grid.");
                return false;
            }
            prms.Add("@UID", this.UID);
            rows = data.UpdateMerchantPCI(prms);
        }

        if (rows > 0)
            perform = true;
        else
            this.Master.AddMessageError("Failed to save the data.");

        return perform;
    }

    public override void FormNew()
    {
        this.FormClear();
        this.Adding = true;
        this.EditMode = true;
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        ListHandler.ListFindItem(VendorID, "3"); // Select self as vendor
        grd.Enabled = false;
        VendorID.Enabled = false;
        RequestDate.Value = DateTime.Today.AddDays(1);
        this.ToggleButtons();

    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDataCheck()
    {
        string message = string.Empty;

        if (VendorID.SelectedIndex == 0)
            this.Master.AddMessageError("Please select a vendor.");

        if (StatusUID.SelectedIndex == 0)
            this.Master.AddMessageError("Please select a status.");

        if (string.IsNullOrEmpty(RequestDate.Text))
            this.Master.AddMessageError("Please select sent date.");

        if (StatusUID.SelectedItem.Value.ToUpper() == "A769CBEB-F7D8-4D0B-9653-FCE0B3BDBFB7" && string.IsNullOrEmpty(CompletedDate.Text))
            this.Master.AddMessageError("Must enter Completed Date for Compliant status.");

        if (this.Master.ErrorCount() == 0)
            return true;
        else
        {
            //lblError.Text = message;
            return false;
        }
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        this.Adding = false;
        this.ToggleButtons();
        FormShow("");
        //if (!string.IsNullOrEmpty(this.UID))
        //    BindForm();
        //else
        FormClear();
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnAdd.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnCancel.Enabled = this.EditMode;
        btnRefresh.Enabled = !this.EditMode;

        this.Master.ToggleMenu(!this.EditMode);

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
                {   // valid input values.
                    this.EditMode = false;
                    this.Adding = false;
                    FormClear();
                    FormShow("");
                    ToggleButtons();
                }
                break;

            case "Refresh":
                MerchantFacade facade = new MerchantFacade();
                UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
                FormShow("");
                break;

            case "Cancel":
                this.FormCancel();
                break;

            case "Delete":
                if (this.FormDelete())
                    Response.Redirect("frmLeads.aspx");
                break;

            case "Edit":
                {
                    //Modified by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message
                    if (grd.SelectedIndex > -1)
                    {                        
                        this.EditMode = true;
                        FormShow("");
                        string firstUserdetail = MerchantFacade.GetFirstUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID);
                        if (!string.IsNullOrWhiteSpace(firstUserdetail))
                        {
                            string notification = firstUserdetail + " is currently editing this ZID. Please ensure you will not overwrite each other's work.";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Notification", "alert(" + '"' + notification + '"' + ");", true);
                            MasterPageMerchant master = (MasterPageMerchant)this.Master;
                            master.UpdateNotification(firstUserdetail + " is editing this ZID.");
                        }
                        MerchantFacade.AddNewUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
                        /******** End of PXP-2206 **************/
                        this.ToggleButtons();
                    }
                }
                break;
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                LinkButton btn = (LinkButton)e.Row.FindControl("lbtnZID");
                btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "UID").ToString();

                e.Row.Cells[3].Text = WebUtil.ConvertToUserDatePattern(e.Row.Cells[3].Text);
                e.Row.Cells[4].Text = WebUtil.ConvertToUserDatePattern(e.Row.Cells[4].Text);

                break;
            case DataControlRowType.Footer:
                break;
            default:
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "MerchantID":
                LinkButton btn = (LinkButton)e.CommandSource;
                this.UID = btn.CommandArgument;
                grd.SelectedIndex = ((GridViewRow)btn.NamingContainer).RowIndex;
                BindForm();
                break;
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        BindGrid();
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
        BindGrid();
    }

    private void ClearGrid()
    {
        grd.DataSourceID = string.Empty;
        grd.DataBind();
    }

    protected void odsTransactions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = this.m_Prms;
    }

    private void BindGrid()
    {
        m_Prms = new Hashtable();
        if (!m_Prms.ContainsKey("@PageSize"))
            m_Prms.Add("@PageSize", this.PageSize);
        else
            m_Prms["@PageSize"] = this.PageSize;

        if (!m_Prms.ContainsKey("@CurrentPage"))
            m_Prms.Add("@CurrentPage", this.CurrentPage);
        else
            m_Prms["@CurrentPage"] = this.CurrentPage;

        if (!m_Prms.ContainsKey("@SortOrder"))
            m_Prms.Add("@SortOrder", "CustomerID");
        else
            m_Prms["@SortOrder"] = this.SortOrder;

        m_Prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

        if (UserSessions.CurrentMerchantApp != null)
            m_Prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetMerchantPCIPagingRowCount(m_Prms, 0, 0).ToString();

        grd.DataBind();

        pnlRecords.Visible = grd.Rows.Count > 0;
        pnlNoRecords.Visible = grd.Rows.Count == 0;
        grd.SelectedIndex = -1;
    }

    private void BindForm()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@UID", this.UID);

        DataSet ds = DataAccess.DataMerchantAppDao.GetMerchantPCI(prms);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ListHandler.ListFindItem(VendorID, dr["VendorID"].ToString());
            ListHandler.ListFindItem(StatusUID, dr["StatusUID"].ToString());
            RequestDate.Value = dr["RequestDate"].ToString();
            CompletedDate.Value = dr["CompletedDate"].ToString();
        }
    }
}
