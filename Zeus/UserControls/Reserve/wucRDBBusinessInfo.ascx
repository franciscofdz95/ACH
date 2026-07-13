<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucRDBBusinessInfo.ascx.cs"
    Inherits="ZeusWeb.UserControls.Reserve.wucRDBBusinessInfo" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%--<%@ Register src="wucSummaryGrid.ascx" tagname="wucSummaryGrid" tagprefix="uc1" %>--%>

<style type="text/css">
    .rdbbusiness .lblRight
    {
        font-weight: bold;
        white-space: nowrap;
    }

    .rdbbusiness table
    {
        width: 100%;
    }

    .rdbbusiness td
    {
        vertical-align: top;
    }
</style>
<asp:Panel ID="pnlGeneralInfo" CssClass="rdbbusiness" runat="server">
    <table>
        <tr>
            <td class="lblRight">DBA:
            </td>
            <td>
                <asp:Label ID="BusinessDBAName" runat="server"></asp:Label>


            </td>
            <td class="lblRight">MLE:
            </td>
            <td>
                <asp:Label ID="BusinessLegalName" runat="server"></asp:Label>
            </td>
            <td class="lblRight">Status:
            </td>
            <td>
                <asp:Label ID="StatusUID" runat="server">
                </asp:Label>
            </td>
            <td class="lblRight">Bank:
            </td>
            <td>
                <asp:Label ID="MerchantAppTypeUID" runat="server">
                </asp:Label>
            </td>
            <td class="lblRight">ZID:
            </td>
            <td>
                <asp:Label ID="ID" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="lblRight">Front MID:
            </td>
            <td>
                <asp:Label ID="AuthPlatformMid" runat="server"></asp:Label>
            </td>
            <td class="lblRight">Front-End:
            </td>
            <td>
                <asp:Label ID="AuthPlatformUID" runat="server">
                </asp:Label>
            </td>

            <td class="lblRight">Approved Delays:
            </td>
            <td>
                <asp:Label ID="DelaysApproved" runat="server"></asp:Label>
            </td>
            <td class="lblRight">Approved Avg Tkt:</td>
            <td>
                <asp:Label ID="TinfoAverageVMCTicket" runat="server"></asp:Label>
            </td>
            <td class="lblRight">AID: </td>
            <td>
                <asp:Label ID="AchID" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="lblRight">Back MID:
            </td>
            <td>
                <asp:Label ID="SettlePlatformMid" runat="server"></asp:Label>
            </td>
            <td class="lblRight">Back-End:
            </td>
            <td>
                <asp:Label ID="SettlePlatformUID" runat="server">
                </asp:Label>
            </td>
            <td class="lblRight">Approved Vol: </td>
            <td>
                <asp:Label ID="TinfoAverageMonthlyVMCVolume" runat="server"></asp:Label>
            </td>
            <td class="lblRight">Approved High Tkt:</td>
            <td>
                <asp:Label ID="TinfoHighestTicketAmount" runat="server"></asp:Label>
            </td>
            <td class="lblRight">High Risk:</td>
            <td>
                <asp:CheckBox ID="HighRisk" Enabled="false" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="lblRight">Reserve %:
            </td>
            <td>
                <asp:Label ID="ReservePercent" runat="server">
                </asp:Label>
            </td>

            <td class="lblRight">Release Method:</td>
            <td>
                <asp:Label ID="ReleaseMethodUID" runat="server">
                </asp:Label>
            </td>

            <td class="lblRight">% Swiped:
            </td>
            <td>
                <asp:Label ID="TinfoStoreFrontSwipedPercent" runat="server"></asp:Label>
            </td>
            <td class="lblRight">Month End Approved:
            </td>
            <td>
                <asp:CheckBox ID="MonthendApproved" Enabled="false" runat="server" />
            </td>
            <td class="lblRight">&nbsp;</td>
            <td>&nbsp;</td>

        </tr>
    </table>
</asp:Panel>
<%--<uc1:wucSummaryGrid ID="wucSummaryGrid1" ShowFilter="false" ShowExport="false" runat="server" />--%>

<asp:GridView ID="gvBankBalance" runat="server" CssClass="mGrid" OnRowDataBound="gvBankBalance_RowDataBound" Width="324px" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
        <asp:BoundField DataField="ReserveType" HeaderText="Reserve Type" />
        <asp:BoundField DataField="Balance" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"
            HeaderText="Balance" />
    </Columns>
</asp:GridView>

