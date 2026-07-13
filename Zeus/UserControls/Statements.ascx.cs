using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ucontrol_Statements : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

        }
    }

    public void LoadStatements(string settlePlatformMid, string rootPath, string aspxPage, string dba = "")
    {

        if (Directory.Exists(rootPath))
        {
            CommonUtility.Crypto crypto = new CommonUtility.Crypto();

            DataTable dt = this.CreateStatementsTable();

            string[] years = Directory.GetDirectories(rootPath);

            for (int i = years.Length - 1; i >= 0; i--)
            {
                string[] arrYear = years[i].Split(new char[] { '\\' });

                if (CommonUtility.Util.IsValidInt32(arrYear[arrYear.Length - 1]))
                {
                    int myyear = CommonUtility.Util.if_i(arrYear[arrYear.Length - 1], 0);

                    if (myyear > 0)
                    {

                        string[] months = Directory.GetDirectories(years[i]);

                        DataRow dr = null;

                        for (int j = months.Length - 1; j >= 0; j--)
                        {
                            
                            if (File.Exists(months[j] + "\\" + settlePlatformMid + ".pdf"))
                            {
                                dr = dt.NewRow();

                                dr["Year"] = years[i].Substring(years[i].LastIndexOf("\\") + 1);
                                dr["Month"] = this.GetMonths(months[j].Substring(months[j].LastIndexOf("\\") + 1));
                                dr["View"] = aspxPage + "?a=" + HttpUtility.UrlEncode(crypto.Encrypt(settlePlatformMid)) + "&m=" + HttpUtility.UrlEncode(crypto.Encrypt(months[j].Substring(months[j].LastIndexOf("\\") + 1))) + "&y=" + HttpUtility.UrlEncode(crypto.Encrypt(dr["Year"].ToString()));

                                if (!string.IsNullOrEmpty(dba))
                                {
                                    dr["View"] += "&d=" + HttpUtility.UrlEncode(crypto.Encrypt(dba));
                                }

                                dt.Rows.Add(dr);
                            }
                        }
                    }
                }
            }

            this.grdStatements.DataSource = dt;
            this.grdStatements.DataBind();
        }
        else
        {
            // error: contact admin
        }
    }

    protected void grdStatements_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink link = (HyperLink)e.Row.Cells[2].Controls[0];

            if (link.Text.ToUpper() == "NOT AVAILABLE")
            {
                link.NavigateUrl = string.Empty;
                link.Text = "Not Available";
            }
            else
            {
                link.Text = "View";
            }
        }
    }

    private string GetMonths(string month)
    {
        switch (month)
        {
            case "01": return "JAN";
            case "02": return "FEB";
            case "03": return "MAR";
            case "04": return "APR";
            case "05": return "MAY";
            case "06": return "JUN";
            case "07": return "JUL";
            case "08": return "AUG";
            case "09": return "SEP";
            case "10": return "OCT";
            case "11": return "NOV";
            case "12": return "DEC";
            default: return string.Empty;
        }
    }

    private DataTable CreateStatementsTable()
    {
        DataTable dt = new DataTable("MerchantStatements");

        DataColumn dc = new DataColumn();
        dc.DataType = System.Type.GetType("System.Int32");
        dc.ColumnName = "Year";
        dt.Columns.Add(dc);

        dc = new DataColumn();
        dc.DataType = System.Type.GetType("System.String");
        dc.ColumnName = "Month";
        dt.Columns.Add(dc);

        dc = new DataColumn();
        dc.DataType = System.Type.GetType("System.String");
        dc.ColumnName = "View";
        dt.Columns.Add(dc);

        return dt;
    }
}
