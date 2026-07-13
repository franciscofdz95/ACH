using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

// taken from http://alex.buayacorp.com/merge-pdf-files-with-itext-and-net.html
public class PdfMerge
{
    private BaseFont baseFont;
    private bool enablePagination = false;
    private readonly List<PdfReader> documents;
    private int totalPages;

    public BaseFont BaseFont
    {
        get { return baseFont; }
        set { baseFont = value; }
    }

    private string _HeaderText = "";

    public string HeaderText
    {
        get { return _HeaderText; }
        set { _HeaderText = value; }
    }

    public bool EnablePagination
    {
        get { return enablePagination; }
        set
        {
            enablePagination = value;
            if (value && baseFont == null)
            {
                baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            }
        }
    }

    public List<PdfReader> Documents
    {
        get { return documents; }
    }

    public void AddDocument(string filename)
    {
        documents.Add(new PdfReader(filename));
    }
    public void AddDocument(Stream pdfStream)
    {
        documents.Add(new PdfReader(pdfStream));
    }
    public void AddDocument(byte[] pdfContents)
    {
        documents.Add(new PdfReader(pdfContents));
    }
    public void AddDocument(PdfReader pdfDocument)
    {
        documents.Add(pdfDocument);
    }

    public void Merge(string outputFilename)
    {
        Merge(new FileStream(outputFilename, FileMode.Create));
    }
    public void Merge(Stream outputStream)
    {
        if (outputStream == null || !outputStream.CanWrite)
        {
            throw new Exception("OutputStream error. cannot write.");
        }

        Document newDocument = null;
        try
        {
            newDocument = new Document();

            PdfWriter pdfWriter = PdfWriter.GetInstance(newDocument, outputStream);

            newDocument.Open();
            PdfContentByte pdfContentByte = pdfWriter.DirectContent;

            //newDocument.AddHeader("myname", "myheader");

            if (EnablePagination)
            {
                documents.ForEach(delegate(PdfReader doc)
                                  {
                                      totalPages += doc.NumberOfPages;
                                  });
            }

            int currentPage = 1;
            foreach (PdfReader pdfReader in documents)
            {
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    PdfImportedPage importedPage = pdfWriter.GetImportedPage(pdfReader, page);

                    // detect orientation, and then create new page.
                    newDocument.SetPageSize(new Rectangle(0.0F, 0.0F, importedPage.Width, importedPage.Height));
                    newDocument.NewPage();

                    if (this._HeaderText != "" )
                    {
                        newDocument.Add(new Paragraph(this._HeaderText));
                    }
                    
                    // we shift it down a notch to fit the case number on top.
                    pdfContentByte.AddTemplate(importedPage, 0, -20); 
                    
                    if (EnablePagination)
                    {
                        pdfContentByte.BeginText();
                        pdfContentByte.SetFontAndSize(baseFont, 9);
                        if (this._HeaderText != "")
                        {
                            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, string.Format("{2} - {0} of {1}", currentPage++, totalPages, this._HeaderText), 0, 5, 0);
                        }
                        else
                        {
                            pdfContentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, string.Format("{0} of {1}", currentPage++, totalPages), 520, 5, 0);
                        }
                        
                        pdfContentByte.EndText();
                    }
                }
            }
        }
        finally
        {
            outputStream.Flush();
            if (newDocument != null)
            {
                newDocument.Close();
            }
            outputStream.Close();
        }
    }

    public PdfMerge()
    {
        documents = new List<PdfReader>();
    }
}