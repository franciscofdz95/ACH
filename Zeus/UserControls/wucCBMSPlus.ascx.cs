using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtility;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls
{
    public partial class wucCBMSPlus : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IEnumerable<string> Validate()
        {
            if (!string.IsNullOrEmpty(this.txtEthocaMemberId.Text.Trim())
                && !Validation.IsNumeric(this.txtEthocaMemberId.Text, 10))
            {
                yield return "Numeric Ethoca Member ID required.";
            }

            if (!string.IsNullOrEmpty(this.txtVerifiMemberId.Text.Trim())
                && !Validation.IsNumeric(this.txtVerifiMemberId.Text, 20))
            {
                yield return "Numeric Verifi Member ID required.";
            }
        }

        public void Save(int merchantId)
        {
            DataCBDef.UpdatePlatformMemberId(merchantId, CBPlusPlatform.Ethoca, this.txtEthocaMemberId.Text.Trim());
            DataCBDef.UpdatePlatformMemberId(merchantId, CBPlusPlatform.Verifi, this.txtVerifiMemberId.Text.Trim());
        }


        public void ShowCBMSConfiguration(int merchantId)
        {
            Dictionary<CBPlusPlatform, string> memberIds = DataCBDef.GetPlatformMemberIds(merchantId);

            if (memberIds.ContainsKey(CBPlusPlatform.Ethoca))
                this.txtEthocaMemberId.Text = memberIds[CBPlusPlatform.Ethoca];

            if (memberIds.ContainsKey(CBPlusPlatform.Verifi))
                this.txtVerifiMemberId.Text = memberIds[CBPlusPlatform.Verifi];

        }
    }
}