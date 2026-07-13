<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true" Inherits="frmRiskBatchDetails" Title="Batch Details" Codebehind="frmRiskBatchDetails.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucBusinessInfo_Risk.ascx" TagName="wucBusinessInfo_Risk"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/wucTransactionCCReadOnly.ascx" TagName="wucTransaction"
    TagPrefix="uc2" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="menufooter2">
        <b>Batch Details</b>
    </div>
    <uc1:wucBusinessInfo_Risk ID="WucBusinessInfo_Risk1" runat="server" />
    <fieldset>
        <legend>Batch Information</legend>
        <table>
            <tr>
                <td>
                    Sales Cnt:</td>
                <td>
                    <asp:TextBox ID="txtSalesCount" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
                <td>
                    Sales Amt:</td>
                <td>
                    <asp:TextBox ID="txtSalesAmount" runat="server" ReadOnly="True" Width="175px"></asp:TextBox>
                </td>
                <td>
                    Credit Cnt:</td>
                <td>
                    <asp:TextBox ID="txtCreditCount" runat="server" ReadOnly="True" Width="175px"></asp:TextBox>
                </td>
                <td>
                    Credit Amt:</td>
                <td>
                    <asp:TextBox ID="txtCreditAmount" runat="server" ReadOnly="True" Width="175px"></asp:TextBox>
                </td>
                <td>
                    MTD Volume</td>
                <td>
                    <asp:TextBox ID="txtMTDVolume" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Batch Status:</td>
                <td>
                    <asp:TextBox ID="BatchStatus" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
                <td>
                    Suspended:</td>
                <td>
                    <asp:TextBox ID="txtDateSuspended" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
                <td>
                    Held:</td>
                <td>
                    <asp:TextBox ID="txtDateHeld" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
                <td>
                    Released:</td>
                <td>
                    <asp:TextBox ID="txtDateReleased" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
                <td>
                    Voided:</td>
                <td>
                    <asp:TextBox ID="txtDateVoided" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Max Batch Amt:</td>
                <td>
                    <asp:TextBox ID="txtMaxDebit" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
                <td>
                    Min Batch Credit:</td>
                <td>
                    <asp:TextBox ID="txtMaxCredit" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
                <td>
                    Max Date:</td>
                <td>
                    <asp:TextBox ID="txtMaxDebitDate" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
                <td>
                    Min Date:</td>
                <td>
                    <asp:TextBox ID="txtMaxCreditDate" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
                <td>
                    First Batch:</td>
                <td>
                    <asp:TextBox ID="txtFirstBatchDate" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Last 30 days Violations:</td>
                <td>
                    <asp:TextBox ID="txtVoilations" runat="server" ReadOnly="True" Width="175px"></asp:TextBox></td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>Filters</legend>
        <table>
            <tr>
                <td>
                    Exceptions:</td>
                <td>
                    <asp:DropDownList ID="BatchExceptions" runat="server">
                    </asp:DropDownList></td>
                <td>
                    Status:</td>
                <td>
                    <asp:DropDownList ID="StatusID" runat="server">
                        <asp:ListItem Value="-1">All</asp:ListItem>
                        <asp:ListItem Value="0">Approved</asp:ListItem>
                        <asp:ListItem Value="19">Declined</asp:ListItem>
                        <asp:ListItem Value="6,7,8,9,16,21,26,86">Internal Declined</asp:ListItem>
                    </asp:DropDownList></td>
                <td>
                    Trans Type:
                </td>
                <td>
                    <asp:DropDownList ID="lstTransTypeID" runat="server">
                        <asp:ListItem Value="-1">All</asp:ListItem>
                        <asp:ListItem Value="312">Sale</asp:ListItem>
                        <asp:ListItem Value="310">Auth Only</asp:ListItem>
                        <asp:ListItem Value="622">Auth-STL</asp:ListItem>
                        <asp:ListItem Value="314">Rtrn</asp:ListItem>
                        <asp:ListItem Value="326">Rtrn-BL</asp:ListItem>
                    </asp:DropDownList></td>
                <td>
                    Current Batch Only:</td>
                <td>
                    <asp:CheckBox ID="CurrentBatchOnly" runat="server" Checked="True" /></td>
                <td>
                    <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                        AccessKey="h">
                        <Appearance>
                            <Image Url="~/Images/Check.png" />
                        </Appearance>
                    </igtxt:WebImageButton>
                </td>
                <td>
                    <igtxt:WebImageButton ID="btnClear" runat="server" Text="Clear" AccessKey="l" OnClick="btnClear_Click">
                        <Appearance>
                            <Image Url="~/Images/delete.png" />
                        </Appearance>
                    </igtxt:WebImageButton>
                </td>
            </tr>
        </table>
        <div style="text-align: center;">
            <br />
        </div>
    </fieldset>
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
                        <asp:UpdatePanel runat="server" ID="pnl1">
                            <ContentTemplate>
                                <asp:GridView ID="grd" runat="server" AutoGenerateColumns="false" CssClass="mGrid" DataSourceID="odsRisk"
                                    AllowSorting="true" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                                    OnSorting="grd_Sorting">
                                    <AlternatingRowStyle CssClass="alt" />
                                    <PagerStyle CssClass="pgr" />
                                    <FooterStyle CssClass="footer" HorizontalAlign="Right" />
                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                    <Columns>
                                        <asp:TemplateField SortExpression="TransID" HeaderText="TransID">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkTransID" Text='<%# Eval("TransID") %>' OnClick="lnkTransID_Click"
                                                    CausesValidation="false"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="true" DataField="TransDate"
                                            SortExpression="TransDate" HeaderText="Date">
                                            <ItemStyle Width="75px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TransAmount" DataFormatString="{0:0.00}" SortExpression="TransAmount"
                                            HeaderText="Amount">
                                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BillingName" SortExpression="BillingName" HeaderText="Name">
                                            <ItemStyle Width="230px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CardNumberMask" SortExpression="CardNumberMask" HeaderText="Card No">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StatusName" SortExpression="StatusName" HeaderText="Status">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VoidDate" SortExpression="VoidDate" DataFormatString="{0:MM/dd/yyyy hh:mm tt}"
                                            HeaderText="Date Voided"></asp:BoundField>
                                        <asp:BoundField DataField="TransTypeName" HeaderText="Trans Type">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EntryMode" SortExpression="EntryMode" HeaderText="Entry Mode">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RiskExceptions" SortExpression="RiskExceptions" HeaderText="Risk Exceptions">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ResponseAVS" SortExpression="ResponseAVS" HeaderText="AVS Response">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ResponseCVV2" SortExpression="ResponseCVV2" HeaderText="CVV2 Response">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DeclinedReason" SortExpression="DeclinedReason" HeaderText="Declined Reason">
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                  <asp:ObjectDataSource ID="odsRisk" runat="server" SelectMethod="GetRiskBatchDetailsPaging"
                                    TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                    SelectCountMethod="GetRiskBatchDetailsPagingRowCount" StartRowIndexParameterName="CurrentPage"
                                    OldValuesParameterFormatString="original_{0}" OnSelecting="odsRisk_Selecting">
                                    <SelectParameters>
                                        <asp:Parameter Name="prms" Type="Object" />
                                        <asp:Parameter Name="PageSize" Type="Int32" />
                                        <asp:Parameter Name="CurrentPage" Type="Int32" />
                                        <asp:Parameter Name="ControlID" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
    </fieldset>
</asp:Content>
