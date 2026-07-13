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

public partial class wucCashAdvance : System.Web.UI.UserControl
{
    public delegate void ButtonClickHandler(object sender, EventArgs e);
    public event ButtonClickHandler ButtonClick;

    public Panel pnlAdv
    {
        get { return pnlAdvances; }
    }

    public int grdcount
    {
        get { return grd.Rows.Count; }
    }

    public bool grdVisible
    {
        get { return grd.Visible; }
        set { grd.Visible = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            WebUtil.SetUserSpecificDisplayMode(DateFunded);
            LookupTableHandler.LoadCashAdvanceCollection(CollectionMethodID, false);
            LookupTableHandler.LoadCashAdvanceLender(LenderID, false);
            LookupTableHandler.LoadCashAdvanceStatus(StatusID, false);
            LoadCashAdvance();
        }
        lblError.Text = "";
    }

    protected void odsCashAdvance_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();

        if(UserSessions.CurrentMerchantApp != null)
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        e.InputParameters[0] = prms;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(PaybackAmount.Text) || string.IsNullOrEmpty(HoldbackPct.Text) ||
            LenderID.SelectedIndex == 0 || CollectionMethodID.SelectedIndex == 0 || StatusID.SelectedIndex == 0)
        {
            lblError.Text = "Please provide all values.";
            return;
        }

        try
        {
            CashAdvance cashAdv = new CashAdvance();
            cashAdv.MerchantID = UserSessions.CurrentMerchantApp.ID;
            cashAdv.MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            cashAdv.AmountBorrowed = AmountBorrowed.ValueDecimal;
            cashAdv.HoldbackPct = HoldbackPct.ValueDecimal;
            cashAdv.PaybackAmount = PaybackAmount.ValueDecimal;
            cashAdv.LenderID = LenderID.SelectedValue;
            cashAdv.CollectionMethodID = CollectionMethodID.SelectedValue;
            cashAdv.StatusID = StatusID.SelectedValue;
            if (DateFunded.Text != string.Empty)
                cashAdv.DateFunded = DataLayer.Field2Date(DateFunded.Value);
            cashAdv.UserCreated = UserSessions.CurrentUser.UserName;
            int rows = DataAccess.DataRiskDao.InsertCashAdvance(cashAdv);
            if (rows > 0)
            {
                FormHandler.ClearAllControls(pnlCash);
                hdnCashadvance.Value = "";
                btnAdd.Visible = true;
                btnSave.Visible = false;
                btnClearFields.Text = "Clear";
            }
        }
        //catch (Exception exc)
        catch
        { }
        LoadCashAdvance();

        if (ButtonClick != null)
        {
            ButtonClick(sender, e);
        }
    }

    public void LoadCashAdvance()
    {
        grd.DataBind();
        pnlAdvances.Visible = grd.Rows.Count > 0;
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                LinkButton lnkID = (LinkButton)e.Row.FindControl("lnkID");
                lnkID.Text = DataBinder.Eval(e.Row.DataItem, "CashAdvanceID").ToString();
                e.Row.Cells[8].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[8].Text);
                e.Row.Cells[10].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[10].Text);
               break;
            default:
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton btn = null;
        if (e.CommandSource is LinkButton)
        {
            btn = (LinkButton)e.CommandSource;
            switch (btn.CommandName)
            {
                case "Select":
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    HoldbackPct.Text = row.Cells[6].Text;
                    AmountBorrowed.Text = row.Cells[4].Text;
                    PaybackAmount.Text = row.Cells[5].Text;
                    ListHandler.ListFindItem(LenderID, row.Cells[11].Text.Trim());
                    ListHandler.ListFindItem(CollectionMethodID, row.Cells[12].Text.Trim());
                    ListHandler.ListFindItem(StatusID, row.Cells[13].Text.Trim());
                    hdnCashadvance.Value = row.Cells[1].Text.Trim();
                    DateFunded.Value = row.Cells[8].Text;
                    btnSave.Visible = true;
                    btnAdd.Visible = false;
                    btnClearFields.Text = "Cancel";
                    break;
            }
            LoadCashAdvance();
        }
        else
            return;
    }

    protected void btnClearFields_Click(object sender, EventArgs e)
    {
        Formclear();
    }

    public void Formclear()
    {
        FormHandler.ClearAllControls(pnlCash);
        hdnCashadvance.Value = "";
        btnSave.Visible = false;
        btnAdd.Visible = true;
        btnClearFields.Text = "Clear";
        lblError.Text = "";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(PaybackAmount.Text) || string.IsNullOrEmpty(HoldbackPct.Text) || string.IsNullOrEmpty(AmountBorrowed.Text) ||
            LenderID.SelectedIndex == 0 || CollectionMethodID.SelectedIndex == 0 || StatusID.SelectedIndex == 0 )
        {
            lblError.Text = "Please provide all values.";
            return;
        }

        try
        {
            CashAdvance cashAdv = new CashAdvance();
            cashAdv.CashAdvanceID = hdnCashadvance.Value;
            cashAdv.MerchantID = UserSessions.CurrentMerchantApp.ID;
            cashAdv.MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            cashAdv.AmountBorrowed = AmountBorrowed.ValueDecimal;
            cashAdv.HoldbackPct = HoldbackPct.ValueDecimal;
            cashAdv.PaybackAmount = PaybackAmount.ValueDecimal;
            cashAdv.LenderID = LenderID.SelectedValue;
            cashAdv.CollectionMethodID = CollectionMethodID.SelectedValue;
            cashAdv.StatusID = StatusID.SelectedValue;
            cashAdv.DateFunded = Convert.ToDateTime(DateFunded.Value);
            cashAdv.UserUpdated = UserSessions.CurrentUser.UserName;

            int rows = DataAccess.DataRiskDao.UpdateCashAdvance(cashAdv);
            if (rows > 0)
            {
                FormHandler.ClearAllControls(pnlCash);
                hdnCashadvance.Value = "";
                btnAdd.Visible = true;
                btnSave.Visible = false;
                btnClearFields.Text = "Clear";
            }
        }
        //catch(Exception exc) 
        catch
        { }
        LoadCashAdvance();

        if (ButtonClick != null)
        {
            ButtonClick(sender, e);
        }
    }
}
