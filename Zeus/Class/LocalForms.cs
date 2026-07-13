using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for LocalForms
/// </summary>
[DataObject(true)]
public class LocalForms
{
    private string _Type;
    private string _FileName;
    private long _FileSize;

	public LocalForms()
	{

    }

    #region " Setters / Getters "

    public string Type
    {
        get { return this._Type; }
        set { this._Type = value; }
    }

    public string FileName
    {
        get { return this._FileName; }
        set { this._FileName = value; }
    }

    public long FileSize
    {
        get { return this._FileSize; }
        set { this._FileSize = value; }
    }

    #endregion

    #region " Data Access "

    [DataObjectMethod(DataObjectMethodType.Select)]
    public ICollection<LocalForms> GetMerchantApplicationForms()
    {
        string path = System.Web.HttpContext.Current.Server.MapPath("~\\forms\\merchant");

        List<LocalForms> forms = new List<LocalForms>();

        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] filenames = info.GetFiles();

        foreach (FileInfo fi in filenames)
        {
            LocalForms form = new LocalForms();
            form.Type = fi.Extension.Substring(1).ToUpper();
            form.FileName = fi.Name.Substring(0, fi.Name.LastIndexOf("."));
            form.FileSize = fi.Length;
            forms.Add(form);
        }

        return forms;
    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public static DataTable GetForms(string path)
    {
        DataTable dt = CreateDataTable();

        string p = System.Web.HttpContext.Current.Server.MapPath(path);

        DirectoryInfo info = new DirectoryInfo(p);

        FileInfo[] filenames = info.GetFiles();

        foreach (FileInfo fi in filenames)
        {
            DataRow dr = dt.NewRow();
            dr["Type"] = fi.Extension.Substring(1).ToUpper();
            dr["Name"] = fi.Name.Substring(0, fi.Name.LastIndexOf("."));
            dr["Size"] = fi.Length;
            dt.Rows.Add(dr);
        }

        return dt;
    }

    #endregion

    #region " Helper Functions "



    private static DataTable CreateDataTable()
    {
        DataTable dt = new DataTable("MerchantForms");

        DataColumn dc = new DataColumn();
        dc.DataType = System.Type.GetType("System.String");
        dc.ColumnName = "Type";
        dt.Columns.Add(dc);

        dc = new DataColumn();
        dc.DataType = System.Type.GetType("System.String");
        dc.ColumnName = "Name";
        dt.Columns.Add(dc);

        dc = new DataColumn();
        dc.DataType = System.Type.GetType("System.Int64");
        dc.ColumnName = "Size";
        dt.Columns.Add(dc);

        return dt;
    }

    #endregion
}
