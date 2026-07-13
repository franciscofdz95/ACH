<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true" Inherits="frmMerchantStatements" Title="Statements" CodeBehind="frmMerchantStatements.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>


<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ Register TagPrefix="mps" TagName="Statemetns" Src="~/UserControls/Statements.ascx" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:Panel ID="pnlTools" runat="server">
        </asp:Panel>
        <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
        <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td style="width: 50%; vertical-align: top;">
                       <asp:Panel runat="server" ID="pnlStamtBank">
                        <fieldset style="height: 330px;">
                            <legend>Bank Statements</legend>
                            <asp:Panel runat="server" ID="pnlStmts" Height="300px" ScrollBars="vertical" Width="">
                                <mps:Statemetns ID="ctrlStatements" runat="server" />
                            </asp:Panel>
                        </fieldset>
                       </asp:Panel>
                    </td>
                    <td style="width: 50%; vertical-align: top;">
                        <asp:Panel runat="server" ID="pnlStamt" Visible="false">
                            <fieldset style="height: 330px;">
                                <legend>Paysafe Statements</legend>
                                <asp:Panel runat="server" ID="Panel1" Height="300px" ScrollBars="vertical" Width="">
                                    <asp:GridView ID="grdStatements" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
                                        ShowFooter="True" CssClass="mGrid" Width="96%" OnRowDataBound="grdStatements_RowDataBound">
                                        <FooterStyle CssClass="footer" HorizontalAlign="Right" />
                                        <PagerStyle CssClass="pgr" />
                                        <AlternatingRowStyle CssClass="alt" />
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
                                        <Columns>
                                            <asp:BoundField DataField="Year" HeaderText="Year" />
                                            <asp:BoundField DataField="MonthName" HeaderText="Month" />
                                            <asp:HyperLinkField DataNavigateUrlFields="BillDate" DataTextField="BillDate"
                                                Target="_blank" />
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </fieldset>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="BillingRDRPanel" runat="server" Height="100%" Width="100%">
            <table cellpadding="2" cellspacing="2" border="0" style="text-align: left" width="100%">
                <tr>
                    <td style="width: 50%; vertical-align: top;">
                        <fieldset>
                            <legend>Paysafe Billing Debits and Credits</legend>
                            <table width="100%">
                                <tr>
                                    <td valign="top">
                                        <div style="padding-left: 1px;" align="left">
                                            <table cellpadding="2" cellspacing="2" border="0" style="text-align: left">
                                                <tr>
                                                    <th class="lblEdit">Billing Adjustment Date From:</th>
                                                    <td>
                                                        <ig:WebDatePicker ID="BillingDateFrom" runat="server" ToolTip="From Date" Width="130px"
                                                            Nullable="False">
                                                        </ig:WebDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th class="lblEdit">Billing Adjustment Date To:</th>
                                                    <td>
                                                        <ig:WebDatePicker ID="BillingDateTo" runat="server" ToolTip="To Date" Width="130px"
                                                            Nullable="False">
                                                        </ig:WebDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th></th>
                                                    <td>
                                                        <asp:Button ID="btnBillingSearch" runat="server" Text="Search" OnClick="btnBillingSearch_Click" />&nbsp;
                                                            <asp:Button ID="btnBillingReset" runat="server" Text="Clear" CausesValidation="False" OnClick="btnBillingReset_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel runat="server" ID="Panel2" ScrollBars="vertical" Width="">
                                <fieldset>
                                    <table width="100%">

                                        <tr>
                                            <td class="lblRight">Total Records Found: 
                                <asp:Label ID="lblRecordCount" runat="server" Text="0"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="BillingRDRStatementGrid" runat="server"
                                                    AllowPaging="True"
                                                    OnRowDataBound="Billing_OnRowDataBoundStatement"
                                                    OnPageIndexChanging="Billing_OnPageIndexChangingStatement"                                                   
                                                    AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="billingLblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField> 
                                                        <asp:TemplateField  Visible ="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="billingLblInvoiceID" runat="server" Text='<%# Bind("InvoiceID") %>'></asp:Label> 
                                                            </ItemTemplate>
                                                        </asp:TemplateField> 
                                                        <asp:BoundField DataField="DateBillingAdjustment" HtmlEncode="False" HeaderText="Date of Billing Adjusment"
                                                            DataFormatString="{0:MM/dd/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="BillingPeriodFrom" HtmlEncode="False" HeaderText="Billing Period From"
                                                            DataFormatString="{0:MM/dd/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="BillingDateTo" HtmlEncode="False" HeaderText="Billing Period To"
                                                            DataFormatString="{0:MM/dd/yyyy}" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Product" HtmlEncode="False" HeaderText="Product"
                                                            SortExpression="Product" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Description" HtmlEncode="False" HeaderText="Description"
                                                            ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Volume" HtmlEncode="False" HeaderText="Volume"
                                                            DataFormatString="{0:D}" ItemStyle-HorizontalAlign="Center"
                                                            ControlStyle-CssClass="decValue" />
                                                        <asp:BoundField DataField="Rate" HtmlEncode="False" HeaderText="Rate"
                                                            DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Center"
                                                            ControlStyle-CssClass="decValue" />
                                                        <asp:BoundField DataField="Total" HtmlEncode="False" HeaderText="Total"
                                                            DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Center"
                                                            ControlStyle-CssClass="decValue" />
                                                        <asp:BoundField DataField="BillingMethod" HtmlEncode="False" HeaderText="Billing Method"
                                                            DataFormatString="{0}" ItemStyle-HorizontalAlign="Center"
                                                            ControlStyle-CssClass="strValue" />
                                                        <asp:BoundField DataField="DateCreated" HtmlEncode="False" HeaderText="Date Created"
                                                            DataFormatString="{0:MM/dd/yyyy hh:mm:ss tt}"   ItemStyle-HorizontalAlign="Center" />
                                                        <asp:TemplateField HeaderText="Insight Visible">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:CheckBox ID="billingChkStatus" runat="server" OnCheckedChanged="billing_ChangedStatus" AutoPostBack="true" />
                                                                </center>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Status" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="billingLblStatus" runat="server" Text='<%# Bind("InsightVisible") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        No Data Found.
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>* AR = Accounts Receivable
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Billing events occurring on the same day may be grouped together on your bank statement
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="bucketfooter">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left" style="width: 33%;">
                                                                <asp:LinkButton ID="btnExpExcelAccountsDay" runat="server" OnClick="btnBillingExpExcel_Click">
                                                                    <span style="height: 25px; vertical-align: middle;">
                                                                        <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span>
                                                                </asp:LinkButton>&nbsp;&nbsp;
                                                            &nbsp;&nbsp;
                                                            </td>
                                                            <td align="right">Export:&nbsp;
                                                            </td>
                                                            <td align="left">
                                                                <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                                                    <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                                                    <asp:ListItem Value="1">All Pages</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td style="width: 33%;" align="right">Page Size:
                                                    <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="BillingOnSelectedIndexChanged">
                                                        <asp:ListItem Selected="True">10</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>20</asp:ListItem>
                                                        <asp:ListItem>25</asp:ListItem>
                                                        <asp:ListItem>50</asp:ListItem>
                                                        <asp:ListItem>100</asp:ListItem>
                                                    </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </asp:Panel>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </asp:Panel>               
    </div>
</asp:Content>
