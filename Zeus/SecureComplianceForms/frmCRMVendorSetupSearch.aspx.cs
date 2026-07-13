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
using PaymentXP.BusinessObjects;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PaymentXP.Facade;


public partial class frmCRMVendorSetupSearch : frmBaseSearch
    {
        private int MAX_CHARACTERS_TO_DISPLAY = 40;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script type='text/javascript'>force_min();</script>", false);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ((HyperLink)this.Master.FindControl("lnkSearchCRM")).CssClass = "active";

            if (!IsPostBack)
            {
                CRMID.Focus();
                LookupTableHandler.LoadInternalUsers(UserCreated, true);
                //Apply security settings
                FormHandler.SetSecurity(this.Page);
                SortOrder = "CRMID";
                SortDirectionSearch = SortDirection.Descending;

            }


        }

        protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            //validate search params
            string valError = ValidateSearchParams();

            if (!string.IsNullOrWhiteSpace(valError))
            {
                FormHandler.DisplayMessage(Page.ClientScript, valError);
                return;
            }

            SearchParameters = null;
            CurrentPage = 1;
            grd.PageIndex = 0;
            SortOrder = "CRMID";
            SortDirectionSearch = SortDirection.Descending;

            Search(false);
        }

        protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            this.FormClear();
        }

        protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            string url = "~/SecureComplianceForms/frmCRMVendorSetupDetail.aspx?Adding=true";
            url += "&PostBackURL=~/SecureComplianceForms/frmCRMVendorSetupSearch.aspx";
            Response.Redirect(url);
        }

        protected void odsCRM_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //hash table is use to store parameters which will be passed to the stored procedure
            Hashtable prms = new Hashtable();
            SearchParameter CRM = null;

            if (CRMName.Text != string.Empty)
                prms.Add("@CRMName", CRMName.Text);

            if (UserCreated.SelectedIndex > 0)
                prms.Add("@UserCreated", UserCreated.SelectedItem.Value);


            if (CRMType.Text != string.Empty)
                prms.Add("@CRMType", CRMType.Text);

            DateTime date;
            if (!string.IsNullOrEmpty(PCIValidationDate.Text) && DateTime.TryParseExact(this.PCIValidationDate.Text.Trim(), UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                prms.Add("@PCIValidationDate", date);

            if (!string.IsNullOrEmpty(LastScannedDate.Text) && DateTime.TryParseExact(this.LastScannedDate.Text.Trim(), UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                prms.Add("@LastScannedDate", date);

            if (CRMID.Text != string.Empty)
                prms.Add("@CRMID", CRMID.Text.Trim());           


            //If procedure is called for the first time pass a dummy parameter to initial the grid
            if (prms.Count > 0)
            {
                if (UserSessions.CurrentUser.IsInternal)
                    prms.Add("@InternalUserUID", UserSessions.CurrentUser.UID);

                //Save search fields in session variable
                CRM = new SearchParameter();
                FormBinding.BindControlsToObject(CRM, pnlSearch);

                this.SearchParameters = CRM;

                prms.Add("@PageSize", this.PageSize);
                prms.Add("@CurrentPage", this.CurrentPage);
                grd.PageSize = this.PageSize;
                grd.PageIndex = this.CurrentPage - 1;

                if (this.SortOrder == string.Empty)
                    this.SortOrder = "@CRMID";
                prms.Add("@SortOrder", this.SortOrder);

                prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));
            }
            else
            {
                prms.Add("@CRMID", 0);
            }



            e.InputParameters[0] = prms;
            e.InputParameters[3] = this.grd.ID;
            lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetCRMPagingCount(prms, 0, 0, this.grd.ID).ToString();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                    HyperLink btn = (HyperLink)e.Row.FindControl("hypTID");
                    btn.Attributes.Add("title", this.BuildTooltip(((DataRowView)e.Row.DataItem).Row));

                    break;
                case DataControlRowType.Footer:
                    break;
                default:
                    break;
            }
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            this.CurrentPage = e.NewPageIndex + 1;
            this.Search(false);
        }

        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.CurrentPage = 1;
            this.SortOrder = e.SortExpression;
            this.SortDirectionSearch = e.SortDirection;
            this.Search(false);
        }

        protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = 1;
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
            this.Search(false);
        }

        private void ClearGrid()
        {
            grd.DataSourceID = string.Empty;
            grd.DataBind();
        }

        public override void Search(bool IsOnLoad)
        {
            //Populate search fields        
            if (IsOnLoad && this.SearchParameters != null)
            {
                SearchParameter CRM = (SearchParameter)this.SearchParameters;
                FormBinding.BindObjectToControls(CRM, pnlSearch);

            }
           
            grd.DataBind();

            if (IsOnLoad)
                grd.Sort(this.SortOrder, this.SortDirectionSearch);

            pnlRecords.Visible = grd.Rows.Count > 0;
            pnlNoRecords.Visible = grd.Rows.Count == 0;


        }

        private void FormClear()
        {
            this.SearchParameters = null;
            FormHandler.ClearAllControls(this);
            //   grd.Columns.Clear();
            ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
            grd.PageIndex = 0;
            this.Search(false);

            pnlRecords.Visible = false;
            pnlNoRecords.Visible = true;

        }

        private int ConvertSortDirectionToSql(SortDirection direction)
        {
            int newSortDirection;

            switch (direction)
            {
                case SortDirection.Descending:
                    newSortDirection = 1;
                    this.SortDirectionSearch = SortDirection.Descending;
                    break;

                default:
                    newSortDirection = 0;
                    this.SortDirectionSearch = SortDirection.Ascending;
                    break;
            }
            return newSortDirection;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (rdExport.SelectedValue.Equals("1"))
            {
                this.PageSize = 5000;
                this.CurrentPage = 1;
            }
            else
            {
                this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
                this.CurrentPage = grd.PageIndex + 1;
            }
            TogglegridFields(true);
            Search(false);
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            FormHandler.Export2Excel("TPPList.xls", grd);
            TogglegridFields(false);
        }

        public void TogglegridFields(bool visible)
        {
            for (int i = 0; i < grd.Columns.Count; i++)
            {
                if (grd.Columns[i].ItemStyle.CssClass == "togle")
                {
                    grd.Columns[i].Visible = visible;
                }
            }
        }

        

        private string BuildTooltip(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            rdExport.SelectedValue = "0";
            sb.Append("<table class='mGrid'>");

            if (dr.Table.Columns.Contains("DueDate"))
            {
                sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "Due Date", WebUtil.ConvertToUserDateTimeSettings(dr["DueDate"].ToString()));
            }

            if (dr.Table.Columns.Contains("Days"))
            {
                sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "Days Aged", dr["Days"].ToString());
            }

            if (dr.Table.Columns.Contains("Tags"))
            {
                sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "Tags", dr["Tags"].ToString());
            }

            if (dr.Table.Columns.Contains("MLEName") && dr["IsMLETicket"].ToString() == "True")
            {
                sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "MLE", dr["MLEName"].ToString());
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        private string ValidateSearchParams()
        {
            StringBuilder sb = new StringBuilder();
            DateTime date;

            if (!string.IsNullOrEmpty(this.PCIValidationDate.Text)
                && !DateTime.TryParseExact(this.PCIValidationDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                this.PCIValidationDate.Text = string.Empty;
                sb.Append("Please enter a valid PCI Validation Date.\\n");
            }

            if (!string.IsNullOrEmpty(this.LastScannedDate.Text)
                && !DateTime.TryParseExact(this.LastScannedDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                this.LastScannedDate.Text = string.Empty;
                sb.Append("Please enter a valid Last Scanned Date.\\n");
            }

            return sb.ToString();
        }
    }
