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
using System.Collections.Generic;

public partial class UserControls_wucDocumentGrid : System.Web.UI.UserControl
{
    public bool EditMode
    {
        get
        {
            if (ViewState["EditMode"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["EditMode"]);
        }
        set { ViewState["EditMode"] = value; }

    }

    public List<UWConditionDocument> MyDataSource
    {
        set
        {
            ViewState["MyDataSource"] = value;
        }
        get
        {
            if (ViewState["MyDataSource"] != null)
                return (List<UWConditionDocument>)ViewState["MyDataSource"];
            else
                return new List<UWConditionDocument>();
        }
    }

    public void MyDataBind()
    {
        this.LoadDocuments();
    }

    public bool UpdateControl()
    {
        bool ret = false;

        foreach (GridViewRow grv in GridView1.Rows)
        {
            HiddenField hfCDID = (HiddenField)grv.FindControl("hidConditionDetailID");
            HiddenField hfDocID = (HiddenField)grv.FindControl("hidDocID");

            string condition_detail_id = hfCDID.Value;
            int DocID = CommonUtility.Util.if_i(hfDocID.Value, 0);

            if (DocID > 0)
            {
                DropDownList ddl = (DropDownList)grv.FindControl("ApprovalStatusID");
                TextBox tbMessageZeus = (TextBox)grv.FindControl("MessageZeus");

                Hashtable prms = new Hashtable();
                prms.Add("@ConditionDetailID", condition_detail_id);
                prms.Add("@DocID", DocID);
                List<UWConditionDocument> li = DataConditions.SelectUWConditionDocument(prms);

                if (li != null && li.Count == 1)
                {
                    UWConditionDocument obj = li[0];

                    MDoc m = DataDocuments.GetInstance().GetMDocument(DocID);

                    m.ApprovalStatusID = Convert.ToInt32(ddl.SelectedValue);

                    m.MessageZeus = tbMessageZeus.Text;
                    obj.MessageZeus = tbMessageZeus.Text;
                    // m.MessageAgent = tbMessageAgent.Text;

                    if (m.ApprovalEmailSent == DateTime.MinValue)
                    {
                        if (this.EmailDocumentApprovalToAgent(obj, (MDoc.eApprovalStatusID)m.ApprovalStatusID))
                        {
                            // mark time sent.
                            m.ApprovalEmailSent = DateTime.Now;
                        }
                    }
                    ret = DataDocuments.GetInstance().UpdateMDocument(m);
                }
            }
        }

        return ret;

    }

    private bool EmailDocumentApprovalToAgent(UWConditionDocument obj, MDoc.eApprovalStatusID eASID)
    {
        bool ret = false;
        // email to agent.
        if (eASID == MDoc.eApprovalStatusID.Denied || eASID == MDoc.eApprovalStatusID.Approved)
        {

            // first, setup template
            string mytemplate = @"
            Business DBA: [[BusinessDBAName]] ([[MerchantID]])<br />
            Condition: [[ConditionName]] ([[ConditionDetailID]])<br />
            Filename: [[Filename]]<br />
            Approval Status: [[ApprovalStatus]]<br />
            Comments: [[Comments]]
            ";

            // fill dictionary.
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("BusinessDBAName", obj.BusinessDBAName);
            di.Add("MerchantID", obj.MerchantID.ToString());
            di.Add("ConditionName", obj.ConditionName);
            di.Add("ConditionDetailID", obj.ConditionDetailID.ToString());
            di.Add("Filename", obj.OrigName);
            di.Add("ApprovalStatus", eASID.ToString());
            di.Add("Comments", CommonUtility.Util.if_s(obj.MessageZeus, "N/A"));

            // format subject
            string subject = string.Format("Document was {0} for {1}, ({2})", eASID.ToString(), obj.BusinessDBAName, obj.MerchantID.ToString());

            // fill email body
            string mybody = "";
            CommonUtility.Template t = new CommonUtility.Template();

            if (t.getTemplateAndFill_FromString(mytemplate, di, ref mybody))
            {
                // send email
                bool perform = PaymentXP.Facade.MerchantFacade.SendEmail(subject, mybody, mybody, Constants.RELATIONSHIP_MANAGEMENT_EMAIL, obj.AgentEmail, "", "", new Hashtable(), obj.MerchantUID, UserSessions.CurrentUser.UserName);
                ret = true;

                if(perform)
                    ZeusWeb.Logging.EmailLog.InfoFormat("Successfully sent email of document approval to agent for the merchant:{0} Email sent to: {1}", obj.BusinessDBAName, obj.AgentEmail);
                else
                    ZeusWeb.Logging.EmailLog.InfoFormat("Error while sending email of Document approval to agent for merchant:{0} Email to: {1}", obj.BusinessDBAName, obj.AgentEmail);
            }
            else
            {
                throw new Exception("Error in EmailDocumentApprovalToAgent(): " + mybody);
            }
        }

        return ret;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.always_init();

        if (!Page.IsPostBack)
        {
            this.initialize();
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            UWConditionDocument ucd = (UWConditionDocument)e.Row.DataItem;

            DropDownList ddl = (DropDownList)e.Row.FindControl("ApprovalStatusID");
            ddl.SelectedValue = Convert.ToString(ucd.ApprovalStatusID);

            HyperLink hl = (HyperLink)e.Row.FindControl("hypFilename");

            Dictionary<string, string> di = new Dictionary<string, string>();

            di.Add("MerchantID", ucd.MerchantID.ToString());
            di.Add("DocID", ucd.DocID.ToString());

            string encrypted_value = Server.UrlEncode(CommonUtility.Crypto.EncryptUrl(di));

            hl.NavigateUrl = string.Format("~/SecureMerchantManagementForms/frmMerchantDocumentPreview.aspx?x={0}", encrypted_value);

        }
    }

    protected void initialize()
    {
        this.LoadDocuments();
    }

    protected void always_init()
    {

    }


    protected void LoadDocuments()
    {
        if (this.MyDataSource != null && this.MyDataSource.Count > 0)
        {
            GridView1.DataSource = this.MyDataSource;
            GridView1.DataBind();

            FormHandler.SetControlEditMode(GridView1, this.EditMode);
        }
    }


    //protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    //MDoc obj = DataAccess.DataDocumentsDao.GetMDocument(Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["DocID"].ToString()));

    //    //if (obj != null)
    //    //{
    //    //    DropDownList ddp = (DropDownList)GridView1.Rows[e.RowIndex].Cells[3].FindControl("ddpType");
    //    //    obj.DocTypeID = DataLayer.Field2Int(ddp.SelectedValue);
    //    //    obj.Description = ((TextBox)GridView1.Rows[e.RowIndex].Cells[4].FindControl("txtDescription")).Text;

    //    //    DataAccess.DataDocumentsDao.UpdateMDocument(obj);

    //    //    GridView1.EditIndex = -1;
    //    //    LoadDocuments();
    //    //}


    //    string condition_document_uid = Convert.ToString(GridView1.DataKeys[e.RowIndex].Values["UID"]);
    //    int DocID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["DocID"]);

    //    DropDownList ddl = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ApprovalStatusID");
    //    TextBox tbMessageZeus = (TextBox)GridView1.Rows[e.RowIndex].FindControl("MessageZeus");
    //   // TextBox tbMessageAgent = (TextBox)GridView1.Rows[e.RowIndex].FindControl("MessageAgent");

    //    Hashtable prms = new Hashtable();
    //    prms.Add("@UID", condition_document_uid);
    //    prms.Add("@DocID", DocID);
    //    List<UWConditionDocument> li = DataConditions.SelectUWConditionDocument(prms);

    //    if (li != null && li.Count == 1)
    //    {
    //        UWConditionDocument obj = li[0];

    //        MDoc m = DataDocuments.GetInstance().GetMDocument(DocID);

    //        m.ApprovalStatusID = Convert.ToInt32(ddl.SelectedValue);

    //        m.MessageZeus = tbMessageZeus.Text;
    //       // m.MessageAgent = tbMessageAgent.Text;

    //        DataDocuments.GetInstance().UpdateMDocument(m);

    //        GridView1.EditIndex = -1;
    //        this.LoadDocuments();
    //    }


    //}

    //protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    GridView1.EditIndex = e.NewEditIndex;
    //    LoadDocuments();
    //}

    //protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView1.EditIndex = -1;
    //    LoadDocuments();
    //}
}
