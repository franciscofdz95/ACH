<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucRecentlyAccessedMerchants.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucRecentlyAccessedMerchants" %>
<asp:Panel runat="server" ID="pnlRecent">
    <fieldset style="width: 320px;">
        <legend>My Recently Accessed Merchants</legend>
        <asp:Literal runat="server" ID="litRecent"></asp:Literal>
    </fieldset>
</asp:Panel>
