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

public partial class wucSalesDetailedWeekly : System.Web.UI.UserControl
{
    decimal[] a = new decimal[42];
    //a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a23, a24,
    //a25, a26, a27, a28, a29, a30, a31, a32, a33, a34, a35, a36, a37, a38, a39, a40, a41, a42, a43, a44;

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

    protected void Page_Load(object sender, EventArgs e)
    {
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
                //oCell2.Text += "<td align='center' colspan='8' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Applications Submitted</td><td align='center' colspan='8' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Applications Approved</td><td align='center' colspan='8' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Running Profit</td><td align='center' colspan='8' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Monthly Profit</td><td align='center' colspan='2' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Approval Averages</td></tr>";
                //e.Row.Cells.AddAt(0, oCell2);

                for (m = 0; m < 34; m++)
                    a[m] = 0.0M;

                int i = 0, j = 0;
                i = 2 + ((int)mStartDate.DayOfWeek - 1);
                int k = i - 1;

                for (j = 1; k > 1; k--, j++)
                {
                    e.Row.Cells[k].Text += "(" + mStartDate.AddDays(j * -1).ToString("MM/dd") + ")";
                    e.Row.Cells[k + 8].Text += "(" + mStartDate.AddDays(j * -1).ToString("MM/dd") + ")";
                    e.Row.Cells[k + 16].Text += "(" + mStartDate.AddDays(j * -1).ToString("MM/dd") + ")";
                    e.Row.Cells[k + 24].Text += "(" + mStartDate.AddDays(j * -1).ToString("MM/dd") + ")";
                    // e.Row.Cells[k + 32].Text += "(" + mStartDate.AddDays(j * -1).ToString("MM/dd") + ")";
                }

                for (j = 0; i < 7; i++, j++)
                {
                    e.Row.Cells[i].Text += "(" + mStartDate.AddDays(j).ToString("MM/dd") + ")";
                    e.Row.Cells[i + 8].Text += "(" + mStartDate.AddDays(j).ToString("MM/dd") + ")";
                    e.Row.Cells[i + 16].Text += "(" + mStartDate.AddDays(j).ToString("MM/dd") + ")";
                    e.Row.Cells[i + 24].Text += "(" + mStartDate.AddDays(j).ToString("MM/dd") + ")";
                    //e.Row.Cells[i + 32].Text += "(" + mStartDate.AddDays(j).ToString("MM/dd") + ")";
                }

                break;

            case DataControlRowType.DataRow:

                for (m = 0; m < 34; m++)
                    if (e.Row.Cells[m + 2].Text != string.Empty)
                        a[m] += Decimal.Parse(e.Row.Cells[m + 2].Text.Replace("$", "").Replace("(", "-").Replace(")", ""));

                break;

            case DataControlRowType.Footer:

                int cnt = ((GridView)sender).Rows.Count;

                if (cnt > 0)
                {
                    e.Row.Cells.Clear();
                    TableCell oCell1 = new TableCell();
                    oCell1.ColumnSpan = 1;

                    oCell1.Text = "Avg Per Rep</td><td></td>";

                    for (m = 0; m < 16; m++)
                        oCell1.Text += "<td align='right'>" + String.Format("{0:0}", a[m] / cnt) + "</td>";

                    for (m = 16; m < 34; m++)
                        oCell1.Text += "</td><td align='right'>" + String.Format("{0:0.00}", a[m] / cnt) + "</td>";

                    oCell1.Text += "<tr class='footer'><td>Totals</td><td></td>";

                    for (m = 0; m < 16; m++)
                        oCell1.Text += "<td align='right'>" + a[m].ToString("0") + "</td>";

                    for (m = 16; m < 34; m++)
                        oCell1.Text += "<td align='right'>" + a[m].ToString("0.00")/*"c")*/ + "</td>";

                    oCell1.Text += "</tr>";
                    e.Row.Cells.Add(oCell1);

                }
                break;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        string fileName = "DetailedWeeklyReport_" + mStartDate.ToString("MMM dd") + " to " + mEndDate.ToString("MMM dd") + ".xls";
        FormHandler.Export2Excel(fileName, grd);
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

        grd.DataSource = objLead.GetInsideSalesWeeklyAppReport(prms);
        grd.DataBind();

        pnl.Visible = (grd.Rows.Count > 0);
        lblData.Visible = (grd.Rows.Count == 0);
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

            //Add Submitted
            oTableCell = new TableCell();
            oTableCell.Text = "Applications Submitted";
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

            //Add Approved
            oTableCell = new TableCell();
            oTableCell.Text = "Applications Approved";
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


            //Add Running Profit
            oTableCell = new TableCell();
            oTableCell.Text = "Running Profit";
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

            //Add Monthly Profit
            oTableCell = new TableCell();
            oTableCell.Text = "Monthly Profit";
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

            //Add Approval Averages
            oTableCell = new TableCell();
            oTableCell.Text = "Approval Averages";
            oTableCell.BorderColor = System.Drawing.Color.FromArgb(212, 208, 200);
            oTableCell.BorderWidth = new Unit("1px");
            oTableCell.BorderStyle = BorderStyle.Solid;
            oTableCell.Font.Size = FontUnit.XSmall;
            oTableCell.Font.Name = "Verdana";
            oTableCell.Font.Bold = true;
            oTableCell.BackColor = System.Drawing.Color.FromName("#e5e5e5");
            oTableCell.ColumnSpan = 2;
            oTableCell.HorizontalAlign = HorizontalAlign.Center;
            oGridViewRow.Cells.Add(oTableCell);
            oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);

        }
    }

}
