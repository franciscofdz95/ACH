<%@ Page Language="C#" MasterPageFile="~/MasterPageLogin.master" AutoEventWireup="true"
    Inherits="frmLogin" Title="Login" CodeBehind="frmLogin.aspx.cs" Theme="" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language='JavaScript' type="text/javascript">

        function SetFocus() {
            document.getElementById('<%=txtUserName.ClientID %>').focus();
         }
         window.onload = SetFocus;
        
         function CheckLabel() {            
             
             if (Page_ClientValidate()) {
                 return true;
             }
             var lblError= $('#<%=lblError.ClientID %>');
             if (lblError.length > 0) {
                 lblError.text('');
                 lblError.hide();
             }
             return false;
         }
      //  window.onsubmit = CheckLabel();

         //        $(document).ready(function () {

         //            $("#tabLogin").tabs();
         //        });

    </script>
    <%-- Change the design for PXP-7232 By Ali Khan --%>
    <div class="col-lg-6 col-12 page-left">
        <h2 class="welcome-msg">Welcome to the Zeus Portal</h2>
    </div>
    <div class="col-lg-6 col-12 page-right">
        <div align="left">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="error" runat="server"></asp:ValidationSummary>
            <asp:Label runat="server" ID="lblError" Visible="false" CssClass="errorSpan" ></asp:Label>
        </div>
        <div class="form-group">
            <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" placeholder="Username"></asp:TextBox>
            <span class="highlight"></span>
            <span class="bar"></span>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName"
                Display="None" ErrorMessage="User Name is required."></asp:RequiredFieldValidator>
            <span class="highlight"></span>
            <span class="bar"></span>
        </div>
        <div class="form-group">
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" AutoCompleteType="disabled" placeholder="Password" CssClass="form-control"></asp:TextBox>
            <span class="highlight"></span>
            <span class="bar"></span>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                Display="None" ErrorMessage="Password is required."></asp:RequiredFieldValidator>
            <span class="highlight"></span>
            <span class="bar"></span>
        </div>
        <div class="row form-group forgot-password">
            <div class="col-lg-12 col-12 text-right">
                <asp:HyperLink runat="server" ID="hypReset" NavigateUrl="~/ResetPassword.aspx">Forgot Password</asp:HyperLink>
            </div>
        </div>
        <div class="row form-group text-right login">
            <div class="col-lg-12 col-12 text-md-right">
                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" CausesValidation="true" OnClientClick="return CheckLabel();" CssClass="btn btn-primary" />              
            </div>
        </div>
    </div>

    <%-- End Change the design for PXP-7232 By Ali Khan --%>
</asp:Content>
