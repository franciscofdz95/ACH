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

public partial class UserControls_TestGroup : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void Add()
    {
        ArrayList al = null;
        if (ViewState["OwnerInformation"] != null)
            al = (ArrayList)ViewState["OwnerInformation"];
        else
            al = new ArrayList();

        Control ctrl = null;

        string count = string.Empty;
        Label lbl = null;
        for (int i = 0; i < al.Count; i++)
        {
            count = (i + 1).ToString();
            ctrl = LoadControl(@"~\UserControls\Test.ascx");
            ctrl.ID = "ctrl" + count;
            lbl =(Label) ctrl.FindControl("lblTitle");
            lbl.Text = "Owner " + count;
            pnlGroup.Controls.Add(ctrl);
        }

        ctrl = LoadControl(@"~\UserControls\Test.ascx");
        count = Convert.ToString(al.Count + 1);
        al.Add(count);
        ctrl.ID = "ctrl" + count;
        lbl = (Label)ctrl.FindControl("lblTitle");
        lbl.Text = "Owner " + count;
        pnlGroup.Controls.Add(ctrl);
        ViewState["OwnerInformation"] = al;

    }
}
