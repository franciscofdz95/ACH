<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageMerchant.master"
    Title="Application Info" Inherits="frmCreateMerchantApp" Codebehind="frmCreateMerchantApp.aspx.cs" %>

<%@ Register Src="~/UserControls/NewApp.ascx" TagName="NewApp" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <div id="contentpage"><asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <uc1:NewApp ID="newApp" runat="server" AllFields="true" />
    </div>
</asp:Content>
