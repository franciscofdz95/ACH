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
using System.Text;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

using Infragistics.WebUI.WebControls;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

public partial class SecureReports_frmRMOverviewReport : frmBaseSearch
{
    private List<string> _liDates = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        this._liDates = new List<string>();

        if (!Page.IsPostBack)
        {

            DateTime working_date = DateTime.Now;

            if (working_date.Day == 1)
            {
                DateStart.Value = working_date;
            }
            else
            {
                DateStart.Value = working_date.AddDays(-1 * working_date.Day + 1);
            }


            DateEnd.Value = working_date;
            this.Search(false);
        }


    }

    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        //GridView1.Columns[0].Visible = cbChart.Checked;
        //if (GridView1.Columns.Count > 1)
        //{
        //    GridView1.Columns[1].ItemStyle.Width = new Unit(300);
        //}
    }



    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {

            int colcount = e.Row.Cells.Count;

            for (int i = 1; i < colcount; i++)
            {
                e.Row.Cells[i].Style.Add("text-align", "right");

                string[] arr = e.Row.Cells[i].Text.Split(new char[] { '-' });

                if (arr.Length == 3)
                {
                    // daily
                    e.Row.Cells[i].Text = DateTime.Parse(e.Row.Cells[i].Text).ToString("MM/dd/yy");
                }
                else if( arr.Length == 2)
                {
                    // monthly
                    e.Row.Cells[i].Text = DateTime.Parse(e.Row.Cells[i].Text).ToString("MM/yyyy");
                }
                

                //e.Row.Cells[i].Style.Add("-webkit-transform", "rotate(-45deg)");
                //e.Row.Cells[i].Style.Add("-moz-transform", "rotate(-45deg)");
                //e.Row.Cells[i].Style.Add(" filter", "progid:DXImageTransform.Microsoft.BasicImage(rotation=5)");



            }

            e.Row.Cells[0].Style.Add("width", "300px");

            if (cbChart.Checked)
            {


                int max = e.Row.Cells.Count;

                for (int i = 0; i < max; i++)
                {
                    string val = CommonUtility.Util.if_s(e.Row.Cells[i].Text);

                    if (val == "&nbsp;" || val == "Report")
                    {
                        // skip these.
                    }
                    else
                    {
                        this._liDates.Add(CommonUtility.Util.if_s(e.Row.Cells[i].Text));
                    }



                }
            }



        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Style.Add("width", "400px");

            if (cbChart.Checked)
            {

                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                DataRowView drv = (DataRowView)e.Row.DataItem;

                int index_max = this._liDates.Count + 1;
                //int index_max = this._liDates.Count;
                List<string> liData = new List<string>();

                // start at 1
                for (int index = 1; index < index_max; index++)
                {
                    liData.Add(CommonUtility.Util.if_s(drv.Row[index], "0"));
                }

                img.ImageUrl = this.GetChartImage(liData, this._liDates);

                e.Row.Cells[0].Controls.Add(img);
            }

            int colcount = e.Row.Cells.Count;

            for (int i = 1; i < colcount; i++)
            {
                e.Row.Cells[i].Style.Add("text-align", "right");
            }

            Label lb = new Label();
            lb.Text = e.Row.Cells[0].Text.Substring(4);
            e.Row.Cells[0].Controls.Add(lb);


        }
    }

    private string GetChartImage(List<string> liData, List<string> liColumns)
    {

        int maxval = 0;

        foreach (string s in liData)
        {
            int i = CommonUtility.Util.if_i(s, 0);
            if (i > maxval)
            {
                maxval = i;
            }
        }

        Dictionary<string, string> di = new Dictionary<string, string>();

        di.Add("cht", "bvs"); // vertical charts
        di.Add("chxt", "x,y"); // x and y axis
        di.Add("chs", "400x125"); // dimension of image
        di.Add("chds", "0," + maxval.ToString()); // y-axis max height

        if (ddlDateGrouping.SelectedValue == "DateMonthly")
        {
            di.Add("chxl", "0:|" + CommonUtility.Util.implode(liColumns, "|")); // our x-axis labels
        }
        else
        {
            // turn it off because its too cluttered.
            di.Add("chxl", "0:|"); // our x-axis labels
        }

        di.Add("chd", "t:" + CommonUtility.Util.implode(liData, ",")); // our value list
        
        di.Add("chco", "76A4FB"); // bar colors
        di.Add("chbh", "a"); // auto spacing
        di.Add("chxr", "1,0," + maxval.ToString()); // y axis labels
        di.Add("chm", "N,000000,0,-1,11"); // i dont understand this, but it embedds the value in the chart.

        string url = "https://chart.googleapis.com/chart?" + CommonUtility.Util.DictToUrl(di);

        return url;

    }

    public override void Search(bool IsOnLoad)
    {
        SearchParameter app;

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }

        if (this.isDataValid())
        {

            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;

            Hashtable ht = new Hashtable();

            ht.Add("@StartDate", DateStart.Text);
            ht.Add("@EndDate", DateEnd.Text);
            ht.Add("@DateRange", ddlDateGrouping.SelectedValue);

            DataTable dt = DataMerchantAppPaging.GetReportRMOverview(ht);

            if (dt != null && dt.Rows.Count > 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }

            //pnlRecords.Visible = (GridView1.Rows.Count > 0);
            // pnlNoRecords.Visible = !(GridView1.Rows.Count > 0);
        }
    }

    private bool isDataValid()
    {
        if (!(DateStart.Value != null && DateStart.Value.ToString() != ""))
        {

            WucMessage1.AddMessageError("Start Date Required");
            return false;
        }

        if (!(DateEnd.Value != null && DateEnd.Value.ToString() != ""))
        {

            WucMessage1.AddMessageError("End Date Required");
            return false;
        }

        DateTime dtS = DateTime.Parse(DateStart.Text);
        DateTime dtE = DateTime.Parse(DateEnd.Text);

        if (dtS > dtE)
        {
            WucMessage1.AddMessageError("Begin Date must be less than End Date");
            return false;
        }

        // also check that it is less than 60 days.
        if (dtS.AddDays(60) < dtE && ddlDateGrouping.SelectedValue == "DateDaily")
        {
            WucMessage1.AddMessageError("If using Daily, then date range must be less than 60 days");
            return false;
        }

        return true;
    }

    private void FormClear()
    {
        this.SearchParameters = null;

        FormHandler.ClearAllControls(this);
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.SortOrder = string.Empty;
        //this.IsKeywordSearch = false;
        this.SearchParameters = null;
        this.CurrentPage = 1;
        this.PageSize = 500;
        this.Search(false);
        //this.GridView1.DataBind();
    }

    protected void lbSentToExcel_Click(object sender, EventArgs e)
    {
        GridView1.PageSize = 10000;
        Search(false);
        FormHandler.Export2Excel(string.Format("RM_OVERVIEW_{0}.xls", CommonUtility.Util.GetDateTimeStamp()), GridView1);
    }


    protected void ddlTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
    {

        switch (ddlTimeFrame.SelectedValue)
        {
            case "1":
                // last 30 days
                DateStart.Value = DateTime.Now.AddDays(-30);
                DateEnd.Value = DateTime.Now;

                break;

            case "2":
                // last 60 days
                DateStart.Value = DateTime.Now.AddDays(-60);
                DateEnd.Value = DateTime.Now;
                break;

            case "3":
                // last 90 days
                DateStart.Value = DateTime.Now.AddDays(-90);
                DateEnd.Value = DateTime.Now;

                ddlDateGrouping.SelectedValue = "DateMonthly";

                break;

            case "4":
                // last month
                DateTime dtLastMonth = DateTime.Now.AddMonths(-1);
                DateStart.Value = new DateTime(dtLastMonth.Year, dtLastMonth.Month, 1);
                DateEnd.Value = new DateTime(dtLastMonth.Year, dtLastMonth.Month, DateTime.Now.AddDays(DateTime.Now.Day * -1).Day);
                break;

            case "5":
                // month to date
                DateStart.Value = DateTime.Now.AddDays((DateTime.Now.Day * -1) + 1);
                DateEnd.Value = DateTime.Now;
                break;

            case "6":
                // last 6 months
                DateStart.Value = DateTime.Now.AddDays(-1 * DateTime.Now.Day + 1).AddMonths(-6);
                DateEnd.Value = DateTime.Now;
                ddlDateGrouping.SelectedValue = "DateMonthly";
                break;

            case "7":
                // last 12 months.
                DateStart.Value = DateTime.Now.AddDays(-1 * DateTime.Now.Day + 1).AddMonths(-12);
                DateEnd.Value = DateTime.Now;
                ddlDateGrouping.SelectedValue = "DateMonthly";
                break;

        }

        GridView1.DataBind();

    }

}
