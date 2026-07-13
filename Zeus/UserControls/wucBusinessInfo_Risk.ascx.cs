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

public partial class wucBusinessInfo_Risk : System.Web.UI.UserControl
{
    public Panel pnlInfo
    {
        get { return pnlGeneralInfo; }
    }

    override protected void OnInit(EventArgs e)
    {
        if (!this.IsPostBack)
        {
            LookupTableHandler.MerchantAppStatus(StatusUID, false, "Merchant Management");
            LookupTableHandler.MerchantAppStatus(ACHStatusUID, false, "Merchant Management");
            LookupTableHandler.LoadFrontEndPlatforms(AuthPlatformUID, false);
            LookupTableHandler.LoadBackEndPlatforms(SettlePlatformUID, false);
            LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, false);
            LookupTableHandler.LoadReleaseMethods(this.ReleaseMethodUID, false);
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (UserSessions.CurrentMerchantApp != null)
            {
                bool isACHonly = UserSessions.CurrentMerchantApp.AchID > 0 && MerchantAppTypeUID.SelectedValue.ToUpper() == Constants.BANK_ACH_ONLY;
                setACHStatus(isACHonly);
            }
        }
    }

    private void setACHStatus(bool isAchonly)
    {
        MerchantApp app = UserSessions.CurrentMerchantApp;

        if (isAchonly)
        {
            //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
            UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(UserSessions.CurrentMerchantApp.ID));

            if (UserSessions.ActiveAchMerchant != null)
            {
                AchMerchant achmerchant = UserSessions.ActiveAchMerchant;
                LookupTableHandler.MerchantAppStatus(ACHStatusUID, false, "Merchant Management", achmerchant.MerchantStatusName.Substring(0, 2));
                ListHandler.ListFindItem(ACHStatusUID, achmerchant.MerchantStatusUID);
            }
            else
                LookupTableHandler.MerchantAppStatus(ACHStatusUID, false, "Merchant Management", "SS");

            lblACHStatus.Style["display"] = "block";
            lblCCStatus.Style["display"] = "none";

            ACHStatusUID.Style["display"] = "block";
            StatusUID.Style["display"] = "none";
        }
        else
        {
            lblACHStatus.Style["display"] = "none";
            lblCCStatus.Style["display"] = "block";

            ACHStatusUID.Style["display"] = "none";
            StatusUID.Style["display"] = "block";
        }

    }

}
