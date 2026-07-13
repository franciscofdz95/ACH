<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMerchantCardCurrency.ascx.cs" Inherits="ZeusWeb.UserControls.wucMerchantCardCurrency" %>

<fieldset>
    <legend>Card Currency</legend>    
    <asp:Panel ID="pnlCardCurrency" runat="server" Height="" Width="100%">
        <asp:GridView ID="gvCardCurrency" runat="server" GridLines="none" Font-Names="Verdana"
            Font-Size="x-small" OnRowDataBound="gvCardCurrency_RowDataBound"
            EmptyDataText="No Card Currencies..." Style="table-layout: fixed" Width="100%" AutoGenerateColumns="false"
            CssClass="mGrid" >
            <PagerStyle CssClass="pgr" />
            <AlternatingRowStyle CssClass="alt" />
            <HeaderStyle HorizontalAlign="center" />
            <Columns>
                <asp:BoundField DataField="CurrencyDesc" HeaderText="" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                <asp:TemplateField HeaderText="Visa" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" class="CardCurrency" ID="chkVisa" Visible="false" />
                        <asp:HiddenField runat="server" ID="hdnVisaPCCId" Value='<%#Eval("AllowVisa") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mastercard" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" class="CardCurrency" ID="chkMastercard" Visible="false" />
                        <asp:HiddenField runat="server" ID="hdnMastercardPCCId" Value='<%#Eval("AllowMC") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amex" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px" HeaderStyle-Width="40px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" class="CardCurrency" ID="chkAmex" Visible="false" />
                        <asp:HiddenField runat="server" ID="hdnAmexPCCId" Value='<%#Eval("AllowAmex") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Discover" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" class="CardCurrency" ID="chkDiscover" Visible="false" />
                        <asp:HiddenField runat="server" ID="hdnDiscoverPCCId" Value='<%#Eval("AllowDiscover") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MerchantVisaEnabled" Visible="false" />
                <asp:BoundField DataField="MerchantMCEnabled" Visible="false" />
                <asp:BoundField DataField="MerchantAmexEnabled" Visible="false" />
                <asp:BoundField DataField="MerchantDiscoverEnabled" Visible="false" />
            </Columns>
        </asp:GridView>
        <asp:Label ID="lblError" runat="server" EnableViewState="false" Font-Size="10pt" ForeColor="Red"></asp:Label>
    </asp:Panel>
</fieldset>

