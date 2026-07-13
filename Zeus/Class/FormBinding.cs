using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Collections.Generic;

using iTextSharp.text.html;
using iTextSharp.text.pdf;

using System.IO;

using Infragistics.WebUI.WebSchedule;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.EditorControls;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using System.Collections;

public class FormBinding
{
    static bool isFirstTime = true;
    /// <summary>
    /// Binds an object's properties to <see cref="Control"/>s with the same ID as the propery name. 
    /// </summary>
    /// <param name="obj">The object whose properties are being bound to forms Controls</param>
    /// <param name="container">The control in which the form Controls reside (usually a Page or ContainerControl)</param>

    public static void BindObjectToPDF(object obj, string PDF, string filename, IDictionary<string, string> customfields)
    {
        string formFile = HttpContext.Current.Server.MapPath(PDF);
        PdfReader reader = new PdfReader(formFile);
        MemoryStream ms = new MemoryStream();

        PdfStamper stamper = new PdfStamper(reader, ms);
        AcroFields fields = stamper.AcroFields;
        Type objType = obj.GetType();
        PropertyInfo[] objPropertiesArray = objType.GetProperties();

        MerchantApp app = UserSessions.CurrentMerchantApp;
        int primary_key_id = 0;
        string primary_key_uid = string.Empty;
        bool isUploadFormType = false;
        // code added for PXP-9145 and PXP-9310  by koshlendra start
        bool isMRPUploadForm = false;
        // code added for PXP-9145 and PXP-9310  by koshlendra end
        // thank you SO: http://stackoverflow.com/questions/18433532/fill-pdf-form-with-unicode-characters
        var arialFontPath = HttpContext.Current.Server.MapPath("~/PDF/ARIALUNI.TTF");
        var arialBaseFont = BaseFont.CreateFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        fields.AddSubstitutionFont(arialBaseFont);

        foreach (PropertyInfo prop in objPropertiesArray)
        {
            if (prop.GetValue(obj, null) != null)
            {
                string propertyValue = prop.GetValue(obj, null).ToString();

                fields.SetField(prop.Name, propertyValue);
            }
        }

        if (customfields != null)
        {
            foreach (KeyValuePair<string, string> kvp in customfields)
            {
                fields.SetField(kvp.Key, kvp.Value);
                if (kvp.Key.Equals("UploadFormType") && kvp.Value.Equals("CUPkg_Ops_Form"))
                    isUploadFormType = true;
                if (kvp.Key.Equals("UploadFormType") && kvp.Value.Equals("Send_MRP_Req_Form"))
                    isMRPUploadForm = true;


            }
        }

        stamper.Writer.CloseStream = false;
        stamper.FormFlattening = true;
        stamper.Close();

        ms.WriteTo(HttpContext.Current.Response.OutputStream);
        if (isMRPUploadForm)
            UploadFile(filename, ms, primary_key_id, primary_key_uid, Convert.ToInt32(app.ID), app.MerchantAppUID, app.UserUpdated, isFirstTime, isMRPUploadForm);
        else
        {
            if (isUploadFormType)
            {
                UploadFile(filename, ms, primary_key_id, primary_key_uid, Convert.ToInt32(app.ID), app.MerchantAppUID, app.UserUpdated, isFirstTime, isMRPUploadForm);

                if (isFirstTime)
                {
                    isFirstTime = false;

                    //Bug Fix - 6132 (‘Card Present (Swiped)%, Card Present (Keyed)% and Card Not Present (Keyed)% should be populated from Fees page on Ops Form )
                    if ((app.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || app.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                        || app.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS)
                        && app.Office == CommonUtility.Util.Offices.Irvine)
                    {
                        FormBinding.BindObjectToPDF(obj, "~/PDF/Ops_Form_woodforest.pdf", "Ops_Form_" + app.ID.ToString() + ".pdf", customfields);
                    }
                    else
                    {
                        FormBinding.BindObjectToPDF(obj, "~/PDF/Ops_Form.pdf", "Ops_Form_" + app.ID.ToString() + ".pdf", customfields);
                    }
                }
            }
            else
            {
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                HttpContext.Current.Response.End();
            }
        }
        isFirstTime = true;
    }


    /// Code added for PXP-9310[Generate and save High Risk Merchant registration request in csv format] by koshlendra start

    public static void BindObjectToCSV(string csv, string filename, string data)
    {
        try
        {



            FileStream fs = new FileStream(csv, FileMode.Open, FileAccess.Read);

            // Create a byte array of file stream length
            byte[] filebytes = new byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(filebytes, 0, System.Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();
            MerchantApp app = UserSessions.CurrentMerchantApp;
            int primary_key_id = 0;
            string primary_key_uid = string.Empty;

            ZeusWeb.MDocWS.FileUpload fu = new ZeusWeb.MDocWS.FileUpload();

            fu.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];

            string orig_filename = filename;
            int merchantapp_id = CommonUtility.Util.if_i(app.ID, 0);
            string merchantapp_uid = CommonUtility.Util.if_s(app.MerchantAppUID, "");
            int mdoc_sourceid = 5;
            int doctypeid = 114;
            string descr = "MRP Request Form";
            string username = CommonUtility.Util.if_s(app.UserUpdated, "");

            if (filebytes != null)
            {
                // TODO: change merchantid and merchantuid.
                ZeusWeb.MDocWS.UploadResponse resp = fu.UploadFileWithSourceAndUser(filebytes, merchantapp_id, merchantapp_uid, 0, null, doctypeid, orig_filename, "Merchant", doctypeid, descr, primary_key_uid, primary_key_id, mdoc_sourceid, username);
                if ((resp != null) && (resp.StatusMessage.Equals("Success")))
                {
                    //isPrivate variable is always true as we need the form should be private when in the Documents section.
                    if (resp.DocID > 0)
                        DataAccess.DataDocumentsDao.UpdatePrivateMDoc(resp.DocID, true);
                }

                UpdateUsername(username, resp);

            }
            File.Delete(csv);
        }
        catch (Exception)
        {

        }



    }
    public static void BindUploadBytesToCSV(byte[] excelBytes, string filename, int merchantID, string merchantUID , string userUpdated )
    {
        try
        {
            ZeusWeb.MDocWS.FileUpload fu = new ZeusWeb.MDocWS.FileUpload();
            fu.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];

            if (excelBytes != null)
            {
                // TODO: change merchantid and merchantuid.
                ZeusWeb.MDocWS.UploadResponse resp = fu.UploadFileWithSourceAndUser(excelBytes, merchantID, merchantUID, 0, null, 115, filename, "Merchant", 115, "VIRP Request Form", "", 0, 5, userUpdated);
                if ((resp != null) && (resp.StatusMessage.Equals("Success")))
                {
                    if (resp.DocID > 0)
                        DataAccess.DataDocumentsDao.UpdatePrivateMDoc(resp.DocID, true);
                }

                UpdateUsername(userUpdated, resp);

            }
        }
        catch (Exception ex)
        {

        }
    }


    /// Code added for PXP-9310[Generate and save High Risk Merchant registration request in csv format] by koshlendra  end

    /// <summary>
    /// code added for PXP-9310 for new parameter isMRPForm
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="ms"></param>
    /// <param name="primary_key_id"></param>
    /// <param name="primary_key_uid"></param>
    /// <param name="merchantapp_id"></param>
    /// <param name="merchantapp_uid"></param>
    /// <param name="username"></param>    
    private static void UploadFile(string filename, MemoryStream ms, int primary_key_id, string primary_key_uid, int merchantapp_id, string merchantapp_uid, string username, bool isCUForm,bool isMRPForm)
    {
        int doctypeid = 0;
        string descr = string.Empty;

        ZeusWeb.MDocWS.FileUpload fu = new ZeusWeb.MDocWS.FileUpload();
        fu.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];
        byte[] b = null;
        using (var memoryStream = new MemoryStream())
        {
            b = ms.ToArray();
        }

        int mdoc_sourceid = 5;
        //code updated for PXP-9310[Generate and save High Risk Merchant registration request in csv format] start
        if(isMRPForm)
        {
            doctypeid = 114;
            descr = "MRP Request Form";
            ZeusWeb.MDocWS.UploadResponse mrpResp = fu.UploadFileWithSourceAndUser(b, merchantapp_id, merchantapp_uid, 0, null, doctypeid, filename, "Merchant", doctypeid, descr, primary_key_uid, primary_key_id, mdoc_sourceid, username);
            if ((mrpResp != null) && (mrpResp.StatusMessage.Equals("Success")))
            {
                //isPrivate variable is always true as we need the form should be private when in the Documents section.
                if (mrpResp.DocID > 0)
                    DataAccess.DataDocumentsDao.UpdatePrivateMDoc(mrpResp.DocID, true);
            }

            UpdateUsername(username, mrpResp);
        }
        else
        {
            if (isCUForm)
            {
                doctypeid = 50;
                descr = "Credit Underwriting Form";
                ZeusWeb.MDocWS.UploadResponse cuResp = fu.UploadFileWithSourceAndUser(b, merchantapp_id, merchantapp_uid, 0, null, doctypeid, filename, "Merchant", doctypeid, descr, primary_key_uid, primary_key_id, mdoc_sourceid, username);
                if ((cuResp != null) && (cuResp.StatusMessage.Equals("Success")))
                {
                    //isPrivate variable is always true as we need the form should be private when in the Documents section.
                    if (cuResp.DocID > 0)
                        DataAccess.DataDocumentsDao.UpdatePrivateMDoc(cuResp.DocID, true);
                }

                UpdateUsername("3DE", cuResp);
            }
            else
            {
                doctypeid = 107;
                descr = "Operations Form";
                ZeusWeb.MDocWS.UploadResponse opsResp = fu.UploadFileWithSourceAndUser(b, merchantapp_id, merchantapp_uid, 0, null, doctypeid, filename, "Merchant", doctypeid, descr, primary_key_uid, primary_key_id, mdoc_sourceid, username);
                UpdateUsername(username, opsResp);
            }
        }
        //code updated for PXP-9310[Generate and save High Risk Merchant registration request in csv format] end

    }

    private static void UpdateUsername(string username, ZeusWeb.MDocWS.UploadResponse resp)
    {
        try
        {
            if (resp != null && resp.DocID > 0)
            {
                Hashtable prms = new Hashtable();

                prms.Add("@DocumentID", resp.DocID);
                prms.Add("@UserName", username);

                DataDocuments.GetInstance().DocUploadUpdateUserIDMdocument(prms);
            }
        }
        catch
        {
            //suppress any exceptions
        }
    }

    public static Control FindFormControl(Control Ctrls, string ControlName)
    {
        foreach (Control Ctrl in Ctrls.Controls)
        {
            if (Ctrl.ID == ControlName)
                return Ctrl;

            if (Ctrl.HasControls())
            {
                Control Ctrl2 = FindFormControl(Ctrl, ControlName);
                if (Ctrl2 != null)
                    return Ctrl2;
            }
        }
        return null;
    }

    public static void BindObjectToControls(object obj, Control container)
    {
        if (obj == null) return;

        // Get the properties of the business object

        Type objType = obj.GetType();
        PropertyInfo[] objPropertiesArray = objType.GetProperties();

        foreach (PropertyInfo prop in objPropertiesArray)
        {
            Control control = FormHandler.FindFormControl(container, prop.Name);

            if (prop.Name.Equals("OfficeAccess"))
            {

                string test = "111";
            }

            // handle ListControls (DropDownList, CheckBoxList, RadioButtonList)

            if (control != null)
            {
                if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;
                    listControl.SelectedIndex = -1;

                    string propertyValue = prop.GetValue(obj, null).ToString();

                    if (prop.GetValue(obj, null).GetType().IsEnum)
                    {
                        propertyValue = Convert.ToInt32(prop.GetValue(obj, null)).ToString();
                    }
                    else
                    {
                        propertyValue = prop.GetValue(obj, null).ToString();
                    }

                    ListItem listItem = listControl.Items.FindByValue(propertyValue);
                    if (listItem != null) listItem.Selected = true;
                }
                else if (control is TextBox)
                {
                    TextBox txt = (TextBox)control;
                    string str = (prop.GetValue(obj, null) == null) ? string.Empty : prop.GetValue(obj, null).ToString();
                    txt.Text = str;

                    if (prop.PropertyType.Name == "Decimal" && prop.GetValue(obj, null) != null)
                    {
                        if (prop.Name.Contains("DiscountQual"))
                        {
                            string pricingTypeControlName = prop.Name.Replace("DiscountQual", "PricingTypeID");
                            Control pricingControl = FormHandler.FindFormControl(container, pricingTypeControlName);

                            if (pricingControl is DropDownList)
                            {
                                DropDownList ddl = (DropDownList)pricingControl;
                                if (ddl.SelectedValue == "7") // Flat Rate
                                {
                                    txt.Text = Convert.ToDecimal(prop.GetValue(obj, null)).ToString("0.0000");
                                }
                                else
                                {
                                    txt.Text = Convert.ToDecimal(prop.GetValue(obj, null)).ToString("#,##0.000");
                                }
                            }
                            else
                            {
                                txt.Text = Convert.ToDecimal(prop.GetValue(obj, null)).ToString("#,##0.000");
                            }
                        }
                        else
                        {
                            txt.Text = Convert.ToDecimal(prop.GetValue(obj, null)).ToString("#,##0.000");
                        }
                    }
                }
                else if (control is Label)
                {
                    Label txt = (Label)control;
                    string str = (prop.GetValue(obj, null) == null) ? string.Empty : prop.GetValue(obj, null).ToString();
                    txt.Text = str;
                    if (prop.PropertyType.Name == "Decimal" && prop.GetValue(obj, null) != null)
                        txt.Text = Convert.ToDecimal(prop.GetValue(obj, null)).ToString("#,##0.00");
                }
                else if (control is HiddenField)
                {
                    HiddenField txt = (HiddenField)control;
                    txt.Value = prop.GetValue(obj, null).ToString();
                }
                else if (control is CheckBox)
                {
                    CheckBox chk = (CheckBox)control;
                    chk.Checked = Convert.ToBoolean(prop.GetValue(obj, null));
                }
                else if (control is WebDateTimeEditor)
                {
                    WebDateTimeEditor txt = (WebDateTimeEditor)control;
                    if (prop.GetValue(obj, null) == null || prop.GetValue(obj, null).ToString() == string.Empty)
                        txt.Value = null;
                    else
                        if (Convert.ToDateTime(prop.GetValue(obj, null)) == DateTime.MinValue)
                        txt.Value = null;
                    else
                        txt.Value = Convert.ToDateTime(prop.GetValue(obj, null));

                    //WebDateTimeEditor txt = (WebDateTimeEditor)control;
                    //if (prop.GetValue(obj, null).ToString() == string.Empty)
                    //    txt.Text = "";
                    //else
                    //    if (Convert.ToDateTime(prop.GetValue(obj, null)) == DateTime.MinValue)
                    //        txt.Text = "";
                    //    else
                    //        txt.Value = Convert.ToDateTime(prop.GetValue(obj, null));


                    //WebDateTimeEditor txt = (WebDateTimeEditor)control;
                    //if (prop.GetValue(obj, null) == null)
                    //    txt.Value = null;
                    //else
                    //    if (Convert.ToDateTime(prop.GetValue(obj, null)) == DateTime.MinValue)
                    //        txt.Value = null;
                    //    else
                    //        txt.Value = Convert.ToDateTime(prop.GetValue(obj, null));
                }

                else if (control is WebNumericEditor)
                {
                    WebNumericEditor txt = (WebNumericEditor)control;

                    switch (txt.DataMode)
                    {
                        case NumericDataMode.Int:
                            txt.Value = Convert.ToInt32(prop.GetValue(obj, null));
                            break;
                        default:
                            txt.Value = Convert.ToDecimal(prop.GetValue(obj, null));
                            break;
                    }
                }
                else if (control is WebCurrencyEditor)
                {
                    WebCurrencyEditor txt = (WebCurrencyEditor)control;
                    txt.Value = Convert.ToDecimal(prop.GetValue(obj, null));
                }
                else if (control is WebPercentEditor)
                {
                    WebPercentEditor txt = (WebPercentEditor)control;
                    txt.Value = Convert.ToDecimal(prop.GetValue(obj, null));
                }
                else
                {
                    // get the properties of the control
                    //
                    Type controlType = control.GetType();
                    PropertyInfo[] controlPropertiesArray = controlType.GetProperties();

                    // test for common properties
                    //
                    bool success = false;
                    success = FindAndSetControlProperty(obj, prop, control, controlPropertiesArray, "Checked", typeof(bool));

                    if (!success)
                        success = FindAndSetControlProperty(obj, prop, control, controlPropertiesArray, "SelectedDate", typeof(DateTime));

                    if (!success)
                        success = FindAndSetControlProperty(obj, prop, control, controlPropertiesArray, "Value", typeof(String));

                    if (!success)
                        success = FindAndSetControlProperty(obj, prop, control, controlPropertiesArray, "Text", typeof(String));

                    if (!success)
                        success = FindAndSetControlProperty(obj, prop, control, controlPropertiesArray, "Value", typeof(object));
                }
            }
        }
    }

    private static bool FindAndSetControlProperty(object obj, PropertyInfo objProperty, Control control, PropertyInfo[] controlPropertiesArray, string propertyName, Type type)
    {
        // iterate through control properties
        //
        foreach (PropertyInfo controlProperty in controlPropertiesArray)
        {
            // check for matching name and type
            //
            if (controlProperty.Name == propertyName && controlProperty.PropertyType == type && controlProperty.CanWrite)
            {
                // set the control's property to the business object property value
                //
                controlProperty.SetValue(control, Convert.ChangeType(objProperty.GetValue(obj, null), type), null);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Binds your the values in <see cref="Control"/>s to a business object.
    /// </summary>
    /// <param name="obj">The object whose properties are being bound to Control values</param>
    /// <param name="container">The control in which the form Controls reside (usually a Page or ContainerControl)</param>
    public static void BindControlsToObject(object obj, Control container)
    {
        if (obj == null) return;

        // Get the properties of the business object
        //			
        Type objType = obj.GetType();
        PropertyInfo[] objPropertiesArray = objType.GetProperties();

        foreach (PropertyInfo prop in objPropertiesArray)
        {


             if (prop.Name.Equals("listOfficeAccess"))
            {

                string test = "111";
            }



            Control control = FormHandler.FindFormControl(container, prop.Name);
            //Control control = container.FindControl(prop.Name);
            if (control != null)
            {
                if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;

                    if (listControl.SelectedItem != null)
                    {
                        // major change, if you're trying to bind a list (ie, dropdownlist) and the target object is an enum, then it will
                        // normally throw an exception. so first, we check to see if its an enum, and if it is, then we cast it into an
                        // integer first. NOTE: we assume its an integer, otherwise it will throw an exception
                        if (prop.PropertyType.IsEnum)
                        {
                            prop.SetValue(obj, Convert.ToInt32(listControl.SelectedItem.Value), null);
                        }
                        // Adding the condtion to mange the merchantOfficeAccess
                        else if (prop.Name.Equals("listOfficeAccess"))
                        {
                            // List<int> officeAccess = new List<int>();
                            //foreach (ListItem item in listControl.Items)
                            //  {
                            //  if (item.Selected)
                            //      {

                            //  }
                        }
                        else
                        {
                            prop.SetValue(obj, Convert.ChangeType(listControl.SelectedItem.Value, prop.PropertyType), null);
                        }
                    }

                }
                else if (control is HiddenField)
                {
                    HiddenField txt = (HiddenField)control;
                    prop.SetValue(obj, txt.Value, null);
                }
                else if (control is CheckBox)
                {
                    CheckBox chk = (CheckBox)control;
                    prop.SetValue(obj, chk.Checked , null);
                }
                else if (control is WebDateTimeEditor)
                {
                    WebDateTimeEditor txt = (WebDateTimeEditor)control;

                    if (string.IsNullOrEmpty(txt.Text))
                        prop.SetValue(obj, null, null);
                    else
                        if (prop.PropertyType.Equals(typeof(DateTime)))
                        prop.SetValue(obj, Convert.ToDateTime(txt.Value), null);
                    else
                        prop.SetValue(obj, Convert.ToDateTime(txt.Value).ToString(), null);
                }
                else if (control is WebNumericEditor)
                {
                    WebNumericEditor txt = (WebNumericEditor)control;
                    switch (txt.DataMode)
                    {
                        case NumericDataMode.Int:
                            prop.SetValue(obj, txt.Value == null ? Convert.ToInt32(0) : Convert.ToInt32(txt.Value), null);
                            break;
                        case NumericDataMode.Long: // Code added by amit for PXP-6006
                            prop.SetValue(obj, txt.Value == null ? Convert.ToInt64(0) : Convert.ToInt64(txt.Value), null);
                            break;
                        default:
                            prop.SetValue(obj, txt.Value == null ? Convert.ToDecimal(0) : Convert.ToDecimal(txt.Value), null);
                            break;
                    }
                }
                else if (control is WebCurrencyEditor)
                {
                    WebCurrencyEditor txt = (WebCurrencyEditor)control;
                    prop.SetValue(obj, txt.Value == null ? Convert.ToDecimal(0) : Convert.ToDecimal(txt.Value), null);
                }
                else if (control is WebPercentEditor)
                {
                    WebPercentEditor txt = (WebPercentEditor)control;
                    prop.SetValue(obj, txt.Value == null ? Convert.ToDecimal(0) : Convert.ToDecimal(txt.Value), null);
                }
                else if (control is WebMaskEditor)
                {
                    WebMaskEditor txt = (WebMaskEditor)control;
                    prop.SetValue(obj, txt.Value.ToString().Trim() == string.Empty ? string.Empty : txt.Text, null);
                }

                else
                {
                    // get the properties of the control
                    //
                    Type controlType = control.GetType();
                    PropertyInfo[] controlPropertiesArray = controlType.GetProperties();

                    // test for common properties
                    //
                    bool success = FindAndGetControlProperty(obj, prop, control, controlPropertiesArray, "Checked", typeof(bool));

                    if (!success)
                        success = FindAndGetControlProperty(obj, prop, control, controlPropertiesArray, "SelectedDate", typeof(DateTime));

                    if (!success)
                        success = FindAndGetControlProperty(obj, prop, control, controlPropertiesArray, "Value", typeof(String));

                    if (!success)
                        success = FindAndGetControlProperty(obj, prop, control, controlPropertiesArray, "Text", typeof(String));
                }
            }
        }
    }

    /// <summary>
    /// Looks for a property name and type on a control and attempts to set it to the value in an object's property 
    /// of the same name.
    /// </summary>
    /// <param name="obj">The object whose properties are being set</param>
    /// <param name="objProperty">The property of the object being set</param>
    /// <param name="control">The control whose ID matches the object's property name.</param>
    /// <param name="controlPropertiesArray">An array of the control's properties</param>
    /// <param name="propertyName">The name of the Control property being retrieved</param>
    /// <param name="type">The correct type for the Control property</param>
    /// <returns>Boolean for whether the property was found and retrieved</returns>
    private static bool FindAndGetControlProperty(object obj, PropertyInfo objProperty, Control control, PropertyInfo[] controlPropertiesArray, string propertyName, Type type)
    {
        // iterate through control properties
        //
        foreach (PropertyInfo controlProperty in controlPropertiesArray)
        {
            // check for matching name and type
            //
            if (controlProperty.Name == "Text" && controlProperty.PropertyType == typeof(String))
            {
                // set the control's property to the business object property value
                //
                try
                {
                    objProperty.SetValue(obj,
                                  Convert.ChangeType(
                                  controlProperty.GetValue(control, null),
                                  objProperty.PropertyType), null);
                    //objProperty.SetValue(control, Convert.ChangeType(controlProperty.GetValue(obj, null), objProperty.PropertyType), null);
                    return true;
                }
                catch
                {
                    // the data from the form control could not be converted to objProperty.PropertyType
                    //
                    return false;
                }
            }
            else if (controlProperty.Name == "Checked" && controlProperty.PropertyType == typeof(bool))
            {
                try
                {
                    objProperty.SetValue(obj,
                                  Convert.ChangeType(
                                  controlProperty.GetValue(control, null),
                                  objProperty.PropertyType), null);
                    //objProperty.SetValue(control, Convert.ChangeType(controlProperty.GetValue(obj, null), objProperty.PropertyType), null);
                    return true;
                }
                catch
                {
                    // the data from the form control could not be converted to objProperty.PropertyType
                    //
                    return false;
                }

            }
        }
        return false;
    }


}
