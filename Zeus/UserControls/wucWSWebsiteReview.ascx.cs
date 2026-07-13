using iTextSharp.text;
using iTextSharp.text.pdf;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZeusWeb.UserControls
{
    public partial class wucWSWebsiteReview : wucBaseDataEntry
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pnlWebsiteReview.Visible = false;
            if (HasRoles())
            {
                pnlWebsiteReview.Visible = true;
            }
        }

        public bool HasRoles()
        {
            return UserSessions.CurrentUser
                .UserRoles
                .Any(p => p.Value.RoleID.ToUpper() == Constants.ROLE_CREDIT_UNDERWRITING || p.Value.RoleID.ToUpper() == Constants.ROLE_RISK);
        }

        public void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            var IsSuccess = true;
            var _Date = DateTime.Now;

            if (rq26.SelectedValue == "Yes")
            {
                if (rh6.Text.Length == 0)
                {
                    wucWsReviewMessage2.AddMessageError("Required: Number URLs Submitting to CU to Date");
                    IsSuccess = false;
                }

                if (rq5.SelectedValue == "Yes" && rq6.Text.Length == 0)
                {
                    wucWsReviewMessage2.AddMessageError("Required: Provide unrelated URL(s)");
                    IsSuccess = false;
                }

                if (rq28.Text.Length == 0)
                {
                    wucWsReviewMessage2.AddMessageError("Required: How long is the trial period?");
                    IsSuccess = false;
                }
            }
            //Begin DM-3933
            if (ddl_lq3.SelectedValue == "No" && String.IsNullOrEmpty(rq3.Text.Trim())) //This validates at least 1 char and not null
            {
                wucWsReviewMessage2.AddMessageError("Required: Is the website registered to the merchant of record? is required to have at least 1 character entered or is needed to be answered");
                IsSuccess = false;
            } //End DM-3933

            if (!IsSuccess)
            {
                return;
            }

            string _descriptor = !string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.HighRiskDescriptor) ? UserSessions.CurrentMerchantApp.HighRiskDescriptor : UserSessions.CurrentMerchantApp.Descriptor;

            var custom = new Dictionary<string, string>();
            custom.Add("Date", _Date.ToString("MM-dd-yyyy"));
            custom.Add("Website URL1", UserSessions.CurrentMerchantApp.BusinessWebsite);
            custom.Add("DBA Name", UserSessions.CurrentMerchantApp.BusinessDBAName);
            custom.Add("ZID", UserSessions.CurrentMerchantApp.ID);
            custom.Add("Descriptor", _descriptor);
            custom.Add("URLs", rh6.Text);
            custom.Add("Q1", rq1.SelectedValue);
            if (rq1.SelectedValue == "Yes")
            {
                custom.Add("Q2", rq2.SelectedValue);
                custom.Add("Free1", rq3.Text);
                custom.Add("Q", ddl_lq3.SelectedValue);
                custom.Add("Q3", rq4.SelectedValue);
                custom.Add("Q4", rq5.SelectedValue);
                custom.Add("Free2", rq6.Text);
                custom.Add("Q5", rq7.SelectedValue);
                if (rq7.SelectedValue == "Yes")
                {
                    custom.Add("Q6", rq8.SelectedValue);
                    custom.Add("Q7", rq9.SelectedValue);
                    custom.Add("Q8", rq10.SelectedValue);
                    custom.Add("Q9", rq11.SelectedValue);
                    custom.Add("Q10", rq12.SelectedValue);
                    custom.Add("Q11", rq13.SelectedValue);
                    custom.Add("Q12", rq14.SelectedValue);
                    custom.Add("Q13", rq15.SelectedValue);
                    custom.Add("Q14", rq16.SelectedValue);
                    custom.Add("Q15", rq17.SelectedValue);
                    custom.Add("Q16", rq18.SelectedValue);
                    custom.Add("Q17", rq19.SelectedValue);
                    custom.Add("Q18", rq20.SelectedValue);
                    custom.Add("Q19", rq21.SelectedValue);
                    custom.Add("Q20", rq22.SelectedValue);
                    custom.Add("Q21", rq23.SelectedValue);
                    custom.Add("FreeText1", rq24.Text);
                    custom.Add("FreeText2", rq25.Text);
                    custom.Add("Q25b", rq25b.SelectedValue);
                    custom.Add("Q22", rq26.SelectedValue);

                    if (rq26.SelectedValue == "Yes")
                    {
                        custom.Add("Q23", rq27.SelectedValue);
                        custom.Add("Free5", rq28.Text);
                        custom.Add("Q24", rq29.SelectedValue);
                        custom.Add("Q25", rq30.SelectedValue);
                        custom.Add("Q26", rq31.SelectedValue);
                        custom.Add("Q27", rq32.SelectedValue);
                        custom.Add("Q28", rq33.SelectedValue);
                        custom.Add("Q29", rq34.SelectedValue);
                        custom.Add("Q30", rq35.SelectedValue);
                        custom.Add("Q31", rq36.SelectedValue);
                        custom.Add("Q32", rq37.SelectedValue);
                        custom.Add("Q33", rq38.SelectedValue);
                        custom.Add("Q34", rq39.SelectedValue);
                        custom.Add("FreeText3", rq40.Text);
                        custom.Add("FreeText4", rq41.Text);
                    }
                }
            }
            custom.Add("Website URL2", rq42.Text);

            var pdfData = FillPDF("~/PDF/Website Review PDF Example Editable.pdf", custom);
            var fileName = "Website_Review_" + _Date.ToString("MMddyy_mmHHss") + ".pdf";
            IsSuccess = UploadToMDocsWS(pdfData, fileName);
            if (IsSuccess)
            {
                wucWsReviewMessage2.AddMessageSuccess("PDF submitted correct " + fileName);
                Response.Redirect(string.Format("~/SecureMerchantManagementForms/frmCompliance.aspx?MerchantAppUID={0}", UserSessions.CurrentMerchantApp.MerchantAppUID));
            }
        }

        public bool UploadToMDocsWS(byte[] fileData, string fileName)
        {
            var IsSuccess = true;
            ZeusWeb.MDocWS.FileUpload objFU = new ZeusWeb.MDocWS.FileUpload();

            objFU.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];

            ZeusWeb.MDocWS.UploadResponse objResp = objFU.UploadFileWithSourceAndUser(
                f: fileData
                , MerchantID: Convert.ToInt32(UserSessions.CurrentMerchantApp.ID)
                , MerchantAppUID: UserSessions.CurrentMerchantApp.MerchantAppUID
                , AgentID: CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.AgentID, 0)
                , AgentUID: CommonUtility.Util.if_s(UserSessions.CurrentMerchantApp.AgentUID)
                , DocTypeID: (int)MDoc.eMDocType.CU_WebsiteReview
                , OrigName: fileName
                , Source: "Website Review"
                , ConditionID: 0
                , Description: "Website Review"
                , PrimaryKeyUID: string.Empty
                , PrimaryKeyID: 0
                , MDocSourceID: (int)MDoc.eMDocSourceID.Merchant
                , UserCreated: UserSessions.CurrentUser.UserName
                );

            if (objResp != null && objResp.DocID > 0)
            {
                DataAccess.DataDocumentsDao.UpdatePrivateMDoc(objResp.DocID, true); //DM-3671 .- line added to solve it

                var prms = new Hashtable();
                prms.Add("@DocumentID", objResp.DocID);
                prms.Add("@UserName", UserSessions.CurrentUser.UserName);

                bool IsuserNameUpdated = DataDocuments.GetInstance().DocUploadUpdateUserIDMdocument(prms);
                if (!IsuserNameUpdated)
                {
                    wucWsReviewMessage2.AddMessageError("Username of uploader not updated");
                    IsSuccess = false;
                }
            }
            else
            {
                wucWsReviewMessage2.AddMessageError("ERROR: Could not upload the file: " + fileName);
                IsSuccess = false;
            }
            return IsSuccess;
        }

        public static byte[] FillPDF(string templatePdf, IDictionary<string, string> customfields)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                string formFile = HttpContext.Current.Server.MapPath(templatePdf);
                PdfReader reader = new PdfReader(formFile);
                PdfStamper stamper = new PdfStamper(reader, ms);
                AcroFields fields = stamper.AcroFields;

                // thank you SO: http://stackoverflow.com/questions/18433532/fill-pdf-form-with-unicode-characters
                var arialFontPath = HttpContext.Current.Server.MapPath("~/PDF/ARIALUNI.TTF");
                var arialBaseFont = BaseFont.CreateFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                fields.AddSubstitutionFont(arialBaseFont);

                if (customfields != null)
                {
                    foreach (KeyValuePair<string, string> kvp in customfields)
                    {
                        fields.SetField(kvp.Key, kvp.Value);
                    }
                }

                stamper.Writer.CloseStream = false;
                stamper.FormFlattening = true;
                stamper.Close();
                return ms.ToArray();
            }
        }

        public override void FormShow(string ID)
        {
            throw new NotImplementedException();
        }

        public override void FormClear()
        {
            throw new NotImplementedException();
        }

        public override bool FormSave()
        {
            throw new NotImplementedException();
        }

        public override void FormNew()
        {
            throw new NotImplementedException();
        }

        public override bool FormDelete()
        {
            throw new NotImplementedException();
        }

        public override bool FormDataCheck()
        {
            throw new NotImplementedException();
        }

        public override void FormCancel()
        {
            throw new NotImplementedException();
        }

        public override void ToggleButtons()
        {
            throw new NotImplementedException();
        }
    }
}