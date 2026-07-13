using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using PaymentXP.DataObjects;

public partial class wucTopMenu : System.Web.UI.UserControl
{
    //private bool _IsEnabled = true;

    //public bool IsEnabled
    //{
    //    get { return _IsEnabled; }
    //    set { _IsEnabled = value; }
    //}



    public string StatusBarText
    {
        get { return lblStatusBarText.Text; }
        set { lblStatusBarText.Text = value; }
    }

    public string StatusBarZIDText
    {
        get { return lblZIDText.Text; }
        set { lblZIDText.Text = value; }
    }
    //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
    public string NotificationText
    {
        get { return lblUserEditingNotice.Text; }
        set { lblUserEditingNotice.Text = value; }
    }
    /******** End of PXP-2206 **************/
    //PXP-7674 abarua
    public enum eTopMenuItems : int
    {
        NotSet = 0,
        Home,
        Merchants,
        Risk,
        Reports,
        Forms,
        Sales,
        Partners,
        FirstTeam,
        Admin,
        Tickets,
        Accounting,
        Compliance,
        Quality,
        Allocations
    }

    private eTopMenuItems _TopMenuSelect = eTopMenuItems.NotSet;

    public eTopMenuItems TopMenuSelect
    {
        get { return _TopMenuSelect; }
        set { _TopMenuSelect = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.FormShow();
        }
    }

    protected void FormShow()
    {

        if (Session != null && UserSessions.CurrentUser != null)
        {

            this.PreRender += new EventHandler(wucTopMenu_PreRender);

            if (!this.IsPostBack)
            {
                if (UserSessions.CurrentUser != null)
                {
                    lblWelcome.Text = String.Format("{0}!", UserSessions.CurrentUser.UserName);
                }


                foreach (Control c in ulMenu.Controls)
                {
                    if (c is HyperLink)
                    {
                        HyperLink h = (HyperLink)c;

                        if (h.CssClass.ToLower() == string.Format("enum_{0}", this._TopMenuSelect.ToString().ToLower()))
                        {
                            h.CssClass = h.CssClass + " current";
                        }
                        else
                        {
                            h.CssClass = h.CssClass.Replace(" current", "");
                        }

                    }
                }
            }

            // handle a special case for bank logins.
            if (UserSessions.CurrentUser.IsBank)
            {
                HyperLink h = this.GetHyperlinkByText("risk");

                if (h != null)
                {
                    h.NavigateUrl = "~/SecureRiskForms/frmRiskMonthlyReviews.aspx";
                }
            }
        }
        else
        {
            ulMenu.Visible = false;
            topright.Visible = false;
        }



        imgBeta.Visible = (
            HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower() == "zeus"    // hide beta flag on internal zeus
            || HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToLower().Contains("zeus.paysafe.com") // hide for external banks
            ) ? false : false;
    }

    protected void wucTopMenu_PreRender(object sender, EventArgs e)
    {


        //if (this._IsEnabled == false)
        //{
        //    foreach (Control c in pnlMenu.Controls)
        //    {
        //        if (c is HyperLink)
        //        {
        //            HyperLink h = (HyperLink)c;
        //            h.NavigateUrl = "";
        //            h.CssClass = "disabledText";


        //        }


        //    }
        //}
    }

    public void ToggleMenu(bool IsEnabled)
    {
        foreach (Control c in ulMenu.Controls)
        {
            if (c is HyperLink)
            {

                HyperLink h = (HyperLink)c;
                h.Enabled = IsEnabled;
                h.CssClass = (IsEnabled) ? "" : "disabledText";
            }
        }
    }

    protected HyperLink GetHyperlinkByText(string text)
    {
        HyperLink retHyp = null;

        if (!string.IsNullOrEmpty(text))
        {
            foreach (Control c in ulMenu.Controls)
            {
                if (c.GetType() == typeof(HyperLink))
                {
                    HyperLink h = (HyperLink)c;

                    if (h != null && h.Text.ToLower() == text.ToLower())
                    {
                        retHyp = h;
                    }
                }
            }
        }

        return retHyp;
    }

    protected void lblWelcome_Click(object sender, EventArgs e)
    {
        wucUserProfile1.FormShow();
        WebDialogWindow4.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

}
