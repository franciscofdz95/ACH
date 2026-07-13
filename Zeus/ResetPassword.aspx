<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageLogin.master" Inherits="ResetPassword" Theme="" Codebehind="ResetPassword.aspx.cs" %>
<%-- Change the design for PXP-7232 By Ali Khan  --%>
<%@ Register Src="UserControls/ResetPassword.ascx" TagName="ResetPassword" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">   
        <uc1:ResetPassword ID="ResetPassword1" runat="server" />  
</asp:Content>
