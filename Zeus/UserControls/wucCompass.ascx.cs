using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls
{
    public partial class wucCompass : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        
        public IEnumerable<string> Validate()
        {
            int divNumber;

            if (!int.TryParse(this.txtDivisionNumber.Text, out divNumber))
            {
                yield return "Invalid Division Number: Division Number must be a numeric value.";
            }
        }

        
        public bool Save()
        {
            int rowsAffected = DataRisk.GetInstance().UpdateCompassPlatformParameters(int.Parse(UserSessions.CurrentMerchantApp.ID), this.txtDivisionNumber.Text);
            
            if (rowsAffected > 0)
            {
                UserSessions.CurrentMerchantApp.DivisionNumber = this.txtDivisionNumber.Text;
                return true;
            }

            return false;
        }

        
        public void ShowCompassParameters()
        {
            this.txtDivisionNumber.Text = UserSessions.CurrentMerchantApp.DivisionNumber;
        }
    }
}