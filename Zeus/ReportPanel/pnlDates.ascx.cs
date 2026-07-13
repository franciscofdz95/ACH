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

public partial class pnlDates : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            DateTime date = DateTime.Today;
            DateTime date2 = DateTime.Today.AddMonths(1);

            StartDate.Value = Convert.ToDateTime(date.Month.ToString() + "/1/" + date.Year.ToString());
            EndDate.Value = Convert.ToDateTime(date2.Month.ToString() + "/1/" + date2.Year.ToString()).AddDays(-1);
        }
    }
}
