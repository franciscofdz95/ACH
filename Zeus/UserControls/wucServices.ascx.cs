using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using System.Text;


public partial class wucServices : System.Web.UI.UserControl
{
    //private static int selectedVendorID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    public void UpdateServices(string MerchantAppUID, string CategoryID)
    {
        //PXP-2590
        bool ethocaChecked = false;
        bool verifiChecked = false;
        bool ethocaChanged = false;
        bool verifiChanged = false;
        //Get Existing value for teh services
        DataMerchantServices data = new DataMerchantServices();
        Hashtable prms = new Hashtable();
        IList<MerchantServices> services = new List<MerchantServices>();
        prms.Add("@MerchantAppUID", DataLayer.UID2Field(MerchantAppUID));
        prms.Add("@CategoryID", CategoryID);
        services = data.GetMerchantServicesWithoutProducts(prms);

        IList<MerchantServices> updatedservices = new List<MerchantServices>();
        StringBuilder sbServiceUIDList = new StringBuilder();
        foreach (ListItem item in chkServices.Items)
        {
            updatedservices.Add(new MerchantServices { MerchantServiceID = item.Value, Description = item.Text, Checked = item.Selected, Name = item.Text });
            if (item.Selected)
            {
                sbServiceUIDList.Append("," + item.Value);
            }
        }

        string serviceUIDList = sbServiceUIDList.ToString();

        if (serviceUIDList.Length > 0) //remove initial comma
            serviceUIDList = serviceUIDList.Substring(1);

        prms = new Hashtable();
        prms.Add("@MerchantAppUID", DataLayer.UID2Field(MerchantAppUID));
        prms.Add("@CategoryID", CategoryID);
        prms.Add("@ServiceUIDList", serviceUIDList);

        data.UpdateMerchantServices(prms);

        StringBuilder sb = new StringBuilder();
        string s1 = string.Empty;
        string s2 = string.Empty;
        //Log change to the selection of service.
        
        //PXP- 3169   
        List<Product> productList = DataProduct.GetAgentProductList(new Guid(UserSessions.CurrentMerchantApp.AgentUID), int.Parse(UserSessions.CurrentMerchantApp.ID), false, UserSessions.CurrentMerchantApp.Brand);

        for (int i = 0; i < services.Count; i++)
        {
            s1 = services[i].Checked == null ? string.Empty : Convert.ToString(services[i].Checked).ToString();
            s2 = updatedservices[i].Checked == null ? string.Empty : Convert.ToString(updatedservices[i].Checked).ToString();
            if (updatedservices[i].Name.Equals("Ethoca"))
            {
                ethocaChecked = updatedservices[i].Checked;
            }
            if (updatedservices[i].Name.Equals("Verifi"))
            {
                verifiChecked = updatedservices[i].Checked;
            }
            if (s1 != s2)
            {
                sb.Append(services[i].Description + ": " + s1 + ", " + s2 + " | ");
                //PXP-2590 Rohit Thakur
                if (updatedservices[i].Name.Equals("Ethoca"))
                {
                    ethocaChecked = updatedservices[i].Checked; 
                    ethocaChanged = true; 
                }         
                if (updatedservices[i].Name.Equals("Verifi"))
                {
                    verifiChecked = updatedservices[i].Checked;
                    verifiChanged = true; 
                }
                
                if (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
                {
                    if (services[i].Name.Equals("CBMS"))
                    {//PXP- 3169  
                        int productId = Constants.PRODUCT_CBMS;
                        Product product = productList.FirstOrDefault<Product>(x => x.ProductId == productId);
                        if (product != null)
                        {
                            if (updatedservices[i].Checked)
                            {
                                PaymentXP.Facade.ProductSubscriptionService.Subscribe(UserSessions.CurrentMerchantApp, UserSessions.CurrentUser, ePortals.ZEUS, product);
                                //PXP-10890:Sanidhya Start
                                PaymentXP.Facade.ProductSubscriptionService.Cbms_CbmsPlusAutomaticSubscriptionOn(UserSessions.CurrentMerchantApp, productId);
                                //PXP-10890:Sanidhya end
                            }
                            else
                            {   //Both Ethoca and Verify are checked then Unsubscribe
                                PaymentXP.Facade.ProductSubscriptionService.TurnOffSubscription(UserSessions.CurrentMerchantApp, UserSessions.CurrentUser, ePortals.ZEUS, product);
                                //PXP-10890 Sanidhya Start
                                PaymentXP.Facade.ProductSubscriptionService.Cbms_CbmsPlusAutomaticSubscriptionOff(UserSessions.CurrentMerchantApp, productId);
                                //PXP-10890 Sanidhya End
                            }
                        }
                    }
                }
            }
        }

        //PXP-3169 Rohit Thakur  
        if (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
        {
            int productId = Constants.PRODUCT_CBMSPLUS;
            Product product = productList.FirstOrDefault<Product>(x => x.ProductId == productId);
            if (product != null)
            {
                if (ethocaChanged && verifiChanged)
                {
                    if (ethocaChecked.Equals(verifiChecked))
                    {
                        if (ethocaChecked)
                        {   //Both Ethoca and Verify are checked then Subscribe
                            PaymentXP.Facade.ProductSubscriptionService.Subscribe(UserSessions.CurrentMerchantApp, UserSessions.CurrentUser, ePortals.ZEUS, product);
                            //PXP-10890:Sanidhya Start
                            PaymentXP.Facade.ProductSubscriptionService.Cbms_CbmsPlusAutomaticSubscriptionOn(UserSessions.CurrentMerchantApp, productId);
                            //PXP-10890:Sanidhya end
                        }
                        else
                        {   //Both Ethoca and Verify are checked then Unsubscribe
                            PaymentXP.Facade.ProductSubscriptionService.TurnOffSubscription(UserSessions.CurrentMerchantApp, UserSessions.CurrentUser, ePortals.ZEUS, product);
                            //PXP-10890 Sanidhya Start
                            PaymentXP.Facade.ProductSubscriptionService.Cbms_CbmsPlusAutomaticSubscriptionOff(UserSessions.CurrentMerchantApp, productId);
                            //PXP-10890 Sanidhya End
                        }
                    }
                }
                else if (ethocaChanged || verifiChanged)  //If only one of them is changed
                {
                    if (ethocaChecked.Equals(verifiChecked)) //If both same 
                    {
                        if (ethocaChecked.Equals(false)) // If both UNchecked then UNsubscribe
                        {  //when Both Ethoca and Verify are checked then Unsubscribe if only one was changed
                            PaymentXP.Facade.ProductSubscriptionService.TurnOffSubscription(UserSessions.CurrentMerchantApp, UserSessions.CurrentUser, ePortals.ZEUS, product);
                            //PXP-10890 Sanidhya Start
                            PaymentXP.Facade.ProductSubscriptionService.Cbms_CbmsPlusAutomaticSubscriptionOff(UserSessions.CurrentMerchantApp, productId);
                            //PXP-10890 Sanidhya End
                        }
                        else
                        {
                            if (!IsSubscribed(productId))
                            {
                                PaymentXP.Facade.ProductSubscriptionService.Subscribe(UserSessions.CurrentMerchantApp, UserSessions.CurrentUser, ePortals.ZEUS, product);
                                //PXP-10890:Sanidhya Start
                                PaymentXP.Facade.ProductSubscriptionService.Cbms_CbmsPlusAutomaticSubscriptionOn(UserSessions.CurrentMerchantApp, productId);
                                //PXP-10890:Sanidhya end
                            }
                        }
                    }
                    else //If both Different 
                    {
                        if ((ethocaChanged && ethocaChecked) || (verifiChanged && verifiChecked))
                            PaymentXP.Facade.ProductSubscriptionService.Subscribe(UserSessions.CurrentMerchantApp, UserSessions.CurrentUser, ePortals.ZEUS, product);
                        //PXP-10890:Sanidhya Start
                        PaymentXP.Facade.ProductSubscriptionService.Cbms_CbmsPlusAutomaticSubscriptionOn(UserSessions.CurrentMerchantApp, productId);
                        //PXP-10890:Sanidhya end
                    }
                }
            }
        }

        if (sb.ToString() != string.Empty)
        {
            DataUser datauser = DataAccess.DataUserDao;
            datauser.InsertChangeLog(UserSessions.CurrentMerchantApp.BusinessDBAName, UserSessions.CurrentUser.UserName, MerchantAppUID, Convert.ToInt32(UserSessions.CurrentMerchantApp.ID), "Merchant Services", sb.ToString(), Constants.PORTAL_ZEUS);
        }

    }
    //PXP-10890 By Sanidhya:End

    public void LoadServices(string MerchantAppUID, ServiceCategories CategoryID, int RepeatColumns)
    {
        chkServices.RepeatColumns = RepeatColumns;

        DataMerchantServices data = new DataMerchantServices();
        Hashtable prms = new Hashtable();
        IList<MerchantServices> services = new List<MerchantServices>();
        prms.Add("@MerchantAppUID", DataLayer.UID2Field(MerchantAppUID));
        prms.Add("@CategoryID", CategoryID.GetHashCode());
        services = data.GetMerchantServicesWithoutProducts(prms);
        switch (CategoryID)
        {
            case ServiceCategories.DEPLOYMENT:
                DataMerchantApp _obj = new DataMerchantApp();
                var _data = new Hashtable();
                _data.Add("@MerchantMID", UserSessions.CurrentMerchantApp.SettlePlatformMid);
                DataTable _result = _obj.GetStatusByMerchantMID(_data);
                // Adding an extra check for "XCaliber CB Data"
                services.Add(new MerchantServices()
                {                   
                    Checked = _result.Rows.Count == 0 ? false : (bool)_result.Rows[0].ItemArray[0],
                    Description = "XCaliber CB Data",
                    MerchantServiceID = "checkXcaliberMID"
                });
                break;
        }        
        chkServices.DataSource = services;
        chkServices.DataTextField = "Description";
        chkServices.DataValueField = "MerchantServiceID";
        
        chkServices.DataBind();
        
        foreach (MerchantServices service in services)
        {
            chkServices.Items.FindByValue(service.MerchantServiceID).Selected = service.Checked;
        }
    }

    private bool IsSubscribed(int productId)
    {
        List<PaymentXP.BusinessObjects.Subscription> SubscriptionList = null;
        SubscriptionList = PaymentXP.DataObjects.DataProduct.GetMerchantCurrentProductSubscriptionList(int.Parse(UserSessions.CurrentMerchantApp.ID));
        if (SubscriptionList != null)
        {
            Subscription subscription = SubscriptionList.FirstOrDefault<Subscription>(x => x.Product.ProductId == productId);
            if (subscription != null && subscription.IsActive)
            {
                return true;
            }
        }
        return false;
    }
}
