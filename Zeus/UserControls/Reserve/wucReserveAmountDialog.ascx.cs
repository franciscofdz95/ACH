using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.BusinessObjects;
using PaymentXP.BusinessObjects.Reserve;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucReserveAmountDialog : wucBaseDataEntry
    {
        public delegate void EventClickSaveSuccess();
        public event EventClickSaveSuccess event_click_savesuccess;

        public WebDialogWindow WinInstance
        {
            get { return dlgReserveAmount; }
        }

        public int ZID
        {
            get { return (int)(ViewState["ZID"] ?? 0); }
            set { ViewState["ZID"] = value; }
        }

        public int ReserveID
        {
            get { return (int)(ViewState["ReserveID"] ?? 0); }
            set { ViewState["ReserveID"] = value; }
        }

        private RDBReserve VSReserve
        {
            get { return (RDBReserve)(ViewState["VSReserve"] ?? null); }
            set { ViewState["VSReserve"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void FormShow(string ID)
        {
            MerchantApp ma = DataMerchantApp.GetInstance().GetMerchantApp(this.ZID);

            this.VSReserve = DataReserve.GetRDBReserve(this.ReserveID, this.ZID);

            if (this.VSReserve != null)
            {
                wceReserveAmount.Value = this.VSReserve.Reserve;
                lblDivertAmount.Text = string.Format("{0:0.00}", this.VSReserve.Divert);
                lblAmountWithheld.Text = string.Format("{0:0.00}", this.VSReserve.Amount);

                lblDBA.Text = ma.BusinessDBAName;
                lblZID.Text = ma.ID;

            
            }

        }

        protected void wceReserveAmount_TextChanged(object sender, EventArgs e)
        {
            decimal nDivert = this.VSReserve.Amount - Convert.ToDecimal( this.wceReserveAmount.Value);
            lblDivertAmount.Text = string.Format("{0:0.00}", nDivert);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.ZID > 0 && this.VSReserve != null && this.VSReserve.ReserveID == this.ReserveID && this.VSReserve.ZID == this.ZID)
            {
                if (this.FormDataCheck())
                {
                    this.VSReserve.Reserve = Convert.ToDecimal(wceReserveAmount.Value);
                    this.VSReserve.Divert = this.VSReserve.Amount - Convert.ToDecimal(wceReserveAmount.Value);

                    DataReserve.UpdateRDBReserve(this.VSReserve);
                    this.dlgReserveAmount.WindowState = DialogWindowState.Hidden;

                    if (event_click_savesuccess != null)
                    {
                        event_click_savesuccess();
                    }
                }
            }

        }

        public override bool FormDataCheck()
        {
            int count = 0;

            foreach (string str in this.ValidateForm())
            {
                count++;
            }

            // no errors, so all good.
            return (count == 0);
        }

        protected IEnumerable<string> ValidateForm()
        {
            if (Convert.ToDecimal(wceReserveAmount.Value) > this.VSReserve.Amount)
            {
                cvReserveAmount.IsValid = false;
                cvReserveAmount.ErrorMessage = "Reserve amount cannot be greater than Amount Withheld";
                yield return cvReserveAmount.ErrorMessage;
            }
            else if (Convert.ToDecimal(wceReserveAmount.Value) < 0)
            {
                cvReserveAmount.IsValid = false;
                cvReserveAmount.ErrorMessage = "Reserve amount must non-negative";
                yield return cvReserveAmount.ErrorMessage;
            }
        }


        
        

        public override void FormClear()
        {
            throw new NotImplementedException();
        }

        public override bool FormSave()
        {
            throw new NotImplementedException();
        }

        public override void FormNew()
        {
            throw new NotImplementedException();
        }

        public override bool FormDelete()
        {
            throw new NotImplementedException();
        }

        public override void FormCancel()
        {
            throw new NotImplementedException();
        }

        public override void ToggleButtons()
        {
            throw new NotImplementedException();
        }
    }
}