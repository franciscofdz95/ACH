using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;

namespace ACH2007
{
    public interface iFormData 
    {
        iData Data
        {
            get;
            set;
        }

        string KeyColumnName
        {
            get;
            set;
        }

        SqlDataReader Dr
        {
            get;
            set;
        }

        string ChangeValue
        {
            get;
            set;
        }

        frmSearch SearchForm
        {
            get;
            set;
        }

        long ID
        {
            get;
            set;
        }
        bool Showing
        {
            get;
            set;
        }
        bool Adding
        {
            get;
            set;
        }
        bool IsDirty
        {
            get;
            set;
        }
        Form GetForm();
        void FormOpen(UltraGridRow dr);
        bool FormFind();
        void FormShow();
        void FormNew();
        void FormClear();
        void FormUndo();
        bool FormDelete(UltraGridRow dr, bool Prompt);
        bool FormDelete(long lngID, bool Prompt);
        bool FormAdd();
        bool FormSave();
        bool FormUpdate();
        void FormControlChanged();
        void FormToggleButtons();
        bool FormDataCheck();
        void FormLogChange();

    }
}
