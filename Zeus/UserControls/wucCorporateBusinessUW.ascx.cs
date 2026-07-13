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
using Infragistics.Web.UI.EditorControls;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.BusinessObjects;
using System.Collections.Generic;
using PaymentXP.BusinessObjects.Reponse;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Text;
using PaymentXP.BusinessObjects.Request;
using System.Xml;
using iTextSharp.text.pdf;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using CommonUtility;
using ZeusWeb;

public partial class wucCorporateBusinessUW : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void FillCorporateBusiness(Owner cbOwner)
    {
        FullName.Text = cbOwner.FullName;
        HomePhone.Text = cbOwner.HomePhone;
        Email.Text = cbOwner.Email;
        PercentOwnership.Text = cbOwner.PercentOwnership.ToString();
        TaxID.Text = cbOwner.TaxID;
        Address1.Text = cbOwner.Address1;
        City.Text = cbOwner.City;
        State.Text = cbOwner.State;
        Zip.Text = cbOwner.Zip;

    }
    
}
