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

public partial class frmWeeklySummaryReport : frmBaseSearch
{
   

    protected void Page_Load(object sender, EventArgs e)
    {
        ((HyperLink)this.Master.FindControl("lnkLeadReports")).CssClass = "active";

        if (!IsPostBack)
        {
            LoadDate();
            LoadSalesSummary();
        }
    }

    protected void grdCat_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string AgentUID = string.Empty; int Row;
        DateTime StartDate = new DateTime();
        DateTime EndDate = new DateTime();

        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                break;
            case DataControlRowType.DataRow:

                wucSalesWeekly SalesWeekly1 = (wucSalesWeekly)e.Row.Cells[0].FindControl("SalesWeekly1");

                StartDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "StartDate"));
                EndDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "EndDate"));
                Row = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Row"));
                AgentUID = DataBinder.Eval(e.Row.DataItem, "AgentUID").ToString();

                SalesWeekly1.LoadGrid(StartDate, EndDate, Row, AgentUID);
                ((Label)e.Row.Cells[0].FindControl("lblName")).Text = SetLabel(StartDate, EndDate, Row);

                ((HtmlImage)e.Row.FindControl("img1")).Attributes.Add("onclick", "CollapseExpand('" + ((HtmlGenericControl)e.Row.FindControl("div1")).ClientID + "',null,'" + ((HtmlImage)e.Row.FindControl("img1")).ClientID + "')");

                break;
            default:
                break;
        }
    }

    public string SetLabel(DateTime dt1, DateTime dt2, int Week)
    {
        return "Week" + Week + " (" + dt1.ToString("MM/dd") + " - " + dt2.ToString("MM/dd") + ")";
    }

    public void LoadSalesSummary()
    {
        string str = string.Empty, AgentUID = string.Empty;
        DateTime StartDate = new DateTime();
        string[] str1 = Date.SelectedItem.Text.Split('/');
        int days = System.DateTime.DaysInMonth(Convert.ToInt32(str1[1]), Convert.ToInt32(str1[0]));
        int i = 1, j = 1, k = 5;

        DataTable dt = new DataTable();
        dt.Columns.Add("StartDate", Type.GetType("System.DateTime"));
        dt.Columns.Add("EndDate", Type.GetType("System.DateTime"));
        dt.Columns.Add("Row", Type.GetType("System.Int32"));
        dt.Columns.Add("AgentUID", Type.GetType("System.String"));

        DataRow dr;

        if (wucAgentSelector.m_AgentUID != string.Empty)
        {
            AgentUID = wucAgentSelector.m_AgentUID;

            str = Date.SelectedItem.Text;
            str = str.Insert(str.IndexOf("/"), "/" + i.ToString());
            StartDate = Convert.ToDateTime(str);

            k = (int)StartDate.DayOfWeek;
            i = 9 - k;

            if (k == 0)
            {
                dr = dt.NewRow();
                dr[0] = StartDate.AddDays(1);
                dr[1] = StartDate.AddDays(7);
                dr[2] = j;
                dr[3] = AgentUID;

                dt.Rows.Add(dr);
            }
            //SalesWeekly1.LoadGrid(StartDate.AddDays(1), StartDate.AddDays(7), j, AgentUID);


            if (k < 6 && k > 0)
            {
                dr = dt.NewRow();
                dr[0] = StartDate;
                dr[1] = StartDate.AddDays(6 - k);
                dr[2] = j;
                dr[3] = AgentUID;

                dt.Rows.Add(dr);
            }
            // SalesWeekly1.LoadGrid(StartDate, StartDate.AddDays(7 - k), j, AgentUID);

            for (j = 2; i + 6 <= days; i = i + 7, j++)
            {
                str = Date.SelectedItem.Text;
                str = str.Insert(str.IndexOf("/"), "/" + i.ToString());
                StartDate = Convert.ToDateTime(str);

                dr = dt.NewRow();

                dr[0] = StartDate;
                dr[1] = StartDate.AddDays(6);
                dr[2] = j;
                dr[3] = AgentUID;

                dt.Rows.Add(dr);
            }

            if (i <= days)
            {
                str = Date.SelectedItem.Text;
                str = str.Insert(str.IndexOf("/"), "/" + i.ToString());
                StartDate = Convert.ToDateTime(str);
                // SalesWeekly5.LoadGrid(StartDate, StartDate.AddDays(days - i), j, AgentUID);

                dr = dt.NewRow();
                dr[0] = StartDate;
                dr[1] = StartDate.AddDays(days - i);
                dr[2] = j;
                dr[3] = AgentUID;
                dt.Rows.Add(dr);
            }
        }

        pnlSales.Visible = dt.Rows.Count > 0;
        lblData.Visible = dt.Rows.Count == 0;

        grdCat.DataSource = dt;
        grdCat.DataBind();
    }

    private void LoadDate()
    {
        DateTime dt = DateTime.Today;
        int i = 1;
        Date.Items.Clear();
        while (i < 13)
        {
            string month = dt.Month.ToString();
            string year = dt.Year.ToString();
            Date.Items.Add(new ListItem(month + "/" + year, dt.ToShortDateString()));
            dt = dt.AddMonths(-1);
            i++;
        }
        Date.SelectedIndex = 0;
    }

    //protected void AgentAgentID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    LoadSalesSummary();
    //}

    //protected void Date_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    LoadSalesSummary();
    //}

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadSalesSummary();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        FormHandler.ClearAllControls(pnlSearch);
        wucAgentSelector.FormClear();
        LoadSalesSummary();
    }
}
