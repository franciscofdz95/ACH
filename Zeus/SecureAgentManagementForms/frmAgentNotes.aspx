<%@ Page Language="C#" MasterPageFile="~/MasterPageAgent.master" AutoEventWireup="true"
    ValidateRequest="false" Inherits="frmAgentNotes"
    Title="Agent Notes" Codebehind="frmAgentNotes.aspx.cs" %>

<%@ Register Src="../UserControls/wucAgentNotes.ascx" TagName="wucAgentNotes" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/wucAgentsMerchantNotes.ascx" TagName="wucAgentsMerchantNotes" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <asp:Label ID="lblError" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
                        <uc1:wucAgentNotes ID="WucAgentNotes1" runat="server"></uc1:wucAgentNotes>
                       
                    </asp:Panel>
                    <asp:Panel ID="pnlAgentsMerchantNotes" runat="server" Height="100%" Width="100%">
                         <uc2:wucAgentsMerchantNotes ID ="wucAgentsMerchantNotes1" runat="server"></uc2:wucAgentsMerchantNotes>
                    </asp:Panel>
                    <br />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
