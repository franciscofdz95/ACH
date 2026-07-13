<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucMRule.ascx.cs" Inherits="ZeusWeb.UserControls.wucMRule" %>
<%@ Register Src="wucMessage.ascx" TagName="wucMessage" TagPrefix="uc1" %>
<uc1:wucMessage ID="wucMessage1" runat="server" />
<asp:Panel runat="server" ID="pnlView">
    <asp:GridView ID="GridView1" runat="server" CssClass="mGrid" AutoGenerateColumns="False"
        OnPreRender="GridView1_PreRender" OnRowCommand="GridView1_RowCommand" DataKeyNames="MRuleID"
        OnRowDataBound="GridView1_RowDataBound" EnableModelValidation="True">
        <Columns>
            <asp:TemplateField HeaderText="Enabled" ItemStyle-Width="60px">
                <ItemTemplate>
                    <asp:CheckBox ID="cbEnabled" AutoPostBack="true" runat="server" OnCheckedChanged="cbEnabled_CheckedChanged" />
                    <asp:HiddenField ID="hidMRuleID" runat="server" Value='<%# Bind("MRuleID") %>' />
                    <asp:HiddenField ID="hidMerchantID" runat="server" Value='<%# Bind("MerchantID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Rule Name" ItemStyle-Width="130px">
                <ItemTemplate>
                    <asp:LinkButton ID="lbRuleName" runat="server" CommandName="RuleName" CommandArgument='<%# Bind("MRuleID") %>'
                        Text='<%# Bind("RuleNameNice") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="RuleDescription" HeaderText="Rule Description" />
            <asp:TemplateField HeaderText="Parameters">
                <ItemTemplate>
                    <%--  <asp:Literal runat="server" ID="litParams"></asp:Literal>--%>
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Width="100%"
                        GridLines="None" BorderColor="White" ShowHeader="False" OnRowDataBound="GridView2_RowDataBound"
                        EnableModelValidation="True">
                        <Columns>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%# Bind("ParamName") %>' ID="lblName"></asp:Label>:
                                </ItemTemplate>
                                <ItemStyle Width="50%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Value">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidMRuleParamID" Value='<%# Bind("MRuleParamID") %>' />
                                    <asp:HiddenField runat="server" ID="hidMRuleID" Value='<%# Bind("MRuleID") %>' />
                                    <asp:TextBox runat="server" AutoPostBack="true" Text='<%# Bind("ParamValue") %>' OnTextChanged="tbDefaultParamValue_TextChanged"
                                        ID="tbValue"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Width="50%" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No Parameters
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Panel>
