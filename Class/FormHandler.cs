using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Infragistics.Win.UltraWinToolbars;
using System.IO;
using System.Threading;


using Nmc.Ach.Dal;

namespace AchSystem
{
    class FormHandler
    {


        public static void CreateToolBarButton(UltraToolbarsManager tbr, string Key)
        {
            tbr.Toolbars[0].Tools.AddTool(Key);
            tbr.Toolbars[0].Tools[Key].InstanceProps.IsFirstInGroup = true;
        }

        public static void PopulateMerchantInfo(UltraGridRow row, UltraExpandableGroupBoxPanel pnl)
        {
            if (row != null)
            {
                if (pnl.Controls.Find("txtMerchantID", true).Length > 0)
                    pnl.Controls["txtMerchantID"].Text = row.Cells["Merchant ID"].Value.ToString();

                if (pnl.Controls.Find("txtAchID", true).Length > 0)
                    pnl.Controls["txtAchID"].Text = row.Cells["AchID"].Value.ToString();

                if (pnl.Controls.Find("txtMerchantName", true).Length > 0)
                    pnl.Controls["txtMerchantName"].Text = row.Cells["Merchant Name"].Value.ToString();

                if (pnl.Controls.Find("txtAchTransRoute", true).Length > 0)
                    pnl.Controls["txtAchTransRoute"].Text = row.Cells["Trans Route"].Value.ToString();

                if (pnl.Controls.Find("txtAchAccountNo", true).Length > 0)
                    pnl.Controls["txtAchAccountNo"].Text = row.Cells["Account No"].Value.ToString();

                if (pnl.Controls.Find("txtAchAccountName", true).Length > 0)
                    pnl.Controls["txtAchAccountName"].Text = row.Cells["Account Name"].Value.ToString();

                if (pnl.Controls.Find("txtAchCompanyName", true).Length > 0)
                    pnl.Controls["txtAchCompanyName"].Text = row.Cells["Merchant Name"].Value.ToString();

                if (pnl.Controls.Find("txtAchDescription", true).Length > 0)
                    pnl.Controls["txtAchDescription"].Text = row.Cells["Ach Description"].Value.ToString();

                if (pnl.Controls.Find("cboAchSecc", true).Length > 0)
                {
                    ComboBox cbo = (ComboBox) pnl.Controls["cboAchSecc"];
                    ListHandler.ListFindItem(cbo, row.Cells["Secc"].Value.ToString().ToUpper());
                }

            }
        }

        public static void PopulateMerchantInfo(UltraGridRow row, Panel pnl)
        {
            if (row != null)
            {
                if (pnl.Controls.Find("txtMerchantID", true).Length > 0)
                    pnl.Controls["txtMerchantID"].Text = row.Cells["Merchant ID"].Value.ToString();

                if (pnl.Controls.Find("txtAchID", true).Length > 0)
                    pnl.Controls["txtAchID"].Text = row.Cells["AchID"].Value.ToString();

                if (pnl.Controls.Find("txtMerchantName", true).Length > 0)
                    pnl.Controls["txtMerchantName"].Text = row.Cells["Merchant Name"].Value.ToString();

                if (pnl.Controls.Find("txtAchTransRoute", true).Length > 0)
                    pnl.Controls["txtAchTransRoute"].Text = row.Cells["Trans Route"].Value.ToString();

                if (pnl.Controls.Find("txtAchAccountNo", true).Length > 0)
                    pnl.Controls["txtAchAccountNo"].Text = row.Cells["Account No"].Value.ToString();

                if (pnl.Controls.Find("txtAchAccountName", true).Length > 0)
                    pnl.Controls["txtAchAccountName"].Text = row.Cells["Account Name"].Value.ToString();

                if (pnl.Controls.Find("txtAchCompanyName", true).Length > 0)
                    pnl.Controls["txtAchCompanyName"].Text = row.Cells["Merchant Name"].Value.ToString();

                if (pnl.Controls.Find("txtAchDescription", true).Length > 0)
                    pnl.Controls["txtAchDescription"].Text = row.Cells["Ach Description"].Value.ToString();

                if (pnl.Controls.Find("cboAchSecc", true).Length > 0)
                {
                    ComboBox cbo = (ComboBox)pnl.Controls["cboAchSecc"];
                    ListHandler.ListFindItem(cbo, row.Cells["Secc"].Value.ToString().ToUpper());
                }

            }
        }

        public static void PopulateAchID(UltraGridRow row, Control ctrl)
        {
            if (row != null)
            {

                if (ctrl.Controls.Find("txtAchID", true).Length > 0)
                    ctrl.Controls["txtAchID"].Text = row.Cells["AchID"].Value.ToString();              
            }
        }

        //public static void ExportGridToExcel(UltraGrid grd)
        //{

        //    if (grd.Rows.Count == 0)
        //        return;

        //    UltraGridExcelExporter exp = new UltraGridExcelExporter();

        //    try
        //    {

        //        string filename = Application.StartupPath + @"\temp.xls";
        //        exp.Export(grd, filename);

        //        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        //        proc.StartInfo.FileName = filename;
        //        proc.Start();
        //        //if (dr == DialogResult.OK && frm.FileName != string.Empty)
        //        //{
        //        //    FormHandler.DispalyInformationMessage(frm.FileName + " exported successfully.");
        //        //}
        //    }
        //    catch (Exception exc)
        //    {
        //        FormHandler.DispalyErrorMessage("Export failed.", exc);
        //    }
        //    finally
        //    {
        //        exp = null;
        //    }

        //}

        public static void ExportGridToExcel(UltraGrid grd)
        {

            if (grd.Rows.Count == 0)
                return;

            UltraGridExcelExporter exp = new UltraGridExcelExporter();

            try
            {


                
                string file = Application.StartupPath + @"\grd_";
                string seqno = string.Empty;

                seqno = GetNextFile(file, ".xls");

                file = file + seqno + ".xls";
                exp.Export(grd, file);

                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.EnableRaisingEvents = false;

                proc.StartInfo.FileName = file;

                proc.Start();

            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Export failed.", exc);
            }
            finally
            {
                exp = null;
            }

        }

        public static string GetNextFile(string strPath, string strFileType)
        {
            string strFile;
            string strFileSeqNumber;
            for (int loopCnt = 1; ; loopCnt++)
            {
                strFileSeqNumber = loopCnt.ToString().PadLeft(2, Char.Parse("0"));
                strFile = strPath + strFileSeqNumber + strFileType;
                if (!File.Exists(strFile))
                {
                    return strFileSeqNumber;
                }
            }
        }
        //public static void ExportGridToExcel(UltraGrid grd)
        //{

        //    if (grd.Rows.Count == 0)
        //        return;

        //    SaveFileDialog frm = new SaveFileDialog();
        //    UltraGridExcelExporter exp = new UltraGridExcelExporter();

        //    try
        //    {



        //        frm.Filter = "Microsoft Office Excel Files (*.xls)|*.xls";

        //        DialogResult dr = frm.ShowDialog();

        //        if (dr == DialogResult.OK && frm.FileName != string.Empty)
        //        {
        //            exp.Export(grd, frm.FileName);
        //            FormHandler.DispalyInformationMessage(frm.FileName + " exported successfully.");
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        FormHandler.DispalyErrorMessage("Export failed.", exc);
        //    }
        //    finally
        //    {
        //        frm = null;
        //        exp = null;
        //    }

        //}

        public static DialogResult DispalyErrorMessage(string message, Exception exc)
        {
            string msg = string.Empty;
            
            msg += message + "\n";
            msg += "Error Message: " + exc.Message + "\n";
            msg += "Error Trace: " + exc.StackTrace;

            return MessageBox.Show(msg,
                Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error );
        }

        public static DialogResult DispalyErrorMessage(string message)
        {
            return MessageBox.Show(message,
                Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

        }

        public static DialogResult DispalyInformationMessage(string message)
        {
            return MessageBox.Show(message,
                Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

        }

        public static DialogResult DispalyQuestionMessage(string message)
        {
            return MessageBox.Show(message,
                Application.ProductName,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

        }

        public static DialogResult DispalyCloseWindowMessage(string message)
        {
            return MessageBox.Show(message,
                Application.ProductName,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

        }

        public static DialogResult DispalyWarningMessage(string message)
        {
            return MessageBox.Show(message,
                Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

        }

        public static void OpenDataForm(iFormData frm, object lngID)
        {
            try
            {
                frm.ID = Convert.ToInt32(lngID);
                if (frm.FormFind())
                {
                    frm.FormShow();
                    frm.GetForm().ShowDialog();
                }
                else
                {
                    FormHandler.DispalyInformationMessage("Unable to find record.");
                }
                frm = null;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        private static void AddChangeEvent(frmBase frm,Control grp)
        {
            foreach (Control ctrl in grp.Controls)
            {
                if (ctrl.Controls.Count > 0)
                    AddChangeEvent(frm,ctrl);
                else
                {
                    switch (ctrl.GetType().ToString())
                    {
                        case "System.Windows.Forms.CheckBox":
                            CheckBox chk = (CheckBox)ctrl;
                            chk.CheckedChanged += new System.EventHandler(frm.Text_TextChanged);
                            break;
                        case "System.Windows.Forms.MaskedTextBox":
                            MaskedTextBox txtMask = (MaskedTextBox)ctrl;
                            txtMask.TextChanged += new System.EventHandler(frm.Text_TextChanged);
                            break;
                        case "System.Windows.Forms.TextBox":
                            TextBox txt = (TextBox)ctrl;
                            txt.TextChanged += new System.EventHandler(frm.Text_TextChanged);
                            break;
                        case "System.Windows.Forms.ComboBox":
                            ComboBox cbo = (ComboBox)ctrl;
                            cbo.SelectedIndexChanged += new System.EventHandler(frm.Text_TextChanged);
                            break;
                        case "System.Windows.Forms.PictureBox":
                            PictureBox pic = (PictureBox)ctrl;
                            pic.BackgroundImageChanged += new System.EventHandler(frm.Text_TextChanged);  
                            break;
                        case "System.Windows.Forms.DateTimePicker":
                            DateTimePicker txtDate = (DateTimePicker)ctrl;
                            txtDate.TextChanged += new System.EventHandler(frm.Text_TextChanged);
                            break;

                    }
                }
            }        
        }

        public static void AddControlChangedEvent(frmBase frm)
        {
            foreach (Control ctrl in frm.Controls)
            {
                AddChangeEvent(frm,ctrl);        
            }
        }

        //public static void AddControlChangedEvent(frmBase frm, GroupBox grp)
        //{
        //    foreach (Control ctrl in grp.Controls)
        //    {
        //        switch (ctrl.GetType().ToString())
        //        {
        //            case "System.Windows.Forms.CheckBox":
        //                CheckBox chk = (CheckBox)ctrl;
        //                chk.CheckedChanged += new System.EventHandler(frm.Text_TextChanged);
        //                break;
        //            case "System.Windows.Forms.MaskedTextBox":
        //                MaskedTextBox txtMask = (MaskedTextBox)ctrl;
        //                txtMask.TextChanged += new System.EventHandler(frm.Text_TextChanged);
        //                break;
        //            case "System.Windows.Forms.TextBox":
        //                TextBox txt = (TextBox)ctrl;
        //                txt.TextChanged += new System.EventHandler(frm.Text_TextChanged);
        //                break;
        //            case "System.Windows.Forms.ComboBox":
        //                ComboBox cbo = (ComboBox)ctrl;
        //                cbo.SelectedIndexChanged += new System.EventHandler(frm.Text_TextChanged);
        //                break;
        //        }
        //    }
        //}

        //public static void AddControlChangedEvent(frmBase frm, GroupBox grp)
        //{
        //    foreach (Control ctrl in grp.Controls)
        //    {
        //        switch (ctrl.GetType().ToString())
        //        {
        //            case "System.Windows.Forms.CheckBox":
        //                CheckBox chk = (CheckBox)ctrl;
        //                chk.CheckedChanged += new System.EventHandler(frm.Text_TextChanged);
        //                break;
        //            case "System.Windows.Forms.MaskedTextBox":
        //                MaskedTextBox txtMask = (MaskedTextBox)ctrl;
        //                txtMask.TextChanged += new System.EventHandler(frm.Text_TextChanged);
        //                break;
        //            case "System.Windows.Forms.TextBox":
        //                TextBox txt = (TextBox)ctrl;
        //                txt.TextChanged += new System.EventHandler(frm.Text_TextChanged);
        //                break;
        //            case "System.Windows.Forms.ComboBox":
        //                ComboBox cbo = (ComboBox)ctrl;
        //                cbo.SelectedIndexChanged += new System.EventHandler(frm.Text_TextChanged);
        //                break;
        //        }
        //    }
        //}

        public static void DisableContols(Control ctrls)
        {
            foreach (Control ctrl in ctrls.Controls)
            {
                if (ctrl.Controls.Count > 0)
                    DisableContols(ctrl);
                else
                {
                    switch (ctrl.GetType().ToString())
                    {
                        case "System.Windows.Forms.ToolStrip":
                        case "System.Windows.Forms.ToolStripButton":
                        case "System.Windows.Forms.LinkLabel":
                        case "System.Windows.Forms.Label":
                            break;
                        default:
                            ctrl.Enabled = false;
                            break;
                    }
                }
            }
        }

        public static void SetSecurity(frmBase frm)
        {
            if (main.g_User.IsAdmin)
                return;

            frm.tbrTop.Toolbars[0].Tools["New"].SharedProps.Enabled  = false;
            frm.tbrTop.Toolbars[0].Tools["Save"].SharedProps.Enabled  = false;
            frm.tbrTop.Toolbars[0].Tools["Undo"].SharedProps.Enabled  = false;
            frm.tbrTop.Toolbars[0].Tools["Delete"].SharedProps.Enabled  = false;

            foreach (Control ctrl in frm.Controls)
            {
                DisableContols(ctrl);
            }
        }

        public static void GetChangedValue(Control ctrls, ref string strValue)
        {
            foreach (Control ctrl in ctrls.Controls)
            {
                if (ctrl.Controls.Count > 0)
                    GetChangedValue(ctrl, ref strValue);
                else
                {
                    if (ctrl.Name.Trim() != string.Empty)
                        if (ctrl.Tag != null)
                            if (ctrl.Name.Substring(0, 3) == "txt" || ctrl.Name.Substring(0, 3) == "cbo" || ctrl.Name.Substring(0, 3) == "chk")
                                if (ctrl.Tag.ToString() != ctrl.Text)
                                    strValue += ctrl.Name.Replace("txt", "").Replace("cbo", "").Replace("chk","") + ":Orig=" + ctrl.Tag.ToString() + ", Mod=" + ctrl.Text + "\n";
                }
            }
        }

        public static void PopulateNewRecord(frmSearch frm1, iFormData frm2)
        {
            foreach (Control ctrl in frm1.m_Ctrl.Controls["grpRequired"].Controls)
            {
                foreach (Control ctrl2 in frm2.GetForm().Controls)
                {
                    if (ctrl2.Controls.Count > 0)
                    {
                        foreach (Control ctrl3 in ctrl2.Controls)
                        {
                            if (ctrl.Name == ctrl3.Name && (ctrl.Name.Substring(0,3) == "txt" || ctrl.Name.Substring(0,3) == "cbo"))
                            {
                                ctrl3.Text = ctrl.Text;
                            }
                        }
                    }
                }
            }
        }

        public static string GetControlChangedValue(Form frm)
        {
            string strValue = string.Empty;

            foreach (Control ctrl in frm.Controls)
            {
                if (ctrl.Controls.Count > 0)
                    GetChangedValue(ctrl, ref strValue);
                else
                {
                    if (ctrl.Name.Substring(0, 3) == "txt" || ctrl.Name.Substring(0, 3) == "cbo")
                        if (ctrl.Tag.ToString() != ctrl.Text)
                            strValue += ctrl.Name.Replace("txt", "").Replace("cbo", "") + ":Orig=" + ctrl.Tag.ToString() + ", Mod=" + ctrl.Text + "\n";
                }
            }

            return strValue;
        }

        public static string GetControlTextValue(Form frm)
        {
            string strValue = string.Empty;

            foreach (Control ctrl in frm.Controls)
            {
                if (ctrl.Controls.Count > 0)
                    GetTextValue(ctrl, ref strValue);
                else
                {
                    if (ctrl.Name.Substring(0, 3) == "txt" || ctrl.Name.Substring(0, 3) == "cbo")
                        strValue += ctrl.Name.Replace("txt", "").Replace("cbo", "").Replace("chk", "") + ": " + ctrl.Text + "\n";
                }
            }


            return strValue;
        }

        public static void GetTextValue(Control ctrls, ref string strValue)
        {
            foreach (Control ctrl in ctrls.Controls)
            {
                if (ctrl.Controls.Count > 0)
                    GetTextValue(ctrl, ref strValue);
                else
                {
                    if (ctrl.Name.Trim() != string.Empty)
                        if (ctrl.Name.Substring(0, 3) == "txt" || ctrl.Name.Substring(0, 3) == "cbo" || ctrl.Name.Substring(0, 3) == "chk")
                            strValue += ctrl.Name.Replace("txt", "").Replace("cbo", "").Replace("chk","") + ": " + ctrl.Text + "\n";
                }
            }
        }
        

        public static void PopulateControlTag(Form frm)
        {
            foreach (Control ctrl in frm.Controls)
            {
                if (ctrl.Controls.Count > 0)
                    PopulateTag(ctrl);
                else
                    ctrl.Tag = ctrl.Text;
            }
        }

        public static void PopulateTag(Control ctrls)
        {
            ctrls.Tag = ctrls.Text;
            foreach (Control ctrl in ctrls.Controls)
            {
                if (ctrl.Controls.Count > 0)
                    PopulateTag(ctrl);
                else
                    ctrl.Tag = ctrl.Text;
            }
        }

        public static void ClearControlTag(Form frm)
        {
            foreach (Control ctrl in frm.Controls)
            {
                ctrl.Tag = string.Empty;
                foreach (Control ctrl2 in ctrl.Controls)
                {
                    ctrl2.Tag = string.Empty;
                    foreach (Control ctrl3 in ctrl2.Controls)
                    {
                        ctrl3.Tag = string.Empty;
                    }
                }
            }
        }


    }
}
