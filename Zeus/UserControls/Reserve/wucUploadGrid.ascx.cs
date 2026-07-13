using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.BusinessObjects.Reserve;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls.Reserve
{  

    public partial class wucUploadGrid : wucBaseSearch
    {
        public eRDBReserveUploadTypeID UploadType { get; set; }

        public GridView UploadGrid
        {
            get
            {
                return this.grdUpload;
            }
        }

        public DateTime LastUploadedDate
        {
            get { return (DateTime)(ViewState["LastUploadedDate"] ?? DateTime.MinValue); }
            set { ViewState["LastUploadedDate"] = value; }
        }

        #region sorting_stuff
        public SortDirection VsReportDate
        {
            get
            {
                if (ViewState["VsReportDate"] == null)
                {
                    ViewState["VsReportDate"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsReportDate"];
            }
            set { ViewState["VsReportDate"] = value; }
        }
        public SortDirection VsAmount
        {
            get
            {
                if (ViewState["VsAmount"] == null)
                {
                    ViewState["VsAmount"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsAmount"];
            }
            set { ViewState["VsAmount"] = value; }
        }
        public SortDirection VsTransType
        {
            get
            {
                if (ViewState["VsTransType"] == null)
                {
                    ViewState["VsTransType"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsTransType"];
            }
            set { ViewState["VsTransType"] = value; }
        }
        public SortDirection VsReserveType
        {
            get
            {
                if (ViewState["VsReserveType"] == null)
                {
                    ViewState["VsReserveType"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsReserveType"];
            }
            set { ViewState["VsReserveType"] = value; }
        }
        public SortDirection VsMethod
        {
            get
            {
                if (ViewState["VsMethod"] == null)
                {
                    ViewState["VsMethod"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsMethod"];
            }
            set { ViewState["VsMethod"] = value; }
        }
        public SortDirection VsBank
        {
            get
            {
                if (ViewState["VsBank"] == null)
                {
                    ViewState["VsBank"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsBank"];
            }
            set { ViewState["VsBank"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.BindGrid();
            }

            
        }

        protected void grdUpload_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                RDBUpload objU = (RDBUpload)e.Row.DataItem;

                Label lbDate = (Label)e.Row.FindControl("lblDateCompleted");

                if (objU.DateCompleted == DateTime.MinValue)
                {
                    lbDate.Text = "Never";
                }
                else
                {
                    lbDate.Text = objU.DateCompleted.ToString();
                }

                this.LastUploadedDate = objU.LastUploadedInType;
                
            }
        }

        public void BindGrid()
        {
            if (this.UploadType != eRDBReserveUploadTypeID.NotSet)
            {
                Hashtable prms = new Hashtable();

                this.PageSize = 999;

                prms.Add("@UploadTypeID", this.UploadType);
                prms.Add("@PageSize", this.PageSize);

                this.m_Prms = prms;

                List<RDBUpload> li = DataReserve.GetRDBUpload(this.m_Prms);

                if (li == null)
                {
                    li = new List<RDBUpload>();
                }

                this.grdUpload.DataSource = li;
                this.grdUpload.DataBind();
            }

        }


      
    }
}
