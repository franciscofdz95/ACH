using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

/// <summary>
/// Summary description for XMLUtility
/// </summary>
public class XMLUtility
{
    public XMLUtility()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void Tag(XmlTextWriter xtw, string tagName, string tagValue)
    {
        string tagValueNull = "null";

        if (tagValue == null || tagValue.Trim().Length == 0)
            tagValue = tagValueNull;

        xtw.WriteStartElement(tagName);
        xtw.WriteString(tagValue);
        xtw.WriteEndElement();
    }
    //---------------------------------------------------------------------------------------------------------------
    public static void Tag(XmlTextWriter xtw, string tagName, string tagValue, string attributeName, string attributeValue)
    {
        xtw.WriteStartElement(tagName);
        xtw.WriteAttributeString(attributeName, attributeValue);
        xtw.WriteString(tagValue);
        xtw.WriteEndElement();
    }

    public static void Tag(XmlWriter xtw, string tagName, string tagValue)
    {
        xtw.WriteStartElement(tagName);
        xtw.WriteString(tagValue);
        xtw.WriteEndElement();
    }
    //---------------------------------------------------------------------------------------------------------------
    public static void Tag(XmlWriter xtw, string tagName, string tagValue, string attributeName, string attributeValue)
    {
        xtw.WriteStartElement(tagName);
        xtw.WriteAttributeString(attributeName, attributeValue);
        xtw.WriteString(tagValue);
        xtw.WriteEndElement();
    }
}

/*
   private void GenerateXMLFile(DataSet ds, string filePath)
   {
       try
       {
           //FileStream outstream = new FileStream(filePath, FileMode.Create);
           //outstream.Close();
           //outstream = null;

           //ds.ReadXmlSchema(@"C:\Projects\dev2005\web_application\nmc.intranet.centurion\Centurion2WebPortal\Underwriting\MerchAppVerification\VerificationReportSchema.xsd");
           //ds.WriteXml(filePath);

           if (ds.Tables[0].Rows.Count > 0)
           {
               FileStream outstream = new FileStream(filePath, FileMode.Create);
               XmlTextWriter xtw = new XmlTextWriter(outstream, null);
               xtw.Formatting = Formatting.Indented;
               xtw.Indentation = 2;

               xtw.WriteStartDocument();
               xtw.WriteStartElement("VerificationMerchantApplication");
               xtw.WriteAttributeString("MerchantID", this.lblMerchantID.Text);
               xtw.WriteAttributeString("DBA", this.lblDBAName.Text);
               //-------------------------------------

               string parentVerificationUID = String.Empty;
               string parentVerificationName = String.Empty;

               // loop through parent table.
               foreach (DataRow parentDr in ds.Tables[0].Rows)
               {
                   parentVerificationUID = parentDr["ParentVerificationUID"].ToString().Trim();
                   parentVerificationName = parentDr["Name"].ToString().Trim();

                   xtw.WriteStartElement("Verification");
                   //-------------------------------------                                                

                   this.Tag(xtw, "Name", parentVerificationName, "UID", parentVerificationUID);

                   //-------------------------------------                                                

                   if (ds.Tables[1].Rows.Count > 0)
                   {
                       string child1DocumentUID = String.Empty;
                       string child1DocumentName = String.Empty;

                       foreach (DataRow childDr1 in ds.Tables[1].Rows)
                       {
                           if (parentVerificationUID.Equals(childDr1["ParentVerificationUID"].ToString().Trim()))
                           {
                               child1DocumentUID = childDr1["DocumentUID"].ToString();
                               child1DocumentName = childDr1["DocumentName"].ToString();

                               xtw.WriteStartElement("Document");
                               xtw.WriteAttributeString("Name", child1DocumentName);
                               xtw.WriteAttributeString("UID", child1DocumentUID);

                               //------------------------------------- 

                               if (ds.Tables[2].Rows.Count > 0)
                               {
                                   string child2DocumentUID = String.Empty;

                                   foreach (DataRow childDr2 in ds.Tables[2].Rows)
                                   {
                                       if (child1DocumentUID.Equals(childDr2["DocumentUID"].ToString().Trim()))
                                       {
                                           xtw.WriteStartElement("DocumentHistory");

                                           for (int i = 1; i < ds.Tables[2].Columns.Count; i++)
                                           {
                                               this.Tag(xtw, ds.Tables[2].Columns[i].ToString(), childDr2[i].ToString().Trim());
                                           }

                                           xtw.WriteEndElement(); //xtw.WriteStartElement("DocumentHistory");
                                       }
                                   }
                               }

                               //-------------------------------------
                               xtw.WriteEndElement(); //xtw.WriteStartElement("Document");
                           }
                       }
                   }

                   //-------------------------------------
                   xtw.WriteEndElement(); //xtw.WriteStartElement("Verification");
               }

               //-------------------------------------
               xtw.WriteEndElement();
               xtw.WriteEndDocument();                

               xtw.Flush();
               xtw.Close();
           }
       }
       catch (Exception ex)
       {
           throw ex;
       }                
   }
   */