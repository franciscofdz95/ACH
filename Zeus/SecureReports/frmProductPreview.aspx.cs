using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Data;

public partial class frmProductPreview : System.Web.UI.Page
{
    #region Properties

    private List<PaymentXP.BusinessObjects.Subscription> SubscriptionList = null;

    private int ProductId = int.MinValue;
    private Guid AgentUID = Guid.Empty;

    #endregion

    #region Implementing frmBaseDataEntry

    public void FormShow(string ID)
    {
        List<Product> productList = DataProduct.GetAgentProductList(AgentUID);

        Product product = productList.FirstOrDefault<Product>(x => x.ProductId == this.ProductId);

        if (product == null) { return; }

        this.ProductName.Text = product.Name;
        this.MarketingContent.Text = product.MarketingContent;

        if (product.FeeList != null)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("FeeName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("MerchantCost", typeof(System.Decimal)));

            DataRow row = null;
            foreach (var x in product.FeeList)
            {
                row = dt.NewRow();
                row["FeeName"] = x.Name;
                row["MerchantCost"] = x.MerchantCost;
                dt.Rows.Add(row);
            }

            FeeGrid.DataSource = dt;
            FeeGrid.DataBind();
        }
        else
        {
            FeePanel.Visible = false;
            return;
        }
    }

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        this.SetProductId();

        FeePanel.Visible = true;

        if (!Page.IsPostBack)
        {
            this.FormShow("");
        }
    }

    #endregion

    #region Helper Methods

    private void SetProductId()
    {
        if (Request.QueryString["Id"] != null)
        {
            int.TryParse(Request.QueryString["Id"], out ProductId);
        }

        if (Request.QueryString["AgentUID"] != null)
        {
            Guid.TryParse(Request.QueryString["AgentUID"], out AgentUID);
        }
    }

    #endregion
}