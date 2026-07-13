using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZeusWeb.UserControls
{
    public partial class wucDBInfo : System.Web.UI.UserControl
    {
        bool AtLeastOneLive = false;
        bool AtLeastOneStage = false;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (CommonUtility.Util.if_s(ConfigurationManager.AppSettings["ShowDebugBar"]) == "1")
                {

                    pnlDBDebug.Visible = true;


                    litAchDB_Server.Text = CommonUtility.Util.if_s(ConfigurationManager.AppSettings["AchDB_Server"], "NONE");
                    pnlAchDB_Server.CssClass = GetClass(litAchDB_Server.Text);
                    pnlAchDB_Server.Visible = !(litAchDB_Server.Text == "NONE");
                    
                    litFDRDB_Server.Text = CommonUtility.Util.if_s(ConfigurationManager.AppSettings["FDRDB_Server"], "NONE");
                    pnlFDRDB_Server.CssClass = GetClass(litFDRDB_Server.Text);
                    pnlFDRDB_Server.Visible = !(litFDRDB_Server.Text == "NONE");

                    litMerchantDB_Server.Text = CommonUtility.Util.if_s(ConfigurationManager.AppSettings["MerchantDB_Server"], "NONE");
                    pnlMerchantDB_Server.CssClass = GetClass(litMerchantDB_Server.Text);
                    pnlMerchantDB_Server.Visible = !(litMerchantDB_Server.Text == "NONE");

                    litRawDataDB_Server.Text = CommonUtility.Util.if_s(ConfigurationManager.AppSettings["RawDataDB_Server"], "NONE");
                    pnlRawDataDB_Server.CssClass = GetClass(litRawDataDB_Server.Text);
                    pnlRawDataDB_Server.Visible = !(litRawDataDB_Server.Text == "NONE");

                    litTransDB_Server.Text = CommonUtility.Util.if_s(ConfigurationManager.AppSettings["TransDB_Server"], "NONE");
                    pnlTransDB_Server.CssClass = GetClass(litTransDB_Server.Text);
                    pnlTransDB_Server.Visible = !(litTransDB_Server.Text == "NONE");

                    if (AtLeastOneLive)
                    {
                        pnlDBDebug.CssClass += " itemlive";
                    }
                    else if (AtLeastOneStage)
                    {
                        pnlDBDebug.CssClass += " itemstaging";
                    }
                    else
                    {
                        pnlDBDebug.CssClass += " itemdev";
                    }


                }


            }
        }

        private string GetClass(string servername)
        {
            string ret = "";

            if (!string.IsNullOrWhiteSpace(servername))
            {
                switch (servername.ToLower())
                {
                    case "sqlp":
                    case "sqlprpt":
                        ret = "itemlive";
                        AtLeastOneLive = true;
                        break;

                    case "mpsstage":
                        ret = "itemstaging";
                        AtLeastOneStage = true;
                        break;

                    default:
                        ret = "itemdev";
                        break;
                }
            }

            return ret;
        }
    }
}