<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucUploadADR060.ascx.cs" Inherits="ZeusWeb.UserControls.Reserve.wucUploadADR060" %>

<%@ Register Src="wucUploadGrid.ascx" TagName="wucUploadGrid" TagPrefix="uc1" %>
<fieldset>
     <legend>Upload ADR060</legend>


    <asp:Panel runat="server" CssClass="errorlist" ID="pnlWarning">
        <ul>
            <li>Please Upload Today's ADR060 File! Report Date: <asp:Literal ID="litDivertLastReportDate" runat="server"></asp:Literal></li>
        </ul>
    </asp:Panel>
     <asp:Panel runat="server" CssClass="errorlist" ID="pnlWarningFiletype" Visible="false">
         <ul>
            <li>
               Please select a valid excel file to upload.   </li>
        </ul>
      
    </asp:Panel>

   
   
    <span style="padding: 0px 10px;">Import From Excel:
                <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button runat="server" ID="btnUpload" Text="Submit Excel File" OnClick="btnUpload_Click" />
    </span>

    <uc1:wucUploadGrid ID="wucUploadGrid1" runat="server"  />

</fieldset>