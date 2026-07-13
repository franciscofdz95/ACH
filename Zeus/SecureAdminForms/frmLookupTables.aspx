<%@ Page Language="C#" MasterPageFile="~/MasterPageAdmin.master" AutoEventWireup="true" Inherits="frmLookupTables" Title="Untitled Page" Codebehind="frmLookupTables.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPageAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div id="contentpage">
    <br />
<asp:Button ID="btnReset" runat="server" Text="Reset Cache Tables" OnClick="btnReset_Click" />
</div>
</asp:Content>

