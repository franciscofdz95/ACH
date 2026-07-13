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

public partial class frmAllQueues : frmBaseSearch
{
    int a1, a5, a7, a8, a9, a10, a11;
    decimal a2, a6, a4, a3;

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!IsPostBack)
        {
            LookupTableHandler.LoadAgentQueues(Queues, true);
            StartDateTime.Value = DateTime.Now.ToString("MM/dd/yyyy");
            EndDateTime.Value = DateTime.Now.ToString("MM/dd/yyyy");
            LookupTableHandler.LoadHour(StartTime);
            LookupTableHandler.LoadHour(EndTime);
            EndTime.SelectedIndex = EndTime.Items.Count - 1;
            chkHour.Checked = false;
            this.Search();
        }
    }

    public void Search()
    {
        DataAgent data = DataAccess.DataAgentDao;
        DataSet ds = null;
        Hashtable prms = new Hashtable();

        prms.Add("@StartDate", Convert.ToDateTime(StartDateTime.Text.ToString() + ' ' + StartTime.SelectedItem.Text));
        prms.Add("@EndDate", Convert.ToDateTime(EndDateTime.Text.ToString() + ' ' + EndTime.SelectedItem.Text));
        prms.Add("@IsHour", chkHour.Checked);

        if (Queues.SelectedIndex > 0)
            prms.Add("@QueueName", Queues.SelectedItem.Value);

        ds = data.GetAllAgentsQueue(prms);

        grdQueue.DataSource = ds;
        grdQueue.DataBind();
        pnlgrd.Visible = (grdQueue.Rows.Count > 0);
    }

    protected void grdQueue_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                a1 = 0; a5 = 0; a7 = 0; a8 = 0; a9 = 0; a10 = 0; a11 = 0;
                a2 = 0.0M; a4 = 0.0M; a6 = 0.0M; a3 = 0.0M;
                break;

            case DataControlRowType.DataRow:
                a1 += int.Parse(e.Row.Cells[1].Text);
                a2 += decimal.Parse(e.Row.Cells[2].Text);
                a3 += decimal.Parse(e.Row.Cells[3].Text);
                a4 += decimal.Parse(e.Row.Cells[4].Text);
                a5 += int.Parse(e.Row.Cells[5].Text);
                a6 += decimal.Parse(e.Row.Cells[6].Text);
                a7 += int.Parse(e.Row.Cells[7].Text);
                a8 += int.Parse(e.Row.Cells[8].Text);
                a9 += int.Parse(e.Row.Cells[9].Text);
                a10 += int.Parse(e.Row.Cells[10].Text);
                a11 += int.Parse(e.Row.Cells[11].Text);
                break;

            case DataControlRowType.Footer:
                e.Row.Cells[0].Text = "Totals";
                e.Row.Cells[1].Text = a1.ToString();
                e.Row.Cells[2].Text = "";//a2.ToString();
                e.Row.Cells[3].Text = "";//a3.ToString();
                e.Row.Cells[4].Text = a4.ToString();
                e.Row.Cells[5].Text = a5.ToString();
                e.Row.Cells[6].Text = "";//a6.ToString();
                e.Row.Cells[7].Text = a7.ToString();
                e.Row.Cells[8].Text = a8.ToString();
                e.Row.Cells[9].Text = a9.ToString();
                e.Row.Cells[10].Text = a10.ToString();
                e.Row.Cells[11].Text = a11.ToString();
                break;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        StartDateTime.Value = DateTime.Now.ToString("MM/dd/yyyy");
        EndDateTime.Value = DateTime.Now.ToString("MM/dd/yyyy");
        LookupTableHandler.LoadHour(StartTime);
        LookupTableHandler.LoadHour(EndTime);
        EndTime.SelectedIndex = EndTime.Items.Count - 1;
        chkHour.Checked = false;
        grdQueue.DataSource = null;
        grdQueue.DataBind();
        pnlgrd.Visible = (grdQueue.Rows.Count > 0);
    }

    protected void chkHour_CheckedChanged(object sender, EventArgs e)
    {
        if (chkHour.Checked)
        {
            LookupTableHandler.LoadTime(StartTime);
            LookupTableHandler.LoadTime(EndTime);
        }
        else
        {
            LookupTableHandler.LoadHour(StartTime);
            LookupTableHandler.LoadHour(EndTime);
        }
        EndTime.SelectedIndex = EndTime.Items.Count - 1;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        Search();
        FormHandler.Export2Excel("SummaryForAllQueues.xls", grdQueue);
    }
}
