<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true" Inherits="frmRiskBatchPreviewDetail"
    Title="Batch Preview Details" Codebehind="frmRiskBatchPreviewDetail.aspx.cs" %>
<%@ Register Src="~/UserControls/wucTransactionCCReadOnly.ascx" TagName="wucTransaction"
    TagPrefix="uc2" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <div id="contentpage">
        <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
        <fieldset>
            <br />
            <asp:Label ID="noRecords" Text="No Data.." runat="server"></asp:Label>
            <asp:Panel runat="server" ID="pnl" Width="100%">
                <table width="100%">
                    <tr>
                        <td class="lblLeft">
                            Page Size:
                            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                                <asp:ListItem Selected="True">250</asp:ListItem>
                                <asp:ListItem>500</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="lblRight">
                            <asp:Label ID="lblRecordCount" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                                AllowSorting="true" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                                OnSorting="grd_Sorting">
                                <AlternatingRowStyle CssClass="alt" />
                                <PagerStyle CssClass="pgr" />
                                <FooterStyle CssClass="footer" HorizontalAlign="right" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                <Columns>
                                    <asp:TemplateField SortExpression="TransID" HeaderText="TransID">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkTransID" Text='<%# Eval("TransID") %>' OnClick="lnkTransID_Click"
                                                CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataFormatString="{0:MM/dd/yyyy hh:mm tt}" DataField="TransDate"
                                        SortExpression="TransDate" ItemStyle-Width="75px" HeaderText="Date"></asp:BoundField>
                                    <asp:BoundField DataField="BillingName" SortExpression="BillingName" ItemStyle-Width="230px"
                                        HeaderText="Name"></asp:BoundField>
                                    <asp:BoundField DataField="CardNumberMask" SortExpression="CardNumberMask" HeaderText="Card No">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TransAmount" DataFormatString="{0:0.00}" SortExpression="TransAmount"
                                        HeaderText="Amount">
                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="Status">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CardType" SortExpression="CardType" ItemStyle-Width="50px"
                                        HeaderText="Card"></asp:BoundField>
                                    <asp:BoundField DataField="AuthCode" SortExpression="AuthCode" ItemStyle-Width="50px"
                                        HeaderText="Auth Code"></asp:BoundField>
                                    <asp:BoundField DataField="SettleRequestDateTime" SortExpression="SettleRequestDateTime"
                                        ItemStyle-Width="80px" HeaderText="Request Date" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="bucketfooter">
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 33%;">
                                            <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                <span style="height: 25px; vertical-align: middle;">
                                                    <asp:Image ID="Image3" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                        Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnExportPDF_Click">
                                                <span style="height: 25px; vertical-align: middle;">
                                                    <asp:Image ID="Image4" runat="server" SkinID="SavePDF" /></span><span style="margin-left: 5px;">Save
                                                        PDF</span></asp:LinkButton>
                                        </td>
                                        <td align="right">
                                            Export:&nbsp;</td>
                                        <td align="left">
                                            <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                                <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                                <asp:ListItem Value="1">All Pages</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td align="right" style="width: 33%;">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="600px" Width="800px"
                Modal="True" InitialLocation="Centered" WindowState="Hidden">
                <ContentPane>
                    <Template>
                        <uc2:wucTransaction runat="server" ID="pnlTransaction" />
                    </Template>
                </ContentPane>
                <Header CaptionText="Transaction Details">
                </Header>
            </ig:WebDialogWindow>
        </fieldset>
    </div>
</asp:Content>
