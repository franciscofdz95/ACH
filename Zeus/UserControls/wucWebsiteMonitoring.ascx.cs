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
using System.IO;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
public partial class wucWebsiteMonitoring : System.Web.UI.UserControl
{
    //public delegate void GridRowCommandHandler(object sender, GridViewCommandEventArgs e);
    //public event GridRowCommandHandler GridRowCommand;

    public int CurrentPage
    {
        get
        {
            if (ViewState["CurrentPage"] == null)
                return 1;
            else
                return (int)ViewState["CurrentPage"];
        }
        set { ViewState["CurrentPage"] = value; }
    }

    public int PageSize
    {
        get
        {
            if (ViewState["PageSize"] == null)
                return 10;
            else
                return (int)ViewState["PageSize"];
        }
        set { ViewState["PageSize"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
            IDictionary<string, WebsiteURL> list = DataAccess.DataRiskDao.GetWebsiteURLCol(prms);

            lstURLs.Items.Clear();
            lstURLs.Items.Add(new ListItem("All", string.Empty));
            foreach (KeyValuePair<string, WebsiteURL> kvp in list)
            {
                lstURLs.Items.Add(new ListItem(kvp.Key.ToLower(), kvp.Value.URLID));
            }

        }
    }
    protected void odsWebsiteURLs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        e.InputParameters[0] = prms;
    }
    protected void btnAddURL_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(URL.Text))
            return;

        if (this.URLTypeID.SelectedItem.Value == "1")
        {	

            Hashtable prms = new Hashtable();
            prms.Add("@MerchantID",UserSessions.CurrentMerchantApp.ID);
            prms.Add("@Enabled",1);
            prms.Add("@URLTypeID","1");
            IDictionary <string,WebsiteURL> list = DataAccess.DataRiskDao.GetWebsiteURLCol(prms);

            if (list.Count > 0)
            {
                CustomValidator1.IsValid = false;
                CustomValidator1.ErrorMessage = "Cannot add more than 1 active Terms and Conditions URL.";
                return;
            }
        }
        try
        {
            WebsiteURL url = new WebsiteURL();

            url.MerchantID = UserSessions.CurrentMerchantApp.ID;
            url.UserUpdated = UserSessions.CurrentUser.UserName;
            url.URL = URL.Text;
            url.URLTypeID = this.URLTypeID.SelectedItem.Value;
            url.Enabled = true;
            url.CrawlURL = CrawlURL.Checked;
            url.CheckTC = CheckTC.Checked;
            int rows = DataAccess.DataRiskDao.InsertWebsiteURL(url);
        }
        //catch (Exception exc) { }
        catch { }

        LoadWebsiteURLs();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.DeleteWebsiteURL();
        LoadWebsiteURLs();
    }

    public void LoadWebsiteURLs()
    {
        grd.DataBind();

        //btnDelete.Visible = grd.Rows.Count > 0;

        LoadHistory();

    }

    public void DeleteWebsiteURL()
    {
        try
        {
            WebsiteURL url = new WebsiteURL();

            foreach (GridViewRow row in grd.Rows)
            {
                url = new WebsiteURL();

                bool perform = DataLayer.Field2Bool(((CheckBox)row.FindControl("chkDelete")).Checked);

                if (perform)
                {
                    url.URLID = grd.DataKeys[row.RowIndex].Values["URLID"].ToString();
                    url.MerchantID = UserSessions.CurrentMerchantApp.ID;
                    int rows = DataAccess.DataRiskDao.DeleteWebsiteURL(url);
                }
            }

        }
        catch (Exception exc)
        {
            throw exc;
        }
    }
    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                Button btnOn = (Button)e.Row.FindControl("btnOn");
                Button btnOff = (Button)e.Row.FindControl("btnOff");
                Button btnCrawler = (Button)e.Row.FindControl("btnCrawler");
                CheckBox chkCrawlURL = (CheckBox)e.Row.FindControl("chkCrawlURL");
                CheckBox chkCheckTC = (CheckBox)e.Row.FindControl("chkCheckTC");

                btnOn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "URLID").ToString() + "," + DataBinder.Eval(e.Row.DataItem, "URL").ToString();
                btnOff.CommandArgument = btnOn.CommandArgument;
                btnCrawler.CommandArgument = btnOn.CommandArgument;
                chkCrawlURL.Checked = DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "CrawlURL"));
                chkCheckTC.Checked = DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "CheckTC"));

                if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Enabled").ToString()))
                {
                    btnOn.Text = "Approved";
                    btnOn.Enabled = false;
                    btnOff.Enabled = true;
                    btnOff.Text = "Deny";
                    URLReferrer.Text = DataBinder.Eval(e.Row.DataItem, "URL").ToString();
                }
                else
                {
                    btnOn.Text = "Approve";
                    btnOn.Enabled = true;
                    btnOff.Text = "Denied";
                    btnOff.Enabled = false;
                }

                break;
            default:
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        Button btn = null;

        if (e.CommandSource is Button)
            btn = (Button)e.CommandSource;
        else
            return;

        string[] str = e.CommandArgument.ToString().Split(new char[] { ',' });

        switch (btn.Text)
        {
            case "Approve":
            case "Deny":
                WebsiteURL url = new WebsiteURL();

                bool active = btn.Text == "Approve";

                url.URLID = str[0];
                url.MerchantID = UserSessions.CurrentMerchantApp.ID;
                url.UserUpdated = UserSessions.CurrentUser.UserName;
                url.Enabled = active;
                int rows = DataAccess.DataRiskDao.UpdateWebsiteURL(url);
                break;
            case "Create Crawler Request":
                int r = DataAccess.DataRiskDao.InsertCrawler(UserSessions.CurrentMerchantApp.ID
                        , str[0], str[1], UserSessions.CurrentUser.UserName);

                break;
        }

        LoadWebsiteURLs();

    }


    protected void grd2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        grd.DataBind();
    }

    protected void odsCrawlers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        if (lstURLs.SelectedIndex > 0)
            prms.Add("@URLID", lstURLs.SelectedItem.Value);

        prms.Add("@PageSize", grd2.PageSize);

        prms.Add("@CurrentPage", this.CurrentPage);


        e.InputParameters[0] = prms;
    }



    public void LoadHistory()
    {
        grd2.DataBind();

    }

    //public void DeleteWebsiteURL()
    //{
    //    try
    //    {
    //        WebsiteURL url = new WebsiteURL();

    //        foreach (GridViewRow row in grd2.Rows)
    //        {
    //            url = new WebsiteURL();

    //            bool perform = DataLayer.Field2Bool(((CheckBox)row.FindControl("chkDelete")).Checked);

    //            if (perform)
    //            {
    //                url.URLID = grd2.DataKeys[row.RowIndex].Values["URLID"].ToString();
    //                url.MerchantID = UserSessions.CurrentMerchantApp.ID;
    //                int rows = DataAccess.DataRiskDao.DeleteWebsiteURL(url);
    //            }
    //        }

    //    }
    //    catch (Exception exc)
    //    {
    //        throw exc;
    //    }
    //}
    protected void grd2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                int statusID = DataLayer.Field2Int(DataBinder.Eval(e.Row.DataItem, "StatusID"));
                string crawlerID = DataBinder.Eval(e.Row.DataItem, "CrawlerID").ToString();
                string urlid = DataBinder.Eval(e.Row.DataItem, "URLID").ToString();
                string merchantid = DataBinder.Eval(e.Row.DataItem, "MerchantID").ToString();
                string activecrawlerid = DataBinder.Eval(e.Row.DataItem, "ActiveCrawlerID").ToString();
                                                                          
                HyperLink url = (HyperLink)e.Row.FindControl("lnkURL");
                url.Text = DataBinder.Eval(e.Row.DataItem, "URL").ToString();
                url.NavigateUrl = DataBinder.Eval(e.Row.DataItem, "URL").ToString();

                string path = ConfigurationManager.AppSettings["MDoc_Crawler_Output"] + merchantid + @"\" + crawlerID + @"\";
                string file = path + DataBinder.Eval(e.Row.DataItem, "ScreenShotName").ToString() + ".pdf";
                bool fileexists = false;

                fileexists = File.Exists(file);
                HyperLink url2 = (HyperLink)e.Row.FindControl("lnkScreenShot");
                url2.NavigateUrl = path + DataBinder.Eval(e.Row.DataItem, "ScreenShotName").ToString() + ".pdf";
                url2.Visible = fileexists && (statusID == 2 || statusID == 4); //completed

                file = path + DataBinder.Eval(e.Row.DataItem, "ReportName").ToString() + ".pdf";
                fileexists = File.Exists(file);
                HyperLink url3 = (HyperLink)e.Row.FindControl("lnkReport");
                url3.NavigateUrl = path + DataBinder.Eval(e.Row.DataItem, "ReportName").ToString() + ".pdf";
                url3.Visible = fileexists && (statusID == 2 || statusID == 4); //completed

                file = path + DataBinder.Eval(e.Row.DataItem, "HTMLName").ToString() + ".html";
                fileexists = File.Exists(file);
                HyperLink url4 = (HyperLink)e.Row.FindControl("lnkHTML");
                //url4.NavigateUrl = path + DataBinder.Eval(e.Row.DataItem, "HTMLName").ToString() + ".html";
                url4.NavigateUrl = "~/SecureMerchantManagementForms/frmCompare.aspx?CrawlerID=" + crawlerID;
                url4.Visible = fileexists && (statusID == 2 || statusID == 4) && activecrawlerid != "0"; //completed
                url4.Text = "Compare";

                Button btnEnbaled = (Button)e.Row.FindControl("btnEnbaled");

                btnEnbaled.CommandArgument = DataBinder.Eval(e.Row.DataItem, "CrawlerID").ToString() + "," + urlid;

                bool ApprovedContent = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "ApprovedContent").ToString());
                string MaxCrawlerID = DataBinder.Eval(e.Row.DataItem, "MaxCrawlerID").ToString();
                string ApprovedCrawlerID = DataBinder.Eval(e.Row.DataItem, "ApprovedCrawlerID").ToString();

                if (ApprovedContent)
                {
                    btnEnbaled.Text = "Approved";
                    btnEnbaled.Enabled = false;
                }
                else
                {
                    btnEnbaled.Text = "Enable";

                    if (crawlerID == MaxCrawlerID && (statusID == 2 || (statusID == 4 && ApprovedCrawlerID == "0")))
                        btnEnbaled.Enabled = true;
                    else
                        btnEnbaled.Enabled = false;
                }

                break;
            default:
                break;
        }
    }

    protected void grd2_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        Button btn = null;

        if (e.CommandSource is Button)
            btn = (Button)e.CommandSource;
        else
            return;

        string[] str = e.CommandArgument.ToString().Split(new char[] { ',' });

        switch (btn.Text)
        {
            case "Enable":
            case "Approved":
                bool active = btn.Text == "Enable";

                int rows = DataAccess.DataRiskDao.UpdateApprovedContent(str[0], str[1], UserSessions.CurrentMerchantApp.ID, active, UserSessions.CurrentUser.UserName);

                if (rows == -1)
                {
                    CustomValidator2.IsValid = false;
                    CustomValidator2.ErrorMessage = "Please approve Website URL prior to enabling content.";
                }


                break;
        }
        LoadHistory();


    }

    protected void lstURLs_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadHistory();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        LoadHistory();
    }
    protected void chkCheckTC_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string urlID = string.Empty;
            //bool checktc = false;

            WebsiteURL url = new WebsiteURL();


            CheckBox chk = (CheckBox)sender;

            GridViewRow grdRow = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int rowIndex = grdRow.RowIndex;
            url.URLID = grd.DataKeys[rowIndex].Values["URLID"].ToString();

            url.MerchantID = UserSessions.CurrentMerchantApp.ID;
            url.UserUpdated = UserSessions.CurrentUser.UserName;
            url.CheckTC = chk.Checked;

            int rows = DataAccess.DataRiskDao.UpdateWebsiteURLCheckTCStatus(url);

        }
        catch (Exception exc)
        {
            throw exc;
        }

    }


    protected void chkCrawlURL_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string urlID = string.Empty;
            //bool crawlURL = false;

            WebsiteURL url = new WebsiteURL();

            CheckBox chk = (CheckBox)sender;

            GridViewRow grdRow = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int rowIndex = grdRow.RowIndex;
            url.URLID = grd.DataKeys[rowIndex].Values["URLID"].ToString();

            url.MerchantID = UserSessions.CurrentMerchantApp.ID;
            url.UserUpdated = UserSessions.CurrentUser.UserName;
            url.CrawlURL = chk.Checked;

            int rows = DataAccess.DataRiskDao.UpdateWebsiteURLCrawlURLStatus(url);

        }
        catch (Exception exc)
        {
            throw exc;
        }

    }


}
