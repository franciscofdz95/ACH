using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using System.Data;

public partial class frmProducts : frmBaseDataEntry
{
    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        this.FormShow("");
    }

    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "~/SecureReports/frmProductManagement.aspx?Adding=true";

        Response.Redirect(url);
    }

    #endregion


    #region Implement frmBaseDataEntry

    public override void FormShow(string ID)
    {
        List<Product> productList = PaymentXP.DataObjects.DataProduct.GetAllProducts(true);

        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ProductID", typeof(System.Int32)));
        dt.Columns.Add(new DataColumn("ProductName", typeof(System.String)));
        dt.Columns.Add(new DataColumn("PortalList", typeof(System.String)));
        dt.Columns.Add(new DataColumn("DateCreated", typeof(System.DateTime)));
        dt.Columns.Add(new DataColumn("IsVisibleOnPrivateLabel", typeof(System.String)));
        dt.Columns.Add(new DataColumn("IsActive", typeof(System.String)));
        dt.Columns.Add(new DataColumn("CreateTicket",typeof(System.String)));
        
        DataRow row = null;        
        foreach (var x in productList)
        {
            row = dt.NewRow();
            row["ProductId"] = x.ProductId;
            row["PortalList"] = BuildPortalList(x);
            row["ProductName"] = x.Name;
            row["DateCreated"] = x.DateCreated;
            row["IsVisibleOnPrivateLabel"] = (x.IsVisibleOnPrivateLabel) ? "Yes" : "No";
            row["IsActive"] = (x.IsActive) ? "Yes" : "No";
            row["CreateTicket"] = (x.CreateTicket) ? "Yes" : "No"; 
            dt.Rows.Add(row);
        }

        ProductGrid.DataSource = dt;
        ProductGrid.DataBind();
    }

    private string BuildPortalList(Product x)
    {
        string portalList = string.Empty;
        if (x.IsVisibleOnPaymentXP) { portalList += ", Payment XP"; }
        if (x.IsVisibleOnMerchantPortal) { portalList += ", Insight"; }
        if (x.IsVisibleOnAgentPortal) { portalList += ", Apex"; }
        if (x.IsVisibleOnZeus) { portalList += ", Zeus"; }
        if (x.IsVisibleOnApplicationXP) { portalList += ", ApplicationXP"; }

        return (portalList.Length > 1) ? portalList.Substring(2) : "None";
    }

    public override void FormClear()
    {
        throw new NotImplementedException();
    }

    public override bool FormSave()
    {
        throw new NotImplementedException();
    }

    public override void FormNew()
    {
        throw new NotImplementedException();
    }

    public override bool FormDelete()
    {
        throw new NotImplementedException();
    }

    public override bool FormDataCheck()
    {
        throw new NotImplementedException();
    }

    public override void FormCancel()
    {
        throw new NotImplementedException();
    }

    public override void ToggleButtons()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Helper Methods

    #endregion
}