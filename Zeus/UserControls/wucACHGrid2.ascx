<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucACHGrid2" Codebehind="wucACHGrid2.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<table width="100%">
    <tr>
        <td class="lblLeft">
        </td>
        <td class="lblRight">
            <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label></td>
    </tr>
</table>
<asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                OnPageIndexChanging="grd_PageIndexChanging" DataKeyNames="TransID,Action" OnRowDataBound="grd_RowDataBound"
                DataSourceID="odsTransactions" OnRowCommand="grd_RowCommand" AllowSorting="True"
                OnSorting="grd_Sorting" ShowFooter="True" OnDataBinding="grd_DataBinding">
                <Columns>
                    <asp:TemplateField HeaderText="Action" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnk1" runat="server" Text="" CausesValidation="false" />
                            &nbsp;<asp:LinkButton ID="lnk2" runat="server" Text="" CausesValidation="false" />
                        </ItemTemplate>
                        <ItemStyle Width="100px" HorizontalAlign="Left" Wrap="False" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="TransID" HeaderText="PXP TransID" SortExpression="TransID">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Trans Date" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyyy hh:mm:ss tt}"
                        SortExpression="Trans Date">
                        <ItemStyle Width="75px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PaymentType" HeaderText="Type" SortExpression="PaymentType">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Masked Account Number" HeaderText="Acct. #" SortExpression="Masked Account Number">
                        <ItemStyle Width="40px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="TransRoute" HeaderText="Routing #" SortExpression="TransRoute">
                        <ItemStyle Width="55px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Account Name" HeaderText="Account Name" SortExpression="Account Name">
                        <ItemStyle Width="125px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:0.00}" SortExpression="Amount">
                        <ItemStyle Width="45px" HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="AddedBy" HeaderText="User Created" DataFormatString="{0:0.00}"
                        SortExpression="AddedBy">
                        <ItemStyle Width="65px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Ref ID" HeaderText="Ref No" SortExpression="Ref ID" ItemStyle-Width="35px" />
                    <asp:BoundField DataField="Description" HeaderText="Category" SortExpression="Description"
                        ItemStyle-Width="60px" />
                    <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" Visible="False" />
                    <asp:BoundField DataField="CustomerName" HeaderText="CustomerName" Visible="False" />
                    <asp:BoundField DataField="Origin" HeaderText="Type" Visible="False" />
                    <asp:BoundField DataField="Next Process Date" HeaderText="Next Process Date" Visible="False" />
                    <asp:BoundField DataField="BankAccountType" HeaderText="BankAccountType" Visible="False" />
                    <asp:BoundField DataField="PaymentType" HeaderText="PaymentType" Visible="False" />
                    <asp:BoundField DataField="Description" HeaderText="Description" Visible="False" />
                    <asp:BoundField DataField="Merchant ID" HeaderText="ZID" Visible="False" SortExpression="Merchant ID" />
                    <asp:BoundField DataField="Merchant Name" HeaderText="Merchant Name" Visible="False"
                        SortExpression="Merchant Name" />
                    <asp:BoundField DataField="CustomInfo1" HeaderText="Custom Info 1" Visible="False" />
                    <asp:BoundField DataField="CustomInfo2" HeaderText="Custom Info 2" Visible="False" />
                    <asp:BoundField DataField="CustomInfo3" HeaderText="Custom Info 3" Visible="False" />
                    <asp:BoundField DataField="BillingFirstName" HeaderText="Billing First Name" Visible="False"
                        SortExpression="BillingFirstName" />
                    <asp:BoundField DataField="BillingLastName" HeaderText="Billing Last Name" Visible="False"
                        SortExpression="BillingLastName" />
                    <asp:BoundField DataField="BillingAddress" HeaderText="Billing Address" Visible="False"
                        SortExpression="BillingAddress" />
                    <asp:BoundField DataField="BillingCity" HeaderText="Billing City" Visible="False"
                        SortExpression="BillingCity" />
                    <asp:BoundField DataField="BillingState" HeaderText="Billing State" Visible="False"
                        SortExpression="BillingState" />
                    <asp:BoundField DataField="BillingZip" HeaderText="Billing Zip" Visible="False" SortExpression="BillingZip" />
                    <asp:BoundField DataField="BillingCountry" HeaderText="Billing Country" Visible="False"
                        SortExpression="BillingCountry" />
                    <asp:BoundField DataField="BillingPhone" HeaderText="Billing Phone" Visible="False"
                        SortExpression="BillingPhone" />
                    <asp:BoundField DataField="BillingFax" HeaderText="Billing Fax" Visible="False" SortExpression="BillingFax" />
                    <asp:BoundField DataField="BillingEmail" HeaderText="Billing Email" Visible="False"
                        SortExpression="BillingEmail" />
                </Columns>
                <PagerStyle CssClass="pgr" />
                <AlternatingRowStyle CssClass="alt" />
                <FooterStyle CssClass="footer" />
                <PagerSettings Mode="NumericFirstLast" />
            </asp:GridView>
            <asp:ObjectDataSource ID="odsTransactions" runat="server" EnablePaging="True" MaximumRowsParameterName="PageSize"
                OnSelecting="odsTransactions_Selecting" StartRowIndexParameterName="CurrentPage"
                TypeName="DataMerchantAppPaging">
                <SelectParameters>
                    <asp:Parameter Name="prms" Type="Object" />
                    <asp:Parameter Name="PageSize" Type="Int32" />
                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                    <asp:Parameter Name="ControlID" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table width="100%">
        <tr>
            <td align="left">
                <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnExcel_Click" CausesValidation="false">
                    <span style="height: 25px; vertical-align: middle;">
                        <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                            Excel</span></asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton ID="btnPDF" runat="server" OnClick="btnPDF_Click" Visible="false" CausesValidation="false">
                    <span style="height: 25px; vertical-align: middle;">
                        <asp:Image ID="Image2" runat="server" SkinID="SavePDF" /></span><span style="margin-left: 5px;">Save
                            PDF</span></asp:LinkButton>
            </td>
            <td align="right">
                Export:&nbsp;</td>
            <td align="left">
                <asp:RadioButtonList ID="lstExportPageSize" runat="server" RepeatColumns="2">
                    <asp:ListItem Selected="true">Current Page</asp:ListItem>
                    <asp:ListItem>All Pages</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td align="right">
                Note: Export contains additional fields.</td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
    No data...
</asp:Panel>
