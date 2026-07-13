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

public partial class wucSalesWeekly : wucBaseSearch
{
    decimal[] a = new decimal[16];
    int[] b = new int[16];

    public DateTime mStartDate
    {
        get { if (ViewState["mStartDate"] != null) return Convert.ToDateTime(ViewState["mStartDate"]); else return DateTime.Today; }
        set { ViewState["mStartDate"] = value; }
    }

    public DateTime mEndDate
    {
        get { if (ViewState["mEndDate"] != null) return Convert.ToDateTime(ViewState["mEndDate"]); else return DateTime.Today; }
        set { ViewState["mEndDate"] = value; }
    }


    public int mWeek
    {
        get
        {
            if (ViewState["Week"] != null)
                return Convert.ToInt32(ViewState["Week"]);
            else
                return 0;
        }
        set { ViewState["Week"] = value; }
    }

    public string mAgentUID
    {
        get
        {
            if (ViewState["AgentUID"] == null)
                return string.Empty;
            else
                return ViewState["AgentUID"].ToString();
        }
        set { ViewState["AgentUID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void LoadGrid(DateTime dt1, DateTime dt2, int Week, string AgentUID)
    {
        DataLead objLead = DataAccess.DataLeadDao;
        Hashtable prms = new Hashtable();

        prms.Add("@AgentUID", AgentUID);
        prms.Add("@StartDate", dt1);
        prms.Add("@WeekDate", dt2);

        mStartDate = dt1;
        mEndDate = dt2;
        mWeek = Week;
        mAgentUID = AgentUID;

        DataSet ds = objLead.GetInsideSalesWeeklyReport(prms);
        DataView dv = ds.Tables[0].DefaultView;

        //if (this.SortOrder == string.Empty)
        //    this.SortOrder = "Name";

        //dv.Sort = this.SortOrder + " " + ConvertSortDirectionToSql(this.SortDirectionSearch);

        grd.DataSource = dv;
        grd.DataBind();

        pnl.Visible = (grd.Rows.Count > 0);
        lblData.Visible = (grd.Rows.Count == 0);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        string fileName = "WeeklyReport_" + mStartDate.ToString("MMM dd") + " to " + mEndDate.ToString("MMM dd") + ".xls";
        FormHandler.Export2Excel(fileName, grd);
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int m = 0;
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                //TableCell oCell2 = new TableCell();
                //oCell2.ColumnSpan = 2;
                //oCell2.BackColor = System.Drawing.Color.FromName("#e5e5e5");
                //oCell2.Text += "<td align='center' colspan='8' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Outgoing Calls</td><td align='center' colspan='8' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Statements Received</td></tr>";
                //e.Row.Cells.AddAt(0, oCell2);

                for (m = 0; m < 16; m++)
                    a[m] = 0.0M;

                int i = 0, j = 0;
                i = 2 + ((int)mStartDate.DayOfWeek - 1);
                int k = i - 1;

                for (j = 1; k > 1; k--, j++)
                {
                    e.Row.Cells[k].Text += "<br>(" + mStartDate.AddDays(j * -1).ToString("MM/dd") + ")";
                    e.Row.Cells[k + 8].Text += "<br>(" + mStartDate.AddDays(j * -1).ToString("MM/dd") + ")";
                }

                for (j = 0; i < 7; i++, j++)
                {
                    e.Row.Cells[i].Text += "<br>(" + mStartDate.AddDays(j).ToString("MM/dd") + ")";
                    e.Row.Cells[i + 8].Text += "<br>(" + mStartDate.AddDays(j).ToString("MM/dd") + ")";
                }

                break;

            case DataControlRowType.DataRow:

                for (m = 0; m < 16; m++)
                {
                    a[m] += Decimal.Parse(e.Row.Cells[m + 2].Text);
                    if (Decimal.Parse(e.Row.Cells[m + 2].Text) > 0.0M)
                        b[m] += 1;
                }

                break;

            case DataControlRowType.Footer:

                int cnt = ((GridView)sender).Rows.Count;

                if (cnt > 0)
                {
                    string cnt1 = "0.0";
                    e.Row.Cells.Clear();
                    TableCell oCell1 = new TableCell();
                    oCell1.ColumnSpan = 1;
                    oCell1.Text = "Avg Per Rep</td><td></td>";
                    for (m = 0; m < 16; m++)
                    {
                        if (m < 8)
                            cnt1 = (b[m] > 0) ? String.Format("{0:0.0}", a[m] / b[m]) : "0.0";
                        else
                            cnt1 = String.Format("{0:0.0}", a[m] / cnt);

                        oCell1.Text += "<td align='right'>" + cnt1 + "</td>";
                    }

                    oCell1.Text += "</tr>";

                    oCell1.Text += "<tr class='footer'><td style='width:100px;'>Totals</td><td></td>";
                    for (m = 0; m < 16; m++)
                        oCell1.Text += "<td align='right'>" + a[m].ToString("0") + "</td>";
                    oCell1.Text += "</tr>";

                    e.Row.Cells.Add(oCell1);
                }
                break;
        }
    }

    private string ConvertSortDirectionToSql(SortDirection direction)
    {
        string newSortDirection;

        switch (direction)
        {
            case SortDirection.Descending:
                newSortDirection = "DESC";
                this.SortDirectionSearch = SortDirection.Descending;
                break;

            default:
                newSortDirection = "ASC";
                this.SortDirectionSearch = SortDirection.Ascending;
                break;
        }
        return newSortDirection;
    }

    protected void grd_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Build custom header.
            GridView oGridView = (GridView)sender;
            GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell oTableCell = new TableCell();

            //Add empty
            oTableCell.Text = "";
            oTableCell.ColumnSpan = 2;
            oTableCell.BorderColor = System.Drawing.Color.FromArgb(212, 208, 200);
            oTableCell.BorderWidth = new Unit("1px");
            oTableCell.BorderStyle = BorderStyle.Solid;
            oTableCell.BackColor = System.Drawing.Color.FromName("#e5e5e5");
            oTableCell.HorizontalAlign = HorizontalAlign.Center;
            oGridViewRow.Cells.Add(oTableCell);

            //Add Count
            oTableCell = new TableCell();
            oTableCell.Text = "Outgoing Calls";
            oTableCell.BorderColor = System.Drawing.Color.FromArgb(212, 208, 200);
            oTableCell.BorderWidth = new Unit("1px");
            oTableCell.BorderStyle = BorderStyle.Solid;
            oTableCell.BackColor = System.Drawing.Color.FromName("#e5e5e5");
            oTableCell.Font.Size = FontUnit.XSmall;
            oTableCell.Font.Name = "Verdana";
            oTableCell.Font.Bold = true;
            oTableCell.ColumnSpan = 8;
            oTableCell.HorizontalAlign = HorizontalAlign.Center;
            oGridViewRow.Cells.Add(oTableCell);
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);

            //Add Amount
            oTableCell = new TableCell();
            oTableCell.Text = "Statements";
            oTableCell.BorderColor = System.Drawing.Color.FromArgb(212, 208, 200);
            oTableCell.BorderWidth = new Unit("1px");
            oTableCell.BorderStyle = BorderStyle.Solid;
            oTableCell.Font.Size = FontUnit.XSmall;
            oTableCell.Font.Name = "Verdana";
            oTableCell.Font.Bold = true;
            oTableCell.BackColor = System.Drawing.Color.FromName("#e5e5e5");
            oTableCell.ColumnSpan = 8;
            oTableCell.HorizontalAlign = HorizontalAlign.Center;
            oGridViewRow.Cells.Add(oTableCell);
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);            
        }
    }

    //protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    if (e.SortExpression != this.SortOrder)
    //        this.SortDirectionSearch = e.SortDirection;

    //    this.SortOrder = e.SortExpression;

    //    LoadGrid(mStartDate, mEndDate, mWeek, mAgentUID);
    //}
}
