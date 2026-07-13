<%@ Page Language="C#" MasterPageFile="~/MasterPageForms.master" AutoEventWireup="true" Inherits="frmForms" Title="Forms" Codebehind="frmForms.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <table width="100%">
            <tr>
                <td>
                    <fieldset>
                        <legend>Merchant Applications/Addendums</legend>
                        <br />
                        <asp:GridView ID="grdFormMerchants" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                            Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            DataSourceID="odsMerchantSource">
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="Name,Type" DataNavigateUrlFormatString="~/forms/merchant/{0}.{1}"
                                    Target="_blank" DataTextField="Name" HeaderText="Form Name" SortExpression="Name" />
                                <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsMerchantSource" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetForms" TypeName="LocalForms">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="~/forms/merchant" Name="path" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </fieldset>
                    <fieldset>
                        <legend>Bank Forms</legend>
                        <br />
                        <asp:GridView ID="grdFormBanks" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                            Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            DataSourceID="odsBankSource">
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="Name,Type" DataNavigateUrlFormatString="~/forms/merchant/{0}.{1}"
                                    Target="_blank" DataTextField="Name" HeaderText="Form Name" SortExpression="Name" />
                                <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsBankSource" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetForms" TypeName="LocalForms" EnableCaching="True">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="~/forms/bank" Name="path" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </fieldset>
                    <fieldset>
                        <legend>IT Forms</legend>
                        <br />
                        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                            Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            DataSourceID="odsITSource">
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="Name,Type" DataNavigateUrlFormatString="~/forms/merchant/{0}.{1}"
                                    Target="_blank" DataTextField="Name" HeaderText="Form Name" SortExpression="Name" />
                                <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsITSource" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetForms" TypeName="LocalForms" EnableCaching="True">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="~/forms/it" Name="path" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </fieldset>
                    <fieldset>
                        <legend>ISO Setup and Order Forms</legend>
                        <br />
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                            Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            DataSourceID="odsIsoOrderSource">
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="Name,Type" DataNavigateUrlFormatString="~/forms/merchant/{0}.{1}"
                                    Target="_blank" DataTextField="Name" HeaderText="Form Name" SortExpression="Name" />
                                <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsIsoOrderSource" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetForms" TypeName="LocalForms">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="~/forms/iso" Name="path" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </fieldset>
                    <fieldset>
                        <legend>Underwriting Forms</legend>
                        <br />
                        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                            Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            DataSourceID="odsUnderwritingSource">
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="Name,Type" DataNavigateUrlFormatString="~/forms/merchant/{0}.{1}"
                                    Target="_blank" DataTextField="Name" HeaderText="Form Name" SortExpression="Name" />
                                <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsUnderwritingSource" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetForms" TypeName="LocalForms" EnableCaching="True">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="~/forms/underwriting" Name="path" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </fieldset>
                    <fieldset>
                        <legend>Other Forms</legend>
                        <br />
                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                            Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            DataSourceID="odsOtherSource">
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="Name,Type" DataNavigateUrlFormatString="~/forms/merchant/{0}.{1}"
                                    Target="_blank" DataTextField="Name" HeaderText="Form Name" SortExpression="Name" />
                                <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsOtherSource" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetForms" TypeName="LocalForms">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="~/forms/iso" Name="path" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
