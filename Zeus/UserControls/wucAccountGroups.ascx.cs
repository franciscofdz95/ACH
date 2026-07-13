using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;

namespace ZeusWeb.UserControls
{
    public partial class wucAccountGroups : System.Web.UI.UserControl
    {
        List<GenericListItem> lstAccountGroups = new List<GenericListItem>();
        List<GenericListItem> lstMerchantAccountGroups = new List<GenericListItem>();
        public List<GenericListItem> MerchantAccountGroups
        {
            get { return (List<GenericListItem>)ViewState["MerchantAccountGroups"]; }
            set { ViewState["MerchantAccountGroups"] = value; }

        }

        public Unit ControlWidth
        {
            set { ViewState["IDWidth"] = value; }
            get
            {
                if (ViewState["IDWidth"] == null)
                    return Unit.Pixel(180);
                else
                    return (Unit)ViewState["IDWidth"];
            }
        }

        public ControlStyle ControlType
        {
            get
            {
                if (ViewState["ControlType"] == null)
                    return (ControlStyle.BulletList);
                else
                    return (ControlStyle)ViewState["ControlType"];
            }
            set { ViewState["ControlType"] = value; }
        }

        public string AccountGroupIds
        {
            set { ViewState["AccountGroupIds"] = value; }
            get
            {
                if (ViewState["AccountGroupIds"] == null)
                    return (null);
                else
                return ViewState["AccountGroupIds"].ToString();
            }
        }


        public string ZID
        {
            set { ViewState["ZID"] = value; }
            get
            {
                if (ViewState["ZID"] == null)
                    return (null);
                else
                    return ViewState["ZID"].ToString();
            }
        }

        

        public enum ControlStyle
        {
            Textbox = 1,
            BulletList = 2
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(wucAccountGroups_PreRender);
        }

        void wucAccountGroups_PreRender(object sender, EventArgs e)
        {
            switch (this.ControlType)
            {
                case ControlStyle.BulletList:
                    this.AccountGroups1.Visible = true;
                    AccountGroups1.Width = ControlWidth;
                    break;

                default:
                    this.txtAccountGroups.Visible = true;
                    txtAccountGroups.Width = ControlWidth;
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lstAccountGroups = DeepCopy(LookupTableHandler.LoadAccountGroups());

            if (!IsPostBack)
            {
                if (lstMerchantAccountGroups.Count == 0)
                {
                    if (ZID != null)
                    {
                        MerchantFacade facade = new MerchantFacade();
                        DataTable dtMerchantAccountGroups = facade.GetMerchantAccountGroups(Convert.ToInt32(ZID));
                        GenericListItem item = null;

                        foreach (DataRow dr in dtMerchantAccountGroups.Rows)
                        {
                            item = new GenericListItem();
                            item.ItemText = dr["AccountGroup"].ToString();
                            item.ItemValue = dr["AccountGroupID"].ToString();
                            lstMerchantAccountGroups.Add(item);
                        }
                        AssignAccountGroups(lstMerchantAccountGroups);
                        MerchantAccountGroups = lstMerchantAccountGroups;

                        FormShow(this.txtAccountGroupName.Text.Trim());
                    }
                }
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FormShow(this.txtAccountGroupName.Text.Trim());
        }

        //This Assignment happens only when the web user control is not used for search and an control is loaded with the ZID
        protected void AssignAccountGroups(List<GenericListItem> lstMerchantAccountGroups)
        {
            string Names = null;
            AccountGroupIds = null;
            for (int i = 0; i < lstMerchantAccountGroups.Count; i++)
            {
                Names += lstMerchantAccountGroups[i].ItemText + ",";
                AccountGroupIds += lstMerchantAccountGroups[i].ItemValue + ",";  
                this.AccountGroups1.Items.Add(new ListItem(lstMerchantAccountGroups[i].ItemText,lstMerchantAccountGroups[i].ItemValue));
            }
        }

        protected void AddAccountGroups_Click(object sender, EventArgs e)
        {
            if (MerchantAccountGroups != null)
            {
                lstMerchantAccountGroups = MerchantAccountGroups;
            }
            
            foreach (GridViewRow rw in grdAccountGroupsSearch.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkEnabled");
                if (chkBx != null && chkBx.Checked)
                {
                    TextBox theTextBox = rw.FindControl("AccountGroupID") as TextBox;
                    lstMerchantAccountGroups.Add(lstAccountGroups.FirstOrDefault(o => o.ItemValue == theTextBox.Text));
                }
            }

            MerchantAccountGroups = lstMerchantAccountGroups;
            FormShow(this.txtAccountGroupName.Text.Trim());
        }

        public List<GenericListItem> DeepCopy(List<GenericListItem> Temp)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, Temp);

            ms.Position = 0;
            return (List<GenericListItem>)bf.Deserialize(ms);
        }

        private void FormShow(string AccountGroup = null)
        {
            lstMerchantAccountGroups = MerchantAccountGroups;

            if ((!string.IsNullOrEmpty(AccountGroup)))
            {
                lstAccountGroups = lstAccountGroups.Where(stringToCheck => stringToCheck.ItemText.ToUpper().Contains(AccountGroup.ToUpper())).ToList<GenericListItem>();

                List<GenericListItem> lstRemoveAccountGroups = new List<GenericListItem>();
                if (lstMerchantAccountGroups != null)
                {
                    foreach (GenericListItem Selectedlistitem in lstMerchantAccountGroups)
                    {
                        foreach (GenericListItem Searchlistitem in lstAccountGroups)
                        {
                            if (Selectedlistitem.ItemValue == Searchlistitem.ItemValue)
                            {
                                lstRemoveAccountGroups.Add(Searchlistitem);
                            }
                        }
                    }


                    foreach (GenericListItem RemoveItem in lstRemoveAccountGroups)
                    {
                        lstAccountGroups.Remove(RemoveItem);
                    }
                }

                this.grdAccountGroupsSearch.DataSource = lstAccountGroups;
                grdAccountGroupsSearch.DataBind();

                
            }

            grdAccountGroupsSearch.DataBind();
            this.grdAccountGroupsSelected.DataSource = lstMerchantAccountGroups;
            grdAccountGroupsSelected.DataBind();
        }

        protected void DeleteAccountGroups_Click(object sender, EventArgs e)
        {
            lstMerchantAccountGroups = MerchantAccountGroups;
            //txtAccountGroupName.Text = null;
            foreach (GridViewRow rw in grdAccountGroupsSelected.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkEnabled");
                if (chkBx != null && chkBx.Checked == true)
                {
                    TextBox theTextBox = rw.FindControl("AccountGroupID") as TextBox;
                    lstMerchantAccountGroups.Remove(lstMerchantAccountGroups.FirstOrDefault(o => o.ItemValue == theTextBox.Text));

                }
            }
            MerchantAccountGroups = lstMerchantAccountGroups;
            FormShow(this.txtAccountGroupName.Text.Trim());
        }

        protected void btnAddAccGroups_Click(object sender, EventArgs e)
        {
            this.dlgAccountGroups.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
            FormShow();
        }

        protected void Ok_Click(object sender, EventArgs e)
        {
            AccountGroups1.Items.Clear();
            string Value = null;
            AccountGroupIds = null;
            foreach (GridViewRow rw in grdAccountGroupsSelected.Rows)
            {
                    TextBox theTextBox = rw.FindControl("AccountGroupID") as TextBox;
                    this.AccountGroups1.Items.Add(new ListItem(rw.Cells[1].Text.ToString(), theTextBox.Text));
                    AccountGroupIds += theTextBox.Text + ",";  
                    Value += rw.Cells[1].Text.ToString() + ",";
            }
            this.txtAccountGroups.Text = Value; 
            this.dlgAccountGroups.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
            txtAccountGroupName.Text = string.Empty;
        }


      
    }
}