<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageSales.master"
    Title="Application" Inherits="frmCreateMerchantApp2" Codebehind="frmCreateMerchantApp2.aspx.cs" %>

<%@ Register Src="~/UserControls/NewApp.ascx" TagName="NewApp" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <div class="dialog" style="padding-right: 10px;">
            <uc1:NewApp ID="newApp" runat="server" AllFields="false" />
        </div>
    </div>
</asp:Content>
