using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.Facade;

namespace ZeusWeb.UserControls
{
    public partial class wucQIRlookup : System.Web.UI.UserControl
    {
        public Unit txtQIRCodeWidth
        {
            set { ViewState["txtQIRCodeWidth"] = value; }
            get
            {
                if (ViewState["txtQIRCodeWidth"] == null)
                    return Unit.Pixel(90);
                else
                    return (Unit)ViewState["txtQIRCodeWidth"];
            }
        }

        public Unit txtCompanyWidth
        {
            set { ViewState["txtCompanyWidth"] = value; }
            get
            {
                if (ViewState["txtCompanyWidth"] == null)
                    return Unit.Pixel(132);
                else
                    return (Unit)ViewState["txtCompanyWidth"];
            }
        }

        public Unit lblQIRCodeWidth
        {
            set { ViewState["lblQIRCodeWidth"] = value; }
            get
            {
                if (ViewState["lblQIRCodeWidth"] == null)
                    return Unit.Pixel(60);
                else
                    return (Unit)ViewState["lblQIRCodeWidth"];
            }
        }

        public Unit lblCompanyWidth
        {
            set { ViewState["lblCompanyWidth"] = value; }
            get
            {
                if (ViewState["lblCompanyWidth"] == null)
                    return Unit.Pixel(60);
                else
                    return (Unit)ViewState["lblCompanyWidth"];
            }
        }

        public bool txtCompanyReadonly
        {
            set { ViewState["txtCompanyReadonly"] = value; }
            get
            {
                if (ViewState["txtCompanyReadonly"] == null)
                    return false;
                else
                    return (bool)ViewState["txtCompanyReadonly"];
            }
        }

        public bool txtQIRCodeReadonly
        {
            set { ViewState["txtQIRCodeReadonly"] = value; }
            get
            {
                if (ViewState["txtQIRCodeReadonly"] == null)
                    return false;
                else
                    return (bool)ViewState["txtQIRCodeReadonly"];
            }
        }

        public short QIRLookupButtonTabIndex
        {
            set { ViewState["QIRLookupButtonTabIndex"] = value; }
            get
            {
                if (ViewState["QIRLookupButtonTabIndex"] == null)
                    return 0;
                else
                    return (short)ViewState["QIRLookupButtonTabIndex"];
            }
        }

        public string m_QIRId
        {
            get { return txtQIRVID.Text; }
            set { txtQIRVID.Text = value; }
        }

        public string m_SicCodeDesc
        {
            get { return txtQIRCompany.Text; }
            set { txtQIRCompany.Text = value; }
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.PreRender += new EventHandler(wucQIRlookup_PreRender);
        }

        protected void wucQIRlookup_PreRender(object sender, EventArgs e)
        {
            txtQIRVID.Width = txtQIRCodeWidth;
            txtQIRCompany.Width = txtCompanyWidth;
            lblQIRID.Width = lblQIRCodeWidth;
            lblQIRCompany.Width = lblCompanyWidth;
            txtQIRVID.ReadOnly = txtCompanyReadonly;
            txtQIRCompany.ReadOnly = txtQIRCodeReadonly;

            if (this.QIRLookupButtonTabIndex > 0)
                btnLookup.TabIndex = this.QIRLookupButtonTabIndex;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnLookup_Click(object sender, EventArgs e)
        {
            WebDialogWindow1.WindowState = DialogWindowState.Normal;
        }

        //protected void grdQIR_RowDataBound(object sender, GridViewRowEventArgs e)
        //{

        //    switch (e.Row.RowType)
        //    {
        //        case DataControlRowType.DataRow:

        //            break;
        //        default:
        //            break;
        //    }
        //}

        protected void grdQIR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Select":


                    LinkButton lb = (LinkButton)e.CommandSource;

                    string code = ((Label)(lb.Parent.FindControl("lblID"))).Text;
                    string desc = ((Label)(lb.Parent.FindControl("lblCompany"))).Text.Replace("'", "^");

                    txtQIRVID.Text = code;
                    //GridViewRow gvrow = lb.NamingContainer as GridViewRow;
                    //QIRID = grdQIR.DataKeys[gvrow.RowIndex].Value.ToString();
                    //QIRVendorID.Value = grdQIR.DataKeys[gvrow.RowIndex].Value.ToString();
                    txtQIRCompany.Text = Server.HtmlDecode(desc.Replace("^", "'"));

                    WebDialogWindow1.WindowState = DialogWindowState.Hidden;

                    break;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtQIR.Text = string.Empty;
            grdQIR.DataSource = null;
            grdQIR.DataBind();
            lblNoRecords.Visible = true;
        }

        private void SearchQIRVendor()
        {
            MerchantFacade facade = new MerchantFacade();
            Hashtable prms = new Hashtable();

            if (txtQIR.Text.Trim() != string.Empty)
                prms.Add("@Company", txtQIR.Text);


            DataSet ds = facade.GetQIRVendors(prms);
            DataView dv = ds.Tables[0].DefaultView;
            grdQIR.DataSource = dv;
            grdQIR.DataBind();

            lblNoRecords.Visible = (grdQIR.Rows.Count == 0);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.SearchQIRVendor();
        }

       
    }
}