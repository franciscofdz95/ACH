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

namespace AchSystem
{
    public partial class frmUpdatePRIRoutingTable : Form
    {
        private DataSet m_Data = null;

        public frmUpdatePRIRoutingTable()
        {
            InitializeComponent();

        }

        public frmUpdatePRIRoutingTable(string strOption, DataSet ds)
        {
            InitializeComponent();

            m_Data = ds;

            switch (strOption)
            {
                case "Release Account Block":
                    tabMain.SelectedIndex = 0;
                    break;
                case "Add Account Block":
                    tabMain.SelectedIndex = 1;
                    break;
            }
        }

        private bool FormSave()
        {
            DataTable dt;
            ArrayList prms = new ArrayList();
            DataBank data = new DataBank();

            dt = m_Data.Tables[0];

                if (tabMain.SelectedIndex == 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        prms.Clear();

                        prms.Add(new SqlParameter("@AchID", Convert.ToInt32(dr["ACH ID"])));
                        prms.Add(new SqlParameter("@EntryID", dr["Entry ID"].ToString()));
                        prms.Add(new SqlParameter("@TransRoute", dr["Trans Route"].ToString()));
                        prms.Add(new SqlParameter("@AccountNo", dr["Account No"].ToString()));
                        prms.Add(new SqlParameter("@TransType", dr["Trans Type"].ToString()));
                        prms.Add(new SqlParameter("@ReasonCode", cboReasonCode2.Text));
                        prms.Add(new SqlParameter("@UserID", main.Current_User));

                        if (data.UpdatePRIRouting(prms) == 0)
                            FormHandler.DispalyErrorMessage("Failed to update PRI routing table for Trans Route " + dr["Trans Route"].ToString() + " and Account No " + dr["Account No"].ToString());
                    }
                }
                else if (tabMain.SelectedIndex == 1)
                {
                    prms.Add(new SqlParameter("@AchID", Convert.ToInt32(txtAchID.Text )));
                    prms.Add(new SqlParameter("@TransRoute", txtTransRoute.Text ));
                    prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text ));
                    prms.Add(new SqlParameter("@TransType", cboTransType.Text ));
                    prms.Add(new SqlParameter("@ReasonCode", cboReasonCode.Text ));
                    prms.Add(new SqlParameter("@UserID", main.Current_User));

                    if (data.InsertPRIRouting(prms) == 0)
                        FormHandler.DispalyErrorMessage("Failed to insert into PRI routing table for Trans Route " + txtTransRoute.Text + " and Account No " + txtAccountNo.Text);
                }

            return true;
        }

        private void tbrTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "&Save":
                    this.FormSave();
                    this.Close();
                    break;
                case "&Close":
                    this.Close();
                    break;
            }

        }

        private void frmUpdatePRIRoutingTable_Load(object sender, EventArgs e)
        {
            LookUpTableHandler.LoadPRIReasonCodes(cboReasonCode);
            LookUpTableHandler.LoadPRIReasonCodes(cboReasonCode2);
            LookUpTableHandler.LoadTransactionTransType(cboTransType);
        }
 
        
    }
}