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
public partial class wucTermsConditions : System.Web.UI.UserControl
{
   

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void odsTerms_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        e.InputParameters[0] = prms;
    }
    protected void btnAddTerms_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtTerms.Text) || string.IsNullOrEmpty(txtShortTerms.Text))
            return;

        try
        {
            TermsCondition terms = new TermsCondition();

            
            terms.MerchantID = UserSessions.CurrentMerchantApp.ID;
            terms.UserUpdated = UserSessions.CurrentUser.UserName;
            terms.Name = txtName.Text;
            terms.Description = txtTerms.Text;
            terms.ShortDescription = txtShortTerms.Text;

            int rows = DataAccess.DataRiskDao.InsertTerms(terms);

            if (rows > 0)
            {
                txtName.Text = string.Empty;
                txtTerms.Text = string.Empty;
                txtShortTerms.Text = string.Empty;
            }
        }
        //catch (Exception exc) { }
        catch { }

        LoadTerms();
    }
    //protected void btnDelete_Click(object sender, EventArgs e)
    //{
    //    this.DeleteTerms();
    //    LoadTerms();
    //}

    public void LoadTerms()
    {
        grd.DataBind();

        //btnDelete.Visible = grd.Rows.Count > 0;


    }

    //public void DeleteTerms()
    //{
    //    try
    //    {
    //        WebsiteURL url = new WebsiteURL();

    //        foreach (GridViewRow row in grd.Rows)
    //        {
    //            url = new WebsiteURL();

    //            bool perform = DataLayer.Field2Bool(((CheckBox)row.FindControl("chkDelete")).Checked);

    //            if (perform)
    //            {
    //                url.URLID = grd.DataKeys[row.RowIndex].Values["URLID"].ToString();
    //                url.MerchantID = UserSessions.CurrentMerchantApp.ID;
    //                int rows = DataAccess.DataRiskDao.DeleteWebsiteURL(url);
    //            }
    //        }

    //    }
    //    catch (Exception exc)
    //    {
    //        throw exc;
    //    }
    //}
    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                Button btnRecurring = (Button)e.Row.FindControl("btnRecurring");
                Button btnEnbaled = (Button)e.Row.FindControl("btnEnbaled");
                Button btnEnbaled2 = (Button)e.Row.FindControl("btnEnbaled2");
                LinkButton lnkName = (LinkButton)e.Row.FindControl("lnkName");

                lnkName.Text = DataBinder.Eval(e.Row.DataItem, "Name").ToString();
                lnkName.CommandArgument = DataBinder.Eval(e.Row.DataItem, "Description").ToString();

                btnEnbaled.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TCID").ToString();
                btnEnbaled2.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TCID").ToString();
                btnRecurring.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TCID").ToString();

                string OccurID = DataBinder.Eval(e.Row.DataItem, "OccurID").ToString();
                
                if (OccurID == "0")
                    btnRecurring.Text = "Add";
                else
                    btnRecurring.Text = "View";

                
                string status = DataBinder.Eval(e.Row.DataItem, "StatusID").ToString();

                switch (status)
                { 
                    case "0":
                        btnEnbaled.Text = "Approve";
                        btnEnbaled.Enabled = true;

                        btnEnbaled2.Text = "Deny";
                        btnEnbaled2.Visible = true;
                        break;
                    case "1":
                        btnEnbaled.Text = "Approve";
                        btnEnbaled.Enabled = true;
                        btnEnbaled2.Visible = false;
                        break;
                    case "4":
                        btnEnbaled2.Visible = false;
                        if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Enabled").ToString()))
                        {
                            btnEnbaled.Text = "Active";
                            btnEnbaled.Enabled = false;
                            //URLReferrer.Text = DataBinder.Eval(e.Row.DataItem, "URL").ToString();
                        }
                        else
                        {
                            btnEnbaled.Text = "Enable";
                            btnEnbaled.Enabled = true;
                        }
                        break;
                }
                


                break;
            default:
                break;
        }
    }
    
    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        Button btn = null;
        int rows = 0;

        if (e.CommandSource is Button)
        {
            btn = (Button)e.CommandSource;

            string[] str = e.CommandArgument.ToString().Split(new char[] { ',' });

            switch (btn.Text)
            {
                case "Enable":
                case "Active":

                    bool active = btn.Text == "Enable";

                    rows = DataAccess.DataRiskDao.UpdateTermsConditionsEnable(str[0], UserSessions.CurrentMerchantApp.ID, active, UserSessions.CurrentUser.UserName);
                    break;
                case "Approve":
                    rows = DataAccess.DataRiskDao.UpdateTermsConditionsStatus(str[0], UserSessions.CurrentMerchantApp.ID, 4, UserSessions.CurrentUser.UserName);

                    break;
                case "Deny":
                    rows = DataAccess.DataRiskDao.UpdateTermsConditionsStatus(str[0], UserSessions.CurrentMerchantApp.ID, 1, UserSessions.CurrentUser.UserName);
                    break;
                case "Add":
                case "View":
                    WucShedule1.OpenWindow(str[0]);
                    break;
            }
            LoadTerms();


        }
        else if (e.CommandSource is LinkButton)
        {
            LinkButton lnk = (LinkButton)e.CommandSource;
            GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            txtName.Text = lnk.Text;
            txtShortTerms.Text = row.Cells[1].Text;
            txtTerms.Text = lnk.CommandArgument;
        }
        else
            return;

        

        
    }






    protected void btnClearFields_Click(object sender, EventArgs e)
    {
        txtShortTerms.Text = string.Empty;
        txtName.Text = string.Empty;
        txtTerms.Text = string.Empty;
    }
}
