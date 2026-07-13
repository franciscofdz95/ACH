using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;

namespace ZeusWeb.SecureReports
{
    public partial class AccountGroupsReport : frmBaseSearch
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
            blError.Items.Clear();
            if (!IsPostBack)
            {
                FormShow();
            }
        }

        private void FormShow()
        {
            
            if ((!string.IsNullOrEmpty(txtAccountGroup.Text.Trim())) || (!string.IsNullOrEmpty(txtAccountGroupDesc.Text.Trim())))
            {
                DataMerchantApp data = new DataMerchantApp();
                Hashtable prms = new Hashtable();
                prms.Add("@AccountGroup", txtAccountGroup.Text.Trim());
                prms.Add("@Description", txtAccountGroupDesc.Text.Trim());
                DataTable dt = data.GetAccountGroups(prms);
                this.grdAccountGroupsSearch.DataSource = dt;
                this.grdAccountGroupsSearch.DataBind();
            }
            else
            {
                grdAccountGroupsSearch.DataSource = null;
                grdAccountGroupsSearch.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
          if(string.IsNullOrEmpty(txtAccountGroup.Text.Trim()) && string.IsNullOrEmpty(txtAccountGroupDesc.Text.Trim()))
          {
              WucMessage.AddMessageError("Please Enter Account Group Name or Description.");
          }
          else
          {
            FormShow();
          }
        }

        protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            txtAccountGroup.Text = null;
            txtAccountGroupDesc.Text = null;

            FormShow();
        }

        protected void grdAccountGroupsSearch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //HiddenField hidID = null;
            GridViewRow grdRow = null;
            string accountgroup = null;
            string description = null;
            string id = null;
            bool hide = false;
            if (e.CommandSource is LinkButton)
            {
           
                
                if (grdAccountGroupsSearch != null)
                {
                    grdRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    accountgroup = grdAccountGroupsSearch.DataKeys[grdRow.RowIndex].Values["AccountGroup"].ToString();
                    description = grdAccountGroupsSearch.DataKeys[grdRow.RowIndex].Values["Description"].ToString();
                    id = grdAccountGroupsSearch.DataKeys[grdRow.RowIndex].Values["AccountGroupID"].ToString();
                    hide = Convert.ToBoolean(grdAccountGroupsSearch.DataKeys[grdRow.RowIndex].Values["IsDisabled"].ToString());
                }
            }
            switch (e.CommandName)
            {
                case "View":
                    dlgEditAccountGroups.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                    txtEditAccountGroup.Text = accountgroup;
                    txtEditAccountGroupDesc.Text = description;
                    txtAccountGroupID.Text = id;
                    chkEditAccountGroupHide.Checked = hide;
                    break;

                
            }
        }

        public List<GenericListItem> DeepCopy(List<GenericListItem> Temp)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, Temp);

            ms.Position = 0;
            return (List<GenericListItem>)bf.Deserialize(ms);
        }

        protected void btnEditSave_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(txtEditAccountGroup.Text.Trim()))
            {
                blError.Items.Add("Account Group Name is required.");
            }

            if (string.IsNullOrEmpty(txtEditAccountGroupDesc.Text.Trim()))
            {
                blError.Items.Add("Account Group Description is required.");
                
            }
            if(!string.IsNullOrEmpty(txtEditAccountGroup.Text.Trim()) && !string.IsNullOrEmpty(txtEditAccountGroupDesc.Text.Trim()))
            {
                DataMerchantApp data = new DataMerchantApp();
                string Change = null;
                if (string.IsNullOrEmpty(txtAccountGroupID.Text))
                {
                    Change = "AccountGroup: ," + txtEditAccountGroup.Text.Trim() + "| Description: ," + txtEditAccountGroupDesc.Text.Trim();
                }
                else
                {
                    Hashtable prms = new Hashtable();
                    DataTable dt = data.GetAccountGroups(prms);
                    DataRow dr = dt.AsEnumerable().Where(r => (r["AccountGroupID"].ToString()) == txtAccountGroupID.Text.Trim()).Single();
                    Change = "AccountGroup: " + dr["AccountGroup"].ToString() + "," + txtEditAccountGroup.Text.Trim() + "| Description: " + dr["Description"].ToString() + "," + txtEditAccountGroupDesc.Text.Trim();
                }
                DataUser datauser = DataAccess.DataUserDao;
                datauser.InsertChangeLog("Account Group", UserSessions.CurrentUser.UserName, null, txtAccountGroupID.Text.Trim(), "Account Group", Change, Constants.PORTAL_ZEUS);

                data.UpdateAccountGroup(txtAccountGroupID.Text.Trim(), txtEditAccountGroup.Text.Trim(), txtEditAccountGroupDesc.Text.Trim(), chkEditAccountGroupHide.Checked, UserSessions.CurrentUser.UserName);

                dlgEditAccountGroups.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
                txtEditAccountGroup.Text = null;
                txtEditAccountGroupDesc.Text = null;
                txtAccountGroupID.Text = null;
                chkEditAccountGroupHide.Checked = false;
                LookupTableHandler.m_AccountGroups = null;
                FormShow();
            }
        }

        protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            txtEditAccountGroup.Text = null;
            txtEditAccountGroupDesc.Text = null;
            txtAccountGroupID.Text = null;
            chkEditAccountGroupHide.Checked = false;
            dlgEditAccountGroups.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
            LookupTableHandler.m_AccountGroups = null;
        }

        protected void btnCancel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
        {
            dlgEditAccountGroups.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
            txtEditAccountGroup.Text = null;
            txtEditAccountGroupDesc.Text = null;
            txtAccountGroupID.Text = null;
            FormShow();

        }
    }
}