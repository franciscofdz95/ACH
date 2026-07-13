<%@ Page Language="C#" MasterPageFile="~/MasterPageHome.master" AutoEventWireup="true"
    Inherits="frmQueueDE" Title="Data Entry Queue" CodeBehind="frmQueueDE.aspx.cs" %>

<%@ Register Src="../UserControls/wucApplicationQueue.ascx" TagName="wucApplicationQueue"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageHome.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="contentpage">
        <asp:Panel ID="Panel1" runat="server" Height="" Width="">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <fieldset>
                        <table>
                            <tr>
                                <td style="vertical-align: top;">Office:</td>
                                <td style="vertical-align: top;">
                                    <asp:CheckBoxList ID="lstOfficeAccess" runat="server" RepeatColumns="4" Style="margin-top: -5px;" AutoPostBack="true" OnSelectedIndexChanged="lstOfficeAccess_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
            <uc1:wucApplicationQueue ID="pnlReceived" Type="DE" PostBackURL="~/SecureHomeForms/frmQueueDE.aspx" runat="server" />
            <br />
            <uc1:wucApplicationQueue ID="pnlReview" Type="DE"  PostBackURL="~/SecureHomeForms/frmQueueDE.aspx" runat="server" />
            <br />
            <uc1:wucApplicationQueue ID="pnlQA" Type="DE" PostBackURL="~/SecureHomeForms/frmQueueDE.aspx" runat="server" />
            <br />
        </asp:Panel>
    </div>

</asp:Content>
