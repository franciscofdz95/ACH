using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 
/// </summary>
public class GridViewExportUtil
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="gv"></param>
    public static void Export(string fileName, GridView gv)
    {
        string style = @"<style> .text { mso-number-format:\@; } </style> ";

        HttpContext.Current.Response.Clear();

        string header = @"<html xmlns:x=""urn:schemas-microsoft-com:office:excel"">
                    <head>
                      <style>
                      <!--table
                      br {mso-data-placement:same-cell;}
                      tr {vertical-align:top;}
                      -->
                      </style>
                    </head>
                    <body>";
        HttpContext.Current.Response.Write(header);
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(style);


        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a table to contain the grid
                Table table = new Table();

                //  include the gridline settings
                table.GridLines = gv.GridLines;



                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    if (gv.Controls[0].HasControls())
                    {
                        GridViewExportUtil.PrepareControlForExport(((GridViewRow)gv.Controls[0].Controls[0]));
                        ((GridViewRow)gv.Controls[0].Controls[0]).Attributes.Add("style", "border: none;");
                        table.Rows.Add(((GridViewRow)gv.Controls[0].Controls[0]));
                    }

                    GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    //DateTime dt;//code changes by koshlendra for PXP-16084
                    GridViewExportUtil.PrepareControlForExport(row);
                    for (int col = 0; col < row.Cells.Count; col++)
                    {
                        
                       
                        if (row.Cells[col].Text.Contains("<dl>") || row.Cells[col].Text.Contains("<dt>"))
                            row.Cells[col].Text = row.Cells[col].Text.Replace("<dl>", "").Replace("</dl>", "").Replace("<dt>", "<br>").Replace("</dt>", "").Replace("<dd>", "<br>").Replace("<li>", "&nbsp; &nbsp; &nbsp;<b>.</b>&nbsp;").Replace("</li>", "").Replace("</dd>", "").Trim();
                        //code changes by koshlendra for PXP-16084 start
                        //if (DateTime.TryParse(row.Cells[col].Text, out dt))
                        //{
                            
                            row.Cells[col].Attributes.Add("class", "text");
                        //}
                        //code changes by koshlendra for PXP-16084 end
                    }
                    table.Rows.Add(row);
                }

                //  add the footer row to the table
                if (gv.FooterRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }


                //suppress hidden columns
                for (int i = 0; i < gv.Columns.Count; i++)
                {
                    if (!gv.Columns[i].Visible)
                    {
                        foreach (TableRow row in table.Rows)
                        {
                            row.Cells[i].Visible = false;
                        }
                    }
                }

                //  render the table into the htmlwriter
                table.RenderControl(htw);

                //  render the htmlwriter into the response

                // set content type and character set to cope with european chars like the umlaut.
                HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">\n");

                // add the style props to get the page orientation
                HttpContext.Current.Response.Write(AddExcelStyling());
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.Write("</body>"); HttpContext.Current.Response.Write("</html>");
                HttpContext.Current.Response.End();
            }
        }
    }

    /// <summary>
    /// Replace any of the contained controls with literals
    /// </summary>
    /// <param name="control"></param>
    private static void PrepareControlForExport(Control control)
    {
        for (int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if (current is LinkButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));

                //if (current.Visible)
                //{
                //    control.Controls.Remove(current);
                //    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                //}
                //else
                //{
                //    /* added to prevent duplicate values in the export */
                //    control.Controls.Remove(current);
                //    control.Controls.AddAt(i, new LiteralControl(""));
                //}

            }
            if (current is TextBox)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as TextBox).Text));
            }
            if (current is Label)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as Label).Text));
            }
            else if (current is ImageButton)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
            }
            else if (current is HyperLink)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
            }
            else if (current is DropDownList)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
            }
            else if (current is CheckBox)
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
            }
            else if (current is Button)
            {
                control.Controls.Remove(current);
                //control.Controls.AddAt(i, new LiteralControl(""));
            }
            else if (current is HtmlImage)
            {
                control.Controls.Remove(current);
            }
            else if (current is Image)
            {
                control.Controls.Remove(current);
            }
            else if (current is GridView)
            {
                control.Controls.Remove(current);
                //  Create a table to contain the grid
                Table table = new Table();
                GridView gv = (GridView)current;
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        //  include the gridline settings
                        table.GridLines = gv.GridLines;

                        //  add the header row to the table
                        if (gv.HeaderRow != null)
                        {
                            GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                            table.Rows.Add(gv.HeaderRow);
                        }

                        //  add each of the data rows to the table
                        foreach (GridViewRow row in gv.Rows)
                        {
                            GridViewExportUtil.PrepareControlForExport(row);
                            table.Rows.Add(row);
                        }

                        //  add the footer row to the table
                        if (gv.FooterRow != null)
                        {
                            GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                            table.Rows.Add(gv.FooterRow);
                        }

                        //  render the table into the htmlwriter
                        table.RenderControl(htw);

                        //  render the htmlwriter into the response
                        control.Controls.AddAt(i, new LiteralControl(sw.ToString()));
                    }
                }

            }
            if (current.HasControls())
            {
                GridViewExportUtil.PrepareControlForExport(current);
            }
        }
    }

    private static string AddExcelStyling()
    {

        // add the style props to get the page orientation

        StringBuilder sb = new StringBuilder();

        sb.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office'\n" + "xmlns:x='urn:schemas-microsoft-com:office:excel'\n" +
        "xmlns='http://www.w3.org/TR/REC-html40'>\n" +
        "<head>\n");
        sb.Append("<style>\n");
        sb.Append("@page");

        //page margin can be changed based on requirement.....
        sb.Append("{margin:.5in .75in .5in .75in;\n");
        sb.Append("mso-header-margin:.5in;\n");
        sb.Append("mso-footer-margin:.5in;\n");
        sb.Append("mso-page-orientation:landscape;}\n");
        sb.Append("</style>\n");
        sb.Append("<!--[if gte mso 9]><xml>\n");
        sb.Append("<x:ExcelWorkbook>\n");
        sb.Append("<x:ExcelWorksheets>\n");
        sb.Append("<x:ExcelWorksheet>\n");
        sb.Append("<x:Name>Projects 3 </x:Name>\n");
        sb.Append("<x:WorksheetOptions>\n");
        sb.Append("<x:Print>\n");
        sb.Append("<x:ValidPrinterInfo/>\n");
        sb.Append("<x:PaperSizeIndex>9</x:PaperSizeIndex>\n");
        sb.Append("<x:HorizontalResolution>600</x:HorizontalResolution\n");
        sb.Append("<x:VerticalResolution>600</x:VerticalResolution\n");
        sb.Append("</x:Print>\n");
        sb.Append("<x:Selected/>\n");
        sb.Append("<x:DoNotDisplayGridlines/>\n");
        sb.Append("<x:ProtectContents>False</x:ProtectContents>\n");
        sb.Append("<x:ProtectObjects>False</x:ProtectObjects>\n");
        sb.Append("<x:ProtectScenarios>False</x:ProtectScenarios>\n");
        sb.Append("</x:WorksheetOptions>\n");
        sb.Append("</x:ExcelWorksheet>\n");
        sb.Append("</x:ExcelWorksheets>\n");
        sb.Append("<x:WindowHeight>12780</x:WindowHeight>\n");
        sb.Append("<x:WindowWidth>19035</x:WindowWidth>\n");
        sb.Append("<x:WindowTopX>0</x:WindowTopX>\n");
        sb.Append("<x:WindowTopY>15</x:WindowTopY>\n");
        sb.Append("<x:ProtectStructure>False</x:ProtectStructure>\n");
        sb.Append("<x:ProtectWindows>False</x:ProtectWindows>\n");
        sb.Append("</x:ExcelWorkbook>\n");
        sb.Append("</xml><![endif]-->\n");
        sb.Append("</head>\n");
        sb.Append("<body>\n");
        return sb.ToString();
    }

    public static void ExportText(string fileName, GridView gv, char delimiter, char textqualifier, int start_ind, int end_ind)
    {
        //string style = @"<style> .text { mso-number-format:\@; } </script> ";


        StringBuilder sb = new StringBuilder();
        string output = "";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {

                //  add the header row to the table
                if (gv.HeaderRow != null)
                {
                    sb.Append(GridViewExportUtil.PrepareControlForExportText(gv.HeaderRow, delimiter, textqualifier, 0, start_ind, end_ind));
                    sb.Append(Environment.NewLine);
                }

                //  add each of the data rows to the table
                foreach (GridViewRow row in gv.Rows)
                {
                    sb.Append(GridViewExportUtil.PrepareControlForExportText(row, delimiter, textqualifier, 0, start_ind, end_ind));
                    sb.Append(Environment.NewLine);
                }

                //  add the footer row to the table
                //if (gv.FooterRow != null)
                //{
                //    sb.Append(GridViewExportUtil.PrepareControlForExportText(gv.FooterRow, delimiter, textqualifier) + "\n");
                //}



                output = sb.ToString();

            }
        }

        if (output != "")
        {

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", fileName));
            HttpContext.Current.Response.AddHeader("Content-Length", output.Length.ToString());
            HttpContext.Current.Response.ContentType = "text/plain";
            HttpContext.Current.Response.Write(output);
            HttpContext.Current.Response.End();
        }
    }

    private static string PrepareControlForExportText(Control control, char delimiter, char textqualifier, int level)
    {

        return PrepareControlForExportText(control, delimiter, textqualifier, level, 0, control.Controls.Count);
    }

    private static string PrepareControlForExportText(Control control, char delimiter, char textqualifier, int level, int start_ind, int end_ind)
    {
        ++level;

        int virt_start_ind = 0;
        int virt_end_ind = 0;

        if (level == 1)
        {
            virt_start_ind = start_ind;
            virt_end_ind = end_ind;
        }
        else
        {
            virt_start_ind = 0;
            virt_end_ind = control.Controls.Count;
        }



        StringBuilder sb = new StringBuilder();

        string s = string.Empty;

        for (int i = virt_start_ind; i < virt_end_ind; i++)
        {
            s = String.Empty;

            Control current = control.Controls[i];
            if (current is LinkButton)
            {
                s = (current as LinkButton).Text;
            }
            else if (current is ImageButton)
            {
                s = (current as ImageButton).AlternateText;
            }
            else if (current is HyperLink)
            {
                s = (current as HyperLink).Text;
            }
            else if (current is Label)
            {
                s = (current as Label).Text;
            }
            else if (current is DropDownList)
            {
                s = (current as DropDownList).SelectedItem.Text;
            }
            else if (current is CheckBox)
            {
                s = ((current as CheckBox).Checked ? "True" : "False");
            }
            else if (current is DataControlFieldHeaderCell)
            {
                s = (current as DataControlFieldHeaderCell).Text;
            }
            else if (current is DataControlFieldCell)
            {
                s = (current as DataControlFieldCell).Text.Replace("&nbsp;", "");
            }


            if (current.HasControls())
            {
                s = GridViewExportUtil.PrepareControlForExportText(current, delimiter, textqualifier, level) + " ";
            }

            if (textqualifier != '\0' && level != 2)
            {
                sb.Append(textqualifier);
            }

            //if (textqualifier != '\0' && s.Contains(textqualifier.ToString()))
            //{
            //    s = s.Replace(textqualifier.ToString(), @"\" + textqualifier.ToString());
            //}


            sb.Append(s);


            if (textqualifier != '\0' && level != 2)
            {
                sb.Append(textqualifier);
            }

            if (i != (virt_end_ind - 1) && level != 2)
            {
                sb.Append(delimiter);
            }

        }

        return sb.ToString();
    }


    public static void PrepareWorksheetFromDataTable(ExcelWorksheet ws, DataTable dt, string[,] liHeader)
    {
        // formatting the excel columns based on the type in the datatable.
        for (int i = 0; i < dt.Columns.Count; i++)
        {

            if (liHeader != null)
            {
                string[] arr = GetRowByIndex(liHeader, 1, dt.Columns[i].ColumnName);

                if (arr != null)
                {
                    string header_type = arr[2];

                    switch (header_type)
                    {
                        case "currency":
                            ws.Column(i + 1).Style.Numberformat.Format = "$#,##0.00 ; ($#,##0.00)";
                            break;

                        case "percent":
                        case "percentage":
                            ws.Column(i + 1).Style.Numberformat.Format = "0.00%";
                            break;
                    }
                }
            }
           
        }

        // freeze the top row
        ws.View.FreezePanes(2, 1);

    }

    public static DataTable GetExportableDataTable<T>(IEnumerable<T> collection, string[,] liHeader)
    {
        DataTable dt = new DataTable("DataTable");

        Type myType = typeof(T);

        DataColumn[] arrSortedDC = new DataColumn[liHeader.GetLength(0)];

        int index = 0;

        // loop through each item in our custom header. the order of the dictionary sets the order of the columns.

        for (int i = 0; i < liHeader.GetLength(0); i++)
        //foreach (KeyValuePair<string, string> kvp in diHeader)
        {
            string property_name = liHeader[i, 0];
            string header_name = liHeader[i, 1];
            string header_type = liHeader[i, 2];
            // loop through each property in the item (the class)
            foreach (PropertyInfo info in myType.GetProperties())
            {

                if (property_name == info.Name)
                {
                    string column_name = header_name;

                    DataColumn dc = new DataColumn(column_name);

                    switch (header_type)
                    {
                        case "currency":
                            dc.DataType = typeof(Decimal);
                            break;

                        case "integer":
                            dc.DataType = typeof(Int32);
                            break;

                        case "percent":
                            dc.DataType = typeof(Decimal);
                            break;

                        case "datetime":
                            dc.DataType = typeof(DateTime);
                            break;

                        default: // string
                            dc.DataType = typeof(String);
                            break;
                    }

                    arrSortedDC[index] = dc;
                    break;
                }
            }
            index++;
        }

        // add our sorted columns to the datatable.
        for (int i = 0; i < arrSortedDC.Length; i++)
        {
            if (arrSortedDC[i] == null)
            {
                // this should never be!
                dt.Columns.Add(new DataColumn("Empty Column"));
            }
            else
            {
                dt.Columns.Add(arrSortedDC[i]);
            }

        }


        //Populate the data table
        if (collection != null)
        {
            foreach (T item in collection)
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                // loop through each property in the item
                foreach (PropertyInfo pi in myType.GetProperties())
                {
                    if (pi.GetValue(item, null) != null)
                    {

                        string[] arr = GetRowByIndex(liHeader, 0, pi.Name);

                        if (arr != null)
                        {
                            string property_name = arr[0];
                            string header_name = arr[1];
                            string header_type = arr[2];

                            if (dr.Table.Columns.Contains(header_name))
                            {

                                dr[header_name] = pi.GetValue(item, null);

                                

                            }
                        }


                    }
                }
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    private static string[] GetRowByIndex(string[,] arr, int index, string value)
    {
        string[] ret = null;

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            if (arr[i, index] == value)
            {
                ret = new string[3] { arr[i, 0], arr[i, 1], arr[i, 2] };
                break;
            }
        }

        return ret;
    }


    /// <summary>
    /// given a raw datatable, keep only the columns listed in index0, rename that column to index1, and convert that type to index2
    /// </summary>
    /// <param name="dataTable"></param>
    /// <param name="liHeaders"></param>
    /// <returns></returns>
    public static DataTable GetExportableDataTable(DataTable dataTable, string[,] liHeader)
    {
        DataTable dt = new DataTable("DataTable");

        DataColumn[] arrSortedDC = new DataColumn[liHeader.GetLength(0)];

        int index = 0;

        // loop through each item in our custom header. the order of the dictionary sets the order of the columns.

        for (int i = 0; i < liHeader.GetLength(0); i++)
        //foreach (KeyValuePair<string, string> kvp in diHeader)
        {
            string property_name = liHeader[i, 0];
            string header_name = liHeader[i, 1];
            string header_type = liHeader[i, 2];
            // loop through each property in the item (the class)
            foreach (DataColumn origdc in dataTable.Columns)
            {

                if (property_name == origdc.ColumnName)
                {
                    string column_name = header_name;

                    DataColumn dc = new DataColumn(column_name);

                    switch (header_type)
                    {
                        case "currency":
                            dc.DataType = typeof(Decimal);
                            break;

                        case "integer":
                            dc.DataType = typeof(Int32);
                            break;

                        case "percent":
                            dc.DataType = typeof(Decimal);
                            break;

                        case "datetime":
                            dc.DataType = typeof(DateTime);
                            break;

                        default: // string
                            dc.DataType = typeof(String);
                            break;
                    }

                    arrSortedDC[index] = dc;
                    break;
                }
            }
            index++;
        }

        // add our sorted columns to the datatable.
        for (int i = 0; i < arrSortedDC.Length; i++)
        {
            if (arrSortedDC[i] == null)
            {
                // this should never be!
                dt.Columns.Add(new DataColumn("Empty Column " + i.ToString()));
            }
            else
            {
                dt.Columns.Add(arrSortedDC[i]);
            }

        }


        //Populate the data table
        if (dataTable != null)
        {
            foreach (DataRow origdr in dataTable.Rows)
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                // loop through each property in the item
                //foreach (PropertyInfo pi in myType.GetProperties())

                // find the value from the orig table.



                // add it to the new one.

                for( int i = 0; i < liHeader.GetLength(0) ; i++ )
                {
                    string property_name = liHeader[i, 0];
                    string header_name = liHeader[i, 1];
                    string header_type = liHeader[i, 2];

                    if (dataTable.Columns.Contains(property_name))
                    {
                        dr[header_name] = origdr[property_name];
                    }

                }
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }
}

