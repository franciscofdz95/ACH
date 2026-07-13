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
using PaymentXP.Facade;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.LayoutControls;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CommonUtility;
using System.Globalization;
using PaymentXP.BusinessObjects.Tickets;
using System.Linq;
using HtmlAgilityPack;
using PaymentXP.BusinessObjects.Zeus;


    public partial class wucCompliance : wucBaseDataEntry
    {
       // Property added by koshlendra for PXP-8410 start 
        public string vsChangeHistoryFieldID
        {
            get { return CommonUtility.Util.if_i(ViewState["vsChangeHistoryFieldID"], -1).ToString(); }
            set { ViewState["vsChangeHistoryFieldID"] = value; }
        }
        // Property added by koshlendra for PXP-8410 end  


        override protected void OnInit(EventArgs e)
        {
            
            base.OnInit(e);
            EnsureChildControls();
        }
        string _WindowCallID = "";

        /// <summary>
        /// the ticket control can either be called from a page, or a window. if its called from a window, then this value gets populated.
        /// </summary>
        public string WindowCallID
        {
            get { return _WindowCallID; }
            set { _WindowCallID = value; }
        }
        protected string GetSubmitPostBack()
        {
            return Page.ClientScript.GetPostBackEventReference(btnSave, string.Empty);
        }
        private void FillDropDown()
        {
            LookupTableHandler.MerchantAppStatus(StatusUID, false, "CRM");
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {

          
            this.calCallbackDate.Format = UserSessions.CurrentUser.DatePattern;
            this._WindowCallID = CommonUtility.Util.if_s(Request.QueryString["WindowCallID"]);

            

            this.UID = CommonUtility.Util.if_s(Request.QueryString["CRMUID"], null);

           

            if (UserSessions.CurrentUser == null)
            {
                Response.Redirect("~/frmLogin.aspx");
            }

            if (!IsPostBack)
            {
                //// default document sorting, and order
                //this.SortDirectionSearch = SortDirection.Ascending;
                //this.SortOrder = "ORIGNAME";

                
                lblError.Text = "";

                FillDropDown();

                

                //set Adding flag
                if (this.Adding)
                {
                    this.FormNew();
                }
                else
                {

                   

                    if (this.UID == string.Empty)
                    {
                        this.UID = (UserSessions.CurrentCRM == null) ? string.Empty : UserSessions.CurrentCRM.CRMUID;
                    }

                    if (!string.IsNullOrEmpty(this.UID))
                    {
                        this.FormShow(this.UID);
                        //Code added for PXP-8410 by koshlendra start                
                        DropDownList ddl = (DropDownList)grdChangeHistory.HeaderRow.FindControl("ddlChangeType");
                        LookupTableHandler.LoadChangeHistoryFields(ddl, true, ChangeHistoryFields.ChangeHistoryFieldSource.Compliance);
                        ddl.SelectedValue = this.vsChangeHistoryFieldID;
                        //Code added for PXP-8410 by koshlendra end
                    }
                    else if (!string.IsNullOrEmpty(this._WindowCallID))
                    {
                        this.FormNew();
                    }
                }
                    
            }
           


        }
        protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            WebImageButton btn = (WebImageButton)sender;

            string url = string.Empty;
            switch (btn.CommandName)
            {
                case "Add":

                    if (string.IsNullOrEmpty(this.WindowCallID))
                    {
                        Response.Redirect(WebUtil.GetMyUrl("Adding=true"));
                    }
                    else
                    {
                        Response.Redirect(WebUtil.GetMyUrl("Adding=true&WindowCallID=" + this.WindowCallID));
                    }

                    this.FormNew();

                    break;


                case "Save":
                    {
                        Compliance t = this.FormSaveCRM();
                        if (t != null && !String.IsNullOrEmpty(t.CRMUID))
                        {
                            if (!string.IsNullOrEmpty(this._WindowCallID))
                            {
                                ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), _WindowCallID, "javascript:window.close();", true);
                            }
                            else
                            {
                                Response.Redirect(String.Format("~/SecureComplianceForms/frmCRMVendorSetupDetail.aspx?Adding=False&CRMUID={0}", t.CRMUID));
                            }
                        }
                    }

                    break;

                case "Refresh":


                    if (UserSessions.CurrentCRM != null)
                    {
                        Hashtable prms = new Hashtable();
                        prms.Add("@CRMUID", UserSessions.CurrentCRM.CRMUID);
                        //UserSessions.CurrentCRM = DataAccess.DataTicketDao.GetTicket(prms, UserSessions.CurrentUser.TimeZone);
                        Response.Redirect(WebUtil.GetMyUrl("Adding=false&CRMUID=" + UserSessions.CurrentCRM.CRMUID));
                    }

                    break;

                case "Cancel":

                    if (!string.IsNullOrEmpty(this._WindowCallID))
                    {
                        ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), _WindowCallID, "javascript:window.close();", true);
                    }
                    else
                    {

                        if (this.UID == string.Empty)
                        {
                            this.FormClose(sender, e);
                        }
                        else
                        {
                            this.FormCancel();
                        }
                    }
                    
                    break;

                case "Edit":

                    this.EditMode = true;
                    this.FormShow(this.UID);
                    this.ToggleButtons();
                    break;
            }
        }

        public override void FormNew()
        {
            this.FormClear();
            this.Adding = true;
            this.EditMode = true;
            FormHandler.SetControlEditMode(pnlCRMDetail, this.EditMode);
            this.ToggleButtons();
            FillDropDown();
            pnlID.Visible = false;
            imgCallbackDate.Enabled = true;

        }

        public override void FormShow(string CRMUID)
        {
            DataCompliance data = DataAccess.DataComplianceDao;
            Compliance compliance = new Compliance();
            Hashtable prms = new Hashtable();


            prms.Add("@CRMUID", CRMUID);
            compliance = data.GetCRM(prms);
            pnlID.Visible = true;
            UserSessions.CurrentCRM = compliance;



            FormBinding.BindObjectToControls(compliance, pnlCRMDetail);
            FormHandler.SetControlEditMode(pnlCRMDetail, this.EditMode);
            //Code added for PXP-8410 by koshlendra start
            LoadCRMChangeHistory();
            //Code added for PXP-8410 by koshlendra end

            if (!string.IsNullOrWhiteSpace(compliance.CertifiedDate.ToString()) && compliance.CertifiedDate != DateTime.MinValue && compliance.CertifiedDate.Year != 0001)
            {
                CertifiedDate.Text = WebUtil.ConvertToUserDatePattern(compliance.CertifiedDate.ToString());
            }

            if (!string.IsNullOrWhiteSpace(compliance.PCIValidationDate.ToString()) && compliance.PCIValidationDate != DateTime.MinValue && compliance.PCIValidationDate.Year != 0001)
            {
                PCIValidationDate.Text = WebUtil.ConvertToUserDatePattern(compliance.PCIValidationDate.ToString());
            }

            if (!string.IsNullOrWhiteSpace(compliance.LastScannedDate.ToString()) && compliance.LastScannedDate != DateTime.MinValue && compliance.LastScannedDate.Year != 0001)
            {
                LastScannedDate.Text = WebUtil.ConvertToUserDatePattern(compliance.LastScannedDate.ToString());
            }

            pnlPrivateLabel.Visible = false;
            if (compliance.CertifiedDate == DateTime.MinValue || compliance.CertifiedDate.Year == 0001)
                CertifiedDate.Text = "";

            if (compliance.PCIValidationDate == DateTime.MinValue || compliance.PCIValidationDate.Year == 0001)
                PCIValidationDate.Text = "";

            if (compliance.LastScannedDate == DateTime.MinValue || compliance.LastScannedDate.Year == 0001)
                LastScannedDate.Text = "";
            //CheckAccessToBank();

            CRMName.Enabled = !this.EditMode;
            //Added by asheesh for PXP-8412
            if (StatusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_CRM_NA)
            {
                imgCallbackDate.Enabled = false;
                CertifiedDate.Enabled = false;
                
            }
            else
            {
                imgCallbackDate.Enabled = this.EditMode;
                CertifiedDate.Enabled = this.EditMode;
            }

            imgPCIValidationDate.Enabled = this.EditMode;
            PCIValidationDate.Enabled = this.EditMode;
            imgLastScannedDate.Enabled = this.EditMode;
            LastScannedDate.Enabled = this.EditMode;
        }
        public override void FormClear()
        {
            FormHandler.ClearAllControls(pnlCRMDetail);

        }

        public override void FormCancel()
        {
            lblError.Text = string.Empty;
            this.EditMode = false;
            FormShow(this.UID);
            this.Adding = false;
            this.ToggleButtons();
        }

        public void FormClose(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            string url = string.Empty;

            if (Request.QueryString["PostBackURL"] != null)
            {
                url = Convert.ToString(Request.QueryString["PostBackURL"]);
            }
            else
            {
                url = "~/SecureComplianceForms/frmCRMVendorSetupSearch.aspx";
            }
            if (!url.Equals(string.Empty))
            {
                Response.Redirect(url);
            }
            else
            {
                if (this.Parent.NamingContainer != null && this.Parent.NamingContainer.GetType().Equals(typeof(WebDialogWindow)))
                {
                    FormClear();
                    ((WebDialogWindow)this.Parent.NamingContainer).WindowState = DialogWindowState.Hidden;
                }


                if (!string.IsNullOrEmpty(this._WindowCallID))
                {
                    ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), _WindowCallID, "javascript:window.close();", true);
                }
            }
        }
        private Compliance FormSaveCRM()
        {
            int rows = 0;
            if (!this.FormDataCheck())
            {
                return null;
            }

            try
            {
                lblError.Text = "";
                Compliance compliance=new Compliance();
                FormBinding.BindControlsToObject(compliance, pnlCRMDetail);
                DataCompliance data = DataAccess.DataComplianceDao;
                User user = UserSessions.CurrentUser;
                compliance.UpdatedBy = user.UserName;
                DateTime dtCallBackDate;
                DateTime.TryParseExact(CertifiedDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtCallBackDate);
                compliance.CertifiedDate = CommonUtility.Util.if_date(dtCallBackDate.ToString("MM/dd/yyyy"), DateTime.MinValue);
                // Code updated for PXP-8554 fixes by koshlendra start
                if(StatusUID.SelectedValue.ToUpper() != Constants.QUEUESTATUS_CRM_NA)
                { 
                    if(string.IsNullOrEmpty(CertifiedDate.Text))
                    {                    
                        lblError.Text = "Please select Status Date.";
                        CertifiedDate.Text = "";
                        return null;
                    }
                    else 
                    {
                        if(compliance.CertifiedDate == DateTime.MinValue || compliance.CertifiedDate.Year == 0001)
                        {
                         
                          lblError.Text = "Please select a valid Status Date.";
                          CertifiedDate.Text = "";
                           return null;
                        }
                    }
                }

                // Code updated for PXP-8554 fixes by koshlendra end
                if (!this.Adding)
                {
                    // IN EDITING                    
                    rows = data.UpdateCRM(compliance);
                    UserSessions.CurrentCRM = compliance;
                }
                else
                {
                    // IN ADDING
                    compliance.CreatedDate = DateTime.Now;
                    compliance.CreatedBy = user.UserName;                   
                    data.InsertCRM(compliance);
                    UserSessions.CurrentCRM = compliance;
                }
                this.EditMode = false;
                this.Adding = false;
                FormHandler.SetControlEditMode(pnlCRMDetail, this.EditMode);
                imgCallbackDate.Enabled = this.EditMode;
                imgPCIValidationDate.Enabled = this.EditMode;
                imgLastScannedDate.Enabled = this.EditMode;
                this.ToggleButtons();
                return compliance;
            }
            catch (Exception exc)
            {
                throw exc;
            }

           


        }

        public override bool FormDataCheck()
        {
            string message = string.Empty;
            
            if (StatusUID.SelectedIndex == 0)
            {
                message += "Please select a TPP Certified flag" + Environment.NewLine;
            }
            if (String.IsNullOrEmpty(CRMName.Text))
            {
                message += "Please enter TPP Name." + Environment.NewLine;
            }
                //Added Check for Unique TPP Name for PXP-8417
            else if (this.Adding)
            {
                DataCompliance data = DataAccess.DataComplianceDao;
                Hashtable prms = new Hashtable();
                prms.Add("@TPPName", CRMName.Text.Trim());
                prms.Add("@Type", CRMType.SelectedItem.Value.Trim());

                if (data.IsExistsTTPName(prms))
                {
                    message += "TPP Name already exists." + Environment.NewLine;
                }
            }
            else if (this.EditMode)
            {
                Compliance currentCRM = UserSessions.CurrentCRM;
                if (currentCRM.CRMType.ToUpper() != CRMType.SelectedItem.Value.Trim().ToUpper())
                {
                    DataCompliance data = DataAccess.DataComplianceDao;
                    Hashtable prms = new Hashtable();
                    prms.Add("@TPPName", CRMName.Text.Trim());
                    prms.Add("@Type", CRMType.SelectedItem.Value.Trim());

                    if (data.IsExistsTTPName(prms))
                    {
                        message += "TPP Name already exists." + Environment.NewLine;
                    }
                }
            }
                       
            //code modified by koshlendra for handle Null value validation
                //Added by asheesh for PXP-8412
             if (string.IsNullOrWhiteSpace(CertifiedDate.Text) && StatusUID.SelectedValue.ToUpper() != Constants.QUEUESTATUS_CRM_NA)
                {
                    message += "Please Select Status Date." + Environment.NewLine;
                    CertifiedDate.Text = "";
                }

             //DateTime dtCallBackDate;
             //if (!string.IsNullOrEmpty(PCIValidationDate.Text) && !DateTime.TryParseExact(PCIValidationDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtCallBackDate))
             //{
             //    message += "Please select a Valid PCI Validation Date." + Environment.NewLine;
             //    PCIValidationDate.Text = "";
             //}
             //if (!string.IsNullOrEmpty(LastScannedDate.Text) && !DateTime.TryParseExact(LastScannedDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtCallBackDate))
             //{
             //    message += "Please select a Valid Last Scanned Date." + Environment.NewLine;
             //    LastScannedDate.Text = "";
             //}
            
            if (message == string.Empty)
            {
                return true;
            }
            else
            {
                lblError.Text = message.Replace(Environment.NewLine, "<br/>"); ;
                return false;
            }
        }
        public override void ToggleButtons()
        {
            btnEdit.Enabled = !btnEdit.Enabled;
            btnAdd.Enabled = !btnAdd.Enabled;
            myclick.Enabled = !myclick.Enabled;
            btnRefresh.Enabled = !btnRefresh.Enabled;
            btnCancel.Enabled = !btnCancel.Enabled;
        }
        public override bool FormDelete()
        {
            throw new NotImplementedException();
        }

        public override bool FormSave()
        {
            throw new NotImplementedException();
        }
        //Added by asheesh for PXP-8412
        protected void StatusUID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StatusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_CRM_NA)
            {
                imgCallbackDate.Enabled = false;
                CertifiedDate.Enabled = false;
                CertifiedDate.Text = "";               
            }
            else
            {
                imgCallbackDate.Enabled = true;
                CertifiedDate.Enabled = true;                
            }
        }

        //Code added for PXP-8410 by koshlendra start

        /// <summary>
        /// PXP-8410 
        /// </summary>
        private void LoadCRMChangeHistory()
        {
            DataTable dt = DataChangeLogs.SearchCRMChangeHistory(new Hashtable { { "@CRMID", Convert.ToInt32(UserSessions.CurrentCRM.CRMID) } });

            int ChangeHistoryFieldID = CommonUtility.Util.if_i(this.vsChangeHistoryFieldID, 0);

            if (ChangeHistoryFieldID == 0)
            {
                this.grdChangeHistory.DataSource = dt;
            }
            else
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = String.Format("ChangeHistoryFieldID='{0}'", ChangeHistoryFieldID);
                this.grdChangeHistory.DataSource = dv; //li.FindAll(a => a.ChangeHistoryFieldID == ChangeHistoryFieldID);
            }

            this.grdChangeHistory.DataBind();
        }

        protected void ddlChangeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = ((DropDownList)grdChangeHistory.HeaderRow.FindControl("ddlChangeType"));           
            this.vsChangeHistoryFieldID = CommonUtility.Util.if_i(ddl.SelectedValue, -1).ToString();

            this.LoadCRMChangeHistory();
        }

        protected void ddlChangeType_PreRender(object sender, EventArgs e)
        {
            DropDownList ddl = ((DropDownList)grdChangeHistory.HeaderRow.FindControl("ddlChangeType"));

            LookupTableHandler.LoadChangeHistoryFields(ddl, true, ChangeHistoryFields.ChangeHistoryFieldSource.Compliance);
            ddl.SelectedValue = this.vsChangeHistoryFieldID;
        }

        protected void grdChangeHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                ((Label)e.Row.Cells[1].FindControl("lblNameHeader")).Text = "Value";
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string value = DataBinder.Eval(e.Row.DataItem, "NewValue").ToString();

                ((Label)e.Row.Cells[1].FindControl("lblNewValue")).Text = value;
                e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text);

            }

        }
        protected void grdChangeHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdChangeHistory.PageIndex = e.NewPageIndex;
            LoadCRMChangeHistory();
        }
        
        protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.grdChangeHistory.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
            LoadCRMChangeHistory();
            
        }       
        //Code added for PXP-8410 by koshlendra end
    }
