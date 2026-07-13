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
public partial class wucIPBlocking : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void odsIPBlocking_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        e.InputParameters[0] = prms;
    }
    protected void btnAddIP_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(A1.Text) || string.IsNullOrEmpty(A2.Text) || string.IsNullOrEmpty(A3.Text) || string.IsNullOrEmpty(A4.Text))
            return;

        try
        {
            IPBlock block = new IPBlock();

            block.MerchantID = UserSessions.CurrentMerchantApp.ID;
            block.UserUpdated = UserSessions.CurrentUser.UserName;
            block.A1 = A1.Text;
            block.A2 = A2.Text;
            block.A3 = A3.Text;
            block.A4 = A4.Text;

            int rows = DataAccess.DataRiskDao.InsertIPBlock(block);
        }
        //catch (Exception exc) { }
        catch { }

        LoadIPBlocks();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.DeleteIPAddresses();
        LoadIPBlocks();
    }

    public void LoadIPBlocks()
    {
        grd.DataBind();
        btnDelete.Visible = grd.Rows.Count > 0;
    }

    public void DeleteIPAddresses()
    {
        try
        {
            DataRisk data = DataAccess.DataRiskDao;
            IPBlock block = new IPBlock();

            foreach (GridViewRow row in grd.Rows)
            {
                block = new IPBlock();

                bool perform = DataLayer.Field2Bool(((CheckBox)row.FindControl("chkDelete")).Checked);

                if (perform)
                {
                    block.IPBlockID = grd.DataKeys[row.RowIndex].Values["IPBlockID"].ToString();
                    block.MerchantID = UserSessions.CurrentMerchantApp.ID;
                    int rows = DataAccess.DataRiskDao.DeleteIPBlock(block);
                }
            }

        }
        catch (Exception exc)
        {
            throw exc;
        }
    }
}
