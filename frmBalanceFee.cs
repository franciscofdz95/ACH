using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
namespace AchSystem
{
    public partial class frmBalanceFee : Form
    {
        public frmBalanceFee()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            Nmc.Ach.Dal.DataMerchant data = new Nmc.Ach.Dal.DataMerchant();
            ArrayList ar = new ArrayList();
            ar.Add(new SqlParameter("@BeginPostedDate", txtTransBeginDate.Value));
            DataSet ds = data.SearchBalanceFee(ar);

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables[0];
            grd.DataSource = bs;

            grd.DisplayLayout.Bands[0].Columns["Negative Balance"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Positive Balance"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Total Fees"].Format = "C";

        }

        private void frmBalanceFee_Load(object sender, EventArgs e)
        {
        }
    }
}