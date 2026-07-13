using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using Nmc.Ach.Dal;


namespace AchSystem
{
    class PickerHandler
    {
        public static UltraGridRow PickMerchant()
        {
            frmSearch frm = new frmSearch(ACH_Search.Merchant,new frmMerchant(),true);
            frm.ShowDialog();
            UltraGridRow dr = frm.GetSelectedGridRow();
            frm = null;

            return dr;
        }

        public static UltraGridRow PickTransaction()
        {
            frmSearch frm = new frmSearch(ACH_Search.Transaction , new frmTransaction(), true);
            frm.ShowDialog();
            UltraGridRow dr = frm.GetSelectedGridRow();
            frm = null;

            return dr;
        }

        public static UltraGridRow PickMerchant(int intAchID)
        {
            DataSet ds = null;
            frmSearch frm = new frmSearch(ACH_Search.Merchant, new frmMerchant(), true);
            DataMerchant data = new DataMerchant();
            BindingSource bs = new BindingSource();

            try
            {
                ArrayList prms = new ArrayList();

                prms.Add(new SqlParameter("@AchID", Convert.ToInt32(intAchID)));

                ds = data.Search(prms);

                bs.DataSource = ds.Tables[0];
                frm.grdSearch.DataSource = bs;

                if (frm.grdSearch.Rows.Count > 0)
                {
                    return frm.grdSearch.Rows[0];
                }
                else
                    return null;
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally 
            {
                frm = null;
                data = null;
                bs = null;
            }

        }
    }
}
