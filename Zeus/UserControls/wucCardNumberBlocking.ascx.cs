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
public partial class wucCardNumberBlocking : System.Web.UI.UserControl
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
    protected void btnAddCardNo_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(CardNumber.Text))
            return;


        try
        {
            CreditCardBlock block = new CreditCardBlock();

            block.MerchantID = UserSessions.CurrentMerchantApp.ID;
            block.UserUpdated = UserSessions.CurrentUser.UserName;
            block.CardNumber = CardNumber.Text;
           

            int rows = DataAccess.DataRiskDao.InsertCardNumberBlock(block);
        }
        //catch (Exception exc) 
        catch
        { 
        }

        LoadCardNumberBlocks();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.DeleteCardNumber();
        LoadCardNumberBlocks();
    }

    public void LoadCardNumberBlocks()
    {
        grd.DataBind();

        btnDelete.Visible = grd.Rows.Count > 0;

    }

    public void DeleteCardNumber()
    {
        try
        {
            DataRisk data = DataAccess.DataRiskDao;
            CreditCardTransaction block = new CreditCardTransaction();

            foreach (GridViewRow row in grd.Rows)
            {
                block = new CreditCardTransaction();

                bool perform = DataLayer.Field2Bool(((CheckBox)row.FindControl("chkDelete")).Checked);

                if (perform)
                {
                    block.TransID = Convert.ToInt64(grd.DataKeys[row.RowIndex].Values["CardNumberBlockID"].ToString());
                    block.MerchantID = UserSessions.CurrentMerchantApp.ID;
                    int rows = DataAccess.DataRiskDao.DeleteCardNumberBlock(block);
                }
            }

        }
        catch (Exception exc)
        {
            throw exc;
        }
    }
}
