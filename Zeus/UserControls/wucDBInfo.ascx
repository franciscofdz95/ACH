<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucDBInfo.ascx.cs" Inherits="ZeusWeb.UserControls.wucDBInfo" %>


<asp:Panel runat="server" ID="pnlDBDebug" Visible="false" CssClass="dbDebugInfo">

    <asp:Panel runat="server" ID="pnlAchDB_Server">
        <b>AchDB_Server:</b>
        <asp:Literal runat="server" ID="litAchDB_Server"></asp:Literal>
    </asp:Panel>
    <asp:Panel runat="server"  ID="pnlFDRDB_Server">
        <b>FDRDB_Server:</b>
        <asp:Literal runat="server" ID="litFDRDB_Server"></asp:Literal>
    </asp:Panel>
    <asp:Panel runat="server"  ID="pnlMerchantDB_Server">
        <b>MerchantDB_Server:</b>
        <asp:Literal runat="server" ID="litMerchantDB_Server"></asp:Literal>
    </asp:Panel>
    <asp:Panel runat="server"  ID="pnlRawDataDB_Server">
        <b>RawDataDB_Server:</b>
        <asp:Literal runat="server" ID="litRawDataDB_Server"></asp:Literal>
    </asp:Panel>
    <asp:Panel runat="server"  ID="pnlTransDB_Server">
        <b>TransDB_Server:</b>
        <asp:Literal runat="server" ID="litTransDB_Server"></asp:Literal>
    </asp:Panel>


    <!-- -->
    &nbsp;
</asp:Panel>
