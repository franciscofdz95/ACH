<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true"
    ValidateRequest="false" Inherits="frmMerchantNotes" Title="Merchant Notes" CodeBehind="frmMerchantNotes.aspx.cs" %>

<%@ Register Src="../UserControls/wucNotes.ascx" TagName="wucNotes" TagPrefix="uc3" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:Panel ID="pnlTools" runat="server">
    </asp:Panel>
        <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
            <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
        </asp:Panel>
        <br />
        <uc3:wucNotes ID="WucNotes1" runat="server" />
    </div>
</asp:Content>
