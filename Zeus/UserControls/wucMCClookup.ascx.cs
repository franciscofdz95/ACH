using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.Facade;
using PaymentXP.BusinessObjects;

public partial class wucMCClookup : System.Web.UI.UserControl
{
    // Added by Chandra for PXP-7898
    public bool IsVisible
    {
        set { ViewState["IsVisible"] = value; }
        get
        {
            if (ViewState["IsVisible"] == null)
                return false;
            else
                return (bool)ViewState["IsVisible"];
        }
    }

    public bool IsEnabled
    {
        set { ViewState["IsEnabled"] = value; }
        get
        {
            if (ViewState["IsEnabled"] == null)
                return false;
            else
                return (bool)ViewState["IsEnabled"];
        }
    }

    public Unit txtSicCodeWidth
    {
        set { ViewState["txtSicCodeWidth"] = value; }
        get
        {
            if (ViewState["txtSicCodeWidth"] == null)
                return Unit.Pixel(90);
            else
                return (Unit)ViewState["txtSicCodeWidth"];
        }
    }

    public Unit txtSicCodeDescWidth
    {
        set { ViewState["txtSicCodeDescWidth"] = value; }
        get
        {
            if (ViewState["txtSicCodeDescWidth"] == null)
                return Unit.Pixel(90);
            else
                return (Unit)ViewState["txtSicCodeDescWidth"];
        }
    }

    public Unit lblSicCodeWidth
    {
        set { ViewState["lblSicCodeWidth"] = value; }
        get
        {
            if (ViewState["lblSicCodeWidth"] == null)
                return Unit.Pixel(60);
            else
                return (Unit)ViewState["lblSicCodeWidth"];
        }
    }

    public Unit lblSicCodeDescWidth
    {
        set { ViewState["lblSicCodeDescWidth"] = value; }
        get
        {
            if (ViewState["lblSicCodeDescWidth"] == null)
                return Unit.Pixel(60);
            else
                return (Unit)ViewState["lblSicCodeDescWidth"];
        }
    }

    /// <summary>
    ///  Code added for PXP-12932 by koshlendra Start
    /// </summary>
    public Unit txtVisaSicCodeWidth
    {
        set { ViewState["txtVisaSicCodeWidth"] = value; }
        get
        {
            if (ViewState["txtVisaSicCodeWidth"] == null)
                return Unit.Pixel(90);
            else
                return (Unit)ViewState["txtVisaSicCodeWidth"];
        }
    }

    public Unit txtVisaSicCodeDescWidth
    {
        set { ViewState["txtVisaSicCodeDescWidth"] = value; }
        get
        {
            if (ViewState["txtVisaSicCodeDescWidth"] == null)
                return Unit.Pixel(90);
            else
                return (Unit)ViewState["txtVisaSicCodeDescWidth"];
        }
    }

    public Unit lblVisaSicCodeWidth
    {
        set { ViewState["lblVisaSicCodeWidth"] = value; }
        get
        {
            if (ViewState["lblVisaSicCodeWidth"] == null)
                return Unit.Pixel(60);
            else
                return (Unit)ViewState["lblVisaSicCodeWidth"];
        }
    }

    public Unit lblVisaSicCodeDescWidth
    {
        set { ViewState["lblVisaSicCodeDescWidth"] = value; }
        get
        {
            if (ViewState["lblVisaSicCodeDescWidth"] == null)
                return Unit.Pixel(60);
            else
                return (Unit)ViewState["lblVisaSicCodeDescWidth"];
        }
    }
    public bool txtVisaSicCodeDescReadonly
    {
        set { ViewState["txtVisaSicCodeDescReadonly"] = value; }
        get
        {
            if (ViewState["txtVisaSicCodeDescReadonly"] == null)
                return false;
            else
                return (bool)ViewState["txtVisaSicCodeDescReadonly"];
        }
    }

    public bool txtVisaSicCodeReadonly
    {
        set { ViewState["txtVisaSicCodeReadonly"] = value; }
        get
        {
            if (ViewState["txtVisaSicCodeReadonly"] == null)
                return false;
            else
                return (bool)ViewState["txtVisaSicCodeReadonly"];
        }
    }

    public string m_VisaSicCode
    {
        get { return txtVisaSicCode.Text; }
        set { txtVisaSicCode.Text = value; }
    }

    public string m_VisaSicCodeDesc
    {
        get { return txtVisaSicCodeDesc.Text; }
        set { txtVisaSicCodeDesc.Text = value; }
    }
    /// <summary>
    ///  Code added for PXP-12932 by koshlendra End
    /// </summary>
    /// 
    // Added by Chandra for PXP-7898
    public Unit lblNutraWidth
    {
        set { ViewState["lblNutraWidth"] = value; }
        get
        {
            if (ViewState["lblNutraWidth"] == null)
                return Unit.Pixel(60);
            else
                return (Unit)ViewState["lblNutraWidth"];
        }
    }

    public bool txtSicCodeDescReadonly
    {
        set { ViewState["txtSicCodeDescReadonly"] = value; }
        get
        {
            if (ViewState["txtSicCodeDescReadonly"] == null)
                return false;
            else
                return (bool)ViewState["txtSicCodeDescReadonly"];
        }
    }

    public bool txtSicCodeReadonly
    {
        set { ViewState["txtSicCodeReadonly"] = value; }
        get
        {
            if (ViewState["txtSicCodeReadonly"] == null)
                return false;
            else
                return (bool)ViewState["txtSicCodeReadonly"];
        }
    }

    public short LookupButtonTabIndex
    {
        set { ViewState["LookupButtonTabIndex"] = value; }
        get
        {
            if (ViewState["LookupButtonTabIndex"] == null)
                return 0;
            else
                return (short)ViewState["LookupButtonTabIndex"];
        }
    }

    public string m_SicCode
    {
        get { return txtSicCode.Text; }
        set { txtSicCode.Text = value; }
    }

    public string m_SicCodeDesc
    {
        get { return txtSicCodeDesc.Text; }
        set { txtSicCodeDesc.Text = value; }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this.PreRender += new EventHandler(wucMCClookup_PreRender);
    }

    protected void wucMCClookup_PreRender(object sender, EventArgs e)
    {
        txtSicCode.Width = txtSicCodeWidth;
        txtSicCodeDesc.Width = txtSicCodeDescWidth;
        lblSicCode.Width = lblSicCodeWidth;
        lblSicCodeDesc.Width = lblSicCodeDescWidth;
        txtSicCode.ReadOnly = txtSicCodeReadonly;
        txtSicCodeDesc.ReadOnly = txtSicCodeDescReadonly;

        //pxp-12932
        txtVisaSicCode.Width = txtVisaSicCodeWidth;
        txtVisaSicCodeDesc.Width = txtVisaSicCodeDescWidth;
        lblVisaSicCode.Width = lblVisaSicCodeWidth;
        lblVisaSicCodeDesc.Width = lblVisaSicCodeDescWidth;
        txtVisaSicCode.ReadOnly = txtVisaSicCodeReadonly;
        txtVisaSicCodeDesc.ReadOnly = txtVisaSicCodeDescReadonly;
        //PXP-12932


        // Added by Chandra for PXP-7898
        MerchantApp app = UserSessions.CurrentMerchantApp;
        bool isACHonly = (app != null && app.AchID > 0 && app.MerchantAppTypeUID.ToUpper() == Constants.BANK_ACH_ONLY);
        bool Ccstatus = (app != null && MerchantFacade.ExistsStatus(app.MerchantAppUID, Constants.QUEUESTATUS_MS_ACTIVE) && !isACHonly);
        bool AchStatus = (app != null && MerchantFacade.ExistsACHStatus(app.ID, Constants.QUEUESTATUS_MS_ACTIVE) && isACHonly);
        bool isCURole = (UserSessions.CurrentUser != null && UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_CREDIT_UNDERWRITING);
        bool isMsActive = Ccstatus || AchStatus;


        // PXP-12435 : Start By Anuj Kumar
        if (!IsEnabled || (IsEnabled && !isCURole))
            IsNutraMerchant.Enabled = false;
        // PXP-12435 : End By Anuj Kumar
        //PXP-12671 Fixing- start by koshlendra
        if (app != null && app.Office.Equals(CommonUtility.Util.Offices.Irvine))
        {

            Label1.Text = "Tangible Trial";
        }
        else
        {
            Label1.Text = "Nutra Trial";
        }
        //PXP-12671 Fixing- End by koshlendra
        if (this.LookupButtonTabIndex > 0)
            btnLookup.TabIndex = this.LookupButtonTabIndex;
        Label1.Width = lblNutraWidth;

        pnlNutra.Visible = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnLookup_Click(object sender, EventArgs e)
    {
        //code changes for PXP-12935 & PXP-12932 by koshlendra start
        Button button = (Button)sender;
        string buttonId = button.ID;
        if (buttonId == btnVisaLookup.ID)
            VisaMCC.Value = "Visa MCC:";
        else
            VisaMCC.Value = "Non-Visa MCC:";
        lblMCC.Text = VisaMCC.Value;
        txtMCC.Text = string.Empty;
        txtMCCDesc.Text = string.Empty;
        grd.DataSource = null;
        grd.DataBind();
        //code changes for PXP-12935 & PXP-12932 by koshlendra end
        WebDialogWindow1.WindowState = DialogWindowState.Normal;


    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                break;
            default:
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Select":

                LinkButton lb = (LinkButton)e.CommandSource;

                string code = ((Label)(lb.Parent.FindControl("lblCode"))).Text;
                string desc = ((Label)(lb.Parent.FindControl("lblDesc"))).Text.Replace("'", "^");
                //code changes done for PXP-12935 and PXP-12932 by koshlendra start
                if (VisaMCC.Value == "Visa MCC:")
                {
                    txtVisaSicCode.Text = code;
                    VisaSicCodeDesc.Value = Server.HtmlDecode(desc.Replace("^", "'"));
                    txtVisaSicCodeDesc.Text = Server.HtmlDecode(desc.Replace("^", "'"));
                }
                else
                {
                    txtSicCode.Text = code;
                    SicCodeDesc.Value = Server.HtmlDecode(desc.Replace("^", "'"));
                    txtSicCodeDesc.Text = Server.HtmlDecode(desc.Replace("^", "'"));
                    if (IsVisible)
                    {
                            pnlNutra.Visible = true;
                    }
                }
                //code changes done for PXP-12935 and PXP-12932 by koshlendra end
                WebDialogWindow1.WindowState = DialogWindowState.Hidden;

                break;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtMCC.Text = string.Empty;
        txtMCCDesc.Text = string.Empty;
        grd.DataSource = null;
        grd.DataBind();
        lblNoRecords.Visible = true;
    }

    private void SearchMCC()
    {
        MerchantFacade facade = new MerchantFacade();
        Hashtable prms = new Hashtable();

        if (txtMCC.Text.Trim() != string.Empty)
            prms.Add("@Name", txtMCC.Text);

        if (txtMCCDesc.Text.Trim() != string.Empty)
            prms.Add("@Description", txtMCCDesc.Text);

        DataSet ds = facade.GetSicCodes(prms);
        DataView dv = ds.Tables[0].DefaultView;
        dv.RowFilter = "Name <> '0000'";
        grd.DataSource = dv;
        grd.DataBind();

        lblNoRecords.Visible = (grd.Rows.Count == 0);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.SearchMCC();
    }

}
