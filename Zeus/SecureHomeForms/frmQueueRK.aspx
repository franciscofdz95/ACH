<%@ Page Language="C#" MasterPageFile="~/MasterPageHome.master" AutoEventWireup="true" Inherits="frmQueueRK" Title="Risk Queue" CodeBehind="frmQueueRK.aspx.cs" %>

<%@ Register Src="../UserControls/wucApplicationQueue.ascx" TagName="wucApplicationQueue"
    TagPrefix="uc1" %>


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
            <uc1:wucApplicationQueue ID="pnlAB2Risk" runat="server" />
            <br />
            <uc1:wucApplicationQueue ID="pnlReceived" runat="server" />
            <br />
            <uc1:wucApplicationQueue ID="pnlInReview" runat="server" />
            <br />
            <uc1:wucApplicationQueue ID="pnlSentToDeployment" runat="server" />
            <br />

        </asp:Panel>
    </div>

</asp:Content>

