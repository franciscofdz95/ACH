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

public partial class wucMerchantGeneralInfo : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {

            //Load all dropdownlist
            
            LookupTableHandler.MerchantAppStatus(StatusUID, false, "Merchant Management");
            LookupTableHandler.LoadBusinessStructures(BusinessStructureUID, false);
            LookupTableHandler.LoadAreaZones(AreaZoneUID, false);
            LookupTableHandler.LoadSquareFootage(SquareFootageUID, false);
            LookupTableHandler.LoadLocationTypes(LocationTypeUID, false);
            LookupTableHandler.LoadBusinessPremisesOwnerships(BusinessPremisesOwnershipUID, false);
            LookupTableHandler.LoadReasonChanges(ReasonChangesUID, false);
            LookupTableHandler.LoadReturnPolicies(ReturnPoliciesUID, false);
            LookupTableHandler.LoadTimeZones(TimeZone, false);
            LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, false);
            LookupTableHandler.LoadApplicationTypes(ApplicationTypeUID, false);
       
        
        }
    }
}
