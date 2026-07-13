<%@ Page Language="C#" MasterPageFile="~/MasterPageAgent.master" AutoEventWireup="true" Inherits="frmAgentHistory" Title="Agent History" CodeBehind="frmAgentHistory.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:Panel ID="pnlCCHistory" runat="server">
            <fieldset>
                <legend>Agent Status History</legend>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblStatus" Text="no data.." Visible="false"></asp:Label>
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="true" Font-Names="Verdana"
                            Font-Size="X-Small" CssClass="mGrid" OnRowDataBound="grd_RowDataBound">
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </asp:Panel>
        <asp:Panel ID="Panel2" runat="server" Width="100%">
            <fieldset>
                <legend>Change History</legend>
                <table width="100%">
                    <tr>
                        <td>
                            <b>Select Field:</b><asp:DropDownList ID="ddlChangeType" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlChangeType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView CssClass="mGrid" ID="grdChange" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="false" OnRowDataBound="grdChange_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Field Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblValue" runat="server" Text='<%# Bind("NewValue") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNameHeader" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ChangedDate" HeaderText="Changed Date" />
                                    <asp:BoundField DataField="ChangedBy" HeaderText="Changed By" />
                                </Columns>
                                <EmptyDataTemplate>
                                    No changes for selected field.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
        <asp:Panel ID="pnlPaymentSettingHistory" runat="server" Width="100%">
            <fieldset>
                <legend>Payment Setting History</legend>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:GridView
                                ID="grvPaymentSettingChange"
                                runat="server"
                                OnRowDataBound="grvPaymentSettingChange_RowDataBound"
                                OnPageIndexChanging="grvPaymentSettingChange_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Field Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblValue" runat="server" Text='<%# Bind("NewValue") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNameHeader" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ChangedDate" HeaderText="Changed Date" />
                                    <asp:BoundField DataField="ChangedBy" HeaderText="Changed By" />
                                </Columns>
                                <EmptyDataTemplate>
                                    No changes for selected field.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
        <!--DM-1240 start-->
        <asp:Panel ID="pnlMarchantSplitsHistory" runat="server" Width="100%">
            <fieldset>
                <legend>Merchant Splits History</legend>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:GridView
                                ID="grvMerchantSplitsHistory"
                                runat="server"
                                OnPageIndexChanging="grvMerchantSplitsHistory_PageIndexChanging">
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
        <!--DM-1240 end-->
        <%--DM-1373--%>
        <asp:Panel ID="Panel1" runat="server" Width="100%">
            <fieldset>
                <legend>Agent Fees</legend>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:GridView CssClass="mGrid" ID="grdAgentResidualReportItemsHistory" runat="server" AutoGenerateColumns="False"
                                ShowHeaderWhenEmpty="false"
                                OnPageIndexChanging="grdAgentResidualReportItemsHistory_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="FieldName" HeaderText="Field Name" />
                                    <asp:BoundField DataField="OldValue" HeaderText="Old Value" />
                                    <asp:BoundField DataField="NewValue" HeaderText="New Value" />
                                    <asp:BoundField DataField="DateCreated" HeaderText="Changed Date" />
                                    <asp:BoundField DataField="UserCreated" HeaderText="Changed By" />
                                </Columns>
                                <EmptyDataTemplate>
                                    No changes for selected field.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
        <%--DM-1373--%>
    </div>
</asp:Content>
