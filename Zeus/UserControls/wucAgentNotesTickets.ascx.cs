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
using System.Text;

using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;

using Infragistics.WebUI.WebHtmlEditor;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using System.Collections.Generic;

public partial class wucAgentNotesTickets : wucBaseSearch
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
        }
    }

    public void FormShow()
    {
 
    }
}