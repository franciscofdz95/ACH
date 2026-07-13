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


public partial class frm45DaysInactivity : frmBaseSearch
{
 

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            SearchBeginDate.Value = DateTime.Today;

            
        }
    }

    public override void Search(bool IsOnLoad)
    {
        SearchParameter app;
        DataSet ds = null;

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }

        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@BeginPostedDate", SearchBeginDate.Value);


        

        if (prms.Count > 0)
        {
            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;

            ds = DataAccess.DataBankFilesDao.Get45DaysInactivityReport(prms);
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = this.SortOrder;

            lblRecordCount.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();
        }

        grd.DataSource = ds;
        grd.DataBind();

        pnl1.Visible = (grd.Rows.Count > 0);
        NoData.Visible = !(grd.Rows.Count > 0);
        lblTitle.Text = "Results";
    }

   

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
        this.SearchParameters = null;
        this.Search(false);
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.SortOrder = string.Empty;
        grd.PageIndex = 1;
        this.SearchParameters = null;
        this.Search(false);
    }

    private void FormClear()
    {
        FormHandler.ClearAllControls(this);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("results.xls", grd);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        FormHandler.ExportToPDF(grd, false, "results");
    }

    
}
