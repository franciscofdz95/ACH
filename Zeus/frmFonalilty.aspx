<%@ Page Language="C#" MasterPageFile="~/MasterPageLogin.master" AutoEventWireup="true" Theme="" Inherits="frmFonalilty" Title="Login" Codebehind="frmFonalilty.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language='JavaScript' type="text/javascript">

        function SetFocus()
        {
            document.getElementById('<%=txtUserName.ClientID %>').focus();
        }
        window.onload = SetFocus;
        
        function CheckLabel()
        {
            if (Page_ClientValidate()) {
                return true;
            }
            var lblError = $('#<%=lblError.ClientID %>');
            if (lblError.length > 0) {
                lblError.text('');
                lblError.hide();
            }
            return false;
        }
        
//        $(document).ready(function() {
//            
//	        $( "#tabLogin" ).tabs();
//        });
        
    </script>
    <%-- design Change for PXP-7232 by ali khan --%>
<asp:ValidationSummary ID="ValidationSummary1" CssClass="error col-md-6 col-sm-6 col-12 offset-md-3 offset-sm-3 form-group" runat="server"></asp:ValidationSummary>
    <asp:Label runat="server" ID="lblError" CssClass="col-md-6 col-sm-6 col-12 offset-md-3 offset-sm-3 form-group errorSpan"></asp:Label>
    <div class="col-md-12 col-lg-6 col-sm-6 col-12 offset-lg-3 page-right text-center resetForm">
        <h1 class="page-title">Fonality Interface</h1>
        <div class="row">
            <div class="col-lg-12 col-12">
                <div class="form-group">
				
                     <asp:TextBox ID="txtUserName" placeholder="Username" CssClass="form-control" runat="server" Enabled="False"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName"
                        Display="None" ErrorMessage="User Name is required."></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <asp:TextBox ID="txtPassword" placeholder="Password" CssClass="form-control" runat="server" TextMode="Password" AutoCompleteType="disabled" Enabled="False"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                        Display="None" ErrorMessage="Password is required."></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
				<label>Phone:</label>
                    <asp:Label ID="lblPhone" runat="server" ></asp:Label>
                </div>
                <div class="form-group">
				<label>MID:</label>
                    <asp:Label ID="lblMID" runat="server"  ></asp:Label>
                </div>
                <div class="form-group">
				<label >Landing Page</label>
                    <asp:Label ID="lblLandingPage" runat="server" >Merchant</asp:Label>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary"  Text="Login" OnClientClick="return CheckLabel();" OnClick="btnLogin_Click"/>
                </div>
            </div>
        </div>
    </div>
   
</asp:Content>
