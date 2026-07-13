using OfficeOpenXml;
using PaymentXP.BusinessObjects.Zeus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace ZeusWeb.UserControls.CRM
{
    public partial class wucCRMNotificationGrid : wucBaseSearch
    {

        public List<NotificationHistory> NotificationHistorys
        {
            get { return (List<NotificationHistory>)(this.ViewState["NotificationHistorys"] ?? new List<NotificationHistory>()); }
            set { this.ViewState["NotificationHistorys"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //PXP-13240 : by Satyajit on 03/9/2020
            
            if (IsPostBack)
            {
                grdNotificationHistory.DataSource = NotificationHistorys;
                grdNotificationHistory.DataBind();
            }
        }
        public void BindGrid()
        {
            grdNotificationHistory.DataSource = NotificationHistorys;
            grdNotificationHistory.DataBind();
        }
        //PXP-13240 : by Satyajit on 03/19/2020
        protected string checkForPatterns(string strInput)
        {
            string strFilter1 = string.Empty;
            string strFilter2 = string.Empty;
            string strFilter3 = string.Empty;
            string strFilter4 = string.Empty;
            string strFilter5 = string.Empty;

            strFilter1 = strInput.Replace("  ","");
            strFilter2 = strFilter1.Replace("\t", "");
            strFilter3 = strFilter2.Replace(System.Environment.NewLine, " ");
            strFilter4 = strFilter3.Replace("\r", "");
            strFilter5 = strFilter4.Replace("\n", "");

            return strFilter5;
        }
        //PXP-13240 : by Satyajit on 03/9/2020
        protected void grdNotificationHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableCell tc = new TableCell();
                tc.Text = "HTML";
                tc.BackColor = Color.FromArgb(229, 229, 229);
                tc.Font.Bold = true;
                e.Row.Cells.Add(tc);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell tc = new TableCell();

                if (DataBinder.Eval(e.Row.DataItem, "NotificationContent") != null && !string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "NotificationContent").ToString()))
                {
                    if (DataBinder.Eval(e.Row.DataItem, "NotificationType") != null && !string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString())
                        && DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString().ToUpper().Contains("N1"))
                    {
                        string strUnfilteredContent = DataBinder.Eval(e.Row.DataItem, "NotificationContent").ToString();
                        string strContent = string.Empty;
                        strContent = System.Web.HttpUtility.HtmlEncode(checkForPatterns(strUnfilteredContent));
                        LinkButton hypN1 = new LinkButton();
                        hypN1.Text = " N1 ";
                        hypN1.OnClientClick = "return viewHTMLData('" + DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString().Trim() + "', '" + strContent + "');";
                        ScriptManager smN1 = ScriptManager.GetCurrent(this.Page);
                        smN1.RegisterAsyncPostBackControl(hypN1);
                        tc.Controls.Add(hypN1);
                    }
                    if (DataBinder.Eval(e.Row.DataItem, "NotificationType") != null && !string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString())
                         && DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString().ToUpper().Contains("N2"))
                    {
                        string strUnfilteredContent = DataBinder.Eval(e.Row.DataItem, "NotificationContent").ToString();
                        string strContent = string.Empty;
                        strContent = System.Web.HttpUtility.HtmlEncode(checkForPatterns(strUnfilteredContent));

                        LinkButton hypN2 = new LinkButton();
                        hypN2.Text = " N2 ";
                        hypN2.OnClientClick = "return viewHTMLData('" + DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString().Trim() + "', '" + strContent + "');";
                        ScriptManager smN2 = ScriptManager.GetCurrent(this.Page);
                        smN2.RegisterAsyncPostBackControl(hypN2);
                        tc.Controls.Add(hypN2);
                    }
                    if (DataBinder.Eval(e.Row.DataItem, "NotificationType") != null && !string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString())
                        && DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString().ToUpper().Contains("N3"))
                    {
                        string strUnfilteredContent = DataBinder.Eval(e.Row.DataItem, "NotificationContent").ToString();
                        string strContent = System.Web.HttpUtility.HtmlEncode(checkForPatterns(strUnfilteredContent));
                        LinkButton hypN3 = new LinkButton();
                        hypN3.Text = " N3 ";
                        hypN3.OnClientClick = "return viewHTMLData('" + DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString().Trim() + "', '" + strContent + "');";
                        ScriptManager smN3 = ScriptManager.GetCurrent(this.Page);
                        smN3.RegisterAsyncPostBackControl(hypN3);
                        tc.Controls.Add(hypN3);
                    }
                    if (DataBinder.Eval(e.Row.DataItem, "NotificationType") != null && !string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString())
                        && DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString().ToUpper().Contains("N4"))
                    {
                        string strUnfilteredContent = DataBinder.Eval(e.Row.DataItem, "NotificationContent").ToString();
                        string strContent = System.Web.HttpUtility.HtmlEncode(checkForPatterns(strUnfilteredContent));
                        LinkButton hypN4 = new LinkButton();
                        hypN4.Text = " N4 ";
                        hypN4.OnClientClick = "return viewHTMLData('" + DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString().Trim() + "', '" + strContent + "');";
                        ScriptManager smN4 = ScriptManager.GetCurrent(this.Page);
                        smN4.RegisterAsyncPostBackControl(hypN4);
                        tc.Controls.Add(hypN4);
                    }
                    if (DataBinder.Eval(e.Row.DataItem, "NotificationType") != null && !string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString())
                        && DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString().ToUpper().Contains("N5"))
                    {
                        string strUnfilteredContent = DataBinder.Eval(e.Row.DataItem, "NotificationContent").ToString();
                        string strContent = System.Web.HttpUtility.HtmlEncode(checkForPatterns(strUnfilteredContent));
                        LinkButton hypN5 = new LinkButton();
                        hypN5.Text = " N5 ";
                        hypN5.OnClientClick = "return viewHTMLData('" + DataBinder.Eval(e.Row.DataItem, "NotificationType").ToString().Trim() + "', '" + strContent + "');";
                        ScriptManager smN5 = ScriptManager.GetCurrent(this.Page);
                        smN5.RegisterAsyncPostBackControl(hypN5);
                        tc.Controls.Add(hypN5);
                    }                   
                }
                e.Row.Cells.Add(tc);
            }
        }
    }
}