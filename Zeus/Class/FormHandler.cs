
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics; //PXP-2955
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CommonUtility;
using HtmlAgilityPack;
using Infragistics.Web.UI.EditorControls;
using Infragistics.Web.UI.NavigationControls;
using Infragistics.WebUI.WebDataInput;
using Infragistics.WebUI.WebHtmlEditor;
using Infragistics.WebUI.WebSchedule;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using PaymentXP.BusinessObjects;
using PaymentXP.BusinessObjects.Notify; //PXP-4735
using PaymentXP.BusinessObjects.Tickets;
using PaymentXP.BusinessObjects.Zeus;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using Paysafe.Zeus3DE.Model;
using Tsys.TsysUtility;  //PXP-11670 & PXP-1167
using ZeusWeb;
using ZeusWeb.Class;

/// <summary>
/// Summary description for FormHandler
/// </summary>
public static class FormHandler
{
    #region Members
    public enum eEmailPasswordType : int
    {
        Password = 1,
        MerchantKey = 2
    }

    public enum eEmailUserNameOrPasswordType : int
    {
        UserName = 0,
        Password = 1
    }

    public enum eEmailUserNameOrMerchantKeyType : int
    {
        UserName = 0,
        MerchantKey = 1
    }

    private static IDictionary<string, string> custom = new Dictionary<string, string>();
    #endregion

    public static void SetSecurity(MasterPage page)
    {
        string strPage = "MASTER";//page.ToString();

        //while (strPage.IndexOf("_") > 0)
        //    strPage = strPage.Substring(strPage.IndexOf("_") + 1).Replace("_aspx", "").ToUpper();

        User user = UserSessions.CurrentUser;

        if (user == null)
            return;


        UserForm frm = null;

        if (user.UserForms.TryGetValue(strPage, out frm) && frm.HasAccess)
        {
            if (frm.ControlObjects == null)
                DataAccess.DataUserDao.GetUserObjectPermissions(frm, user, UserSessions.PortalUID);

            foreach (ControlObject obj in frm.ControlObjects)
            {
                LoopingControls(page, obj);
            }
        }
        else
        {
            HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
        }

    }

    public static void SetSecurity(Page page)
    {
        //string strPage = page.ToString();

        //while (strPage.IndexOf("_") > 0)
        //    strPage = strPage.Substring(strPage.IndexOf("_") + 1).Replace("_aspx", "").ToUpper();

        User user = UserSessions.CurrentUser;

        if (user == null)
            return;

        string strPage = Path.GetFileName(HttpContext.Current.Request.Path).Replace(".aspx", "").ToUpper();


        UserForm frm = null;

        if (user.UserForms.TryGetValue(strPage, out frm) && frm.HasAccess)
        {

            if (frm.ControlObjects == null)
                DataAccess.DataUserDao.GetUserObjectPermissions(frm, user, UserSessions.PortalUID);

            foreach (ControlObject obj in frm.ControlObjects)
            {
                LoopingControls(page, obj);
            }
            //PXP-7674 abarua
            if (frm.Description == "Compliance Tab" && user.DefaultRoleUID != Constants.ROLE_COMPLIANCE.ToLower())
            {
                HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
                return;
            }

            //Lock down internal accounts
            if (frm.Description == "MerchantManagement Tab" && UserSessions.CurrentMerchantApp != null && CommonUtility.Util.IsValidInt32(UserSessions.CurrentMerchantApp.ID))
            {
                bool hasaccess = UserSessions.CurrentMerchantApp.HasViewAccess(LookupTableHandler.GetMerchantClass(), UserSessions.CurrentUser);
                if (!hasaccess)
                {
                    HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
                    return;
                }
            }

            //Ani: PXP-17275 - Zeus - Add Tab & Page in Zeus for Quality-- AgentAllocation
            // User will only have access for Role = Quality and  Main Department  = Quality
            if (frm.Description == "Quality Tab" && (user.DefaultRoleUID != Constants.ROLE_QUALITY.ToLower() && !user.UserRoles.Keys.Contains(Constants.ROLE_QUALITY.ToUpper())))
            {
                HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
                return;
            }

            if (frm.Description == "Allocations Tab" && (user.DefaultRoleUID != Constants.ROLE_SALES_SUPPORT.ToLower() && !user.UserRoles.Keys.Contains(Constants.ROLE_SALES_SUPPORT.ToUpper())))
            {
                HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
                return;
            }

        }
        else
        {
            HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
        }
    }

    #region ASPX Methods

    public static void ShowClosureCodes(UserControl ctrl, string StatusUID)
    {

        DropDownList cbo = (DropDownList)ctrl.FindControl("MerchantClosureCodeUID");
        Label lbl = (Label)ctrl.FindControl("lblMerchantClosureCodeUID");

        DropDownList cboRiskStatus = (DropDownList)ctrl.FindControl("RiskStatus");
        Label lblRiskStatus = (Label)ctrl.FindControl("lblRiskStatus");

        if (cbo != null)
        {
            // if (StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_CANCELLATION || StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_PENDING_CANCELLATION)
            if (StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_CANCELLATION) // ***PXP 1261 : remove closur code & ETF validation MS_RETENTION-PENDING-CANCELLATION status
            {
                cbo.Style["display"] = "block";
                lbl.Style["display"] = "block";

                cboRiskStatus.Style["display"] = "block";
                lblRiskStatus.Style["display"] = "block";
            }
            else
            {
                cbo.Style["display"] = "none";
                lbl.Style["display"] = "none";

                cboRiskStatus.Style["display"] = "none";
                lblRiskStatus.Style["display"] = "none";
            }
        }
    }

    public static void DisplayMessage(ClientScriptManager client, string message)
    {
        string script;
        script = "<script type='text/javascript'>";
        script += "alert('" + message + "');";
        script += "</script>";

        client.RegisterStartupScript(client.GetType(), "DisplayMessage", script);
    }

    #region frmBaseDataEntry Page
    public static void LogUserAccess()
    {
        string strPage = Path.GetFileName(HttpContext.Current.Request.Path).Replace(".aspx", "").ToUpper();

        UserForm frm = null;

        if (UserSessions.CurrentUser != null
            && UserSessions.CurrentUser.UserForms != null
            && UserSessions.CurrentUser.UserForms.TryGetValue(strPage, out frm)
            && frm.HasAccess)
        {

            //log user access
            switch (frm.Description)
            {
                case "MerchantManagement Tab":
                    if (UserSessions.CurrentMerchantApp != null)
                        DataAccess.DataUserDao.InsertChangeLog(UserSessions.CurrentMerchantApp.BusinessDBAName
                                , UserSessions.CurrentUser.UserName
                                , UserSessions.CurrentMerchantApp.MerchantAppUID
                                , UserSessions.CurrentMerchantApp.ID, strPage, "User Access", Constants.PORTAL_ZEUS);
                    break;
                case "Lead Tab":
                    if (UserSessions.CurrentLead != null)
                        DataAccess.DataUserDao.InsertChangeLog(UserSessions.CurrentLead.DBAName
                                , UserSessions.CurrentUser.UserName
                                , UserSessions.CurrentLead.LeadUID
                                , UserSessions.CurrentLead.LeadID, strPage, "User Access", Constants.PORTAL_ZEUS);
                    break;
                case "AgentManagement Tab":
                    if (UserSessions.CurrentLoggedInAgent != null)
                        DataAccess.DataUserDao.InsertChangeLog(UserSessions.CurrentLoggedInAgent.AgentDBA
                                , UserSessions.CurrentUser.UserName
                                , UserSessions.CurrentLoggedInAgent.AgentUID
                                , UserSessions.CurrentLoggedInAgent.AgentID, strPage, "User Access", Constants.PORTAL_ZEUS);
                    break;
            }
        }
    }
    #endregion

    #region CommunicationsDetail Page
    public static string LogFormChanged(object obj, Panel pnl)
    {
        string change = string.Empty;

        if (obj == null)
            return string.Empty;

        Type type = obj.GetType();
        PropertyInfo[] props = type.GetProperties();
        foreach (PropertyInfo prop in props)
        {
            Control control = FormHandler.FindFormControl(pnl, prop.Name);
            if (control != null)
            {
                if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;
                    string propValue = prop.GetValue(obj, null).ToString();
                    string listValue = string.Empty;
                    if (listControl.SelectedIndex != -1)
                        listValue = listControl.SelectedItem.Value.ToString();

                    if (propValue != listValue && listValue != "-1")
                        change += prop.Name + ", ";
                }
                else if (control is WebHtmlEditor)
                {
                    WebHtmlEditor txt = (WebHtmlEditor)control;
                    if (txt.Text != prop.GetValue(obj, null).ToString())
                        change += prop.Name + ", ";
                }
                else if (control is TextBox)
                {
                    TextBox txt = (TextBox)control;
                    if (txt.Text != prop.GetValue(obj, null).ToString())
                        change += prop.Name + ", ";
                }
                else if (control is CheckBox)
                {
                    CheckBox chk = (CheckBox)control;
                    if (chk.Checked != (bool)prop.GetValue(obj, null))
                        change += prop.Name + ", ";
                }
                else if (control is WebDateTimeEditor)
                {
                    WebDateTimeEditor txt = (WebDateTimeEditor)control;
                    if ((DateTime)txt.Value != (DateTime)prop.GetValue(obj, null))
                        change += prop.Name + ", ";
                }
                else
                {
                    Type controlType = control.GetType();
                    PropertyInfo[] controlPropertiesArray = controlType.GetProperties();


                    foreach (PropertyInfo controlProperty in controlPropertiesArray)
                    {
                        if (controlProperty.Name == "Text" && controlProperty.PropertyType == typeof(String))
                        {
                            string propValue = prop.GetValue(obj, null).ToString();
                            if (propValue != Convert.ChangeType(controlProperty.GetValue(control, null), typeof(string)).ToString())
                                change += prop.Name + ", ";
                        }
                        else if (controlProperty.Name == "Checked" && controlProperty.PropertyType == typeof(bool))
                        {
                            bool propValue = (bool)prop.GetValue(obj, null);
                            if (propValue != (bool)Convert.ChangeType(controlProperty.GetValue(control, null), prop.PropertyType))
                                change += prop.Name + ", ";
                        }
                    }

                }
            }

        }

        if (!string.IsNullOrWhiteSpace(change))
        {
            string str = change.Substring(0, change.Length - 2);
            if (!str.Contains("TimeSent"))
                change = "Please save or cancel changes. The following fields have changed: " + str + ".";
            else
                change = string.Empty;
        }
        return change;

    }
    #endregion

    #region PROFILE Page
    // This function based on GMA Requirement.
    public static void SetGMADisable(Control container, bool EditMode)
    {
        if (container != null)
        {
            WebImageButton btnBtn = (WebImageButton)container;
            btnBtn.Enabled = EditMode;
        }
    }
    #endregion

    #region UNDERWRITING Page

    public static bool IsConditionExist(string MerchantAppUID)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", MerchantAppUID);
        return DataConditions.GetUWConditionsList(prms).Count > 0 ? true : false;
    }
    public static IList<UWConditions> GetConditionList(string MerchantAppUID)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", MerchantAppUID);
        return DataConditions.GetUWConditionsList(prms);
    }

    public static string getAgentLevel(string aLevel)
    {
        if (!string.IsNullOrWhiteSpace(aLevel) && aLevel != "-1")
        {
            if (LookupTableHandler.m_AgentLevels == null)
                LookupTableHandler.GetAgentLevels();

            GenericListItem item = LookupTableHandler.m_AgentLevels.Where(c => c.ItemValue == aLevel).First();

            if (item != null)
            {
                return item.ItemValue;
            }
        }

        return string.Empty;
    }

    public static string getPciLevel(int PCILevel)
    {
        switch (PCILevel)
        {
            case 0:
                return "Waived";
            case -1:
                return "NULL";
            default:
                return PCILevel.ToString();
        }
    }

    public static string getReleaseType(string releaseUID)
    {
        switch (releaseUID.ToLower())
        {
            case Constants.RELEASE_METHOD_12_MONTH_ROLLING:
                return "12-Month Rolling";
            case Constants.RELEASE_METHOD_SET_SCHEDULE:
                return "Set Schedule";
            case Constants.RELEASE_METHOD_6_MONTH_ROLLING:
                return "6-Month Rolling";
            case Constants.RELEASE_METHOD_PERIODIC_REVIEW:
                return "Periodic Review";
            case Constants.RELEASE_METHOD_OTHER:
                return "Other";
            case Constants.RELEASE_METHOD_9_MONTH_ROLLING:
                return "9-Month Rolling";
            case Constants.RELEASE_METHOD_3_MONTH_ROLLING:
                return "3-Month Rolling";
            default:
                return string.Empty;
        }
    }

    //Check if an MCC is a Restricted MCC
    public static bool IsMCCRestricted(string MCCCode)
    {
        bool isRestricted = false;

        if (!string.IsNullOrEmpty(MCCCode))
        {
            MerchantFacade facade = new MerchantFacade();
            Hashtable prms = new Hashtable();

            if (!string.IsNullOrWhiteSpace(MCCCode))
                prms.Add("@Name", MCCCode);

            DataSet ds = facade.GetSicCodes(prms);
            if (ds.Tables[0].Rows.Count > 0)
            {
                isRestricted = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsRestrictedIndustry"]);
            }
        }
        return isRestricted;
    }

    public static void ExportToPDF(GridView gvReport, bool LandScape, string ReportName, int OnlyReadFirstXColumns)
    {
        int noOfColumns = 0;
        int noOfRows = 0;

        DataTable tbl = null;

        if (gvReport.AutoGenerateColumns)
        {
            tbl = gvReport.DataSource as DataTable; // Gets the DataSource of the GridView Control.
            noOfColumns = (OnlyReadFirstXColumns == -1) ? tbl.Columns.Count : OnlyReadFirstXColumns;
            noOfRows = tbl.Rows.Count;
        }
        else
        {
            noOfColumns = (OnlyReadFirstXColumns == -1) ? gvReport.Columns.Count : OnlyReadFirstXColumns;
            noOfRows = gvReport.Rows.Count;
        }

        float HeaderTextSize = 8;
        float ReportNameSize = 10;
        float ReportTextSize = 8;
        float ApplicationNameSize = 7;

        // Creates a PDF document
        Document document = null;
        if (LandScape == true)
        {
            // Sets the document to A4 size and rotates it so that the orientation of the page is Landscape.
            document = new Document(PageSize.A4.Rotate(), 0, 0, 15, 5);
        }
        else
        {
            document = new Document(PageSize.A4, 0, 0, 15, 5);
        }

        // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
        iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

        // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
        mainTable.HeaderRows = 4;

        // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
        iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(2);

        // Creates a phrase to hold the application name at the left hand side of the header.
        Phrase phApplicationName = new Phrase(ReportName, FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

        // Creates a PdfPCell which accepts a phrase as a parameter.
        PdfPCell clApplicationName = new PdfPCell(phApplicationName);
        // Sets the border of the cell to zero.
        clApplicationName.Border = PdfPCell.NO_BORDER;
        // Sets the Horizontal Alignment of the PdfPCell to left.
        clApplicationName.HorizontalAlignment = Element.ALIGN_LEFT;

        // Creates a phrase to show the current date at the right hand side of the header.
        Phrase phDate = new Phrase(DateTime.Now.Date.ToString("MM/dd/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

        // Creates a PdfPCell which accepts the date phrase as a parameter.
        PdfPCell clDate = new PdfPCell(phDate);
        // Sets the Horizontal Alignment of the PdfPCell to right.
        clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
        // Sets the border of the cell to zero.
        clDate.Border = PdfPCell.NO_BORDER;

        // Adds the cell which holds the application name to the headerTable.
        headerTable.AddCell(clApplicationName);
        // Adds the cell which holds the date to the headerTable.
        headerTable.AddCell(clDate);
        // Sets the border of the headerTable to zero.
        headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

        // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
        PdfPCell cellHeader = new PdfPCell(headerTable);
        cellHeader.Border = PdfPCell.NO_BORDER;
        // Sets the column span of the header cell to noOfColumns.
        cellHeader.Colspan = noOfColumns;
        // Adds the above header cell to the table.
        mainTable.AddCell(cellHeader);

        // Creates a phrase which holds the file name.
        Phrase phHeader = new Phrase(ReportName, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
        PdfPCell clHeader = new PdfPCell(phHeader);
        clHeader.Colspan = noOfColumns;
        clHeader.Border = PdfPCell.NO_BORDER;
        clHeader.HorizontalAlignment = Element.ALIGN_CENTER;
        mainTable.AddCell(clHeader);

        // Creates a phrase for a new line.
        Phrase phSpace = new Phrase("\n");
        PdfPCell clSpace = new PdfPCell(phSpace);
        clSpace.Border = PdfPCell.NO_BORDER;
        clSpace.Colspan = noOfColumns;
        mainTable.AddCell(clSpace);

        // Sets the gridview column names as table headers.
        for (int i = 0; i < noOfColumns; i++)
        {
            Phrase ph = null;

            if (gvReport.AutoGenerateColumns)
            {
                ph = new Phrase(tbl.Columns[i].ColumnName, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
            }
            else
            {
                if (gvReport.HeaderRow.Cells[i].Visible)
                {
                    ph = new Phrase(gvReport.Columns[i].HeaderText, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                }
            }

            if (ph != null)
                mainTable.AddCell(ph);
        }

        // Reads the gridview rows and adds them to the mainTable
        for (int rowNo = 0; rowNo < noOfRows; rowNo++)
        {
            for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
            {
                if (gvReport.HeaderRow.Cells[columnNo].Visible)
                {
                    if (gvReport.AutoGenerateColumns)
                    {
                        string s = gvReport.Rows[rowNo].Cells[columnNo].Text.Trim();
                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                        mainTable.AddCell(ph);
                    }
                    else
                    {

                        if (gvReport.Columns[columnNo] is TemplateField)
                        {
                            string s = string.Empty;
                            foreach (Control ctrl in gvReport.Rows[rowNo].Cells[columnNo].Controls)
                            {
                                if (ctrl is LinkButton)
                                {
                                    LinkButton lc = (LinkButton)ctrl;
                                    s += lc.Text.Trim() + " ";
                                }
                                else if (ctrl is HyperLink)
                                {
                                    HyperLink hy = (HyperLink)ctrl;
                                    s += hy.Text.Trim() + " ";
                                }
                                else if (ctrl is Label)
                                {
                                    Label lb = (Label)ctrl;
                                    s += lb.Text.Trim() + " ";
                                }
                            }

                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                            mainTable.AddCell(ph);
                        }
                        else
                        {
                            string s = gvReport.Rows[rowNo].Cells[columnNo].Text.Trim();

                            if (s.ToLower() == "&nbsp;")
                                s = string.Empty;

                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                            mainTable.AddCell(ph);
                        }
                    }
                }
            }

            // Tells the mainTable to complete the row even if any cell is left incomplete.
            mainTable.CompleteRow();
        }

        // Sets the gridview column names as table footers.
        for (int i = 0; i < noOfColumns; i++)
        {
            Phrase ph = null;

            if (gvReport.AutoGenerateColumns)
            {
                ph = new Phrase(tbl.Columns[i].ColumnName, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
            }
            else
            {
                if (gvReport.HeaderRow.Cells[i].Visible)
                {
                    string s = gvReport.FooterRow.Cells[i].Text;

                    if (s.ToLower() == "&nbsp;")
                        s = string.Empty;

                    ph = new Phrase(s, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.NORMAL));
                }
            }

            if (ph != null)
                mainTable.AddCell(ph);
        }

        // Gets the instance of the document created and writes it to the output stream of the Response object.
        PdfWriter.GetInstance(document, HttpContext.Current.Response.OutputStream);

        // Creates a footer for the PDF document.
        HeaderFooter pdfFooter = new HeaderFooter(new Phrase(), true);
        pdfFooter.Alignment = Element.ALIGN_CENTER;
        pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

        // Sets the document footer to pdfFooter.
        document.Footer = pdfFooter;
        // Opens the document.
        document.Open();
        // Adds the mainTable to the document.
        document.Add(mainTable);
        // Closes the document.
        document.Close();

        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename= " + ReportName + ".pdf");
        HttpContext.Current.Response.End();
    }

    public static void Export2Tab(string filename, GridView grd)
    {
        Export2Tab(filename, grd, 0, grd.Columns.Count);
    }

    public static void Export2Tab(string filename, GridView grd, int start_ind, int end_ind)
    {
        GridViewExportUtil.ExportText(filename, grd, '\t', '\0', start_ind, end_ind);
    }

    public static void Export2CSV(string filename, GridView grd)
    {
        Export2CSV(filename, grd, 0, grd.Columns.Count);
    }

    public static void Export2CSV(string filename, GridView grd, int start_ind, int end_ind)
    {
        GridViewExportUtil.ExportText(filename, grd, ',', '"', start_ind, end_ind);
    }

    #region 3DE Methods
    //make a call to database to check Zeus MultiLink decision and then make a 3DE call
    public static void Call3DEVendors(MerchantApp app, string userName)
    {
        string vendorList = string.Empty;

        foreach (int i in System.Enum.GetValues(typeof(Zeus3DEVendors)))
        {
            if (i != 4) //Ani: List all except Experian Premier
                vendorList += i.ToString() + ",";
        }
        Call3DEVendors(app, userName, true, vendorList);
    }

    public static string Call3DEVendors(MerchantApp app, string userName, bool isAutomated, string vendorList)
    {
        Hashtable prms = new Hashtable();
        string jsonresponse = string.Empty;
        bool isConfiguredAgentID = GetConfiguredAgentID(app.AgentID.ToString());

        try
        {
            // int percentage3DE = 100 / Convert.ToInt32(ConfigurationManager.AppSettings["Percentage3de"]);
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            prms.Add("@UID", app.MerchantAppUID);
            prms.Add("@userName", userName);

            string EndPointURL = ConfigurationManager.AppSettings["Zeus3dePostRequest"];

            ZeusMultiLinkages MultiLinkDecision = data.GetMultiLinkData(prms);

            //Make this call only for manual for Stage 1.
            if (ConfigurationManager.AppSettings["EnableAuto3de"].ToUpper().Equals("TRUE") && isAutomated)
            {
                if (!string.IsNullOrWhiteSpace(vendorList))
                {
                    string json = GetZeus3DERequestData(app, MultiLinkDecision, vendorList, userName);
                    jsonresponse = JSONClient.PostRestService(json, EndPointURL);
                }
            }
            else if (ConfigurationManager.AppSettings["Graduated3de"].ToUpper().Equals("TRUE") && isAutomated && isConfiguredAgentID)
            //&& (LookupTableHandler.m_AgentIDSpecificAppCounter == percentage3DE))
            {
                if (!string.IsNullOrWhiteSpace(vendorList))
                {
                    string json = GetZeus3DERequestData(app, MultiLinkDecision, vendorList, userName);
                    jsonresponse = JSONClient.PostRestService(json, EndPointURL);

                    // LookupTableHandler.m_AgentIDSpecificAppCounter = 0;
                }

            }

            else if (!isAutomated)
            {
                if (!string.IsNullOrWhiteSpace(vendorList))
                {
                    string json = GetZeus3DERequestData(app, MultiLinkDecision, vendorList, userName);        
                    jsonresponse = JSONClient.PostRestService(json, EndPointURL);
                }
            }
            data.UpdateMerchantCheckListFor3DEVendors(app.MerchantAppUID, userName);
        }
        catch (Exception exc)
        {
            // Suppress any Exceptions  here.
            // throw exc;
        }

        return jsonresponse;
    }

    private static string GetZeus3DERequestData(MerchantApp mApp, ZeusMultiLinkages multiLinkDecision, string vendorList, string userRequested)
    {
        List<string> vendorSearches = vendorList.TrimEnd(',').Split(',').ToList().ConvertAll(s => ((Zeus3DEVendors)Convert.ToInt32(s)).ToString());
        //code changes for PXP-13771 changes by koshlendra for PXP-13771
        vendorSearches.Add("Linkages");

        mApp.ContactList = DataContact.SearchContact(Convert.ToInt32(mApp.ID), eControlContactType.Merchant);
        mApp.Owners = DataAccess.DataMerchantAppDao.GetOwners(mApp.MerchantAppUID);

        EquipmentFacade facade = new EquipmentFacade();
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", mApp.MerchantAppUID);
        prms.Add("@IsEnabled", true);
        mApp.Equipments = facade.GetMerchantAppItems(prms);

        //Call 3DE API.
        RequestData zeus3derequestdata = Zeus3DEBuilder.GetZeus3deRequestData(mApp, multiLinkDecision, vendorSearches, userRequested);

        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string json = serializer.Serialize(zeus3derequestdata);

        return json;

    }
    #endregion
    #endregion

    #region UNDERWRITING, FEES Pages

    internal static IList<string> RiskEvaluationCheck(MerchantApp app, string m_Status, string m_CloneStatusUID, string[] ndxDays, decimal[] periodVolumes, out decimal totalPeriodVolume, IList<string> message)
    {
        //IList<string> message = new List<string>();

        ValidateTotalPeriodVolume(periodVolumes, message, out totalPeriodVolume);
        ValidateFullFillmentPeriodAndNDXDays(app, m_Status, m_CloneStatusUID, message, ndxDays, periodVolumes);
        return message;
    }

    private static IList<string> ValidateTotalPeriodVolume(decimal[] periodVolumes, IList<string> message, out decimal totalPeriodVolume)
    {
        totalPeriodVolume = 0;
        if ((periodVolumes[0] != 0) || (periodVolumes[1] != 0) || (periodVolumes[2] != 0))
        {
            totalPeriodVolume = periodVolumes[0] + periodVolumes[1] + periodVolumes[2];
            if (totalPeriodVolume != 100)
            {
                message.Add("Sum of fulfillment period should be equal to 100.");
            }
        }
        return message;
    }

    private static IList<string> ValidateFullFillmentPeriodAndNDXDays(MerchantApp app, string m_Status, string m_CloneStatusUID, IList<string> message, string[] ndxDays, decimal[] periodVolumes)
    {
        UFAMVData uwAMVData = GetTotalApprovedVolume(app);

        if ((uwAMVData.TotalApprovedVolume) >= 100000)
        {
            if (app.IsRolloverAccount != true && app.Office != CommonUtility.Util.Offices.LosAngeles && app.Office != CommonUtility.Util.Offices.Dallas)
            {
                if (m_CloneStatusUID.ToUpper() != Constants.QUEUESTATUS_MS_ACTIVE && m_CloneStatusUID.ToUpper() != Constants.QUEUESTATUS_MS_INACTIVE && m_CloneStatusUID.ToUpper() != Constants.QUEUESTATUS_MS_CANCELLATION && m_CloneStatusUID.ToUpper() != Constants.QUEUESTATUS_MS_PENDING_CANCELLATION)
                {
                    //PXP-3235
                    if (m_Status.ToUpper() == Constants.QUEUESTATUS_MS_ACTIVE || m_Status.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED)
                    {
                        decimal period1NDXDays = 0;
                        decimal.TryParse(ndxDays[0], out period1NDXDays);

                        if (periodVolumes[0] == 0)
                        {
                            message.Add("Please enter Period1 Volume.");
                            if (!string.IsNullOrEmpty(ndxDays[0]))
                            {
                                if (ndxDays[0].Trim().All(char.IsDigit))
                                {
                                    if (period1NDXDays == 0)
                                    {
                                        message.Add("Please enter Period1 NDXDays.");
                                    }
                                }
                            }
                            else
                            {
                                message.Add("Please enter Period1 NDXDays.");
                            }
                        }
                        else if (!string.IsNullOrEmpty(ndxDays[0]))
                        {
                            if (ndxDays[0].Trim().All(char.IsDigit))
                            {
                                if (period1NDXDays == 0)
                                {
                                    message.Add("Please enter Period1 NDXDays.");
                                }
                            }
                        }
                        else
                        {
                            message.Add("Please enter Period1 NDXDays.");
                        }
                        if (!string.IsNullOrEmpty(ndxDays[1]))
                        {
                            decimal period2NDXDays = 0;
                            decimal.TryParse(ndxDays[1], out period2NDXDays);
                            if ((period2NDXDays != 0) && (ndxDays[1].Trim().All(char.IsDigit)))
                            {
                                if (period2NDXDays == 0)
                                {
                                    message.Add("Please enter Period2 NDXDays.");
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(ndxDays[2]))
                        {
                            decimal period3NDXDays = 0;
                            decimal.TryParse(ndxDays[2], out period3NDXDays);
                            if (period3NDXDays != 0 && ndxDays[2].Trim().All(char.IsDigit))
                            {

                                if (period3NDXDays == 0)
                                {
                                    message.Add("Please enter Period3 NDXDays.");
                                }
                            }
                        }
                    }
                }
            }
        }
        return message;
    }

    private static UFAMVData GetTotalApprovedVolume(MerchantApp app)
    {
        decimal vol = app.TinfoAverageMonthlyVMCVolume;
        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        UFAMVData uwAMVData = data.GetAMVData(UserSessions.CurrentMerchantApp.ID);
        if (vol > uwAMVData.ApprovedVolume)
        {
            decimal AMV = vol - uwAMVData.ApprovedVolume;
            uwAMVData.TotalApprovedVolume = uwAMVData.TotalApprovedVolume + AMV;
        }
        else if (vol < uwAMVData.ApprovedVolume)
        {
            decimal AMV = uwAMVData.ApprovedVolume - vol;
            uwAMVData.TotalApprovedVolume = uwAMVData.TotalApprovedVolume - AMV;
        }
        return uwAMVData;
    }
    #endregion

    #region UNDERWRITING, PROFILE Pages
    public static void UploadPDF(bool isOpsForm, bool isStatuschanged, AchMerchant achMerchant)
    {
        MerchantApp app = UserSessions.CurrentMerchantApp;
        custom.Clear();
        //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
        UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(UserSessions.CurrentMerchantApp.ID));
        if (isStatuschanged)
        {
            if ((MerchantFacade.IsFirstInstanceOfStatus(UserSessions.CurrentMerchantApp.MerchantAppUID, Constants.QUEUESTATUS_OP_RECEIVED))
                || ((achMerchant != null) && ((achMerchant.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_OP_RECEIVED) && (achMerchant.AchMerchantClone.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))))))
            {
                custom.Add("UploadFormType", "CUPkg_Ops_Form");
                FillPDF(UserSessions.CurrentMerchantApp.Brand);
            }
        }
        else
        {
            if (isOpsForm)
            {
                FillOpsPDF(UserSessions.CurrentMerchantApp.Brand);
            }
            else
            {
                if ((!app.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_ACH_ONLY)) && (!app.StatusName.ToUpper().Substring(0, 2).Equals(Constants.QueueSS_Status)))
                    FillPDF(UserSessions.CurrentMerchantApp.Brand);
                else if ((app.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_ACH_ONLY)) && (!string.IsNullOrEmpty(app.ACHStatus)))
                {
                    if (!app.ACHStatus.ToUpper().Substring(0, 2).Equals(Constants.QueueSS_Status))
                        FillPDF(UserSessions.CurrentMerchantApp.Brand);
                }
            }
        }

    }
    //modify PXP-7469 abarua
    public static string GetUWIssuesTemplate(string merchantapptypeuid, MerchantBrand brand, int Office)
    {
        string AcqBank_OnlineBankOnly = "8d2281ae-e6f0-429e-94d0-c0fe6bfede01";

        StringBuilder sbuw = new StringBuilder();
        StringBuilder sbcc = new StringBuilder();
        StringBuilder sbob = new StringBuilder();
        StringBuilder sbach = new StringBuilder();

        string line_template = "{0}: {1}";

        if (Office != 1)
        {
            sbuw.AppendLine(string.Format(line_template, "General Set up Details", ""));
            sbuw.AppendLine(string.Format(line_template, "Category Code", ""));
            sbuw.AppendLine(string.Format(line_template, "Min Float", ""));
            sbuw.AppendLine(string.Format(line_template, "Max Float", ""));
        }
        sbcc.AppendLine("   ");
        sbcc.AppendLine(string.Format(line_template, "CC Brand Details", ""));
        sbcc.AppendLine(string.Format(line_template, "[CREDORAX] Signature Key/Authentication Code", ""));
        sbcc.AppendLine(string.Format(line_template, "[NATWEST] Streamline ID", ""));
        sbcc.AppendLine(string.Format(line_template, "[NATWEST] Sort Code", ""));
        sbcc.AppendLine(string.Format(line_template, "[NATWEST] Bank Account (settlement bank)", ""));
        sbcc.AppendLine(string.Format(line_template, "[NAB] Live Transaction Password", ""));
        sbcc.AppendLine(string.Format(line_template, "[CASHFLOWS/VCL] Password/ Business ID", ""));
        sbcc.AppendLine("   ");
        sbcc.AppendLine(string.Format(line_template, "CC Processing Options", ""));
        sbcc.AppendLine(string.Format(line_template, "Store Data Account Group", ""));
        sbcc.AppendLine(string.Format(line_template, "Credit Account Group", ""));
        sbcc.AppendLine(string.Format(line_template, "CVD Captured:  Enforce (E) / Unenforce (U) [default is E]", ""));
        sbcc.AppendLine(string.Format(line_template, "AVS Captured: Enable (E) / Disable (D) and Enforce (E) /Unenforce (U) [default is E/E]", ""));
        sbcc.AppendLine(string.Format(line_template, "Negative Database: Enable (E) / Disable (D) [default is E]", ""));
        sbcc.AppendLine(string.Format(line_template, "Overdraft Protection: Enable (E) / Disable (D) [default is E]", ""));
        sbcc.AppendLine(string.Format(line_template, "Integration method -> Webservices (cardinal) or REST", ""));
        sbcc.AppendLine(string.Format(line_template, "If REST, Hosted API or 3D API", ""));

        if (CommonUtility.Util.if_s(merchantapptypeuid).ToUpper() == AcqBank_OnlineBankOnly.ToUpper())
        {
            sbob.AppendLine("  ");
            sbob.AppendLine(string.Format(line_template, "OB processing options", ""));
            sbob.AppendLine(string.Format(line_template, "Processor", ""));
            sbob.AppendLine(string.Format(line_template, "Merchant Number[003(Uppercase CHID) for Interac]", ""));
            sbob.AppendLine(string.Format(line_template, "CHID", ""));
            sbob.AppendLine(string.Format(line_template, "API TOKEN [Moneris]", ""));
            sbob.AppendLine("  ");
            sbob.AppendLine(string.Format(line_template, "[Sofort/Ideal] Project Name", ""));
            sbob.AppendLine(string.Format(line_template, "[Sofort/Ideal] Merchant URL", ""));
            sbob.AppendLine(string.Format(line_template, "[Sofort/Ideal] Success link", ""));
            sbob.AppendLine(string.Format(line_template, "[Sofort/Ideal] Abort link", ""));
            sbob.AppendLine(string.Format(line_template, "[Sofort/Ideal] Category of Ecommerce from Risk", ""));
            sbob.AppendLine("  ");
            sbob.AppendLine(string.Format(line_template, "[Paypal NA] Username", ""));
            sbob.AppendLine(string.Format(line_template, "[Paypal NA] Password", ""));
            sbob.AppendLine(string.Format(line_template, "[Paypal NA] API Signature", ""));
            sbob.AppendLine(string.Format(line_template, "[Paypal NA] Checkout Brand Text", ""));
            sbob.AppendLine(string.Format(line_template, "[Paypal NA] Cart Border Colour", ""));
            sbob.AppendLine(string.Format(line_template, "[Paypal NA] Checkout Header Image", ""));
            sbob.AppendLine(string.Format(line_template, "[Paypal NA] Instant Payment Only", ""));
            sbob.AppendLine(string.Format(line_template, "[Paypal NA] Paypal Account Login", ""));
            sbob.AppendLine(string.Format(line_template, "[Paypal NA] Display Buyer Paypal Address", ""));
            sbob.AppendLine("   ");
            sbob.AppendLine(string.Format(line_template, "OB Pricing details", ""));

            return sbuw.Append(sbob.ToString()).ToString();

        }


        string bank_achonly_uid = "DADC71E7-A732-4FCA-8C86-DE3E7253209C";
        if (brand == MerchantBrand.Optimal && merchantapptypeuid.ToUpper() == bank_achonly_uid)
        {
            sbach.AppendLine("    ");
            sbach.AppendLine("DD UK properties and values");
            sbach.AppendLine(string.Format(line_template, "Bank Relationship(Gateway/Bureau/Master Merchant)", ""));
            sbach.AppendLine(string.Format(line_template, "Company name or service (max 18 chars)", ""));
            sbach.AppendLine(string.Format(line_template, "Company name must always be OPTIMALPAYMENTSLTD (no spaces) for Master Merchant", ""));
            sbach.AppendLine(string.Format(line_template, "Customer Notice Period(Enter 3 unless advised)", ""));
            sbach.AppendLine(string.Format(line_template, "DD Options Service User Bank Account Number(enter 999999 if not yet allocated)", ""));
            sbach.AppendLine(string.Format(line_template, "DD Options Service User Bank Sort Code(enter 9999 if not yet allocated)", ""));
            sbach.AppendLine(string.Format(line_template, "SUN(enter 999999 if not yet allocated)", ""));
            sbach.AppendLine(string.Format(line_template, "Short name if using Master Merchant(max 8 characters used in Mandate Ref)", ""));
            sbach.AppendLine("    ");
            sbach.AppendLine("DD processing options");
            sbach.AppendLine(string.Format(line_template, "Charge (Y/N) [default is Y]", ""));
            sbach.AppendLine(string.Format(line_template, "Credit (Y/N) [default is Y]", ""));
            sbach.AppendLine(string.Format(line_template, "ILS (Y/N) [default is Y]", ""));
            sbach.AppendLine(string.Format(line_template, "Mandate (Y/N) [default is Y](only select N if Charge is set to N)", ""));
            sbach.AppendLine(string.Format(line_template, "Enable RRE [default is Y]", ""));
            sbach.AppendLine(string.Format(line_template, "Enable Overdraft Funding [default is Y]", ""));
            sbach.AppendLine(string.Format(line_template, "Representment attempts (0/1/2)", ""));
            sbach.AppendLine(string.Format(line_template, "1st Happens", ""));

            return sbuw.Append(sbach.ToString()).ToString();
        }

        return sbuw.Append(sbcc.ToString()).ToString();

    }
    //PXP-7469 abarua
    public static string GetUWIssuesTemplate(MerchantApp agreement)
    {
        return GetUWIssuesTemplate(agreement.MerchantAppUID, agreement.Brand, Convert.ToInt32((CommonUtility.Util.Offices)agreement.Office));
    }
    #endregion

    #region UNDERWRITING, UNDERWRITING PENDING, DEPLOYMENT, PROFILE  Pages
    //This method takes in the BankUID and returns the MID after all the checks.
    //Generate the entire MID on the database itself
    public static string GenerateMID(string MerchantAppType, MerchantBrand brand)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppTypeUID", MerchantAppType);
        prms.Add("@BrandID", Convert.ToInt32(brand));
        return DataAccess.DataMerchantAppDao.GenerateMID(prms);
    }

    /// <summary>
    /// pxp-10043[Auto-populated backend MIDs for all applications on BBVA bank in Irvine office when status changed to CU-Approved] by koshlendra
    /// </summary>
    /// <param name="MerchantAppType"></param>
    /// <param name="brand"></param>
    /// <returns></returns>
    public static MerchantApp CheckGenerateMIDforBBVA(MerchantApp agreement, bool CCApproved, string m_StatusUID)
    {
        //code changes doen for PXP-10201 by koshlendra
        if (agreement.Office == CommonUtility.Util.Offices.Irvine
               && !agreement.SettlePlatformUID.Equals(Constants.BankEndTsysUID)
               && agreement.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS) && agreement.Brand == MerchantBrand.Meritus
               && (CCApproved || m_StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED.ToUpper()) || agreement.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED.ToUpper())))
        {
            //Mid is generated only when the Auth MID and Settle MID are empty and bank is WoodForest
            if (string.IsNullOrEmpty(agreement.SettlePlatformMid))
            {
                agreement.SettlePlatformMid = GenerateMIDFromList(agreement.MerchantAppTypeUID);
                agreement.SettlePlatformUID = Constants.BankEndOmahaUID;
            }
        }
        return agreement;
    }

    //START:PXP-9144 Autogenerated MID for Woodforest bank By Ali Khan
    //Generate Back MIDs for Woodforest Bank and                     
    //if the aplication is moved to CU - Approved Or if it was already in CU- Approved and Bank is changed.
    public static MerchantApp CheckGenerateMIDforWoodforest(MerchantApp agreement, bool CCApproved, string m_StatusUID, string AgentLevel)
    {
        if (agreement.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
           && (CCApproved || m_StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED.ToUpper()))
           && !agreement.SettlePlatformUID.Equals(Constants.BankEndTsysUID)
           && string.IsNullOrEmpty(agreement.SettlePlatformMid)
           && Constants.AgentsCitizens.Contains(agreement.AgentID))
        {
            agreement.SettlePlatformMid = GenerateMIDFromList(agreement.MerchantAppTypeUID, true);
            agreement.SettlePlatformUID = Constants.BankEndOmahaUID;
        }
        else if (agreement.Office == CommonUtility.Util.Offices.Irvine
            && (agreement.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST) || agreement.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)) //DM-177 By Jorge
                                                                                                                                                                    //&& AgentLevel == "800"
                                                                                                                                                                    //&& agreement.IsNutraMerchant
            && (CCApproved || m_StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED.ToUpper()))
            && !agreement.SettlePlatformUID.Equals(Constants.BankEndTsysUID)
            && string.IsNullOrEmpty(agreement.SettlePlatformMid))
        {
            //Mid is generated only when the Auth MID and Settle MID are empty and bank is WoodForest or Citizens
            agreement.SettlePlatformMid = GenerateMIDFromList(agreement.MerchantAppTypeUID);
            agreement.SettlePlatformUID = Constants.BankEndOmahaUID;
        }
        return agreement;
    }

    public static MerchantApp CheckGenerateMIDforChesapeake(MerchantApp agreement, bool CCApproved, string m_StatusUID)
    {
        if (agreement.Office == CommonUtility.Util.Offices.Irvine
               && agreement.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CHESAPEAKE)
               && (CCApproved || m_StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED.ToUpper()) || agreement.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED.ToUpper())))
        {
            //Mid is generated only when the Auth MID and Settle MID are empty
            if (string.IsNullOrEmpty(agreement.SettlePlatformMid))
            {
                agreement.SettlePlatformMid = GenerateMIDFromList(agreement.MerchantAppTypeUID);
                agreement.SettlePlatformUID = Constants.BankEndTsysUID;
            }

            //Discover MID is generated
            if (string.IsNullOrEmpty(agreement.DiscoverMid) && agreement.DiscoverNovus)
            {
                agreement.DiscoverMid = GetDiscoverMID(agreement.MerchantAppTypeUID);
            }
        }
        return agreement;
    }
    
    public static string GenerateMIDFromList(string MerchantAppType, bool IsSpecialAgent = false)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppTypeUID", MerchantAppType);
        prms.Add("@IsSpecialAgent", IsSpecialAgent);
        return DataAccess.DataMerchantAppDao.GetNewMIDFromList(prms);
    }

    public static string GetDiscoverMID(string MerchantAppType)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppTypeUID", MerchantAppType);
        return DataAccess.DataMerchantAppDao.GetDiscoverMID(prms);
    }

    //END:PXP-9144 Autogenerated MID for Woodforest bank By Ali Khan
    #endregion

    #region UNDERWRITING, UNDERWRITING PENDING, DEPLOYMENT, PROFILE, FEES, OWNERS Pages
    //this method is called after updating a merchant account. this sends out an email to agent if there is a status change
    //also creates product tickets baased on the status
    public static void CompleteApplication(MerchantApp agreement, AchMerchant achMerchant, string StatusClone, string UserName, bool isCloseApplicationTicket = true)
    {
        CompleteApplication(agreement, achMerchant, StatusClone, UserName, CommonUtility.Util.Offices.NoOffice, isCloseApplicationTicket);
    }

    public static void CompleteApplication(MerchantApp agreement, AchMerchant achMerchant, string StatusClone, string UserName, CommonUtility.Util.Offices officeClone, bool isCloseApplicationTicket)
    {
        agreement.AgentAlerts = DataAccess.DataAgentDao.LoadAgentAlerts(agreement.AgentID);

        AlertNotification.SendApplicationNotification(agreement, achMerchant, StatusClone, UserName, ePortals.ZEUS);

        // when changed to OP-Received, then we create the product tickets.
        if ((achMerchant != null && achMerchant.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED) ||
            (agreement.StatusUID.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED))
        {
            // note: this implies that if the merchantapp status jumps from op-received to something else, and products
            // are subscribed in the meantime, then once it's changed back, then it will pick up where it left off.
            ProductSubscriptionService.CreateTicketsForProductsWithoutSubscriptionTicket(agreement, UserName, ePortals.ZEUS);   //PXP-2955 Rohit Thakur
        }

        if ((agreement.Office != CommonUtility.Util.Offices.Dallas)
            && ((agreement != null && agreement.StatusUID.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED && StatusClone.ToUpper() != agreement.StatusUID.ToUpper())
            || ((achMerchant != null) && (achMerchant.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_OP_RECEIVED)
            && (achMerchant.AchMerchantClone != null && achMerchant.AchMerchantClone.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))))))
        {
            UploadPDF(false, true, achMerchant);
        }

        //make a call to 3DE for all vendors automatically
        if ((agreement != null && agreement.StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_RECEIVED)
            || (achMerchant != null && achMerchant.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_RECEIVED))
        {
            if ((MerchantFacade.IsFirstInstanceOfStatus(agreement.MerchantAppUID, Constants.QUEUESTATUS_CU_RECEIVED))
                // || ((achMerchant != null) && ((achMerchant.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_RECEIVED) && achMerchant.ACHStatus.ToUpper().Substring(0, 2).Equals(Constants.QueueSS_Status)))))
                && (agreement.Office == CommonUtility.Util.Offices.Irvine))
            {
                if (StatusClone.ToUpper() != agreement.StatusUID.ToUpper() || (officeClone != CommonUtility.Util.Offices.NoOffice && officeClone != agreement.Office))
                {
                    CreditDecisionCall creditDecision;
                    try
                    {
                        DataCreditDecisionCall creditDecisionCallObj = DataCreditDecisionCall.GetInstance();
                        creditDecision = creditDecisionCallObj.GetCreditDecisionCall(agreement.MerchantAppTypeUID);

                        if (creditDecision.IsEnabled)
                        {
                            Call3DEVendors(agreement, UserName);
                        }
                    }

                    catch (Exception e)
                    {
                        Logging.ErrorLog.Info($"Error ocurred while retrieving credit decision call for MerchantTypeUID: {agreement.MerchantAppTypeUID}");
                    }
                }
            }
        }

        //PXP-7620 RThakur
        string statusToCheck;
        if (isCloseApplicationTicket)
        {
            if (achMerchant != null)
            {
                statusToCheck = achMerchant.MerchantStatusUID.ToUpper();
            }
            else
            {
                statusToCheck = agreement.StatusUID.ToUpper();
            }
            if (((StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_CU_RECEIVED) || StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_CU_RECEIVED_PD) || StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_CU_PENDING) ||
                    StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_CU_PRECHECK) || StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_CU_SUBMITTED_TO_BANK) || StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_CU_BANK_REQUESTED) ||
                    StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_CU_IN_REVIEW) || StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED) || StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_CU_DECLINED)
                    || StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_CU_WITHDRAWN) || StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_CU_3DEDECISION))
                    &&
                     (statusToCheck.Equals(Constants.QUEUESTATUS_CU_DECLINED) || statusToCheck.Equals(Constants.QUEUESTATUS_CU_WITHDRAWN)
                     )
                 )
                 ||
                 (StatusClone.ToUpper().Equals(Constants.QUEUESTATUS_SS_APP_INCOMPLETE) && statusToCheck.Equals(Constants.QUEUESTATUS_SS_WITHDRAWN)
                 )
                )
            {
                CloseApplicationTicket(agreement, UserSessions.CurrentUser);
            }
        }

    }

    //PXP-7620 RThakur
    public static void CloseApplicationTicket(MerchantApp app, User user)
    {
        DataTicket data = new DataTicket();
        Ticket ticket = new Ticket();
        IList<Ticket> list = null;

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantUID", app.MerchantAppUID);
        prms.Add("@LastTicketOnly", 1);
        list = data.GetLastNewApplicationTicket(prms);
        if (list.Count > 0)
        {
            ticket = list[0];
            if (ticket.StatusID.ToUpper() != Ticket.TICKET_CLOSE)
            {
                ticket.StatusID = Ticket.TICKET_CLOSE;
                ticket.UserModified = user.UserName;
                ticket.DateModified = DateTime.Now;
                data.UpdateTicket(ticket);
                TicketNotes note = new TicketNotes();
                note.DateCreated = DateTime.Now;
                note.Description = "Automatically closing system generated ticket. Please refer to account status and/or notes for additional information.";
                note.IsInternal = true;
                note.TicketID = ticket.TicketID;
                note.UserCreated = user.UserName;
                note.IsSolution = true;
                DataAccess.DataTicketNotesDao.InsertTicketNotes(note);
                PaymentXP.Facade.TicketNotification.TicketClosed(ticket.TicketUID, false);
            }
        }
    }


    #endregion
    #region UNDERWRITING, UNDERWRITING PENDING, DEPLOYMENT, PROFILE, FEES, OWNERS, ACH Pages
    public static IList<string> MerchantDataCheck(MerchantApp OrigApp, bool isOwner, bool isAdding, string NewStatusUID, AchMerchant achMerchant)
    {
        string Statusuid = string.Empty;
        string Closurecode = string.Empty;
        string bank = string.Empty;
        string AuthPlatformMid = string.Empty;
        string SettlePlatformMid = string.Empty;
        string AuthPlatformUid = string.Empty;
        string SettlePlatformUid = string.Empty;
        string ETFAssessed = string.Empty;
        string OrigStatus = string.Empty;
        string StatusName = string.Empty;

        //get all the UID vlaues into variables
        Closurecode = OrigApp.MerchantClosureCodeUID.Replace("-1", string.Empty).ToUpper();
        bank = OrigApp.MerchantAppTypeUID.Replace("-1", string.Empty).ToUpper();
        AuthPlatformMid = OrigApp.AuthPlatformMid.ToUpper();
        SettlePlatformMid = OrigApp.SettlePlatformMid.ToUpper();
        AuthPlatformUid = OrigApp.AuthPlatformUID.Replace("-1", string.Empty).ToUpper();
        SettlePlatformUid = OrigApp.SettlePlatformUID.Replace("-1", string.Empty).ToUpper();
        ETFAssessed = OrigApp.ETFAssessed;

        MerchantApp cloneApp = null;

        //check to see if the account is ach only
        bool isAchOnly = !isAdding && (bank == Constants.BANK_ACH_ONLY && OrigApp.AchID > 0);
        // check if there is ACH and CC 
        bool isAchExist = !isAdding && OrigApp.AchID > 0;


        //Statusuid = NewStatusUID;

        IList<string> message = new List<string>();
        if (isAchOnly)
        {
            if (achMerchant != null)
            {
                Statusuid = achMerchant.MerchantStatusUID.ToUpper();
                //OrigStatus = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
                if (achMerchant.AchMerchantClone != null)
                {
                    OrigStatus = achMerchant.AchMerchantClone.MerchantStatusUID.ToUpper();
                }

                achMerchant.MerchantStatusUID = NewStatusUID;
                Statusuid = NewStatusUID;
            }

            StatusName = ((achMerchant != null)) ? UserSessions.ActiveAchMerchant.MerchantStatusName.ToUpper() : StatusName;
        }
        else
        {
            Statusuid = OrigApp.StatusUID.ToUpper();
            if (OrigApp.MerchantAppClone != null)
            {
                OrigStatus = OrigApp.MerchantAppClone.StatusUID.ToUpper();
                StatusName = OrigApp.MerchantAppClone.StatusName.ToUpper();
                cloneApp = OrigApp.MerchantAppClone;
            }
            else
            {
                //PXP-3390
                OrigStatus = OrigApp.StatusUID.ToUpper();
                StatusName = OrigApp.StatusName.ToUpper();
            }

            StatusName = ((achMerchant != null) && achMerchant.FromACH) ? UserSessions.ActiveAchMerchant.MerchantStatusName.ToUpper() : StatusName;

        }

        bool fromACH = (achMerchant != null) ? achMerchant.FromACH : false;

        bool fromStatus = String.IsNullOrEmpty(StatusName) ? false : StatusName.ToUpper().Substring(0, 2).Equals(Constants.QueueSS_Status);
        //Fmassoud 2017.08.24 - ACH/CC Validation Only when changing Status to CU Recieved 
        if (fromStatus && NewStatusUID == Constants.QUEUESTATUS_CU_RECEIVED && OrigApp.Office == CommonUtility.Util.Offices.Irvine && OrigApp.Brand == MerchantBrand.Meritus)
        {
            if (isAchExist)
            {
                // ACH Validation when SS Statuses to CU_RECEIVED
                if (achMerchant.MonthlyVolume <= 0)
                    message.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_MonthlyVolume));

                if (achMerchant.AverageTicket <= 0)
                    message.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_AverageTicket));

                if (achMerchant.HighTicket <= 0)
                    message.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_HighTicket));
            }
            // Credit Validation when SS Statuses to CU_RECEIVED
            if (bank != Constants.BANK_ACH_ONLY && !fromACH)
            {
                if (OrigApp.TinfoAverageMonthlyVMCVolume <= 0)
                    message.Add(Constants.GetDescription(Constants.ErrorCodes.Fees_MonthlyVolume));

                if (OrigApp.TinfoAverageVMCTicket <= 0)
                    message.Add(Constants.GetDescription(Constants.ErrorCodes.Fees_AverageTicket));

                if (OrigApp.TinfoHighestTicketAmount <= 0)
                    message.Add(Constants.GetDescription(Constants.ErrorCodes.Fees_HighTicket));
            }
            if (message.Count() > 0)
            {
                if (UserSessions.CurrentAchMerchant != null)
                    UserSessions.CurrentAchMerchant.MerchantStatusUID = OrigStatus;
            }
        }

        //Do this check only when the status is changed.
        if (Statusuid != OrigStatus)
        {
            //User needs to be part of the CU Department to mark the status as CU - Approved.
            if ((Statusuid.ToLower() == Constants.QUEUESTATUS_CU_APPROVED.ToLower() || Statusuid.ToLower() == Constants.QUEUESTATUS_CU_DECLINED.ToLower() || Statusuid.ToLower() == Constants.QUEUESTATUS_CU_WITHDRAWN.ToLower()) && !(UserSessions.CurrentUser.DefaultRoleUID == Constants.ROLE_CREDIT_UNDERWRITING.ToLower()))
            {
                message.Add("You do not have permission to set this account to be CU Approved/Declined/Withdrawn.");
            }

            //PXP-17195 start
            if (OrigStatus.ToLower().Equals(Constants.QUEUESTATUS_SS_RECEIVED.ToLower()))
            {
                if (UserSessions.CurrentMerchantApp != null && (string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.PrimaryContactUID) || UserSessions.CurrentMerchantApp.PrimaryContactUID == "-1"))
                    message.Add("Please select a SS-Rep.");
            }
            //PXP-17195 End
        }

        //if bank is ach only and  user wants to update the CC stauts onhta account wihtout an achid, donot allow.
        //restrict user to update only ach status for the ach only accounts.
        if (string.IsNullOrWhiteSpace(bank))
        {
            message.Add("Please select Acq. Bank.");
        }
        else
        {
            if (bank == Constants.BANK_ACH_ONLY && OrigApp.AchID == 0 && Statusuid != OrigStatus && !isAdding)
                message.Add("Please add ACH Profile for the account before changing the status.");
        }

        if (OrigApp.Brand == MerchantBrand.None)
            message.Add("Please select Merchant Brand.");

        if (string.IsNullOrWhiteSpace(OrigApp.AgentUID))
            message.Add("Please enter AgentID.");

        if (!string.IsNullOrWhiteSpace(OrigApp.AuthPlatformUID)
            && AuthPlatformUid == SettlePlatformUid && AuthPlatformMid != SettlePlatformMid)
        {
            message.Add("Front and Back MID must be equal.");
        }

        if (IsDuplicateMID(AuthPlatformMid, SettlePlatformMid, isAdding, OrigApp.MerchantAppUID, OrigApp.Brand))
        {
            if (OrigApp.Brand == MerchantBrand.Meritus)
            {
                message.Add("Backend MID already exists for another merchant.");
            }

            else if (OrigApp.Brand == MerchantBrand.Optimal)
            {
                message.Add("This Backend MID cannot be entered for this merchant.");
            }
        }
        //PXP-8409 and PXP-8072by Sanidhya
        //Validating CRM flow for Merchant type cc and Nutra trial
        if (!isAchOnly)
        {
            if ((OrigStatus.ToUpper() == Constants.QUEUESTATUS_SS_RECEIVED.ToUpper() || OrigStatus.ToUpper() == Constants.QUEUESTATUS_SS_DRAFT.ToUpper() ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_SS_WITHDRAWN.ToUpper() || OrigStatus.ToUpper() == Constants.QUEUESTATUS_SS_APP_INCOMPLETE.ToUpper())
                && Statusuid.ToUpper() == Constants.QUEUESTATUS_CU_RECEIVED.ToUpper()
                && OrigApp.Office == CommonUtility.Util.Offices.Irvine &&
                (OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS)
                && OrigApp.IsNutraMerchant)//PXP-9347 By Sanidhya
            {
                if (OrigApp.CRMCount == 1)
                {
                    if (OrigApp.CRMStatus == "blank")
                    {
                        message.Add("CRM TPP Registration is blank");

                    }
                    else if (OrigApp.CRMStatus == "Not Approved")
                    {
                        message.Add("CRM TPP Registration is Not Approved");
                    }

                }
            }//Change in if condition due to validation not appear up in Conditions page when saving the form second time
            //Fixes done for PXP-9151 by Sanidhya
            if (Statusuid.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED &&
                (OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_RECEIVED ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_RECEIVED_PD ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_PENDING ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_PRECHECK ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_SUBMITTED_TO_BANK ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_BANK_REQUESTED ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_IN_REVIEW ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_DECLINED ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_WITHDRAWN ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_3DEDECISION)
                && OrigApp.IsNutraMerchant && OrigApp.Office == CommonUtility.Util.Offices.Irvine
                && (OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS))
            {
                if (OrigApp.CRMCount == 1)
                {
                    if (OrigApp.CRMStatus == "blank")
                    {
                        message.Add("CRM TPP Registration is blank");

                    }
                    else if (OrigApp.CRMStatus == "Not Approved")
                    {
                        message.Add("CRM TPP Registration is Not Approved");
                    }
                    else if (OrigApp.CRMStatus == "InActive")
                    {
                        message.Add("CRM TPP Registration is InActive.");
                    }
                    else if (OrigApp.CRMStatus == "NA" && !OrigApp.CRMAcceptTransactions)
                    {
                        message.Add("CRM TPP Registration is NA");
                    }
                }
            }//PXP-9162 Bug Fix by Sanidhya
            if (Statusuid.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED &&
                (OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_RECEIVED ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_RECEIVED_PD ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_PENDING ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_PRECHECK ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_SUBMITTED_TO_BANK ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_BANK_REQUESTED ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_IN_REVIEW ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_DECLINED ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_WITHDRAWN ||
                OrigStatus.ToUpper() == Constants.QUEUESTATUS_CU_3DEDECISION)
                && UserSessions.CurrentMerchantApp.IsNutraMerchant)
            {
                if (OrigApp.CRMCount > 1)
                {
                    message.Add("Merchant have multiple CRM records. There should be only one CRM record.");
                }
            }

        }

        if (!isAdding)
        {
            //Validate the phone number when status change from SS-RECIEVED to CU_RECEVIED and Office ID is IRVINE
            if (Statusuid != OrigStatus)
            {
                if (Statusuid.ToLower() == Constants.QUEUESTATUS_CU_RECEIVED.ToLower())
                {
                    string strBusinessDBAPhonetxt = CommonUtility.Util.GetNumbersFromString(OrigApp.BusinessDBAPhone); //OrigApp.BusinessDBAPhone.Replace("-", "").Trim();
                    string strBusinessFaxtxt = CommonUtility.Util.GetNumbersFromString(OrigApp.BusinessFax); //OrigApp.BusinessFax.Replace("-", "").Trim();
                    string strCustomerPhonetxt = CommonUtility.Util.GetNumbersFromString(OrigApp.CustomerServicePhone); //OrigApp.CustomerServicePhone.Replace("-", "").Trim();

                    if (OrigApp.Office == CommonUtility.Util.Offices.Irvine)
                    {
                        if ((!string.IsNullOrWhiteSpace(strBusinessDBAPhonetxt) && strBusinessDBAPhonetxt.Length != 10) ||
                            (!string.IsNullOrWhiteSpace(strBusinessFaxtxt) && strBusinessFaxtxt.Length != 10) ||
                            (!string.IsNullOrWhiteSpace(strCustomerPhonetxt) && strCustomerPhonetxt.Length != 10) ||
                            (!string.IsNullOrWhiteSpace(OrigApp.BusinessDBAPhoneExt) && string.IsNullOrWhiteSpace(strBusinessDBAPhonetxt)) ||
                            (!string.IsNullOrWhiteSpace(OrigApp.BusinessFaxExt) && string.IsNullOrWhiteSpace(strBusinessFaxtxt)) ||
                            (!string.IsNullOrWhiteSpace(OrigApp.CustomerServicePhoneExt) && string.IsNullOrWhiteSpace(strCustomerPhonetxt)))
                        {
                            message.Add("If entered, Phone and Fax numbers must all be exactly 10 digits: area code + phone number. There is a separate field for country code and extension.");
                        }
                    }

                    if (strBusinessDBAPhonetxt.Length > 12)
                    {
                        message.Add("Please enter at the max 12 digit Business phone number.");
                    }

                    if (strBusinessFaxtxt.Length > 12)
                    {
                        message.Add("Please enter at the max 12 digit Business Fax number.");
                    }

                    if (strCustomerPhonetxt.Length > 12)
                    {
                        message.Add("Please enter at the max 12 digit Customer Service phone number.");
                    }

                }

            }

            // this is a temporary change, we will make the 3DE decisioning available for other offices also.
            if (Statusuid.ToUpper() == Constants.QUEUESTATUS_CU_3DEDECISION && OrigApp.Office != CommonUtility.Util.Offices.Irvine)
            {
                message.Add("3DE decision status is applicable to Irvine office only.");
            }

            //the status check should be done based on the type of account. if account is ach only then use the ach status
            //or else check teh cc status
            if (isAchOnly && achMerchant != null)
            {
                string strerr = MerchantFacade.ACHMerchantStatusChangedCheck(achMerchant, OrigApp);

                if (!string.IsNullOrWhiteSpace(strerr))
                    message.Add(strerr);
            }
            else if (!isAchOnly)
            {
                //PXP-3246
                string str = MerchantFacade.MerchantStatusChangedCheck(cloneApp, OrigApp, UserSessions.CurrentUser.UserName);

                if (!string.IsNullOrWhiteSpace(str))
                    message.Add(str);

                // if (Statusuid == Constants.QUEUESTATUS_MS_CANCELLATION || Statusuid == Constants.QUEUESTATUS_MS_PENDING_CANCELLATION)
                if (Statusuid == Constants.QUEUESTATUS_MS_CANCELLATION)   // ***PXP 1261 : remove closur code & ETF validation MS_RETENTION-PENDING-CANCELLATION status
                {
                    //when account cc status is cancelled or pending cancelled then check for ETf assessed.
                    //ETF should be checked only for cc status ignore for ach status
                    if (ETFAssessed == "-1")
                    {
                        message.Add("Please select ETF assessed.");
                    }
                }
            }


            //when ach/cc status is cancelled or pending cancelled then check for closure code.
            // if (Statusuid == Constants.QUEUESTATUS_MS_CANCELLATION || Statusuid == Constants.QUEUESTATUS_MS_PENDING_CANCELLATION)
            if (Statusuid == Constants.QUEUESTATUS_MS_CANCELLATION)   // ***PXP 1261 : remove closur code & ETF validation MS_RETENTION-PENDING-CANCELLATION status
            {
                if (string.IsNullOrWhiteSpace(Closurecode))
                {
                    message.Add("Please select a closure code.");
                }

                if (Statusuid == Constants.QUEUESTATUS_MS_PENDING_CANCELLATION)
                {
                    if (CommonUtility.Util.if_date(OrigApp.CancellationDate.ToString(), DateTime.MinValue) == DateTime.MinValue)
                        message.Add("Please select a cancellation date.");
                    else if (CommonUtility.Util.if_date(OrigApp.CancellationDate.ToString(), DateTime.MinValue) < DateTime.Today)
                        message.Add("Cancellation date cannot be less than current date.");
                }

            }
            //Code added for PXP-12932 by koshlendra start
            if (Statusuid == Constants.QUEUESTATUS_CU_APPROVED && Statusuid != OrigStatus)
            {
                if (string.IsNullOrWhiteSpace(OrigApp.VisaSicCode))
                    message.Add("Please enter Visa MCC Code.");
            }
            //Code added for PXP-12932 by koshlendra end
            //when tryign to approve a multi linked account, check the UWOverride checkbox on credit page 
            if (!isOwner)
            {
                if (OrigApp.MultiAccLink && Statusuid == Constants.QUEUESTATUS_CU_APPROVED && Statusuid != OrigStatus)
                {
                    if (!OrigApp.UWMultOverride)
                        message.Add("Status change denied. Due to a multiple account link, an override is required in order to change the status to Approved.");
                }

            }

            //the QA and welcome kit sent checkboxes on deployment page should be checked before the account is moved to DP training
            if (!OrigApp.QAPerformed || !OrigApp.WelcomeKitSent)
            {
                //DM-507 -Chandra
                if (Statusuid == Constants.QUEUESTATUS_DP_SCHEDULE_DOWNLOAD_TRAINING_SOFTWARE)// || Statusuid == Constants.QUEUESTATUS_DP_SCHEDULE_DOWNLOAD_TRAINING_HARDWARE)
                {
                    message.Add("Confirm that WelcomeKit is Sent and Q/A is Performed before the status is changed.");
                }
            }

            //when approving an account, check if there are any conditions on the account .
            //if there are any and the accoutn should be approved then check teh Conditional Aprpove checkbox on credit page.
            //check if the PCI level is also selected.this check if for only cc accounts
            if (Statusuid == Constants.QUEUESTATUS_CU_APPROVED && Statusuid != OrigStatus)
            {
                bool chkAllInfo = true;

                IList<GenericListItem> list = DataAccess.DataMerchantAppDao.GetMerchantUWEmailBody(OrigApp.MerchantAppUID, "CU");

                if (list.Count > 0)
                    chkAllInfo = Convert.ToBoolean(list[0].ItemValue);

                if (!chkAllInfo && !OrigApp.ConditionalApproval)
                {
                    message.Add("Application has conditions, please check the 'Conditional Approval' checkbox on credit page to approve.");
                }

                if (!isAchOnly && (OrigApp.PCILevel < 0 || string.IsNullOrWhiteSpace(OrigApp.PCILevel.ToString())))
                    message.Add("Please select PCI level.");
            }

            //before moving an account to CU received, make sure all conditions on the account are cleared
            if (Statusuid == Constants.QUEUESTATUS_CU_RECEIVED && Statusuid != OrigStatus)
            {
                bool chkAllInfo = true;
                IList<GenericListItem> list = DataAccess.DataMerchantAppDao.GetMerchantUWEmailBody(OrigApp.MerchantAppUID, "SS");

                if (list.Count > 0)
                    chkAllInfo = Convert.ToBoolean(list[0].ItemValue);

                if (!chkAllInfo)
                {
                    message.Add("Application has conditions. Please clear all pending conditions before changing the status.");
                }
            }


            //This Check is when moving from OP to MS Received or DP statuses and when the Brand is Optimal/Meritus.
            // You can change to MS Received or DP statuses when the brand is Optimal.
            // You can only change to DP status when brand is Meritus.
            if (Statusuid != OrigStatus)
            {
                bool isDP = (Statusuid == Constants.QUEUESTATUS_DP_RECEIVED_HARDWARE || Statusuid == Constants.QUEUESTATUS_DP_RECEIVED_SOFTWARE);

                if (((Statusuid == Constants.QUEUESTATUS_MS_RECEIVED || isDP) && OrigApp.Brand == MerchantBrand.Optimal)
                    || (isDP && OrigApp.Brand == MerchantBrand.Meritus))
                {
                    Hashtable prms = new Hashtable();

                    DataSet ds = new DataSet();

                    if (isAchOnly)
                    {
                        prms.Add("@MerchantID", OrigApp.ID);
                        ds = DataAccess.DataAchMerchantDao.GetAchMerchantStatusHistory(prms);
                    }
                    else
                    {
                        prms.Add("@MerchantAppUID", OrigApp.MerchantAppUID);
                        ds = DataAccess.DataMerchantAppDao.GetMerchantStatusHistory(prms);
                    }

                    if (ds.Tables[0].AsEnumerable().Where(c => ((string)c["Status"]).Trim().Equals("OP - QA")).Count() == 0)
                    {
                        message.Add("This application has not completed QA and can not be moved to Deployment/Merchant Support.");
                    }
                }

                //PXP-6524 Rohit Thakur
                //code changes for PXP-10225 by koshlendra
                if ((UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS)
                    && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine) && Statusuid.ToUpper().Equals(Constants.QUEUESTATUS_CU_RECEIVED) && !UserSessions.CurrentMerchantApp.StatusName.StartsWith("MS"))
                {
                    if (string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.WFReturnPoliciesUID) || UserSessions.CurrentMerchantApp.WFReturnPoliciesUID == "-1")
                    {
                        if (!message.Contains("Please answer: Do you have a refund policy for Visa/MC/Discover/Amex."))
                            message.Add("Please answer: Do you have a refund policy for Visa/MC/Discover/Amex.");
                    }
                    if (string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.ReturnCancelPolicy))
                    {
                        if (!message.Contains("Please answer: What is your return, cancellation or refund policy."))
                            message.Add("Please answer: What is your return, cancellation or refund policy.");
                    }
                    if (string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.RefundPolicyAwareness) || UserSessions.CurrentMerchantApp.RefundPolicyAwareness == "-1")
                    {
                        if (!message.Contains("Please answer: Is the refund policy in writing and obvious to the cardholder/customer."))
                            message.Add("Please answer: Is the refund policy in writing and obvious to the cardholder/customer.");
                    }
                    if (UserSessions.CurrentMerchantApp.RefundPolicyAwareness.Equals("0"))
                    {
                        if (string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.RefundPolicyAwarenessReason))
                        {
                            if (!message.Contains("Please answer: Explain, if No selected for is the refund policy in writing and obvious to the cardholder/customer."))
                                message.Add("Please answer: Explain, if No selected for is the refund policy in writing and obvious to the cardholder/customer.");
                        }
                    }
                    //code changes done for PXP-10225
                    if (UserSessions.CurrentMerchantApp != null &&
                        (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST ||
                        UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS ||
                        UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS)
                        && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
                    {
                        IList<GenericListItem> listPurchaseProducts = LookupTableHandler.GetPurchaseProducts(UserSessions.CurrentMerchantApp.MerchantAppUID);
                        if (listPurchaseProducts != null)
                        {
                            bool isAnyItemSelected = false;
                            foreach (GenericListItem item in listPurchaseProducts)
                            {
                                if (item.Selected) isAnyItemSelected = true;
                            }
                            if (!isAnyItemSelected)
                            {
                                if (!message.Contains("Please answer: How does the customer purchase/order the product."))
                                    message.Add("Please answer: How does the customer purchase/order the product.");

                            }
                        }
                    }
                    if ((UserSessions.CurrentMerchantApp.Delivery07 == 0) && (UserSessions.CurrentMerchantApp.Delivery08 == 0)
                        && (UserSessions.CurrentMerchantApp.Delivery15 == 0) && (UserSessions.CurrentMerchantApp.Delivery30 == 0))
                    {
                        if (!message.Contains("Please answer: What is the delivery time frame of the product/service to the customer."))
                            message.Add("Please answer: What is the delivery time frame of the product/service to the customer.");
                    }
                    if (UserSessions.CurrentMerchantApp.Deposit_FutureServices == 0 && UserSessions.CurrentMerchantApp.Cash_Carry == 0)
                    {
                        if (!message.Contains("Please answer: What percentage of your business is."))
                            message.Add("Please answer: What percentage of your business is.");
                    }
                    if (string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.GeographicAreas))
                    {
                        if (!message.Contains("Please answer: In what geographic areas will the product(s) be marketed and sold."))
                            message.Add("Please answer: In what geographic areas will the product(s) be marketed and sold.");
                    }
                }


            }

            string strPage = System.IO.Path.GetFileName(HttpContext.Current.Request.Path).Replace(".aspx", "").ToUpper();

            if (strPage != "FRMUNDERWRITING")
            {

                DataUnderwritng data = DataAccess.DataUnderwritingDao;
                Underwriting objUW = DataAccess.DataUnderwritingDao.LoadMerchantUWNotes(OrigApp.MerchantAppUID);
                UWFulfillment uwfulfillment = data.GetUWFulfillment(UserSessions.CurrentMerchantApp.ID);
                UFAMVData uwAMVData = data.GetAMVData(UserSessions.CurrentMerchantApp.ID);


                //TODO change after confirming what to use 
                if ((uwAMVData.TotalApprovedVolume) >= 100000)
                {
                    if (OrigApp.IsRolloverAccount != true && OrigApp.Office != CommonUtility.Util.Offices.LosAngeles && OrigApp.Office != CommonUtility.Util.Offices.Dallas)
                    {
                        if (OrigStatus.ToUpper() != Constants.QUEUESTATUS_MS_ACTIVE && OrigStatus.ToUpper() != Constants.QUEUESTATUS_MS_INACTIVE && OrigStatus.ToUpper() != Constants.QUEUESTATUS_MS_CANCELLATION && OrigStatus.ToUpper() != Constants.QUEUESTATUS_MS_PENDING_CANCELLATION)
                        {
                            //PXP-3235
                            if (Statusuid.ToUpper() == Constants.QUEUESTATUS_MS_ACTIVE || Statusuid.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED)
                            {
                                if (uwfulfillment.Period1Volume == 0)
                                {
                                    message.Add("Please enter Period1 Volume.");

                                    if (uwfulfillment.Period1NDXDays == 0)
                                    {
                                        message.Add("Please enter Period1 NDXDays.");
                                    }

                                }
                                else if (uwfulfillment.Period1NDXDays == 0)
                                {
                                    message.Add("Please enter Period1 NDXDays.");
                                }
                                if (uwfulfillment.Period2Volume != 0)
                                {

                                    if (uwfulfillment.Period2NDXDays == 0)
                                    {
                                        message.Add("Please enter Period2 NDXDays.");
                                    }

                                }

                                if (uwfulfillment.Period3Volume != 0)
                                {
                                    if (uwfulfillment.Period3NDXDays == 0)
                                    {
                                        message.Add("Please enter Period3 NDXDays.");
                                    }

                                }

                                if (uwfulfillment.RiskExposure == 0)
                                {
                                    message.Add("Please enter Risk Exposure.");
                                }

                                if (uwfulfillment.RefundDays == 0)
                                {
                                    message.Add("Please enter Refund Days.");
                                }

                            }
                        }
                    }

                }


                if (Statusuid == Constants.QUEUESTATUS_CU_APPROVED && Statusuid != OrigStatus)
                {
                    if (!isAchOnly)
                    {
                        if (OrigApp.SicCode == "5960" || OrigApp.SicCode == "5962" || OrigApp.SicCode == "5964" || OrigApp.SicCode == "5965" ||
                            OrigApp.SicCode == "5966" || OrigApp.SicCode == "5967" || OrigApp.SicCode == "5968" || OrigApp.SicCode == "5969")
                        {
                            if (objUW == null || string.IsNullOrWhiteSpace(objUW.HighRiskDescriptor))
                                message.Add("Please add High Risk Descriptor on credit page.");
                        }
                    }

                    DataSet ds = DataAccess.DataUnderwritingDao.GetUWCheckList(OrigApp.MerchantAppUID);
                    bool istrue = true;

                    if (!isAchOnly && (objUW == null || string.IsNullOrWhiteSpace(objUW.AgentLevel.Replace("-1", ""))))
                    {
                        if (bank.ToUpper() == Constants.BANK_BMO_HARRIS)
                        {
                            message.Add("Please select Association Number.");
                        }
                        else
                        {
                            message.Add("Please select Agent level.");
                        }
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (OrigApp.Owners.Count > i)
                        {
                            if (!string.IsNullOrWhiteSpace(OrigApp.Owners[i].FullName) || !string.IsNullOrWhiteSpace(OrigApp.Owners[i].SSN.Replace("-", "").Replace("-", "")))
                            {
                                if (OrigApp.Owners[i].NameAddressPhoneSummary == "-1" && !string.IsNullOrWhiteSpace(OrigApp.Owners[i].NameAddressPhoneSummary))
                                    message.Add("Please select a guaranty for Owner" + (i + 1).ToString() + ".");
                            }
                        }
                    }

                    if (OrigApp.ReservePercent > 0 && (OrigApp.ReleaseMethodUID == "-1" || string.IsNullOrWhiteSpace(OrigApp.ReleaseMethodUID)))
                        message.Add("Please select release method.");

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (DataLayer.Field2Bool(row.ItemArray[3]) == false)
                            istrue = false;
                    }

                    if (!istrue)
                        message.Add("Review all the checklist items to approve the account.");


                    //This Check was happening only for Meritus Earlier, Which is no more required.
                    decimal vol = OrigApp.TinfoAverageMonthlyVMCVolume;
                    UWHeirarchyApprovalLimit CurrentHeirarchyapproval = LookupTableHandler.LoadUWHeirarchyApprovalLimit().Where(hm => hm.AMVLowerLimit < vol && hm.AMVUpperLimit > vol && hm.IsMCCRestrictedIndustry == IsMCCRestricted(OrigApp.SicCode)).FirstOrDefault();
                    if (CurrentHeirarchyapproval != null)
                    {
                        if (objUW != null && string.IsNullOrWhiteSpace(objUW.RiskApproval) && string.IsNullOrWhiteSpace(objUW.ExecutiveApproval))
                            message.Add("Monthly volume over limit. Please request approval for this account.");
                    }


                    if (OrigApp.Brand == MerchantBrand.Optimal && OrigApp.DaysHoldTypeID == 0)
                    {
                        message.Add("Please select Days Hold type for this account.");
                    }

                }
            }






            //PXP-3957 Rohit Thakur
            if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING))
            {
                if (UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING].Enabled.Equals(true))
                {
                    if (UserSessions.CurrentMerchantApp != null)
                    {
                        if (OrigApp.StatusName.ToUpper().Contains("CU") && !OrigStatus.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED) && UserSessions.CurrentMerchantApp.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))
                        {
                            //Code added by koshlendra for PXP-4979 start
                            if (OrigApp.Office == CommonUtility.Util.Offices.Irvine && (OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
                                || OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS || OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS))
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    if (OrigApp.Owners.Count > i)
                                    {
                                        if ((!string.IsNullOrEmpty(OrigApp.Owners[i].FullName) || !string.IsNullOrEmpty(OrigApp.Owners[i].SSN)) && (!OrigApp.Owners[i].BeneficialOwner && !OrigApp.Owners[i].AuthorizedSignature))
                                        {

                                            message.Add(Constant.BenefeciaryOwnerErrorMsgForWoodForest + (i + 1).ToString() + ".");

                                        }
                                        if ((!string.IsNullOrEmpty(OrigApp.Owners[i].FullName) || !string.IsNullOrEmpty(OrigApp.Owners[i].SSN)))
                                        {
                                            //For pxp-6736 - Added by abarua
                                            if (strPage != "FRMUNDERWRITING" && i < 1)
                                            {
                                                //For pxp-6935 - Added by abarua
                                                if (!FormHandler.IsAuthorizedSignatureChecked(OrigApp) && !message.Contains(Constant.AuthorizedSignatureErrorMsg))
                                                    message.Add(Constant.AuthorizedSignatureErrorMsg);
                                                else
                                                {
                                                    if (message.Contains(Constant.AuthorizedSignatureErrorMsg))
                                                        message.Remove(Constant.AuthorizedSignatureErrorMsg);
                                                }

                                                if (!FormHandler.IsBeneficialOwnerChecked(OrigApp) && !message.Contains(Constant.BeneficialOwnerErrorMsg) && (OrigApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_SOLE_PROPRIETORSHIP || OrigApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PRIVATE_CORPORATION || OrigApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PARTNERSHIP || OrigApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_LIMITED_LIABILITY_COMPANY))
                                                    message.Add(Constant.BeneficialOwnerErrorMsg);
                                                else
                                                {
                                                    if (message.Contains(Constant.BeneficialOwnerErrorMsg))
                                                        message.Remove(Constant.BeneficialOwnerErrorMsg);
                                                }

                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                //Code added by koshlendra for PXP-4979 end
                                if (!FormHandler.IsBenfOwnerOrAuthSignChecked(OrigApp))
                                {
                                    message.Add(Constant.BenefeciaryOwnerErrorMsg);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (UserSessions.ActiveAchMerchant != null)
                        {
                            if (OrigApp.ACHStatus.ToUpper().Contains("CU") && !OrigStatus.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED) && UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))
                            {
                                //Code added by koshlendra for PXP-4979 start
                                //code changes done for PXP-10232 by koshlendra
                                if (OrigApp.Office == CommonUtility.Util.Offices.Irvine && (OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
                                    || OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS || OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS))
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        if (OrigApp.Owners.Count > i)
                                        {
                                            if ((!string.IsNullOrEmpty(OrigApp.Owners[i].FullName) || !string.IsNullOrEmpty(OrigApp.Owners[i].SSN)) && (!OrigApp.Owners[i].BeneficialOwner && !OrigApp.Owners[i].AuthorizedSignature))
                                            {

                                                message.Add(Constant.BenefeciaryOwnerErrorMsgForWoodForest + (i + 1).ToString() + ".");

                                            }
                                            if ((!string.IsNullOrEmpty(OrigApp.Owners[i].FullName) || !string.IsNullOrEmpty(OrigApp.Owners[i].SSN)))
                                            {
                                                //For pxp-6736 - Added by abarua
                                                if (strPage != "FRMUNDERWRITING" && i < 1)
                                                {
                                                    //For pxp-6935 - Added by abarua
                                                    if (!FormHandler.IsAuthorizedSignatureChecked(OrigApp) && !message.Contains(Constant.AuthorizedSignatureErrorMsg))
                                                        message.Add(Constant.AuthorizedSignatureErrorMsg);
                                                    else
                                                    {
                                                        if (message.Contains(Constant.AuthorizedSignatureErrorMsg))
                                                            message.Remove(Constant.AuthorizedSignatureErrorMsg);
                                                    }

                                                    if (!FormHandler.IsBeneficialOwnerChecked(OrigApp) && !message.Contains(Constant.BeneficialOwnerErrorMsg) && (OrigApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_SOLE_PROPRIETORSHIP || OrigApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PRIVATE_CORPORATION || OrigApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PARTNERSHIP || OrigApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_LIMITED_LIABILITY_COMPANY))
                                                        message.Add(Constant.BeneficialOwnerErrorMsg);
                                                    else
                                                    {
                                                        if (message.Contains(Constant.BeneficialOwnerErrorMsg))
                                                            message.Remove(Constant.BeneficialOwnerErrorMsg);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    //Code added by koshlendra for PXP-4979 end
                                    if (!FormHandler.IsBenfOwnerOrAuthSignChecked(OrigApp))
                                    {
                                        message.Add(Constant.BenefeciaryOwnerErrorMsg);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //PXP-4214 RThakur Dont validate for Owners page
            StackTrace stackTrace = new StackTrace();
            Console.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);
            string callingClassName = stackTrace.GetFrame(1).GetMethod().ReflectedType.Name;
            if (Statusuid.ToUpper().Equals(Constants.QUEUESTATUS_CU_RECEIVED))
            {
                IList<string> ownersValidationMsg = ValidateOwnersSequence(OrigApp);
                foreach (string errMsg in ownersValidationMsg)
                {
                    message.Add(errMsg);
                }
            }
            if (isAchExist)
            {
                if (achMerchant != null && achMerchant.AchMerchantClone != null
                    && NewStatusUID == Constants.QUEUESTATUS_CU_RECEIVED)
                {
                    IList<string> ownersValidationMsg = ValidateOwnersSequence(OrigApp);
                    foreach (string errMsg in ownersValidationMsg)
                    {
                        if (!message.Contains(errMsg))
                            message.Add(errMsg);
                    }
                }
            }
            //Code updated by Koshlendra for PXP-8490 
            if ((OrigApp.Office == CommonUtility.Util.Offices.Irvine) && (Statusuid.ToUpper() == Constants.QUEUESTATUS_CU_RECEIVED) && (bank != Constants.BANK_ACH_ONLY) && Statusuid != OrigStatus)
            {
                //int percentage3DE = 100 / Convert.ToInt32(ConfigurationManager.AppSettings["Percentage3de"]);
                bool isConfiguredAgentID = GetConfiguredAgentID(OrigApp.AgentID.ToString());
                //CommonUtility.Logger.LogInfo("AgentID specific App. counter: " + counter);
                if (ConfigurationManager.AppSettings["EnableAuto3de"].ToUpper().Equals("TRUE") || (ConfigurationManager.AppSettings["Graduated3de"].ToUpper().Equals("TRUE") && isConfiguredAgentID))  //|| (counter == percentage3DE)
                {
                    IList<string> msg = Validate3DEData(OrigApp);

                    if (msg.Count > 0)
                        LookupTableHandler.m_AgentIDSpecificAppCounter -= 1;
                    foreach (string errMsg in msg)
                    {
                        if (!message.Contains(errMsg))
                            message.Add(errMsg);
                    }

                }
            }
            // Business Information-MLE Validation on all page(Profile,ACH/DD,Credit,Condition and Deployment while Status or AchStatus Changes from SS status to CU-Recieved)
            bool fromACHStatus = false;
            if (UserSessions.ActiveAchMerchant != null)
                fromACHStatus = String.IsNullOrEmpty(StatusName) ? false : UserSessions.ActiveAchMerchant.MerchantStatusName.ToUpper().Substring(0, 2).Equals(Constants.QueueSS_Status);
            if (Statusuid != OrigStatus)
            {
                if ((fromStatus && Statusuid == Constants.QUEUESTATUS_CU_RECEIVED) || (fromACHStatus && NewStatusUID == Constants.QUEUESTATUS_CU_RECEIVED))
                {
                    if (string.IsNullOrWhiteSpace(OrigApp.BusinessLegalName))
                        message.Add(Constants.GetDescription(Constants.ErrorCodes.Profile_BusinessLegalName));
                }
            }

            //Niranjan: PXP-2773 Require ACH/DD Bank to be selected by Credit Underwriting Agent
            if (isAchExist)
            {
                //Chandra: bug fix, pxp-2924
                if (achMerchant != null && achMerchant.AchMerchantClone != null
                    && achMerchant.AchMerchantClone.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED
                    && achMerchant.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED)
                {

                    if (achMerchant.AchDescrp.Trim().Length == 0)
                        message.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_Descriptor));

                    //by Chandra for PXP-10595
                    if (achMerchant.AchDiscrtn.Trim().Length == 0 && UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus)
                        message.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_Discretionary));


                    if (achMerchant.AccountType == "-1")
                        message.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_Acctype));
                    //Niranjan: PXP-3059 Remove PXP-2773 for non-Irvine locations
                    if (OrigApp.Brand == MerchantBrand.Meritus)
                    {
                        if (achMerchant.BankID == -1)
                            message.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_Bank));

                        if (!((DataLayer.IsNumeric(achMerchant.NAICS)) && (achMerchant.NAICS.Length == 6)))
                            message.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_NAICS));
                    }
                }
            }
        }
        //PXP:13612 ANI: Start Need to remove this validation.
        // PXP-9859
        //by Chandra for PXP-10595
        //if (Statusuid == Constants.QUEUESTATUS_OP_RECEIVED
        //    && Statusuid != OrigStatus
        //    && OrigApp.IsNutraMerchant == true
        //    && (OrigApp.SicCode.Equals(Constants.NUTRA_MCC[0]) || OrigApp.SicCode.Equals(Constants.NUTRA_MCC[1]))
        //    && OrigApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
        //{
        //    //PXP-10620:Sanidhya
        //    if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS)
        //    {
        //        if (OrigApp.HighRiskRegistered == false && UserSessions.CurrentMerchantApp.MasterCard)
        //        {
        //            message.Add("HR Registered is not checked for Nutra Free Trial account");
        //        }
        //    }
        //    else
        //    {
        //        if (OrigApp.HighRiskRegistered == false)
        //        {
        //            message.Add("HR Registered is not checked for Nutra Free Trial account");
        //        }
        //    }
        //}
        //PXP:13612 ANI: END
        return message;
    }
    /// <summary>
    /// PXP-8409:Validating the CRM Flow by Sanidhya
    /// </summary>
    /// <param name="origApp"></param>
    /// <param name="NewStatusUID"></param>
    /// <param name="achMerchant"></param>
    /// <returns></returns>
    public static IList<string> ValidateCRMFlow(MerchantApp OrigApp, bool isAdding, AchMerchant achMerchant)
    {
        IList<string> _infoMsg = new List<string>();
        string Statusuid = string.Empty;
        string bank = string.Empty;
        string OrigStatus = string.Empty;

        //get all the UID vlaues into variables
        bank = OrigApp.MerchantAppTypeUID.Replace("-1", string.Empty).ToUpper();

        //check to see if the account is cc only
        bool isAchOnly = !isAdding && (bank == Constants.BANK_ACH_ONLY && OrigApp.AchID > 0);
        if (!isAchOnly)
        {
            Statusuid = OrigApp.StatusUID.ToUpper();
            if (OrigApp.MerchantAppClone != null)
            {
                OrigStatus = OrigApp.MerchantAppClone.StatusUID.ToUpper();

            }
            else
            {
                OrigStatus = OrigApp.StatusUID.ToUpper();
            }
        }
        //Validating CRM flow for Merchant type cc and Nutra trial
        if (!isAchOnly)
        {
            if ((OrigStatus.ToUpper() == Constants.QUEUESTATUS_SS_RECEIVED.ToUpper() || OrigStatus.ToUpper() == Constants.QUEUESTATUS_SS_DRAFT.ToUpper()
                || OrigStatus.ToUpper() == Constants.QUEUESTATUS_SS_WITHDRAWN.ToUpper() || OrigStatus.ToUpper() == Constants.QUEUESTATUS_SS_APP_INCOMPLETE.ToUpper())
                && Statusuid.ToUpper() == Constants.QUEUESTATUS_CU_RECEIVED.ToUpper() && OrigApp.Office == CommonUtility.Util.Offices.Irvine
                && (OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || OrigApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS))
            {
                if (OrigApp.IsNutraMerchant && OrigApp.SicCode == "5968")    //PXP-9348
                {
                    if (OrigApp.CRMCount == 1)
                    {
                        if (OrigApp.CRMStatus == "InActive")
                        {
                            _infoMsg.Add("CRM TPP Registration is InActive.");

                        }
                        else if (OrigApp.CRMStatus == "NA" && !OrigApp.CRMAcceptTransactions)
                        {
                            _infoMsg.Add("CRM TPP Registration is NA.");
                        }
                    }
                }
            }

        }
        return _infoMsg;

    }
    private static IList<string> Validate3DEData(MerchantApp OrigApp)
    {
        Validate3DEVendorsData validate3DEVendorsData = new Validate3DEVendorsData();
        return validate3DEVendorsData.ValidateZeus3DEData(OrigApp);
    }

    /// <summary> PXP-4113 RThakur 
    /// This method validates the sequence in which the owners are entered.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    private static IList<string> ValidateOwnersSequence(MerchantApp app)
    {
        IList<string> validationMsgs = new List<string>();
        int prev_OwnerCount = 0;    //As first owner will always get validated so start validation for previous owners after the first owner

        if (app.Owners.Count > 0)
        {
            for (int ownerCount = 0; ownerCount < app.Owners.Count; ownerCount++)
            {
                if ((!string.IsNullOrWhiteSpace(app.Owners[ownerCount].FirstName)) || (!string.IsNullOrWhiteSpace(app.Owners[ownerCount].LastName)))
                {
                    for (int prevOwnerCount = prev_OwnerCount; prevOwnerCount < ownerCount; prevOwnerCount++)//Validate owners in sequence before this owner if they are not filled.
                    {
                        if ((string.IsNullOrWhiteSpace(app.Owners[prevOwnerCount].FirstName)) || (string.IsNullOrWhiteSpace(app.Owners[prevOwnerCount].LastName)))
                        {
                            ValidateOwnersData(app, ref validationMsgs, prevOwnerCount);
                        }
                    }
                    prev_OwnerCount = ownerCount;
                }
            }
        }
        return validationMsgs;
    }

    /// <summary> PXP-4113 RThakur 
    /// This method validates the Owners data.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="validationMsgs"></param>
    /// <param name="ownerCount"></param>
    internal static void ValidateOwnersData(MerchantApp app, ref IList<string> validationMsgs, int ownerCount)
    {
        string msg = string.Empty;
        if (string.IsNullOrWhiteSpace(app.Owners[ownerCount].FirstName) || string.IsNullOrWhiteSpace(app.Owners[ownerCount].LastName))
        {
            msg = Constants.GetDescription(Constants.ErrorCodes.Owner_SequentialOrder);
            if (!string.IsNullOrEmpty(msg) && !validationMsgs.Contains(msg))
                validationMsgs.Add(msg);
        }
    }

    #endregion

    #region UNDERWRITING, UNDERWRITING PENDING, DEPLOYMENT, PROFILE, FEES, OWNERS, ACH, AgentInfo, AgentSplits, FirstTeam, OWNERS Pages
    public static void LogFormChanges(string DBA, string uid, int id, object original, object current, string objectName = null)
    {
        if (original == null || current == null)
            return;

        Type type = original.GetType();
        PropertyInfo[] props = type.GetProperties();


        Type type2 = current.GetType();
        PropertyInfo[] props2 = type2.GetProperties();
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < props.Length; i++)
        {

            if (props[i].GetCustomAttributes(typeof(DonotLogParameter), true).Length == 0)
            {
                object propValue1 = props[i].GetValue(original, null);
                object propValue2 = props2[i].GetValue(current, null);
                string s1 = string.Empty;
                string s2 = string.Empty;


                if (props[i].PropertyType == typeof(string))
                {
                    s1 = propValue1 == null ? string.Empty : propValue1.ToString().Trim();
                    s2 = propValue2 == null ? string.Empty : propValue2.ToString().Trim();

                    if (props[i].Name.ToUpper().IndexOf("UID") != -1 && s2 == "-1")
                        s2 = string.Empty;
                }

                if (props[i].PropertyType == typeof(decimal))
                {
                    s1 = propValue1 == null ? string.Empty : Convert.ToDecimal(propValue1).ToString("###,##0.00");
                    s2 = propValue2 == null ? string.Empty : Convert.ToDecimal(propValue2).ToString("###,##0.00");
                }

                if (props[i].PropertyType == typeof(int))
                {
                    s1 = propValue1 == null ? string.Empty : Convert.ToInt32(propValue1).ToString();
                    s2 = propValue2 == null ? string.Empty : Convert.ToInt32(propValue2).ToString();
                }

                if (props[i].PropertyType == typeof(bool))
                {
                    s1 = propValue1 == null ? string.Empty : Convert.ToBoolean(propValue1).ToString();
                    s2 = propValue2 == null ? string.Empty : Convert.ToBoolean(propValue2).ToString();
                }

                if (props[i].PropertyType.IsEnum)
                {
                    s1 = propValue1 == null ? string.Empty : Convert.ToString(propValue1).ToString();
                    s2 = propValue2 == null ? string.Empty : Convert.ToString(propValue2).ToString();
                }

                if (s1 != s2)
                {
                    sb.Append(props[i].Name + ": " + s1 + ", " + s2 + " | ");
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(sb.ToString()))
        {
            DataUser data = DataAccess.DataUserDao;
            data.InsertChangeLog(DBA, UserSessions.CurrentUser.UserName, uid, id, objectName ?? type.Name, sb.ToString(), Constants.PORTAL_ZEUS);
        }
    }
    #endregion

    #endregion

    #region  ASPX Pages & ASCX User Controls
    /// <summary>
    /// ASPX Pages & ASCX User Controls
    /// </summary>
    /// 
    /// <ASPX Page>
    /// frmAddUser, frmAgentInfo, frmAgentSplits, frmCommunicationsDetail, frmLeadsDetails, frmDeployment, frmFraudXP, frmMerchantACH, frmMerchantCategories,
    /// frmMerchantFees, frmMerchantFirstTeam, frmMerchantInvoicing, frmMerchantOwners, frmMerchantPCI, frmMerchantProducts, frmMerchantProfile, frmMerchantRisk,
    /// frmUnderwriting, frmUnderwritingPending, frmCashAdvance, frmEmailBlaster, frmETF
    /// 
    /// <ASCX User Controls>
    /// wucFTRuleFilter,NewApp, wucDiversionDialog, wucReleaseDialog, wucDocumentGrid, wucEquipment, wucLeadInfo, wucMeritusAlertsDetails, wucMeritusNewsDetails
    /// wucTicket, wucTicketTemplate, wucUserProfile, wucUWFinancialScoreCardGrid, wucComplianceEdit
    /// <param name="container"></param>
    /// <param name="EditMode"></param>
    public static void SetControlEditMode(Control container, bool EditMode)
    {
        foreach (Control control in container.Controls)
        {
            if (control != null)
            {
                if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;
                    listControl.Enabled = EditMode;
                }
                //else if (control is Label)
                //{
                //    Label txt = (Label)control;
                //    txt.Enabled = EditMode;
                //}
                else if (control is TextBox)
                {
                    TextBox txt = (TextBox)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is CheckBox)
                {
                    CheckBox chk = (CheckBox)control;
                    chk.Enabled = EditMode;
                }
                else if (control is Button)
                {
                    Button btn = (Button)control;
                    btn.Enabled = EditMode;
                }
                else if (control is WebTextEditor)
                {
                    WebTextEditor txt = (WebTextEditor)control;
                    txt.ReadOnly = !EditMode;
                    //txt.Enabled = EditMode;
                }
                else if (control is WebMaskEditor)
                {
                    WebMaskEditor txt = (WebMaskEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebDateTimeEditor)
                {
                    WebDateTimeEditor txt = (WebDateTimeEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebDateChooser)
                {
                    WebDateChooser txt = (WebDateChooser)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebNumericEditor)
                {
                    WebNumericEditor txt = (WebNumericEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebCurrencyEditor)
                {
                    WebCurrencyEditor txt = (WebCurrencyEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebPercentEditor)
                {
                    WebPercentEditor txt = (WebPercentEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebHtmlEditor)
                {
                    WebHtmlEditor txt = (WebHtmlEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is FileUpload)
                {
                    FileUpload txt = (FileUpload)control;
                    txt.Enabled = EditMode;
                }
                else if (control is GridView)
                {
                    GridView grd = (GridView)control;
                    grd.Enabled = EditMode;
                }
                if (control.HasControls())
                {
                    SetControlEditMode(control, EditMode);
                }
            }
        }
    }
    public static void SetControlEditMode(Control container, string [] listControls, bool EditMode)
    {
        foreach(string controlName in listControls)
        {
            Control control = FindFormControl(container, controlName);

            if (control != null)
            {
                if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;
                    listControl.Enabled = EditMode;
                }
                //else if (control is Label)
                //{
                //    Label txt = (Label)control;
                //    txt.Enabled = EditMode;
                //}
                else if (control is TextBox)
                {
                    TextBox txt = (TextBox)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is CheckBox)
                {
                    CheckBox chk = (CheckBox)control;
                    chk.Enabled = EditMode;
                }
                else if (control is Button)
                {
                    Button btn = (Button)control;
                    btn.Enabled = EditMode;
                }
                else if (control is WebTextEditor)
                {
                    WebTextEditor txt = (WebTextEditor)control;
                    txt.ReadOnly = !EditMode;
                    //txt.Enabled = EditMode;
                }
                else if (control is WebMaskEditor)
                {
                    WebMaskEditor txt = (WebMaskEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebDateTimeEditor)
                {
                    WebDateTimeEditor txt = (WebDateTimeEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebDateChooser)
                {
                    WebDateChooser txt = (WebDateChooser)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebNumericEditor)
                {
                    WebNumericEditor txt = (WebNumericEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebCurrencyEditor)
                {
                    WebCurrencyEditor txt = (WebCurrencyEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebPercentEditor)
                {
                    WebPercentEditor txt = (WebPercentEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebHtmlEditor)
                {
                    WebHtmlEditor txt = (WebHtmlEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is FileUpload)
                {
                    FileUpload txt = (FileUpload)control;
                    txt.Enabled = EditMode;
                }
                else if (control is GridView)
                {
                    GridView grd = (GridView)control;
                    grd.Enabled = EditMode;
                }
                if (control.HasControls())
                {
                    SetControlEditMode(control, EditMode);
                }
            }
        }
    }
    public static void ClearAllControls(Control container)
    {
        foreach (Control control in container.Controls)
        {
            if (control != null)
            {
                if (control is BulletedList)
                {
                    BulletedList bulletList = (BulletedList)control;
                    bulletList.Items.Clear();
                }
                else if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;
                    listControl.SelectedIndex = -1;
                }
                else if (control is TextBox)
                {
                    TextBox txt = (TextBox)control;
                    txt.Text = string.Empty;
                }
                else if (control is CheckBox)
                {
                    CheckBox chk = (CheckBox)control;
                    chk.Checked = false;
                }
                else if (control is HiddenField)
                {
                    HiddenField txt = (HiddenField)control;
                    txt.Value = string.Empty;
                }

                else if (control is WebMaskEditor)
                {
                    WebMaskEditor txt = (WebMaskEditor)control;
                    txt.Value = null;
                }
                else if (control is WebDateTimeEditor)
                {
                    WebDateTimeEditor txt = (WebDateTimeEditor)control;
                    txt.Value = null;
                }
                else if (control is WebDateChooser)
                {
                    WebDateChooser txt = (WebDateChooser)control;
                    txt.Value = null;
                }
                else if (control is WebNumericEditor)
                {
                    WebNumericEditor txt = (WebNumericEditor)control;
                    txt.Value = null;
                }
                else if (control is WebCurrencyEditor)
                {
                    WebCurrencyEditor txt = (WebCurrencyEditor)control;
                    txt.Value = null;
                }
                else if (control is WebPercentEditor)
                {
                    WebPercentEditor txt = (WebPercentEditor)control;
                    txt.Value = null;
                }
                if (control.HasControls())
                {
                    ClearAllControls(control);
                }
            }
        }
    }

    public static void Export2Excel(string filename, GridView grd)
    {
        GridViewExportUtil.Export(filename, grd);
    }

    public static void ExportToPDF(GridView gvReport, bool LandScape, string ReportName)
    {
        int noOfColumns = 0, noOfRows = 0, noOfVisibleColumns = 0;
        DataTable tbl = null;

        for (int i = 0; i < gvReport.Columns.Count; i++)
        {
            if (gvReport.Columns[i].Visible)
            {
                noOfVisibleColumns += 1;
            }
        }

        if (gvReport.AutoGenerateColumns)
        {
            tbl = gvReport.DataSource as DataTable; // Gets the DataSource of the GridView Control.
            noOfColumns = tbl.Columns.Count;
            noOfRows = tbl.Rows.Count;
        }
        else
        {
            noOfColumns = gvReport.Columns.Count;
            noOfRows = gvReport.Rows.Count;
        }

        float HeaderTextSize = 8;
        float ReportNameSize = 10;
        float ReportTextSize = 8;
        float ApplicationNameSize = 7;

        // Creates a PDF document
        Document document = null;
        if (LandScape == true)
        {
            // Sets the document to A4 size and rotates it so that the orientation of the page is Landscape.
            document = new Document(PageSize.A4.Rotate(), 0, 0, 15, 5);
        }
        else
        {
            document = new Document(PageSize.A4, 0, 0, 15, 5);
        }

        // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
        iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfVisibleColumns);

        // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
        mainTable.HeaderRows = 4;

        // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
        iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(2);

        // Creates a phrase to hold the application name at the left hand side of the header.
        Phrase phApplicationName = new Phrase(ReportName, FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

        // Creates a PdfPCell which accepts a phrase as a parameter.
        PdfPCell clApplicationName = new PdfPCell(phApplicationName);
        // Sets the border of the cell to zero.
        clApplicationName.Border = PdfPCell.NO_BORDER;
        // Sets the Horizontal Alignment of the PdfPCell to left.
        clApplicationName.HorizontalAlignment = Element.ALIGN_LEFT;

        // Creates a phrase to show the current date at the right hand side of the header.
        Phrase phDate = new Phrase(DateTime.Now.Date.ToString("MM/dd/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

        // Creates a PdfPCell which accepts the date phrase as a parameter.
        PdfPCell clDate = new PdfPCell(phDate);
        // Sets the Horizontal Alignment of the PdfPCell to right.
        clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
        // Sets the border of the cell to zero.
        clDate.Border = PdfPCell.NO_BORDER;

        // Adds the cell which holds the application name to the headerTable.
        headerTable.AddCell(clApplicationName);
        // Adds the cell which holds the date to the headerTable.
        headerTable.AddCell(clDate);
        // Sets the border of the headerTable to zero.
        headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

        // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
        PdfPCell cellHeader = new PdfPCell(headerTable);
        cellHeader.Border = PdfPCell.NO_BORDER;
        // Sets the column span of the header cell to noOfColumns.
        cellHeader.Colspan = noOfColumns;
        // Adds the above header cell to the table.
        mainTable.AddCell(cellHeader);

        // Creates a phrase which holds the file name.
        Phrase phHeader = new Phrase(ReportName, FontFactory.GetFont("Arial", ReportNameSize, iTextSharp.text.Font.BOLD));
        PdfPCell clHeader = new PdfPCell(phHeader);
        clHeader.Colspan = noOfColumns;
        clHeader.Border = PdfPCell.NO_BORDER;
        clHeader.HorizontalAlignment = Element.ALIGN_CENTER;
        mainTable.AddCell(clHeader);

        // Creates a phrase for a new line.
        Phrase phSpace = new Phrase("\n");
        PdfPCell clSpace = new PdfPCell(phSpace);
        clSpace.Border = PdfPCell.NO_BORDER;
        clSpace.Colspan = noOfColumns;
        mainTable.AddCell(clSpace);

        // Sets the gridview column names as table headers.
        for (int i = 0; i < noOfColumns; i++)
        {
            if (gvReport.Columns[i].Visible)
            {
                Phrase ph = null;

                if (gvReport.AutoGenerateColumns)
                {
                    ph = new Phrase(tbl.Columns[i].ColumnName, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                }
                else
                {
                    ph = new Phrase(gvReport.Columns[i].HeaderText, FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                }

                mainTable.AddCell(ph);
            }
        }

        // Reads the gridview rows and adds them to the mainTable
        for (int rowNo = 0; rowNo < noOfRows; rowNo++)
        {
            for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
            {
                if (gvReport.Columns[columnNo].Visible)
                {
                    if (gvReport.AutoGenerateColumns)
                    {
                        string s = gvReport.Rows[rowNo].Cells[columnNo].Text.Trim();
                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                        mainTable.AddCell(ph);
                    }
                    else
                    {
                        if (gvReport.Columns[columnNo] is TemplateField)
                        {
                            foreach (Control ctrl in gvReport.Rows[rowNo].Cells[columnNo].Controls)
                            {
                                if (ctrl is LinkButton)
                                {
                                    LinkButton lc = (LinkButton)ctrl;
                                    string s = lc.Text.Trim();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                    mainTable.AddCell(ph);
                                }

                                if (ctrl is HyperLink)
                                {
                                    HyperLink lc = (HyperLink)ctrl;
                                    string s = lc.Text.Trim();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                    mainTable.AddCell(ph);
                                }

                                if (ctrl is Label)
                                {
                                    Label lc = (Label)ctrl;
                                    string s = lc.Text.Trim();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                    mainTable.AddCell(ph);
                                }
                            }
                        }
                        else
                        {
                            string s = gvReport.Rows[rowNo].Cells[columnNo].Text.Trim();
                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                            mainTable.AddCell(ph);
                        }
                    }
                }
            }

            // Tells the mainTable to complete the row even if any cell is left incomplete.
            // mainTable.CompleteRow();
        }

        // Gets the instance of the document created and writes it to the output stream of the Response object.
        PdfWriter.GetInstance(document, HttpContext.Current.Response.OutputStream);

        // Creates a footer for the PDF document.
        HeaderFooter pdfFooter = new HeaderFooter(new Phrase(), true);
        pdfFooter.Alignment = Element.ALIGN_CENTER;
        pdfFooter.Border = iTextSharp.text.Rectangle.NO_BORDER;

        // Sets the document footer to pdfFooter.
        document.Footer = pdfFooter;
        // Opens the document.
        document.Open();
        // Adds the mainTable to the document.
        document.Add(mainTable);
        // Closes the document.
        document.Close();

        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename= " + ReportName + ".pdf");
        HttpContext.Current.Response.End();
    }

    #endregion

    #region HANDLER Methods

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

    public static string OpenPopUpWindow(string url, WindowProperties properties)
    {
        string pathURL =
            "javascript:var x=window.open('" + url + "', null,'" + properties.ToString() + "');";

        return pathURL;
    }

    public static void OpenModalWindowRefresh(Page page, string url, string btnNameID, int width, int height)
    {
        string script;
        script = "<script type='text/javascript'>";
        script += "OpenModalDialog('" + url + "','" + btnNameID + "','" + width + "','" + height + "');";
        script += "</script>";

        page.ClientScript.RegisterStartupScript(page.GetType(), "OpenModalWindow", script);
    }

    public static void OpenModalWindow(Page page, string url, int width, int height)
    {
        string script;
        script = "<script type='text/javascript'>";
        script += "window.showModalDialog('" + url + "',window,'status:no;scroll:no;center:yes;resizable:no;dialogWidth:" + width.ToString() + "px;dialogHeight:" + height.ToString() + "px');";
        script += "</script>";

        page.ClientScript.RegisterStartupScript(page.GetType(), "OpenModalWindow", script);
    }

    public static string OpenModalWindowScript(string scriptID, string url, int width, int height)
    {
        string script;
        script = "<script language='javascript' type='text/javascript' id='" + scriptID + "'>";
        script += "window.showModalDialog('" + url + "',window,'status:no;scroll:no;center:yes;resizable:no;dialogWidth:" + width.ToString() + "px;dialogHeight:" + height.ToString() + "px');";
        script += "</script>";

        return script;
    }

    public static void CloseModalWindowAndRefresh(ClientScriptManager client)
    {
        string script;
        script = "<script type='text/javascript'>";
        script += "window.returnValue = true;";
        script += "window.close();";
        script += "</script>";

        client.RegisterClientScriptBlock(client.GetType(), "CloseModalWindow", script);
    }

    public static void CloseModalWindowAndRefreshMain(ClientScriptManager client)
    {
        string script;
        script = "<script type='text/javascript'>";
        script += "window.close();";
        script += "var xWin=window.dialogArguments;";
        script += "xWin.location.replace(xWin.location);";
        script += "</script>";

        client.RegisterClientScriptBlock(client.GetType(), "CloseModalWindow", script);
    }

    public static void CloseModalWindow(ClientScriptManager client)
    {
        string script;
        script = "<script type='text/javascript'>";
        script += "window.close();";
        script += "</script>";

        client.RegisterClientScriptBlock(client.GetType(), "CloseModalWindow", script);
    }

    public static void OpenLeadList(Page page, string DBANameClientID, string LeadIDClientID, string url, int width, int height)
    {
        string script;
        script = "<script type='text/javascript'>";
        script += "var str = window.showModalDialog('" + url + "',window,'status:no;scroll:no;center:yes;resizable:no;dialogWidth:" + width.ToString() + "px;dialogHeight:" + height.ToString() + "px');";
        script += "if (str != null)";
        script += "{";
        script += "     document." + page.Form.ClientID + "." + LeadIDClientID + ".value = str.substring(0,36);";
        script += "     document." + page.Form.ClientID + "." + DBANameClientID + ".value = str.substring(36,50);";
        script += "}";
        script += "</script>";

        page.ClientScript.RegisterStartupScript(page.GetType(), "OpenModalWindow", script);
    }

    public static void CloseLeadList(ClientScriptManager client, string ReturnValue)
    {
        string script;
        script = "<script type='text/javascript'>";
        script += "window.close();";
        script += "window.returnValue = '" + ReturnValue + "';";
        script += "</script>";

        client.RegisterClientScriptBlock(client.GetType(), "CloseModalWindow", script);
    }

    public static bool LogEmail(string strSubject, string strBody, string strBodyHTML, string strFrom, string strTo, string strCC, string strBCC, Hashtable attachments, string MerchantAppUID)
    {
        bool perform = false;
        try
        {
            Communication comm = new Communication();
            comm.From = strFrom;
            comm.To = strTo;
            comm.Cc = strCC;
            comm.Bcc = strBCC;
            comm.Subject = strSubject;
            comm.Body = strBody;
            comm.HTMLBody = strBodyHTML;
            comm.MerchantAppUID = MerchantAppUID;
            DataCommunication data = DataAccess.DataCommunicationDao;

            comm.IsEmail = true;

            User user = UserSessions.CurrentUser;
            comm.UserUpdated = user.UserName;
            comm.UserCreated = user.UserName;

            data.InsertCommunication(comm);

            if (comm.CommunicationID != "-1")
            {
                foreach (object a in attachments.Keys)
                {
                    string uid = string.Empty;
                    data.InsertCustomAttachements(uid, comm.CommunicationID, a.ToString(), (byte[])attachments[a]);

                }
            }

            perform = true;
        }
        catch (Exception exc)
        {
            throw exc;
        }

        return perform;
    }

    public static string GetWelcomeEmailPaymentXP(string DBA, string UserName, string Password, string PrivateLabel)
    {
        StringBuilder sb = new StringBuilder();
        string url = "https://www.paymentxp.com";
        string company = "Paysafe Group plc";
        string infoUrl = "<a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>";
        string CSPhone = "1-888-851-7558.";

        switch (PrivateLabel.ToUpper())
        {
            case "350158A1-EF06-4853-844F-AF02BFD70F7C": //emc2billing
                url = "https://avt.emc2billing.com";
                company = "EMC2Billing";
                infoUrl = "<a href='info@emc2billing.com'>info@emc2billing.com</a>";
                CSPhone = "818-704-1900<br> Ex. 707";
                break;
            case "CB6FC9EF-9F16-4F52-BAB5-2B1B22EFBA94": //Debt Pay Gateway
                url = "http://www.depositchecks.com";
                company = "Deby Pay Gateway";
                infoUrl = "<a href='info@debtpaygateway.com'>info@debtpaygateway.com</a>";
                CSPhone = "866-927-7180";
                break;
            case "FB6636ED-3131-43C0-B1F5-8EE03BF67011": //bankcard consultants
                url = "https://www.securedach.com";
                company = "Bank Card Consultants";
                infoUrl = "<a href='cs@bankcardconsultants.com'>cs@bankcardconsultants.com</a>";
                CSPhone = "949-333-1580.";
                break;
            case "2B3DAB92-D0D0-4461-9C21-012224FFC89F": //Avantus Merchant Solutions
                url = "https://www.avantusmerchantsolutions.com";
                company = "Avantus Merchant Solutions";
                infoUrl = "<a href='shawn@oscilent.com'>shawn@oscilent.com</a>";
                CSPhone = "949-252-5522";
                break;
            case "B7A118A0-0C04-4520-A30E-BA510E8CC779": // Guardian Merchant Services
                url = "https://www.guardianmerchantservices.com";
                company = "Guardian Merchant Services";
                infoUrl = "<a href='info@gmsmailbox.com'>info@gmsmailbox.com</a>";
                CSPhone = "800-774-8334";
                break;

        }


        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>" + company + " Virtual Terminal</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing " + company + " and the " + company + " Virtual Terminal.</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><b>Login Information:</b><br>You can access the Virtual Terminal by using the following information. If this is your first time logging in, you will be required to generate a password.</span></p>");

        sb.Append("<p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>DBA: </span>" + DBA + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>LoginURL: </span><a href='" + url + "'>" + url + "</a><br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Login ID: </span>" + UserName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Password: </span>" + "For security, your password will be sent in a separate email." + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at " + infoUrl + ", or call us at " + CSPhone + "</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");

        return sb.ToString();
    }

    public static string GetWelcomeEmailGatewayKey(string DBA, string UserName, string PrivateLabel)
    {
        StringBuilder sb = new StringBuilder();

        //string url = "https://www.paymentxp.com";
        string company = "Paysafe Group plc";
        string CSPhone = "1-888-851-7558.";
        string infoUrl = "<a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>";
        string portal = "PaymentXP";

        switch (PrivateLabel.ToUpper())
        {
            case "350158A1-EF06-4853-844F-AF02BFD70F7C": //emc2billing
                //url = "https://avt.emc2billing.com";
                company = "EMC2Billing";
                infoUrl = "<a href='info@emc2billing.com'>info@emc2billing.com</a>";
                CSPhone = "818-704-1900<br> Ex. 707";
                portal = "EMC2Billing";
                break;
            case "CB6FC9EF-9F16-4F52-BAB5-2B1B22EFBA94": //Debt Pay Gateway
                //url = "https://www.depositchecks.com";
                company = "Deby Pay Gateway";
                infoUrl = "<a href='info@debtpaygateway.com'>info@debtpaygateway.com</a>";
                CSPhone = "866-927-7180";
                portal = "Deposit Checks";
                break;
            case "FB6636ED-3131-43C0-B1F5-8EE03BF67011": //bankcard consultants
                //url = "https://www.securedach.com";
                company = "Bank Card Consultants";
                infoUrl = "<a href='cs@bankcardconsultants.com'>cs@bankcardconsultants.com</a>";
                CSPhone = "949-333-1580.";
                portal = "Secured ACH";
                break;
            case "2B3DAB92-D0D0-4461-9C21-012224FFC89F": //Avantus Merchant Solutions
                //url = "https://www.avantusmerchantsolutions.com";
                company = "Avantus Merchant Solutions";
                infoUrl = "<a href='shawn@oscilent.com'>shawn@oscilent.com</a>";
                CSPhone = "949-252-5522";
                portal = "Avantus";
                break;
            case "B7A118A0-0C04-4520-A30E-BA510E8CC779": // Guardian Merchant Services
                //url = "https://www.guardianmerchantservices.com";
                company = "Guardian Merchant Services";
                infoUrl = "<a href='info@gmsmailbox.com'>info@gmsmailbox.com</a>";
                CSPhone = "800-774-8334";
                portal = "Guardian Merchant Services";
                break;
        }

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>" + company + "</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing " + company + ".</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><b>Login Information:</b><br>You can access the " + portal + " Gateway by using the following information.</span></p>");

        sb.Append("<p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>DBA: </span>" + DBA + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Gateway URL: Please refer to User Interface Guide document.</span><br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Username: </span>" + UserName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>MerchantKey: </span>" + "For security, your password will be sent in a separate email." + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at " + infoUrl + ", or call us at " + CSPhone + "</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");

        return sb.ToString();
    }

    public static string GetDCAWelcomeEmailGatewayKey(string DBA, string UserName, string PrivateLabel)
    {
        StringBuilder sb = new StringBuilder();

        //string url = "https://www.paymentxp.com";
        string company = "Capo Payments";

        switch (PrivateLabel.ToUpper())
        {
            case "350158A1-EF06-4853-844F-AF02BFD70F7C": //emc2billing
                //url = "https://avt.emc2billing.com";
                company = "EMC2Billing";
                break;
            case "FB6636ED-3131-43C0-B1F5-8EE03BF67011": //bankcard consultants
                //url = "https://www.securedach.com";
                company = "Bank Card Consultants";
                break;
            case "CB6FC9EF-9F16-4F52-BAB5-2B1B22EFBA94": //Debt Pay Gateway
                //url = "https://www.paymentxp.com?pl=dpg";
                company = "Deby Pay Gateway";
                break;
            case "2B3DAB92-D0D0-4461-9C21-012224FFC89F": //Avantus Merchant Solutions
                //url = "https://www.avantusmerchantsolutions.com";
                company = "Avantus Merchant Solutions";
                break;
            case "B7A118A0-0C04-4520-A30E-BA510E8CC779": // Guardian Merchant Services
                //url = "https://www.guardianmerchantservices.com";
                company = "Guardian Merchant Services";
                break;
        }

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>" + company + "</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing " + company + ".</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><b>Login Information:</b><br>You can access the PaymentXP Gateway by using the following information.</span></p>");

        sb.Append("<p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>DBA: </span>" + DBA + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Gateway URL: Please refer to User Interface Guide document.</span><br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Username: </span>" + UserName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>MerchantKey: </span>" + "For security, your password will be sent in a separate email." + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at <a href='clientservices@capopayments.com'>clientservices@capopayments.com</a>, or call us at 1-888-866-0611.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");

        return sb.ToString();
    }

    public static string GetWelcomeEmailMerchantWebsite(string DBA, string UserName, string Password, string PrivateLabel)
    {
        StringBuilder sb = new StringBuilder();

        //string url = "https://www.paymentXP.com/merchants";
        string company = "Paysafe Group plc";
        string infoUrl = "<a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>";
        string CSPhone = "1-888-851-7558";

        switch (PrivateLabel.ToUpper())
        {
            case "350158A1-EF06-4853-844F-AF02BFD70F7C": //emc2billing
                //url = "https://avt.emc2billing.com";
                company = "EMC2Billing";
                infoUrl = "<a href='info@emc2billing.com'>info@emc2billing.com</a>";
                CSPhone = "818-704-1900<br> Ex. 707";
                break;
            case "CB6FC9EF-9F16-4F52-BAB5-2B1B22EFBA94": //Debt Pay Gateway
                //url = "https://www.depositchecks.com";
                company = "Deby Pay Gateway";
                infoUrl = "<a href='info@debtpaygateway.com'>info@debtpaygateway.com</a>";
                CSPhone = "866-927-7180";
                break;
            case "FB6636ED-3131-43C0-B1F5-8EE03BF67011": //bankcard consultants
                //url = "https://www.securedach.com";
                company = "Bank Card Consultants";
                infoUrl = "<a href='cs@bankcardconsultants.com'>cs@bankcardconsultants.com</a>";
                CSPhone = "949-333-1580.";
                break;
            case "2B3DAB92-D0D0-4461-9C21-012224FFC89F": //Avantus Merchant Solutions
                //url = "https://www.avantusmerchantsolutions.com";
                company = "Avantus Merchant Solutions";
                infoUrl = "<a href='shawn@oscilent.com'>shawn@oscilent.com</a>";
                CSPhone = "949-252-5522";
                break;
            case "B7A118A0-0C04-4520-A30E-BA510E8CC779": // Guardian Merchant Services
                //url = "https://www.guardianmerchantservices.com";
                company = "Guardian Merchant Services";
                infoUrl = "<a href='info@gmsmailbox.com'>info@gmsmailbox.com</a>";
                CSPhone = "800-774-8334";
                break;
        }

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>" + company + "</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing " + company + ".</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><b>Login Information:</b><br>You can access the Merchant Reporting Website by using the following information. If this is your first time logging in, you will be required to generate a password.</span></p>");

        sb.Append("<p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>DBA: </span>" + DBA + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Website Login URL: </span><a href='https://www.paymentXP.com/merchants'>https://www.paymentXP.com/merchants</a><br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Login ID: </span>" + UserName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Password: </span>" + "For security, your password will be sent in a separate email." + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at " + infoUrl + ", or call us at " + CSPhone + ".</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");

        return sb.ToString();
    }

    public static string GetWelcomeEmailMerchantWebsiteBCC(string DBA, string UserName, string Password)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>Bank Card Consultants</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing Bank Card Consultants.</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><b>Login Information:</b><br>You can access the Merchant Reporting Website by using the following information. If this is your first time logging in, you will be required to generate a password.</span></p>");

        sb.Append("<p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>DBA: </span>" + DBA + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Website Login URL: </span><a href='https://www.paymentXP.com/merchants'>https://www.paymentXP.com/merchants</a><br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Login ID: </span>" + UserName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Password: </span>" + "For security, your password will be sent in a separate email." + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at <a href='cs@bankcardconsultants.com'>cs@bankcardconsultants.com</a>, or call us at 949-333-1580.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");

        return sb.ToString();
    }

    public static string GetWelcomeEmailAgentWebsite(string DBA, string UserName, string Password)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>Paysafe Group plc</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing Paysafe Group plc.</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><b>Login Information:</b><br>You can access the Partner Website by using the following information. If this is your first time logging in, you will be required to generate a password.</span></p>");

        sb.Append("<p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>DBA: </span>" + DBA + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Partner Website Login URL: </span><a href='https://www.paymentXP.com/apex'>https://www.paymentXP.com/apex</a><br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Login ID: </span>" + UserName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Password: </span>" + "For security, your password will be sent in a separate email." + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at <a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>, or call us at 1-888-851-7558.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");

        return sb.ToString();
    }

    public static string GetDCAWelcomeEmailPassword(string Portal, string Password, string PrivateLabel)
    {
        StringBuilder sb = new StringBuilder();
        //string url = "https://www.paymentxp.com";
        string company = "Capo Payments";

        switch (PrivateLabel.ToUpper())
        {
            case "350158A1-EF06-4853-844F-AF02BFD70F7C": //emc2billing
                //url = "https://avt.emc2billing.com";
                company = "EMC2Billing";
                break;
            case "FB6636ED-3131-43C0-B1F5-8EE03BF67011": //bankcard consultants
                //url = "https://www.securedach.com";
                company = "Bank Card Consultants";
                break;
            case "CB6FC9EF-9F16-4F52-BAB5-2B1B22EFBA94": //Debt Pay Gateway
                //url = "https://www.paymentxp.com?pl=dpg";
                company = "Deby Pay Gateway";
                break;
            case "2B3DAB92-D0D0-4461-9C21-012224FFC89F": //Avantus Merchant Solutions
                //url = "https://www.avantusmerchantsolutions.com";
                company = "Avantus Merchant Solutions";
                break;
            case "B7A118A0-0C04-4520-A30E-BA510E8CC779": // Guardian Merchant Services
                //url = "https://www.guardianmerchantservices.com";
                company = "Guardian Merchant Services";
                break;
        }

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>" + company + "</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing " + company + ".</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><b>Login Information:</b><br>You can access the " + Portal + " by using the following information..</span></p>");

        sb.Append("<p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Password: </span>" + Password + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at <a href='clientservices@capopayments.com'>clientservices@capopayments.com</a>, or call us at 1-888-866-0611.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");

        return sb.ToString();
    }

    public static string GetWelcomeEmailMerchantKey(string Portal, string Password)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>Paysafe Group plc</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing Paysafe Group plc.</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><b>Login Information:</b><br>You can access the " + Portal + " by using the following information..</span></p>");

        sb.Append("<p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>MerchantKey: </span>" + Password + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at <a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>, or call us at 1-888-851-7558.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");

        return sb.ToString();
    }

    public static void SendAppApprovedEmail(string MerchantAppUID, string MerchantName, string Address, string Telephone, string MID, string Email)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>Paysafe Group plc</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing Paysafe Group plc</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><br><b><u>Your application is Credit Approved!</u></b></span></p>");

        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Name: </span>" + MerchantName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Address: </span>" + Address + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Telephone: </span>" + Telephone + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Your account has been moved to our Operations Department.  You will be notified when the merchant account is active.  If you have any questions or comments please contact us directly at <a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>, or call us at 888-869-0469.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");
        FormHandler.SendEmail("Application Approved", "", sb.ToString(), Constants.CREDIT_UNDERWRITING_EMAIL, Email, "", ResourceService.AppResources["PaysafeWestProductOwner"], new Hashtable(), MerchantAppUID);
    }

    public static void SendAppInReviewEmail(string MerchantAppUID, string MerchantName, string Address, string Telephone, string MID, string Email)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>Paysafe Group plc</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing Paysafe Group plc</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><br>Application Inreview. Underwriting needs more time to review the account and an update will be forthcoming before 3 p.m. tomorrow.</span></p>");

        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Name: </span>" + MerchantName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Address: </span>" + Address + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Telephone: </span>" + Telephone + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, Merchant Support is our number one priority.  Please let us know if we can be of assistance.  If you have any questions or comments, please contact us directly at <a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>, or call us at 888-869-0469.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");
        FormHandler.SendEmail("Application Inreview.", "", sb.ToString(), Constants.RELATIONSHIP_MANAGEMENT_EMAIL, Email, "", ResourceService.AppResources["PaysafeWestProductOwner"], new Hashtable(), MerchantAppUID);
    }

    public static void SendAppClosedEmail(string MerchantAppUID, string MerchantName, string Address, string Telephone, string MID, string Email)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>Paysafe Group plc</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing Paysafe Group plc</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><br>Your merchant account is Closed.</span></p>");

        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Name: </span>" + MerchantName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Address: </span>" + Address + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Telephone: </span>" + Telephone + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Your merchant account has been closed.  For further information please contact your Partner Relations Department at 888-869-0469.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");
        FormHandler.SendEmail("Your merchant application has been closed.", "", sb.ToString(), Constants.RELATIONSHIP_MANAGEMENT_EMAIL, Email, "", ResourceService.AppResources["PaysafeWestProductOwner"], new Hashtable(), MerchantAppUID);
    }

    public static void SendAppDeclinedEmail(string MerchantAppUID, string MerchantName, string Address, string Telephone, string MID, string Email)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>Paysafe Group plc</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing Paysafe Group plc</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><br>Application Declined. Unfortunately, we are unable to approve your merchant application at this time.</span></p>");

        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Name: </span>" + MerchantName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Address: </span>" + Address + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Telephone: </span>" + Telephone + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Your merchant application was Credit Declined.  For further information please contact your Partner Relations Department at 888-869-0469.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");
        FormHandler.SendEmail("Unfortunately, your merchant application has been credit declined.", "", sb.ToString(), Constants.RELATIONSHIP_MANAGEMENT_EMAIL, Email, "", ResourceService.AppResources["PaysafeWestProductOwner"], new Hashtable(), MerchantAppUID);
    }

    public static void SendAppReceivedEmail(string MerchantAppUID, string MerchantName, string Address, string Telephone, string MID, string Email)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>Paysafe Group plc</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing Paysafe Group plc</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><br><b><u>Your application has been received!</u></b></span></p>");

        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Name: </span>" + MerchantName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Address: </span>" + Address + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Telephone: </span>" + Telephone + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, Merchant Support is our number one priority.  Please let us know if we can be of assistance.  If you have any questions or comments, please contact us directly at <a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>, or call us at 888-869-0469.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");
        FormHandler.SendEmail("Application Received", "", sb.ToString(), Constants.APPLICATION_EMAIL, Email, "", ResourceService.AppResources["PaysafeWestProductOwner"], new Hashtable(), MerchantAppUID);
    }

    public static void SendAppMerchantActiveEmail(string MerchantAppUID, string MerchantName, string Address, string Telephone, string MID, string Email)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>Paysafe Group plc</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing Paysafe Group plc</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><br><b><u>Your Merchant Application is Active.</u></b></span></p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>MID: </span>" + MID + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Name: </span>" + MerchantName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Address: </span>" + Address + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Telephone: </span>" + Telephone + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Please contact out Client Relations Department for download instructions, As always, Merchant Support is our number one priority.  Please let us know if we can be of assistance.  If you have any questions or comments, please contact us directly at ClientServices@paysafe.com or call us at 888-851-7558.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");
        FormHandler.SendEmail("Application Approved/Active", "", sb.ToString(), Constants.CLIENT_SERVICE_EMAIL, Email, "", ResourceService.AppResources["PaysafeWestProductOwner"], new Hashtable(), MerchantAppUID);
    }

    public static void SendAppWelcomeCallCompleteEmail(string MerchantAppUID, string MerchantName, string Address, string Telephone, string MID, string Email)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>Paysafe Group plc</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing Paysafe Group plc</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><br><b><u>A Welcome Call has been conducted.</u></b></span></p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>MID: </span>" + MID + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Name: </span>" + MerchantName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Address: </span>" + Address + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Telephone: </span>" + Telephone + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Your Client has been contacted and given necessary information to process credit card transactions.  Thank you for choosing Paysafe Group plc, where Merchant Support is our number one priority.  Please let us know if we can be of assistance.  If you have any questions or comments, please contact us directly at ClientServices@paysafe.com or call us at 888-851-7558.</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");
        FormHandler.SendEmail("Merchant Complete", "", sb.ToString(), Constants.CLIENT_SERVICE_EMAIL, Email, "", null, new Hashtable(), MerchantAppUID);
    }

    /// <summary>
    /// Method Used for sending notification mail to the Compliance team. 
    /// </summary> PXP-8618 Rohit Thakur
    /// <param name="subject"></param>
    /// <param name="From"></param>
    /// <param name="MerchantAppUID"></param>
    /// <param name="MerchantName"></param>
    /// <param name="LegalName"></param>
    /// <param name="AlertName"></param>
    /// <param name="Email"></param>
    /// <param name="CC"></param>
    /// <param name="Note"></param>
    /// <param name="AlertTypeName"></param>
    /// <param name="AgentUID"></param>
    /// <param name="CRMName"></param>
    public static void SendCRMAddedNotification(string subject, string From, string MerchantAppUID, string MerchantDBAName, string LegalName, string AlertName, string Email, string CC, string Note, string AlertTypeName, string AgentUID, string CRMName, string AgentDBA)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<p><span style='font-size:22.0pt;color:#024c8b'>Paysafe<o:p></o:p></span></p>");
        sb.Append("<p><span style='font-size:18.0pt;color:brown;text-decoration: underline;'>Alert Notification</span></p>");
        sb.Append("<p><b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'>DATE:</span></b><span style='font-size:10.0pt;font-family:Verdana,sans-serif;'> " + DateTime.Today.ToString("MM/dd/yyyy") + "</span><br><br>");
        sb.Append("<b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'>ALERT TYPE:</span></b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'> " + AlertName + "</span><span style='color:#1F497D'></span><br><br>");
        sb.Append("<b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'>LEGAL NAME:</span></b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'> " + LegalName + "</span><span style='color:#1F497D'></span><br><br>");
        sb.Append("<b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'>MERCHANT DBA:</span></b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'> " + MerchantDBAName + "</span><span style='color:#1F497D'></span><br><br>");
        sb.Append("<b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'>BANK:</span></b><span style='font-size:10.0pt;font-family:Verdana,sans-serif;'>WoodForest</span></p>");
        sb.Append("<b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'>PARTNER DBA:</span></b><span style='font-size:10.0pt;font-family:Verdana,sans-serif;'> " + AgentDBA + "</span></p>");
        sb.Append("<b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'>CRM NAME:</span></b><span style='font-size:10.0pt;font-family:Verdana,sans-serif;'> " + CRMName + "</span></p>");
        sb.Append("<p style='margin:0in;margin-bottom:.0001pt;font-size:12.0pt;font-family:Times New Roman,serif;'><o:p>&nbsp;</o:p></p>");
        string msg = string.Format("<p><br><span style='font-size:10.0pt;font-family:Verdana,sans-serif'>This is to inform that new CRM {0} is added in Zeus system.", CRMName);
        sb.Append(msg);
        sb.Append("<p style='margin:0in;font-size:12.0pt;font-family:Times New Roman,serif;margin-bottom:12.0pt'><o:p>&nbsp;</o:p></p>");

        FormHandler.SendEmail(subject, "", sb.ToString(), From, Email, CC, ConfigurationManager.AppSettings["MeritcardMailsSendToAddress"], new Hashtable(), MerchantAppUID, AgentUID);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DBA"></param>
    /// <param name="userName"></param>
    /// <param name="merchantKey"></param>
    /// <param name="privateLabel"></param>
    /// <param name="emailUserNameOrMerchantKeyType"></param>
    /// <returns></returns>
    public static string GetPaymentXPEmailBody(string DBA, string userName, string password, PrivateLabel privateLabel, eEmailUserNameOrPasswordType emailUserNameOrPasswordType)
    {
        EmailTemplateData emailTemplateData = (privateLabel == null) ? new EmailTemplateData() : new EmailTemplateData(privateLabel);

        StringBuilder sb = new StringBuilder();
        EmailTemplateBuilder.CreateBoldLine(ref sb, "Welcome to " + emailTemplateData.ProductName + "!");
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateLine(ref sb, emailTemplateData.ProductName + " offers a quick and easy way to process credit card transactions using your internet browser in a safe and secure connection. ");
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateLine(ref sb, "The following information is needed to complete the initial setup for your " + emailTemplateData.ProductName + " Virtual Terminal and begin processing test transactions and payments. This e-mail contains information about your " + emailTemplateData.ProductName + " Login Information.");
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateBoldLine(ref sb, emailTemplateData.ProductName + " Virtual Terminal Login Information:");
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);

        if (emailUserNameOrPasswordType == eEmailUserNameOrPasswordType.UserName)
        {
            EmailTemplateBuilder.CreateSimpleLine(ref sb, "<TABLE><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">Merchant Name: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + DBA + "</TD></TR><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">User Name: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + userName + "</TD></TR><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">Password: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">For security purposes, password will be sent in a separate e-mail.</TD></TR></TABLE>");
        }
        else if (emailUserNameOrPasswordType == eEmailUserNameOrPasswordType.Password)
        {
            EmailTemplateBuilder.CreateSimpleLine(ref sb, "<TABLE><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">Merchant Name: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + DBA + "</TD></TR><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">User Name: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">For security purposes, your Username will be sent in a separate e-mail.</TD></TR><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">Password: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + password + "</TD></TR></TABLE>");
        }

        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateBoldLine(ref sb, "Logging In!");
        EmailTemplateBuilder.CreateLine(ref sb, String.Format("Visit: {0} and logon to your account using your User Name and Password. ", EmailTemplateBuilder.CreateUrl(emailTemplateData.ProductUrl)));
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateLine(ref sb, "We strongly recommend you visiting and bookmarking the following resources:");
        EmailTemplateBuilder.CreateLine(ref sb, "<TABLE><TR><TD><TABLE><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">- User Guide: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + EmailTemplateBuilder.Constants.UserGuide + "</TD></TR><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">- Merchant Interface Guide: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + EmailTemplateBuilder.Constants.InterfaceGuide + "</TD></TR></TABLE></TD></TR></TABLE>");
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateBoldLine(ref sb, "Forgot Your Password?");
        EmailTemplateBuilder.CreateLine(ref sb, "If you have forgotten your password, select the \"Forgot Your Password\" identified on the login page.");
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateEmailFooter(ref sb, emailTemplateData.ProductName, emailTemplateData.ProductUrl, emailTemplateData.ClientServiceEmailAddress);

        return sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dba"></param>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="privateLabel"></param>
    /// <param name="emailUserNameOrPasswordType"></param>
    /// <returns></returns>
    public static string GetApiEmailBody(string DBA, string userName, string merchantKey, PrivateLabel privateLabel, eEmailUserNameOrMerchantKeyType emailUserNameOrMerchantKeyType)
    {
        EmailTemplateData emailTemplateData = (privateLabel == null) ? new EmailTemplateData() : new EmailTemplateData(privateLabel);

        System.Text.StringBuilder sb = new StringBuilder();
        EmailTemplateBuilder.CreateBoldLine(ref sb, String.Format("Welcome to {0}!", emailTemplateData.ProductName));
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateLine(ref sb, String.Format("{0} offers a quick and easy way to process credit card transactions using your internet browser in a safe and secure connection.", emailTemplateData.ProductName));
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateLine(ref sb, "The following information is needed to complete your registration. Please provide your User Name and Merchant Key to your 3rd Party Vendor.");
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateBoldLine(ref sb, String.Format("{0} API Gateway Information:", emailTemplateData.ProductName));
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);

        if (emailUserNameOrMerchantKeyType == eEmailUserNameOrMerchantKeyType.UserName)
        {
            EmailTemplateBuilder.CreateSimpleLine(ref sb, "<TABLE><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">Merchant Name: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + DBA + "</TD></TR><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">User Name: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + userName + "</TD></TR><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">Merchant Key: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">For security purposes, password will be sent in a separate e-mail.</TD></TR></TABLE>");
        }
        else if (emailUserNameOrMerchantKeyType == eEmailUserNameOrMerchantKeyType.MerchantKey)
        {
            EmailTemplateBuilder.CreateSimpleLine(ref sb, "<TABLE><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">Merchant Name: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + DBA + "</TD></TR><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">User Name: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">For security purposes, your Merchant Key will be sent in a separate e-mail.</TD></TR><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">Merchant Key: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + merchantKey + "</TD></TR></TABLE>");
        }

        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateBoldLine(ref sb, "Merchant Interface Documentation!");
        EmailTemplateBuilder.CreateLine(ref sb, "<TABLE><TR><TD><TABLE><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">- Merchant Interface Guide: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + EmailTemplateBuilder.Constants.InterfaceGuide + "</TD></TR><TR><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">-  " + emailTemplateData.ProductName + " Posting URL: </TD><TD " + EmailTemplateBuilder.GetStyleAttribute() + ">" + EmailTemplateBuilder.Constants.WebHostPostingUrl + "</TD></TR></TABLE></TD></TR></TABLE>");
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateSingleLineBreak(ref sb);
        EmailTemplateBuilder.CreateEmailFooter(ref sb, emailTemplateData.ProductName, EmailTemplateBuilder.CreateUrl(EmailTemplateBuilder.Constants.PaymentXpUrl), EmailTemplateBuilder.CreateMailTo(emailTemplateData.ClientServiceEmailAddress));

        return sb.ToString();
    }

    public static string GetNewPassword(int PasswordLength)
    {
        Hashtable prms = new Hashtable();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "Select dbo.fn_GeneratePassword(" + PasswordLength.ToString() + ")";
        return DataLayer.ExecuteScalar(cmd, DataLayer.ConnectStringBuild());
    }

    public static bool HasRoleAccess(string queue)
    {
        bool perform = false;

        foreach (KeyValuePair<string, UserRole> kvp in UserSessions.CurrentUser.UserRoles)
        {
            if (kvp.Value.Name == queue || kvp.Value.Name == "Admin") //has role access or is admin
            {
                perform = true;
                break;
            }
        }

        return perform;
    }

    public static void LoopingControls(Control Ctrls, ControlObject obj)
    {
        foreach (Control Ctrl in Ctrls.Controls)
        {

            if (Ctrl.ID == obj.ID)
            {
                if (Ctrl.GetType().Equals(typeof(TextBox)) && obj.Type == "TextBox")
                {

                }
                else if (Ctrl.GetType().Equals(typeof(Label)) && obj.Type == "Label")
                {
                    Label lbl = (Label)Ctrl;
                    lbl.Enabled = obj.IsEnabled;
                    lbl.Visible = obj.IsVisible;
                }
                else if (Ctrl.GetType().Equals(typeof(HyperLink)) && obj.Type == "HyperLink")
                {
                    HyperLink lnk = (HyperLink)Ctrl;
                    lnk.Enabled = obj.IsEnabled;
                    lnk.Visible = obj.IsVisible;

                    if (!lnk.Enabled)
                        lnk.CssClass = "disabledText";
                }
                else if (Ctrl.GetType().Equals(typeof(LinkButton)) && obj.Type == "LinkButton")
                {
                    LinkButton lnk = (LinkButton)Ctrl;
                    lnk.Enabled = obj.IsEnabled;
                    lnk.Visible = obj.IsVisible;


                    if (!lnk.Enabled)
                        lnk.CssClass = "disabledText";

                }
                else if (Ctrl.GetType().Equals(typeof(Panel)) && obj.Type == "Panel")
                {
                    Panel pnl = (Panel)Ctrl;
                    pnl.Enabled = obj.IsEnabled;
                    pnl.Visible = obj.IsVisible;
                }
                else if (Ctrl.GetType().Equals(typeof(WebImageButton)) && obj.Type == "WebImageButton")
                {
                    WebImageButton btn = (WebImageButton)Ctrl;
                    btn.Enabled = obj.IsEnabled;
                    btn.Visible = obj.IsVisible;
                }
                else if (Ctrl.GetType().Equals(typeof(DropDownList)) && obj.Type == "DropDownList")
                {
                    DropDownList cbo = (DropDownList)Ctrl;
                    cbo.Enabled = obj.IsEnabled;
                    cbo.Visible = obj.IsVisible;

                }
                else if (Ctrl.GetType().Equals(typeof(WebDataTree)) && obj.Type == "UltraWebTree")
                {
                    WebDataTree uwt = (WebDataTree)Ctrl;

                    if (obj.ControlObjectDetails != null)
                    {
                        foreach (ControlObject o in obj.ControlObjectDetails)
                        {
                            try
                            {
                                //Node n = uwt.Find(o.ID); //marknguyen20120124
                                //n.Enabled = false;
                            }
                            catch { }
                        }
                    }
                }
                else if (Ctrl.GetType().Equals(typeof(Button)) && obj.Type == "Button")
                {
                    Button cbo = (Button)Ctrl;
                    cbo.Enabled = obj.IsEnabled;
                    cbo.Visible = obj.IsVisible;

                }


                else if (Ctrl.GetType().Equals(typeof(WebImageButton)))
                {
                    WebImageButton imgbtn = (WebImageButton)Ctrl;
                    imgbtn.Enabled = obj.IsEnabled;
                    imgbtn.Visible = obj.IsVisible;
                }
            }

            if (Ctrl.HasControls())
                LoopingControls(Ctrl, obj);
        }

    }

    public static void LoopingControls2(Control Ctrls, UserForm frm)
    {
        foreach (ControlObject obj in frm.ControlObjects)
        {
            Control Ctrl = FormHandler.FindFormControl(Ctrls, obj.ID);
            if (Ctrl != null)
            {
                if (Ctrl.GetType().Equals(typeof(TextBox)))
                {

                }
                else if (Ctrl.GetType().Equals(typeof(Label)))
                {
                    Label lbl = (Label)Ctrl;
                    lbl.Enabled = obj.IsEnabled;
                    lbl.Visible = obj.IsVisible;

                }
                else if (Ctrl.GetType().Equals(typeof(DropDownList)))
                {
                    DropDownList cbo = (DropDownList)Ctrl;
                    cbo.Enabled = obj.IsEnabled;
                    cbo.Visible = obj.IsVisible;

                }
                else if (Ctrl.GetType().Equals(typeof(WebDataTree)))
                {
                    WebDataTree uwt = (WebDataTree)Ctrl;

                    if (obj.ControlObjectDetails != null)
                    {
                        foreach (ControlObject o in obj.ControlObjectDetails)
                        {
                            try
                            {
                                //Node n = uwt.Nodes.FindNodeByKey(o.ID); //marknguyen20120124
                                //n.Enabled = false;
                            }
                            catch { }
                        }
                    }
                }

                else if (Ctrl.GetType().Equals(typeof(WebImageButton)))
                {
                    WebImageButton imgbtn = (WebImageButton)Ctrl;
                    imgbtn.Enabled = false;

                }
            }

            if (Ctrl.HasControls())
                LoopingControls(Ctrl, obj);
        }
    }

    /// <summary>
    /// given a container, if your Control.ID is in the list, then disable it. used for disabling individual controls within a container
    /// </summary>
    /// <param name="container"></param>
    /// <param name="SpecialAccessEditList"></param>
    public static void LockDownInList(Control container, List<string> SpecialAccessEditList)
    {
        foreach (Control control in container.Controls)
        {
            if (control != null)
            {
                if (!string.IsNullOrWhiteSpace(control.ID))
                {
                    string controlid = control.ID.ToLower();
                    bool yes_lock = SpecialAccessEditList.Contains(controlid);

                    if (SpecialAccessEditList.Contains(controlid))
                    {
                        if (control is ListControl)
                        {
                            ListControl listControl = (ListControl)control;
                            listControl.Enabled = false;
                            //listControl.Attributes.Add("class", "culocked");
                        }
                        else if (control is TextBox)
                        {
                            TextBox txt = (TextBox)control;
                            txt.ReadOnly = true;
                            //txt.Attributes.Add("class", "culocked");

                        }
                        else if (control is CheckBox)
                        {
                            CheckBox chk = (CheckBox)control;
                            chk.Enabled = false;
                            //chk.Attributes.Add("class", "culocked");
                        }
                        else if (control is Button)
                        {
                            Button btn = (Button)control;
                            btn.Enabled = false;
                            //btn.Attributes.Add("class", "culocked");
                        }
                        else if (control is WebMaskEditor)
                        {
                            WebMaskEditor txt = (WebMaskEditor)control;
                            txt.ReadOnly = true;
                            //txt.Attributes.Add("class", "culocked");
                        }
                        else if (control is WebDateTimeEditor)
                        {
                            WebDateTimeEditor txt = (WebDateTimeEditor)control;
                            txt.ReadOnly = true;
                            //txt.Attributes.Add("class", "culocked");
                        }
                        else if (control is WebDateChooser)
                        {
                            WebDateChooser txt = (WebDateChooser)control;
                            txt.ReadOnly = true;
                            //txt.Attributes.Add("class", "culocked");
                        }
                        else if (control is WebNumericEditor)
                        {
                            WebNumericEditor txt = (WebNumericEditor)control;
                            txt.ReadOnly = true;
                            //txt.Attributes.Add("class", "culocked");
                        }
                        else if (control is WebCurrencyEditor)
                        {
                            WebCurrencyEditor txt = (WebCurrencyEditor)control;
                            txt.ReadOnly = true;
                            //txt.Attributes.Add("class", "culocked");
                        }
                        else if (control is WebPercentEditor)
                        {
                            WebPercentEditor txt = (WebPercentEditor)control;
                            txt.ReadOnly = true;
                            //txt.Attributes.Add("class", "culocked");
                        }
                        else if (control is WebHtmlEditor)
                        {
                            WebHtmlEditor txt = (WebHtmlEditor)control;
                            txt.ReadOnly = true;
                            //txt.Attributes.Add("class", "culocked");
                        }
                        else if (control is FileUpload)
                        {
                            FileUpload txt = (FileUpload)control;
                            txt.Enabled = false;
                            //txt.Attributes.Add("class", "culocked");
                        }
                        else if (control is GridView)
                        {
                            GridView grd = (GridView)control;
                            grd.Enabled = false;
                            //grd.Attributes.Add("class", "culocked");
                        }
                    }



                }

                if (control.HasControls())
                {
                    LockDownInList(control, SpecialAccessEditList);
                }
            }
        }
    }

    public static string GetObjectPropertyValues(object obj)
    {
        if (obj == null)
            return string.Empty;

        // Get the properties of the business object
        //
        Type objType = obj.GetType();
        PropertyInfo[] objPropertiesArray = objType.GetProperties();

        StringBuilder sb = new StringBuilder();

        foreach (PropertyInfo prop in objPropertiesArray)
        {
            sb.Append(prop.Name + " = " + prop.GetValue(obj, null).ToString() + "\n");
        }

        return sb.ToString();
    }

    public static string GetWelcomeEmailGatewayKey(string DBA, string UserName, PrivateLabel objPrivateLabel)
    {
        StringBuilder sb = new StringBuilder();

        Dictionary<string, string> di = new Dictionary<string, string>();

        di["url"] = "https://www.paymentxp.com";
        di["company"] = "Paysafe Group plc";
        di["CSPhone"] = "1-888-851-7558.";
        di["infoUrl"] = "<a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>";
        di["portal"] = "PaymentXP";
        di["DBA"] = DBA;
        di["UserName"] = UserName;

        if (objPrivateLabel != null)
        {
            di["url"] = objPrivateLabel.VTURL;
            di["company"] = objPrivateLabel.PLCompanyName;
            di["infoUrl"] = "<a href='" + objPrivateLabel.PLEmail + "'>" + objPrivateLabel.PLEmail + "</a>";
            di["CSPhone"] = objPrivateLabel.PLPhone;
            di["portal"] = "Virtual Terminal";
        }


        string template = @"
        <span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>[[company]]</span>
        <br>
        <p>
            <span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing [[company]].</span>
        </p>
        <p>
            <span style='font-size:10.0pt;font-family:Verdana;'>
                <b>Login Information:</b><br>
                You can access the [[portal]] Gateway by using the following information.
            </span>
        </p>
        <p>
        <span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>DBA: </span>[[DBA]]
        <br>
        <span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Gateway URL: </span>Please refer to User Interface Guide document.
        <br>
        <span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Username: </span>[[UserName]]
        <br>
        <span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Merchant Key: </span> For security, your password will be sent in a separate email.</p>
        <p>
            <span style='font-size:10.0pt;font-family:Verdana;'>
                As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at [[infoUrl]], or call us at [[CSPhone]]
            </span>
        </p>
        <br>
        <br>
        ";

        string body = "";
        CommonUtility.Template t = new Template();

        if (t.getTemplateAndFill_FromString(template, di, ref body))
        {
            return body;
        }
        else
        {
            return "";
        }

    }

    public static string GetWelcomeEmailPaymentXP(string DBA, string UserName, string Password, PrivateLabel objPrivateLabel)
    {
        StringBuilder sb = new StringBuilder();
        string url = "https://www.paymentxp.com";
        string company = "Paysafe Group plc";
        string infoUrl = "<a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>";
        string CSPhone = "1-888-851-7558.";

        if (objPrivateLabel != null)
        {
            url = objPrivateLabel.VTURL;
            company = objPrivateLabel.PLCompanyName;
            infoUrl = "<a href='" + objPrivateLabel.PLEmail + "'>" + objPrivateLabel.PLEmail + "</a>";
            CSPhone = objPrivateLabel.PLPhone;
        }

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>" + company + " Virtual Terminal</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing " + company + " and the " + company + " Virtual Terminal.</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><b>Login Information:</b><br>You can access the Virtual Terminal by using the following information. If this is your first time logging in, you will be required to generate a password.</span></p>");

        sb.Append("<p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>DBA: </span>" + DBA + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>LoginURL: </span><a href='" + url + "'>" + url + "</a><br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Login ID: </span>" + UserName + "<br>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Password: </span>" + "For security, your password will be sent in a separate email." + "</p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at " + infoUrl + ", or call us at " + CSPhone + "</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");

        return sb.ToString();
    }

    public static bool VerifyAdd(string url)
    {
        HtmlAgilityPack.HtmlDocument m_WebDoc;
        HtmlWeb hw = new HtmlWeb();
        try
        {
            m_WebDoc = hw.Load(url);
        }
        catch
        {
            m_WebDoc = null;
        }

        if (m_WebDoc != null)
        {
            string html = m_WebDoc.DocumentNode.InnerHtml;
            if (html.ToUpper().Contains("ADDRESS VERIFIED"))
                return true;
        }
        return false;
    }



    public static bool GetConfiguredAgentID(string agentID)
    {
        bool isConfiguredAgentID = false;
        List<string> allowedAgentIDs = ConfigurationManager.AppSettings["AgentIdsFor3de"].Split(',').ToList();
        if (allowedAgentIDs.Contains(agentID))
        {
            isConfiguredAgentID = true;
        }
        return isConfiguredAgentID;
    }

    private static bool IsDuplicateMID(string AuthPlatformMid, string SettlePlatformMid, bool Adding, string UID, MerchantBrand Brand)
    {
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        Hashtable prms = new Hashtable();

        if (string.IsNullOrWhiteSpace(AuthPlatformMid) && string.IsNullOrWhiteSpace(SettlePlatformMid))
            return false;

        if (!string.IsNullOrWhiteSpace(AuthPlatformMid))
            prms.Add("@AuthPlatformMID", AuthPlatformMid.Trim());

        if (!string.IsNullOrWhiteSpace(SettlePlatformMid))
            prms.Add("@SettlePlatformMID", SettlePlatformMid.Trim());

        prms.Add("@Adding", Adding);
        prms.Add("@BrandID", (int)Brand);

        if (!Adding)
            prms.Add("@UID", UID);

        bool perform = data.IsDuplicateMID(prms, Brand);
        return perform;
    }

    /// <summary>
    /// defaults to Meritus Brand
    /// </summary>
    public static void FillPDF()
    {
        FillPDF(MerchantBrand.Meritus);
    }

    public static void FillPDF(MerchantBrand brand)
    {
        string frmType = string.Empty;

        if (custom.TryGetValue("UploadFormType", out frmType))
            FillOpsPDF(brand);

        // on default, set this to meritus brand
        if (brand == MerchantBrand.None)
        {
            brand = MerchantBrand.Meritus;
        }

        MerchantApp app = UserSessions.CurrentMerchantApp;
        Underwriting objUW = new Underwriting();
        DataUnderwritng data = DataAccess.DataUnderwritingDao;

        objUW = data.LoadMerchantUWNotes(app.MerchantAppUID);
        if (objUW == null)
        {
            objUW = new Underwriting();
            objUW.MerchantAppUID = app.MerchantAppUID;
            DataAccess.DataUnderwritingDao.UpdateMerchantUWNotes(objUW, app.ID, UserSessions.CurrentUser.UserName);
            objUW = data.LoadMerchantUWNotes(app.MerchantAppUID);
        }
        StringBuilder sb = new StringBuilder();
        if (string.IsNullOrEmpty(frmType))
        {
            custom.Add("BankName", app.Bank);
            custom.Add("BusinessDBAName", app.BusinessDBAName);
            custom.Add("BusinessLegalName", app.BusinessLegalName);

            if (objUW != null && objUW.BusinessStartDate != null && objUW.BusinessStartDate != DateTime.MinValue)
                custom.Add("BusinessStartDate", objUW.BusinessStartDate.ToShortDateString());
            else
                custom.Add("BusinessStartDate", "");

            string Swiped = app.TinfoElectronicDataCaptureSwipedPercent.ToString("N0") + "%";
            custom.Add("Swiped", Swiped);

            //Niranjan PXP-5225 Zeus: Changes on Ops Form and CU Form for Additional Information fields
            // PXP-10211 Zeus: Changes on Ops Form and CU Form for Additional Information fields for BBVA applications
            if ((UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS)
                && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
            {
                string Keyed = Convert.ToDecimal(app.TinfoManualEntryWithImprintPercent).ToString("N0") + "%";
                custom.Add("Keyed", Keyed);
            }
            else
            {
                string Keyed = Convert.ToDecimal(100 - app.TinfoElectronicDataCaptureSwipedPercent).ToString("N0") + "%";
                custom.Add("Keyed", Keyed);
            }
            string card_not_present_keyed = (app.TinfoManualEntryNoCardNoImprintPercent + app.TinfoVoiceAuthPercent).ToString("N0") + "%";
            custom.Add("card_not_present_keyed", card_not_present_keyed);

            custom.Add("TinfoAverageVMCTicket", app.TinfoAverageVMCTicket.ToString("c").Replace("$", ""));
            custom.Add("TinfoHighestTicketAmount", app.TinfoHighestTicketAmount.ToString("c").Replace("$", ""));
            custom.Add("TinfoAverageMonthlyVMCVolume", app.TinfoAverageMonthlyVMCVolume.ToString("c").Replace("$", ""));

            custom.Add("MerchantSells", app.MerchantSells);

            EquipmentFacade facade = new EquipmentFacade();
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
            DataSet ds = facade.GetMerchantAppItem(prms);
            DataRow dr = null;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dr = ds.Tables[0].Rows[i];
                int j = i + 1;

                if (DataLayer.Field2Bool(dr["IsEnabled"]))
                {
                    sb.Append(j.ToString() + ") Type: " + dr["EquipmentType"].ToString());
                    sb.Append(", Maker: " + dr["EquipmentMaker"].ToString());
                    sb.Append(", Model: " + dr["Model"].ToString() + "; ");
                }
            }

            if (!string.IsNullOrWhiteSpace(sb.ToString()))
                custom.Add("Equipment", sb.ToString());
            else
                custom.Add("Equipment", "No Equipment Information");
        }
        // start DM-1915 by Jorge Perez
        if (app.Owners != null && app.Owners.Count > 0)
        {            
            for (int i = 0; i < app.Owners.Count; i++)
            {
                string suffLbl = i > 0 ? i.ToString() : "";

                app.Owners[i].FullName = string.Format("{0} {1} {2} {3}",app.Owners[i].FirstName, app.Owners[i].MiddleName, app.Owners[i].LastName, app.Owners[i].Suffix);
                custom.Add(string.Format("FullName{0}", suffLbl), app.Owners[i].FullName);

                var CreditScoreLbl = string.Format("CreditScore{0}", suffLbl);
                var NoofTradesLbl = string.Format("NoofTrades{0}", suffLbl);
                var OldestTradeLbl = string.Format("OldestTrade{0}", suffLbl);
                var TradeStatusLbl = string.Format("TradeStatus{0}", suffLbl);

                if (!string.IsNullOrWhiteSpace(app.Owners[i].FullName))
                {
                    if (CommonUtility.Util.if_i(app.Owners[i].CreditScore, 0) > 0)
                    {
                        custom.Add(CreditScoreLbl, app.Owners[i].CreditScore);
                        custom.Add(NoofTradesLbl, CommonUtility.Util.if_i(app.Owners[i].NoofTrades, 0).ToString());
                        custom.Add(OldestTradeLbl, DataLayer.Field2Date(app.Owners[i].OldestTrade) != DateTime.MinValue ? DataLayer.Field2Date(app.Owners[i].OldestTrade).ToShortDateString() : "");
                        custom.Add(TradeStatusLbl, app.Owners[i].TradeStatus);
                    }
                    else
                    {
                        custom.Add(CreditScoreLbl, app.Owners[i].TCreditScore);
                        custom.Add(NoofTradesLbl, CommonUtility.Util.if_i(app.Owners[i].TNoofTrades, 0).ToString());
                        custom.Add(OldestTradeLbl, DataLayer.Field2Date(app.Owners[i].TOldestTrade) != DateTime.MinValue ? DataLayer.Field2Date(app.Owners[i].TOldestTrade).ToShortDateString() : "");
                        custom.Add(TradeStatusLbl, app.Owners[i].TTradeStatus);
                    }
                }
                else
                {
                    custom.Add(CreditScoreLbl, "");
                    custom.Add(NoofTradesLbl, "");
                    custom.Add(OldestTradeLbl, "");
                    custom.Add(TradeStatusLbl, "");
                }
            }
            // end DM-1915 by Jorge Perez
        }

        if (string.IsNullOrEmpty(frmType))
        {
            custom.Add("SicCode", app.SicCode);
            custom.Add("SicCodeDesc", app.SicCodeDesc);

            string txtRelType = getReleaseType(app.ReleaseMethodUID);

            custom.Add("ReleaseMethod", txtRelType);

            if (app.MonthendApproved)
                custom.Add("DiscountMethod", "Monthly");
            else
                custom.Add("DiscountMethod", "Daily");

            app.DelaysApproved = app.DelaysApproved == "-1" ? app.DelaysApproved.Replace("-1", "0") : app.DelaysApproved;
            custom.Add("DelaysApproved", string.IsNullOrWhiteSpace(app.DelaysApproved) ? "0" : app.DelaysApproved);
            custom.Add("ReservePercent", app.ReservePercent.ToString("0.00") + "%");
            custom.Add("UpfrontReserve", app.UpfrontReserve.ToString("c").Replace("$", ""));
        }
        sb = new StringBuilder();
        StringBuilder sbNotes;
        StringBuilder sbLog;
        string chk, exp;

        DataSet ds1 = data.GetUWCheckList(app.MerchantAppUID);

        foreach (DataRow grdRow in ds1.Tables[0].Rows)
        {
            if (grdRow.ItemArray[3].ToString().ToLower() == "true") chk = "Yes"; else chk = "No";
            if (grdRow.ItemArray[5].ToString().ToLower() == "true") exp = "Yes"; else exp = "No";
            custom.Add(grdRow.ItemArray[0].ToString().ToLower(), chk);
            custom.Add(grdRow.ItemArray[0].ToString().ToLower() + "_Exc", exp);

            if (!string.IsNullOrWhiteSpace(grdRow.ItemArray[6].ToString()))
            {
                sbNotes = new StringBuilder();
                sbNotes.AppendFormat(grdRow.ItemArray[6].ToString());
                custom.Add(grdRow.ItemArray[0].ToString().ToLower() + "_Notes", sbNotes.ToString());
            }
            if (!string.IsNullOrWhiteSpace(grdRow.ItemArray[8].ToString()))
            {
                sbLog = new StringBuilder();
                sbLog.AppendFormat(grdRow.ItemArray[8].ToString());
                custom.Add(grdRow.ItemArray[0].ToString().ToLower() + "_Log", sbLog.ToString());

                sb.Append("\r\n" + grdRow.ItemArray[1].ToString() + ": " + grdRow.ItemArray[6].ToString());
            }
        }

        // store this here for now. we might need to append conditions later down the page.
        string checklist_notes = sb.ToString();
        if (string.IsNullOrEmpty(frmType))
        {
            if (UserSessions.CurrentMerchantApp.DaysHoldTypeID == 1)
            {
                custom.Add("HoldArrearsType", "Calendar Days");
            }
            else if (UserSessions.CurrentMerchantApp.DaysHoldTypeID == 2)
            {
                custom.Add("HoldArrearsType", "Business Days");
            }

            if (objUW != null)
            {

                custom.Add("Reservepct", objUW.Reservepct.ToString("#0.00") + "%");

                if (objUW != null && (objUW.CUDate != DateTime.MinValue || objUW.ACHCUDate != DateTime.MinValue))
                    custom.Add("CUDate", (objUW.ACHCUDate != DateTime.MinValue) ? objUW.ACHCUDate.ToShortDateString() : objUW.CUDate.ToShortDateString());
                else
                    custom.Add("CUDate", string.Empty);

                if (objUW != null && (!string.IsNullOrEmpty(objUW.ApprovedUser) || !string.IsNullOrEmpty(objUW.ACHApprovedUser)))
                    custom.Add("ApprovedUser", !string.IsNullOrEmpty(objUW.ACHApprovedUser) ? objUW.ACHApprovedUser : objUW.ApprovedUser);
                else
                    custom.Add("ApprovedUser", string.Empty);

                custom.Add("FlagsOffUponBoarding", (objUW.FlagsOffUponBoarding.ToLower() == "true" ? "Yes" : "No"));
                custom.Add("DivertUponBoarding", (objUW.DivertUponBoarding.ToLower() == "true" ? "Yes" : "No"));
                custom.Add("BuyPassIndicator", (objUW.BuyPassIndicator.ToLower() == "true" ? "Yes" : "No"));

                if((app.MerchantAppTypeUID.ToUpper() == Constants.BANK_CHESAPEAKE && app.SettlePlatformUID.ToUpper() == SettlementPlatforms.Tsys) ||
                    app.MerchantAppTypeUID.ToUpper() == Constants.BANK_BMO_HARRIS)
                    custom.Add("AgentLevel", string.IsNullOrEmpty(objUW.AssociationNumber) ? "100001" : objUW.AssociationNumber);
                else
                    custom.Add("AgentLevel", objUW.AgentLevel);
                // populate the products
                // logic taken from wucServices.ascx.cs -> LoadServices()
                List<string> liP = new List<string>();
                DataMerchantServices data1 = new DataMerchantServices();
                Hashtable prms1 = new Hashtable();
                IList<MerchantServices> services1 = new List<MerchantServices>();
                prms1.Add("@MerchantAppUID", DataLayer.UID2Field(app.MerchantAppUID));
                prms1.Add("@CategoryID", CommonUtility.Util.if_s((int)ServiceCategories.CREDITPRODUCTS));
                services1 = data1.GetMerchantServicesWithoutProducts(prms1);

                foreach (MerchantServices service in services1)
                {
                    if (service.Checked)
                    {
                        liP.Add(service.Name);
                    }
                }

                custom.Add("ProductsRequired", string.Join(", ", liP));

                if (objUW.PaymentFrequencyID > 0)
                {
                    custom.Add("PaymentFrequency", GetDescription(((Underwriting.ePaymentFrequency)objUW.PaymentFrequencyID)));
                }
                if (objUW.PaymentScheduleID > 0)
                {
                    custom.Add("PaymentSchedule", ((Underwriting.ePaymentSchedule)objUW.PaymentScheduleID).ToString());
                }
            }
            else
            {
                objUW = new Underwriting();
                custom.Add("CUDate", "");
                custom.Add("FlagsOffUponBoarding", "No");
                custom.Add("DivertUponBoarding", "No");
                custom.Add("BuyPassIndicator", "No");
                custom.Add("AgentLevel", "No");
                custom.Add("Reservepct", "0.00 %");
            }

            custom.Add("UWIssues", UserSessions.CurrentMerchantApp.UWNotes + "\r\n \r\nSpecial Requests: \r\n" + UserSessions.CurrentMerchantApp.SpecialRequest);
            custom.Add("HighRiskDescriptor", GetDescriptorList(app));

            StringBuilder sbcond = new StringBuilder();

            if (app.ConditionalApproval)
            {
                sbcond.AppendLine(string.Format("This application is conditionally approved with conditions due on {0}:", app.ConditionalDueDate.ToShortDateString()));

                // find pending condition
                IList<UWConditions> liUWC = DataMerchantApp.GetInstance().GetUWConditionsList(app.MerchantAppUID);

                if (liUWC != null)
                {
                    foreach (UWConditions obj in liUWC)
                    {
                        if (obj.ReceivedInfo == false)
                        {
                            sbcond.AppendLine(string.Format("* {0}", obj.ConditionName));
                            sbcond.AppendLine(string.Format("   - {0}", obj.EmailText));
                        }
                    }

                    // append to the checklist notes
                    checklist_notes += sbcond.ToString();
                }
            }

            custom.Add("UWConditionNotes", sbcond.ToString());

        }
        if (objUW != null)
        {
            custom.Add("NotifyRiskDept", objUW.NotifyRiskDept ? "Yes" : "No");
        }
        else
        {
            objUW = new Underwriting();
            custom.Add("NotifyRiskDept", "No");
        }
        if (string.IsNullOrEmpty(frmType))
        {
            if (objUW != null)
            {

                custom.Add("AchAverageTicket", objUW.AchAverageTicket.ToString("c").Replace("$", ""));
                custom.Add("AchHighTicket", objUW.AchHighTicket.ToString("c").Replace("$", ""));
                custom.Add("AchMonthlyVolume", objUW.AchMonthlyVolume.ToString("c").Replace("$", ""));
            }
            else
            {
                objUW = new Underwriting();

                custom.Add("AchAverageTicket", "0.00");
                custom.Add("AchHighTicket", "0.00");
                custom.Add("AchMonthlyVolume", "0.00");
            }
        }
        //Niranjan PXP-5225 Zeus: Changes on Ops Form and CU Form for Additional Information fields
        // PXP-10211 Zeus: Changes on Ops Form and CU Form for Additional Information fields for BBVA applications
        if ((UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS))
            && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
        {
            FormBinding.BindObjectToPDF(objUW, "~/PDF/UW_Verification_Form_woodforest.pdf", "CU_Form_" + app.ID.ToString() + ".pdf", custom);
        }
        else
        {
            FormBinding.BindObjectToPDF(objUW, "~/PDF/UW_Verification_Form.pdf", "CU_Form_" + app.ID.ToString() + ".pdf", custom);
        }
    }

    public static void FillOpsPDF(MerchantBrand brand)
    {
        string frmType = string.Empty;
        custom.TryGetValue("UploadFormType", out frmType);

        // on default, set this to meritus brand
        if (brand == MerchantBrand.None)
        {
            brand = MerchantBrand.Meritus;
        }

        MerchantApp app = UserSessions.CurrentMerchantApp;
        Underwriting objUW = new Underwriting();
        DataUnderwritng data = DataAccess.DataUnderwritingDao;

        objUW = data.LoadMerchantUWNotes(app.MerchantAppUID);
        // PXP-12436: Start by Rohit Thakur
        if (app.Office == CommonUtility.Util.Offices.Irvine)
        {
            if (objUW != null)
            {
                if (objUW.UWIssues.ToString().Contains("'Nutra Trial' account"))
                {
                    objUW.UWIssues = objUW.UWIssues.ToString().Replace("Nutra", "Tangible");
                }
            }
        }

        // PXP-12436: End by Rohit Thakur
        if (objUW != null)
            app.UWNotes = objUW.UWIssues;
        else
        {

            objUW = new Underwriting();
            objUW.MerchantAppUID = app.MerchantAppUID;
            DataAccess.DataUnderwritingDao.UpdateMerchantUWNotes(objUW, app.ID, UserSessions.CurrentUser.UserName);
            objUW = data.LoadMerchantUWNotes(app.MerchantAppUID);
        }
        if (string.IsNullOrWhiteSpace(app.UWNotes))
        {
            app.UWNotes = FormHandler.GetUWIssuesTemplate(app);
        }

        LoadBusinessInformation(custom, app, objUW);
        LoadMerchantProfile(custom, app, objUW);
        LoadConditionalNotes(custom, app);
        objUW = LoadOperationsInstructions(custom, app, objUW);

        if (string.IsNullOrEmpty(frmType))
        {
            //code changes doen for PXP-7470 by koshlendra start
            if (UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
            {
                //Niranjan PXP-5225 Zeus: Changes on Ops Form and CU Form for Additional Information fields
                // PXP-10211 Zeus: Changes on Ops Form and CU Form for Additional Information fields for BBVA applications
                if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS))
                {
                    FormBinding.BindObjectToPDF(objUW, "~/PDF/Ops_Form_woodforest.pdf", "Ops_Form_" + app.ID.ToString() + ".pdf", custom);
                }
                else
                {
                    FormBinding.BindObjectToPDF(objUW, "~/PDF/Ops_Form_Irvine.pdf", "Ops_Form_" + app.ID.ToString() + ".pdf", custom);
                }
                //code changes doen for PXP-7470 by koshlendra end
            }
            else
            {
                FormBinding.BindObjectToPDF(objUW, "~/PDF/Ops_Form.pdf", "Ops_Form_" + app.ID.ToString() + ".pdf", custom);
            }
        }
    }

    private static void LoadBusinessInformation(IDictionary<string, string> custom, MerchantApp app, Underwriting objUW)
    {
        custom.Add("BankName", app.Bank);
        custom.Add("BusinessDBAName", app.BusinessDBAName);
        custom.Add("BusinessLegalName", app.BusinessLegalName);

        if (objUW != null && (objUW.CUDate != DateTime.MinValue || objUW.ACHCUDate != DateTime.MinValue))
            custom.Add("CUDate", (objUW.ACHCUDate != DateTime.MinValue) ? objUW.ACHCUDate.ToShortDateString() : objUW.CUDate.ToShortDateString());
        else
            custom.Add("CUDate", string.Empty);

        if (objUW != null && (!string.IsNullOrEmpty(objUW.ApprovedUser) || !string.IsNullOrEmpty(objUW.ACHApprovedUser)))
            custom.Add("ApprovedUser", !string.IsNullOrEmpty(objUW.ACHApprovedUser) ? objUW.ACHApprovedUser : objUW.ApprovedUser);
        else
            custom.Add("ApprovedUser", string.Empty);
    }

    private static void LoadMerchantProfile(IDictionary<string, string> custom, MerchantApp app, Underwriting objUW)
    {
        if (objUW != null && objUW.BusinessStartDate != null && objUW.BusinessStartDate != DateTime.MinValue)
            custom.Add("BusinessStartDate", objUW.BusinessStartDate.ToShortDateString());
        else
            custom.Add("BusinessStartDate", "");

        string Swiped = app.TinfoElectronicDataCaptureSwipedPercent.ToString("N0") + "%";
        custom.Add("Swiped", Swiped);

        //Niranjan PXP-5225 Zeus: Changes on Ops Form and CU Form for Additional Information fields
        //Niranjan: PXP-10211
        if ((UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS))
            && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
        {
            string Keyed = Convert.ToDecimal(app.TinfoManualEntryWithImprintPercent).ToString("N0") + "%";
            custom.Add("Keyed", Keyed);
        }
        else
        {
            string Keyed = Convert.ToDecimal(100 - app.TinfoElectronicDataCaptureSwipedPercent).ToString("N0") + "%";
            custom.Add("Keyed", Keyed);
        }
        string card_not_present_keyed = (app.TinfoManualEntryNoCardNoImprintPercent + app.TinfoVoiceAuthPercent).ToString("N0") + "%";
        custom.Add("card_not_present_keyed", card_not_present_keyed);

        custom.Add("TinfoAverageVMCTicket", app.TinfoAverageVMCTicket.ToString("c").Replace("$", ""));
        custom.Add("TinfoHighestTicketAmount", app.TinfoHighestTicketAmount.ToString("c").Replace("$", ""));
        custom.Add("TinfoAverageMonthlyVMCVolume", app.TinfoAverageMonthlyVMCVolume.ToString("c").Replace("$", ""));


        if (objUW != null)
        {
            custom.Add("AchAverageTicket", objUW.AchAverageTicket.ToString("c").Replace("$", ""));
            custom.Add("AchHighTicket", objUW.AchHighTicket.ToString("c").Replace("$", ""));
            custom.Add("AchMonthlyVolume", objUW.AchMonthlyVolume.ToString("c").Replace("$", ""));
        }
        else
        {
            objUW = new Underwriting();
            custom.Add("AchAverageTicket", "0.00");
            custom.Add("AchHighTicket", "0.00");
            custom.Add("AchMonthlyVolume", "0.00");
        }
        //code added for PXP-2691[Zeus: Add "Annual Volume" field in Merchant Profile section on Ops form] by koshlendra start
        if (app.Office == CommonUtility.Util.Offices.Irvine)
        {
            custom.Add("AnnualVolume", (app.TinfoAverageMonthlyVMCVolume * 12).ToString("c").Replace("$", ""));

            //code changes for PXP-11701 by koshlendra start
            string billingMethod = "";

            switch (app.BillingMethodUID.ToUpper())
            {
                case "436661DF-998D-4031-B3A1-DD15D5296733":
                    billingMethod = "Individual";
                    break;
                case "17D8B7FA-19FF-49CF-A232-024A22668AEB":
                    billingMethod = "Combined";
                    break;
                case "F4D9E484-80D0-465B-93CA-B976F24232A8":
                    billingMethod = "Separate";
                    break;
            }

            string supressProcessStmts = "";
            switch (app.SuppressProcessingStatements)
            {
                case true:
                    supressProcessStmts = "Yes";
                    break;
                case false:
                    supressProcessStmts = "No";
                    break;
            }
            string chargebackExcessiveFeeWaived = "";
            switch (app.ChargebackExcessiveFeeWaived)
            {
                case true:
                    chargebackExcessiveFeeWaived = "Yes";
                    break;
                case false:
                    chargebackExcessiveFeeWaived = "No";
                    break;
            }
            string waiveOtherItemFee = "";
            switch (app.WaiveOtherItemFee)
            {
                case true:
                    waiveOtherItemFee = "Yes";
                    break;
                case false:
                    waiveOtherItemFee = "No";
                    break;
            }
            custom.Add("BillingMethod", billingMethod);
            custom.Add("SupressProcessStmts", supressProcessStmts);
            custom.Add("ChargebackExcessiveFeeWaived", chargebackExcessiveFeeWaived);
            custom.Add("WaiveOtherItemFee", waiveOtherItemFee);
            //code changes for PXP-11701 by koshlendra end
        }
        //code added for PXP-2691[Zeus: Add "Annual Volume" field in Merchant Profile section on Ops form] by koshlendra end

        LoadEquipments(custom);
        custom.Add("MerchantSells", app.MerchantSells);
    }

    private static void LoadConditionalNotes(IDictionary<string, string> custom, MerchantApp app)
    {
        StringBuilder sbcond = new StringBuilder();

        if (app.ConditionalApproval)
        {
            sbcond.AppendLine(string.Format("This application is conditionally approved with conditions due on {0}:", app.ConditionalDueDate.ToShortDateString()));

            // find condition
            IList<UWConditions> liUWC = DataMerchantApp.GetInstance().GetUWConditionsList(app.MerchantAppUID);

            if (liUWC != null)
            {
                foreach (UWConditions obj in liUWC)
                {
                    if (obj.ReceivedInfo == false)
                    {
                        sbcond.AppendLine(string.Format("* {0}", obj.ConditionName));
                        sbcond.AppendLine(string.Format("   - {0}", obj.EmailText));
                    }
                }
            }
        }
        custom.Add("UWConditionNotes", sbcond.ToString());
    }

    private static Underwriting LoadOperationsInstructions(IDictionary<string, string> custom, MerchantApp app, Underwriting objUW)
    {
        string frmType = string.Empty;
        custom.TryGetValue("UploadFormType", out frmType);

        custom.Add("SicCode", app.SicCode);
        custom.Add("HighRiskDescriptor", GetDescriptorList(app));
        custom.Add("SicCodeDesc", app.SicCodeDesc);

        //Start Code added for PXP-12937
        custom.Add("VisaSicCode", app.VisaSicCode);
        custom.Add("VisaSicCodeDesc", app.VisaSicCodeDesc);
        //End code added for PXP-12937

        string txtRelType = getReleaseType(app.ReleaseMethodUID);

        custom.Add("ReleaseMethod", txtRelType);

        if (app.MonthendApproved)
            custom.Add("DiscountMethod", "Monthly");
        else
            custom.Add("DiscountMethod", "Daily");

        app.DelaysApproved = app.DelaysApproved == "-1" ? app.DelaysApproved.Replace("-1", "0") : app.DelaysApproved;
        custom.Add("DelaysApproved", string.IsNullOrWhiteSpace(app.DelaysApproved) ? "0" : app.DelaysApproved);
        custom.Add("ReservePercent", app.ReservePercent.ToString("0.00") + "%");
        custom.Add("UpfrontReserve", app.UpfrontReserve.ToString("c").Replace("$", ""));

        if (UserSessions.CurrentMerchantApp.DaysHoldTypeID == 1)
        {
            custom.Add("HoldArrearsType", "Calendar Days");
        }
        else if (UserSessions.CurrentMerchantApp.DaysHoldTypeID == 2)
        {
            custom.Add("HoldArrearsType", "Business Days");
        }

        if (objUW != null)
        {
            custom.Add("Reservepct", objUW.Reservepct.ToString("#0.00") + "%");
            custom.Add("FlagsOffUponBoarding", (objUW.FlagsOffUponBoarding.ToLower() == "true" ? "Yes" : "No"));
            custom.Add("DivertUponBoarding", (objUW.DivertUponBoarding.ToLower() == "true" ? "Yes" : "No"));
            custom.Add("BuyPassIndicator", (objUW.BuyPassIndicator.ToLower() == "true" ? "Yes" : "No"));
            if ((app.MerchantAppTypeUID.ToUpper() == Constants.BANK_CHESAPEAKE && app.SettlePlatformUID.ToUpper() == SettlementPlatforms.Tsys) ||
                    app.MerchantAppTypeUID.ToUpper() == Constants.BANK_BMO_HARRIS)
                    custom.Add("AgentLevel", string.IsNullOrEmpty(objUW.AssociationNumber) ? "100001" : objUW.AssociationNumber);
            else
                custom.Add("AgentLevel", objUW.AgentLevel);

            // populate the products
            LoadProducts(custom, app);

            if (objUW.PaymentFrequencyID > 0)
            {
                custom.Add("PaymentFrequency", GetDescription(((Underwriting.ePaymentFrequency)objUW.PaymentFrequencyID)));
            }

            if (objUW.PaymentScheduleID > 0)
            {
                custom.Add("PaymentSchedule", ((Underwriting.ePaymentSchedule)objUW.PaymentScheduleID).ToString());
            }

        }
        else
        {
            objUW = new Underwriting();
            custom.Add("FlagsOffUponBoarding", "No");
            custom.Add("DivertUponBoarding", "No");
            custom.Add("BuyPassIndicator", "No");
            custom.Add("AgentLevel", "No");
            custom.Add("Reservepct", "0.00 %");
        }

        if (app.ConditionalApproval)
        {
            StringBuilder sbcond = new StringBuilder();

            sbcond.AppendLine(string.Format("This application is conditionally approved with conditions due on {0}:", app.ConditionalDueDate.ToShortDateString()));

            // find pending condition
            IList<UWConditions> liUWC = DataMerchantApp.GetInstance().GetUWConditionsList(app.MerchantAppUID);

            if (liUWC != null)
            {
                foreach (UWConditions obj in liUWC)
                {
                    if (obj.ReceivedInfo == false)
                    {
                        sbcond.AppendLine(string.Format("* {0}", obj.ConditionName));
                        sbcond.AppendLine(string.Format("   - {0}", obj.EmailText));
                    }
                }
            }
        }
        // custom.Add("UWIssues","Special Requests: \r\n" + UserSessions.CurrentMerchantApp.SpecialRequest+ "\r\n \r\n"+ UserSessions.CurrentMerchantApp.UWNotes );

        custom.Add("UWIssues", System.Web.HttpUtility.HtmlDecode(UserSessions.CurrentMerchantApp.UWNotes));
        custom.Add("UWIssuesExt", UserSessions.CurrentMerchantApp.SpecialRequest);
        return objUW;
    }

    public static string GetDescription(Underwriting.ePaymentFrequency @enum)
    {
        if (@enum == null)
            return null;

        string description = @enum.ToString();

        try
        {
            FieldInfo fi = @enum.GetType().GetField(@enum.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                description = attributes[0].Description;
        }
        catch
        {
        }

        return description;
    }

    private static void LoadEquipments(IDictionary<string, string> custom)
    {
        EquipmentFacade facade = new EquipmentFacade();
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
        DataSet ds = facade.GetMerchantAppItem(prms);
        DataRow dr = null; StringBuilder sb = new StringBuilder();

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            dr = ds.Tables[0].Rows[i];
            int j = i + 1;

            if (DataLayer.Field2Bool(dr["IsEnabled"]))
            {
                sb.Append(j.ToString() + ") Type: " + dr["EquipmentType"].ToString());
                sb.Append(", Maker: " + dr["EquipmentMaker"].ToString());
                sb.Append(", Model: " + dr["Model"].ToString() + "; ");
            }
        }

        if (!string.IsNullOrWhiteSpace(sb.ToString()))
            custom.Add("Equipment", sb.ToString());
        else
            custom.Add("Equipment", "No Equipment Information");
    }

    private static void LoadProducts(IDictionary<string, string> custom, MerchantApp app)
    {
        // logic taken from wucServices.ascx.cs -> LoadServices()
        List<string> liP = new List<string>();
        DataMerchantServices data1 = new DataMerchantServices();
        Hashtable prms1 = new Hashtable();
        IList<MerchantServices> services1 = new List<MerchantServices>();
        prms1.Add("@MerchantAppUID", DataLayer.UID2Field(app.MerchantAppUID));
        prms1.Add("@CategoryID", CommonUtility.Util.if_s((int)ServiceCategories.CREDITPRODUCTS));
        services1 = data1.GetMerchantServicesWithoutProducts(prms1);

        foreach (MerchantServices service in services1)
        {
            if (service.Checked)
            {
                liP.Add(service.Name);
            }
        }

        custom.Add("ProductsRequired", string.Join(", ", liP));
    }

    public static string GetDescriptorList(MerchantApp ma)
    {
        DataSet dsDescr = DataAccess.DataMerchantAppDao.GetMerchantDescriptors(ma.ID);
        List<string> liDescr = new List<string>();
        if (dsDescr != null && dsDescr.Tables.Count > 0 && dsDescr.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow drDescr in dsDescr.Tables[0].Rows)
            {
                string d = drDescr["Descriptor"].ToString();
                if (ma.Brand == MerchantBrand.Optimal && !string.IsNullOrWhiteSpace(drDescr["Descriptor2"].ToString()))
                {
                    d += ", " + drDescr["Descriptor2"].ToString();
                }
                if (!liDescr.Contains(d))
                {
                    // don't add duplicates
                    liDescr.Add(d);
                }
            }
        }

        return string.Join("; ", liDescr.ToArray());
    }

    #region Not Referenced Methods
    /// <summary>
    /// Gets the body template for merchant portal username 
    /// </summary>
    /// <param name="DBA"></param>
    /// <param name="UserName"></param>
    /// <param name="objPrivateLabel"></param>
    /// <returns></returns>
    public static string GetWelcomeEmailPXP(string DBA, string UserName, PrivateLabel objPrivateLabel)
    {
        Dictionary<string, string> di = new Dictionary<string, string>();

        di["company"] = "Paysafe Group plc";
        di["infoUrl"] = "<a href='mailto:ClientServices@paysafe.com'>ClientServices@paysafe.com</a>";
        di["CSPhone"] = "1-888-851-7558";
        di["Url"] = "<a href='https://www.paymentxp.com'>https://www.paymentxp.com</a>";
        di["image"] = "https://www.paymentxp.com/applicationxp/images/PaysafeBannerHeader.jpg";
        di["comp"] = "Paysafe";
        di["UserName"] = UserName;
        di["DBA"] = DBA;
        di["portal"] = "Payment XP";
        di["otherPortal"] = "Insight";

        return GetPortalsUserName(di, objPrivateLabel, Constants.PORTAL_PAYMENTXP);

    }

    /// <summary>
    /// generates the body template for PXP Password
    /// </summary>
    /// <param name="Password"></param>
    /// <param name="objPrivateLabel"></param>
    /// <param name="login_type"></param>
    /// <returns></returns>
    public static string GetPXPPassword(string Password, string GatewayKey, PrivateLabel objPrivateLabel)
    {
        StringBuilder sb = new StringBuilder();

        string company = "Paysafe Group plc";
        string infoUrl = "<a href='mailto:ClientServices@paysafe.com'>ClientServices@paysafe.com</a>";
        string CSPhone = "1-888-851-7558";
        string Url = "<a href='https://www.paymentxp.com'>https://www.paymentxp.com</a>";
        string image = "https://www.paymentxp.com/applicationxp/images/PaysafeBannerHeader.jpg";
        string comp = "Paysafe";
        string portal = "Payment XP";
        string otherPortal = "Insight ";

        if (objPrivateLabel != null)
        {
            company = objPrivateLabel.PLCompanyName;
            infoUrl = "<a href='mailto:" + objPrivateLabel.PLEmail + "'>" + objPrivateLabel.PLEmail + "</a>";
            CSPhone = objPrivateLabel.PLPhone;
            Url = "<a href='" + objPrivateLabel.VTURL + "'>" + objPrivateLabel.VTURL + "</a>";
            if (!string.IsNullOrWhiteSpace(objPrivateLabel.PLHeaderLogo))
                image = "https://www.paymentxp.com/applicationxp/images/" + objPrivateLabel.PLHeaderLogo;
            else
                image = string.Empty;

            comp = objPrivateLabel.PLCompanyName;

            portal = "Virtual Terminal";
        }

        if (!string.IsNullOrWhiteSpace(image))
            sb.Append("<span style='font-size:22.0pt;font-family:Century Gothic;color:#024c8b'><img src='" + image + "' alt='' /></span><br><br>");
        else
            sb.Append("<span style='color:#024c8b;font-size:22.0pt;font-family:Century Gothic;'>" + company + "</span><br><br>");

        sb.Append("<p><span style='font-size:11.0pt;font-family:Century Gothic;'>Your temporary <b>" + portal + "</b> password is below.</span></p><br>");
        sb.Append("<p>");
        sb.Append("<span style='font-size:11.0pt;font-family:Century Gothic;'><b>Password:</b> " + Password + "</span><br><br>");
        sb.Append("<span style='font-size:11.0pt;font-family:Century Gothic;'><b>Payment XP:</b> " + Url + "</span><br><br>");
        sb.Append("<span style='font-size:11.0pt;font-family:Century Gothic;'><b>Gateway Key:</b> " + GatewayKey + "</span><br><br><br>");
        sb.Append("<p><span style='font-size:11.0pt;font-family:Century Gothic;'>Please note, if you will be using our <b>" + otherPortal + "</b>merchant website your login information will be the same as your " + portal + " gateway login information. If you would like to schedule a training session with one of our " + portal + " experts please contact " + infoUrl + " today.</span></p><br><br>");
        sb.Append("<p><span style='font-size:11.0pt;font-family:Century Gothic;'>Thank you for choosing " + company + ".</span></p><br>");
        sb.Append("<br>");
        sb.Append("<span style='font-size:8.0pt;font-family:Century Gothic;'>This is an auto generated email. If you have any questions or comments, please contact us at " + infoUrl + ", or call us at " + CSPhone + ".<br><br><br>");

        return sb.ToString();
    }
    #endregion

    #endregion

    #region ASCX Methods

    #region _ COMMON
    public static bool SendEmail(string strSubject, string strBody, string strBodyHTML, string strFrom, string strTo, string strCC, string strBCC, Hashtable attachments, string MerchantAppUID)
    {
        bool perform = false;
        try
        {
            perform = MerchantFacade.SendEmail(strSubject, strBody, strBodyHTML, strFrom, strTo, strCC, strBCC, attachments, MerchantAppUID, UserSessions.CurrentUser.UserName);
            if (perform)
            {
                ZeusWeb.Logging.EmailLog.InfoFormat("Email sent for EmailSubject: {0} Email to: {1}", strSubject, strTo);
                ZeusWeb.Logging.EmailLog.InfoFormat("Email sent for MerchantAppUID: {0} username: {1}", MerchantAppUID, UserSessions.CurrentUser.UserName);
            }
            else
            {
                ZeusWeb.Logging.EmailLog.InfoFormat("Error while sending Email for EmailSubject: {0} Email to: {1}", strSubject, strTo);
                ZeusWeb.Logging.EmailLog.InfoFormat("Error while sending Email for MerchantAppUID: {0} username: {1}", MerchantAppUID, UserSessions.CurrentUser.UserName);
            }
        }
        catch (Exception ex)
        {
            ZeusWeb.Logging.EmailLog.ErrorFormat("Error while sending an email for MerchantAppUID:{0} which having EmailSubject:{1}", MerchantAppUID, strSubject);
            ZeusWeb.Logging.EmailLog.ErrorFormat("Message:{0}", ex.Message);
            ZeusWeb.Logging.EmailLog.ErrorFormat("StackTrace:{0}", ex.StackTrace);
            perform = false;
        }
        return perform;
    }
    #endregion

    #region _ wucCreateUser Control

    public static string GetWelcomeEmailPassword(string Portal, string Password, string PrivateLabelUID)
    {
        StringBuilder sb = new StringBuilder();
        //string url = "https://www.paymentxp.com";
        string company = "Paysafe Group plc";
        string CSPhone = "1-888-851-7558.";
        string infoUrl = "<a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>";

        if (!string.IsNullOrWhiteSpace(PrivateLabelUID))
        {
            PrivateLabel objPrivateLabel = DataAccess.DataMerchantAppDao.GetPrivateLabel(PrivateLabelUID);

            company = objPrivateLabel.PLCompanyName;
            infoUrl = "<a href='" + objPrivateLabel.PLEmail + "'>" + objPrivateLabel.PLEmail + "</a>";
            CSPhone = objPrivateLabel.PLPhone;
            Portal = objPrivateLabel.PLCompanyName;
        }

        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>" + company + "</span><br>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing " + company + ".</span></p>");
        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'><b>Login Information:</b><br>You can access the " + Portal + " by using the following information..</span></p>");

        sb.Append("<p>");
        sb.Append("<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Password: </span>" + Password + "</p>");

        sb.Append("<p><span style='font-size:10.0pt;font-family:Verdana;'>As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at " + infoUrl + ", or call us at " + CSPhone + "</span></p>");
        sb.Append("<br>");
        sb.Append("<br>");

        return sb.ToString();
    }

    public static string GetWelcomeEmailPassword(string Portal, string Password, PrivateLabel objPrivateLabel, string login_type)
    {
        return GetWelcomeEmailPassword(Portal, Password, objPrivateLabel, login_type, eEmailPasswordType.Password);
    }

    public static string GetWelcomeEmailPassword(string Portal, string Password, PrivateLabel objPrivateLabel, string login_type, eEmailPasswordType ept)
    {
        Dictionary<string, string> di = new Dictionary<string, string>();

        string pw_text = "";

        switch (ept)
        {
            case eEmailPasswordType.MerchantKey:
                pw_text = "Merchant Key";
                break;

            default:
                pw_text = "Gateway Password";
                break;
        }


        if (Portal.ToUpper().Contains("PAYMENTXP"))
        {
            di["url_text"] = "Website Login URL: ";
            di["url"] = "https://www.paymentxp.com";
        }
        else if (Portal.ToUpper().Contains("MERCHANT"))
        {
            di["url_text"] = "Merchant Website Login URL: ";
            di["url"] = "https://www.paymentXP.com/merchants/";
        }

        di["company"] = "Paysafe Group plc";
        di["CSPhone"] = "1-888-851-7558.";
        di["infoUrl"] = "<a href='Irvine.salessupport@paysafe.com'>Irvine.salessupport@paysafe.com</a>";
        di["Portal"] = Portal;
        di["Password"] = Password;
        di["pw_text"] = pw_text;

        if (objPrivateLabel != null)
        {
            di["url"] = objPrivateLabel.VTURL;
            di["company"] = objPrivateLabel.PLCompanyName;
            di["infoUrl"] = "<a href='" + objPrivateLabel.PLEmail + "'>" + objPrivateLabel.PLEmail + "</a>";
            di["CSPhone"] = objPrivateLabel.PLPhone;
            if (Portal.ToUpper().Contains("PAYMENTXP"))
                di["Portal"] = Portal.Replace("PaymentXP", "Virtual Terminal");
            else if (Portal.ToUpper().Contains("Paysafe"))
                di["Portal"] = Portal.Replace("Paysafe", objPrivateLabel.PLCompanyName);
        }


        // special rule, for agent id 1956 (Dennis Ideue), we send them the Merchant Pin, in addition to the merchant key
        if (login_type.ToUpper().Contains("MERCHANT")
             && (UserSessions.CurrentMerchantApp != null
             && (UserSessions.CurrentMerchantApp.AgentUID.Trim().ToUpper() == "95A056DD-30FF-4B9D-82BA-95D44AA0E8C4" || UserSessions.CurrentMerchantApp.HasMerchantPin)))
        {
            di["PIN"] = string.Format("<p><span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>Mobile Password: </span>{0}</p>", UserSessions.CurrentMerchantApp.MerchantPIN);
        }

        string str = string.Empty;

        if (di.ContainsKey("url"))
            str = "<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>[[url_text]]: </span>[[url]] <br/>";

        string template = @"
        <span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>[[company]]</span>
        <br>
        <p>
            <span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing [[company]].</span>
        </p>
        <p>
            <span style='font-size:10.0pt;font-family:Verdana;'><b>Login Information:</b><br>You can access [[Portal]] by using the following information.</span>
        </p>
        <p>"
         + str +
        @"<span style='font-size:9.0pt;font-family:Verdana;color:#CE3201;font-weight:bold'>[[pw_text]]: </span>[[Password]]
        </p> 
        [[PIN]]
        <p>
            <span style='font-size:10.0pt;font-family:Verdana;'>As always, customer service is our number one priority. Please let us know if we can be of assistance. If you have any questions or comments, please contact us directly at [[infoUrl]], or call us at [[CSPhone]]</span>
        </p>
        <br>
        <br>";

        string body = "";
        CommonUtility.Template t = new Template();

        if (t.getTemplateAndFill_FromString(template, di, ref body))
        {
            return body;
        }
        else
        {
            return "";
        }


    }

    public static bool SendEmail(string strSubject, string strBody, string strBodyHTML, string strFrom, string strTo, string strCC, string strBCC, Hashtable attachments, string MerchantAppUID, string AgentUID)
    {
        bool perform = false;
        try
        {
            perform = MerchantFacade.SendEmail(strSubject, strBody, strBodyHTML, strFrom, strTo, strCC, strBCC, attachments, MerchantAppUID, UserSessions.CurrentUser.UserName, AgentUID);
            if (perform)
            {
                ZeusWeb.Logging.EmailLog.InfoFormat("Email sent for EmailSubject: {0} Email to: {1}", strSubject, strTo);
                ZeusWeb.Logging.EmailLog.InfoFormat("Email sent for MerchantAppUID: {0} username: {1}", MerchantAppUID, UserSessions.CurrentUser.UserName);
            }
            else
            {
                ZeusWeb.Logging.EmailLog.InfoFormat("Error while sending Email for EmailSubject: {0} Email to: {1}", strSubject, strTo);
                ZeusWeb.Logging.EmailLog.InfoFormat("Error while sending Email for MerchantAppUID: {0} username: {1}", MerchantAppUID, UserSessions.CurrentUser.UserName);
            }
        }
        catch (Exception ex)
        {
            ZeusWeb.Logging.EmailLog.ErrorFormat("Error while sending an email for MerchantAppUID:{0} which having EmailSubject:{1}", MerchantAppUID, strSubject);
            ZeusWeb.Logging.EmailLog.ErrorFormat("Message:{0}", ex.Message);
            ZeusWeb.Logging.EmailLog.ErrorFormat("StackTrace:{0}", ex.StackTrace);
            perform = false;
        }
        return perform;
    }

    /// <summary>
    /// generates the body template for merchant portal Password
    /// </summary>
    /// <param name="Password"></param>
    /// <param name="objPrivateLabel"></param>
    /// <param name="login_type"></param>
    /// <returns></returns>
    public static string GetMerchantWebsitePassword(string Password, PrivateLabel objPrivateLabel, string login_type)
    {
        StringBuilder sb = new StringBuilder();

        string company = "Paysafe Group plc";
        string infoUrl = "<a href='mailto:ClientServices@paysafe.com'>ClientServices@paysafe.com</a>";
        string CSPhone = "1-888-851-7558";
        string MerUrl = "<a href='https://www.paymentXP.com/merchants'>https://www.paymentXP.com/merchants</a>";
        string image = "https://www.paymentxp.com/applicationxp/images/PaysafeBannerHeader.jpg";
        string comp = "Paysafe";

        if (objPrivateLabel != null)
        {
            company = objPrivateLabel.PLCompanyName;
            infoUrl = "<a href='mailto:" + objPrivateLabel.PLEmail + "'>" + objPrivateLabel.PLEmail + "</a>";
            CSPhone = objPrivateLabel.PLPhone;
            MerUrl = "<a href='" + objPrivateLabel.MerchantPortalUrl + "'>" + objPrivateLabel.MerchantPortalUrl + "</a>";
            if (!string.IsNullOrWhiteSpace(objPrivateLabel.PLHeaderLogo))
                //Modify for PXP-8058 by Anuj kumar
                image = "https://www.paymentxp.com/applicationxp/images/" + objPrivateLabel.PLHeaderLogo;
            else
                image = string.Empty;

            comp = objPrivateLabel.PLCompanyName;
        }

        if (!string.IsNullOrWhiteSpace(image))
            sb.Append("<span style='font-size:22.0pt;font-family:Century Gothic;color:#024c8b'><img src='" + image + "' alt='' /></span><br><br>");
        else
            sb.Append("<span style='color:#024c8b;font-size:22.0pt;font-family:Century Gothic;'>" + company + "</span><br><br>");

        sb.Append("<p><span style='font-size:11.0pt;font-family:Century Gothic;'>Your temporary <b>Insight</b> password is below.</span></p><br>");
        sb.Append("<p>");
        sb.Append("<span style='font-size:11.0pt;font-family:Century Gothic;'><b>Insight Password:</b> " + Password + "</span><br><br>");
        sb.Append("<span style='font-size:11.0pt;font-family:Century Gothic;'><b>Insight:</b> " + MerUrl + "</span><br><br>");

        // special rule, for agent id 1956 (Dennis Ideue), we send them the Merchant Pin, in addition to the merchant key
        if (login_type.ToUpper().Contains("MERCHANT")
             && (UserSessions.CurrentMerchantApp != null
             && (UserSessions.CurrentMerchantApp.AgentUID.Trim().ToUpper() == "95A056DD-30FF-4B9D-82BA-95D44AA0E8C4" || UserSessions.CurrentMerchantApp.HasMerchantPin)))
        {
            sb.Append(string.Format("<span style='font-size:11.0pt;font-family:Century Gothic;'><b>Mobile Password:</b> {0}</span><br><br>", UserSessions.CurrentMerchantApp.MerchantPIN));
        }

        sb.Append("<br>");
        sb.Append("<p><span style='font-size:11.0pt;font-family:Century Gothic;'>Please note, if you will be using our <b>Payment XP</b> gateway your login information will be the same as your Insight login information. If you would like to schedule a training session with one of our Insight experts please contact " + infoUrl + " today.</span></p><br><br>");
        sb.Append("<p><span style='font-size:11.0pt;font-family:Century Gothic;'>Thank you for choosing " + company + ".</span></p><br>");
        sb.Append("<br>");
        sb.Append("<span style='font-size:8.0pt;font-family:Century Gothic;'>This is an auto generated email. If you have any questions or comments, please contact us at " + infoUrl + ", or call us at " + CSPhone + ".<br><br><br>");

        return sb.ToString();
    }

    // Email templates for the login inforamtion of portals insight & PXP

    /// <summary>
    /// Gets the body template for merchant portal username 
    /// </summary>
    /// <param name="DBA"></param>
    /// <param name="UserName"></param>
    /// <param name="objPrivateLabel"></param>
    /// <returns></returns>
    public static string GetWelcomeEmailMerchant(string DBA, string UserName, PrivateLabel objPrivateLabel)
    {
        Dictionary<string, string> di = new Dictionary<string, string>();

        di["company"] = "Paysafe Group plc";
        di["infoUrl"] = "<a href='mailto:ClientServices@paysafe.com'>ClientServices@paysafe.com</a>";
        di["CSPhone"] = "1-888-851-7558";
        di["Url"] = "<a href='https://www.paymentXP.com/merchants'>https://www.paymentXP.com/merchants</a>";
        di["image"] = "https://www.paymentxp.com/applicationxp/images/PaysafeBannerHeader.jpg";
        di["comp"] = "Paysafe";
        di["UserName"] = UserName;
        di["DBA"] = DBA;
        di["portal"] = "Insight";
        di["otherPortal"] = "Payment XP";
        return GetPortalsUserName(di, objPrivateLabel, Constants.PORTAL_MERCHANT);

    }

    /// <summary>
    /// generates body template for merchant/PXP portal username 
    /// </summary>
    /// <param name="DBA"></param>
    /// <param name="UserName"></param>
    /// <param name="objPrivateLabel"></param>
    /// <returns>body template string</returns>
    public static string GetPortalsUserName(Dictionary<string, string> di, PrivateLabel objPrivateLabel, string portal)
    {

        StringBuilder sb = new StringBuilder();

        if (objPrivateLabel != null)
        {
            di["company"] = objPrivateLabel.PLCompanyName;
            di["infoUrl"] = "<a href='mailto:" + objPrivateLabel.PLEmail + "'>" + objPrivateLabel.PLEmail + "</a>";
            di["CSPhone"] = objPrivateLabel.PLPhone;
            if (!string.IsNullOrWhiteSpace(objPrivateLabel.PLHeaderLogo))
                //Modify for PXP-8058 by Anuj kumar
                di["image"] = "https://www.paymentxp.com/applicationxp/images/" + objPrivateLabel.PLHeaderLogo;
            else
                di["image"] = string.Empty;

            di["comp"] = objPrivateLabel.PLCompanyName;

            if (portal == Constants.PORTAL_MERCHANT)
            {
                di["Portal"] = "Insight";
                di["otherPortal"] = "Virtual Terminal";
                di["Url"] = "<a href='" + objPrivateLabel.MerchantPortalUrl + "'>" + objPrivateLabel.MerchantPortalUrl + "</a>";

            }
            else if (portal == Constants.PORTAL_PAYMENTXP)
            {
                di["portal"] = "Virtual Terminal";
                di["otherPortal"] = "Insight";
                di["Url"] = "<a href='" + objPrivateLabel.VTURL + "'>" + objPrivateLabel.VTURL + "</a>";

            }

        }


        if (portal == Constants.PORTAL_MERCHANT)
        {
            di["PInfo"] = "Welcome to " + di["comp"] + "! You now have access to <b>" + di["portal"] + "</b>, our proprietary merchant portal. " + di["portal"] + " is a web-based account management system that allows you to view net processing, transaction details, batch summaries, and more.";
        }
        else if (portal == Constants.PORTAL_PAYMENTXP)
        {
            di["PInfo"] = "Welcome to " + di["comp"] + "! You now have access to <b>" + di["portal"] + "</b>, our proprietary gateway. This global payment platform will allow you to accept multiple forms of payment and provide in depth reporting.";
        }

        di["OPInfo"] = "Please note, if you will be using our <b>" + di["otherPortal"] + "</b> your login information will be the same as your " + di["portal"] + " login information. ";

        // first step of the email template is add the image if available or jsut show the company name
        if (!string.IsNullOrWhiteSpace(di["image"]))
            sb.Append("<span style='font-size:22.0pt;font-family:Century Gothic;color:#024c8b'><img src='" + di["image"] + "' alt='' /></span><br><br>");
        else
            sb.Append("<span style='color:#024c8b;font-size:22.0pt;font-family:Century Gothic;'>" + di["company"] + "</span><br><br>");

        //this describes the portal for which the email is intended to
        sb.Append("<p><span style='font-size:11.0pt;font-family:Century Gothic;'>" + di["PInfo"] + "</span></p>");
        sb.Append("<p><span style='font-size:11.0pt;font-family:Century Gothic;'>You can access <b>" + di["portal"] + "</b> by using the following information:</span></p><br>");

        //this describes the url of the portal
        sb.Append("<p><span style='font-size:11.0pt;font-family:Century Gothic;'><b>" + di["portal"] + ":</b> " + di["Url"] + "</span><br>");

        //this line is for the username of the portal
        sb.Append("<span style='font-size:11.0pt;font-family:Century Gothic;'><b>Login ID:</b> " + di["UserName"] + "</span><br>");

        //this line states that passowrd will be sent in next email
        sb.Append("<span style='font-size:11.0pt;font-family:Century Gothic;'><b>Password:</b> " + "For security, your password will be sent in a separate email.</span><br>");

        //this line is for the DBA
        sb.Append("<span style='font-size:11.0pt;font-family:Century Gothic;'><b>DBA:</b> </span>" + di["DBA"] + ".</p><br>");

        //this line describes teh portals info
        sb.Append("<p><span style='font-size:11.0pt;font-family:Century Gothic;'>" + di["OPInfo"] + " If you would like to schedule a training session with one of our " + di["portal"] + " experts please contact " + di["infoUrl"] + " today.</span></p><br><br>");
        sb.Append("<p><span style='font-size:11.0pt;font-family:Century Gothic;'>Thank you for choosing " + di["company"] + ".</span></p><br>");
        sb.Append("<br>");
        sb.Append("<span style='font-size:8.0pt;font-family:Century Gothic;'>This is an auto generated email. If you have any questions or comments, please contact us at " + di["infoUrl"] + ", or call us at " + di["CSPhone"] + ".<br><br><br>");

        return sb.ToString();

    }

    #endregion

    #region _ wucBusinessInfo Control
    /// <summary>
    /// this method is used to make aall child controls enable or disable
    /// </summary>
    /// <param name="pnlGeneralInfo"></param>
    /// <param name="p"></param>
    public static void SetControlTrueEditMode(Control container, bool EditMode)
    {
        foreach (Control control in container.Controls)
        {
            if (control != null)
            {
                if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;
                    listControl.Enabled = EditMode;
                }
                else if (control is TextBox)
                {
                    TextBox txt = (TextBox)control;
                    txt.ReadOnly = !EditMode;
                    txt.Enabled = EditMode;
                }
                else if (control is CheckBox)
                {
                    CheckBox chk = (CheckBox)control;
                    chk.Enabled = EditMode;
                }
                else if (control is Button)
                {
                    Button btn = (Button)control;
                    btn.Enabled = EditMode;
                }
                else if (control is WebTextEditor)
                {
                    WebTextEditor txt = (WebTextEditor)control;
                    txt.ReadOnly = !EditMode;
                    txt.Enabled = EditMode;
                }
                else if (control is WebMaskEditor)
                {
                    WebMaskEditor txt = (WebMaskEditor)control;
                    txt.ReadOnly = !EditMode;
                    txt.Enabled = EditMode;
                }
                else if (control is WebDateTimeEditor)
                {
                    WebDateTimeEditor txt = (WebDateTimeEditor)control;
                    txt.ReadOnly = !EditMode;
                    txt.Enabled = EditMode;
                }
                else if (control is WebDateChooser)
                {
                    WebDateChooser txt = (WebDateChooser)control;
                    txt.ReadOnly = !EditMode;
                    txt.Enabled = EditMode;
                }
                else if (control is WebNumericEditor)
                {
                    WebNumericEditor txt = (WebNumericEditor)control;
                    txt.ReadOnly = !EditMode;
                    txt.Enabled = EditMode;
                }
                else if (control is WebCurrencyEditor)
                {
                    WebCurrencyEditor txt = (WebCurrencyEditor)control;
                    txt.ReadOnly = !EditMode;
                    txt.Enabled = EditMode;
                }
                else if (control is WebPercentEditor)
                {
                    WebPercentEditor txt = (WebPercentEditor)control;
                    txt.ReadOnly = !EditMode;
                    txt.Enabled = EditMode;
                }
                else if (control is WebHtmlEditor)
                {
                    WebHtmlEditor txt = (WebHtmlEditor)control;
                    txt.ReadOnly = !EditMode;
                    txt.Enabled = EditMode;
                }
                else if (control is FileUpload)
                {
                    FileUpload txt = (FileUpload)control;
                    txt.Enabled = EditMode;
                }
                else if (control is GridView)
                {
                    GridView grd = (GridView)control;
                    grd.Enabled = EditMode;
                }
                if (control.HasControls())
                {
                    SetControlTrueEditMode(control, EditMode);
                }
            }
        }
    }
    #endregion

    #region _ wucTicket
    public static bool FormChanged(object current, object original)
    {
        if (original == null || current == null)
            return false;

        Type type = original.GetType();
        PropertyInfo[] props = type.GetProperties();


        Type type2 = current.GetType();
        PropertyInfo[] props2 = type2.GetProperties();
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < props.Length; i++)
        {
            object propValue1 = props[i].GetValue(original, null);
            object propValue2 = props2[i].GetValue(current, null);
            string s1 = string.Empty;
            string s2 = string.Empty;

            if (props[i].PropertyType == typeof(string))
            {
                s1 = propValue1 == null ? string.Empty : propValue1.ToString().Trim();
                s2 = propValue2 == null ? string.Empty : propValue2.ToString().Trim();

                if (props[i].Name.ToUpper().IndexOf("UID") != -1 && s2 == "-1")
                    s2 = string.Empty;
            }

            if (props[i].PropertyType == typeof(decimal))
            {
                s1 = propValue1 == null ? string.Empty : Convert.ToDecimal(propValue1).ToString("###,##0.00");
                s2 = propValue2 == null ? string.Empty : Convert.ToDecimal(propValue2).ToString("###,##0.00");
            }

            if (props[i].PropertyType == typeof(int))
            {
                s1 = propValue1 == null ? string.Empty : Convert.ToInt32(propValue1).ToString();
                s2 = propValue2 == null ? string.Empty : Convert.ToInt32(propValue2).ToString();
            }

            if (props[i].PropertyType == typeof(bool))
            {
                s1 = propValue1 == null ? string.Empty : Convert.ToBoolean(propValue1).ToString();
                s2 = propValue2 == null ? string.Empty : Convert.ToBoolean(propValue2).ToString();
            }

            if (s1 != s2)
            {
                sb.Append(props[i].Name + ": " + s1 + ", " + s2 + " | ");
            }
        }

        return (!string.IsNullOrWhiteSpace(sb.ToString()));

    }

    public static void LogTicketFormChanges(string DBA, string uid, string id, object original, object current, string notes)
    {
        if (original == null || current == null)
            return;

        Type type = original.GetType();

        if (string.IsNullOrWhiteSpace(notes))
        {
            LogFormChanges(DBA, uid, CommonUtility.Util.if_i(id, 0), original, current);
        }
        else
        {
            DataUser data = DataAccess.DataUserDao;
            data.InsertChangeLog(DBA, UserSessions.CurrentUser.UserName, uid, CommonUtility.Util.if_i(id, 0).ToString(), type.Name, notes, Constants.PORTAL_ZEUS);
        }
    }
    #endregion

    #region _ wucConditions
    //Add new application tickets from different pages.
    public static void AddTicket(MerchantApp app, string notes)
    {
        DataTicket data = new DataTicket();
        Ticket ticket = new Ticket();
        IList<Ticket> list = null;
        User user = UserSessions.CurrentUser;

        Hashtable prms = new Hashtable();

        prms.Add("@MerchantUID", app.MerchantAppUID);
        prms.Add("@LastTicketOnly", 1);

        list = data.GetLastNewApplicationTicket(prms);

        if (list == null || list.Count == 0 || (list.Count > 0 && list[0].StatusID.ToUpper() == Ticket.TICKET_CLOSE))
        {

            List<User> ActiveUsers = (List<User>)LookupTableHandler.m_ActiveUsers;
            bool isActive = ActiveUsers.Exists(x => x.UID == app.PrimaryContactUID);
            DateTime dt = DateTime.Today.AddDays(1);

            ticket = new Ticket();
            ticket.TicketSource = "i";
            ticket.Origin = 4;
            ticket.ZID = app.ID;
            ticket.MerchantAppUID = app.MerchantAppUID;
            ticket.AgentUID = app.AgentUID;
            ticket.DepartmentID = "8";
            ticket.CategoryID = "898";
            ticket.StatusID = isActive ? Ticket.TICKET_ASSIGNED : Ticket.TICKET_OPEN;
            ticket.UserID = isActive ? app.PrimaryContactUID : null;
            ticket.TimeZone = "6";
            ticket.Priority = "Low";
            ticket.Problem = "New application created. Update notes with all actions taken on this account.";

            ticket.UserCreated = "System";
            ticket.DateCreated = DateTime.Now;
            ticket.UserCreatedUserUID = null;
            ticket.UserModified = "System";
            ticket.DueDate = new DateTime(dt.Year, dt.Month, dt.Day, 18, 0, 0).AddDays(29);
            ticket.OfficeID = app.Office.GetHashCode();

            data.InsertTicket(ticket);

            PaymentXP.Facade.TicketNotification.NewTicketCreated(ticket.TicketUID, false, true);
            ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for ID : {0} .", ticket.TicketUID);
            //PXP-2955 Rohit Thakur
            InsertMerchantNotesForTicket(app.MerchantAppUID, user.UserName, ticket);
        }
        else if (list.Count > 0)
        {
            ticket = list[0];
        }

        if (ticket != null)
        {
            if (string.IsNullOrWhiteSpace(notes))
            {
                ticket.StatusID = Ticket.TICKET_CLOSE;
            }

            ticket.UserModified = user.UserName;
            ticket.DateModified = DateTime.Now;

            data.UpdateTicket(ticket);

            if (ticket.StatusID == Ticket.TICKET_CLOSE)
            {
                PaymentXP.Facade.TicketNotification.TicketClosed(ticket.TicketUID, false);
            }

            if (!string.IsNullOrWhiteSpace(notes))
            {
                TicketNotes note = new TicketNotes();
                note.DateCreated = DateTime.Now;
                note.Description = notes;
                note.IsInternal = true;
                note.TicketID = ticket.TicketID;
                note.UserCreated = user.UserName;

                DataAccess.DataTicketNotesDao.InsertTicketNotes(note);
            }
        }

    }
    #endregion

    #endregion


    //PXP-2955 Rohit Thakur
    /// <summary>
    /// Add MerchantNotes for Ticket
    /// </summary>
    /// <param name="merchantAppUID"></param>
    /// <param name="purchaserUserName"></param>
    /// <param name="portal"></param>
    /// <param name="note"></param>
    /// <param name="departmentID"></param>
    public static void InsertMerchantNotesForTicket(string merchantAppUID, string userName, Ticket ticket)
    {
        string SubjectForNoteTemplate = "Department:{0}; Category:{1}; Subcategory:{2}";
        string subject = string.Empty;

        Hashtable tDeptParms = new Hashtable();
        tDeptParms.Add("@DeptID", ticket.DepartmentID);
        IList<TicketDepartment> deptList = CachedLookupFacade.GetCachedTicketDepartmentList(tDeptParms);

        Hashtable tCatParms = new Hashtable();
        tCatParms.Add("@CategoryID", ticket.CategoryID);
        IList<TicketCategories> categList = CachedLookupFacade.GetCachedTicketCategoryList(tCatParms);
        Hashtable tParentCatParms = new Hashtable();
        tParentCatParms.Add("@CategoryID", categList.FirstOrDefault().ParentID); //PXP-3675
        IList<TicketCategories> parentCateg = CachedLookupFacade.GetCachedTicketCategoryList(tParentCatParms);

        subject = String.Format(SubjectForNoteTemplate, deptList.FirstOrDefault().Name, parentCateg.FirstOrDefault().Name, categList.FirstOrDefault().Name);

        MerchantNotes ObjMerchantNotes = new MerchantNotes();
        ObjMerchantNotes.MerchantAppUID = merchantAppUID;
        ObjMerchantNotes.Subject = subject;
        ObjMerchantNotes.Notes = ticket.Problem;
        ObjMerchantNotes.NoteID = string.Empty; //TODO confirm if Empty is OK
        ObjMerchantNotes.View_Agent = true;
        ObjMerchantNotes.View_Bank = true;
        ObjMerchantNotes.View_MPSAll = true;
        ObjMerchantNotes.Email_Agent = false;
        ObjMerchantNotes.UserCreated = userName;
        ObjMerchantNotes.DateCreated = DateTime.Now;
        if (!string.IsNullOrEmpty(ticket.TicketID))
        {
            ObjMerchantNotes.TicketID = ticket.TicketID.Substring(6);
        }

        DataAccess.DataMerchantAppDao.InsertMerchantNotes(ObjMerchantNotes);
    }


    #region RiskParameter metods
    //Added by Koshlendra for PXP-2023,PXP-3529: Zeus: Set default Risk parameters for PaymentXP merchants Start  
    /// <summary>
    /// added to Get MCC for Getting risk parameter
    /// </summary>
    /// <returns></returns>
    public static string GetMerchantRiskMCC(MerchantApp origApp)
    {
        string mcc = string.Empty;
        try
        {
            bool perform = CheckRiskData(origApp);
            return GetMcc(perform, origApp);



        }
        catch (Exception)
        {

            throw;
        }

    }
    //Added by Koshlendra for PXP-2023,PXP-3529: Zeus: Set default Risk parameters for PaymentXP merchants End  
    //Added by Koshlendra for PXP-3529- Zeus: Set default Risk parameters for Active merchants start

    public static string GetMcc(bool perform, MerchantApp origApp)
    {
        string mcc = string.Empty;
        if (perform)
        {
            if (!string.IsNullOrWhiteSpace(origApp.SicCode))
            {
                mcc = Convert.ToString(origApp.SicCode);
            }
            else
            {
                mcc = Constants.DefaultMCC;
            }

        }
        else
        {
            mcc = Constants.NaMCC;
        }
        return mcc;
    }
    public static bool CheckRiskData(MerchantApp origApp)
    {
        bool perform = false;
        bool ispaymentXPMerchant = false;
        try
        {
            if (origApp.Office == CommonUtility.Util.Offices.Irvine && origApp.Brand == MerchantBrand.Meritus)
            {

                EquipmentFacade facade = new EquipmentFacade();
                Hashtable prms = new Hashtable();
                prms.Add("@MerchantAppUID", origApp.MerchantAppUID);
                prms.Add("@IsEnabled", true);
                DataSet ds = facade.GetMerchantAppItem(prms);

                foreach (DataRow rs in ds.Tables[0].Rows)
                {
                    //Ani: DM-5589
                    if (rs["EquipmentType"].ToString() == "Gateway" && rs["Model"].ToString() == "Payment XP")
                        ispaymentXPMerchant = true;
                }

                string achSatausName = string.Empty;
                if (UserSessions.ActiveAchMerchant != null)
                    achSatausName = ((UserSessions.ActiveAchMerchant != null)) ? UserSessions.ActiveAchMerchant.MerchantStatusName.ToUpper() : achSatausName;
                bool appACHStatus = String.IsNullOrEmpty(achSatausName) ? false : achSatausName.Substring(0, 2).Equals(Constants.QueueMS_Status);

                string statusName = string.Empty;
                statusName = ((origApp != null)) ? origApp.StatusName.ToUpper() : statusName;
                bool appStatus = String.IsNullOrEmpty(statusName) ? false : statusName.ToUpper().Substring(0, 2).Equals(Constants.QueueMS_Status);


                if (ispaymentXPMerchant && (origApp.GatewayAllowACH || origApp.GatewayAllowCC) && (!appStatus && !appACHStatus))
                    perform = true;
            }
            else
            {
                perform = false;
            }


            return perform;
        }
        catch (Exception)
        {

            return perform;
        }

    }

    public static DataSet UpdateMerchantRiskParameters(MerchantApp origApp)
    {
        try
        {
            string mcc = GetMerchantRiskMCC(origApp);
                Hashtable prms = new Hashtable();
                prms.Add("@MerchantAppUID", origApp.MerchantAppUID);
                prms.Add("@Enabled", 1);
                prms.Add("@MCC", mcc);
                DataSet ds = DataAccess.DataRiskDao.GetMerchantRiskParameters(prms);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataRisk data = DataAccess.DataRiskDao;
                        MerchantRiskParameter param = null;
                        param = new MerchantRiskParameter();
                        param.UID = dr["UID"].ToString();
                    if (Convert.ToInt32(dr["RiskID"]) == 26 || Convert.ToInt32(dr["RiskID"]) == 27)
                        param.Threshold = origApp.TinfoAverageMonthlyVMCVolume;
                    else
                        param.Threshold = Convert.ToDecimal(dr["Threshold"]);

                    param.Enabled = Convert.ToBoolean(dr["Enable"]);
                        param.UserUpdated = UserSessions.CurrentUser.UserName;
                        param.MCC = mcc;
                        data.UpdateMerchantRiskParameter(param);

                    }
                }
             ds = DataAccess.DataRiskDao.GetMerchantRiskParameters(prms);

            return ds;
        }
        catch (Exception)
        {

            throw;
        }
    }
    /// <summary>
    /// GetMerchantMCCForInsert: function used to get and validate MCC value of Merchant app to assign risk parameter while assign paymentXP access.
    /// </summary>
    /// <returns> string MCC value</returns>
    public static string GetMerchantMCCForInsert()
    {
        string mcc = string.Empty;

        try
        {
            if (UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine && UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus)
            {
                mcc = GetMcc(true, UserSessions.CurrentMerchantApp);
            }
            else
            {
                mcc = GetMcc(false, UserSessions.CurrentMerchantApp);
            }
            return mcc;
        }
        catch (Exception)
        {

            throw;
        }

    }
    public static bool AddNotes(string notes, string notesUID)
    {
        try
        {
            DataAgent data = DataAccess.DataAgentDao;
            User user = UserSessions.CurrentUser;
            if (UserSessions.CurrentAgent != null)
                data.InsertAgentNotes(UserSessions.CurrentAgent.AgentUID, notes, user.UserName, notesUID);

            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }
    //Allow PaymentXP access automatically PXP-11451
    public static void AllowPxpForNutra()
    {
        try
        {

            Hashtable prms = new Hashtable();
            prms.Add("@HookTableKeyUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
            prms.Add("@Username", UserSessions.CurrentMerchantApp.ID);
            UserFacade facade = new UserFacade();
            User user = facade.GetUser(prms);

            if (user == null)
            {
                user = new User();
                user.HookTableUID = "904683f4-094b-4bda-aef2-1bd7931c77d0"; //Merchant
                user.HookTableKeyUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                user.UserName = UserSessions.CurrentMerchantApp.ID;
                user.Password = RandomWordGenerator.WordSymNumber(2, true, 3);
                user.FirstName = UserSessions.CurrentMerchantApp.BusinessDBAName;
                user.LastName = string.Empty;
                user.Email = DataAccess.DataMerchantAppDao.IsMeritCardUser(UserSessions.CurrentMerchantApp.AgentUID, ConfigurationManager.AppSettings["MeritcardParentAgentUID"]) ? ConfigurationManager.AppSettings["MeritcardMailsSendToAddress"] : UserSessions.CurrentMerchantApp.BusinessEmailAddress;
                user.GroupUID = string.Empty;
                user.Disabled = false;
                user.HasDBAccess = true;
                user.AccessLevelUID = "7b824322-b5a6-4abf-8810-a29ff271d8b6";
                user.PasswordQuestion = string.Empty;
                user.PasswordAnswer = string.Empty;
                user.UserCreated = DateTime.Now.ToString();
                user.Office = UserSessions.CurrentMerchantApp.Office;

                user.DefaultRoleUID = "a6a9eb47-6312-4e5a-8342-acaf457d8322"; //Merchant

                user.ParentUID = string.Empty;

                facade.InsertUser(user);

                if (user.UID != "-1")
                {
                    facade.InsertUserRoles(user.UID, "A6A9EB47-6312-4E5A-8342-ACAF457D8322", true); //Merchant            
                }
            }
            //Active
            string active = string.Empty;
            facade.UpdateUserStatus(user.UID, true);
            AddNotes("Agent login is Active", "6f065641-bc3b-47b2-aa93-c4497b91f954");

            facade.InsertUserPortals(user.UID, "4A77C310-4264-45C6-96C1-F7EFE61C7D2E", true);
            String mcc = FormHandler.GetMerchantMCCForInsert();
            DataAccess.DataMerchantAppDao.ActivatePaymentXP(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentMerchantApp.ID, mcc);

            if (UserSessions.CurrentMerchantApp.AuthPlatformUID.ToUpper() == "E10439C3-F025-43F5-A8F4-0D3BD4D5EB2F" //frontend -- compass
                && UserSessions.CurrentMerchantApp.SettlePlatformUID.ToUpper() == "9505AB8F-1F8C-463C-90C2-1445189AE79A") // backend -- Front
            {
                DataGatewayPage.InsertLevelDataDefaults(UserSessions.CurrentMerchantApp.ID);
            }


        }
        catch (Exception)
        {
            throw;
        }
    }
    public static string GetMerchantAppRiskMCC()
    {
        string merchantAppMcc = string.Empty;
        try
        {

            DataRisk data = DataAccess.DataRiskDao;

            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
            DataSet ds = data.GetMerchantRiskParameterMCC(prms);
            if (ds.Tables[0].Rows.Count > 0)
            {
                merchantAppMcc = ds.Tables[0].Rows[0]["MCC"].ToString();
            }
            return merchantAppMcc;
        }
        catch (Exception)
        {

            throw;
        }
    }
    //Added by Koshlendra for PXP-3529- Zeus: Set default Risk parameters for Active merchants end

    #endregion


    //PXP-11452 By Sanidhya 
    public static MerchantApp ManageDP_SoftwareStatus(MerchantApp agreement)
    {
        string StatusUID = string.Empty;
        string bank = string.Empty;
        string OrigStatus = string.Empty;

        if (agreement != null && agreement.MerchantAppClone != null)
        {
            OrigStatus = agreement.MerchantAppClone.StatusUID.ToUpper();
            StatusUID = agreement.StatusUID.ToUpper();
        }
        else
        {
            OrigStatus = agreement != null ? agreement.StatusUID.ToUpper() : string.Empty;
            StatusUID = agreement != null ? agreement.StatusUID.ToUpper() : string.Empty;
        }

        if (agreement != null) //Removed ISNutra check for PXP-17614 by koshlendra 
        {
            if (OrigStatus != StatusUID)
            {
                if (StatusUID.ToUpper() == Constants.QUEUESTATUS_DP_RECEIVED_SOFTWARE || StatusUID.ToUpper() == Constants.QUEUESTATUS_DP_RECEIVED_HARDWARE)
                {
                    agreement.AllowCredits = true;
                    agreement.GatewayAllowCC = true;
                    agreement.GatewayModeUID = Constants.GATEWAY_MODE_LIVE;
                    agreement.SettlementSchedule = Constants.DPSOFT_SETTLEMENT_TIME;

                    Dictionary<string, Decimal> _dict = new Dictionary<string, decimal>();
                    if (_dict.Count == 0)
                    {
                        _dict.Add("StoreFront", agreement.TinfoStoreFrontSwipedPercent);
                        _dict.Add("Internet", agreement.TinfoInterntPercent);
                        _dict.Add("MailOrder", agreement.TinfoMailOrderPercent);
                        _dict.Add("OffPremise", agreement.TinfoOffPremisePercent);
                        _dict.Add("TradeShow", agreement.TinfoTradeShowPercent);
                        _dict.Add("Other", agreement.TinfoOtherPercent);
                    }

                    var _keyVal = _dict.OrderByDescending(x => x.Value).FirstOrDefault().Key;
                    if (_keyVal == "Internet" || _keyVal == "MailOrder")
                    {
                        if (_keyVal == "Internet")
                        {
                            agreement.Classification = Constants.CLASSIFICATION_ECOMM;

                        }
                        else if (_keyVal == "MailOrder")
                        {
                            agreement.Classification = Constants.CLASSIFICATION_MOTO;
                        }
                    }
                    else if (_keyVal == "StoreFront" || _keyVal == "OffPremise" ||
                       _keyVal == "TradeShow" || _keyVal == "Other")
                    {
                        agreement.Classification = Constants.CLASSIFICATION_RETAIL;
                    }


                }
            }
        }
        return agreement;
    }





    //For pxp-6935 - Added by abarua 
    /// <summary>
    /// Method to check if any one of the BeneficialOwner is checked.
    /// </summary>
    /// <returns></returns>
    public static bool IsBeneficialOwnerChecked(MerchantApp agreement)
    {
        bool isASignChecked = false;

        agreement.Owners = DataAccess.DataMerchantAppDao.GetOwners(agreement.MerchantAppUID);

        foreach (Owner ownr in agreement.Owners)
        {
            if ((!string.IsNullOrEmpty(ownr.FirstName) || !string.IsNullOrEmpty(ownr.SSN)) && ownr.BeneficialOwner) isASignChecked = true;
        }
        return isASignChecked;
    }

    //For pxp-6736 - Added by abarua 
    /// <summary>
    /// Method to check if any one of the AuthorizedSignature is checked.
    /// </summary>
    /// <returns></returns>
    public static bool IsAuthorizedSignatureChecked(MerchantApp agreement)
    {
        bool isASignChecked = false;

        agreement.Owners = DataAccess.DataMerchantAppDao.GetOwners(agreement.MerchantAppUID);

        foreach (Owner ownr in agreement.Owners)
        {
            if ((!string.IsNullOrEmpty(ownr.FirstName) || !string.IsNullOrEmpty(ownr.SSN)) && ownr.AuthorizedSignature) isASignChecked = true;
        }
        return isASignChecked;
    }


    //RThakur PXP-3957 
    /// <summary>
    /// Method to check if any one of the BeneficialOwner or AuthorizedSignature is checked.
    /// </summary>
    /// <returns></returns>
    public static bool IsBenfOwnerOrAuthSignChecked(MerchantApp agreement)
    {
        bool isBOwnOrASignChecked = false;

        agreement.Owners = DataAccess.DataMerchantAppDao.GetOwners(agreement.MerchantAppUID);

        foreach (Owner ownr in agreement.Owners)
        {
            if ((!string.IsNullOrEmpty(ownr.FirstName) || !string.IsNullOrEmpty(ownr.SSN)) && (ownr.BeneficialOwner || ownr.AuthorizedSignature)) isBOwnOrASignChecked = true;
        }
        return isBOwnOrASignChecked;
    }

    /// <summary>
    /// Add merchant note every time when account goes 'In Collections'
    /// </summary>
    /// <returns></returns>
    public static string AddMerchantNotes()
    {
        string error = string.Empty;

        if (string.IsNullOrEmpty(Constants.MERCHANT_NOTE))
            error = "Please enter notes.<br>";
        if (string.IsNullOrEmpty(Constants.MERCHANT_NOTE_CODE))
            error += "Please select notecode.<br>";

        if (error.Trim() != string.Empty)
            return error;
        try
        {
            if (UserSessions.CurrentMerchantApp != null)
            {
                MerchantFacade facade = new MerchantFacade();
                MerchantApp app = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
                User user = UserSessions.CurrentUser;
                string subject = string.Empty;

                if (!string.IsNullOrEmpty(Constants.MERCHANT_NOTE_CODE))
                    subject = Constants.MERCHANT_NOTE_CODE;

                MerchantNotes ObjMerchantNotes = new MerchantNotes()
                {
                    MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID,
                    Subject = subject,
                    Notes = Constants.MERCHANT_NOTE,
                    RequiresCallback = Constants.MERCHANT_RequiresCallback_Checked,
                    View_Agent = Constants.MERCHANT_View_Agent_Checked,
                    View_Bank = Constants.MERCHANT_View_Bank_Checked,
                    View_MPSAll = Constants.MERCHANT_View_MPSAll_Checked,
                    Email_Agent = Constants.MERCHANT_Email_Agent_Checked,
                    UserCreated = user.UserName
                };

                if (Constants.MERCHANT_ApplySameLegalName_Checked == false)
                    DataAccess.DataMerchantAppDao.InsertMerchantNotes(ObjMerchantNotes);
                else
                    DataAccess.DataMerchantAppDao.InsertMerchantNotesLegalName(ObjMerchantNotes);
            }
        }
        catch (Exception ex)
        {
            error = "Exception catched: " + ex.StackTrace;
            return ex.Message;
        }
        return error;

    }
    /// <summary>
    /// function added validate the MerchantAppTypeUID is valid for AcquirerId or not for PXP-8073 by koshlendra
    /// </summary>
    /// <param name="merchantAppTypeUID"></param>
    /// <returns></returns>
    public static bool ISMerchantAppTypeUIDForAcquirerID(string merchantAppTypeUID)
    {
        bool isValid = false;
        switch (merchantAppTypeUID.ToUpper())
        {
            //PXP-10046 Fady Massoud Add BBVA for 3DE Call ( only Match from db handled)
            case Constants.BANK_BBVACOMPASS:
            case Constants.BANK_BMO_HARRIS:
                isValid = true;
                break;
            case Constants.BANK_WELLS_FARGO:
                isValid = true;
                break;
            case Constants.BANK_WOODFOREST:
            case Constants.BANK_CITIZENS:
                isValid = true;
                break;
            // code added to implement PXP8073 by koshlendra start
            case Constants.CORPORATE:
                isValid = true;
                break;
            case Constants.BANK_HEADQUARTER:
                isValid = true;
                break;
            case Constants.BANK_ACH_ONLY:
                isValid = true;
                break;
            // Bank added to implement PXP8073 by koshlendra end
            case Constants.BANK_CHESAPEAKE: 
                isValid = true;
                break;
            case Constants.BANK_WOODFOREST_SB:
                isValid = true;
                break;
            default:
                break;
        }

        return isValid;

    }



    /// <summary>
    /// Code added for PXP-9145[Ability to generate and review High Risk Merchant registration request for Mastercardconnect] by koshlendra Start
    /// </summary>
    /// <param name="isOpsForm"></param>
    /// <param name="isStatuschanged"></param>
    /// <param name="achMerchant"></param>
    public static void Upload_MRP_PDF(bool sendMRPRequest)
    {
        MerchantApp app = UserSessions.CurrentMerchantApp;
        custom.Clear();
        if (sendMRPRequest)
            custom.Add("UploadFormType", "Send_MRP_Req_Form");
        FillMRPPDF(UserSessions.CurrentMerchantApp.Brand);


    }
    //code changes done for PXP-9525 by koshlendra start
    public static void FillMRPPDF(MerchantBrand brand)
    {
        if (brand == MerchantBrand.None)
        {
            brand = MerchantBrand.Meritus;
        }

        MerchantApp app = UserSessions.CurrentMerchantApp;
        Underwriting objUW = new Underwriting();
        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        objUW = data.LoadMerchantUWNotes(app.MerchantAppUID);
        //START PXP-10056 change accountId for BBVA By sanidhya
        //string accountID = (app.Office == CommonUtility.Util.Offices.Irvine)&& app.IsNutraMerchant && app.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS ? "6412" : "6323";
        //code added for PXP-16950 start 
        string accountID = GetMRPAcquirerID(app); // DM-1648 by Jorge
        //code added for PXP-16950 end
        custom.Add("AcquirerID", accountID);
        //END PXP-10056 change accountId for BBVA By sanidhya
        StringBuilder sb = new StringBuilder();
        custom.Add("MCC", app.SicCode);
        if (objUW != null && (objUW.CUDate != DateTime.MinValue || objUW.ACHCUDate != DateTime.MinValue))
            custom.Add("CU_Approved_Date", (objUW.ACHCUDate != DateTime.MinValue) ? objUW.ACHCUDate.ToString("yyyyMMdd") : objUW.CUDate.ToString("yyyyMMdd"));
        else
            custom.Add("CU_Approved_Date", string.Empty);
        custom.Add("BackMID", (app.SettlePlatformMid.Length > 15) ? app.SettlePlatformMid.Remove(0, app.SettlePlatformMid.Length - 15) : app.SettlePlatformMid);
        custom.Add("MLE", CommonUtility.Util.TruncateTextWithLength(app.BusinessLegalName.Replace(",", ""), 40));
        custom.Add("DBA", CommonUtility.Util.TruncateTextWithLength(app.BusinessDBAName, 40));
        custom.Add("Addr1", CommonUtility.Util.TruncateTextWithLength(app.BusinessAddress.Replace(",", ""), 40));
        custom.Add("Addr2", CommonUtility.Util.TruncateTextWithLength(app.BusinessAddressLine2.Replace(",", ""), 40));
        custom.Add("City", CommonUtility.Util.TruncateTextWithLength(app.BusinessCity, 20));
        custom.Add("State_RegionCode", CommonUtility.Util.TruncateTextWithLength(app.BusinessState, 2));
        custom.Add("Postal_Code", CommonUtility.Util.TruncateTextWithLength(app.BusinessZip, 10));
        custom.Add("PrimaryContactPhone", CommonUtility.Util.TruncateTextWithLength(app.CustomerServicePhone.Replace("-", ""), 12));
        custom.Add("TaxReg", CommonUtility.Util.TruncateTextWithLength(app.BusinessTaxID.Replace("-", ""), 25));
        custom.Add("Website", ModifyRegisteredUrls(app.RegisteredURLs));
        custom.Add("ServiceDescription_MerchantSells", CommonUtility.Util.TruncateTextWithLength(app.MerchantSells, 300));
        custom.Add("CustomerServiceEmail", CommonUtility.Util.TruncateTextWithLength(app.CustomerServiceEmail, 100));

        if (app.Owners != null)
        {
            if (app.Owners.Count > 0)
            {
                custom.Add("Owner1_LastName", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].LastName, 35));
                custom.Add("Owner1_FirstName", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].FirstName, 25));
                custom.Add("Owner1_MiddleInitial", (string.IsNullOrWhiteSpace(app.Owners[0].MiddleName)) ? "" : app.Owners[0].MiddleName.Substring(0, 1));
                custom.Add("Owner1_Street_Address1", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].Address1.Replace(",", ""), 40));
                custom.Add("Owner1_City", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].City, 20));
                custom.Add("Owner1_State", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].State, 2));
                custom.Add("Owner1_PostalCode", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].Zip, 10));
                //code changes for handle country null or empty value start
                custom.Add("Owner1_CountryCode", (app.Owners[0].Country == "US" || string.IsNullOrWhiteSpace(app.Owners[0].Country)) ? "USA" : app.Owners[0].Country);
                //code changes for handle country null or empty value end
                custom.Add("Owner1Phone", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].HomePhone.Replace("-", ""), 12));
                custom.Add("Owner1SSN", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].SSN.Replace("-", ""), 13));
                custom.Add("Owner1DL", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].DriversLicense, 25));
                custom.Add("Owner1DLState", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].DriversLicenseState, 2));


            }

            if (app.Owners.Count > 1)
            {
                custom.Add("Owner2_LastName", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].LastName, 35));
                custom.Add("Owner2_FirstName", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].FirstName, 25));
                custom.Add("Owner2_MiddleInitial", (string.IsNullOrWhiteSpace(app.Owners[1].MiddleName)) ? "" : app.Owners[1].MiddleName.Substring(0, 1));
                custom.Add("Owner2_Street_Address1", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].Address1.Replace(",", ""), 40));
                custom.Add("Owner2_City", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].City, 20));
                custom.Add("Owner2_State", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].State, 2));
                custom.Add("Owner2_PostalCode", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].Zip, 10));
                //code changes for handle country null or empty value start
                custom.Add("Owner2_CountryCode", (app.Owners[1].Country == "US" || string.IsNullOrWhiteSpace(app.Owners[1].Country)) ? "USA" : app.Owners[1].Country);
                //code changes for handle country null or empty value end
                custom.Add("Owner2Phone", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].HomePhone.Replace("-", ""), 12));
                custom.Add("Owner2SSN", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].SSN.Replace("-", ""), 13));
                custom.Add("Owner2DL", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].DriversLicense, 25));
                custom.Add("Owner2DLState", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].DriversLicenseState, 2));


            }
        }

        FormBinding.BindObjectToPDF(objUW, "~/PDF/MRP_Form.pdf", "MRPRequest_" + app.BusinessLegalName.Replace(",", "") + "_" + app.ID.ToString() + "_" + CommonUtility.Util.GetDateTimeStamp() + ".pdf", custom);


    }

    // START DM-1648 by Jorge
    private static string GetMRPAcquirerID(MerchantApp app)
    {        
        if (app.Office == CommonUtility.Util.Offices.Irvine && app.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS)
        {
            return ConfigurationManager.AppSettings["Zeus3de.BBVA.AcquirerId"];
        }
      
        return app.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS ?
                    ConfigurationManager.AppSettings["Zeus3de.CitizensBank.AcquirerId"] :
                    ConfigurationManager.AppSettings["Zeus3de.WoodforestNationalBankICA.AcquirerId"];
    }
    // END DM-1648 by Jorge

    /// Code added for PXP-9145[Ability to generate and review High Risk Merchant registration request for Mastercardconnect] by koshlendra end
    /// Code added for PXP-9310[Generate and save High Risk Merchant registration request in csv format] by koshlendra start
    public static void Upload_MRP_CSV()
    {

        try
        {
            MerchantApp app = UserSessions.CurrentMerchantApp;
            Underwriting objUW = new Underwriting();
            DataUnderwritng data = DataAccess.DataUnderwritingDao;
            string cudate = string.Empty;
            objUW = data.LoadMerchantUWNotes(app.MerchantAppUID);

            if (objUW != null && (objUW.CUDate != DateTime.MinValue || objUW.ACHCUDate != DateTime.MinValue))
                cudate = (objUW.ACHCUDate != DateTime.MinValue) ? objUW.ACHCUDate.ToString("yyyyMMdd") : objUW.CUDate.ToString("yyyyMMdd");


            string path = ConfigurationManager.AppSettings["LogFilePath"] + DateTime.Today.ToString("yyyyMM") + @"\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);


            string filePath = path + "MRPRequest_" + app.BusinessLegalName.Replace(",", "") + "_" + app.ID.ToString() + "_" + CommonUtility.Util.GetDateTimeStamp() + ".csv";

            string delimiter = ",";
            //START PXP-10068 change accountId for BBVA By Ali 
            //Code changes done for PXP-16951 start
            string accountID = GetMRPAcquirerID(app); //DM-1648 by Jorge
            //Code changes done for PXP-16951 end
            //string accountID = app.IsNutraMerchant && app.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS ? "6412" : "6323";
            //END PXP-10068 change accountId for BBVA By Ali
            string[] strMain = new string[] { "A", accountID, "1", "NB", app.SicCode, cudate, (app.SettlePlatformMid.Length > 15) ? app.SettlePlatformMid.Remove(0, app.SettlePlatformMid.Length - 15) : app.SettlePlatformMid, CommonUtility.Util.TruncateTextWithLength(app.BusinessLegalName.Replace(",", ""), 40), CommonUtility.Util.TruncateTextWithLength(app.BusinessDBAName, 40), CommonUtility.Util.TruncateTextWithLength(app.BusinessAddress.Replace(",", ""), 40), CommonUtility.Util.TruncateTextWithLength(app.BusinessAddressLine2.Replace(",", ""), 40), CommonUtility.Util.TruncateTextWithLength(app.BusinessCity, 20), CommonUtility.Util.TruncateTextWithLength(app.BusinessState, 2), " ", CommonUtility.Util.TruncateTextWithLength(app.BusinessZip, 10), "USA", CommonUtility.Util.TruncateTextWithLength(app.CustomerServicePhone.Replace("-", ""), 12), "N", " ", CommonUtility.Util.TruncateTextWithLength(app.BusinessTaxID.Replace("-", ""), 25), "highriskregistration@paysafe.com", ModifyRegisteredUrls(app.RegisteredURLs), CommonUtility.Util.TruncateTextWithLength(app.MerchantSells, 300), CommonUtility.Util.TruncateTextWithLength(app.CustomerServiceEmail, 100), "Secured website and/or credit card authorization form", "Trial offer and monthly billing", "Y", " ", "N", " ", " ", " ", " ", " ", "N", " ", "Y" };

            string[] strOwner0 = null;
            string[] strOwner1 = null;
            string[] strOwner2 = null;
            string[] strOwner3 = null;
            string[] strOwner4 = null;
            if (app.Owners != null)
            {
                //code changes for handle owner's country null or empty value start
                //START PXP-10068 change accountId for BBVA By Ali 
                if (app.Owners.Count > 0)
                {
                    //code added for handle owner's null country code to resolve production issue by koshlendra start
                    if (!string.IsNullOrWhiteSpace(app.Owners[0].FirstName) && !string.IsNullOrWhiteSpace(app.Owners[0].LastName))
                        strOwner0 = new string[] { "A", accountID, "2", "1", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].LastName, 35), CommonUtility.Util.TruncateTextWithLength(app.Owners[0].FirstName, 25), (string.IsNullOrWhiteSpace(app.Owners[0].MiddleName)) ? "" : app.Owners[0].MiddleName.Substring(0, 1), CommonUtility.Util.TruncateTextWithLength(app.Owners[0].Address1.Replace(",", ""), 40), " ", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].City, 20), CommonUtility.Util.TruncateTextWithLength(app.Owners[0].State, 2), " ", CommonUtility.Util.TruncateTextWithLength(app.Owners[0].Zip, 10), (app.Owners[0].Country == "US" || string.IsNullOrWhiteSpace(app.Owners[0].Country)) ? "USA" : app.Owners[0].Country, CommonUtility.Util.TruncateTextWithLength(app.Owners[0].HomePhone.Replace("-", ""), 12), CommonUtility.Util.TruncateTextWithLength(app.Owners[0].SSN.Replace("-", ""), 13), CommonUtility.Util.TruncateTextWithLength(app.Owners[0].DriversLicense, 25), CommonUtility.Util.TruncateTextWithLength(app.Owners[0].DriversLicenseState, 2), string.IsNullOrWhiteSpace(app.Owners[0].DriversLicense) ? "" : "USA" };
                }
                if (app.Owners.Count > 1)
                {
                    if (!string.IsNullOrWhiteSpace(app.Owners[1].FirstName) && !string.IsNullOrWhiteSpace(app.Owners[1].LastName))
                        strOwner1 = new string[] { "A", accountID, "2", "2", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].LastName, 35), CommonUtility.Util.TruncateTextWithLength(app.Owners[1].FirstName, 25), (string.IsNullOrWhiteSpace(app.Owners[1].MiddleName)) ? "" : app.Owners[1].MiddleName.Substring(0, 1), CommonUtility.Util.TruncateTextWithLength(app.Owners[1].Address1.Replace(",", ""), 40), " ", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].City, 20), CommonUtility.Util.TruncateTextWithLength(app.Owners[1].State, 2), " ", CommonUtility.Util.TruncateTextWithLength(app.Owners[1].Zip, 10), (app.Owners[1].Country == "US" || string.IsNullOrWhiteSpace(app.Owners[1].Country)) ? "USA" : app.Owners[1].Country, CommonUtility.Util.TruncateTextWithLength(app.Owners[1].HomePhone.Replace("-", ""), 12), CommonUtility.Util.TruncateTextWithLength(app.Owners[1].SSN.Replace("-", ""), 13), CommonUtility.Util.TruncateTextWithLength(app.Owners[1].DriversLicense, 25), CommonUtility.Util.TruncateTextWithLength(app.Owners[1].DriversLicenseState, 2), string.IsNullOrWhiteSpace(app.Owners[1].DriversLicense) ? "" : "USA" };
                }
                if (app.Owners.Count > 2)
                {
                    if (!string.IsNullOrWhiteSpace(app.Owners[2].FirstName) && !string.IsNullOrWhiteSpace(app.Owners[2].LastName))
                        strOwner2 = new string[] { "A", accountID, "2", "3", CommonUtility.Util.TruncateTextWithLength(app.Owners[2].LastName, 35), CommonUtility.Util.TruncateTextWithLength(app.Owners[2].FirstName, 25), (string.IsNullOrWhiteSpace(app.Owners[2].MiddleName)) ? "" : app.Owners[2].MiddleName.Substring(0, 1), CommonUtility.Util.TruncateTextWithLength(app.Owners[2].Address1.Replace(",", ""), 40), " ", CommonUtility.Util.TruncateTextWithLength(app.Owners[2].City, 20), CommonUtility.Util.TruncateTextWithLength(app.Owners[2].State, 2), " ", CommonUtility.Util.TruncateTextWithLength(app.Owners[2].Zip, 10), (app.Owners[2].Country == "US" || string.IsNullOrWhiteSpace(app.Owners[2].Country)) ? "USA" : app.Owners[2].Country, CommonUtility.Util.TruncateTextWithLength(app.Owners[2].HomePhone.Replace("-", ""), 12), CommonUtility.Util.TruncateTextWithLength(app.Owners[2].SSN.Replace("-", ""), 13), CommonUtility.Util.TruncateTextWithLength(app.Owners[2].DriversLicense, 25), CommonUtility.Util.TruncateTextWithLength(app.Owners[2].DriversLicenseState, 2), string.IsNullOrWhiteSpace(app.Owners[2].DriversLicense) ? "" : "USA" };
                }
                if (app.Owners.Count > 3)
                {
                    if (!string.IsNullOrWhiteSpace(app.Owners[3].FirstName) && !string.IsNullOrWhiteSpace(app.Owners[3].LastName))
                        strOwner3 = new string[] { "A", accountID, "2", "4", CommonUtility.Util.TruncateTextWithLength(app.Owners[3].LastName, 35), CommonUtility.Util.TruncateTextWithLength(app.Owners[3].FirstName, 25), (string.IsNullOrWhiteSpace(app.Owners[3].MiddleName)) ? "" : app.Owners[3].MiddleName.Substring(0, 1), CommonUtility.Util.TruncateTextWithLength(app.Owners[3].Address1.Replace(",", ""), 40), " ", CommonUtility.Util.TruncateTextWithLength(app.Owners[3].City, 20), CommonUtility.Util.TruncateTextWithLength(app.Owners[3].State, 2), " ", CommonUtility.Util.TruncateTextWithLength(app.Owners[3].Zip, 10), (app.Owners[3].Country == "US" || string.IsNullOrWhiteSpace(app.Owners[3].Country)) ? "USA" : app.Owners[3].Country, CommonUtility.Util.TruncateTextWithLength(app.Owners[3].HomePhone.Replace("-", ""), 12), CommonUtility.Util.TruncateTextWithLength(app.Owners[3].SSN.Replace("-", ""), 13), CommonUtility.Util.TruncateTextWithLength(app.Owners[3].DriversLicense, 25), CommonUtility.Util.TruncateTextWithLength(app.Owners[3].DriversLicenseState, 2), string.IsNullOrWhiteSpace(app.Owners[3].DriversLicense) ? "" : "USA" };
                }
                if (app.Owners.Count > 4)
                {
                    if (!string.IsNullOrWhiteSpace(app.Owners[4].FirstName) && !string.IsNullOrWhiteSpace(app.Owners[4].LastName))
                        strOwner4 = new string[] { "A", accountID, "2", "5", CommonUtility.Util.TruncateTextWithLength(app.Owners[4].LastName, 35), CommonUtility.Util.TruncateTextWithLength(app.Owners[4].FirstName, 25), (string.IsNullOrWhiteSpace(app.Owners[4].MiddleName)) ? "" : app.Owners[4].MiddleName.Substring(0, 1), CommonUtility.Util.TruncateTextWithLength(app.Owners[4].Address1.Replace(",", ""), 40), " ", CommonUtility.Util.TruncateTextWithLength(app.Owners[4].City, 20), CommonUtility.Util.TruncateTextWithLength(app.Owners[4].State, 2), " ", CommonUtility.Util.TruncateTextWithLength(app.Owners[4].Zip, 10), (app.Owners[4].Country == "US" || string.IsNullOrWhiteSpace(app.Owners[4].Country)) ? "USA" : app.Owners[4].Country, CommonUtility.Util.TruncateTextWithLength(app.Owners[4].HomePhone.Replace("-", ""), 12), CommonUtility.Util.TruncateTextWithLength(app.Owners[4].SSN.Replace("-", ""), 13), CommonUtility.Util.TruncateTextWithLength(app.Owners[4].DriversLicense, 25), CommonUtility.Util.TruncateTextWithLength(app.Owners[4].DriversLicenseState, 2), string.IsNullOrWhiteSpace(app.Owners[4].DriversLicense) ? "" : "USA" };
                    //code added for handle owner's null country code to resolve production issue by koshlendra End
                }
                //END PXP-10068 change accountId for BBVA By Ali 
                //END PXP-10068 change accountId for BBVA By Ali 
                //code changes for handle owner's country null or empty value end
            }
            string[][] output = new string[][]{
            strMain,
            strOwner0,
            strOwner1,
            strOwner2,
            strOwner3,
            strOwner4

         };
            int length = output.GetLength(0);
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < length; index++)
            {
                if (output[index] != null)
                    sb.AppendLine(string.Join(delimiter, output[index]));
            }


            File.WriteAllText(filePath, sb.ToString());
            FormBinding.BindObjectToCSV(filePath, "MRPRequest_" + app.BusinessLegalName.Replace(",", "") + "_" + app.ID.ToString() + "_" + CommonUtility.Util.GetDateTimeStamp() + ".csv", sb.ToString());



        }
        catch (Exception)
        {


        }
    }

    public static void Upload_VIRP_CSV()
    {
        try
        {
            MerchantApp app = UserSessions.CurrentMerchantApp; 
            string fileName = "VIRPRequest_" + app.BusinessLegalName.Replace(",", "") + "_" + app.ID.ToString() + "_" + CommonUtility.Util.GetDateTimeStamp() + ".xlsx";
            Dictionary<int, Dictionary<string, string>> information = new Dictionary<int, Dictionary<string, string>>();
            Dictionary<string, string> dataInfo = new Dictionary<string, string>();
            dataInfo.Add("A6", "UNITED STATES OF AMERICA - 840");
            dataInfo.Add("B6", app.BusinessLegalName);
            dataInfo.Add("C6", app.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS ? "405449" : "");
            dataInfo.Add("D6", app.CustomerServicePhone.Replace("-", "")); 
            dataInfo.Add("E6", app.SettlePlatformMid);
            dataInfo.Add("F6", app.BusinessAddress);
            dataInfo.Add("G6", app.BusinessCity);
            dataInfo.Add("H6", app.BusinessState);
            dataInfo.Add("I6", app.BusinessMailingZip);
            dataInfo.Add("J6", app.BusinessCountry);
            dataInfo.Add("K6", app.VisaSicCode);
            dataInfo.Add("L6", "Merchant");
            dataInfo.Add("M6", "UNITED STATES OF AMERICA - 840");
            dataInfo.Add("N6", "UNITED STATES OF AMERICA - 840");
            dataInfo.Add("O6", "No");
            dataInfo.Add("P6", "Yes");
            dataInfo.Add("Q6", "No");
            dataInfo.Add("R6", "No");
            dataInfo.Add("AE6", app.SettlePlatformMid.Substring(0, 15));
            dataInfo.Add("AF6",  app.HighRiskDescriptor);
            dataInfo.Add("AI6",  app.Owners.First().FirstName);
            if (string.IsNullOrEmpty(app.VIRPRegisteredURLs))
            {
                dataInfo.Add("AG6", app.BusinessWebsite.Replace("-", ","));
            }
            else
            {
                dataInfo.Add("AG6", app.VIRPRegisteredURLs.Replace("-", ","));
            }
            dataInfo.Add("AJ6",  app.Owners.First().LastName);
            dataInfo.Add("AK6", app.Owners.First().HomePhone.Replace("-", ""));
            dataInfo.Add("AL6",  "YES");
            dataInfo.Add("AM6", string.Format("{0:0.00}", app.TinfoAverageMonthlyVMCVolume));
            decimal averageMontlyTransactions = (app.TinfoAverageMonthlyVMCVolume == 0 || app.TinfoAverageVMCTicket == 0) ? 0 :
                app.TinfoAverageMonthlyVMCVolume / app.TinfoAverageVMCTicket;

            dataInfo.Add("AN6", string.Format("{0:0.00}", averageMontlyTransactions));
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID", app.MerchantAppUID);
            prms.Add("@SortOrder", 1);
            prms.Add("@SortDirection", 1);
            prms.Add("@PageSize", 25);
            prms.Add("@CurrentPage", 1);
            DataTable _table = DataMerchantAppPaging.GetSearchStatusHistoryPaging(prms, 25, 0);//GetStatusHistory(prms);
            DateTime _dateCUApproved = (from row in _table.AsEnumerable()
                                        where row.Field<string>("Status").ToUpper() == "CU - APPROVED"
                                        select row.Field<DateTime>("Changed Date")
                 ).FirstOrDefault();
            dataInfo.Add("AO6", _dateCUApproved == DateTime.MinValue ? string.Empty : _dateCUApproved.ToString("MM/dd/yyyy"));
            dataInfo.Add("AP6", app.BusinessStartDate.ToString("MM/dd/yyyy"));
            dataInfo.Add("AR6", app.MerchantSells);
            dataInfo.Add("AS6", "NO");
            information.Add(1, dataInfo);

            byte[] data = ProductSubscriptionService.CreateExcelPackage(HttpContext.Current.Server.MapPath(ConstantFacade.VIRP.EXCEL_VIRP_TEMPLATE_PATH), information); 
            FormBinding.BindUploadBytesToCSV(data, fileName,Convert.ToInt32(app.ID), app.MerchantAppUID, UserSessions.CurrentUser.UserName);
                
           
        }
        catch (Exception ex)
        {
            Logging.ErrorLog.Error(ex.Message);
        }
    }
    /// Code added for PXP-9310[Generate and save High Risk Merchant registration request in csv format] by koshlendra end
    //code changes done for PXP-9525 by koshlendra end

    /// <summary>PXP-9051 RThakur 
    /// To check is IsNewVertical checkbox is checked and any VerticalMarkets checkbox is checked or not.
    /// </summary>
    /// <returns></returns>
    public static bool SetHdnValueIsAnyVerticalMarketsChecked(string statusName, string statusUID, bool isNewVertical)
    {
        bool retVal = false;
        if (statusName.ToUpper().Contains("CU") && statusUID.ToUpper() != Constants.QUEUESTATUS_CU_APPROVED)
        {
            bool isAnyVerticalMarketSelected = false;
            Hashtable prms = new Hashtable();
            DataSet dt = new DataSet();
            prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
            dt = DataAccess.DataMerchantAppDao.GetVerticalMarkets(prms);
            DataRow[] drVerticalMarket = dt.Tables[0].Select("VerticalMarketTypeID=" + 1);
            foreach (DataRow dr in drVerticalMarket)
            {
                if (DataLayer.Field2Bool(dr.ItemArray[4]))
                {
                    isAnyVerticalMarketSelected = true;
                    break;
                }
            }
            if (!isAnyVerticalMarketSelected && isNewVertical)
            {
                retVal = false;
            }
            else
            {
                retVal = true;
            }
        }
        return retVal;
    }

    //PXP-9750 Rohit Thakur >> Start
    //Create new ticket for ‘Mastercard De-registration process
    public static void AddTicketForMastercard(MerchantApp app, string ticketSource, string departmentID, string parentID, string categoryID, string priority, string timeZone)
    {
        String IssueText = string.Empty;
        DataTicket data = new DataTicket();
        Ticket ticket = new Ticket();
        User user = UserSessions.CurrentUser;

        DateTime dt = AddBusinessDays(DateTime.Now, 3);

        ticket = new Ticket();
        ticket.TicketSource = ticketSource;
        ticket.Origin = 4;
        ticket.ZID = app.ID;
        ticket.MerchantAppUID = app.MerchantAppUID;
        ticket.AgentUID = app.AgentUID;
        ticket.DepartmentID = departmentID;
        ticket.ParentID = parentID;
        ticket.CategoryID = categoryID;
        ticket.StatusID = Ticket.TICKET_OPEN;
        ticket.TimeZone = timeZone;
        ticket.Priority = priority;
        ticket.Problem = app.MasterMRP ? Constant.IssueMasterAccount : Constant.IssueChildAccount;
        ticket.UserCreated = "System";
        ticket.DateCreated = DateTime.Now;
        ticket.UserCreatedUserUID = null;
        ticket.UserModified = "System";
        ticket.DueDate = new DateTime(dt.Year, dt.Month, dt.Day, 18, 0, 0);
        ticket.OfficeID = app.Office.GetHashCode();

        data.InsertTicket(ticket);
        PaymentXP.Facade.TicketNotification.NewTicketCreated(ticket.TicketUID, false, false);
        InsertMerchantNotesForTicket(app.MerchantAppUID, "System", ticket);
        ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for Mastercard De-registration process ID : {0} .", ticket.TicketUID);
    }
    //Create new ticket for ‘Mastercard De-registration process
    public static void AddTicketForVisa(MerchantApp app, string ticketSource, string departmentID, string parentID, string categoryID, string priority, string timeZone)
    {
        String IssueText = string.Empty;
        DataTicket data = new DataTicket();
        Ticket ticket = new Ticket();
        User user = UserSessions.CurrentUser;

        DateTime dt = AddBusinessDays(DateTime.Now, 3);

        ticket = new Ticket();
        ticket.TicketSource = ticketSource;
        ticket.Origin = 4;
        ticket.ZID = app.ID;
        ticket.MerchantAppUID = app.MerchantAppUID;
        ticket.AgentUID = app.AgentUID;
        ticket.DepartmentID = departmentID;
        ticket.ParentID = parentID;
        ticket.CategoryID = categoryID;
        ticket.StatusID = Ticket.TICKET_OPEN;
        ticket.TimeZone = timeZone;
        ticket.Priority = priority;
        ticket.Problem = app.MasterVIRP ? Constant.IssueVisaAccount : Constant.IssueVisaChildAccount;
        ticket.UserCreated = "System";
        ticket.DateCreated = DateTime.Now;
        ticket.UserCreatedUserUID = null;
        ticket.UserModified = "System";
        ticket.DueDate = new DateTime(dt.Year, dt.Month, dt.Day, 18, 0, 0);
        ticket.OfficeID = app.Office.GetHashCode();

        data.InsertTicket(ticket);
        PaymentXP.Facade.TicketNotification.NewTicketCreated(ticket.TicketUID, false, false);
        InsertMerchantNotesForTicket(app.MerchantAppUID, "System", ticket);
        ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for VIRP De-registration process ID : {0} .", ticket.TicketUID);
    }

    private static DateTime AddBusinessDays(DateTime date, int days)
    {

        if (days == 0) return date;

        if (date.DayOfWeek == DayOfWeek.Saturday)
        {
            date = date.AddDays(2);
            days -= 1;
        }
        else if (date.DayOfWeek == DayOfWeek.Sunday)
        {
            date = date.AddDays(1);
            days -= 1;
        }

        date = date.AddDays(days / 5 * 7);
        int extraDays = days % 5;

        if ((int)date.DayOfWeek + extraDays > 5)
        {
            extraDays += 2;
        }

        return date.AddDays(extraDays);

    }
    //PXP-9750 Rohit Thakur >> End

    //PXP-11670 & PXP-11671  >> Rthakur
    public static bool CheckURLseparators(string RegistrdURLs)
    {
        if (!string.IsNullOrWhiteSpace(RegistrdURLs) && RegistrdURLs.RemoveWhitespace().Length > 100)
        {
            if (!CommonUtility.Util.TruncateTextWithLength(RegistrdURLs, 100).Contains(';'))
            {
                return false;
            }
        }

        return true;
    }
    private static string ModifyRegisteredUrls(string registeredUrls)
    {
        if (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
        {
            if (!string.IsNullOrWhiteSpace(registeredUrls) && registeredUrls.RemoveWhitespace().Length >= 101)
            {
                registeredUrls = registeredUrls.RemoveWhitespace().Replace(",", ";");
                if (registeredUrls[100] == ';')
                {
                    registeredUrls = registeredUrls.Substring(0, 100);
                }
                else
                {
                    registeredUrls = registeredUrls.Substring(0, CommonUtility.Util.TruncateTextWithLength(registeredUrls, 100).LastIndexOf(';'));
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.RegisteredURLs) && !string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.BusinessWebsite))
                    registeredUrls = CommonUtility.Util.TruncateTextWithLength(UserSessions.CurrentMerchantApp.BusinessWebsite, 100);
            }
        }
        else
        {
            registeredUrls = CommonUtility.Util.TruncateTextWithLength(UserSessions.CurrentMerchantApp.BusinessWebsite, 100);
        }

        return registeredUrls.RemoveWhitespace().Replace(",", ";");
    } 
}
