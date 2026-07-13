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
using CommonUtility;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Collections.Generic;

public partial class wucEmail : System.Web.UI.UserControl
{
    public string AgentEmail
    {
        get { return hfdAgent.Value; }
        set { hfdAgent.Value = value; }
    }

    public string MerchantEmail
    {
        get
        { return hfdMerchant.Value; }
        set { hfdMerchant.Value = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //lstCustomAttachments.Attributes.Add("ondblclick", "ListBox1_DoubleClick()");
            //chkMerchant.Attributes.Add("onclick", "copyMerchant()");
            //chkAgent.Attributes.Add("onclick", "copyAgent()");
            //if (!string.IsNullOrEmpty(CommunicationID.Value))
            //{
            //    LookupTableHandler.LoadAttachments(lstCustomAttachments, false, CommunicationID.Value);
            //    lstCustomAttachments.SelectedIndex = 0;
            //}
            lblMessage.Text = "";
            lblError.Text = "";
        }

        //if (Request.Params["ListBox1Hidden"] != null && (string)Request.Params["ListBox1Hidden"] == "doubleclicked")
        //{
        //    if (lstCustomAttachments.SelectedIndex == -1)
        //        lstCustomAttachments.SelectedIndex = 0;
        //    int idx = lstCustomAttachments.SelectedIndex;
        //    ListItem item = lstCustomAttachments.SelectedItem;

        //    string[] values = lstCustomAttachments.SelectedValue.Split(';');

        //    string fi = values[0].ToString().Substring(values[0].ToString().LastIndexOf("\\") + 1);
        //    string path = values[0].ToString();

        //    //get file object as FileInfo
        //    System.IO.FileInfo file = new System.IO.FileInfo(path);

        //    if (String.IsNullOrEmpty(Request.Form["0"]))   
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "EnterText", "document.forms[0].ListBox1Hidden.value = '';", true);   

        //    //-- if the file exists on the server
        //    //set appropriate headers
        //    if (file.Exists)
        //    {
        //        lstCustomAttachments.SelectedIndex = -1;
        //        Response.Clear();
        //        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
        //        Response.AddHeader("Content-Length", file.Length.ToString());
        //        Response.ContentType = "application/octet-stream";
        //        Response.WriteFile(file.FullName);
        //        Response.End();
        //        //if file does not exist
        //    }
        //    else
        //    {
        //        Response.Write("This file does not exist.");
        //    }
        //}
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        //if (this.fuplAttachments.HasFile)
        //{
        //    string fileSavePath = this.Server.MapPath("../EmailAttachment") + "\\" + UserSessions.CurrentUser.UserName;

        //    if (!Directory.Exists(fileSavePath))
        //        Directory.CreateDirectory(fileSavePath);

        //    string filename = fileSavePath + "\\" + fuplAttachments.FileName;

        //    if (File.Exists(filename))
        //        File.Delete(filename);

        //    this.fuplAttachments.SaveAs(filename);

        //    ListItem item = new ListItem();
        //    item.Text = fuplAttachments.FileName;
        //    item.Value = filename;
        //    lstCustomAttachments.Items.Add(item);
        //}
    }

    //protected void btnRemove_Click(object sender, EventArgs e)
    //{
    //    if (lstCustomAttachments.SelectedIndex != -1)
    //    {
    //        DataCommunication data = DataAccess.DataCommunicationDao;
    //        string[] values = lstCustomAttachments.SelectedItem.Value.Split(new char[] { ';' });

    //        if (values.Length == 1)
    //            lstCustomAttachments.Items.Remove(lstCustomAttachments.SelectedItem);
    //        else if (values.Length == 2)
    //        {
    //            lstCustomAttachments.Items.Remove(lstCustomAttachments.SelectedItem);
    //            data.UpdateOutboxAttachment(CommunicationID.Value, values[1].ToString(), false);
    //        }
    //    }
    //}

    private bool Check()
    {
        string error = string.Empty;

        if (To.Text.Trim() == string.Empty)
            error += "Please enter recipient(s)<br>";

        if (Subject.Text.Trim() == string.Empty)
            error += "Please enter a subject<br>";


        if (error == string.Empty)
            return true;
        else
        {
            lblError.Text = error;
            return false;
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        string uid = string.Empty;
        //Attachments att = new Attachments();

        //foreach (ListItem item in lstCustomAttachments.Items)
        //{
        //    object[] values = item.Value.Split(new char[] { ';' });

        //    if (values.Length == 1) //new attachment
        //    {
        //        FileInfo info = new FileInfo(item.Value);
        //        byte[] content = new byte[info.Length];
        //        FileStream imagestream = info.OpenRead();
        //        imagestream.Read(content, 0, content.Length);
        //        imagestream.Close();

        //        att.FileName = values[0].ToString();
        //        att.Image = content;
        //        att.Name = item.Text;
        //    }
        //    else if (values.Length == 2) //existing attachment
        //    {
        //        att = DataAccess.DataCommunicationDao.GetAttachments(values[1].ToString())[0];
        //    }
        //}

        bool perform = FormHandler.SendEmail(Subject.Text, txtHTMLBody.TextPlain, txtHTMLBody.TextWindow.Text, UserSessions.CurrentUser.Email, To.Text, Cc.Text, Bcc.Text, new Hashtable(), UserSessions.CurrentMerchantApp.MerchantAppUID);
        if (perform)
        {
            lblMessage.Text = "Message sent successfully!";
        }
        else
        {
            lblError.Text = "Message sent failed!";
        }
    }
}
