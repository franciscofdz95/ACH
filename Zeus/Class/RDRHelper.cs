using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeusWeb.Class
{
    public static class RDRHelper
    {
        public static readonly IReadOnlyList<int> rdrProducts = new List<int>() { 92, 93 };
        public static int RDR_PRODUCT_ID = 92;
        public static int RDR_SETUP_PRODUCT_ID = 93;

        public static void AutoSubcribeFromApplicacionXP(MerchantApp merchant, User user, string excelTemplatePath)
        {
            if (merchant.IsRDRChecked && !merchant.IsRDRSubscribed)
            {
                if (DataRDR.CopyAppsRulesToMerchantRules(merchant.ID, user.FirstName))
                {
                    Subscribe(merchant, user, excelTemplatePath);
                }
            }
        }

        public static void Subscribe(MerchantApp merchant, User user, string excelTemplatePath)
        {
            try
            {
                List<Product> productList = DataProduct.GetAgentProductList(new Guid(merchant.AgentUID), int.Parse(merchant.ID), false, merchant.Brand);

                foreach (Product _product in productList.Where(x => rdrProducts.Contains(x.ProductId)))
                {
                    PaymentXP.Facade.ProductSubscriptionService.Subscribe(merchant, user, ePortals.ZEUS, _product);
                }

                VerifiRequestingEnrollment(merchant, RDR_SETUP_PRODUCT_ID, SubscriptionAction.Enrollment, excelTemplatePath);

                ProductSubscriptionService.SendMerchantSubscribeEmail(merchant, user, ePortals.ZEUS);
            }
            catch (Exception ex)
            {
                ZeusWeb.Logging.ErrorLog.Error(String.Format("Error Exception happened. Failed to subscribet RDR For MerchantId: {0}", merchant.ID, ex.Message), ex);
            }

        }

        public static void VerifiRequestingEnrollment(MerchantApp merchant, int productId, SubscriptionAction subscriptionAction, string excelTemplatePath)
        {
            try
            {
                string excelFilename = string.Format("Verifi RDR {0} Form.xls", subscriptionAction.GetAttribute<ActionAttribute>().ActionText);
                bool isRuleVisible = subscriptionAction != SubscriptionAction.Unenrollment;

                Dictionary<string, byte[]> excelAttach = ProductSubscriptionService.CreateVerifiExcel(merchant, ePortals.ZEUS, isRuleVisible, productId, excelFilename, excelTemplatePath);
                if (excelAttach != null && excelAttach.ContainsKey(excelFilename))
                {
                    var excelAttachHash = new Hashtable(excelAttach);
                    var isSent = ProductSubscriptionService.SendVerifiRequestingEnrollmentEmail(merchant, RDR_SETUP_PRODUCT_ID, UserSessions.CurrentUser.UserName, ePortals.ZEUS, subscriptionAction, excelAttachHash);
                    if (!isSent)
                    {
                        ZeusWeb.Logging.ErrorLog.Info("Email to Verifi could not be sent.");
                    }

                    //Upload document to Zeus
                    string filename = String.Format("Presolved_{0}_{1}.xls", merchant.ID, DateTime.Now.ToString("MMddyy_HHmmss"));
                    var fu = new ZeusWeb.MDocWS.FileUpload();
                    fu.Url = Constants.COF_MDOCWS_FILE_UPLOAD;
                    byte[] _attach = excelAttach[excelFilename];
                    ZeusWeb.MDocWS.UploadResponse resp = fu.UploadFileWithSourceAndUser(_attach, Convert.ToInt32(merchant.ID), merchant.MerchantAppUID, merchant.AgentID, merchant.AgentUID, (int)MDoc.eMDocType.Visa_RDR_Form, filename, ePortals.ZEUS.ToString().ToLower(), 0, subscriptionAction.ToString(), "", Convert.ToInt32(merchant.ID), (int)MDoc.eMDocSourceID.Merchant, UserSessions.CurrentUser.UserName);
                }
                else
                {
                    ZeusWeb.Logging.ErrorLog.Info("Created Verifi Excel failed.");
                }
            }
            catch (Exception ex)
            {
                ZeusWeb.Logging.ErrorLog.Error(String.Format("Error Exception happened. Failed to create and to sent RDR Verify Email with Excel subscription Form For MerchantId: {0} ProductId: RDR ", merchant.ID), ex);
            }
        }
    }
}
