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
using System.IO;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Collections.Generic;

using CommonUtility;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Reflection;
using System.Text.RegularExpressions;
using PaymentXP.Facade;


public partial class wucEmailBlaster : System.Web.UI.UserControl
{
    public string CSubject
    {
        get { return Subject.Text; }
    }

    public bool CAgentUID
    {
        get { return (wucAgentSelector.m_AgentUID != string.Empty); }
    }

    public int CMerchantAppTypeUID
    {
        get { return MerchantAppTypeUID.SelectedIndex; }
    }

    public string CrblEmail
    {
        get { return rblEmail.SelectedValue; }
    }

    public string CrblMail
    {
        get { return rbMail.SelectedValue; }
    }

    public bool CPortalUID
    {
        get
        {
            for (int i = 0; i < lstPortals.Items.Count; i++)
            {
                if (lstPortals.Items[i].Selected)
                    return true;
            }
            return false;
        }
    }

    public int CAgentTypeUID
    {
        get
        {
            for (int i = 0; i < AgentTypeUID.Items.Count; i++)
            {
                if (AgentTypeUID.Items[i].Selected)
                    return 1;
            }
            return 0;
        }
    }

    public string CHTMLContent
    {
        get { return txtHTMLBody.Text; }
    }

    public ListBox PropertiesList
    {
        get { return lstProperties; }
    }

    override protected void OnInit(EventArgs e)
    {
        LookupTableHandler.LoadAgentTypes(AgentTypeUID, false);
        LookupTableHandler.LoadPortals(lstPortals, false);
        // LookupTableHandler.LoadAgentsNew(AgentUID, false);
        LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, false);
        AgentTypeUID.Items.RemoveAt(0);

        FillProperties();
        LoadTypes();
        lblMessage.Text = "";
        lblError.Text = "";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            lstCustomAttachments.Attributes.Add("ondblclick", "ListBox1_DoubleClick()");
            lstProperties.Attributes.Add("onDblClick", "ListBox2_DoubleClick()");

            if (UserSessions.CurrentEmailBlaster != null)
            {
                EmailBlasterID.Value = UserSessions.CurrentEmailBlaster.EmailBlasterID;
                LoadValues(UserSessions.CurrentEmailBlaster);
            }
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (this.fuplAttachments.HasFile)
        {
            string fileSavePath = this.Server.MapPath("../EmailAttachment") + "\\" + UserSessions.CurrentUser.UserName;

            if (!Directory.Exists(fileSavePath))
                Directory.CreateDirectory(fileSavePath);

            string filename = fileSavePath + "\\" + fuplAttachments.FileName;

            if (File.Exists(filename))
                File.Delete(filename);

            this.fuplAttachments.SaveAs(filename);

            ListItem item = new ListItem();
            item.Text = fuplAttachments.FileName;
            item.Value = filename;
            lstCustomAttachments.Items.Add(item);
        }
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        if (lstCustomAttachments.SelectedIndex != -1)
        {
            DataCommunication data = DataAccess.DataCommunicationDao;
            string[] values = lstCustomAttachments.SelectedItem.Value.Split(new char[] { ';' });

            if (values.Length == 1)
                lstCustomAttachments.Items.Remove(lstCustomAttachments.SelectedItem);
            else if (values.Length == 2)
            {
                lstCustomAttachments.Items.Remove(lstCustomAttachments.SelectedItem);
                data.DeleteAttachments(EmailBlasterID.Value);
            }
        }
    }

    private bool Check(bool isTest)
    {
        string error = string.Empty;

        if (isTest && txtTestEmailAddress.Text.Trim() == string.Empty) //for test emails check if To address is empty
            error += "Please enter recipient.<br>";

        if (Subject.Text.Trim() == string.Empty)
            error += "Please enter a subject.<br>";

        if (error == string.Empty)
            return true;
        else
        {
            lblError.Text = error;
            return false;
        }
    }

    protected void btnOpen_Click(object sender, EventArgs e)
    {
        if (lstCustomAttachments.SelectedIndex > -1)
        {
            int idx = lstCustomAttachments.SelectedIndex;
            ListItem item = lstCustomAttachments.SelectedItem;

            string[] values = lstCustomAttachments.SelectedValue.Split(';');

            string fi = values[0].ToString().Substring(values[0].ToString().LastIndexOf("\\") + 1);
            string path = values[0].ToString();

            //get file object as FileInfo
            System.IO.FileInfo file = new System.IO.FileInfo(path);

            //-- if the file exists on the server
            //set appropriate headers
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.End();
                //if file does not exist
            }
            else
            {
                Response.Write("This file does not exist.");
            }
        }
    }

    public IList<Attachments> GetAttachements()
    {
        IList<Attachments> att = null;
        int i = 0;
        Attachments attachment = null;

        foreach (ListItem item in lstCustomAttachments.Items)
        {
            att = new List<Attachments>();

            object[] values = item.Value.Split(new char[] { ';' });

            if (values.Length == 1) //new attachment
            {
                FileInfo info = new FileInfo(item.Value);
                byte[] content = new byte[info.Length];
                FileStream imagestream = info.OpenRead();
                imagestream.Read(content, 0, content.Length);
                imagestream.Close();

                attachment = new Attachments();
                attachment.FileName = values[0].ToString();
                attachment.Image = content;
                attachment.Name = item.Text;
                att.Add(attachment);
            }
            else if (values.Length == 2) //existing attachment
            {
                attachment = new Attachments();
                attachment = DataAccess.DataCommunicationDao.GetAttachments(values[1].ToString(), "")[0];
                att.Add(attachment);
            }
            i++;
        }

        return att;
    }

    protected void btnSendTestEmail_Click(object sender, EventArgs e)
    {
        SendEmails(true);
        txtTestEmailAddress.Text = "";
        //WucMessage1.AddMessageStatus("Email Sent!");
        FormHandler.DisplayMessage(Page.ClientScript, "Email has been sent successfully");
    }

    public void SendMail(bool isTest)
    {
        if (rbMail.SelectedIndex == 0)
        {
            SendEmails(isTest);
            FormHandler.DisplayMessage(Page.ClientScript, "Email has been sent successfully");
        }
        else
        {
            SendMailMergeEmails(isTest);
            FormHandler.DisplayMessage(Page.ClientScript, "Email has been sent successfully");
        }
    }

    private void SendMailMergeEmails(bool isTest)
    {
        string uid = string.Empty;
        IList<Attachments> att = new List<Attachments>();

        if (Check(isTest))
        {

            att = GetAttachements();

            string eblaster = string.Empty, Toadd = string.Empty;
            bool isMerchant = false;
            DataCommunication data = DataAccess.DataCommunicationDao;
            DataTable dt = new DataTable();
            IDictionary<string, string> EmailList = new Dictionary<string, string>();
            Dictionary<string, string> objProperties = new Dictionary<string, string>();
            string body = string.Empty, PortalUIDList = string.Empty, AgentUIDList = string.Empty;

            if (rblEmail.SelectedValue == "0") // Merchants
            {
                eblaster = "M";
                isMerchant = true;
                PortalUIDList = "";
                foreach (ListItem chk in lstPortals.Items)
                {
                    if (chk.Selected)
                        PortalUIDList += chk.Value + ",";
                }
            }
            else  // Agents
            {
                eblaster = "A";
                isMerchant = false;
                AgentUIDList = "";
                foreach (ListItem chkAgnt in AgentTypeUID.Items)
                {
                    if (chkAgnt.Selected)
                        AgentUIDList += chkAgnt.Value + ",";
                }
            }

            if (eblaster != string.Empty)
            {
                Hashtable prms = new Hashtable();

                if (isMerchant)
                    prms.Add("@IsMerchant", "1");
                else
                    prms.Add("@IsMerchant", "0");


                if (string.IsNullOrEmpty(IDList.Text))
                {
                    if (isTest) // Test Email
                    {
                        if (isMerchant)
                            prms.Add("@ID", "10012");
                        else
                            prms.Add("@ID", "1086");
                    }
                    else
                    {
                        //prms.Add("@IsMerchant", isMerchant);

                        if (wucAgentSelector.m_AgentUID != string.Empty)
                        {
                            if (IncludeSubAgents.Checked)
                                prms.Add("@MasterAgentUID", wucAgentSelector.m_AgentUID);
                            else
                                prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);
                        }

                        if (isMerchant)
                        {
                            prms.Add("@PortalUIDList", PortalUIDList.TrimEnd(','));

                            if (MerchantAppTypeUID.SelectedIndex > 0)
                            {
                                prms.Add("@MerchantTypeUID", MerchantAppTypeUID.SelectedValue);
                            }
                        }
                        else
                        {
                            prms.Add("@AgentTypeUIDList", AgentUIDList.TrimEnd(','));
                        }
                    }
                }
                else
                {
                    prms.Add("@IDList", IDList.Text);
                }

                dt = data.GetActiveEmails(prms);
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (isTest) // Test Email
                        Toadd = txtTestEmailAddress.Text; //To.Value;
                    else
                        Toadd = dt.Rows[j]["EmailList"].ToString().TrimEnd(';').Trim(); //Agent or Merchant email

                    if (!EmailList.ContainsKey(Toadd) && (!(Regex.IsMatch(Toadd, Constants.EmailRegex)) && Toadd != ""))
                    {
                        EmailList.Add(Toadd, Toadd);

                        Template t = new Template();
                        MerchantApp app = new MerchantApp();
                        Agent agent = new Agent();
                        object obj = null;
                        Type objType = null;
                        string MerchantUID = string.Empty;
                        string AgentUID = string.Empty;
                        string FromEmail = Constants.RELATIONSHIP_MANAGEMENT_EMAIL;

                        if (isMerchant) // Merchants
                        {
                            MerchantFacade facade = new MerchantFacade();
                            app = facade.GetMerchantAppZeus(dt.Rows[j]["UID"].ToString());
                            obj = (object)app;
                            objType = ((object)app).GetType();
                            MerchantUID = app.MerchantAppUID;
                            FromEmail = Constants.CLIENT_SERVICE_EMAIL;
                        }
                        else // Agents
                        {
                            agent = DataAccess.DataAgentDao.GetAgent(dt.Rows[j]["UID"].ToString());
                            obj = (object)agent;
                            objType = ((object)agent).GetType();
                            AgentUID = agent.AgentUID;
                            FromEmail = Constants.RELATIONSHIP_MANAGEMENT_EMAIL;
                        }

                        if (objType != null)
                        {
                            PropertyInfo[] objPropertiesArray = objType.GetProperties();
                            string subject = string.Empty;

                            objProperties.Clear();
                            if (obj != null)
                            {
                                foreach (PropertyInfo prop in objPropertiesArray)
                                {
                                    if (!prop.PropertyType.IsGenericType)
                                        objProperties.Add(prop.Name, prop.GetValue(obj, null).ToString()); // get the values for all the properties.
                                }
                                t.getTemplateAndFill_FromString(Subject.Text, objProperties, ref subject);

                                if (t.getTemplateAndFill_FromString(txtHTMLBody.TextWindow.Text, objProperties, ref body)) // Replace the template fields with property values using the object
                                {
                                    body = body.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                                    body = "<html><body bgcolor='#E1E2E3'>" + body + "</body></html>";

                                    this.SendEmail(subject, "", body, FromEmail, Toadd, "", "", att, UserSessions.CurrentUser.UserName, MerchantUID, AgentUID);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void SendEmails(bool isTest)
    {
        string uid = string.Empty;
        IList<Attachments> att = new List<Attachments>();

        if (Check(isTest))
        {
            att = GetAttachements();

            string eblaster = string.Empty, Toadd = string.Empty;
            bool isMerchant = false;
            DataCommunication data = DataAccess.DataCommunicationDao;
            DataTable dt = new DataTable();
            IDictionary<string, string> EmailList = new Dictionary<string, string>();
            string body = string.Empty, PortalUIDList = string.Empty, AgentUIDList = string.Empty;

            if (rblEmail.SelectedValue == "0") // Merchants
            {
                eblaster = "M";
                isMerchant = true;
                PortalUIDList = "";
                foreach (ListItem chk in lstPortals.Items)
                {
                    if (chk.Selected)
                        PortalUIDList += chk.Value + ",";
                }
            }
            else  // Agents
            {
                eblaster = "A";
                isMerchant = false;
                AgentUIDList = "";
                foreach (ListItem chkAgnt in AgentTypeUID.Items)
                {
                    if (chkAgnt.Selected)
                        AgentUIDList += chkAgnt.Value + ",";
                }
            }

            if (eblaster != string.Empty)
            {
                Hashtable prms = new Hashtable();

                if (isMerchant)
                    prms.Add("@IsMerchant", "1");
                else
                    prms.Add("@IsMerchant", "0");


                if (string.IsNullOrEmpty(IDList.Text))
                {
                    if (isTest) // Test Email
                    {
                        if (isMerchant)
                            prms.Add("@ID", "10012");
                        else
                            prms.Add("@ID", "1086");
                    }
                    else
                    {
                        if (wucAgentSelector.m_AgentUID != string.Empty)
                        {
                            if (IncludeSubAgents.Checked)
                                prms.Add("@MasterAgentUID", wucAgentSelector.m_AgentUID);
                            else
                                prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);
                        }

                        if (isMerchant)
                        {
                            prms.Add("@PortalUIDList", PortalUIDList.TrimEnd(','));

                            if (MerchantAppTypeUID.SelectedIndex > 0)
                            {
                                prms.Add("@MerchantTypeUID", MerchantAppTypeUID.SelectedValue);
                            }
                        }
                        else
                        {
                            prms.Add("@AgentTypeUIDList", AgentUIDList.TrimEnd(','));
                        }
                    }
                }
                else
                {
                    prms.Add("@IDList", IDList.Text);
                }

                string strEmails = string.Empty;
                string MerchantUID = string.Empty;
                string AgentsUID = string.Empty;

                string FromEmail = Constants.RELATIONSHIP_MANAGEMENT_EMAIL;
                string subject = Subject.Text;

                body = txtHTMLBody.TextWindow.Text;
                body = body.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                body = "<html><body>" + body + "</body></html>";

                dt = data.GetActiveEmails(prms);

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (isTest) // Test Email
                        Toadd = txtTestEmailAddress.Text; 
                    else
                        Toadd = dt.Rows[j]["EmailList"].ToString().TrimEnd(';').Trim(); //Agent or Merchant email

                    if (!EmailList.ContainsKey(Toadd))
                    {
                        EmailList.Add(Toadd, Toadd);

                        MerchantUID = string.Empty;
                        AgentsUID = string.Empty;

                        if (isMerchant) // Merchants
                        {
                            FromEmail = Constants.CLIENT_SERVICE_EMAIL;
                            MerchantUID = dt.Rows[j]["UID"].ToString();
                        }
                        else // Agents
                        {
                            FromEmail = Constants.RELATIONSHIP_MANAGEMENT_EMAIL;
                            AgentsUID = dt.Rows[j]["UID"].ToString();
                        }

                        if (strEmails.Length + Toadd.Length > 255 || strEmails.Split(';').Length == 50)
                        {
                            this.SendEmail(subject, "", body, FromEmail, "", "", strEmails, att, UserSessions.CurrentUser.UserName, MerchantUID, AgentsUID);
                            strEmails = string.Empty;
                        }

                        if (Toadd.Trim() != string.Empty)
                        {
                            List<string> str = CommonUtility.Validation.GetValidEmails(Toadd);
                            if (str != null)
                            {
                                for (int i = 0; i < str.Count; i++)
                                    strEmails += str[i].ToString() + ";";
                            }
                        }
                    }
                }

                if (strEmails != string.Empty)
                    this.SendEmail(subject, "", body, FromEmail, "", "", strEmails, att, UserSessions.CurrentUser.UserName, MerchantUID, AgentsUID);
            }
        }
    }

    public string SaveEmail(EmailBlaster objEM)
    {
        try
        {
            DataCommunication data = DataAccess.DataCommunicationDao;
            IList<Attachments> att = new List<Attachments>();
            string AgentTypeUIDList = string.Empty, PortalUIDList = string.Empty;

            if (wucAgentSelector.m_AgentUID != string.Empty)
                objEM.AgentUIDList = wucAgentSelector.m_AgentUID;
            else
                objEM.AgentUIDList = string.Empty;

            objEM.IncludeSubAgents = IncludeSubAgents.Checked;

            if (rbMail.SelectedValue == "0")
                objEM.IsMailMerge = false;
            else
                objEM.IsMailMerge = true;

            if (rblEmail.SelectedValue == "0") // Merchants
            {
                objEM.EmailBlast = "M";
                foreach (ListItem item in lstPortals.Items)
                {
                    if (item.Selected)
                        PortalUIDList += item.Value + ",";
                }


                if (MerchantAppTypeUID.SelectedIndex > 0)
                    objEM.MerchantTypeUID = MerchantAppTypeUID.SelectedValue;

                objEM.PortalUIDList = PortalUIDList.TrimEnd(',');
            }
            else
            {
                objEM.EmailBlast = "A";// Agents
                foreach (ListItem item1 in AgentTypeUID.Items)
                {
                    if (item1.Selected)
                        AgentTypeUIDList += item1.Value + ",";
                }
                objEM.AgentTypeUIDList = AgentTypeUIDList.TrimEnd(',');
            }

            att = GetAttachements();

            objEM.Subject = Subject.Text;
            objEM.HTMLContent = txtHTMLBody.TextWindow.Text;
            objEM.UserUpdated = UserSessions.CurrentUser.UserName;
            objEM.UserCreated = UserSessions.CurrentUser.UserName;
            objEM.HasAttachments = (att != null && att.Count > 0);
            objEM.IDList = IDList.Text;

            if (EmailBlasterID.Value == string.Empty)
                data.InsertEmailBlaster(objEM);
            else
                data.UpdateEmailBlaster(objEM);

            if (objEM.EmailBlasterID != "-1")
            {
                EmailBlasterID.Value = objEM.EmailBlasterID;

                if (att != null)
                {
                    for (int i = 0; i < att.Count; i++)
                    {
                        att[i].EmailBlasterID = objEM.EmailBlasterID;
                        att[i].Checked = true;
                        data.InsertAttachments(att[i]);
                    }
                }
            }
        }
        catch (Exception exc)
        {
            throw exc;
        }

        return objEM.EmailBlasterID;
    }

    public bool SendEmail(string strSubject, string strBody, string strBodyHTML, string strFrom, string strTo, string strCC, string strBCC, IList<Attachments> att, string UserName, string MerchantAppUID, string AgentUID)
    {
        try
        {
            DataCommunication data = DataAccess.DataCommunicationDao;
            Communication comm = new Communication();

            comm.From = strFrom;
            comm.To = strTo;
            comm.Cc = strCC;
            comm.Bcc = strBCC;
            comm.Subject = strSubject;
            comm.Body = strBody;
            comm.HTMLBody = strBodyHTML;
            comm.IsEmail = true;
            comm.UserUpdated = UserName;
            comm.UserCreated = UserName;
            comm.TimeSent = DateTime.MinValue;
            comm.AgentUID = AgentUID;
            comm.MerchantAppUID = MerchantAppUID;
            comm.HasAttachments = (att != null && att.Count > 0);
            comm.EmailBlasterID = EmailBlasterID.Value;

            data.InsertCommunication(comm);
            ZeusWeb.Logging.EmailLog.InfoFormat("Sending Email for Subject :{0} Sending Email to: {1}", strSubject, strTo);

            return true;
        }
        catch (Exception exc)
        {
            ZeusWeb.Logging.EmailLog.ErrorFormat("Error while Sending Email for Subject :{0} Email to: {1}", strSubject, strTo);
            throw exc;
        }
    }

    protected void rblEmail_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillProperties();
    }

    protected void rblMail_SelectedIndexChanged(object sender, EventArgs e)
    {
        lstProperties.Enabled = !(rbMail.SelectedValue == "0");
    }

    private void FillProperties()
    {
        Type objType = null;
        lstProperties.Items.Clear();

        switch (rblEmail.SelectedValue)
        {
            case "0": // Merchants

                MerchantApp objApp = new MerchantApp();
                objType = ((object)objApp).GetType();
                lblFields.Text = "Merchant Fields";
                lblSelect.Text = "Portals";
                lstPortals.Visible = true;
                AgentTypeUID.Visible = false;
                pnlMerType.Visible = true;
                pnlAgent.Visible = true;
                lblIDList.Text = "Enter ZID(s) to Email - Comma-Separated Format";
                break;

            case "1": // Agents

                Agent objAgent = new Agent();
                objType = ((object)objAgent).GetType();
                lblFields.Text = "Agent Fields";
                lblSelect.Text = "Agent Types";
                lstPortals.Visible = false;
                AgentTypeUID.Visible = true;
                pnlMerType.Visible = false;
                pnlAgent.Visible = true;
                lblIDList.Text = "Enter Agent ID(s) to Email - Comma-Separated Format";

                break;
        }

        if (objType != null)
        {
            List<string> item = new List<string>();

            PropertyInfo[] objPropertiesArray = objType.GetProperties();

            foreach (PropertyInfo prop in objPropertiesArray)
            {

                if (!prop.PropertyType.IsGenericType)
                    item.Add(prop.Name);
            }
            item.Sort();

            for (int k = 0; k < item.Count; k++)
                lstProperties.Items.Add(new ListItem(item[k]));
        }
    }

    public void LoadTypes()
    {
        foreach (ListItem item in AgentTypeUID.Items)
        {
            item.Selected = true;
        }

        foreach (ListItem item1 in lstPortals.Items)
        {
            item1.Selected = true;
        }
    }

    public void LoadValues(EmailBlaster comm)
    {
        FormBinding.BindObjectToControls(comm, pnlDetail);

        txtHTMLBody.Text = comm.HTMLContent;
        lstCustomAttachments.Items.Clear();
        rblEmail.SelectedIndex = 0;
        rbMail.SelectedIndex = 0;
        lblMessage.Text = "";
        lblError.Text = "";
        LoadTypes();

        if (comm.Attachments != null)
        {
            foreach (Attachments item in comm.Attachments)
            {
                lstCustomAttachments.Items.Add(new ListItem(item.Name, item.FileName + ";" + item.AttachmentID));
            }
        }

        if (comm.EmailBlast.Trim() == "M")
        {
            rblEmail.SelectedIndex = 0; FillProperties();
            ListHandler.SetListSelectedItemValues(lstPortals, comm.PortalUIDList);
            //ListHandler.ListFindItem(AgentUID, comm.AgentUIDList);
            wucAgentSelector.m_AgentDBA = comm.AgentDBA;
            wucAgentSelector.m_AgentID = comm.AgentID.ToString();
            wucAgentSelector.m_AgentUID = comm.AgentUIDList;
            ListHandler.ListFindItem(MerchantAppTypeUID, comm.MerchantTypeUID);
        }
        else if (comm.EmailBlast.Trim() == "A")
        {
            if (comm.AgentID.ToString() != string.Empty)
            {
                wucAgentSelector.m_AgentDBA = comm.AgentDBA;
                wucAgentSelector.m_AgentID = comm.AgentID.ToString();
                wucAgentSelector.m_AgentUID = comm.AgentUIDList;
            }

            rblEmail.SelectedIndex = 1; FillProperties();
            ListHandler.SetListSelectedItemValues(AgentTypeUID, comm.AgentTypeUIDList);
        }

        if (comm.IsMailMerge)
            ListHandler.ListFindItem(rbMail, "1");
        else
            ListHandler.ListFindItem(rbMail, "0");
    }

}
