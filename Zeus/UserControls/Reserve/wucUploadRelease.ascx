<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucUploadRelease.ascx.cs" Inherits="ZeusWeb.UserControls.Reserve.wucUploadRelease" %>
<%@ Register Src="wucUploadGrid.ascx" TagName="wucUploadGrid" TagPrefix="uc1" %>
<fieldset>

    <legend>Upload Release</legend>
     <asp:Panel runat="server" CssClass="errorlist" ID="pnlWarning" Visible="false">
         <ul>
            <li>
               Please select a valid excel file to upload.  </li>
        </ul>
      
    </asp:Panel>
    <span style="padding: 0px 10px;">Import From Excel:
                <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button runat="server" ID="btnUpload" Text="Submit Excel File" OnClick="btnUpload_Click" />
    </span>
    
    <uc1:wucUploadGrid ID="wucUploadGrid1" runat="server"  />

</fieldset>
