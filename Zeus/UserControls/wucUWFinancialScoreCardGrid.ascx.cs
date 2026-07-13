using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using CommonUtility;
using System.Drawing;
using Infragistics.WebUI.WebDataInput;

public partial class wucFinancialScoreBoardGrid : wucBaseDataEntry
{

    public delegate void ButtonClickHandler(object sender, string args);
    public event ButtonClickHandler ButtonClick;

    private int _MerchantID;

    public int MerchantID
    {
        get { return _MerchantID; }
        set { _MerchantID = value; }
    }

    public int _ScoreCardID
    {
        get { return CommonUtility.Util.if_i(ScoreCardID.Value, 0); }
        set { ScoreCardID.Value = value.ToString(); }
    }

    public string _ScoreCardName
    {
        get { return ScoreCardName.Text; }
        set { ScoreCardName.Text = value; }
    }

    public string _TimePeriod
    {
        get { return TimePeriod.Text; }
        set { TimePeriod.Text = value; }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public override void FormShow(string ID)
    {
        FormHandler.SetControlEditMode(pnlEdit, EditMode);
        int ScoreCardID = int.Parse(ID);

        ScoreCardName.Enabled = (this.Adding && this.EditMode);
        TimePeriod.Enabled = (this.Adding && this.EditMode);

        grd1.DataSource = DataScoreCard.GetMerchantScoreCardItem(ScoreCardID, eScoreCardCategory.LiquidityRatios);
        grd1.DataBind();

        grd2.DataSource = DataScoreCard.GetMerchantScoreCardItem(ScoreCardID, eScoreCardCategory.ProfitabilityRatios);
        grd2.DataBind();

        grd3.DataSource = DataScoreCard.GetMerchantScoreCardItem(ScoreCardID, eScoreCardCategory.EfficiencyRatios);
        grd3.DataBind();

        grd4.DataSource = DataScoreCard.GetMerchantScoreCardItem(ScoreCardID, eScoreCardCategory.CashFlow);
        grd4.DataBind();

        if (!this.EditMode)
        {
            FormHandler.SetControlEditMode(pnlEdit, this.EditMode);
        }

    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlEdit);

        lblError.Text = string.Empty;

        ScoreCardName.BorderColor = Color.FromName("#999999");
        ScoreCardName.BorderWidth = Unit.Pixel(2);
        ScoreCardName.BackColor = Color.White;

        TimePeriod.BorderColor = Color.FromName("#999999");
        TimePeriod.BorderWidth = Unit.Pixel(2);
        TimePeriod.BackColor = Color.White;

    }

    public override bool FormSave()
    {
        List<string> liItems = new List<string>();
        List<string> errorlist = new List<string>();
        lblError.Text = string.Empty;

        if (string.IsNullOrWhiteSpace(ScoreCardName.Text))
        {
            errorlist.Add("Scorecard name is required");
            ScoreCardName.BorderColor = Color.Red;
            ScoreCardName.BorderWidth = Unit.Pixel(2);
            ScoreCardName.BackColor = Color.FromName("#fcc");
        }
        else
        {
            ScoreCardName.BorderColor = Color.FromName("#999999");
            ScoreCardName.BorderWidth = Unit.Pixel(2);
            ScoreCardName.BackColor = Color.White;
        }

        if (string.IsNullOrWhiteSpace(TimePeriod.Text))
        {
            errorlist.Add("Time period is required");
            TimePeriod.BorderColor = Color.Red;
            TimePeriod.BorderWidth = Unit.Pixel(2);
            TimePeriod.BackColor = Color.FromName("#fcc");
        }
        else
        {
            TimePeriod.BorderColor = Color.FromName("#999999");
            TimePeriod.BorderWidth = Unit.Pixel(2);
            TimePeriod.BackColor = Color.White;
        }

        for (int count = 1; count < 5; count++)
        {
            GridView grd = (GridView)this.FindControl("grd" + count.ToString());

            foreach (GridViewRow gvr in grd.Rows)
            {
                TextBox defValue = (TextBox)gvr.FindControl("txtValue");
                Label lblValue = (Label)gvr.FindControl("lblValue");
                HiddenField hisci = (HiddenField)gvr.FindControl("hidScoreCardItemID");
                HiddenField hisr = (HiddenField)gvr.FindControl("hidIsRequired");
                HiddenField hia = (HiddenField)gvr.FindControl("hidAllowNegative");
                HiddenField hie = (HiddenField)gvr.FindControl("hidEditable");
                HiddenField hin = (HiddenField)gvr.FindControl("hidName");

                string dvalue = string.Empty;
                Control ctnr = null;
                string error = string.Empty;
                string str = string.Empty;
                bool AllowNegatives = hia != null && CommonUtility.Util.if_b(hia.Value, false);

                if (defValue != null && defValue.Visible)
                {
                    dvalue = defValue.Text;
                    ctnr = defValue;
                }
                else if (lblValue != null && lblValue.Visible)
                {
                    dvalue = lblValue.Text;
                    ctnr = lblValue;
                }

                string propName = hin.Value;

                if (hie != null && CommonUtility.Util.if_b(hie.Value, false))
                {
                    if (hisr != null && CommonUtility.Util.if_b(hisr.Value, false))
                    {
                        if ((string.IsNullOrWhiteSpace(dvalue) || CommonUtility.Util.if_dec(dvalue.Trim(), 0.0M) == 0.0M))
                        {
                            error = propName + " is required.";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(dvalue))
                    {
                        if (AllowNegatives && (!Validation.IsUnsignedNumeric(dvalue) && !Validation.IsSignedNumeric(dvalue)))
                        {
                            error = "Enter a valid " + propName + ".";
                        }
                        else if (!AllowNegatives && !Validation.IsUnsignedNumeric(dvalue))
                        {
                            error = "Enter a valid " + propName + ".";
                        }
                        else
                        {
                            dvalue = CommonUtility.Util.if_dec(dvalue.Trim(), 0.0M).ToString("0.00");
                        }
                    }
                }

                Color strColor = Color.FromName("#999999");
                Color backClr = Color.White;

                if (!string.IsNullOrWhiteSpace(error))
                {
                    errorlist.Add(error);
                    strColor = Color.Red;
                    backClr = Color.FromName("#fcc");
                }

                if (ctnr is TextBox)
                {
                    ((TextBox)ctnr).BorderColor = strColor;
                    ((TextBox)ctnr).BorderWidth = Unit.Pixel(2);
                    ((TextBox)ctnr).BackColor = backClr;
                }

                liItems.Add(string.Format("{0}^{1}", CommonUtility.Util.if_i(hisci.Value, 0), dvalue));

            }

        }

        if (errorlist.Count > 0)
        {

            foreach (string str in errorlist)
                lblError.Text += str + "<br>";

            return false;
        }

        string mystr = CommonUtility.Util.implode(liItems, "|");

        if (_ScoreCardID > 0)
            DataScoreCard.UpdateScoreCardItems(_ScoreCardID, mystr, UserSessions.CurrentUser.UserName);
        else if (UserSessions.CurrentMerchantApp != null)
            _ScoreCardID = DataScoreCard.InsertScoreCardItems(int.Parse(UserSessions.CurrentMerchantApp.ID), ScoreCardName.Text, TimePeriod.Text, mystr, UserSessions.CurrentUser.UserName);

        liItems.Clear();

        // once we save, replace with the old score card with the new one.        
        UserSessions.CurrentScoreCard = DataScoreCard.GetMerchantScoreCardItem(_ScoreCardID);

        return true;
    }

    public override void FormNew()
    {
        this.FormClear();

        this.Adding = true;
        this.EditMode = true;

        FormHandler.SetControlEditMode(pnlEdit, this.EditMode);

        this.ToggleButtons();

        FormShow("0");
    }

    public override bool FormDelete()
    {
        DataScoreCard.DeleteScoreCard(_ScoreCardID);
        return true;
    }

    public override bool FormDataCheck()
    {
        throw new NotImplementedException();
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        this.Adding = false;
        this.ToggleButtons();
    }

    protected void gvCommon_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            MerchantScoreCardItem msci = (MerchantScoreCardItem)e.Row.DataItem;
            GridView grd = (GridView)sender;
            int rowindex = e.Row.RowIndex;

            TextBox DefaultValue = (TextBox)e.Row.FindControl("txtValue");
            Label CalValue = (Label)e.Row.FindControl("lblValue");

            if (msci.IsEditable && DefaultValue != null)
            {
                DefaultValue.Visible = true;
                DefaultValue.Text = CommonUtility.Util.if_dec(msci.Value, 0.0M).ToString("0.00");
            }
            else if (!msci.IsEditable && CalValue != null)
            {
                CalValue.Visible = true;
                CalValue.Text = CommonUtility.Util.if_dec(msci.Value, 0.0M).ToString("0.00");
            }

        }
    }

    public void SetControlEditMode(bool isEdit)
    {
        FormHandler.SetControlEditMode(pnlEdit, isEdit);
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnAdd.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnCancel.Enabled = this.EditMode;
        btnDelete.Enabled = !this.EditMode;
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;
        string url = string.Empty;


        switch (btn.Text)
        {
            case "Add":

                FormNew();

                break;

            case "Save":

                if (this.FormSave())
                {
                    lblError.Text = "";
                    this.Adding = false;
                    this.EditMode = false;
                    ToggleButtons();

                    FormShow(_ScoreCardID.ToString());
                }

                break;

            case "Refresh":
                this.FormShow(_ScoreCardID.ToString());
                break;

            case "Cancel":

                if (this.Adding)
                {
                    this.FormShow("0");
                }
                else
                {
                    this.FormShow(_ScoreCardID.ToString());
                }

                this.Adding = false;
                this.EditMode = false;

                this.ToggleButtons();

                if (this.ButtonClick != null)
                    ButtonClick(null, string.Empty);

                break;

            case "Delete":
                this.FormDelete();

                if (this.ButtonClick != null)
                    ButtonClick(null, string.Empty);

                break;

            case "Edit":
                this.EditMode = true;
                this.FormShow(_ScoreCardID.ToString());
                this.ToggleButtons();
                break;

        }
    }

}
