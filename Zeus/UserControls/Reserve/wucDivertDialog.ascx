<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucDivertDialog.ascx.cs"
    Inherits="ZeusWeb.UserControls.Reserve.wucDivertDialog" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<ig:WebDialogWindow ID="dlg" runat="server" Height="500px" Width="980px" Modal="True"
    InitialLocation="Centered" WindowState="Hidden">
    <ContentPane>
        <Template>
            <div class="dialog">
                <asp:Panel runat="server" ID="pnlDetails">
                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="MyDivertDialog" CssClass="errorlist"
                        runat="server" />
                    <asp:CustomValidator ID="cvCustomRow" runat="server" Visible="false" ValidationGroup="MyDivertDialog"
                        ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="cvCustomDivertZero" runat="server" Visible="false" ValidationGroup="MyDivertDialog"
                        ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="cvCustomReserveZero" runat="server" Visible="false" ValidationGroup="MyDivertDialog"
                        ErrorMessage=""></asp:CustomValidator>

                    <br />
                    <fieldset>
                        <legend>Details</legend>



                        <b>ZID:</b>
                        <asp:Label runat="server" ID="lblZID"></asp:Label><span style="padding: 0px 15px;">&nbsp;</span>
                        <b>MID:</b>
                        <asp:Label runat="server" ID="lblMID"></asp:Label><span style="padding: 0px 15px;">&nbsp;</span>
                        <b>DBA:</b>
                        <asp:Label runat="server" ID="lblDBA"></asp:Label>


                        <asp:GridView ID="grdDivertDetails" runat="server" Font-Names="Verdana" ShowHeaderWhenEmpty="True"
                            DataKeyNames="DivertID" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                            AlternatingRowStyle-CssClass="alt" SelectedRowStyle-BackColor="#fffacd" AutoGenerateColumns="False"
                            OnRowDataBound="grdDivertDetails_RowDataBound" ShowFooter="True">
                            <Columns>
                                <asp:BoundField DataField="DivertID" HeaderText="DivertID" Visible="false" />
                                <asp:BoundField DataField="BankName" HeaderText="Bank" Visible="false" />
                                <asp:BoundField DataField="DivertCategory" ItemStyle-HorizontalAlign="Right" HeaderText="Category">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <%--                                <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="BatchWithHeldAmount" HeaderText="Batch WithHeld" ItemStyle-HorizontalAlign="Right" />
                                --%>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="Amount"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sent to Rsrv Acct">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="BatchWithHeldAmount"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reserve">
                                    <ItemTemplate>
                                        <ig:WebNumericEditor ID="Reserve" runat="server" Value='<%# Bind("Reserve") %>'
                                            CausesValidation="true" ValidationGroup="MyDivertDialog" ValueText="0" Width="70px"
                                            MaxDecimalPlaces="2" MinDecimalPlaces="2" NullText="0.00" DataMode="Decimal">
                                        </ig:WebNumericEditor>
                                        <%--OnTextChanged="value_TextChanged" AutoPostBackFlags-ValueChanged="On"--%>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Divert">
                                    <ItemTemplate>
                                        <ig:WebNumericEditor ID="DivertClear" runat="server" Value='<%# Bind("DivertClear") %>'
                                            CausesValidation="true" ValidationGroup="MyDivertDialog" ValueText="0" Width="70px"
                                            MaxDecimalPlaces="2" MinDecimalPlaces="2" NullText="0.00" DataMode="Decimal">
                                        </ig:WebNumericEditor>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                               
                                <asp:TemplateField HeaderText="Paysafe">
                                    <ItemTemplate>
                                        <ig:WebNumericEditor ID="DivertReject" runat="server" Value='<%# Bind("DivertReject") %>'
                                            CausesValidation="true" ValidationGroup="MyDivertDialog" ValueText="0" Width="70px"
                                            MaxDecimalPlaces="2" MinDecimalPlaces="2" NullText="0.00" DataMode="Decimal">
                                        </ig:WebNumericEditor>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Merchant">
                                    <ItemTemplate>
                                        <ig:WebNumericEditor ID="PostMerchant" runat="server" Value='<%# Bind("PostMerchant") %>'
                                            CausesValidation="true" ValidationGroup="MyDivertDialog" ValueText="0" Width="70px"
                                            MaxDecimalPlaces="2" MinDecimalPlaces="2" NullText="0.00" DataMode="Decimal">
                                        </ig:WebNumericEditor>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Need to Allocate">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSum"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle BackColor="LemonChiffon" HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Positive">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPositive" runat="server" Text='<%# Bind("Positive", "{0:0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Negative">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNegative" runat="server" Text='<%# Bind("Negative", "{0:0.00}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                            <EmptyDataTemplate>
                                No Records
                            </EmptyDataTemplate>
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <SelectedRowStyle BackColor="LemonChiffon" />
                        </asp:GridView>
                    </fieldset>
                    <div style="text-align: right">
                        <asp:LinkButton ID="btnReCalcuate" runat="server" OnClick="btnReCalcuate_Click">Re-Calculate</asp:LinkButton>
                    </div>
                    <table class="mGrid">
                        <tr>
                            <th style="text-align: left;">Report Date:
                    <asp:Label runat="server" ID="ReportDate"></asp:Label>
                            </th>
                            <th style="text-align: left;">Beginning Balance
                            </th>
                            <th style="text-align: left;">
                            Updated Balance
                            </td>
                        </tr>
                        <tr>
                            <td>Reserve Amount:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblReserveAmount">0.00</asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="afterReserve">0.00</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Divert Amount:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblDivertAmount">0.00</asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="afterDivert">0.00</asp:Label>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <div style="text-align: center">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="MyDivertDialog"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" CausesValidation="false" Text="Close" OnClick="btnClose_Click" />
                        <asp:Button ID="btnReset" runat="server" Text="Reset Default Dispositions" OnClick="btnReset_Click" />
                    </div>
                </asp:Panel>
            </div>
        </Template>
    </ContentPane>
    <Header CaptionText="Divert">
    </Header>
</ig:WebDialogWindow>
