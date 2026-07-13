using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for wucBaseDataEntry
/// </summary>
public abstract class wucBaseDataEntry : System.Web.UI.UserControl
{
    public wucBaseDataEntry()
    {
        //
        // TODO: Add constructor logic here
        //
    }

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

    public bool Adding
    {
        get { return Convert.ToBoolean(ViewState["Adding"]); }
        set { ViewState["Adding"] = Convert.ToBoolean(value); }
    }

    public string UIDName
    {
        get { return Convert.ToString(ViewState["UIDName"]); }
        set { ViewState["UIDName"] = Convert.ToString(value); }
    }

    public int RecordIndex
    {
        get
        {
            if (ViewState["RecordIndex"] == null)
                return -1;
            else
                return Convert.ToInt32(ViewState["RecordIndex"]);
        }
        set { ViewState["RecordIndex"] = Convert.ToInt32(value); }
    }

    public string UID
    {
        get { return Convert.ToString(ViewState["UID"]); }
        set { ViewState["UID"] = Convert.ToString(value); }
    }
    
    public abstract void FormShow(string ID);

    public abstract void FormClear();

    public abstract bool FormSave();

    public abstract void FormNew();

    public abstract bool FormDelete();

    public abstract bool FormDataCheck();

    public abstract void FormCancel();

    public abstract void ToggleButtons();
}
