<%@ Page Language="C#" MasterPageFile="~/MasterPageLogin.master" Theme="" AutoEventWireup="true" Inherits="frmChangePassword" Title="User Profile" CodeBehind="frmChangePassword.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucUserProfile.ascx" TagName="UserProfile" TagPrefix="uc1" %>
<%-- design Change for PXP-7232 by ali khan --%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function CheckLabel() {
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
    </script>
    <asp:Label ID="lblError" CssClass="col-md-6 col-sm-6 col-12 offset-md-3 offset-sm-3 form-group errorSpan" runat="server" Text=""></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="error col-md-6 col-sm-6 col-12 offset-md-3 offset-sm-3 form-group" ValidationGroup="ChangePassword" />
    <div class="col-md-12 col-lg-6 col-sm-6 col-12 offset-lg-3 page-right text-center resetForm">
        <h1 class="page-title">Change Password</h1>
        <div class="row">
            <div class="col-lg-12 col-12">
                <div class="form-group">
                    <asp:TextBox ID="txtOldPassword" CssClass="form-control" placeholder="Old Password" runat="server" TextMode="Password" ValidationGroup="ChangePassword"></asp:TextBox></td>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtOldPassword"
                         Display="None" ErrorMessage="Must enter your old password." ValidationGroup="ChangePassword"></asp:RequiredFieldValidator>&nbsp;
                                                <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="txtOldPassword" ValidationGroup="ChangePassword"
                                                    ControlToValidate="txtNewPassword" Display="None" ErrorMessage="New password cannot be the same as old password."
                                                    Operator="NotEqual"></asp:CompareValidator>

                </div>
                <div class="form-group">
                    <asp:TextBox ID="txtNewPassword" CssClass="form-control" placeholder="New Password" runat="server" TextMode="Password" ValidationGroup="ChangePassword"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="ChangePassword" runat="server" ControlToValidate="txtNewPassword" Display="None"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="ChangePassword" ControlToValidate="txtNewPassword"
                        Display="None" ErrorMessage="Must enter a new password."></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <asp:TextBox ID="txtConfirmation" CssClass="form-control" placeholder="Confirmation" runat="server" TextMode="Password" ValidationGroup="ChangePassword"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" Display="None" ValidationGroup="ChangePassword" ErrorMessage="Confirmation password does not match new password."
                        ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmation"></asp:CompareValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtConfirmation"
                        Display="None" ErrorMessage="Must enter a confirmation." ValidationGroup="ChangePassword"></asp:RequiredFieldValidator>
                </div>
                <div class="row form-group text-right">
                    <div class="col-lg-12 col-12 text-md-center">
                        <asp:Button ID="btnChange" runat="server" Text="Change" CssClass="btn btn-primary" OnClick="btnChange_Click" OnClientClick="return CheckLabel();" ValidationGroup="ChangePassword" />
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn btn-primary" OnClick="btnCancel_Click" ValidationGroup="ChangePassword" OnClientClick="Page_ValidationActive =false;"
                            Text="Cancel" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
