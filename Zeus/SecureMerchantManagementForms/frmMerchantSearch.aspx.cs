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
using System.Data.SqlClient;
using Infragistics.WebUI.WebControls;
using CommonUtility;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;

public partial class frmMerchantSearch : frmBaseSearch
{
    

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

    public override void Search(bool IsOnLoad)
    {
        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }

        grd.DataBind();

        pnlRecords.Visible = (grd.Rows.Count > 0);
        pnlNoRecords.Visible = !(grd.Rows.Count > 0);
    }

    private void FormClear()
    {
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);
        ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
        rdExport.SelectedIndex = 0;
        wucAgentSelector.FormClear();
        wucAccountGroups.AccountGroupIds = null;
        wucAccountGroups.MerchantAccountGroups = null;
        this.Search(false);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        WebUtil.SetUserSpecificDisplayMode(SearchBeginDate);
        WebUtil.SetUserSpecificDisplayMode(SearchEndDate);

        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Search);

            wucAgentSelector.EnableAgentDBA();

            //load all dropdownlists
            if (UserSessions.CurrentUser.IsAgent)
            {            
                pnlSpace.Visible = true;
                pnlAgent.Visible = false;
            }
            else
            {
                pnlSpace.Visible = false; pnlAgent.Visible = true;
            }
            
            LookupTableHandler.MerchantAppStatus(StatusUID, true, "Merchant Management");
            LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, true);
            LookupTableHandler.LoadActiveInternalUsers(AssignToUID, true);
            LookupTableHandler.LoadFrontEndPlatforms(AuthPlatformUID, true);
            LookupTableHandler.LoadBackEndPlatforms(SettlePlatformUID, true);
            LookupTableHandler.LoadUsersByRole(FTRep, true, Constants.ROLE_FIRSTTEAM);

            ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
            this.Search(true);
        }

        if (UserSessions.CurrentUser.IsBank)
        {
            MerchantAppTypeUID.Visible = false;
            lblBank.Visible = false;

            pnlAgent.Visible = false;

            lblACHID.Visible = false;
            AchID.Visible = false;

            lblMerchantTag.Visible = false;
            MerchantTag.Visible = false;

            lblAssignTo.Visible = false;
            AssignToUID.Visible = false;

            ListHandler.ListFindItem(MerchantAppTypeUID, UserSessions.CurrentUser.HookTableKeyUID);
        }

        if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper().Equals(Constants.ROLE_BANK))
        {
            this.pnlAccGroupSearch.Visible = false;
        }
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        grd.PageIndex = 0;
        this.CurrentPage = 1;
        this.SortOrder = string.Empty;
        this.SearchParameters = null;
        this.Search(false);
    }

    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=true";
        url += "&PostBackURL=~/SecureMerchantManagementForms/frmMerchantSearch.aspx";

        Response.Redirect(url);
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grd.PageSize = this.PageSize;
        this.Search(false);
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
            this.CurrentPage = grd.PageIndex + 1;
        }

        Search(false);
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        FormHandler.Export2Excel("MerchantList.xls", grd);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        if (rdExport.SelectedValue.Equals("1"))
        {
            this.PageSize = 5000;
            this.CurrentPage = 1;
        }
        else
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = grd.PageIndex + 1;
        }

        Search(false);
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        FormHandler.ExportToPDF(grd, true, "Merchant Search Results");
    }

    protected void odsMerchants_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter app;

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@BeginPostedDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndPostedDate", SearchEndDate.Value);


        //If procedure is called for the first time pass a dummy parameter to initial the grid
        if (BusinessLegalName.Text != string.Empty)
            prms.Add("@BusinessLegalName", BusinessLegalName.Text);

        if (BusinessDBAName.Text != string.Empty)
            prms.Add("@BusinessDBAName", BusinessDBAName.Text);

        if (BusinessContact.Text != string.Empty)
            prms.Add("@BusinessContact", BusinessContact.Text);

        if (StatusUID.SelectedIndex > 0)
            prms.Add("@StatusUID", StatusUID.SelectedItem.Value);

        if (BusinessZip.Text != string.Empty)
            prms.Add("@BusinessZip", BusinessZip.Text);

        if (BusinessPhone.Text.ToString().Trim() != string.Empty)
            prms.Add("@BusinessPhone", BusinessPhone.Text.Trim());

        if (BusinessFax.Value.ToString().Trim() != string.Empty)
            prms.Add("@BusinessFax", BusinessFax.Text);

        if (BusinessEmailAddress.Text != string.Empty)
            prms.Add("@BusinessEmailAddress", BusinessEmailAddress.Text);

        if (MerchantID.Text != string.Empty)
            prms.Add("@ID", MerchantID.Text);

        if (AchID.Text != string.Empty)
            prms.Add("@AchID", AchID.Text);

        if (SettlePlatformMid.Text != string.Empty)
            prms.Add("@SettlePlatformMid", SettlePlatformMid.Text);

        if (SSN.Value.ToString().Trim() != string.Empty)
            prms.Add("@SSN", SSN.Text);

        if (BusinessDBAPhone.Value.ToString().Trim() != string.Empty)
            prms.Add("@BusinessDBAPhone", BusinessDBAPhone.Text.Trim());

        if (OwnerName.Text != string.Empty)
            prms.Add("@OwnerName", OwnerName.Text);
        
        if (!string.IsNullOrEmpty(wucAccountGroups.AccountGroupIds))
            prms.Add("@AccountGroupIds", wucAccountGroups.AccountGroupIds.TrimEnd(','));
        
        if (MerchantAppTypeUID.SelectedIndex > 0)
            prms.Add("@MerchantAppTypeUID", MerchantAppTypeUID.SelectedItem.Value);

        if (BusinessState.SelectedIndex > 0)
            prms.Add("@BusinessState", BusinessState.SelectedItem.Value);

        if (AssignToUID.SelectedIndex > 0)
            prms.Add("@AssignToUID", AssignToUID.SelectedItem.Value);

        if (MerchantTag.Text != string.Empty)
            prms.Add("@MerchantTag", MerchantTag.Text.Trim());

        if (SubAgent.Checked && wucAgentSelector.m_AgentUID != string.Empty)
        {
            prms.Add("@AgentUIDSub", wucAgentSelector.m_AgentUID);
        }
        else
        {
            if (wucAgentSelector.m_AgentID != string.Empty)
                prms.Add("@AgentID", wucAgentSelector.m_AgentID);

            if (wucAgentSelector.m_AgentDBA != string.Empty)
                prms.Add("@AgentDBA", wucAgentSelector.m_AgentDBA);
        }

        if (SettlePlatformUID.SelectedIndex > 0)
            prms.Add("@SettlePlatformUID", SettlePlatformUID.SelectedItem.Value);

        if (AuthPlatformUID.SelectedIndex > 0)
            prms.Add("@AuthPlatformUID", AuthPlatformUID.SelectedItem.Value);

        if (AuthPlatformMid.Text != string.Empty)
            prms.Add("@AuthPlatformMid", AuthPlatformMid.Text);

        if (TID.Text != string.Empty)
            prms.Add("@TID", TID.Text.Trim());

        if (FirstTeam.SelectedIndex == 1)
            prms.Add("@FirstTeam", true);
        else if (FirstTeam.SelectedIndex == 2)
            prms.Add("@FirstTeam", false);

        if (FTRep.SelectedIndex > 0)
            prms.Add("@FTRep", FTRep.SelectedValue);
       
        if (!string.IsNullOrWhiteSpace(FMAID.Text))
            prms.Add("@FMAID", FMAID.Text.Trim());

        if (!string.IsNullOrWhiteSpace(MerchantBrandID.Text))
            prms.Add("@MerchantBrandID", MerchantBrandID.Text.Trim());

        ///Code added by koshlendra for PXP-1621 - User should able to search merchant by MCC code and PXP-4226 start
        if (!string.IsNullOrWhiteSpace(MCC.Text) && MCC.Text.Trim().Length==4)
            prms.Add("@MCC", MCC.Text.Trim());
        ///Code added by koshlendra for PXP-1621- User should able to search merchant by MCC code and PXP-4226 end
        // Niranjan:-PXP-8060
        if (NMIUserName.Text != string.Empty)
            prms.Add("@HostUserName", NMIUserName.Text);
        ///Code added by koshlendra for PXP-12945 - Zeus:Update Merchants Search Tab to Search Merchants by ‘Visa MCC’  start
        if (!string.IsNullOrWhiteSpace(VisaMCC.Text) && VisaMCC.Text.Trim().Length == 4)
            prms.Add("@VisaMCC", VisaMCC.Text.Trim());
        ///Code added by koshlendra for PXP-12945- Zeus:Update Merchants Search Tab to Search Merchants by ‘Visa MCC’  end
        ///Code added by koshlendra for PXP-12946 - Zeus:Update Merchants Search Tab to Search Merchants by ‘Descriptor’  start
        if (!string.IsNullOrWhiteSpace(Descriptor.Text.Trim()))
            prms.Add("@Descriptor", Descriptor.Text.Trim());
        ///Code added by koshlendra for PXP-12946- Zeus: Zeus:Update Merchants Search Tab to Search Merchants by ‘Descriptor’ end

        //DM-7281 Nisha Magnani
        if (Tilled.Checked)
        {
            prms.Add("@IsTilled", Tilled.Checked);
        }

        if (prms.Count > 0)
        {
            //Do not move code below
            if (UserSessions.CurrentUser.IsAgent)
                prms.Add("@AgentUIDSub", UserSessions.CurrentUser.HookTableKeyUID);
            else if (UserSessions.CurrentUser.IsInternal)
                prms.Add("@InternalUserUID", UserSessions.CurrentUser.UID);

            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;
            
            prms.Add("@PageSize", this.PageSize);

            prms.Add("@CurrentPage", this.CurrentPage);
            grd.PageSize = this.PageSize;
            grd.PageIndex = this.CurrentPage - 1;

            if (this.SortOrder == string.Empty)
                prms["@SortOrder"] = "ID";
            else
                prms["@SortOrder"] = this.SortOrder;

           // UserSessions.CurrentUser.OfficeAccess.

           
                //.Find(x => x.Equals(ObjUser.Office)) == 0)

             List<int> officelist = new List<int>();
            foreach (Office objOffice in UserSessions.CurrentUser.OfficeAccess)
            {
                officelist.Add(objOffice.OfficeID);
            }
            if (officelist.Find((x => x.Equals((int)CommonUtility.Util.Offices.LosAngeles))) != 0)
            {
                prms["@officeID"] = string.Join(",", officelist);  
            }
            //if (UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.LosAngeles))
            //    prms["@officeID"] = 5;

            prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

            e.InputParameters[0] = prms;
            e.InputParameters[3] = this.grd.ID;
            lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetMerchantAppsPagingRowCount(prms, 0, 0, this.grd.ID).ToString();
        }
        else
        {
            // we do this so that we don't call the SP with zero paramters. it's very slow otherwise.
            e.Cancel = true;
        }

    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                e.Row.Cells[4].Attributes.Add("class", "text");

                if (!Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HasConditions")))
                {
                    HyperLink hyDBA = (HyperLink)e.Row.FindControl("hypDBAName");
                    hyDBA.NavigateUrl = string.Empty;
                    hyDBA.Style.Add("text-decoration", "none");
                    hyDBA.Style.Add("color", "black");
                }

                if (DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "FirstTeam")))
                {
                    e.Row.BackColor = System.Drawing.Color.LightBlue;
                }

                if (DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "IsTilled")))
                {
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                }

                //List of Private Label MBC Agent ID which cannot be put in PrivateLabel table of PrimusPayments Database but still need to be shown as private label MBC.
                string strPrivateLabelAgentId = ConfigurationManager.AppSettings["PrivateLabelAgentIds"];
                if (string.IsNullOrEmpty(strPrivateLabelAgentId))
                {
                    strPrivateLabelAgentId = "5159,5209";
                }
                if ((!string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "PrivateLabelUID").ToString())) || (strPrivateLabelAgentId.Contains(DataBinder.Eval(e.Row.DataItem, "AgentID").ToString())))
                {
                    e.Row.CssClass = "pLabel";
                }

                e.Row.Cells[9].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[9].Text);

                Label MBID = (Label)e.Row.FindControl("lblMerchantBrandID");
                MBID.Text = CommonUtility.Util.if_i(MBID.Text, 1) == 1 ? "3rd Party" : "NetBanx";

               // MBID.Text = ((MerchantBrand)(CommonUtility.Util.if_i(MBID.Text, 1))).ToString();

                //eluxa 2015/06/11 rollback 7733: Create a way to identify App Types in the App Queues
                //decimal swipe_percent = CommonUtility.Util.if_dec(DataBinder.Eval(e.Row.DataItem, "TinfoElectronicDataCaptureSwipedPercent"), 0);
                //Label lbRetail = (Label)e.Row.FindControl("lblRetail");
                //if (swipe_percent >= .70m)
                //{
                //    lbRetail.Visible = true;
                //    lbRetail.Attributes.Add("title", String.Format("Swipe Percent: {0:F2}%", swipe_percent * 100m));
                //}
                //else
                //{
                //    lbRetail.Visible = false;
                //}

                //Label lbPL = (Label)e.Row.FindControl("lblPL");
                //if (!String.IsNullOrWhiteSpace(DataBinder.Eval(e.Row.DataItem, "PLCompanyName").ToString()))
                //{
                //    lbPL.Visible = true;
                //    lbPL.Attributes.Add("title", string.Format("Private Label: {0}", DataBinder.Eval(e.Row.DataItem, "PLCompanyName").ToString()));
                //}
                //else
                //{
                //    lbPL.Visible = false;
                //}

                //Label lbFT = (Label)e.Row.FindControl("lblFT");
                //if (!String.IsNullOrWhiteSpace(DataBinder.Eval(e.Row.DataItem, "PremierRepName").ToString()))
                //{
                //    lbFT.Visible = true;
                //    lbFT.Attributes.Add("title", string.Format("Premier Rep: {0}", DataBinder.Eval(e.Row.DataItem, "PremierRepName").ToString()));
                //}
                //else
                //{
                //    lbFT.Visible = false;
                //}



                break;

            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
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

   

}
