<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucRecentlyAccessedPartners.ascx.cs"
    Inherits="ZeusWeb.UserControls.SidePanel.wucRecentlyAccessedPartners" %>
<asp:Panel runat="server" ID="pnlRecent">
    <fieldset style="width: 320px;">
        <legend>Recently Accessed Agents</legend>
        <asp:Literal runat="server" ID="litRecent"></asp:Literal>
    </fieldset>
</asp:Panel>
