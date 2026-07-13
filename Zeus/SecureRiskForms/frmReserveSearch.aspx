<%@ Page Title="Manage Reserves" Language="C#" MasterPageFile="~/MasterPageRisk.master"
    AutoEventWireup="True" CodeBehind="frmReserveSearch.aspx.cs" Inherits="ZeusWeb.frmReserveSearch" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/Reserve/wucBalanceGrid.ascx" TagName="wucBalanceGrid"
    TagPrefix="uc9" %>
<%@ Register Src="../UserControls/Reserve/wucSummaryGrid.ascx" TagName="wucSummaryGrid"
    TagPrefix="uc7" %>
<%@ Register Src="../UserControls/Reserve/wucStatementGrid.ascx" TagName="wucStatementGrid"
    TagPrefix="uc8" %>
<%@ Register Src="~/UserControls/Reserve/wucReserveGrid.ascx" TagName="wucReserveGrid"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/Reserve/wucManualDialog.ascx" TagName="wucManualDialog"
    TagPrefix="uc10" %>
<%@ Register Src="../UserControls/Reserve/wucDivertDetailGrid.ascx" TagName="wucDivertDetailGrid" TagPrefix="uc6" %>
<%@ Register Src="../UserControls/Reserve/wucRejectGrid.ascx" TagName="wucRejectGrid" TagPrefix="uc12" %>

<%@ Register Src="~/UserControls/Reserve/wucReleaseDialog.ascx" TagName="wucReleaseDialog"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControls/Reserve/wucReleaseGrid.ascx" TagName="wucReleaseGrid"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControls/Reserve/wucTransferDialog.ascx" TagName="wucTransferDialog"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/Reserve/wucRDBBusinessInfo.ascx" TagName="wucRDBBusinessInfo"
    TagPrefix="uc3" %>
<%@ Register Src="../UserControls/Reserve/wucReserveBatchDetailsDialog.ascx" TagName="wucReserveBatchDetailsDialog" TagPrefix="uc11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <style type="text/css">
        .simplepadding
        {
            padding: 10px;
        }
    </style>
    <div id="container" style="width: 1231px">
        <fieldset style="width: 100%;">
            <legend>Search Reserves</legend>
            <asp:Panel runat="server" ID="pnlSearch">
                <table>
                    <tr>
                        <td>ZID
                        </td>
                        <td>
                            <asp:TextBox ID="MerchantID" runat="server"></asp:TextBox>
                        </td>
                        <td>MID
                        </td>
                        <td>
                            <asp:TextBox ID="SettlePlatformUID" runat="server"></asp:TextBox>
                        </td>
                        <td>DBA
                        </td>
                        <td>
                            <asp:TextBox ID="BusinessDBAName" runat="server"></asp:TextBox>
                        </td>
                        <td>Legal
                        </td>
                        <td>
                            <asp:TextBox ID="BusinessLegalName" runat="server"></asp:TextBox>
                        </td>
                        <%-- <td>
                    PartnerID
                </td>
                <td>
                    <asp:TextBox ID="PartnerID" runat="server"></asp:TextBox>
                </td>--%>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <hr />
            <uc9:wucBalanceGrid ID="wucBalanceGrid1" runat="server" />

        </fieldset>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlMerchant" Visible="false">
                    <br />
                    <fieldset>
                        <legend>Business Info</legend>
                        <uc3:wucRDBBusinessInfo ID="wucRDBBusinessInfo1" runat="server" />
                    </fieldset>

                    <asp:Panel runat="server" ID="pnlRDBButtons" style="padding:10px 0px;">
                        <asp:Button ID="btnAddManual" runat="server" OnClick="btnAddManual_Click" Text="Add Manual Reserve" />
                        <asp:Button ID="btnAddRelease" runat="server" OnClick="btnAddRelease_Click" Text="Release Funds" />
                        <asp:Button ID="btnDivertToReserve" runat="server" OnClick="btnDRTransfer_Click" Enabled="false"
                            Text="Transfer from Divert to Reserve" />
                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
                    </asp:Panel>

                    <asp:Panel runat="server" HorizontalAlign="Right" ID="pnlRHAM">
                        <asp:CheckBox runat="server" ID="cbRHAM" AutoPostBack="true" Checked="true" Text="Include Reserves Held at Paysafe" OnCheckedChanged="cbRHAM_CheckedChanged" />
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnlOmaha" CssClass="errorlist" Visible="false" style="margin:10px 0px;">

                        <ul>
                            <li>The ReserveDB system is only compatible with Omaha merchants.</li>
                        </ul>

                    </asp:Panel>

                    <ig:WebTab ID="tabReport" runat="server" Width="1231" SelectedIndex="0">
                        <Tabs>
                            <ig:ContentTabItem runat="server" Text="Summary">
                                <Template>
                                    <div class="simplepadding">
                                        <uc7:wucSummaryGrid ID="wucSummaryGrid1" runat="server" />
                                    </div>
                                </Template>
                            </ig:ContentTabItem>
                            <ig:ContentTabItem runat="server" Text="Statement">
                                <Template>
                                    <div class="simplepadding">
                                        <uc8:wucStatementGrid ID="wucStatementGrid1" runat="server" />
                                    </div>
                                </Template>
                            </ig:ContentTabItem>

                            <ig:ContentTabItem runat="server" Text="Reserve">
                                <Template>
                                    <div class="simplepadding">
                                        <uc2:wucReserveGrid ID="wucReserveGrid1" ViewPendingRecords="false" runat="server" />
                                    </div>
                                </Template>
                            </ig:ContentTabItem>

                            <ig:ContentTabItem runat="server" Text="Divert">
                                <Template>
                                    <div class="simplepadding">
                                        <%-- <uc14:wucDivertGrid ID="wucDivertGrid1" runat="server" />--%>

                                        <uc6:wucDivertDetailGrid ID="wucDivertDetailGrid1" ViewPendingRecords="false" runat="server" />
                                    </div>
                                </Template>
                            </ig:ContentTabItem>

                            <ig:ContentTabItem runat="server" Text="Rejects">
                                <Template>
                                    <div class="simplepadding">
                                        <uc12:wucRejectGrid ID="wucRejectGrid1" runat="server" />
                                    </div>
                                </Template>
                            </ig:ContentTabItem>

                            <ig:ContentTabItem runat="server" Text="Releases">
                                <Template>
                                    <div class="simplepadding">
                                        <uc4:wucReleaseGrid ID="wucReleaseGrid1" ViewPendingRecords="false" runat="server" />
                                    </div>
                                </Template>
                            </ig:ContentTabItem>

                        </Tabs>
                    </ig:WebTab>

                    <uc10:wucManualDialog ID="wucManualDialog1" runat="server" />
                    <uc5:wucReleaseDialog ID="wucReleaseDialog1" runat="server" />
                    <%--<uc6:wucDivertDialog ID="wucDivertDialog1" runat="server" />--%>
                    <uc1:wucTransferDialog ID="wucTransferDialog1" runat="server" />
                    <uc11:wucReserveBatchDetailsDialog ID="wucReserveBatchDetailsDialog1" runat="server" />




                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
