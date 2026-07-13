using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

public partial class frmMerchantChainIDManagement : frmBaseSearch
{

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!IsPostBack)
        {
            LoadChainIDs();
            CurrentExp = "ChainID";
            CurrentSort = SortDirection.Descending;

            WucSelectMerchant1.WebDialogWindowClientID = this.WebDialogWindow2.ClientID;
            WucSelectMerchant1.HookTableDBAClientID = "";
            WucSelectMerchant1.HookTableIDClientID = ZID.ClientID;
            WucSelectMerchant1.HookTableUIDClientID = HookTableKeyUID.ClientID;
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                HtmlAnchor anc1 = ((HtmlAnchor)e.Row.FindControl("lnk1"));
                HtmlAnchor anc2 = ((HtmlAnchor)e.Row.FindControl("lnk2"));

                anc1.Attributes.Add("onclick", "return OpenCloseHeader('" + anc1.ClientID + "','" + anc2.ClientID + "','Open');");
                anc2.Attributes.Add("onclick", "return OpenCloseHeader('" + anc1.ClientID + "','" + anc2.ClientID + "','Close');");

                break;

            case DataControlRowType.DataRow:

                string MerchantIDs = DataBinder.Eval(e.Row.DataItem, "MerchantIDs").ToString();

                e.Row.Cells[2].Attributes.Add("title", Server.HtmlDecode(MerchantIDs).Trim());

                Label Notes1 = ((Label)e.Row.Cells[2].FindControl("Notes1"));
                Label Notes2 = ((Label)e.Row.Cells[2].FindControl("Notes2"));

                HtmlAnchor anchor1 = ((HtmlAnchor)e.Row.FindControl("lnk1"));
                HtmlAnchor anchor2 = ((HtmlAnchor)e.Row.FindControl("lnk2"));

                if (MerchantIDs.Length > 45)
                {
                    Notes1.Text = MerchantIDs.Substring(0, 45).Trim() + "...  ";
                    anchor1.Attributes.Add("style", "display:inline;cursor: pointer;");
                    anchor2.Style.Add("display", "none");
                }
                else
                {
                    Notes1.Text = MerchantIDs.Trim() + "  ";
                    anchor1.Style.Add("display", "none");
                    anchor2.Style.Add("display", "none");
                }

                Notes2.Text = MerchantIDs;

                anchor1.Attributes.Add("onclick", "return OpenClose('" + Notes1.ClientID + "','" + Notes2.ClientID + "','" + anchor1.ClientID + "','" + anchor2.ClientID + "','Open');");
                anchor2.Attributes.Add("onclick", "return OpenClose('" + Notes1.ClientID + "','" + Notes2.ClientID + "','" + anchor1.ClientID + "','" + anchor2.ClientID + "','Close');");

                break;

            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
    }
    
    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        LoadChainIDs();
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        LoadChainIDs();
        SortGrid(e.SortExpression);
    }

    protected void SortGrid(string SortExpression)
    {
        DataTable dataTable = ((DataView)grd.DataSource).Table;
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            if (CurrentExp != SortExpression)
                CurrentSort = SortDirection.Descending;
            dataView.Sort = SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
            grd.DataSource = dataView;
            grd.DataBind();
            CurrentExp = SortExpression;
        }
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grd.PageSize = this.PageSize;
        LoadChainIDs();
    }

    public void LoadChainIDs()
    {
        DataTable ds = new DataTable();
        Hashtable prms = new Hashtable();

        if (!string.IsNullOrEmpty(txtChainID.Text))
            prms.Add("@ChainID", txtChainID.Text);

        if (!string.IsNullOrEmpty(txtZID.Text))
            prms.Add("@MerchantID", txtZID.Text);

        if (!string.IsNullOrEmpty(txtDesc.Text))
            prms.Add("@ChainDescription", txtDesc.Text);

        if (prms.Count > 0)
        {
            prms.Add("@PageSize", this.PageSize);
            prms.Add("@CurrentPage", this.CurrentPage);
            
            grd.PageSize = this.PageSize;
            grd.PageIndex = this.CurrentPage - 1;
                 
            ds = DataMerchantAppPaging.GetMerchantChainPaging(prms,0,0,this.grd.ID);

            DataView dv = ds.DefaultView;

            grd.PageSize = this.PageSize;
            grd.PageIndex = this.CurrentPage - 1;

            dv.Sort = CurrentExp + " " + GetSortDirectionToSql(CurrentSort);

            grd.DataSource = dv;
            grd.DataBind();

            int recordcnt = DataMerchantAppPaging.GetMerchantChainPagingRowCount(prms, 0, 0, this.grd.ID);
            lblRecordCount.Text = "Total Records Found: " + recordcnt.ToString();
            lblData.Visible = (recordcnt == 0);
            pnlRecords.Visible = (recordcnt > 0);

        }

    }

    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;
        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "DESC";
                CurrentSort = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                newSortDirection = "ASC";
                CurrentSort = SortDirection.Ascending;
                break;
        }
        return newSortDirection;
    }

    private string GetSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;
        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "ASC";
                break;
            case SortDirection.Descending:
                newSortDirection = "DESC";
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
                return SortDirection.Descending;
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
                return "ChainID";
            }
            return ViewState["sortExp"].ToString();
        }
        set { ViewState["sortExp"] = value; }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {          

            int chainId = 0;

            Control btn1 = (Control)e.CommandSource;
            GridViewRow grdRow = ((GridViewRow)btn1.NamingContainer);
            chainId = CommonUtility.Util.if_i(grd.DataKeys[grdRow.RowIndex].Values["ChainID"], 0);


            switch(e.CommandName)
            {
                case "ChainID":

                    chainId = CommonUtility.Util.if_i(chainId, 0);

                    ChainID.Text = chainId.ToString();
                    ChainDescription.Text = e.CommandArgument.ToString();

                    LoadMerchants(chainId);

                    break;

                case "EditID":

                    ChainID1.Value = chainId.ToString();
                    ChainDescription1.Text = e.CommandArgument.ToString();

                    LoadMerchants(chainId);
                    pnlNew.Visible = true;
                    pnlDetails.Visible = false;

                    break;
        }

    }

    private void LoadMerchants(int chainid)
    {
        Hashtable prms = new Hashtable();

        prms.Add("@ChainID", DataLayer.Int2Field(chainid));

        DataSet ds = DataAccess.DataMerchantAppDao.GetChainID(prms);

        lstAccounts.Items.Clear();

        if (ds.Tables.Count > 1)
        {
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[1].Rows[i];
                ListItem item = new ListItem(dr["BusinessDBAName"].ToString(), dr["MerchantID"].ToString());
                item.Attributes.Add("title", dr["MerchantID"].ToString());
                lstAccounts.Items.Add(item);
                
            }
        }

        pnlDetails.Visible = true;
        pnlNew.Visible = false;
    }

    protected void btnAddChain_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        ChainID1.Value = string.Empty;
        ChainDescription1.Text = string.Empty;
        pnlNew.Visible = true;
        pnlDetails.Visible = false;
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        pnlDetails.Visible = pnlNew.Visible = false;
        CurrentSort = SortDirection.Descending;
        CurrentExp = "ChainID";
        LoadChainIDs();
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        FormHandler.ClearAllControls(pnlSearch);
        pnlRecords.Visible = false;
        lblData.Visible = true;
        pnlDetails.Visible = pnlNew.Visible = false;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int chainID = 0;

        if (!string.IsNullOrWhiteSpace(ChainDescription1.Text))
        {
            pnlNew.Visible = false;
            pnlDetails.Visible = false;

            if (!string.IsNullOrWhiteSpace(ChainID1.Value))
            {
                chainID = CommonUtility.Util.if_i(ChainID1.Value,0);

                if (chainID > 0)
                {
                    DataAccess.DataMerchantAppDao.InsertChainId(chainID, ChainDescription1.Text, UserSessions.CurrentUser.UserName);
                    LoadChainIDs();
                    ChainDescription1.Text = string.Empty;
                    WucMessage1.AddMessageSuccess("Chain ID " + chainID + " is updated.");
                }
                else
                {
                    WucMessage1.AddMessageError("Please select a valid Chain ID.");
                }

            }
            else
            {
                chainID = DataAccess.DataMerchantAppDao.InsertChainId(0,ChainDescription1.Text, UserSessions.CurrentUser.UserName);
            
            if (chainID > 0)
            {
                LoadChainIDs();
                ChainDescription1.Text = string.Empty;
                WucMessage1.AddMessageSuccess("New Chain ID " + chainID + " is added.");
            }
}
        }
        else
        {
             WucMessage1.AddMessageError("Please enter the Chain Description.");
            return;
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlNew.Visible = false;
        ChainDescription1.Text = string.Empty;
    }

    protected void btnAddZID_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(ZID.Text))
        {
            Hashtable prms = new Hashtable();
            int ID = CommonUtility.Util.if_i(ZID.Text, 0);

            if (ID > 0)
            {
                MerchantApp app = DataAccess.DataMerchantAppDao.GetMerchantApp(ID);

                if (app != null)
                {
                    if (app.ChainID > 0)
                    {
                        WucMessage1.AddMessageError("ZID " + ID.ToString() + " already associated with a Chain ID " + ChainID.Text + ".");
                        return;
                    }
                    else
                    {
                        int id = DataAccess.DataMerchantAppDao.InsertMerchantChain(CommonUtility.Util.if_i(ZID.Text, 0), CommonUtility.Util.if_i(ChainID.Text, 0), UserSessions.CurrentUser.UserName);
                        
                        if (id > 0)
                        {
                            this.LoadMerchants(CommonUtility.Util.if_i(ChainID.Text, 0));
                            LoadChainIDs();
                            WucMessage1.AddMessageSuccess("ZID " + ID.ToString() + " is now associated with Chain ID " + ChainID.Text + ".");
                        }
                    }
                }
                else
                {
                    WucMessage1.AddMessageError("Invalid ZID "+ID.ToString()+".");
                    return;       
                }

               ZID.Text = string.Empty;                 
            }
            else
            {
                WucMessage1.AddMessageError("Please enter a valid ZID.");
            }
        }
        else
            WucMessage1.AddMessageError("Please enter ZID or use the Lookup link to select a merchant.");

    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        if (lstAccounts.SelectedIndex != -1)
        {

            int ZID = CommonUtility.Util.if_i(lstAccounts.SelectedValue, 0);
            int chainID = CommonUtility.Util.if_i(ChainID.Text, 0);

            if(ZID > 0 && chainID > 0)
                DataAccess.DataMerchantAppDao.DeleteMerchantChainID(chainID, ZID);

            txtZID.Text = string.Empty;
            this.LoadMerchants(chainID);
            LoadChainIDs();

            WucMessage1.AddMessageSuccess("ZID " + ZID + " is removed from the Chain ID " + ChainID.Text + ".");
        }
        else
        {
            WucMessage1.AddMessageError("Please select a merchant to remove");
        }

    }


}