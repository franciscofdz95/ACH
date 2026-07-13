using CommonUtility;
using CommonUtility.Extensions;
using Newtonsoft.Json;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Tsys.TsysUtility;

namespace ZeusWeb.Class
{
    public class NMIMerchantOnboardingAPI
    {
        private string Version = "LOCKED";
        private DataTable dt = new DataTable();
        private string AuthorizationKey = string.Empty;
        private string PlatformKey = string.Empty;
        // string AffiliateProcessorConfig = string.Empty;

        public NMIMerchantOnboardingAPI(string version)
        {
            Version = version;
            if (Version.Equals("LOCKED"))
            {
                AuthorizationKey = ConfigurationManager.AppSettings["LockdownAuthKey"];
                PlatformKey = "LockdownPlatform";
            }
            else
            {
                AuthorizationKey = ConfigurationManager.AppSettings["AffiliateAuthKey"];
                PlatformKey = "AffiliatePlatform";
            }
        }
        private NMIApiResponse PostGatewayAccount(GatewayAccounts gatewayAccount, string PlanID, string PlanName)
        {
            MerchantFacade facade = new MerchantFacade();
            NMIApiResponse result = new NMIApiResponse();
            Logging.NMIMerchantOnboardingAPIog.Info("Posting to NMI");
            string apiType = Version.Equals("LOCKED") ? "LOCKDOWN_MERCHANT" : "AFFILIATE_MERCHANT";
            try
            {
                string jsonresponse = string.Empty;
                JavaScriptSerializer Serializer = new JavaScriptSerializer();
                Crypto crypto = new Crypto();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //Required for SSL certificate validation.
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (se, cert, chain, sslerror) =>
                {
                    return true;
                };

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["NMIMerchantGatewayURL"]);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", AuthorizationKey);

                string gatewayRequestJson = string.Empty;
                try
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        gatewayRequestJson = JsonConvert.SerializeObject(gatewayAccount, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("NMI Request String: {0} ", gatewayRequestJson);
                        streamWriter.Write(gatewayRequestJson);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    ServicePointManager.Expect100Continue = true;
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            MerchantApp app = UserSessions.CurrentMerchantApp;
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                jsonresponse = reader.ReadToEnd();
                                GatewayAccountResponse ackObject = Serializer.Deserialize<GatewayAccountResponse>(jsonresponse);
                                result.message = ackObject.id.ToString();
                                Logging.NMIMerchantOnboardingAPIog.InfoFormat("NMI Response String: {0} ", jsonresponse);

                                if (Version.Equals("LOCKED"))
                                {
                                    app.LockdownNMIId = ackObject.id;
                                    app.LockdownKey = ackObject.security_key;
                                    app.LockdownNMIUsername = ackObject.username;
                                    app.LockdownDate = DateTime.Now;
                                    app.LockdownPlanId = PlanID;
                                }
                                else
                                {
                                    app.AffiliateNMIId = ackObject.id;
                                    app.AffiliateKey = ackObject.security_key;
                                    app.AffiliateUsername = ackObject.username;
                                    app.AffiliateDate = DateTime.Now;
                                    app.AffiliatePlanId = PlanID;
                                 //   AffiliateProcessorConfig = ackObject.security_key;
                                }

                                facade.SaveNMIMerchantAPI(UserSessions.CurrentMerchantApp);
                                try
                                {
                                    int rows = facade.SaveNMIMerchantAPIResponse(UserSessions.CurrentMerchantApp.ID, apiType, gatewayRequestJson, jsonresponse.ToString());
                                }
                                catch (Exception exx)
                                {
                                    Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception PostGatewayAccount() -> SaveNMIMerchantAPIResponse: {0}", exx.Message);
                                }
                                result.IsError = false;
                                return result;
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    WebResponse errorResponse = ex.Response;
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                        jsonresponse = reader.ReadToEnd();
                        result = Serializer.Deserialize<NMIApiResponse>(jsonresponse);
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Error  Response String: {0} ", jsonresponse);
                    }
                    try
                    {
                        facade.SaveNMIMerchantAPIResponse(UserSessions.CurrentMerchantApp.ID, apiType, gatewayRequestJson, jsonresponse.ToString());
                    }
                    catch(Exception exx)
                    {
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception PostGatewayAccount() -> SaveNMIMerchantAPIResponse: {0}", exx.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception PostGatewayAccount(): {0}", ex.Message);
                return result;
            }

            return result;

        }
        private NMIApiResponse PostProcessorAccount(ProcessorAccount processorAccount, string gatewayid)
        {
            Logging.NMIMerchantOnboardingAPIog.Info("Posting processor to NMI");
            NMIApiResponse result = new NMIApiResponse();
            try
            {
                string jsonresponse = string.Empty;
                Crypto crypto = new Crypto();
                string apiType = Version.Equals("LOCKED") ? "LOCKDOWN_PROCESSOR" : "AFFILIATE_PROCESSOR";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //Required for SSL certificate validation.
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (se, cert, chain, sslerror) =>
                {
                    return true;
                };

                string proceesorurl = string.Format(ConfigurationManager.AppSettings["NMIMerchantProcessorURL"].Trim(), gatewayid);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(proceesorurl);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", AuthorizationKey);

                string processorRequestJson = string.Empty;
                try
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        processorRequestJson = JsonConvert.SerializeObject(processorAccount, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Processor Request String: {0} ", processorRequestJson);
                        streamWriter.Write(processorRequestJson);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    ServicePointManager.Expect100Continue = true;
                    //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                jsonresponse = reader.ReadToEnd();
                                Logging.NMIMerchantOnboardingAPIog.InfoFormat("Processor Response String: {0} ", jsonresponse);
                            }
                            result.IsError = false;
                        }
                        try
                        {
                            MerchantFacade facade = new MerchantFacade();
                            int rows = facade.SaveNMIMerchantAPIResponse(UserSessions.CurrentMerchantApp.ID, apiType, processorRequestJson, jsonresponse.ToString());
                        }
                        catch (Exception exx)
                        {
                            Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception PostProcessorAccount() -> SaveNMIMerchantAPIResponse: {0}", exx.Message);
                        }
                    }
                }
                catch (WebException ex)
                {
                    WebResponse errorResponse = ex.Response;
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                        jsonresponse = reader.ReadToEnd();
                        JavaScriptSerializer Serializer = new JavaScriptSerializer();
                        result = Serializer.Deserialize<NMIApiResponse>(jsonresponse);
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Error Processor Response String: {0} ", jsonresponse);
                    }
                    try
                    {
                        MerchantFacade facade = new MerchantFacade();
                        int rows = facade.SaveNMIMerchantAPIResponse(UserSessions.CurrentMerchantApp.ID, apiType, processorRequestJson, jsonresponse.ToString());
                    }
                    catch (Exception exx)
                    {
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception PostProcessorAccount() -> SaveNMIMerchantAPIResponse: {0}", exx.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception PostProcessorAccount(): {0}", ex.Message);

            }
            return result;

        }
        private NMIApiResponse PostServiceConfiguration(string gatewayid, string custVaultStatus)
        {
            NMIApiResponse result = new NMIApiResponse();
            ServiceConfiguration serviceconfig = new ServiceConfiguration();
            serviceconfig.customer_vault = new CustomerVault();
            serviceconfig.customer_vault.status = custVaultStatus;

            Logging.NMIMerchantOnboardingAPIog.Info("Posting service config to NMI");

            try
            {
                string jsonresponse = string.Empty;
                Crypto crypto = new Crypto();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //Required for SSL certificate validation.
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (se, cert, chain, sslerror) =>
                {
                    return true;
                };


                string proceesorurl = string.Format(ConfigurationManager.AppSettings["NMIMerchantServiceConfigURL"].Trim(), gatewayid);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(proceesorurl);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", AuthorizationKey);

                string serviceConfigJson = string.Empty;
                try
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        serviceConfigJson = JsonConvert.SerializeObject(serviceconfig, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Service config Request String: {0} ", serviceConfigJson);
                        streamWriter.Write(serviceConfigJson);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    ServicePointManager.Expect100Continue = true;
                    //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                jsonresponse = reader.ReadToEnd();
                                //  Logging.NMILoadLog.InfoFormat("Service config Response String: {0} ", jsonresponse);
                                result.IsError = false;
                            }
                            try
                            {
                                MerchantFacade facade = new MerchantFacade();
                                string apiType = Version.Equals("LOCKED") ? "LOCKDOWN_SERVICE_CONFIG" : "AFFILIATE_SERVICE_CONFIG";
                                int rows = facade.SaveNMIMerchantAPIResponse(UserSessions.CurrentMerchantApp.ID, apiType, serviceConfigJson, jsonresponse.ToString());
                            }
                            catch (Exception exx)
                            {
                                Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception PostServiceConfiguration() -> SaveNMIMerchantAPIResponse: {0}", exx.Message);
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    WebResponse errorResponse = ex.Response;
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                        jsonresponse = reader.ReadToEnd();
                        JavaScriptSerializer Serializer = new JavaScriptSerializer();
                        result = Serializer.Deserialize<NMIApiResponse>(jsonresponse);
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Error Service config  Response String: {0} ", jsonresponse);
                    }
                    try
                    {
                        MerchantFacade facade = new MerchantFacade();
                        string apiType = Version.Equals("LOCKED") ? "LOCKDOWN_SERVICE_CONFIG" : "AFFILIATE_SERVICE_CONFIG";
                        int rows = facade.SaveNMIMerchantAPIResponse(UserSessions.CurrentMerchantApp.ID, apiType, serviceConfigJson, jsonresponse.ToString());
                    }
                    catch (Exception exx)
                    {
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception PostServiceConfiguration() -> SaveNMIMerchantAPIResponse: {0}", exx.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception Service config PostServiceConfiguration(): {0}", ex.Message);

            }
            return result;
        }
        private NMIApiResponse PostTermsOfService(ServiceTerms terms, string gatewayid)
        {
            NMIApiResponse result = new NMIApiResponse();
            Logging.NMIMerchantOnboardingAPIog.Info("Posting Terms to NMI");

            try
            {
                string jsonresponse = string.Empty;
                Crypto crypto = new Crypto();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //Required for SSL certificate validation.
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (se, cert, chain, sslerror) =>
                {
                    return true;
                };

                string termofServiceUrl = string.Format(ConfigurationManager.AppSettings["NMIMerchantTermsOfServiceURL"].Trim(), gatewayid);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(termofServiceUrl);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", AuthorizationKey);

                string processorRequestJson = string.Empty;
                try
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        processorRequestJson = JsonConvert.SerializeObject(terms, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("TOS Request String: {0} ", processorRequestJson);
                        streamWriter.Write(processorRequestJson);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    ServicePointManager.Expect100Continue = true;
                    //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                jsonresponse = reader.ReadToEnd();
                                Logging.NMIMerchantOnboardingAPIog.InfoFormat("TOS Response String: {0} ", jsonresponse);
                                result.IsError = false;
                            }
                            try
                            {
                                MerchantFacade facade = new MerchantFacade();
                                string apiType = Version.Equals("LOCKED") ? "LOCKDOWN_TOS" : "AFFILIATE_TOS";
                                int rows = facade.SaveNMIMerchantAPIResponse(UserSessions.CurrentMerchantApp.ID, apiType, processorRequestJson, jsonresponse.ToString());
                            }
                            catch (Exception exx)
                            {
                                Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception PostTermsOfService() -> SaveNMIMerchantAPIResponse: {0}", exx.Message);
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    WebResponse errorResponse = ex.Response;
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                        jsonresponse = reader.ReadToEnd();
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Error TOS Response String: {0} ", jsonresponse);
                        JavaScriptSerializer Serializer = new JavaScriptSerializer();
                        result = Serializer.Deserialize<NMIApiResponse>(jsonresponse);
                    }
                    try
                    {
                        MerchantFacade facade = new MerchantFacade();
                        string apiType = Version.Equals("LOCKED") ? "LOCKDOWN_TOS" : "AFFILIATE_TOS";
                        int rows = facade.SaveNMIMerchantAPIResponse(UserSessions.CurrentMerchantApp.ID, apiType, processorRequestJson, jsonresponse.ToString());
                    }
                    catch (Exception exx)
                    {
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception PostTermsOfService() -> SaveNMIMerchantAPIResponse: {0}", exx.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception TOS PostTermsOfService(): {0}", ex.Message);

            }
            return result;
            //Feedback.CallStackList.Add(Call);

        }
        public NMIApiResponse LoadGatewayAccounts(MerchantApp app, string PlanID, string PlanName)
        {
            NMIApiResponse result = new NMIApiResponse();
            GatewayAccounts gatewayAccount = new GatewayAccounts();
            string NmiApiErrorMsg = Version.Equals("LOCKED") ? "Merchant boarding failed at NMI lock-down. " : "Merchant boarding failed at NMI Paysafe Affiliate. ";

            gatewayAccount.address_1 = app.BusinessAddress.RemoveSpecialCharactersNoWhiteSpace();
            gatewayAccount.address_2 = app.BusinessAddressLine2.RemoveSpecialCharactersNoWhiteSpace();
            gatewayAccount.city = app.BusinessCity.RemoveSpecialCharactersNoWhiteSpace();
            gatewayAccount.company = app.BusinessDBAName.RemoveSpecialCharactersNoWhiteSpace();
            if (app.ContactList != null)
            {
                var contactList = app.ContactList.FirstOrDefault();
                gatewayAccount.contact_first_name = contactList.FirstName.RemoveSpecialCharactersNoWhiteSpace();
                gatewayAccount.contact_last_name = contactList.LastName.RemoveSpecialCharactersNoWhiteSpace();
                gatewayAccount.contact_email = Version.Equals("LOCKED") ? Constants.NMIlockdown_defaultEmail : contactList.EmailAddress;
                if (contactList.PhoneList != null)
                {
                    gatewayAccount.contact_phone = contactList.PhoneList.FirstOrDefault().PhoneNumber.RemoveSpecialCharactersNoWhiteSpace();
                }
            }
            gatewayAccount.country = app.BusinessCountry.RemoveSpecialCharactersNoWhiteSpace();

            gatewayAccount.fee_plan = Convert.ToInt32(PlanName);
            gatewayAccount.locale = "en_US";
            gatewayAccount.postal = app.BusinessZip.RemoveSpecialCharactersNoWhiteSpace();
            gatewayAccount.state = app.BusinessState.RemoveSpecialCharactersNoWhiteSpace();
            gatewayAccount.timezone_id = 6;
            gatewayAccount.url = app.BusinessWebsite;
            string businessDBAName = app.BusinessDBAName.RemoveSpecialCharacters();
            if (Version.Equals("LOCKED"))
            {
                gatewayAccount.username = "PXP" + businessDBAName.Substring(0, Math.Min(businessDBAName.Length, 20));//PXP-11312 bug fix asheesh               
            }
            else
            {
                gatewayAccount.username = app.ID + businessDBAName.Substring(0, Math.Min(businessDBAName.Length, 20));
            }
            if (!Version.Equals("LOCKED"))
            {
                gatewayAccount.account_number = app.AccountNumber.RemoveSpecialCharactersNoWhiteSpace();
                gatewayAccount.routing_number = app.RoutingNumber.RemoveSpecialCharactersNoWhiteSpace();
            }
            gatewayAccount.external_identifier = app.ID;
            gatewayAccount.generate_security_key = true;
            gatewayAccount.security_key_type = "api";

            result = PostGatewayAccount(gatewayAccount, PlanID, PlanName);
            string gatewayId = result.message;
            ProcessorAccount processorAccount = new ProcessorAccount();

            ///DM-5800 by Jorge 
            processorAccount.platform = (Version.Equals("LOCKED") ? NMIMerchantOnboardingAPI.GetPlatform(app) : Platform.PaysafeContinuity).GetDescription();
            processorAccount.default_industry_classification = "ecommerce";
            processorAccount.max_transaction_amount = "99999.00";
            processorAccount.max_monthly_volume = "0.00";
            processorAccount.enable_duplicate_checking = false;
            processorAccount.allow_duplicate_checking_override = false;
            processorAccount.duplicate_checking_seconds = 0;
            processorAccount.processor_description = app.Descriptor;
            processorAccount.mcc = Convert.ToInt32(app.SicCode.RemoveSpecialCharactersNoWhiteSpace());
            processorAccount.currencies = "USD";
            processorAccount.payment_types = "visa, mastercard, amex, discover";
            processorAccount.required_fields = string.Empty;
            if (Version.Equals("LOCKED"))
            {               
                //PXP-13342 by Satyajit
                processorAccount.processor_config_3 = app.IsNutraMerchant;
                if (app.VerticalMarket_BillingTypes != null && app.VerticalMarket_BillingTypes.Count > 0)
                {
                    if (app.VerticalMarket_BillingTypes.ContainsKey("DG"))
                        processorAccount.processor_config_4 = app.VerticalMarket_BillingTypes["DG"];
                    if (app.VerticalMarket_BillingTypes.ContainsKey("FT"))
                        processorAccount.processor_config_5 = app.VerticalMarket_BillingTypes["FT"];
                    if (app.VerticalMarket_BillingTypes.ContainsKey("Continuity"))
                        processorAccount.processor_config_6 = app.VerticalMarket_BillingTypes["Continuity"];
                    if (app.VerticalMarket_BillingTypes.ContainsKey("OTS"))
                        processorAccount.processor_config_7 = app.VerticalMarket_BillingTypes["OTS"];
                }

                ///DM-5800 by Jorge
                if (processorAccount.platform == Platform.FirstDataNashville.GetDescription())
                {
                    processorAccount.processor_config_1 = app.AuthPlatformMid;
                    processorAccount.processor_config_2 = app.TID;
                    processorAccount.processor_config_3 = true;
                    processorAccount.processor_config_4 = null;
                    processorAccount.processor_config_5 = null;
                    processorAccount.processor_config_6 = null;
                    processorAccount.processor_config_7 = null;
                    if (ConfigurationManager.AppSettings.AllKeys.Contains("NMISettlementTime"))
                    {
                        processorAccount.settlement_time = ConfigurationManager.AppSettings["NMISettlementTime"];
                    }
                }
                else if (processorAccount.platform == Platform.PaysafeProcessingPxP.GetDescription())
                {
                    processorAccount.processor_config_1 = app.ID;
                    processorAccount.processor_config_2 = app.MerchantKey;
                }
                ///END DM-5800
            }
            else
            {
                // processorAccount.processor_config_1 = AffiliateProcessorConfig;
                processorAccount.processor_config_1 = app.LockdownKey;

                processorAccount.processor_config_3 = null;
                processorAccount.processor_config_4 = null;
                processorAccount.processor_config_5 = null;
                processorAccount.processor_config_6 = null;
                processorAccount.processor_config_7 = null;

            }
            ServiceTerms terms = new ServiceTerms();
            terms.agree_to_billing_auth = true;
            terms.agree_to_tos = true;

            if (result != null && !result.IsError)
            {
                if (!string.IsNullOrEmpty(gatewayId))
                {
                    result = PostProcessorAccount(processorAccount, gatewayId);
                    if (result.IsError) 
                    {
                        result.message = NmiApiErrorMsg + result.message ?? "";
                        var deletedResult = DeleteGatewayAccount(gatewayId); // DM-5815 by Jorge
                        return result;
                    }
                       
                    if (!Version.Equals("LOCKED"))
                    {
                        if (app.VerticalMarket_BillingTypes["FT"] || app.VerticalMarket_BillingTypes["Continuity"])
                            result = PostServiceConfiguration(gatewayId, "active");
                        else if (app.VerticalMarket_BillingTypes["OTS"])
                            result = PostServiceConfiguration(gatewayId, "offered");
                         
                        if (result.IsError)
                        {
                            result.message = NmiApiErrorMsg + result.message ?? "";
                            DeleteGatewayAccount(gatewayId); // DM-5815 by Jorge
                            return result;
                        }
                    }
                    result = PostTermsOfService(terms, gatewayId);
                    if (result.IsError)
                    {
                        result.message = NmiApiErrorMsg + result.message ?? "";
                        DeleteGatewayAccount(gatewayId); // DM-5815 by Jorge
                        return result;
                    }
                    result.IsError = false;
                    result.message = "Merchant boarded onto NMI Paysafe Affiliate successfully";
                }
                else
                {
                    result.message = NmiApiErrorMsg;
                }
            }
            else if (result != null)
            {
                result.message = NmiApiErrorMsg +  result.message ?? "";
            }
            return result;
        }

        /// DM-5815 by Jorge
        private NMIApiResponse SetGatewayAccountStatus(string gatewayid, NMIMerchantStatus status)
        {
            NMIApiResponse result = new NMIApiResponse();
            Logging.NMIMerchantOnboardingAPIog.Info("Set Merchant Status");

            try
            {
                string jsonresponse = string.Empty;
                //Crypto crypto = new Crypto();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //Required for SSL certificate validation.
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (se, cert, chain, sslerror) =>
                {
                    return true;
                };

                string merchantstatusurl = string.Format(ConfigurationManager.AppSettings["NMIMerchantStatusURL"].Trim(), gatewayid);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(merchantstatusurl);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", AuthorizationKey);

                string processorRequestJson = string.Empty;
                try
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        processorRequestJson = JsonConvert.SerializeObject(new { set_merchant_status = status.ToString() }, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Set Status Request String: {0} ", processorRequestJson);
                        streamWriter.Write(processorRequestJson);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    ServicePointManager.Expect100Continue = true;
                    //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream responseStream = response.GetResponseStream())
                            {
                                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                                jsonresponse = reader.ReadToEnd();
                                Logging.NMIMerchantOnboardingAPIog.InfoFormat("Set Status Response String: {0} ", jsonresponse);
                                result.IsError = false;
                            }
                            try
                            {
                                MerchantFacade facade = new MerchantFacade();
                                string apiType = Version.Equals("LOCKED") ? "LOCKDOWN_SetStatus" : "AFFILIATE_SetStatus";
                                int rows = facade.SaveNMIMerchantAPIResponse(UserSessions.CurrentMerchantApp.ID, apiType, processorRequestJson, jsonresponse.ToString());
                            }
                            catch (Exception exx)
                            {
                                Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception SetMerchantStatus() -> SetMerchantStatusAPIResponse: {0}", exx.Message);
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    WebResponse errorResponse = ex.Response;
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                        jsonresponse = reader.ReadToEnd();
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Error Set Status Response String: {0} ", jsonresponse);
                        JavaScriptSerializer Serializer = new JavaScriptSerializer();
                        result = Serializer.Deserialize<NMIApiResponse>(jsonresponse);
                    }
                    try
                    {
                        MerchantFacade facade = new MerchantFacade();
                        string apiType = Version.Equals("LOCKED") ? "LOCKDOWN_SetStatus" : "AFFILIATE_SetStatus";
                        int rows = facade.SaveNMIMerchantAPIResponse(UserSessions.CurrentMerchantApp.ID, apiType, processorRequestJson, jsonresponse.ToString());
                    }
                    catch (Exception exx)
                    {                        
                        Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception SetMerchantStatus() -> SetMerchantStatusAPIResponse: {0}", exx.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Logging.NMIMerchantOnboardingAPIog.InfoFormat("Exception SetMerchantStatus() -> SetMerchantStatusAPIResponse: {0}", ex.Message);

            }
            return result;

        }
        private NMIApiResponse DeleteGatewayAccount(string gatewayid)
        {
            MerchantFacade facade = new MerchantFacade();

            var deletedResult = SetGatewayAccountStatus(gatewayid, NMIMerchantStatus.deleted);
            if (!deletedResult.IsError)
            {
                if (Version.Equals("LOCKED"))
                {
                    UserSessions.CurrentMerchantApp.LockdownNMIId = 0;
                    UserSessions.CurrentMerchantApp.LockdownKey = string.Empty;
                    UserSessions.CurrentMerchantApp.LockdownNMIUsername = string.Empty;
                    UserSessions.CurrentMerchantApp.LockdownDate = DateTime.MinValue;
                    UserSessions.CurrentMerchantApp.LockdownPlanId = string.Empty;
                }
                else
                {
                    UserSessions.CurrentMerchantApp.AffiliateNMIId = 0;
                    UserSessions.CurrentMerchantApp.AffiliateKey = string.Empty;
                    UserSessions.CurrentMerchantApp.AffiliateUsername = string.Empty;
                    UserSessions.CurrentMerchantApp.AffiliateDate = DateTime.MinValue;
                    UserSessions.CurrentMerchantApp.AffiliatePlanId = string.Empty;
                }
                facade.SaveNMIMerchantAPI(UserSessions.CurrentMerchantApp);
            }

            return deletedResult;
        }
        /// end DM-5815

        ///DM-5800 by Jorge 
        public static Platform GetPlatform(MerchantApp app)
        {
            bool BillingTypeFT = (app.VerticalMarket_BillingTypes?.TryGetValue("FT", out bool billingFT) ?? false) && billingFT;
            bool BillingTypeContinuity = (app.VerticalMarket_BillingTypes?.TryGetValue("Continuity", out bool billingContinuity) ?? false) && billingContinuity;
            bool BillingTypeOTS = (app.VerticalMarket_BillingTypes?.TryGetValue("OTS", out bool billingOts) ?? false) && billingOts;
            
            if (!app.IsNutraMerchant && BillingTypeOTS && !BillingTypeFT && !BillingTypeContinuity)
            {
                return Platform.FirstDataNashville;
            }
            else if (!app.IsNutraMerchant && !BillingTypeOTS && !BillingTypeFT && !BillingTypeContinuity)
            {
                return Platform.NoPlatform;
            }
            else //BillingTypeFT or BillingTypeContinuity are true
            {
                return Platform.PaysafeProcessingPxP;
            }
        }

    }
}