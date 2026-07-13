<%@ Page Language="C#" MasterPageFile="~/MasterPageHome.master" AutoEventWireup="true"
    Inherits="frmQueueCU" Title="Credit Underwriting Queue" CodeBehind="frmQueueCU.aspx.cs" %>

<%@ Register Src="../UserControls/wucDocumentUploads.ascx" TagName="wucDocumentUploads"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/wucApplicationQueue.ascx" TagName="wucApplicationQueue"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageHome.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="contentpage">
        <asp:Panel ID="Panel1" runat="server" Height="" Width="">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <fieldset>
                        <table cellspacing="2">
                            <tr>
                                <td style="vertical-align: top;"><b>Office:</b></td>
                                <td style="vertical-align: top;">
                                    <asp:CheckBoxList ID="lstOfficeAccess" runat="server" RepeatColumns="4" Style="margin-top: -5px;" AutoPostBack="true" OnSelectedIndexChanged="lstOfficeAccess_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr><td>&nbsp;</td></tr>
                            <tr>
                                <td style="vertical-align: top;"><b>Volume Level:</b></td>
                                <td style="vertical-align: top;">
                                    <asp:DropDownList ID="lstVolumeLevel" runat="server" Style="margin-top: -5px;" AutoPostBack="true" OnSelectedIndexChanged="lstVolumeLevel_SelectedIndexChanged">
                                        <asp:ListItem Text="-- ALL --" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Low Volume" Value="Low"></asp:ListItem>
                                        <asp:ListItem Text="High Volume" Value="High"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>

                </ContentTemplate>
            </asp:UpdatePanel>
            <uc1:wucApplicationQueue ID="pnlReceived" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" />
            <br />
            <uc1:wucApplicationQueue ID="pnl3DEDecision" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" />
            <br />
            <uc1:wucApplicationQueue ID="pnlReceivedPD" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" />
            <br />
            <uc1:wucApplicationQueue ID="pnlPending" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" />
            <br />
            <uc1:wucApplicationQueue ID="pnlSubmittedToBank" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" />
            <br />
            <uc1:wucApplicationQueue ID="pnlBankRequested" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" />
            <br />
            <uc1:wucApplicationQueue ID="pnlInReview" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" />
            <br />
            <uc1:wucApplicationQueue ID="pnlApproved" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" />
            <br />
            <!--PXP-9308: By Sanidhya-->
            <uc1:wucApplicationQueue ID="pnlPendingRegistration" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" />
            <br />
            <uc1:wucApplicationQueue ID="pnlConditional" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" ConditionDate="true" />
            <br />
            <uc2:wucDocumentUploads ID="WucDocumentUploads1" runat="server" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" />
            <br />
            <uc1:wucApplicationQueue ID="pnlApprovalRequest" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" ApprovalRequested="true" />
            <br />
            <uc1:wucApplicationQueue ID="pnlWithdrawn" runat="server" Type="CU" PostBackURL="~/SecureHomeForms/frmQueueCU.aspx" />
            <br />

        </asp:Panel>
    </div>

</asp:Content>
