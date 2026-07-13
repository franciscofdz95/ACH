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
using PaymentXP.BusinessObjects;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class UserControls_wucMeritusNewsDetail : System.Web.UI.UserControl
{
    private string _template_marker_start = "<!--TEMPLATE_BEGIN";
    private string _template_marker_end = "TEMPLATE_END-->";

    public MeritusNews ObjMN
    {
        get { return (MeritusNews)ViewState["ObjMN"]; }
        set { ViewState["ObjMN"] = value; }
    }

    public String CSubject
    {
        get { return Subject.Text; }
    }

    public String CNewsDate
    {
        get { return NewsDate.Text; }
    }

    public string CDisabledDate
    {
        get { return DisabledDate.Text; }
    }

    public string CUrl
    {
        get { return URL.Text; }
    }

    public string CPortalUID
    {
        get { return PortalUID.SelectedValue; }
    }


    public string CHTMLContent
    {
        get { return HTMLContent.Text; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.init_always();

        if (!Page.IsPostBack)
        {
            this.initialize();
        }
    }

    protected void initialize()
    {
        LookupTableHandler.LoadPortals(PortalUID, false);
        PortalUID.Items.Add(new ListItem("Select Portal", ""));
    }

    protected void init_always()
    {

    }

    public void Clear()
    {
        FormHandler.ClearAllControls(pnlDetail);
        this.ObjMN = null;
        this.NewsID.Text = "";
        this.HTMLContent.Text = "";
    }

    public bool FillMeritusNews(MeritusNews objMN)
    {
        bool blnRet = false;

        if (objMN != null)
        {
            FormBinding.BindObjectToControls(objMN, pnlDetail);

            // a lazy check to make sure our panel has it's objects fill with data.
            if (!string.IsNullOrEmpty(NewsID.Text))
            {
                blnRet = true;
                this.ObjMN = objMN;


                if (NewsDate.Value != null && ((DateTime)NewsDate.Value).Date == DateTime.MinValue.Date)
                {
                    NewsDate.Value = null;
                }

                if (DisabledDate.Value != null && ((DateTime)DisabledDate.Value).Date == DateTime.MaxValue.Date)
                {
                    DisabledDate.Value = null;
                }
            }
        }

        return blnRet;


    }

    protected void UserControls_wucMeritusNewsDetail_PreRender(object sender, EventArgs e)
    {
        if (NewsDate.Value != null && (DateTime)NewsDate.Value == DateTime.MinValue)
        {
            NewsDate.Value = null;
        }

        if (DisabledDate.Value != null && (DateTime)DisabledDate.Value == DateTime.MaxValue)
        {
            DisabledDate.Value = null;
        }
    }

    public void BindControlsToObject()
    {
        FormBinding.BindControlsToObject(this.ObjMN, pnlDetail);
    }

    public void SetControlEditMode(bool isEdit)
    {
        FormHandler.SetControlEditMode(pnlDetail, isEdit);
    }

    protected void PortalUID_SelectedIndexChanged(object sender, EventArgs e)
    {

        switch (PortalUID.SelectedValue.ToUpper())
        {
            case "E263D06C-A6F1-4C27-9103-231D8FEE85C5"://	Zeus
                URL.Text = "";
                break;

            case "76411203-7F8E-4FC1-9CDC-9CF0C8084611"://	Merchant
                URL.Text = "~/ContactForms/frmNews.aspx";
                break;

            case "8D37E9F5-4094-4D92-987F-C3E642E6B092"://	Agent
                URL.Text = "~/web/secureHomeForms/frmNews.aspx";
                break;

            case "4A77C310-4264-45C6-96C1-F7EFE61C7D2E"://	PaymentXP
                URL.Text = "~/FormHome/frmNews.aspx";
                break;
            default:
                URL.Text = string.Empty;
                break;
        }

    }

    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;

        string new_text = "";

        switch (ddl.SelectedValue)
        {
            case "0":
                new_text = this.strip_template(HTMLContent.Text);
                HTMLContent.Text = new_text;
                break;

            
            case "1":
                
                // remove everything between template markers.

                new_text = this.strip_template(HTMLContent.Text);
                HTMLContent.Text = string.Format("{0}top begin{1}{3}{0}top end{1}{2}{0}bottom begin{1}{4}{0}bottom end{1}", this._template_marker_start, this._template_marker_end, new_text, this.Get_MeritusStandard_Top(), this.Get_MeritusStandard_Bottom());

                break;
        }
    }

    private string Get_MeritusStandard_Top()
    {
        return @"
<style type='text/css'>
#Table_01 tr td {
	background-repeat: repeat-x;
	background-image: url('http://dev.zeus/images/template_Body_BG-06.jpg');
	vertical-align: 5%;
	font-size: 12px;
}
#Table_01 tr td #Footer {
	background-image: url('http://dev.zeus/images/template_Footer_bg.jpg');
	background-repeat: repeat;
}
.Info_txt {
	font-family: Arial, Helvetica, sans-serif;
	font-size: 12px;
	color: #00a8db;
}
body,td,th {
	font-family: 'Arial Black', Gadget, sans-serif;
	font-size: 12px;
	color: #000;
}
.Headline {
	font-size: 30px;
	color: #004478;
	line-height: 30px;
}
BodyCopy {
	line-height: 15px;
}
.BodyCopy {
	font-size: 12px;
	line-height: 18px;
}
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
</style>
<table id='Table_01' width='1000' border='0' cellpadding='0' cellspacing='0'>
    <tr>
        <td width='100' rowspan='4'>
            &nbsp;</td>
        <td colspan='3'>
            <img id='Header' src='http://dev.zeus/images/template_Header.jpg' width='800'
                height='100' alt='' /></td>
        <td width='100' rowspan='4'>
            &nbsp;</td>
    </tr>
    <tr>
        <td colspan='3'>
            <img id='Body_BG' src='http://dev.zeus/images/template_Body_BG.jpg' width='800'
                height='25' alt='' /></td>
    </tr>
    <tr>
        <td width='24'>
            &nbsp;</td>
        <td width='750' valign='top'>
".Replace('\r', ' ').Replace('\n',' ');
    }

    private string Get_MeritusStandard_Bottom()
    {
        return @"
  </td>
        <td width='24'>
            &nbsp;</td>
    </tr>
    <tr>
        <td colspan='3' valign='top'>
            <img id='Footer' src='http://dev.zeus/images/template_Footer.jpg' width='800'
                height='200' alt='' /></td>
    </tr>
</table>
".Replace('\r', ' ').Replace('\n',' ');
    }

    private string strip_template(string text)
    {

        Regex r = new Regex( string.Format(@"{0}.+?{1}", this._template_marker_start, this._template_marker_end));
        //Regex r = new Regex(  "<!--TEMPLATE_BEGIN.+?TEMPLATE_END-->");

        MatchCollection MatchList = r.Matches(text);

        List<string> liPairs = new List<string>();

        string first = "";
        bool isfirst = true;
        foreach (Match m in MatchList)
        {
            if (isfirst)
            {
                first = m.Value;
            }
            else
            {
                liPairs.Add(string.Format(@"{0}.+?{1}", first, m.Value));
            }

            isfirst = !isfirst;
        }

        string newtext = text;

        foreach (string str in liPairs)
        {
            newtext = Regex.Replace(newtext, str, "");
        }

        return newtext;

        //textBox3.Text = Regex.Replace(textBox1.Text, textBox2.Text, "");
    }

}

