<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="UserControls_ResetPassword" CodeBehind="ResetPassword.ascx.cs" %>
<%-- Forget/Reset Password Updated design PXP-7232 --%>
<script>
    function CheckLabel() {

        if (Page_ClientValidate()) {
            return true;
        }
        var lblError = $('#<%=blError.ClientID %>');
        if (lblError.length > 0) {
            lblError.text('');
            lblError.hide();
        }
        return false;
    }
</script>
<asp:BulletedList runat="server" ID="blError" CssClass="col-md-6 col-sm-6 col-12 offset-md-3 offset-sm-3 form-group errorSpan">
</asp:BulletedList>
<asp:ValidationSummary ID="ValidationSummary1" CssClass="error col-md-6 col-sm-6 col-12 offset-md-3 offset-sm-3 form-group" runat="server" />
<asp:Panel runat="server" ID="pnlEmail" CssClass="col-md-6 col-sm-6 col-12 offset-md-3 offset-sm-3 page-right forReset">
    <h1 class="page-title">Reset Password
    </h1>
    <p class="page-help">
        In order to reset your password, please enter your Username and click Submit. You will shortly receive
            an email with a link.
    </p>
    <div class="form-group has-help">
        <asp:TextBox runat="server" CssClass="form-control" ID="tbUsername"></asp:TextBox>
        <span class="highlight"></span>
        <span class="bar"></span>
        <span class="dot field-help">? </span>
        <span title="Please note that the password reset will be sent to the email that we have on file." class="dot field-help">?</span>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbUsername" Display="None"
            ErrorMessage="Username Required">*</asp:RequiredFieldValidator>
    </div>
    <div class="form-group">
    </div>
    <div class="row">
        <div class="col-md-6 col-sm-6 col-12 text-center text-md-right">
            <asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" OnClientClick="return CheckLabel()" CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlConfirmation" CssClass="col-md-12 col-lg-6 col-sm-6 col-12 offset-lg-3 page-right text-center" Visible="false">
    <p>
        An email has been sent to
    <b>
        <asp:Label ID="EmailAddressDisplay" runat="server"></asp:Label></b>.
    </p>
    <p>
        Please open it and follow the link provided.
    </p>
    <p>
        The subject of the email is<br />
        <b>
            <asp:Label runat="server" ID="lblEmailSubject"></asp:Label></b>
    </p>
    <p>
        Please wait a few minutes before trying again.
    </p>
    <p class="">
        Note: At this point, your password has not been reset yet. Once you receive the email and follow the link,
            you will be prompted to enter your username again and a new password.
    </p>
</asp:Panel>
<asp:Panel runat="server" ID="pnlNewPassword" CssClass="col-md-12 col-lg-6 col-sm-6 col-12 offset-lg-3 page-right resetForm" Visible="false">
    <h1 class="page-title">Reset Password</h1>
    <p class="page-help">For security purposes, please re-enter your Username. </p>
    <div class="row">
        <div class="col-lg-12 col-12">
            <div class="form-group">
                <asp:TextBox CssClass="form-control" ID="tbUsernameConfirm" placeholder="Username" runat="server"></asp:TextBox>
                <span class="highlight"></span>
                <span class="bar"></span>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="tbUsernameConfirm" Display="None" ErrorMessage="Username Required">*</asp:RequiredFieldValidator>
            </div>
            <div class="form-group">
                <asp:TextBox CssClass="form-control" ID="tbPassword1" TextMode="password" placeholder="New Password" runat="server"></asp:TextBox>
                <span class="highlight"></span>
                <span class="bar"></span>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbPassword1" Display="None">*</asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbPassword1" Display="None" ErrorMessage="Must enter a new password.">*</asp:RequiredFieldValidator>
            </div>
            <div class="form-group">
                <asp:TextBox CssClass="form-control" ID="tbPassword2" placeholder="Confirm Password" TextMode="password" runat="server"></asp:TextBox>
                <span class="highlight"></span>
                <span class="bar"></span>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="tbPassword2" Display="None" ErrorMessage="Confirm Password">*</asp:RequiredFieldValidator>
            </div>
            <div class="row form-group text-right">
                <div class="col-lg-12 col-12 text-md-center">
                    <asp:Button runat="server" ID="btnSubmitConfirm" CssClass="btn btn-primary" Text="Submit" OnClientClick="return CheckLabel()" OnClick="btnSubmitConfirm_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlError" CssClass="col-md-12 col-lg-6 col-sm-6 col-12 offset-lg-3 page-right text-center" Visible="false">
    That link is expired and no longer valid.
    <br />
    <div class="row form-group forgot-password">
        <div class="col-lg-12 col-12 text-center">
            <asp:HyperLink runat="server" NavigateUrl="~/ResetPassword.aspx" ID="hypReset">Request another password reset</asp:HyperLink>.
        </div>
    </div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlThanks" CssClass="col-md-12 col-lg-6 col-sm-6 col-12 offset-lg-3 page-right" Visible="false">
    <div style="padding: 20px">
        Thank you. Your password has been reset. Please
        <asp:HyperLink runat="server" ID="hypLogin" NavigateUrl="~/frmLogin.aspx">Login</asp:HyperLink>
    </div>
</asp:Panel>
<%-- End Forget/Reset Password Updated design PXP-7232 --%>