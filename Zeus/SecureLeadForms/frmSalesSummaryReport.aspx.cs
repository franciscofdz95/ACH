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

public partial class frmSalesSummaryReport : frmBaseSearch
{
    decimal[] a = new decimal[12];
    decimal[] b = new decimal[12];

   
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageSales.eMasterSideMenu.Reports);

            //LookupTableHandler.LoadAgentsNew(AgentAgentID, false);
            //LoadDate();
        }

        //LoadSalesSummary();
    }

    public void LoadSalesSummary()
    {
        DataLead objLead = DataAccess.DataLeadDao;
        Hashtable prms = new Hashtable();

        if (wucAgentSelector.m_AgentUID == string.Empty)
            prms.Add("@AgentUID", "00000000-0000-0000-0000-000000000000");
        else
            prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);

        DateTime EndDate;
        if (SearchBeginDate.Value == null || SearchEndDate.Value == null)
        {
            string str = DateTime.Today.Month.ToString() + "/1/" + DateTime.Today.Year.ToString();
            SearchBeginDate.Value = Convert.ToDateTime(str);
            SearchEndDate.Value = DateTime.Today;
        }

        EndDate = DataLayer.Field2Date(SearchEndDate.Value).AddDays(1);

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@StartDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndDate", SearchEndDate.Value);

        if (prms.Count > 1)
        {
            DataSet ds = objLead.GetInsideSalesReport(prms);
            DataView dv = ds.Tables[0].DefaultView;

            if (this.SortOrder == string.Empty)
                this.SortOrder = "Name";
            dv.Sort = this.SortOrder + " " + ((SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch) == 1) ?  " Desc " : " Asc ");

            grd.DataSource = dv;
            grd.DataBind();
        }

        pnlGrd.Visible = !(grd.Rows.Count == 0);
        pnlBucketFooter.Visible = !(grd.Rows.Count == 0);
    }



    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                for (int i = 0; i < 12; i++)
                {
                    a[i] = 0.0M;
                    b[i] = 0.0M;
                }

                break;

            case DataControlRowType.DataRow:

                for (int j = 0; j < 12; j++)
                {
                    a[j] += Decimal.Parse(e.Row.Cells[j + 1].Text.Replace("$", ""));
                    if ((j > 0 && j <= 6) && Decimal.Parse(e.Row.Cells[j].Text) > 0.0M)
                        b[j - 1] += 1;
                }

                break;

            case DataControlRowType.Footer:

                int k = 0;
                string str = string.Empty;
                int cnt = ((GridView)sender).Rows.Count;

                if (cnt > 0)
                {
                    e.Row.Cells.Clear();
                    TableCell oCell1 = new TableCell();
                    oCell1.ColumnSpan = 1;
                    oCell1.Text = "Avg Per Rep</td>";

                    for (k = 0; k < 12; k++)
                    {
                        if ((k >= 6 && k <= 8))
                            str = String.Format("{0:0}", a[k] / grd.Rows.Count);
                        else if (k >= 9)
                            str = String.Format("{0:0.00}", a[k] / grd.Rows.Count);
                        else
                        {
                            str = (b[k] > 0) ? String.Format("{0:0.0}", a[k] / b[k]) : "0.0";
                            //str = String.Format("{0:0.0}", a[k] / cnt1);
                        }
                        oCell1.Text += "<td align='right'>" + str + "</td>";
                    }

                    oCell1.Text += "<tr class='footer'><td>Totals</td>";

                    for (k = 0; k < 12; k++)
                    {
                        if ((k >= 6 && k < 9) || (k == 2))
                            oCell1.Text += "<td align='right'>" + a[k].ToString("0") + "</td>";
                        else if (k >= 9)
                            oCell1.Text += "<td align='right'>" + a[k].ToString("0.00") + "</td>";//"c")
                        else
                            oCell1.Text += "<td align='right'>" + a[k].ToString("0.0") + "</td>";
                    }

                    oCell1.Text += "</tr>";

                    e.Row.Cells.Add(oCell1);
                }
                break;
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (this.SortOrder != e.SortExpression)
            this.SortDirectionSearch = e.SortDirection;

        this.SortOrder = e.SortExpression;
        LoadSalesSummary();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        //FormHandler.ClearAllControls(pnlSearch);
        //wucAgentSelector.FormClear();

        Response.Redirect("~/SecureLeadForms/frmSalesSummaryReport.aspx");
        
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadSalesSummary();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        LoadSalesSummary();
        FormHandler.Export2Excel("SalesSummaryReport.xls", grd);

    }

}
