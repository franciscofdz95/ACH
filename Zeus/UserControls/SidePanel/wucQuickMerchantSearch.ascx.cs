using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Text;


public partial class wucQuickMerchantSearch : System.Web.UI.UserControl
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ZID.Text = string.Empty;
        MID.Text = string.Empty;
        lblErr.Text = "";
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        lblErr.Text = "";

        MerchantApp app = new MerchantApp();
        Hashtable prms = new Hashtable();
        int zid;
        long fmaid;

        if(!string.IsNullOrWhiteSpace(this.ZID.Text) && int.TryParse(this.ZID.Text, out zid))
        {
            prms.Add("@ID", zid);
        }
        else if (!string.IsNullOrWhiteSpace(this.ZID.Text))
        {
            lblErr.Text = "Please enter a valid ZID.";
            return;
        }

        if (!string.IsNullOrWhiteSpace(this.FMAID.Text) && long.TryParse(this.FMAID.Text, out fmaid))
        {
            prms.Add("@FMAID", fmaid);
        }
        else if (!string.IsNullOrWhiteSpace(this.FMAID.Text))
        {
            lblErr.Text = "Please enter a valid FMAID.";
            return;
        }

        if (MID.Text != string.Empty)
        {
            prms.Add("@SettlePlatformMid", MID.Text);
        }

        if (UserSessions.CurrentUser.IsInternal)
        {
            prms.Add("@InternalUserUID", UserSessions.CurrentUser.UID);
        }

        if (prms.Count > 0)
        {
            app = DataAccess.DataMerchantAppDao.FillMerchantApp(prms);

            if (app != null)
            {

                // note: we always want to purge existing params. 
                // some urls rely on the merchantid and some other id.. if we just swap out the merchantid, then it might create a mismatch.
                // better we just throw an error than have bad data.
                Dictionary<string, string> di = new Dictionary<string,string>();
                di["MerchantAppUID"] = app.MerchantAppUID;

                 // add ACHID to the url for the MerchantAch page
                string strPage = System.IO.Path.GetFileName(Request.Path).Replace(".aspx", "").ToUpper();
                if (strPage == "FRMMERCHANTACH")
                {
                    di["AchID"] = app.AchID.ToString();
                }

                string url = string.Format(WebUtil.GetMyUrl(di, true));

                Response.Redirect(url);
            }
            else
            {
                lblErr.Text = "Enter a valid merchant";
            }
        }
        else
        {
            lblErr.Text = "Enter a valid merchant";
        }
    }
}
