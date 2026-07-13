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

public partial class wucOwner : System.Web.UI.UserControl
{
    public delegate void ValueChangeHandler(object sender, Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventArgs e, string ownerID, string ssn, string parentID);
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
            LookupTableHandler.LoadCountries(IDNationality);
            IDNationality.Items.Insert(0, new ListItem("--Select--", ""));
            //PXP-4979- Code changes added by koshlendra start
            this.CBRWaived.Enabled = false;
            //PXP-4979- Code changes added by koshlendra end
        }
    }

    public Panel pnlOwner
    {
        get { return pnlOwn; }
    }

    private bool checkAddress()
    {
        //string ownerAdd = Address1.Text;
        //String ZipRegex = @"^\s*[Pp]\.?\s?[Oo]\.?\s[Bb][Oo][Xx]";
        //if (Regex.IsMatch(ownerAdd, ZipRegex))
        return true;

        //else
        //    return false;
    }

    public bool verifyAddress()
    {
        //string url = "http://www.melissadata.com/lookups/AddressVerify.asp?name=&Company=&Address=" + Address1.Text + "&city=" + City.Text + "&state=" + State.SelectedItem.Text + "&zip=" + Zip.Text;
        //if (Address1.Text != string.Empty || City.Text != string.Empty || State.SelectedIndex > 0 || Zip.Text != string.Empty)
        //    return FormHandler.VerifyAdd(url);
        return true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        WebUtil.SetUserSpecificDisplayMode(DOB);
        WebUtil.SetUserSpecificDisplayMode(DriversLicenseExp);
    }

    protected void SSN_ValueChange(object sender, Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventArgs e)
    {
        if (this.ValueChange != null)
        {
            this.ValueChange(SSN, e, OwnerID.Text, SSN.Text, this.ID);
        }
    }

    //Added Owner DBA country code
    protected void Page_PreRender(object sender, EventArgs e)
    {
        HomeCountryCodeDisplay.ReadOnly = true;        
    }

}
