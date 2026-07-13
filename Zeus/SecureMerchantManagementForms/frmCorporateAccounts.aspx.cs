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
using System.Drawing;
using System.Collections.Generic;

public partial class frmCorporateAccounts : frmBaseDataEntry
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
        this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Corporate);

        if (!this.IsPostBack)
        {
            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Corporate");
            }

            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            this.FormShow(this.UID);
            WucSelectMerchant1.WebDialogWindowClientID = this.WebDialogWindow2.ClientID;
            WucSelectMerchant1.HookTableDBAClientID = "";
            WucSelectMerchant1.HookTableIDClientID = txtZID.ClientID;
            WucSelectMerchant1.HookTableUIDClientID = HookTableKeyUID.ClientID;

        }
    }



    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = UserSessions.CurrentMerchantApp;

        FormBinding.BindObjectToControls(agreement, WucBusinessInfo1);
        WucBusinessInfo1.pnlInfo.Enabled = false;

        //check to see if the account is ACH only and get the ach status in case if it is or else the cc status
        bool isACHOnly = WucBusinessInfo1.isACHonly;

        string m_StatusUID = agreement.StatusUID.ToUpper();

        if (isACHOnly && UserSessions.ActiveAchMerchant != null)
        {
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
        }

        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);

        int count = this.LoadCorporateAccounts(Convert.ToInt32(agreement.ID));

        pnlCorporate.Visible = false;
        pnlNoCorporate.Visible = false;

        if (count == 0)
        {
            pnlNoCorporate.Visible = true;
        }
        else
        {
            if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == "CABA4669-7C78-484E-988B-DBCEAA5FC026") // corporate
            {
                pnlCorporate.Visible = true;
            }
        }


        LoadProcessingMerchants();

        if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus)
        {
            this.pnlMCPAccounts.Visible = false;
        }

        else if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Optimal)
        {
            if (UserSessions.CurrentMerchantApp.MCPIndicator == MCPIndicator.Processing)
            {
                this.EnableMCP.Enabled = false;
                this.trchkMCP.Visible = false;
            }
            else if (UserSessions.CurrentMerchantApp.MCPIndicator == MCPIndicator.Settlement)
            {
                this.EnableMCP.Checked = true;
                this.trSearchMCP.Visible = true;

            }
        }

        WucBusinessInfo1.LoadOffice(agreement);
        
    }

    private int LoadCorporateAccounts(int ZID)
    {
        SqlDataReader dr1 = DataAccess.DataMerchantAppDao.GetCorporateAccountsFamily(new Hashtable { { "@ZID", ZID } });
        List<dynamic> li = new List<dynamic>();
        while (dr1.Read())
        {
            li.Add(new
            {
                MerchantAppID = dr1["MerchantAppID"],
                BusinessDBAName = dr1["BusinessDBAName"],
                MerchantAppUID = dr1["MerchantAppUID"],
                IsParent = dr1["IsParent"]
            });
        }
        dr1.Close();

        grdFamily.DataSource = li;
        grdFamily.DataBind();

        if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() != "CABA4669-7C78-484E-988B-DBCEAA5FC026") // coroporate
        {
            // note corporate, so hide 4th column (delete column)

            grdFamily.Columns[3].Visible = false;
        }
        else
        {
            grdFamily.Columns[3].Visible = true;
        }

        return li.Count;
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtZID.Text.Trim()))
        {
            MerchantApp app = (MerchantApp)UserSessions.CurrentMerchantApp;

            MerchantApp m = DataAccess.DataMerchantAppDao.GetMerchantApp(Convert.ToInt32(txtZID.Text));

            if (m == null)
            {
                this.Master.AddMessageError("Merchant does not exist.");
                return;
            }

            if (m.ParentID != 0)
            {
                this.Master.AddMessageError("Merchant already belongs to Corporate Account: " + m.BusinessDBAName);
                return;
            }


            DataAccess.DataMerchantAppDao.UpdateCorporateAccount(txtZID.Text, app.ID);
            txtZID.Text = string.Empty;
            this.LoadCorporateAccounts(Convert.ToInt32(app.ID));


        }

    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;

        string zid = lb.CommandArgument;

        MerchantApp app = (MerchantApp)UserSessions.CurrentMerchantApp;
        DataAccess.DataMerchantAppDao.UpdateCorporateAccount(zid, string.Empty);

        txtZID.Text = string.Empty;

        this.LoadCorporateAccounts(Convert.ToInt32(app.ID));
    }

    private void LoadProcessingMerchants()
    {
        this.grdProcessingMerchants.DataSource = DataMerchantApp.GetInstance().GetMCPMerchants(int.Parse(UserSessions.CurrentMerchantApp.ID));
        this.grdProcessingMerchants.DataBind();
    }

    protected void btnSearchProcessing_Click(object sender, EventArgs e)
    {
        if (ValidateProcessingSearch())
        {
            SearchProcessingMerchants();
        }
    }

    private void SearchProcessingMerchants()
    {
        Hashtable prms = new Hashtable();

        if (!string.IsNullOrEmpty(FMAID.Text))
            prms.Add("@FMAID", FMAID.Text);

        if (!string.IsNullOrEmpty(MerchantDBA.Text))
            prms.Add("@BusinessDBAName", MerchantDBA.Text);

        if (!string.IsNullOrEmpty(ZID.Text))
            prms.Add("@ZID", ZID.Text);

        if (!string.IsNullOrEmpty(LegalName.Text))
            prms.Add("@LegalName", LegalName.Text);

        if (!string.IsNullOrEmpty(MID.Text))
            prms.Add("@MID", MID.Text);

        DataTable tbl = DataMerchantAppPaging.GetMCProcessingMerchantsPaging(prms, 10, 0);
        this.grdProcMerchantResults.DataSource = tbl;
        this.grdProcMerchantResults.DataBind();

        this.lblRecordCount.Text = DataMerchantAppPaging.GetMCProcessingMerchantsPagingCount(prms, 10, 0) + " Processing Merchants Found";
        this.lblSearchError.Text = "";
    }

    private bool ValidateProcessingSearch()
    {
        bool valid = false;
        int iVal;
        long lVal;

        if (!string.IsNullOrWhiteSpace(this.ZID.Text))
        {
            if (!int.TryParse(this.ZID.Text, out iVal))
            {
                this.lblSearchError.Text = "ZID must be a number";
                return false;
            }

            valid = true;
        }

        if (!string.IsNullOrWhiteSpace(this.FMAID.Text))
        {
            if (!long.TryParse(this.FMAID.Text, out lVal))
            {
                this.lblSearchError.Text = "FMA must be a number";
                return false;
            }

            valid = true;
        }

        if (!string.IsNullOrWhiteSpace(this.MerchantDBA.Text))
        {
            valid = true;
        }

        if (!string.IsNullOrWhiteSpace(this.LegalName.Text))
        {
            valid = true;
        }

        if (!string.IsNullOrWhiteSpace(this.MID.Text))
        {
            valid = true;
        }

        if (!valid)
        {
            this.lblSearchError.Text = "Please enter a search criteria";
        }

        return valid;
    }

    protected void grdProcessingMerchants_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;

        if (e.CommandSource is LinkButton)
            lnk = (LinkButton)e.CommandSource;
        else
            return;

        GridViewRow row = (GridViewRow)(lnk.NamingContainer);
        string txtZid = row.Cells[1].Text;
        int zid;

        if (int.TryParse(txtZid, out zid))
        {
            //delete mcp merchant
            DataMerchantApp.GetInstance().RemoveMCPMerchant(zid, UserSessions.CurrentUser.UserName);

            this.lblAddMessage.Text = "MCP ZID " + zid + " removed";
            this.lblAddMessage.ForeColor = Color.Green;

            LoadProcessingMerchants();
        }
    }

    protected void lnkAddMCP_Click(object sender, EventArgs e)
    {
        int mcpZID;

        if (!int.TryParse(this.MCPZID.Text, out mcpZID))
        {
            this.lblAddMessage.Text = "MCP ZID must be a numeric value";
            this.lblAddMessage.ForeColor = Color.Red;
            return;
        }

        string error = DataMerchantApp.GetInstance().AddMCPMerchant(Convert.ToInt32(UserSessions.CurrentMerchantApp.ID), mcpZID, UserSessions.CurrentUser.UserName);

        if (!string.IsNullOrEmpty(error))
        {
            this.lblAddMessage.Text = error;
            this.lblAddMessage.ForeColor = Color.Red;
        }
        else
        {
            this.lblAddMessage.Text = "MCP ZID " + mcpZID + " added";
            this.lblAddMessage.ForeColor = Color.Green;
            this.MCPZID.Text = "";

            LoadProcessingMerchants();
        }
    }

    protected void EnableMCP_CheckedChanged(object sender, EventArgs e)
    {
        string error = DataMerchantApp.GetInstance().EnableMCP(int.Parse(UserSessions.CurrentMerchantApp.ID), this.EnableMCP.Checked, UserSessions.CurrentUser.UserName);

        if (!string.IsNullOrEmpty(error))
        {
            this.EnableMCP.Checked = !this.EnableMCP.Checked;
            this.lblAddMessage.Text = error;
            this.lblAddMessage.ForeColor = Color.Red;
        }
        else
        {
            if (this.EnableMCP.Checked)
            {
                UserSessions.CurrentMerchantApp.MCPIndicator = MCPIndicator.Settlement;
                this.lblAddMessage.Text = "Multi-Currency Processing enabled";
            }
            else
            {
                UserSessions.CurrentMerchantApp.MCPIndicator = MCPIndicator.None;
                this.lblAddMessage.Text = "Multi-Currency Processing disabled";
                LoadProcessingMerchants();
            }

            this.trSearchMCP.Visible = this.EnableMCP.Checked;
            this.lblAddMessage.ForeColor = Color.Green;
        }

        this.EnableMCP.Attributes.Add("onclick", "confirmEnableMCP()");
    }

    protected void grdProcessingMerchants_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (UserSessions.CurrentMerchantApp.MCPIndicator == MCPIndicator.Processing)
        {
            e.Row.Cells[5].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // fetching controls.
            HyperLink h1 = (HyperLink)e.Row.FindControl("hyp1");
            Label ldba = (Label)e.Row.FindControl("lDBA");

            // pulling out values
            string uid = DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();
            string dba = DataBinder.Eval(e.Row.DataItem, "BusinessDBAName").ToString();
            string szid = DataBinder.Eval(e.Row.DataItem, "ZID").ToString();

            // set the hyperlink
            h1.NavigateUrl = WebUtil.GetBaseUrl() + "SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + uid;

            // toggle between the link and the label.
            h1.Visible = (UserSessions.CurrentMerchantApp.ID != szid) ? true : false;
            ldba.Visible = !((UserSessions.CurrentMerchantApp.ID != szid) ? true : false);
        }
    }

    protected void grdFamily_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // fetching controls.
            LinkButton lb = (LinkButton)e.Row.FindControl("lnkRemove");
            HyperLink h1 = (HyperLink)e.Row.FindControl("hyp1");
            Label l1 = (Label)e.Row.FindControl("Label1");
            Label ldba = (Label)e.Row.FindControl("lDBA");

            // pulling out values
            bool isParent = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsParent"));
            string uid = DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();
            string dba = DataBinder.Eval(e.Row.DataItem, "BusinessDBAName").ToString();
            string szid = DataBinder.Eval(e.Row.DataItem, "MerchantAppID").ToString();

            // put a nice display if its parent or child
            l1.Text = (isParent) ? "Parent" : "Child";

            // set the hyperlink
            h1.NavigateUrl = WebUtil.GetBaseUrl() + "SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + uid;

            // toggle between the link and the label.
            h1.Visible = (UserSessions.CurrentMerchantApp.ID != szid) ? true : false;
            ldba.Visible = !((UserSessions.CurrentMerchantApp.ID != szid) ? true : false);


            // you are a parent. don't delete yourself.
            lb.Visible = !isParent;



        }
    }
}
