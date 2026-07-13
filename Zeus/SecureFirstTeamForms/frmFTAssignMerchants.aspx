<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFTAssignMerchants.aspx.cs" Inherits="ZeusWeb.SecureFirstTeamForms.frmFTAssignMerchants" MasterPageFile="~/MasterPageFirstTeam.Master" %>

<%@ Register Src="../UserControls/FirstTeam/wucFTAssignMerchants.ascx" TagName="wucFTAssignMerchants"
    TagPrefix="uc1" %>
<%--<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contentpage" style="padding-left: 20px; padding-bottom: 20px;">
        <div class="titleline">
            Potential Premier Services Merchants 
        </div>
            <uc1:wucFTAssignMerchants ID="wucFTAssignMerchants1" runat="server" />
    </div>
</asp:Content>
