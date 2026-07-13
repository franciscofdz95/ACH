<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true" Inherits="frmStatements" Title="Statements" Codebehind="frmStatements.aspx.cs" %>

<%@ Register TagPrefix="mps" TagName="Statemetns" Src="~/UserControls/Statements.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage"><asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <h2>
            Monthly Statements</h2>
        <table cellpadding="2" cellspacing="2" border="1" style="text-align: left">
            <tr>
                <td>
                    <mps:Statemetns ID="ctrlStatements" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
