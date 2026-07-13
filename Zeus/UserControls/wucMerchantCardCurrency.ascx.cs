using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls
{
    public partial class wucMerchantCardCurrency : System.Web.UI.UserControl
    {
        private bool MultiCurrency
        {
            get
            {
                if (ViewState["MultiCurrency"] == null)
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean(ViewState["MultiCurrency"]);
                }
            }
            set { ViewState["MultiCurrency"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SetDataSource(MerchantApp app)
        {
            this.MultiCurrency = false;

            if (app.AuthPlatformUID.ToUpper() == AuthorizationPlatforms.Borgun
                || app.AuthPlatformUID.ToUpper() == AuthorizationPlatforms.Compass)
            {
                Hashtable prms = new Hashtable();
                prms.Add("@MerchantID", app.ID);
                prms.Add("@PlatformUID", app.AuthPlatformUID);

                this.gvCardCurrency.DataSource = DataMerchantAppPaging.GetMerchantCardCurrency(prms);
                this.gvCardCurrency.DataBind();

                //single currency logic
                SetSingleCurrencyMode();
            }
            else
            {
                this.Visible = false;
            }
        }

        private void SetSingleCurrencyMode()
        {
            StringBuilder js = new StringBuilder();
            
            js.AppendLine(@"    var curRowIndex = 0;");
            js.AppendLine(@"    $('#" + gvCardCurrency.ClientID + " :checkbox').change(function () {");
            js.AppendLine(@"        var rowIndex = $(this).closest('tr').get(0).rowIndex; ");
            js.AppendLine(@"        if (rowIndex != curRowIndex) {");
            js.AppendLine(@"            curRowIndex = rowIndex;");
            js.AppendLine(@"            enforceSingleCurrency(rowIndex);");
            js.AppendLine(@"        }");
            js.AppendLine(@"    });");
            js.AppendLine(@"    function enforceSingleCurrency(rowIndex) {");
            js.AppendLine(@"        $('#" + gvCardCurrency.ClientID + " tr').each(function () {");
            js.AppendLine(@"            if ($(this).get(0).rowIndex != rowIndex) {");
            js.AppendLine(@"                $(this).find(':checkbox').removeAttr('checked');");
            js.AppendLine(@"            }");
            js.AppendLine(@"        });");
            js.AppendLine(@"    }");
            
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SingleCurrency.js", js.ToString(), true);
        }

        public bool FormDataCheck()
        {
            if (this.MultiCurrency)
                return true;

            int currencyCount = 0;

            foreach (GridViewRow row in this.gvCardCurrency.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkVisa = row.FindControl("chkVisa") as CheckBox;
                    CheckBox chkMC = row.FindControl("chkMastercard") as CheckBox;
                    CheckBox chkAmex = row.FindControl("chkAmex") as CheckBox;
                    CheckBox chkDiscover = row.FindControl("chkDiscover") as CheckBox;

                    if (chkVisa.Checked || chkMC.Checked || chkAmex.Checked || chkDiscover.Checked)
                    {
                        currencyCount++;
                    }
                }
            }

            return !(!this.MultiCurrency && currencyCount > 1);
        }

        public bool SaveCardCurrency(MerchantApp app)
        {
            //used to stored selected platform card currencies
            List<string> sCardCurrency = new List<string>();

            foreach (GridViewRow row in this.gvCardCurrency.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkVisa = row.FindControl("chkVisa") as CheckBox;
                    CheckBox chkMC = row.FindControl("chkMastercard") as CheckBox;
                    CheckBox chkAmex = row.FindControl("chkAmex") as CheckBox;
                    CheckBox chkDiscover = row.FindControl("chkDiscover") as CheckBox;

                    if (chkVisa.Checked)
                    {
                        HiddenField hdnVisa = row.FindControl("hdnVisaPCCId") as HiddenField;
                        sCardCurrency.Add(hdnVisa.Value);
                    }

                    if (chkMC.Checked)
                    {
                        HiddenField hdnMC = row.FindControl("hdnMastercardPCCId") as HiddenField;
                        sCardCurrency.Add(hdnMC.Value);
                    }

                    if (chkAmex.Checked)
                    {
                        HiddenField hdnAmex = row.FindControl("hdnAmexPCCId") as HiddenField;
                        sCardCurrency.Add(hdnAmex.Value);
                    }

                    if (chkDiscover.Checked)
                    {
                        HiddenField hdnDiscover = row.FindControl("hdnDiscoverPCCId") as HiddenField;
                        sCardCurrency.Add(hdnDiscover.Value);
                    }
                }
            }

            DataAccess.DataRiskDao.UpdateMerchantCardCurrency(int.Parse(app.ID), string.Join<string>(",", sCardCurrency));

            return true;
        }

        protected void gvCardCurrency_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnVisa = e.Row.FindControl("hdnVisaPCCId") as HiddenField;
                HiddenField hdnMC = e.Row.FindControl("hdnMastercardPCCId") as HiddenField;
                HiddenField hdnAmex = e.Row.FindControl("hdnAmexPCCId") as HiddenField;
                HiddenField hdnDiscover = e.Row.FindControl("hdnDiscoverPCCId") as HiddenField;

                CheckBox chkVisa = e.Row.FindControl("chkVisa") as CheckBox;
                CheckBox chkMC = e.Row.FindControl("chkMastercard") as CheckBox;
                CheckBox chkAmex = e.Row.FindControl("chkAmex") as CheckBox;
                CheckBox chkDiscover = e.Row.FindControl("chkDiscover") as CheckBox;

                if (int.Parse(hdnVisa.Value) > 0)
                {
                    //enable visa checkbox
                    chkVisa.Visible = true;
                    chkVisa.Checked = int.Parse(DataBinder.Eval(e.Row.DataItem, "MerchantVisaEnabled").ToString()) > 0;
                }
                
                if (int.Parse(hdnMC.Value) > 0)
                {
                    //enable MC checkbox
                    chkMC.Visible = true;
                    chkMC.Checked = int.Parse(DataBinder.Eval(e.Row.DataItem, "MerchantMCEnabled").ToString()) > 0;
                }

                if (int.Parse(hdnAmex.Value) > 0)
                {
                    //enable Amex checkbox
                    chkAmex.Visible = true;
                    chkAmex.Checked = int.Parse(DataBinder.Eval(e.Row.DataItem, "MerchantAmexEnabled").ToString()) > 0;
                }

                if (int.Parse(hdnDiscover.Value) > 0)
                {
                    //enable Discover checkbox
                    chkDiscover.Visible = true;
                    chkDiscover.Checked = int.Parse(DataBinder.Eval(e.Row.DataItem, "MerchantDiscoverEnabled").ToString()) > 0;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                //override cssclass 'mGrid' attribute for text-align for header
                foreach (TableCell cell in e.Row.Cells)
                {
                    cell.Attributes.CssStyle.Add("text-align", "center");
                }
            }
        }

        
    }
}