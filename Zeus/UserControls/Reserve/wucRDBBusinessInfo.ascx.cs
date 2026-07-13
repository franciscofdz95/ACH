using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucRDBBusinessInfo : System.Web.UI.UserControl
    {
        public List<int> LiAvailBanks
        {
            get { return (List<int>)(ViewState["LiAvailBanks"] ?? null); }
            set { ViewState["LiAvailBanks"] = value; }
        }

        public List<int> LiAvailReserveTypes
        {
            get { return (List<int>)(ViewState["LiAvailReserveTypes"] ?? null); }
            set { ViewState["LiAvailReserveTypes"] = value; }
        }

        public int ZID
        {
            get { return (int)(ViewState["_ZID"] ?? 0); }
            set { ViewState["_ZID"] = value; }
        }

        public Panel pnlInfo
        {
            get { return pnlGeneralInfo; }
        }

        override protected void OnInit(EventArgs e)
        {
            
            if (!this.IsPostBack)
            {
             

            }


        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);


            DropDownList ddlTemp = new DropDownList();

            ////////////////////////////////
            LookupTableHandler.LoadFrontEndPlatforms(ddlTemp, false);

            if (ddlTemp != null && ddlTemp.Items.Count > 0)
            {
                ListItem liTemp = ddlTemp.Items.FindByValue(AuthPlatformUID.Text);
                if (liTemp != null)
                {
                    AuthPlatformUID.Text = liTemp.Text;
                }

            }

            ////////////////////////////////

            LookupTableHandler.LoadBackEndPlatforms(ddlTemp, false);

            if (ddlTemp != null && ddlTemp.Items.Count > 0)
            {
                ListItem liTemp = ddlTemp.Items.FindByValue(SettlePlatformUID.Text);
                if (liTemp != null)
                {
                    SettlePlatformUID.Text = liTemp.Text;
                }

            }


            ////////////////////////////////

            LookupTableHandler.MerchantAppStatus(ddlTemp, false, "Merchant Management");

            if (ddlTemp != null && ddlTemp.Items.Count > 0)
            {
                ListItem liTemp = ddlTemp.Items.FindByValue(StatusUID.Text);
                if (liTemp != null)
                {
                    StatusUID.Text = liTemp.Text;
                }
            }

            ////////////////////////////////

            LookupTableHandler.LoadMerchantAppTypes(ddlTemp, false);

            if (ddlTemp != null && ddlTemp.Items.Count > 0)
            {
                ListItem liTemp = ddlTemp.Items.FindByValue(MerchantAppTypeUID.Text);
                if (liTemp != null)
                {
                    MerchantAppTypeUID.Text = liTemp.Text;
                }
            }

            ////////////////////////////////


            LookupTableHandler.LoadReleaseMethods(ddlTemp, false);

            if (ddlTemp != null && ddlTemp.Items.Count > 0)
            {
                ListItem liTemp = ddlTemp.Items.FindByValue(this.ReleaseMethodUID.Text);
                if (liTemp != null)
                {
                    this.ReleaseMethodUID.Text = liTemp.Text;
                }
            }


            if (this.ZID > 0)
            {
                gvBankBalance.DataSource = DataReserve.GetBalanceByBank(this.ZID);
                gvBankBalance.DataBind();
            }


            //wucSummaryGrid1.ZID = this.ZID;
            //wucSummaryGrid1.SetDataSource(new Hashtable()); // zid will be passed if no params sent.
        }


        protected void gvBankBalance_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.LiAvailBanks = new List<int>();
                this.LiAvailReserveTypes = new List<int>();
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int bank_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "BankID"));

                if (!this.LiAvailBanks.Contains(bank_id))
                {
                    this.LiAvailBanks.Add(bank_id);
                }

                int reserve_type_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ReserveTypeID"));

                if (!this.LiAvailReserveTypes.Contains(reserve_type_id))
                {
                    this.LiAvailReserveTypes.Add(reserve_type_id);
                }


            }


        }

    }



}