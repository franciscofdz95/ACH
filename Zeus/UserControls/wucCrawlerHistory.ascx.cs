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

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
public partial class wucCrawlerHistory : System.Web.UI.UserControl
{
    public delegate void GridRowCommandHandler(object sender, GridViewCommandEventArgs e);
    public event GridRowCommandHandler GridRowCommand;

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void odsCrawlers_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        e.InputParameters[0] = prms;
    }

    

    public void LoadHistory()
    {
        grd.DataBind();

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
                int statusID = DataLayer.Field2Int(DataBinder.Eval(e.Row.DataItem, "StatusID"));

                HyperLink url = (HyperLink)e.Row.FindControl("lnkURL");
                url.Text = DataBinder.Eval(e.Row.DataItem, "URL").ToString();
                url.NavigateUrl = DataBinder.Eval(e.Row.DataItem, "URL").ToString();

                HyperLink url2 = (HyperLink)e.Row.FindControl("lnkScreenShot");
                url2.NavigateUrl = ConfigurationManager.AppSettings["MDoc_Output"] + @"\" + DataBinder.Eval(e.Row.DataItem, "MerchantID").ToString() + @"\" + DataBinder.Eval(e.Row.DataItem, "ScreenShotName").ToString() + ".pdf";
                url2.Visible = statusID != 0;

                HyperLink url3 = (HyperLink)e.Row.FindControl("lnkReport");
                url3.NavigateUrl = ConfigurationManager.AppSettings["MDoc_Output"] + @"\" + DataBinder.Eval(e.Row.DataItem, "MerchantID").ToString() + @"\" + DataBinder.Eval(e.Row.DataItem, "ReportName").ToString() + ".pdf";
                url3.Visible = statusID != 0;

                HyperLink url4 = (HyperLink)e.Row.FindControl("lnkHTML");
                url4.NavigateUrl = ConfigurationManager.AppSettings["MDoc_Output"] + @"\" + DataBinder.Eval(e.Row.DataItem, "MerchantID").ToString() + @"\" + DataBinder.Eval(e.Row.DataItem, "HTMLName").ToString() + ".html";
                url4.Visible = statusID != 0;

                Button btnEnbaled = (Button)e.Row.FindControl("btnEnbaled");

                btnEnbaled.CommandArgument = DataBinder.Eval(e.Row.DataItem, "CrawlerID").ToString();

                if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "ApprovedContent").ToString()))
                {
                    btnEnbaled.Text = "Active";
                    btnEnbaled.Enabled = false;
                }
                else
                {
                    btnEnbaled.Text = "Enable";
                    btnEnbaled.Enabled = true;
                }

                break;
            default:
                break;
        }
    }
    
    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        //LinkButton lnk = null;

        //if (e.CommandSource is Button)
        //    btn = (LinkButton)e.CommandSource;
        //else
        //    return;

        //e.CommandArgument.ToString();
        //WebsiteURL url = new WebsiteURL();

        //bool active = btn.Text == "Enable";

        //url.URLID = e.CommandArgument.ToString();
        //url.MerchantID = UserSessions.CurrentMerchantApp.ID;
        //url.UserUpdated = UserSessions.CurrentUser.UserName;
        //url.Enabled = active;
        //int rows = DataAccess.DataRiskDao.UpdateWebsiteURL(url);

        //LoadHistory();

        if (this.GridRowCommand != null)
        {
            this.GridRowCommand(grd, e);
        }
    }
    
}
