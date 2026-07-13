using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;

namespace AchSystem
{
    public abstract class SearchBase
    {
        public UserControl m_Ctrl = null;

        public SearchBase()
        { 
        }

        public abstract DataSet FormSearch(ArrayList prms);
        public abstract string FormCheck(UserControl ctrl);
        public virtual void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        { }
        public virtual void GridInitializeLayout(UltraGrid grd)
        { }

        public virtual void GridClickCellButton(object sender, CellEventArgs e)
        { }
        public virtual void GridInitializeRow(object sender, InitializeRowEventArgs e)
        {

        }
    }
}
