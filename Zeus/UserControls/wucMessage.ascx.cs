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
using System.Collections.Generic;

public partial class UserControls_wucMessage : System.Web.UI.UserControl
{
    private const string KEY_STATUS = "wuc_message_key_status";
    private const string KEY_ERROR = "wuc_message_key_error";
    private const string KEY_SUCCESS = "wuc_message_key_success";

    private List<string> _liError;
    private List<string> _liStatus;
    private List<string> _liSuccess;

    private bool _DisplayMessages = true;

    /// <summary>
    /// if this is set to false, you can still add messages, but it just won't display them. this is good when you have an embedded
    /// control, that needs to add messages, but doesn't need to render them, letting the parent page render them.
    /// </summary>
    public bool DisplayMessages
    {
        get { return _DisplayMessages; }
        set { _DisplayMessages = value; }
    }

    public void AddMessageError(string str)
    {
        if (this._liError == null)
        {
            this._liError = new List<string>();
        }

        if (Session[KEY_ERROR + Session.SessionID] != null)
        {
            this._liError = (List<string>)Session[KEY_ERROR + Session.SessionID];
        }

        this._liError.Add(str);


        Session[KEY_ERROR + Session.SessionID] = this._liError;
    }

    public void AddMessageStatus(string str)
    {
        if (this._liStatus == null)
        {
            this._liStatus = new List<string>();
        }

        if (Session[KEY_STATUS + Session.SessionID] != null)
        {
            this._liStatus = (List<string>)Session[KEY_STATUS + Session.SessionID];
        }

        this._liStatus.Add(str);

        Session[KEY_STATUS + Session.SessionID] = this._liStatus;
    }

    public void AddMessageSuccess(string str)
    {
        if (this._liSuccess == null)
        {
            this._liSuccess = new List<string>();
        }

        if (Session[KEY_SUCCESS + Session.SessionID] != null)
        {
            this._liSuccess = (List<string>)Session[KEY_SUCCESS + Session.SessionID];
        }

        this._liSuccess.Add(str);

        Session[KEY_SUCCESS + Session.SessionID] = this._liSuccess;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        blStatus.Items.Clear();
        blError.Items.Clear();
        blSuccess.Items.Clear();

        this.PreRender += new EventHandler(web_Controls_wucMessage_PreRender);
    }

    protected void web_Controls_wucMessage_PreRender(object sender, EventArgs e)
    {
        if (Session[KEY_STATUS + Session.SessionID] != null)
        {
            this._liStatus = (List<string>)Session[KEY_STATUS + Session.SessionID];
            foreach (string str in this._liStatus)
            {
                blStatus.Items.Add(new ListItem(str));
            }
        }

        if (Session[KEY_ERROR + Session.SessionID] != null)
        {
            this._liError = (List<string>)Session[KEY_ERROR + Session.SessionID];
            foreach (string str in this._liError)
            {
                blError.Items.Add(new ListItem(str));
            }
        }

        if (Session[KEY_SUCCESS + Session.SessionID] != null)
        {
            this._liSuccess = (List<string>)Session[KEY_SUCCESS + Session.SessionID];
            foreach (string str in this._liSuccess)
            {
                blSuccess.Items.Add(new ListItem(str));
            }
        }

        Session[KEY_STATUS + Session.SessionID] = null;
        Session[KEY_ERROR + Session.SessionID] = null;
        Session[KEY_SUCCESS + Session.SessionID] = null;

    }

    public int ErrorCount()
    {
        int ret = 0;
        
        List<string> li = (List<string>)Session[KEY_ERROR + Session.SessionID];

        if (li != null)
        {
            ret = li.Count;
        }

        return ret;
    }

    public int StatusCount()
    {
        int ret = 0;

        List<string> li = (List<string>)Session[KEY_STATUS + Session.SessionID];

        if (li != null)
        {
            ret = li.Count;
        }

        return ret;
    }

}
