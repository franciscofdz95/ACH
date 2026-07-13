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
using System.Globalization;

using Infragistics.WebUI.WebDataInput;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using Infragistics.Web.UI.EditorControls;

public partial class frmAchReturnReport : frmBaseSearch
{
    //Procedure below creates a new InitializeDataSource event handler for the leads grid
    override protected void OnInit(EventArgs e)
    {
        grdACHSales.GridViewCommand += new wucACHReturnGrid.GridViewRowCommandHandler(GridItemCommand);
        grdACHSales.GridTitle = "ACH Sales Return History";

        grdACHCredit.GridViewCommand += new wucACHReturnGrid.GridViewRowCommandHandler(GridItemCommand);
        grdACHCredit.GridTitle = "ACH Credit Return History";

        base.OnInit(e);
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadReturnReasons(ReasonCode, true);
            //FormHandler.ResetDataSessions();

            if (Request["StartDate"] != null)
            {
                SearchParameter prms = new SearchParameter();
                prms.SearchBeginDate = Request["StartDate"].ToString();
                prms.SearchEndDate = Request["EndDate"].ToString();

                this.SearchParameters = prms;
            }
            else
            {
                DateTime dt = Convert.ToDateTime(DateTime.Today.ToString("MM/01/yyyy"));
                SearchBeginDate.Value = dt;
                SearchEndDate.Value = dt.AddMonths(1).AddDays(-1);
            }

            this.Search(true);
        }
    }

    public override void Search(bool IsOnLoad)
    {
        SearchParameter sp;

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            sp = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(sp, pnlSearch);
        }

        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        if (this.SearchParameters != null)
        {
            prms.Add("@BeginPostedDate", SearchBeginDate.Value);
            prms.Add("@EndPostedDate", SearchEndDate.Value);
            prms.Add("@TransTypeList", "26,36");

            //Save search fields in session variable
            if (TransID.Text != string.Empty)
                if (TransID.Text == "0")
                    TransID.Text = string.Empty;
                else
                    prms.Add("@TransID", Convert.ToInt64(TransID.Text));

            if (AccountNumber.Text != string.Empty)
                prms.Add("@AccountNo", AccountNumber.Text);

            if (AccountName.Text != string.Empty)
                prms.Add("@AccountName", AccountName.Text);

            if (ReferenceNumber.Text != string.Empty)
                prms.Add("@RefID", ReferenceNumber.Text);            

            if (Amount.Text != string.Empty)
                if (Amount.Text == "0")
                    Amount.Text = string.Empty;
                else
                    prms.Add("@Amount", Convert.ToDecimal(Amount.Text));

            if (ReturnType.SelectedIndex > 0)
                prms.Add("@ReturnType", ReturnType.SelectedItem.Value);

            if (ReasonCode.SelectedIndex > 0)
                prms.Add("@ReasonDesc", ReasonCode.SelectedItem.Value);

            sp = new SearchParameter();
            FormBinding.BindControlsToObject(sp, pnlSearch);
            this.SearchParameters = sp;

            this.LoadSales(prms);
            this.LoadCredits(prms);
        }
    }
    
    private void LoadSales(Hashtable prms)
    {
        AchTransactionFacade facade = new AchTransactionFacade();
        DataSet ds = facade.GetAchReturns(ConfigurationManager.AppSettings["AggregatorAchMerchantID"], prms);
        grdACHSales.SetDataSource(ds);
    }

    private void LoadCredits(Hashtable prms)
    {
        AchTransactionFacade facade = new AchTransactionFacade();
        prms["@TransTypeList"] = "21,31";
        DataSet ds = facade.GetAchReturns(ConfigurationManager.AppSettings["AggregatorAchMerchantID"], prms);
        grdACHCredit.SetDataSource(ds);
    }

    protected void Number_Init(object sender, EventArgs e)
    {
        WebNumericEditor txt = (WebNumericEditor)sender;
        NumberFormatInfo numInfo = new NumberFormatInfo();
        numInfo.NumberGroupSeparator = "";
        //marknguyen20120124txt.NumberFormat = numInfo;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.SearchParameters = new SearchParameter();
        this.Search(false);
    }

    private void GridItemCommand(object sender, GridViewCommandEventArgs e)
    {
        this.Search(false);
    }

    private void FormClear()
    {
        FormHandler.ClearAllControls(pnlSearch);
        this.SearchParameters = null;

        DateTime dt = Convert.ToDateTime(DateTime.Today.ToString("MM/01/yyyy"));
        SearchBeginDate.Value = dt;
        SearchEndDate.Value = dt.AddMonths(1).AddDays(-1);

        grdACHSales.ClearGrid();
        grdACHCredit.ClearGrid();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        this.FormClear();
    }
}
