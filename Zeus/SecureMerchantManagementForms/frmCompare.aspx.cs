using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DifferenceEngine;
using PaymentXP.DataObjects;

public partial class frmCompare : frmBaseDataEntry
{
    private DiffEngineLevel _level = DiffEngineLevel.FastImperfect;

   

    protected void Page_Load(object sender, EventArgs e)
    {
        //Label1.Text = Server.HtmlEncode("<b>test</b>");
        if (Request["CrawlerID"] != null)
        {
            string CrawlerID = Request["CrawlerID"];
            Hashtable prms = new Hashtable();
            prms.Add("@CrawlerID", CrawlerID);
            SqlDataReader dr = DataAccess.DataCrawlerDao.GetCrawlerDR(prms);

            if (dr.Read())
            {
                string ActiveCrawlerID = dr["ActiveCrawlerID"].ToString();
                dr.Close();
                string source = string.Format(ConfigurationManager.AppSettings["MDoc_Crawler_Output"] + @"\{1}\{0}\htm_{0}.html", ActiveCrawlerID, UserSessions.CurrentMerchantApp.ID);
                string dest = string.Format(ConfigurationManager.AppSettings["MDoc_Crawler_Output"] + @"\{1}\{0}\htm_{0}.html", CrawlerID, UserSessions.CurrentMerchantApp.ID);
                TextDiff(source, dest);
            }
        }
    }

    public void Results(DiffList_TextFile source, DiffList_TextFile destination, ArrayList DiffLines, double seconds)
    {
        //this.Text = string.Format("Results: {0} secs.",seconds.ToString("#0.00"));

        //ListItem lviS;
        //ListItem lviD;
        int cnt = 1;
        int i;
        DataTable tblSource = new DataTable();
        tblSource.Columns.Add("Line");
        tblSource.Columns.Add("Data");
        tblSource.Columns.Add("DiffResult");

        DataTable tblDestination = new DataTable();
        tblDestination.Columns.Add("Line");
        tblDestination.Columns.Add("Data");
        tblDestination.Columns.Add("DiffResult");
        //<asp:Label ID="Label2" CssClass="MyStyle" runat="server" Text=<%# DataBinder.Eval(Container.DataItem, "line").ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "data").ToString())  %> ></asp:Label>

        foreach (DiffResultSpan drs in DiffLines)
        {
            switch (drs.Status)
            {
                case DiffResultSpanStatus.DeleteSource:
                    for (i = 0; i < drs.Length; i++)
                    {
                        DataRow s = tblSource.NewRow();
                        s["Line"] = cnt.ToString("00000");
                        s["Data"] = ((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line;
                        s["DiffResult"] = "System.Drawing.Color.Red";
                        tblSource.Rows.Add(s);

                        DataRow d = tblDestination.NewRow();
                        d["Line"] = cnt.ToString("00000");
                        d["Data"] = "";
                        d["DiffResult"] = "System.Drawing.Color.LightGray";
                        tblDestination.Rows.Add(d);
                        cnt++;
                    }

                    break;
                case DiffResultSpanStatus.NoChange:
                    for (i = 0; i < drs.Length; i++)
                    {
                        DataRow s = tblSource.NewRow();
                        s["Line"] = cnt.ToString("00000");
                        s["Data"] = ((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line;
                        s["DiffResult"] = "System.Drawing.Color.White";
                        tblSource.Rows.Add(s);

                        DataRow d = tblDestination.NewRow();
                        d["Line"] = cnt.ToString("00000");
                        d["Data"] = ((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line;
                        d["DiffResult"] = "System.Drawing.Color.White";
                        tblDestination.Rows.Add(d);

                        cnt++;
                    }

                    break;
                case DiffResultSpanStatus.AddDestination:
                    for (i = 0; i < drs.Length; i++)
                    {
                        DataRow s = tblSource.NewRow();
                        s["Line"] = cnt.ToString("00000");
                        s["Data"] = "";
                        s["DiffResult"] = "System.Drawing.Color.LightGray";
                        tblSource.Rows.Add(s);

                        DataRow d = tblDestination.NewRow();
                        d["Line"] = cnt.ToString("00000");
                        d["Data"] = ((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line;
                        d["DiffResult"] = "System.Drawing.Color.LightGreen";
                        tblDestination.Rows.Add(d);

                        cnt++;
                    }

                    break;
                case DiffResultSpanStatus.Replace:
                    for (i = 0; i < drs.Length; i++)
                    {
                        DataRow s = tblSource.NewRow();
                        s["Line"] = cnt.ToString("00000");
                        s["Data"] = ((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line;
                        s["DiffResult"] = "System.Drawing.Color.Red";
                        tblSource.Rows.Add(s);

                        DataRow d = tblDestination.NewRow();
                        d["Line"] = cnt.ToString("00000");
                        d["Data"] = ((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line;
                        d["DiffResult"] = "System.Drawing.Color.LightGreen";
                        tblDestination.Rows.Add(d);

                        cnt++;
                    }

                    break;
            }

        }


        lstSource.DataSource = tblSource;
        lstSource.DataBind();

        lstDestination.DataSource = tblDestination;
        lstDestination.DataBind();

    }

    private void TextDiff(string sFile, string dFile)
    {

        DiffList_TextFile sLF = null;
        DiffList_TextFile dLF = null;
        try
        {
            sLF = new DiffList_TextFile(sFile);
            dLF = new DiffList_TextFile(dFile);
        }
        catch
        {
            return;
        }

        try
        {
            double time = 0;
            DiffEngine de = new DiffEngine();
            time = de.ProcessDiff(sLF, dLF, _level);

            ArrayList rep = de.DiffReport();
            this.Results(sLF, dLF, rep, time);
        }
        catch (Exception ex)
        {
            string tmp = string.Format("{0}{1}{1}***STACK***{1}{2}",
                ex.Message,
                Environment.NewLine,
                ex.StackTrace);
            return;
        }
    }

    protected void lst_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            Label data = (Label)e.Item.FindControl("lblData");
            Label diff = (Label)e.Item.FindControl("lblDiff");

            System.Drawing.Color c = System.Drawing.Color.FromName(diff.Text);

            switch (diff.Text)
            {
                case "System.Drawing.Color.LightGreen":
                    data.BackColor = System.Drawing.Color.LightGreen;
                    break;
                case "System.Drawing.Color.White":
                    data.BackColor = System.Drawing.Color.White;
                    break;
                case "System.Drawing.Color.LightGray":
                    data.BackColor = System.Drawing.Color.LightGray;
                    break;
                case "System.Drawing.Color.Red":
                    data.BackColor = System.Drawing.Color.Red;
                    break;
            }

        }

    }

    public override void FormShow(string ID)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormNew()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDataCheck()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormCancel()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void ToggleButtons()
    {
        throw new Exception("The method or operation is not implemented.");
    }
}
