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

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using ZeusWeb.Class;
using PaymentXP.BusinessObjects.Zeus;
using System.Net;


public partial class frmQueryConsumerConsent : frmBaseSearch
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkRiskQueryConsumerConsent")).CssClass = "active";

        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadNMIVendors(NMIVendorUID, false, true);
        }
    }
    protected void btnSubmit_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        try
        {
            MerchantFacade facade = new MerchantFacade();
            if (!string.IsNullOrEmpty(NMITransactionID.Text.Trim()))
            {
                //PXP-11432 Implement Consumer Consent API Call for M1 CRM By Ali
                ConsentAPIResponse _ConsentAPIService = ConsentAPIService.GetResponse(NMITransactionID.Text.Trim(), int.Parse(NMIVendorUID.SelectedValue == string.Empty? "0": NMIVendorUID.SelectedValue));

                if (_ConsentAPIService != null && _ConsentAPIService.ResponseMsg.Contains("Successful"))
                {
                    pnlMerchant.Visible = true;
                    NoDataRecord.Visible = false;
                    _ConsentAPIService.Merchant.CustomerServicePhone = facade.PhoneFormat(_ConsentAPIService.Merchant.CustomerServicePhone);
                    FormBinding.BindObjectToControls(_ConsentAPIService.Subscription, wucCRMSubscription1);
                    FormBinding.BindObjectToControls(_ConsentAPIService.Merchant, wucCRMSubscription1);
                    if (_ConsentAPIService.NotificationHistory != null)
                    {
                        wucCRMNotificationGrid1.NotificationHistorys = _ConsentAPIService.NotificationHistory;
                        wucCRMNotificationGrid1.NotificationHistorys.Reverse();
                        wucCRMNotificationGrid1.BindGrid();
                    }
                    if (_ConsentAPIService.ConsumerActions != null)
                    {
                        wucCRMCustomerGrid1.ConsumerActions = _ConsentAPIService.ConsumerActions;
                        wucCRMCustomerGrid1.BindGrid();
                    }
                }
                else
                {
                    pnlMerchant.Visible = false;
                    NoDataRecord.Visible = true;
                    this.Master.AddMessageError("Response Code:" + _ConsentAPIService.ResponseCode + " Response Message: " + _ConsentAPIService.ResponseMsg);

                }
               
            }
            //LoadGrid();
        }

        catch (WebException webex)
        {
            if (webex.Status == WebExceptionStatus.Timeout)
            {
                this.Master.AddMessageError("Request TimedOut");
            }

            this.Master.AddMessageError("Error");
        }

        catch (Exception ex)
        {
            this.Master.AddMessageError("Error");
        }
    }

}
