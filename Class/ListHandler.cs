using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Nmc.Ach.Dal;
using Infragistics.Win.UltraWinGrid;

namespace AchSystem
{
    class ListHandler
    {
        public static void ListFindItem(UltraGrid grd, string FieldName, string FieldValue)
        {
            UltraGridRow row = null;

            for (int i = 0; i < grd.Rows.Count; i++)
            {
                row = grd.Rows[i];
                if (row.Cells[FieldName].Value.ToString().Trim() == FieldValue)
                {
                    grd.ActiveRow = row;
                    break;
                }
            }
        }

        public static void ListFindItem(ComboBox cbo, string strValue)
        {
            AchListItem item = null;
            cbo.SelectedIndex = -1;

            for (int i = 0; i < cbo.Items.Count; i++)
            {
                item = (AchListItem)cbo.Items[i];
                if (item.ItemValue == strValue)
                {
                    cbo.SelectedIndex = i;
                    break;
                }
            }
        }
    }
}
