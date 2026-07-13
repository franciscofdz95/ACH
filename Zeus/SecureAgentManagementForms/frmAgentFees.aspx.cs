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
using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.NavigationControls;
using Infragistics.Web.UI.EditorControls;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Collections.Generic;

public partial class frmAgentFees : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentAgent != null) 
            base.UID = UserSessions.CurrentAgent.AgentUID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkAgentFees")).CssClass = "active";

        if (!this.IsPostBack)
        {
            this.FormShow("");
        }
    }

    public override void FormShow(string ID)
    {
        this.UID = UserSessions.CurrentAgent.AgentUID;
        DataAgent data = DataAccess.DataAgentDao;
        DataSet ds = data.GetResidualReportItemsDS(UserSessions.CurrentAgent.AgentUID);
        grdResidualReportItems.DataSource = ds;
        grdResidualReportItems.DataBind();
        grdResidualReportItems.Enabled = btnSave.Enabled;
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
        {
            return false;
        }

        try
        {
            UpdateAgentResidualReportItems();
            this.EditMode = false;
            this.ToggleButtons();
            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    public override void FormNew()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDataCheck()
    {
        decimal maxValue = 999999999;
        foreach (GridViewRow row in grdResidualReportItems.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {

                //if UpdateXP TransFee - Percentage we need to show the value as percentage not numeric
                if (grdResidualReportItems.DataKeys[row.RowIndex].Values["UID"].ToString().ToUpper() == "DAABE8FA-8E3C-4560-A1EE-AE12DE179D0D")
                {
                    WebPercentEditor cost = ((WebPercentEditor)row.Cells[1].FindControl("WebPercentEdit1"));
                    //validate Partner Cost
                    if (cost != null)
                    {
                        if (cost.Text.Trim().Length == 0)
                        {
                            this.Master.AddMessageError(String.Format("Partner Cost does not have a value for {0}", row.Cells[0].Text));
                            return false;
                        }
                        else if (CommonUtility.Util.if_dec(cost.Text, 0M) > maxValue)
                        {
                            this.Master.AddMessageError(String.Format("Partner Cost has an invalid value for {0}", row.Cells[0].Text));
                            return false;
                        }
                    }

                    WebPercentEditor merchantcost = ((WebPercentEditor)row.Cells[2].FindControl("WebPercentEdit2"));
                    //validate Merchant Cost
                    if (merchantcost != null)
                    {
                        if (merchantcost.Text.Trim().Length == 0)
                        {
                            this.Master.AddMessageError(String.Format("Merchant Cost does not have a value for {0}", row.Cells[0].Text));
                            return false;
                        }
                        else if (CommonUtility.Util.if_dec(merchantcost.Text, 0M) > maxValue)
                        {
                            this.Master.AddMessageError(String.Format("Merchant Cost has an invalid value for {0}", row.Cells[0].Text));
                            return false;
                        }
                    }
                }
                else
                {
                    WebNumericEditor cost1 = ((WebNumericEditor)row.Cells[1].FindControl("WebNumericEdit1"));

                    //validate Partner Cost
                    if (cost1 != null)
                    {
                        if (cost1.Text.Trim().Length == 0)
                        {
                            this.Master.AddMessageError(String.Format("Partner Cost does not have a value for {0}", row.Cells[0].Text));
                            return false;
                        }
                        else if (CommonUtility.Util.if_dec(cost1.Text, 0M) > maxValue)
                        {
                            this.Master.AddMessageError(String.Format("Partner Cost has an invalid value for {0}", row.Cells[0].Text));
                            return false;
                        }
                    }


                    WebNumericEditor merchantcost1 = ((WebNumericEditor)row.Cells[1].FindControl("WebNumericEdit2"));

                    //validate Merchant Cost
                    if (merchantcost1 != null)
                    {
                        if (merchantcost1.Text.Trim().Length == 0)
                        {
                            this.Master.AddMessageError(String.Format("Merchant Cost does not have a value for {0}", row.Cells[0].Text));
                            return false;
                        }
                        else if (CommonUtility.Util.if_dec(merchantcost1.Text, 0M) > maxValue)
                        {
                            this.Master.AddMessageError(String.Format("Merchant Cost has an invalid value for {0}", row.Cells[0].Text));
                            return false;
                        }
                            //if the feeId is AU transfee, do not allow values <=0
                        else if (CommonUtility.Util.if_dec(merchantcost1.Text, 0M) <= 0M && grdResidualReportItems.DataKeys[row.RowIndex].Values["UID"].ToString().ToUpper() == "11A20A1A-B660-4633-9319-DFCAAAAA9963")
                        {
                            this.Master.AddMessageError(String.Format("Merchant Cost should be greater than 0 for {0}", row.Cells[0].Text));
                            return false;
                        }
                    }
                }

            }
        }
        return true;
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !btnEdit.Enabled;
        btnSave.Enabled = !btnSave.Enabled;
        btnCancel.Enabled = !btnCancel.Enabled;
        grdResidualReportItems.Enabled = btnSave.Enabled;
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        switch (btn.Text)
        {
            case "Add":
                this.FormNew();
                break;

            case "Save":
                if (this.FormSave())
                {
                    Response.Redirect(String.Format("~/SecureAgentManagementForms/frmAgentFees.aspx?Adding=false&AgentID={0}&AgentUID={1}", this.UID, this.UID));
                }
                break;

            case "Refresh":
                this.FormShow(this.UID);
                break;

            case "Cancel":
                this.FormCancel();
                break;

            case "Close":
                break;

            case "Delete":
                break;

            case "Edit":
                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();
                break;
        }
    }

    protected void grdResidualReportItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                WebNumericEditor lblMerchantCost = (WebNumericEditor)e.Row.FindControl("WebNumericEdit2");
                WebNumericEditor lblCost = (WebNumericEditor)e.Row.FindControl("WebNumericEdit1");
                WebPercentEditor lblCost1 = (WebPercentEditor)e.Row.FindControl("WebPercentEdit1");
                WebPercentEditor lblMerchantCost1 = (WebPercentEditor)e.Row.FindControl("WebPercentEdit2");

                //if UpdateXP TransFee - Percentage we need to show the vlaue as percentage not numeric
                if (grdResidualReportItems.DataKeys[e.Row.RowIndex].Values["UID"].ToString().ToUpper() == "DAABE8FA-8E3C-4560-A1EE-AE12DE179D0D")
                {

                    if (lblCost1 != null)
                    {
                        lblCost1.Text = DataBinder.Eval(e.Row.DataItem, "Cost").ToString();
                        lblCost1.Visible = true;
                    }

                    if (lblMerchantCost1 != null)
                    {
                        lblMerchantCost1.Text = DataBinder.Eval(e.Row.DataItem, "MerchantCost").ToString();
                        lblMerchantCost1.Visible = true;
                    }

                    if (lblMerchantCost != null)
                        lblMerchantCost.Visible = false;

                    if (lblCost != null)
                        lblCost.Visible = false;

                }
                else
                {
                    if (lblCost1 != null)
                        lblCost1.Visible = false;

                    if (lblMerchantCost1 != null)
                        lblMerchantCost1.Visible = false;

                    if (lblMerchantCost != null)
                    {
                        lblMerchantCost.Text = CommonUtility.Util.if_dec(DataBinder.Eval(e.Row.DataItem, "MerchantCost"), 0M).ToString();
                        lblMerchantCost.Visible = true;
                    }

                    if (lblCost != null)
                    {
                        lblCost.Visible = true;
                        lblCost.Text = CommonUtility.Util.if_dec(DataBinder.Eval(e.Row.DataItem, "Cost"), 0M).ToString();
                    }

                }

                CheckBox chkEnable = (CheckBox)e.Row.FindControl("chkEnable");
                chkEnable.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Enabled"));

                break;

            default:
                break;
        }
    }


    public bool UpdateAgentResidualReportItems()
    {
        try
        {
            //// get list of agent's merchantapps.
            //List<int> liZID = new List<int>();

            //Hashtable prms = new Hashtable();
            //prms.Add("@AgentUID", UserSessions.CurrentAgent.AgentUID);
            //DataSet ds = DataMerchantApp.GetInstance().GetMerchantApps(prms);

            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in ds.Tables[0].Rows)
            //    {
            //        liZID.Add(Convert.ToInt32(dr["ID"]));
            //    }
            //}

            // for each fee, perform an update.
            DataAgent data = DataAccess.DataAgentDao;
            foreach (GridViewRow row in grdResidualReportItems.Rows)
            {
                decimal cost = 0M, cost1 = 0M, merchantcost = 0M, merchantcost1 = 0M;

                if (((WebNumericEditor)row.Cells[1].FindControl("WebNumericEdit1")) != null)
                    cost = Convert.ToDecimal(((WebNumericEditor)row.Cells[1].FindControl("WebNumericEdit1")).Value);

                if (((WebPercentEditor)row.Cells[1].FindControl("WebPercentEdit1")) != null)
                    cost1 = Convert.ToDecimal(((WebPercentEditor)row.Cells[1].FindControl("WebPercentEdit1")).Value);

                if (((WebNumericEditor)row.Cells[1].FindControl("WebNumericEdit2")) != null)
                    merchantcost = Convert.ToDecimal(((WebNumericEditor)row.Cells[1].FindControl("WebNumericEdit2")).Value);

                if (((WebPercentEditor)row.Cells[1].FindControl("WebPercentEdit2")) != null)
                    merchantcost1 = Convert.ToDecimal(((WebPercentEditor)row.Cells[1].FindControl("WebPercentEdit2")).Value);

                AgentFee fee = new AgentFee();
                fee.UID = grdResidualReportItems.DataKeys[row.RowIndex].Values["UID"].ToString();
                fee.AgentUID = grdResidualReportItems.DataKeys[row.RowIndex].Values["AgentUID"].ToString();
                fee.Enabled = ((CheckBox)row.Cells[2].FindControl("chkEnable")).Checked;

                if (fee.UID.ToUpper() == "DAABE8FA-8E3C-4560-A1EE-AE12DE179D0D") // UpdateXP transfee percentage
                {
                    fee.Cost = cost1;
                    fee.MerchantCost = merchantcost1;
                }
                else
                {
                    fee.Cost = cost;
                    fee.MerchantCost = merchantcost;
                }

                User user = UserSessions.CurrentUser;
                fee.UserUpdated = user.UserName;
                data.UpdateAgentResidualReportItems(fee);

                // cbmsplus fee
                if (fee.UID.ToUpper() == "F9A1DE6A-86F8-431E-A59E-362261B4B260" && fee.Enabled)
                {
                    // apply to all of this agent's merchantapps
                    //foreach (int zid in liZID)
                    //{
                        DataProduct.UpdateAllMerchantsFee(new Guid(fee.AgentUID), 284, fee.MerchantCost);//, zid);
                    //}
                }

            }
            return true;
        }
        catch (Exception exc)
        {
            this.Master.AddMessageError(exc.Message);
            return false;
        }
    }




}
