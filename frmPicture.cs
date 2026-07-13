using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmPicture : Form
    {
        private Image m_ReleaseForm = null;

        public frmPicture()
        {
            InitializeComponent();
        }
        
        public frmPicture(Image img)
        {
            InitializeComponent();

            m_ReleaseForm = img;
            pic.Image = ImageHandler.ResizeImage(m_ReleaseForm, 850, 1100);
        }

        private void pdMain_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Point p = new Point(0, 0);
            e.Graphics.DrawImage(m_ReleaseForm, p);

            e.HasMorePages = false ;

        }

        private void tbrTop_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            { 
                case "Print":
                    pdMain.Print();
                    break;
            }
        }
    }
}