<%@ Page Language="C#" MasterPageFile="~/MasterPageHome.master" AutoEventWireup="true" Inherits="frmQueueAR" Title="Sales Support Queue" CodeBehind="frmQueueAR.aspx.cs" %>

<%@ Register Src="../UserControls/wucApplicationQueue.ascx" TagName="wucApplicationQueue"
    TagPrefix="uc1" %>

<%@ Register Src="../UserControls/SidePanel/wucLegend.ascx" TagName="wucLegend" TagPrefix="uc2" %>

<asp:Content ID="cphLeft" ContentPlaceHolderID="cphLeftBottom" runat="server">
    <uc2:wucLegend ID="wucLegend1" runat="server" />
</asp:Content>

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
            <uc1:wucApplicationQueue ID="pnlReceived" runat="server" />
            <br />
            <uc1:wucApplicationQueue ID="pnlPendingInternal" runat="server" />
            <br />
            <uc1:wucApplicationQueue ID="pnlSent2DataEntry" runat="server" />
            <br />
        </asp:Panel>
    </div>

</asp:Content>

