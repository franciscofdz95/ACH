<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucProductSubscription.ascx.cs"
    Inherits="wucProductSubscription" %>
<%@ Register Src="~/UserControls/wuConfirmDialog.ascx" TagName="wuConfirm" TagPrefix="uc3" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="PaymentXP.WebControls" Namespace="PaymentXP.WebControls.Editor" TagPrefix="uc4" %>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="ControlPanel" runat="server">
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%; padding: 0px;">
                        <ig:WebTab runat="server" ID="TabControl" Enabled="true" Width="970px">
                            <PostBackOptions EnableAjax="true" EnableAsyncUpdateAllTabs="true" EnableLoadOnDemand="false" />
                            <Tabs>
                                <ig:ContentTabItem Text="Alternative Payments" Key="tabAltPayment" EnableDynamicUpdatePanel="False">
                                    <Template>
                                        <asp:Repeater ID="AlternatePaymentGrid" runat="server" OnItemDataBound="ProductGrid_ItemDataBound" OnItemCommand="ProductGrid_ItemCommand">
                                            <HeaderTemplate>
                                                <table style="width: 100%;">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="width: 100%">&nbsp;<asp:HiddenField ID="ProductKey" Value='<%#Eval("ProductID") %>' runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; border-bottom: 1px solid black" class="title">
                                                        <%#Eval("ProductName")%><asp:Label runat="server" ID="lblProductMessage" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" class="ProductDescription">
                                                        <span class="prodDesc">
                                                            <%#Eval("ProductDescription")%></span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%">
                                                        <asp:GridView ID="ProductFeeGrid" runat="server" CellPadding="3" GridLines="Both" ShowFooter="false"
                                                            CssClass="mGrid" AutoGenerateColumns="false" OnRowDataBound="ProductFeeGrid_ItemDataBound">
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="FeeID" Visible="false" />
                                                                <asp:BoundField DataField="Name" HeaderText="Fee" />
                                                                <asp:BoundField DataField="MerchantCost" HeaderText="Merchant Cost" DataFormatString="{0:0.00}"
                                                                    ItemStyle-Width="120" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; border-bottom: 1px solid black; text-align: right;" class="ProductName">
                                                        <asp:Button Text="Subscribe" Width="120" ID="ManageSubscription" runat="server" CommandName="Save" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </Template>
                                </ig:ContentTabItem>
                                <ig:ContentTabItem Text="Deployment" Key="tabDeployment" EnableDynamicUpdatePanel="False">
                                    <Template>
                                        <asp:Repeater ID="DeploymentGrid" runat="server" OnItemDataBound="ProductGrid_ItemDataBound" OnItemCommand="ProductGrid_ItemCommand">
                                            <HeaderTemplate>
                                                <table style="width: 100%;">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="width: 100%">&nbsp;<asp:HiddenField ID="ProductKey" Value='<%#Eval("ProductID") %>' runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; border-bottom: 1px solid black" class="title">
                                                        <%#Eval("ProductName")%><asp:Label runat="server" ID="lblProductMessage" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" class="ProductDescription">
                                                        <span class="prodDesc">
                                                            <%#Eval("ProductDescription")%></span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%">
                                                        <asp:GridView ID="ProductFeeGrid" runat="server" CellPadding="3" GridLines="Both" ShowFooter="false"
                                                            CssClass="mGrid" AutoGenerateColumns="false" OnRowDataBound="ProductFeeGrid_ItemDataBound">
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="FeeID" Visible="false" />
                                                                <asp:BoundField DataField="Name" HeaderText="Fee" />
                                                                <asp:BoundField DataField="MerchantCost" HeaderText="Merchant Cost" DataFormatString="{0:0.00}"
                                                                    ItemStyle-Width="120" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; border-bottom: 1px solid black; text-align: right;" class="ProductName">
                                                        <asp:Button Text="Subscribe" Width="120" ID="ManageSubscription" runat="server" CommandName="Save" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </Template>
                                </ig:ContentTabItem>
                                <ig:ContentTabItem Text="Payment Acceptance" Key="tabPaymentAcceptance" EnableDynamicUpdatePanel="False">
                                    <Template>
                                        <asp:Repeater ID="PaymentAcceptanceGrid" runat="server" OnItemDataBound="ProductGrid_ItemDataBound" OnItemCommand="ProductGrid_ItemCommand">
                                            <HeaderTemplate>
                                                <table style="width: 100%;">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="width: 100%">&nbsp;<asp:HiddenField ID="ProductKey" Value='<%#Eval("ProductID") %>' runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; border-bottom: 1px solid black" class="title">
                                                        <%#Eval("ProductName")%><asp:Label runat="server" ID="lblProductMessage" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" class="ProductDescription">
                                                        <span class="prodDesc">
                                                            <%#Eval("ProductDescription")%></span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%">
                                                        <asp:GridView ID="ProductFeeGrid" runat="server" CellPadding="3" GridLines="Both" ShowFooter="false"
                                                            CssClass="mGrid" AutoGenerateColumns="false" OnRowDataBound="ProductFeeGrid_ItemDataBound">
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="FeeID" Visible="false" />
                                                                <asp:BoundField DataField="Name" HeaderText="Fee" />
                                                                <asp:BoundField DataField="MerchantCost" HeaderText="Merchant Cost" DataFormatString="{0:0.00}"
                                                                    ItemStyle-Width="120" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; border-bottom: 1px solid black; text-align: right;" class="ProductName">
                                                        <asp:Button Text="Subscribe" Width="120" ID="ManageSubscription" runat="server" CommandName="Save" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </Template>
                                </ig:ContentTabItem>
                                <ig:ContentTabItem Text="Risk Management" Key="tabRiskManagement" EnableDynamicUpdatePanel="False">
                                    <Template>
                                        <asp:Repeater ID="RiskManagementGrid" runat="server" OnItemDataBound="ProductGrid_ItemDataBound" OnItemCommand="ProductGrid_ItemCommand">
                                            <HeaderTemplate>
                                                <table style="width: 100%;">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="width: 100%">&nbsp;<asp:HiddenField ID="ProductKey" Value='<%#Eval("ProductID") %>' runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; border-bottom: 1px solid black" class="title">
                                                        <%#Eval("ProductName")%><asp:Label runat="server" ID="lblProductMessage" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" class="ProductDescription">
                                                        <span class="prodDesc">
                                                            <%#Eval("ProductDescription")%></span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%">
                                                        <asp:GridView ID="ProductFeeGrid" runat="server" CellPadding="3" GridLines="Both" ShowFooter="false"
                                                            CssClass="mGrid" AutoGenerateColumns="false" OnRowDataBound="ProductFeeGrid_ItemDataBound">
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="FeeID" Visible="false" />
                                                                <asp:BoundField DataField="Name" HeaderText="Fee" />
                                                                <asp:BoundField DataField="MerchantCost" HeaderText="Merchant Cost" DataFormatString="{0:0.00}"
                                                                    ItemStyle-Width="120" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; border-bottom: 1px solid black; text-align: right;" class="ProductName">
                                                        <asp:Button Text="Subscribe" Width="120" ID="ManageSubscription" runat="server" CommandName="Save" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </Template>
                                </ig:ContentTabItem>

                                <ig:ContentTabItem Text="Rapid Dispute Resolution" Key="tabRDR" EnableDynamicUpdatePanel="False">
                                    <Template>
                                        <asp:Repeater ID="rdrGrid" runat="server" OnItemDataBound="ProductGrid_ItemDataBound" OnItemCommand="ProductGrid_ItemCommand">
                                            <HeaderTemplate>
                                                <table style="width: 100%;">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="width: 100%">&nbsp;<asp:HiddenField ID="ProductKey" Value='<%#Eval("ProductID") %>' runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; border-bottom: 1px solid black" class="title">
                                                        <%#Eval("ProductName")%><asp:Label runat="server" ID="lblProductMessage" EnableViewState="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" class="ProductDescription">
                                                        <span class="prodDesc">
                                                            <%#Eval("ProductDescription")%></span>&nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%">
                                                        <asp:GridView ID="ProductFeeGrid" runat="server" CellPadding="3" GridLines="Both" ShowFooter="false"
                                                            CssClass="mGrid" AutoGenerateColumns="false" OnRowDataBound="ProductFeeGrid_ItemDataBound">
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <Columns>
                                                                <asp:BoundField DataField="FeeID" Visible="false" />
                                                                <asp:BoundField DataField="Name" HeaderText="Fee" />
                                                                <asp:BoundField DataField="MerchantCost" HeaderText="Merchant Cost" DataFormatString="{0:0.00}"
                                                                    ItemStyle-Width="120" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%; border-bottom: 1px solid black; text-align: right;" class="ProductName">
                                                        <asp:Button Text="Subscribe" Width="120" ID="ManageSubscription" runat="server" CommandName="Save" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        <uc4:wucRDRProductRulesSetup runat="server" ID="wucMerchantProductRuleSetup1" />
                                    </Template>
                                </ig:ContentTabItem>

                            </Tabs>
                        </ig:WebTab>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="ErrorPanel" Visible="false" runat="server">
            <br />
            <asp:Label runat="server" ID="lblError" EnableViewState="false"></asp:Label>
            <br />
            <br />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
