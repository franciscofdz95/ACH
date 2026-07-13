<%@ Page Language="C#" MasterPageFile="~/MasterPageNoMenu.master" AutoEventWireup="true" Inherits="frmChangePasswordNotification"
    Title="Change Password Notification" Codebehind="frmChangePasswordNotification.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="text-align: center; margin-top: 50px;">
        <h2>
            <strong>
                <asp:Label ID="lblMessage" runat="server" Text="Label"></asp:Label></strong></h2>
        <asp:Button ID="btnChangeNow" runat="server" Text="Change Now" OnClick="btnChangeNow_Click" />&nbsp;<asp:Button
            ID="btnChangeLater" runat="server" Text="Change Later" OnClick="btnChangeLater_Click" />
    </div>
</asp:Content>
