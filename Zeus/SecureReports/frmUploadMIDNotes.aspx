<%@ Page Title="Upload Merchant Notes" Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" CodeBehind="frmUploadMIDNotes.aspx.cs" Inherits="SecureReports_frmUploadMIDNotes" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        $(function () {
            $('html,body').css('cursor', 'default');
            $('#<%=btnUpload.ClientID%>').on("click", function () {
                $('html,body').css('cursor', 'wait');
                document.getElementById('<%=lblMsg.ClientID %>').innerHTML = "";
                document.getElementById('<%=lblMsg2.ClientID %>').innerHTML = "";
                document.getElementById('<%=lblProcess.ClientID %>').innerHTML = 'Processing Uploaded File Data........Please Wait !!!';   
		});
	});
    </script>
    <div>
        <div class="title">
            &nbsp;&nbsp;Add Merchant Notes to Multiple ZIDs
        <hr class="line" />
        </div>
         <br />
        <asp:Panel runat="server" ID="pnlIntro" CssClass="intro">
            This facility is provided to upload Multiple Merchant Notes.
             <br />
            <ul>
                <li>Please download &quot;ZeusMerchantNotes.xls&quot; Template by using the &quot;Download&quot; link below. </li>
                <li>Add records in the downloaded file in the order mentioned below : </li>
                <ol>
                    <li> RowID - Integer Number only.</li> 
                    <li> NoteCodeID - Integer Number only (Please refer &quot;NoteCodeID&quot; values available with &quot;RefNoteCodeIDs&quot; workbook from downloaded template sheet). </li>
                    <li> ZID - Integer Number only. </li>
                    <li> Note - Text field.</li>
                </ol>
                <li>Please upload same downloaded file with user added records using Upload button.</li>
            </ul>
            <div style="color:red; font-weight: bold;">
                ** Note: User can add only 300 records(max) in downloaded &quot;ZeusMerchantNotes.xls&quot; Template from this upload facility.
                <br />
                *** Please don&#39;t change file column names, order, extension &amp; workbook name.
            </div>
        </asp:Panel>
        <br />
        <div class="indentedcontent20">
            <asp:Panel ID="pnlDownLoadTemplate" runat="server" Height="" Width="">
                <div>
                    <table>
                        <tr>
                            <td class="lblRight">
                                <asp:Label ID="lblTmplateName" runat="server" Text="Download template : " Width="150px"></asp:Label>
                            </td>
                            <td class="lblLeft">
                                <asp:Label ID="lblFileName" runat="server" Text=" " Width="150px">ZeusMerchantNotes.xls</asp:Label>
                            </td>
                            <td>
                                <asp:HyperLink ID="lnkDownload" runat="server">Download</asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
             <br />
            <asp:Panel ID="pnlUploadFile" runat="server" Height="" Width="">
                <div>
                    <table>
                        <tr>
                            <td>File : </td>
                            <td><asp:FileUpload ID="FileUpload1" runat="server" /></td>
                            <td><asp:Button ID="btnUpload" runat="server" Width="90px" Text="Upload" 
                            CausesValidation="false" OnClick="btnUpload_Click"  /></td>
                        </tr>
                    </table>
                </div>
                <br />
                <asp:Label ID="lblProcess" runat ="server" Font-Bold="true" ForeColor="OrangeRed"></asp:Label>
                 <br />
                <div>
                     <asp:Label ID="lblMsg" runat ="server" Font-Bold="true" ForeColor="Green"></asp:Label>
                     <br />
                    <asp:Label ID="lblMsg2" runat ="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>