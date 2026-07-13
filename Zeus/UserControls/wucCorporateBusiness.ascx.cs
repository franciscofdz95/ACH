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
using System.Text.RegularExpressions;

using Infragistics.WebUI.WebDataInput;
using PaymentXP.BusinessObjects;

public partial class wucCorporateBusiness : System.Web.UI.UserControl
{
    public delegate void ValueChangeHandler(object sender, Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventArgs e);
    public event ValueChangeHandler ValueChange;

    public string SetTitle
    {
        get { return lblTitle.Text; }
        set { lblTitle.Text = value; }
    }

    public bool ValidAdd
    {
        get { return checkAddress(); }
    }

    override protected void OnInit(EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //Added Owners phone country code
            LookupTableHandler.LoadCountryCallingCodes(HomePhoneCountryCode);

            LookupTableHandler.LoadCountries(Country);
        }
    }

    public Panel pnlCorporateBusiness
    {
        get { return pnlCorpBuzMain; }
    }


    private bool checkAddress()
    {
        return true;
    }

    public bool verifyAddress()
    {
        return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ToggalCorporateBusiness();
    }



    //Added Owner DBA country code
    protected void Page_PreRender(object sender, EventArgs e)
    {
        HomeCountryCodeDisplay.ReadOnly = true;
    }

    protected void cbxIsBusinessOwnedByCorporate_CheckedChanged(object sender, EventArgs e)
    {
        ToggalCorporateBusiness();
    }

    internal void ToggalCorporateBusiness()
    {
        if (IsBusinessOwnedByCorporate.Checked)
        {
            pnlCorpBuz.Visible = true;
        }
        else
        {
            pnlCorpBuz.Visible = false;
            FormHandler.ClearAllControls(pnlCorpBuz);
            //ClearControls();
        }
    }

    internal void EnableDisableCorporateBusinessSection(string businessStructureUID)
    {
        if (businessStructureUID.ToUpper().Equals("820184DE-0254-442C-A8DF-11CFC1C7D98D"))
            IsBusinessOwnedByCorporate.Enabled = false;
        else
            IsBusinessOwnedByCorporate.Enabled = true; ;

    }
}
