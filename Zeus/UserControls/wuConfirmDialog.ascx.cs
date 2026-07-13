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

public partial class wuConfirmDialog : System.Web.UI.UserControl
{
    public delegate void ButtonClickHandler(object sender, EventArgs e);
    public event ButtonClickHandler ButtonClick;
    //public event ButtonClickHandler ButtonClick2;

    public void SetValue(bool val)
    {
        txtDDA.Visible = val;
        lblDDA.Visible = val;

        txtTaxID.Visible = val;
        lblTaxID.Visible = val;

        txtMID.Visible = val;
        lblMID.Visible = val;

        txtSSN.Visible = val;
        lblSSN.Visible = val;

        txtBMID.Visible = val;
        lblBMID.Visible = val;

        txtRoute.Visible = val;
        lblRoute.Visible = val;

        if (!val)
            txtSSN.Text = txtMID.Text = txtTaxID.Text = txtDDA.Text = txtBMID.Text = txtRoute.Text = string.Empty;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.ButtonClick != null)
        {
            this.ButtonClick(sender, e);
        }
    }
}
