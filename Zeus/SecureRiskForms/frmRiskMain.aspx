<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true" Inherits="frmRiskMain" Title="Risk" CodeBehind="frmRiskMain.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <fieldset>
        <legend>Batch Summary</legend>
        <br />
        <asp:Label ID="noRecords" Text="No Data.." runat="server"></asp:Label>
        <asp:Panel runat="server" ID="pnl" Width="100%">
            <table width="100%">
                <tr>
                    <td class="lblLeft">Page Size:
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
                        <asp:Label ID="lblRecordCount" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                            ShowFooter="true" AllowSorting="true" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                            OnSorting="grd_Sorting" OnRowDataBound="grd_RowDataBound" DataSourceID="odsRisk">
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerStyle CssClass="pgr" />
                            <FooterStyle CssClass="footer" HorizontalAlign="right" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
                            <Columns>
                                <asp:BoundField DataField="MID" SortExpression="MID" ItemStyle-Width="105px" HeaderText="MID"></asp:BoundField>
                                <asp:BoundField DataField="ZID" SortExpression="ZID" ItemStyle-Width="50px" HeaderText="ZID"></asp:BoundField>
                                <asp:BoundField DataField="DBA" SortExpression="DBA" ItemStyle-Width="230px" HeaderText="DBA Name"></asp:BoundField>
                                <asp:BoundField DataField="Legal" SortExpression="Legal" ItemStyle-Width="230px"
                                    HeaderText="MLE"></asp:BoundField>
                                <asp:BoundField DataField="SalesCount" HeaderText="Sales Count">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="SalesAmount" DataFormatString="{0:0.00}"
                                    HeaderText="Sales Amount">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CreditCount" HeaderText="Credit Count">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CreditAmount" DataFormatString="{0:0.00}"
                                    HeaderText="Credit Amount">
                                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="MerchantAppUID" Visible="false"
                                    HeaderText="MerchantAppUID"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsRisk" runat="server" SelectMethod="GetRiskBatchSummaryPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetRiskBatchSummaryPagingRowCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsRisk_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                                <asp:Parameter Name="ControlID" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
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
                                                <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                    Excel</span>
                                        </asp:LinkButton>&nbsp;&nbsp;
                                        <asp:LinkButton ID="btnPDF" runat="server" OnClick="btnExportPDF_Click">
                                            <span style="height: 25px; vertical-align: middle;">
                                                <asp:Image ID="Image1" runat="server" SkinID="SavePDF" /></span><span style="margin-left: 5px;">Save
                                                    PDF</span>
                                        </asp:LinkButton>
                                    </td>
                                    <td align="right">Export:&nbsp;</td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                            <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                            <asp:ListItem Value="1">All Pages</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td align="right" style="width: 33%;"></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </fieldset>
</asp:Content>
