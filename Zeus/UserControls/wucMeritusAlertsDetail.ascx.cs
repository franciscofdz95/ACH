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

public partial class UserControls_wucMeritusAlertsDetail : System.Web.UI.UserControl
{
    
    public MeritusAlert ObjMA
    {
        get { return (MeritusAlert)ViewState["ObjMA"]; }
        set { ViewState["ObjMA"] = value; }
    }

    public String CSubject
    {
        get { return Subject.Text; } 
    }

    public String CAlertDate
    {
        get { return AlertDate.Text; }
    }

    public string CDisabledDate
    {
        get { return DisabledDate.Text; }
    }

    public string CUrl
    {
        get { return URL.Text; }
    }

    public string CPortalUID
    {
        get { return PortalUID.SelectedValue; }
    }

    public bool CDisplaySplashScreen
    {
        get { return DisplaySplashScreen.Checked; }
    }

    public string CHTMLContent
    {
        get { return HTMLContent.Text; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.init_always();

        if (!Page.IsPostBack)
        {
            this.initialize();
        }
    }

    protected void initialize()
    {
        LookupTableHandler.LoadPortals(PortalUID, false);
        PortalUID.Items.Add(new ListItem("Select Portal", ""));
    }

    protected void init_always()
    {

    }

    public void Clear()
    {
        FormHandler.ClearAllControls(pnlDetail);
        this.ObjMA = null;
        this.AlertID.Text = "";
        this.HTMLContent.Text = "";
    }

    public bool FillMeritusAlerts(MeritusAlert objMA)
    {
        bool blnRet = false;

        if (objMA != null)
        {
            FormBinding.BindObjectToControls(objMA, pnlDetail);

            // a lazy check to make sure our panel has it's objects fill with data.
            if (!string.IsNullOrEmpty( AlertID.Text))
            {
                blnRet = true;
                this.ObjMA = objMA;


                if (AlertDate.Value != null && ((DateTime)AlertDate.Value).Date == DateTime.MinValue.Date)
                {
                    AlertDate.Value = null;
                }

                if (DisabledDate.Value != null && ((DateTime)DisabledDate.Value).Date == DateTime.MaxValue.Date)
                {
                    DisabledDate.Value = null;
                }
            }
        }

        return blnRet;

        
    }

    protected void UserControls_wucMeritusAlertsDetail_PreRender(object sender, EventArgs e)
    {
        if (AlertDate.Value != null && (DateTime)AlertDate.Value == DateTime.MinValue)
        {
            AlertDate.Value = null;
        }

        if (DisabledDate.Value != null && (DateTime)DisabledDate.Value == DateTime.MaxValue)
        {
            DisabledDate.Value = null;
        }
    }

    public void BindControlsToObject()
    {
        FormBinding.BindControlsToObject(this.ObjMA, pnlDetail);
    }

    public void SetControlEditMode(bool isEdit)
    {
        FormHandler.SetControlEditMode(pnlDetail, isEdit);
    }

    protected void PortalUID_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        switch (PortalUID.SelectedValue.ToUpper())
        { 
            case "E263D06C-A6F1-4C27-9103-231D8FEE85C5"://	Zeus
                URL.Text = "";
                break;

            case "76411203-7F8E-4FC1-9CDC-9CF0C8084611"://	Merchant
                URL.Text = "~/ContactForms/frmAlerts.aspx";
                break;

            case "8D37E9F5-4094-4D92-987F-C3E642E6B092"://	Agent
                URL.Text = "~/web/secureHomeForms/frmAlerts.aspx";
                break;

            case "4A77C310-4264-45C6-96C1-F7EFE61C7D2E"://	PaymentXP
                URL.Text = "~/FormHome/frmAlerts.aspx";
                break;
            default:
                URL.Text = string.Empty;
                break;
        }

    }
}
