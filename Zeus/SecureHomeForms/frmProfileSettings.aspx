<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageNoMenu.master" CodeBehind="frmProfileSettings.aspx.cs" Inherits="ZeusWeb.SecureHomeForms.frmProfileSettings" %>

<%--<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>--%>

<%@ Register Src="~/UserControls/wucUserProfile.ascx" TagName="UserProfile" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
    <uc1:UserProfile runat="server" ID="UserProfile1" />
    </div>
</asp:Content>
