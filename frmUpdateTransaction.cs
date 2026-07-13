using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using Nmc.Ach.Dal;

using Infragistics.Win.UltraWinGrid;

namespace AchSystem
{
    public partial class frmUpdateTransaction : AchSystem.frmBase 
    {
        private UltraGrid m_Grid = null;
        private string m_Option = string.Empty;

        public frmUpdateTransaction()
        {
            InitializeComponent();

        }

        public frmUpdateTransaction(UltraGrid grd)
        {
            InitializeComponent();

            m_Grid = grd;

            switch (m_Option)
            { 
                case "Mass Update Trans Status":
                    tabMain.SelectedIndex = 0;
                    break;
                case "Mass Update Next Process Date":
                    tabMain.SelectedIndex = 1;
                    break;
            }
        }

        private void frmUpdateTransaction_Load(object sender, EventArgs e)
        {
            FormHandler.CreateToolBarButton(tbrTop, "Mass Update");
            tbrTop.Toolbars[0].Tools["New"].SharedProps.Visible = false;
            tbrTop.Toolbars[0].Tools["Undo"].SharedProps.Visible = false;
            tbrTop.Toolbars[0].Tools["Delete"].SharedProps.Visible = false;
            tbrTop.Toolbars[0].Tools["Save"].SharedProps.Visible = false;


            LookUpTableHandler.LoadUserTransStatus(cboStatus);
            tabMain_SelectedIndexChanged(tabMain, new EventArgs());
        }

        private bool FormCheck()
        {
            string strError = string.Empty ;

            if (tabMain.SelectedIndex == 0)
            {
                if (cboStatus.SelectedIndex == -1)
                {
                    strError += "Please select a status.\n";
                }
            }

            if (strError != string.Empty)
            {
                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
            else
            {
                return true;
            }
        }

        
        private bool FormSave()
        {
            ArrayList prms = new ArrayList();
            AchListItem item;
            DataTransaction data = new DataTransaction();

            item = (AchListItem)cboStatus.SelectedItem;
            
            foreach(UltraGridRow dr in m_Grid.Rows)
            {
                if (dr.Selected)
                {
                    switch (Convert.ToInt32(dr.Cells["Status ID"].Value))
                    {
                        case 0:
                        case 2:
                        case 3:
                            prms.Clear();

                            if (tabMain.SelectedIndex == 0)
                            {
                                prms.Add(new SqlParameter("@TransID", Convert.ToInt32(dr.Cells["TransID"].Value)));
                                prms.Add(new SqlParameter("@NewStatus", Convert.ToInt32(item.ItemValue)));
                                prms.Add(new SqlParameter("@OldSataus", Convert.ToInt32(dr.Cells["Status ID"].Value)));
                                prms.Add(new SqlParameter("@UserID", main.Current_User));

                                if (data.UpdateTransactionStatus(prms) == 0)
                                    FormHandler.DispalyErrorMessage("Failed to update status for transaction " + dr.Cells["TransID"].Value.ToString());
                            }
                            else if (tabMain.SelectedIndex == 1)
                            {
                                prms.Add(new SqlParameter("@TransID", Convert.ToInt32(dr.Cells["TransID"].Value)));
                                prms.Add(new SqlParameter("@NewDate", txtNextProcessDate.Text + " " + txtNextProcessTime.Text));
                                prms.Add(new SqlParameter("@OldDate", dr.Cells["Next Process Date"].Value.ToString()));
                                prms.Add(new SqlParameter("@UserID", main.Current_User));

                                if (data.UpdateTransactionProcessDate(prms) == 0)
                                    FormHandler.DispalyErrorMessage("Failed to update process date for transaction " + dr.Cells["TransID"].Value.ToString());
                            }

                            break;
                    }
                }

            }

            return true;
        }

       

        private void tbrTop_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "Mass Update":
                    DialogResult result;
                    result = FormHandler.DispalyQuestionMessage("Are you sure you want to update the " + m_Option + " for the selected records.");

                    if (result == DialogResult.No)
                    {
                        return;
                    }

                    if (tabMain.SelectedIndex == 0)
                    {
                        if (cboStatus.SelectedIndex == -1)
                        {
                            FormHandler.DispalyInformationMessage("Please select a status.");
                            return;
                        }
                    }
                    this.FormSave();
                    this.Close();
                    break;
                case "&Close":
                    this.Close();
                    break;
            }
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabMain.SelectedIndex)
            {
                case 0:
                    m_Option = "[Status]";
                    break;
                case 1:
                    m_Option = "[Next Process Date]";
                    break;
            }
        }

       
        
    }
}