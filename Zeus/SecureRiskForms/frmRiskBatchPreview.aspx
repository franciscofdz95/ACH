<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true"
    Inherits="frmRiskBatchPreview" Title="Batch Preview" CodeBehind="frmRiskBatchPreview.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <br />
        <asp:Panel runat="server" ID="pnl" Width="100%">
            <table width="100%">
                <tr>
                    <td class="lblLeft">
                        Page Size:
                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem Selected="True">25</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>100</asp:ListItem>
                            <asp:ListItem>250</asp:ListItem>
                            <asp:ListItem>500</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">
                        <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                            ShowFooter="true" AllowSorting="true" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                            OnSorting="grd_Sorting" OnRowDataBound="grd_RowDataBound" OnRowCommand="grd_RowCommand"
                            DataKeyNames="MerchantAppUID">
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerStyle CssClass="pgr" />
                            <FooterStyle CssClass="footer" HorizontalAlign="right" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
                            <Columns>
                                <asp:TemplateField SortExpression="MID" ItemStyle-Width="105px" HeaderText="MID">
                                    <ItemTemplate>
                                        <asp:HyperLink NavigateUrl='<%#  "~/SecureRiskForms/frmRiskBatchPreviewDetail.aspx?MerchantAppUID=" + Eval("MerchantAppUID")  %>'
                                            runat="server" ID="hypZID" Text='<%# Eval("MID") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ZID" SortExpression="ZID" ItemStyle-Width="50px" HeaderText="ZID">
                                </asp:BoundField>
                                <asp:BoundField DataField="DBA" SortExpression="DBA" ItemStyle-Width="230px" HeaderText="DBA Name">
                                </asp:BoundField>
                                <asp:BoundField DataField="LegalName" SortExpression="LegalName" ItemStyle-Width="230px"
                                    HeaderText="MLE"></asp:BoundField>
                                <asp:BoundField DataField="SalesCount" SortExpression="SalesCount" HeaderText="Sales Count">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesAmount" DataFormatString="{0:0.00}" SortExpression="SalesAmount"
                                    HeaderText="Sales Amount">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CreditCount" SortExpression="CreditCount" HeaderText="Credit Count">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CreditAmount" DataFormatString="{0:0.00}" SortExpression="CreditAmount"
                                    HeaderText="Credit Amount">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="MerchantAppUID" Visible="false" SortExpression="MerchantAppUID"
                                    HeaderText="MerchantAppUID"></asp:BoundField>
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
                                        Export:&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                            <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                            <asp:ListItem Value="1">All Pages</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="right" style="width: 33%;">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="noRecords" Text="No Data.." runat="server"></asp:Label>
    </fieldset>
    <br />
</asp:Content>
