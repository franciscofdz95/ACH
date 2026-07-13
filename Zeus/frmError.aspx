<%@ Page Language="C#" MasterPageFile="~/MasterPageNoMenu.master" AutoEventWireup="true"
    Inherits="frmError" Title="Untitled Page" CodeBehind="frmError.aspx.cs" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="errorpage">
        <p>
            <strong>Error Handler</strong>
        </p>
        <p>
            An error has occurred in the application. The Site Administrator has been Notified.
            Please try again later.
        </p>
        <p>
            <asp:HyperLink ID="HyperLink1" NavigateUrl="~/frmlogin.aspx" runat="server">Return to Login Page</asp:HyperLink>
        </p>
    </div>
</asp:Content>
