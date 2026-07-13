using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZeusWeb.Extensions;
using ZeusWeb.Class;

namespace ZeusWeb.Extensions
{
    public static class GridViewExtensions
    {
        public static void Default(this GridView gridView, bool showHeader = false)
        {
            gridView.AlternatingRowStyle.CssClass = "alt";
            gridView.FooterStyle.CssClass = "footer";
            gridView.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            gridView.AutoGenerateColumns = false;
            gridView.ShowHeaderWhenEmpty = showHeader;
            gridView.CssClass = "mGrid";

            gridView.EmptyDataText = " No changes for selected field.";
        }

        public static void DefaultWithPager(this GridView gridView, int pageSize = 20, bool showHeader = false)
        {
            Default(gridView, showHeader);
            SetPagerSettings(gridView, pageSize);
        }

        public static void SetPagerSettings(this GridView gridView, int pageSize = 20)
        {
            gridView.AllowCustomPaging = true;
            gridView.AllowPaging = true;
            gridView.PageSize = pageSize;

            gridView.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            gridView.PagerSettings.PageButtonCount = 10;
            gridView.PagerSettings.FirstPageText = "&laquo;";
            gridView.PagerSettings.LastPageText = "&raquo;";

            gridView.PagerStyle.CssClass = "pgr";
        }

        public static void AddBoundColumn(this GridView gridView, string headerText, string dataField)
        {
            gridView.Columns.Add(new BoundField
            {
                HeaderText = headerText,
                DataField = dataField,
            });
        }

        public static void SetDataSource(this GridView gridView, DataTable dataSource, int pageIndex, string columnTotalRecordCount = "TotalRecordCount")
        {
            var _totalRecordCount = 0;
            if (dataSource.Rows.Count > 0)
            {
                int.TryParse(dataSource.Rows[0][columnTotalRecordCount].ToString(), out _totalRecordCount);
            }
            SetDataSource(gridView, dataSource, pageIndex, _totalRecordCount);
        }

        public static void SetDataSource(this GridView gridView, object dataSource, int pageIndex, int totalRecordCount)
        {
            gridView.PageIndex = pageIndex;
            gridView.VirtualItemCount = totalRecordCount;
            SetDataSource(gridView, dataSource);
        }

        public static void SetDataSource(this GridView gridView, object dataSource)
        {
            gridView.DataSource = dataSource;
            gridView.DataBind();
        }
    }
}
