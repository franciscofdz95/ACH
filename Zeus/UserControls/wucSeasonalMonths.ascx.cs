using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;


public partial class wucSeasonalMonths : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void UpdateSeasonalMonths(string MerchantAppUID)
    {
        DataMerchantApp data = DataAccess.DataMerchantAppDao;

        foreach (ListItem item in chkSeasonalMonths.Items)
        {
            data.UpdateMerchantsAppSeasonalBusinessMonth(MerchantAppUID, item.Value, item.Selected);
        }
    }

    public void LoadSeasonalMonths(string MerchantAppUID, int RepeatColumns)
    {
        chkSeasonalMonths.RepeatColumns = RepeatColumns;

        DataMerchantApp data = DataAccess.DataMerchantAppDao;

        chkSeasonalMonths.DataSource = data.GetSeasonalBusinessMonths(new Hashtable());
        chkSeasonalMonths.DataTextField = "ItemText";
        chkSeasonalMonths.DataValueField = "ItemValue";
        chkSeasonalMonths.DataBind();

        IList<GenericListItem> seasonalMonths = data.GetMerchantAppSeasonalBusinessMonths(MerchantAppUID);
        for (int i = 0; i < seasonalMonths.Count; i++)
        {
            if (chkSeasonalMonths.Items.FindByValue(seasonalMonths[i].ItemValue) != null)
                chkSeasonalMonths.Items.FindByValue(seasonalMonths[i].ItemValue).Selected = seasonalMonths[i].Selected;
        }
    }
}
