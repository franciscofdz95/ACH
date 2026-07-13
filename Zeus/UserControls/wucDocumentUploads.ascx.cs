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


using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class UserControls_wucDocumentUploads : wucBaseSearch
{
    public List<string> OfficeIDList
    {
        get
        {
            if (ViewState["OfficeIDList"] == null)
                return new List<string>();
            else
                return (List<string>)ViewState["OfficeIDList"];
        }
        set { ViewState["OfficeID"] = value; }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.always_init();

        if (!Page.IsPostBack)
        {
            this.initialize();
        }


    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //UWConditionDocument obj = (UWConditionDocument)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hl = (HyperLink)e.Row.FindControl("hypFilename");

            Dictionary<string, string> di = new Dictionary<string,string>();

            di.Add("MerchantID", DataBinder.Eval(e.Row.DataItem,"MerchantID").ToString());//obj.MerchantID.ToString());
            di.Add("DocID", DataBinder.Eval(e.Row.DataItem, "DocID").ToString());//obj.DocID.ToString());

            string encrypted_value = Server.UrlEncode(CommonUtility.Crypto.EncryptUrl(di));

            hl.NavigateUrl = string.Format("~/SecureMerchantManagementForms/frmMerchantDocumentPreview.aspx?x={0}", encrypted_value);
            e.Row.Cells[3].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[3].Text);
        }
    }

    protected void initialize()
    {
        
    }

    protected void always_init()
    {

        bool has_access = false;

        UserRole role = null;

        if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_RISK, out role))
        {
            has_access = role.Enabled;
        }

        has_access = true;

        if (has_access == false)
        {
            this.Visible = false;
            return;
        }

        ////////////////////

        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);

        this.CurrentPage = 1;
        GridView1.PageIndex = 0;
        this.LoadDocuments();
    }


    public void LoadDocuments()
    {

        int document_rows = 0;

        Hashtable prms = new Hashtable();
        ods.FilterParameters.Clear();
        this.GridView1.PageSize = this.PageSize;

        prms.Add("@IsPending", true);

        prms.Add("@CurrentPage", this.CurrentPage);
        prms.Add("@PageSize", this.PageSize);

        List<string> li = new List<string>();
        li.Add("F5FAF4FE-A132-45F6-A854-8CCFB07AA8D9"); // ap pending
        li.Add("4358B3A7-9936-448B-BEE5-FC8DB48FB9FF"); // cu pending
        li.Add("4358B3A7-9936-448B-BEE5-FC8DB48FB9EE"); // cu pending banking
        prms.Add("@MerchantStatusUIDList", CommonUtility.Util.implode(li, ","));
        
        prms.Add("@UserID", UserSessions.CurrentUser.UserID);

        if (OfficeIDList.Count == 0)
            OfficeIDList.Add("0");

        prms.Add("@OfficeIDList", string.Join(",", OfficeIDList));
        
        foreach (string prm in prms.Keys)
        {
            if (prms[prm] == null)
            {
                ods.FilterParameters.Add(prm, null);
            }
            else
            {
                ods.FilterParameters.Add(prm, prms[prm].ToString());
            }
        }


        this.GridView1.DataBind();
        document_rows = DataMerchantAppPaging.GetUWConditionDocument_PagingCount(prms, this.PageSize, this.CurrentPage);

        this.pnlDocs.Visible = (document_rows > 0);
        this.pnlNone.Visible = !(document_rows > 0);

        lblRecordCount.Text = "Total Records Found: " + document_rows.ToString();

    }



    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.LoadDocuments();
    }

    protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        foreach (Parameter prm in ods.FilterParameters)
        {
            prms.Add(prm.Name, prm.DefaultValue);
        }
        e.InputParameters[0] = prms;
    }

    //private int ConvertSortDirectionToSql(SortDirection direction)
    //{
    //    int newSortDirection;

    //    switch (direction)
    //    {
    //        case SortDirection.Descending:
    //            newSortDirection = 1;
    //            this.SortDirectionSearch = SortDirection.Descending;
    //            break;

    //        default:
    //            newSortDirection = 0;
    //            this.SortDirectionSearch = SortDirection.Ascending;
    //            break;
    //    }

    //    return newSortDirection;


    //}

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        this.CurrentPage = e.NewPageIndex + 1;
        this.LoadDocuments();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        GridView1.PageIndex = 0;
        this.LoadDocuments();
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
            this.CurrentPage = this.GridView1.PageIndex + 1;
        }

        if (this.GridView1.SortExpression != "")
        {
            this.SortOrder = this.GridView1.SortExpression;
        }
        else
        {
            this.SortOrder = "StatusChangedDate";
        }
        this.GridView1.PageSize = this.PageSize;
        GridView1.DataBind();

         string FileName = this.GridView1.Parent.ClientID.ToString();
        string Pattern = "_pnl";
        string[] Result = Regex.Split(FileName, Pattern);
        FormHandler.Export2Excel(Result[1] + ".xls", GridView1);
    }



    //protected void lbMerchantID_Click(object sender, EventArgs e)
    //{
    //    LinkButton lb = (LinkButton)sender;

    //    string merchantappuid = "";

    //    if (lb.CommandName.Trim().ToLower() == "setmerchant")
    //    {
    //        merchantappuid = CommonUtility.Util.if_s(lb.CommandArgument, "");

    //        if (!string.IsNullOrEmpty(merchantappuid))
    //        {
    //            UserSessions.CurrentMerchantApp = DataMerchantApp.GetInstance().GetMerchantApp(merchantappuid);
    //            Response.Redirect("~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false&PostBackURL=~/SecureHomeForms/frmHome.aspx");
    //        }
    //    }
    //    else if (lb.CommandName.Trim().ToLower() == "setmerchantdba")
    //    {
    //        merchantappuid = CommonUtility.Util.if_s(lb.CommandArgument, "");

    //        if (!string.IsNullOrEmpty(merchantappuid))
    //        {
    //            UserSessions.CurrentMerchantApp = DataMerchantApp.GetInstance().GetMerchantApp(merchantappuid);
    //            Response.Redirect("~/SecureMerchantManagementForms/frmUnderwritingPending.aspx");
    //        }
    //    }

    //}
}
