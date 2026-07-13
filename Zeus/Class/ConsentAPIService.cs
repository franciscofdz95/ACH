using System.Web.Script.Serialization;
using PaymentXP.BusinessObjects.Zeus;
using System.IO;
using System.Net;
using System;
using PaymentXP.Facade;
using Newtonsoft.Json;
using PaymentXP.DataObjects;
using System.Collections;
using System.Linq;
using System.Text;
using CommonUtility;

namespace ZeusWeb.Class
{
    //START:PXP-11432 Implement Consumer Consent API Call for M1 CRM By Ali
    public static class ConsentAPIService
    {
        public static JavaScriptSerializer Serializer = new JavaScriptSerializer();
        public static ConsentAPIResponse GetResponse(string nmiTransactionId, int CRMID)
        {
            ConsentAPIResponse CRMApiResponse = null;
            Hashtable prms = new Hashtable();
            DataApp _dataApp = new DataApp();
            var _crmData = _dataApp.GetCRMList(prms).FirstOrDefault(x => x.CRMID == CRMID);
            if (_crmData != null && !string.IsNullOrEmpty(_crmData.EndPointUrl))
            {
                MerchantFacade facade = new MerchantFacade();
                Logging.ConsentAPILog.InfoFormat("[NMITransactionId={0}] Start the service......", nmiTransactionId);
                string EndPointCRMAPIUrl = string.Empty;
                string jsonresponse = string.Empty;
                Logging.ConsentAPILog.InfoFormat("[NMITransactionId={0}] Start the service for {1}......", nmiTransactionId, _crmData.Name);
                if (_crmData.IsTranInEndPoint)
                {
                    EndPointCRMAPIUrl = string.Format(_crmData.EndPointUrl, nmiTransactionId);
                }
                else
                {
                    EndPointCRMAPIUrl = _crmData.EndPointUrl;
                }                
                
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(EndPointCRMAPIUrl);
                    httpWebRequest.Timeout = 1200000;
                    httpWebRequest.Method = "GET";

                    if (_crmData.HasHeaders)
                    {
                        var headers = _dataApp.GetCRMHeaders(new Hashtable() { { "@CRMID", _crmData.CRMID } });
                        APIHandler.SetHeaders(httpWebRequest, headers, Logging.ConsentAPILog);
                    }

                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    if (_crmData.IsAuthenticationBearer)
                    {
                        httpWebRequest.Headers.Add("Authorization", "Bearer " + _crmData.BearerValue);
                    }
                    else if (_crmData.IsAuthenticationBasic)
                    {
                        Crypto crypto = new Crypto();
                        string crmUserName = string.Empty;
                        string crmPassword = string.Empty;
                        string authCredentials = string.Empty;
                        if (!string.IsNullOrEmpty(_crmData.UserName))
                            crmUserName = crypto.Decrypt(_crmData.UserName);
                        if (!string.IsNullOrEmpty(_crmData.Password))
                            crmPassword = crypto.Decrypt(_crmData.Password);
                         authCredentials = crmUserName + ":" + crmPassword;

                        httpWebRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(authCredentials)));
                    }
                    else if(_crmData.IsXApiKey)
                    {
                        httpWebRequest.Headers.Add("x-api-key", _crmData.XApiKey);
                    }

                    httpWebRequest.KeepAlive = true;

                    using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                jsonresponse = reader.ReadToEnd();
                                Logging.ConsentAPILog.InfoFormat("[NMITransactionId={0}] Get response to {1}: {2}", nmiTransactionId, CRMID, jsonresponse);
                            }
                            CRMApiResponse = JsonConvert.DeserializeObject<ConsentAPIResponse>(jsonresponse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                        }
                    }


                }
                catch (Exception ex)
                {
                    Logging.ConsentAPILog.ErrorFormat("[NMITransactionId={0}] Failed to send request to {1}: {2}", nmiTransactionId, CRMID, ex.Message);
                }
                try
                {
                    //PXP-10067 abarua
                    string ResponseData = Serializer.Serialize(CRMApiResponse);
                    int rows = facade.SaveCRMConsentRequestLog(nmiTransactionId, _crmData.Name, UserSessions.CurrentUser.UserName, EndPointCRMAPIUrl, ResponseData);
                }
                catch (Exception exc)
                {
                    Logging.ConsentAPILog.ErrorFormat("[NMITransactionId={0}] Failed {1}: {2}", nmiTransactionId, CRMID, exc.Message);
                }
                return CRMApiResponse;
            }
            else 
            {
                CRMApiResponse = new ConsentAPIResponse 
                {
                    ResponseMsg="Invalid CRM",
                    ResponseCode=500
                };
                return CRMApiResponse;
            }
        }

    }
    //END:PXP-11432 Implement Consumer Consent API Call for M1 CRM By Ali
}