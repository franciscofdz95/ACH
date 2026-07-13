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

public partial class frmAllAgentsSummary : frmBaseSearch
{
    decimal a3, a4, a5, a8, a9, a7, a6;
    int a2, a10, a11, a12, a13, a14;

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!IsPostBack)
        {
            StartDateTime.Value = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy");
            EndDateTime.Value = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy");
            LookupTableHandler.LoadAllDepts(Dept, true);
            Dept.SelectedIndex = 0;
            grd.DataBind();
            pnlgrd.Visible = !(grd.Rows.Count == 0);
            lblData.Visible = (grd.Rows.Count == 0);
            
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                a2 = 0;
                a3 = 0.0M;
                a4 = 0.0M;
                a5 = 0.0M;
                a6 = 0.0M;
                a7 = 0.0M;
                a8 = 0.0M;
                a9 = 0.0M;
                a10 = 0;
                a11 = 0;
                a12 = 0;
                a13 = 0;
                a14 = 0;

                e.Row.Cells[0].Width = Unit.Pixel(130);
                e.Row.Cells[1].Width = Unit.Pixel(30);
                e.Row.Cells[4].Text += " (sec)";
                e.Row.Cells[5].Text += " (sec)";
                e.Row.Cells[6].Text += " (sec)";
                e.Row.Cells[7].Text += " (sec)";
                e.Row.Cells[8].Text += " (sec)";
                e.Row.Cells[9].Text += " (sec)";

                break;

            case DataControlRowType.DataRow:

                a2 += int.Parse(e.Row.Cells[2].Text);
                a3 += Decimal.Parse(e.Row.Cells[3].Text);
                a4 += Decimal.Parse(e.Row.Cells[4].Text);
                a5 += Decimal.Parse(e.Row.Cells[5].Text);
                a6 += decimal.Parse(e.Row.Cells[6].Text);
                a7 += decimal.Parse(e.Row.Cells[7].Text);
                a8 += decimal.Parse(e.Row.Cells[8].Text);
                a9 += decimal.Parse(e.Row.Cells[9].Text);
                a10 += int.Parse(e.Row.Cells[10].Text);
                a11 += int.Parse(e.Row.Cells[11].Text);
                a12 += int.Parse(e.Row.Cells[12].Text);
                a13 += int.Parse(e.Row.Cells[13].Text);
                a14 += int.Parse(e.Row.Cells[14].Text);
                break;

            case DataControlRowType.Footer:

                e.Row.Cells[0].Text = "Totals";
                e.Row.Cells[2].Text = a2.ToString("n0");
                e.Row.Cells[3].Text = a3.ToString("n0");
                e.Row.Cells[4].Text = "";//a4.ToString();
                e.Row.Cells[5].Text = "";// a5.ToString();
                e.Row.Cells[6].Text = "";//a6.ToString("n0");
                e.Row.Cells[7].Text = "";//a7.ToString("n0");
                e.Row.Cells[8].Text = a8.ToString("n0");
                e.Row.Cells[9].Text = a9.ToString("n0");
                e.Row.Cells[10].Text = a10.ToString("n0");
                e.Row.Cells[11].Text = a11.ToString("n0");
                e.Row.Cells[12].Text = a12.ToString("n0");
                e.Row.Cells[13].Text = a13.ToString("n0");
                e.Row.Cells[14].Text = a14.ToString("n0");

                break;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grd.DataBind();
        lblData.Visible = (grd.Rows.Count == 0);
        pnlgrd.Visible = (grd.Rows.Count > 0);
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        StartDateTime.Value = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy");
        EndDateTime.Value = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy");
        Dept.SelectedIndex = 0;
        grd.DataSource = null;
        grd.DataBind();
        lblData.Visible = (grd.Rows.Count == 0);
    }

    protected void odsReports_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();

        prms.Add("@StartDate", Convert.ToDateTime(StartDateTime.Text.ToString()));
        prms.Add("@EndDate", Convert.ToDateTime(EndDateTime.Text.ToString()));

        if (Dept.SelectedIndex > 0)
            prms.Add("@Dept", Dept.SelectedValue);
        e.InputParameters[0] = prms;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        grd.DataBind();
        pnlgrd.Visible = !(grd.Rows.Count == 0);
        lblData.Visible = (grd.Rows.Count == 0);
        FormHandler.Export2Excel("SummaryByQueues.xls", grd);
    }
}
