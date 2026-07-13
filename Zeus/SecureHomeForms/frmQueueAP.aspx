<%@ Page Language="C#" MasterPageFile="~/MasterPageHome.master" AutoEventWireup="true"
    Inherits="frmQueueAP" Title="Sales Support Queue" CodeBehind="frmQueueAP.aspx.cs" %>

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
            <uc1:wucApplicationQueue ID="pnlReceived" runat="server" Type="SS" PostBackUrl="~/SecureHomeForms/frmQueueAP.aspx" />
            <br />
            <uc1:wucApplicationQueue ID="pnlPending" runat="server" Type="SS" PostBackUrl="~/SecureHomeForms/frmQueueAP.aspx" />
            <br />
            <%--code added for XP-10574[Add new application status 'SS-QA' and 'SS-QA' queue on Sales Support dash board] by koshlendra start--%>
            <uc1:wucApplicationQueue ID="pnlQA" runat="server" Type="SS" PostBackUrl="~/SecureHomeForms/frmQueueAP.aspx" />
            <br />
             <%--code added for PXP-10574[Add new application status 'SS-QA' and 'SS-QA' queue on Sales Support dash board] by koshlendra End--%>
              <%--code added for PXP-10755[Add new queue as Draft below App-Incompelet]by koshlendra start--%>
            <uc1:wucApplicationQueue ID="pnlDraft" runat="server" Type="SS" PostBackUrl="~/SecureHomeForms/frmQueueAP.aspx" />
            <br />
             <%--code added for PXP-10755[Add new queue as Draft below App-Incompelet]by koshlendra end--%>
             <uc1:wucApplicationQueue ID="pnlWithdrawn" runat="server" Type="SS" PostBackUrl="~/SecureHomeForms/frmQueueAP.aspx" />
            <br />
        </asp:Panel>

    </div>

</asp:Content>
