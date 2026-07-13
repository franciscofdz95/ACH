<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMerchantUserAccess.ascx.cs" Inherits="ZeusWeb.UserControls.wucMerchantUserAccess" %>
<asp:Panel runat="server" ID="pnlLastViewed">
                    <fieldset style="width: 320px;">
                        <legend>Others Who Have Viewed</legend>
                        <asp:Literal runat="server" ID="litUsers"></asp:Literal>
                    </fieldset>
                </asp:Panel>