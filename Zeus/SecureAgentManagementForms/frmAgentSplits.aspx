<%@ Page Language="C#" MasterPageFile="~/MasterPageAgent.master" AutoEventWireup="true" Inherits="frmAgentSplits" Title="Agent Splits" CodeBehind="frmAgentSplits.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .lblLeft
        {
            width: 200px;
        }

        .auto-style3
        {
            height: 26px;
        }
    </style>
    <div id="contentpage">
        <asp:Label ID="lblError" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
        <table width="100%">
            <tr>
                <td>
                    <div class="tbrtools">
                        <div class="tbrtoolsleft">
                            <table>
                                <tr>
                                    <td>
                                        <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                            AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                            <Appearance>
                                                <Image Url="~/Images/edit.png" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td>
                                        <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false"
                                            AccessKey="s" CommandName="Save" OnClick="tbrTools_ButtonClicked">
                                            <Appearance>
                                                <Image Url="~/Images/disk_blue.png" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                    </td>
                                    <td>
                                        <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                            AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                            <Appearance>
                                                <Image Url="~/Images/disk_blue_error.png" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <asp:Panel runat="server" ID="pnlDetails">
                    <asp:Panel runat="server" ID="pnlAccount">
                            <%-- <table width="100%">
                            <tr>
                                <td colspan="2">--%>
                            <fieldset style="height: 150px; width: 800px;">
                                        <legend>Bank Account Information</legend>
                                        <table width="100%">
                                            <tr>
                                                <td class="lblRight">Account Name:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="BankWireRecepientName" runat="server" Width="145px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="lblRight"><b>US Domestic Bank Account</b></td>
                                                <td></td>
                                                <td class="lblRight"><b>Wire Account Information</b></td>
                                            </tr>
                                            <tr>
                                                <td class="lblRight">Dstn Bank Accnt #:</td>
                                                <td>
                                                    <asp:TextBox ID="BankAccountNumber" runat="server" MaxLength="20" Width="145px"></asp:TextBox>
                                                </td>
                                                <td class="lblRight">IBAN:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="BankIBAN" runat="server" MaxLength="30" Width="145px"></asp:TextBox>

                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td class="lblRight">Dstn Bank Routing #:</td>
                                                <td class="lblLeft">
                                                    <asp:TextBox ID="BankABA" runat="server" MaxLength="11" Width="145px"></asp:TextBox></td>
                                                <td class="lblRight">Bank ID/SwiftCode:
                                                </td>
                                                <td class="lblLeft">
                                                    <asp:TextBox ID="BankSwiftID" runat="server" MaxLength="11" Width="145px"></asp:TextBox>

                                                </td>
                                               
                                            </tr>
                                             <tr>
                                                <td class="lblRight">Dstn Bank currency</td>
                                                <td class="lblLeft">
                                                    <asp:DropDownList runat="server" Width="145px" ID="BankCurrency"></asp:DropDownList>

                                                </td>
                                                <td class="lblRight" title ="The CAD requirement for transit number consists of nine digits. (Bank Code (4 digits) + Branch Code (5 digits))">Transit Number:
                                                </td>
                                                <td class="lblLeft">
                                                    <asp:TextBox ID="BankCode" runat="server" MaxLength="4" Width="60px" ToolTip="Bank Code (4 digits)"></asp:TextBox>
                                                    <asp:TextBox ID="BankBranchCode" runat="server" MaxLength="5" Width="80px" ToolTip ="Branch Code (5 digits)"></asp:TextBox>

                                                </td>

                                            </tr>
                                        </table>
                                    </fieldset>
                            <%--</td>
                            </tr>
                            <tr>
                                <td>--%>

                            <table>
                                <tr>
                                <td>
                                        <fieldset style="height: 190px; width: 385px;">
                                        <legend>Business Address</legend>
                                            <table width="100%">
                                            <tr>
                                                <td class="lblRight">Country:
                                                </td>
                                                <td>
                                                        <asp:DropDownList ID="BillingCountry" runat="server" Width="170px" AutoPostBack="True" OnSelectedIndexChanged="BillingCountry_SelectedIndexChanged1">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="lblRight"></td>
                                            </tr>
                                            <tr>

                                                <td class="lblRight">Name:
                                                </td>
                                                <td>
                                                        <asp:TextBox ID="BillingFullName" Width="170px" AutoPostBack="False" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="lblRight"></td>
                                            </tr>
                                            <tr>

                                                <td class="lblRight">Addr 1:
                                                </td>
                                                <td>
                                                        <asp:TextBox ID="BillingAddressLine1" Width="170px" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="lblRight"></td>
                                            </tr>
                                            <tr>

                                                <td class="lblRight">Addr 2:</td>
                                                <td>
                                                        <asp:TextBox ID="BillingAddressLine2" Width="170px" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="lblRight"></td>
                                            </tr>
                                            <tr>

                                                <td class="lblRight">City:
                                                </td>
                                                <td>
                                                        <asp:TextBox ID="BillingCity" Width="170px" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="lblRight"></td>
                                            </tr>
                                            <tr>

                                                <td class="lblRight">State/Province:
                                                </td>
                                                <td>
                                                        <asp:DropDownList ID="BillingState" runat="server" Width="170px" Visible="true">
                                                        <asp:ListItem Value="--">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="AL">AL</asp:ListItem>
                                                        <asp:ListItem Value="AK">AK</asp:ListItem>
                                                        <asp:ListItem Value="AZ">AZ</asp:ListItem>
                                                        <asp:ListItem Value="AR">AR</asp:ListItem>
                                                        <asp:ListItem Value="CA">CA</asp:ListItem>
                                                        <asp:ListItem Value="CO">CO</asp:ListItem>
                                                        <asp:ListItem Value="CT">CT</asp:ListItem>
                                                        <asp:ListItem Value="DE">DE</asp:ListItem>
                                                        <asp:ListItem Value="DC">DC</asp:ListItem>
                                                        <asp:ListItem Value="FL">FL</asp:ListItem>
                                                        <asp:ListItem Value="GA">GA</asp:ListItem>
                                                        <asp:ListItem Value="HI">HI</asp:ListItem>
                                                        <asp:ListItem Value="IA">IA</asp:ListItem>
                                                        <asp:ListItem Value="ID">ID</asp:ListItem>
                                                        <asp:ListItem Value="IL">IL</asp:ListItem>
                                                        <asp:ListItem Value="IN">IN</asp:ListItem>
                                                        <asp:ListItem Value="KS">KS</asp:ListItem>
                                                        <asp:ListItem Value="KY">KY</asp:ListItem>
                                                        <asp:ListItem Value="LA">LA</asp:ListItem>
                                                        <asp:ListItem Value="ME">ME</asp:ListItem>
                                                        <asp:ListItem Value="MD">MD</asp:ListItem>
                                                        <asp:ListItem Value="MA">MA</asp:ListItem>
                                                        <asp:ListItem Value="MI">MI</asp:ListItem>
                                                        <asp:ListItem Value="MN">MN</asp:ListItem>
                                                        <asp:ListItem Value="MO">MO</asp:ListItem>
                                                        <asp:ListItem Value="MS">MS</asp:ListItem>
                                                        <asp:ListItem Value="MT">MT</asp:ListItem>
                                                        <asp:ListItem Value="NE">NE</asp:ListItem>
                                                        <asp:ListItem Value="NV">NV</asp:ListItem>
                                                        <asp:ListItem Value="NH">NH</asp:ListItem>
                                                        <asp:ListItem Value="NJ">NJ</asp:ListItem>
                                                        <asp:ListItem Value="NM">NM</asp:ListItem>
                                                        <asp:ListItem Value="NY">NY</asp:ListItem>
                                                        <asp:ListItem Value="NC">NC</asp:ListItem>
                                                        <asp:ListItem Value="ND">ND</asp:ListItem>
                                                        <asp:ListItem Value="OH">OH</asp:ListItem>
                                                        <asp:ListItem Value="OK">OK</asp:ListItem>
                                                        <asp:ListItem Value="OR">OR</asp:ListItem>
                                                        <asp:ListItem Value="PA">PA</asp:ListItem>
                                                        <asp:ListItem Value="RI">RI</asp:ListItem>
                                                        <asp:ListItem Value="SC">SC</asp:ListItem>
                                                        <asp:ListItem Value="SD">SD</asp:ListItem>
                                                        <asp:ListItem Value="TN">TN</asp:ListItem>
                                                        <asp:ListItem Value="TX">TX</asp:ListItem>
                                                        <asp:ListItem Value="UT">UT</asp:ListItem>
                                                        <asp:ListItem Value="VT">VT</asp:ListItem>
                                                        <asp:ListItem Value="VA">VA</asp:ListItem>
                                                        <asp:ListItem Value="WA">WA</asp:ListItem>
                                                        <asp:ListItem Value="WV">WV</asp:ListItem>
                                                        <asp:ListItem Value="WI">WI</asp:ListItem>
                                                        <asp:ListItem Value="WY">WY</asp:ListItem>
                                                    </asp:DropDownList>
                                                        <asp:TextBox ID="BillingProvince" Width="170px" runat="server" Visible="false"></asp:TextBox>

                                                </td>
                                                <td class="lblRight"></td>
                                            </tr>
                                            <tr>

                                                <td class="lblRight">Postal Code:
                                                </td>
                                                <td>
                                                        <asp:TextBox ID="BillingZipCode" Width="170px" runat="server"></asp:TextBox>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                                <td>
                                        <fieldset style="height: 190px; width: 385px;">
                                        <legend>Bank Address</legend>
                                            <table width="100%">
                                            <tr>

                                                <td class="lblRight">Country:
                                                </td>
                                                <td>
                                                        <asp:DropDownList ID="BankCountry" runat="server" Width="170px" AutoPostBack="True" OnSelectedIndexChanged="BankCountry_SelectedIndexChanged1">
                                                    </asp:DropDownList></td>
                                                <td class="lblLeft"></td>
                                            </tr>
                                            <tr>

                                                <td class="lblRight">Bank Name:</td>
                                                <td>
                                                        <asp:TextBox ID="BankName" runat="server" MaxLength="100" Width="170px"></asp:TextBox></td>
                                                <td class="lblLeft"></td>
                                            </tr>
                                            <tr>

                                                <td class="lblRight">Addr 1:
                                                </td>
                                                    <td>
                                                        <asp:TextBox ID="BankAddressLine1" Width="170px" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="lblLeft"></td>
                                            </tr>
                                            <tr>

                                                <td class="lblRight">Addr 2:</td>
                                                <td>
                                                        <asp:TextBox ID="BankAddressLine2" Width="170px" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="lblLeft"></td>
                                            </tr>
                                            <tr>
                                                <td class="lblRight">City:
                                                </td>
                                                <td >
                                                        <asp:TextBox ID="BankCity" Width="170px" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="lblLeft"></td>
                                            </tr>
                                            <tr>
                                                <td class="lblRight">State/Province:
                                                </td>
                                                <td>
                                                        <asp:DropDownList ID="BankState" runat="server" Width="170px" Visible="true">
                                                        <asp:ListItem Value="--">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="AL">AL</asp:ListItem>
                                                        <asp:ListItem Value="AK">AK</asp:ListItem>
                                                        <asp:ListItem Value="AZ">AZ</asp:ListItem>
                                                        <asp:ListItem Value="AR">AR</asp:ListItem>
                                                        <asp:ListItem Value="CA">CA</asp:ListItem>
                                                        <asp:ListItem Value="CO">CO</asp:ListItem>
                                                        <asp:ListItem Value="CT">CT</asp:ListItem>
                                                        <asp:ListItem Value="DE">DE</asp:ListItem>
                                                        <asp:ListItem Value="DC">DC</asp:ListItem>
                                                        <asp:ListItem Value="FL">FL</asp:ListItem>
                                                        <asp:ListItem Value="GA">GA</asp:ListItem>
                                                        <asp:ListItem Value="HI">HI</asp:ListItem>
                                                        <asp:ListItem Value="IA">IA</asp:ListItem>
                                                        <asp:ListItem Value="ID">ID</asp:ListItem>
                                                        <asp:ListItem Value="IL">IL</asp:ListItem>
                                                        <asp:ListItem Value="IN">IN</asp:ListItem>
                                                        <asp:ListItem Value="KS">KS</asp:ListItem>
                                                        <asp:ListItem Value="KY">KY</asp:ListItem>
                                                        <asp:ListItem Value="LA">LA</asp:ListItem>
                                                        <asp:ListItem Value="ME">ME</asp:ListItem>
                                                        <asp:ListItem Value="MD">MD</asp:ListItem>
                                                        <asp:ListItem Value="MA">MA</asp:ListItem>
                                                        <asp:ListItem Value="MI">MI</asp:ListItem>
                                                        <asp:ListItem Value="MN">MN</asp:ListItem>
                                                        <asp:ListItem Value="MO">MO</asp:ListItem>
                                                        <asp:ListItem Value="MS">MS</asp:ListItem>
                                                        <asp:ListItem Value="MT">MT</asp:ListItem>
                                                        <asp:ListItem Value="NE">NE</asp:ListItem>
                                                        <asp:ListItem Value="NV">NV</asp:ListItem>
                                                        <asp:ListItem Value="NH">NH</asp:ListItem>
                                                        <asp:ListItem Value="NJ">NJ</asp:ListItem>
                                                        <asp:ListItem Value="NM">NM</asp:ListItem>
                                                        <asp:ListItem Value="NY">NY</asp:ListItem>
                                                        <asp:ListItem Value="NC">NC</asp:ListItem>
                                                        <asp:ListItem Value="ND">ND</asp:ListItem>
                                                        <asp:ListItem Value="OH">OH</asp:ListItem>
                                                        <asp:ListItem Value="OK">OK</asp:ListItem>
                                                        <asp:ListItem Value="OR">OR</asp:ListItem>
                                                        <asp:ListItem Value="PA">PA</asp:ListItem>
                                                        <asp:ListItem Value="RI">RI</asp:ListItem>
                                                        <asp:ListItem Value="SC">SC</asp:ListItem>
                                                        <asp:ListItem Value="SD">SD</asp:ListItem>
                                                        <asp:ListItem Value="TN">TN</asp:ListItem>
                                                        <asp:ListItem Value="TX">TX</asp:ListItem>
                                                        <asp:ListItem Value="UT">UT</asp:ListItem>
                                                        <asp:ListItem Value="VT">VT</asp:ListItem>
                                                        <asp:ListItem Value="VA">VA</asp:ListItem>
                                                        <asp:ListItem Value="WA">WA</asp:ListItem>
                                                        <asp:ListItem Value="WV">WV</asp:ListItem>
                                                        <asp:ListItem Value="WI">WI</asp:ListItem>
                                                        <asp:ListItem Value="WY">WY</asp:ListItem>
                                                    </asp:DropDownList>
                                                        <asp:TextBox ID="BankProvince" Width="170px" runat="server" Visible="false"></asp:TextBox>
                                                </td>
                                                <td class="lblLeft"></td>
                                            </tr>
                                            <tr>

                                                <td class="lblRight">Postal Code:
                                                </td>
                                                <td>
                                                        <asp:TextBox ID="BankZipCode" Width="170px" runat="server"></asp:TextBox></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                            <%--</td>
                            </tr>
                        </table>--%>
                    </asp:Panel>
                        <fieldset style="width: 800px;">
                        <legend>Payout Splits</legend>
                            <table cellpadding="2">
                            <tr>
                                <td>Residual Payout Threshold:</td>
                                <td>
                                    <ig:WebNumericEditor ID="ResidualPayoutThreshold" runat="server" BorderStyle="None" MinDecimalPlaces="2"
                                        Font-Names="Verdana" NullValue="0" Font-Size="8pt" MinValue="0" MaxValue="100000.00" MaxLength="9" HorizontalAlign="Right" Width="145px">
                                    </ig:WebNumericEditor>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pnlDetail" runat="server" ScrollBars="vertical">
                            <asp:GridView ID="grdPayouts" AutoGenerateColumns="false" runat="server" ShowFooter="true"
                                Font-Names="Verdana" Width="70%" Font-Size="X-Small" CssClass="mGrid" OnRowDataBound="grdPayouts_RowDataBound"
                                DataKeyNames="UID">
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                                <FooterStyle CssClass="footer" />
                                <HeaderStyle HorizontalAlign="center" />
                                <Columns>
                                    <asp:BoundField DataField="ID" FooterText="Total:" HeaderText="ID" />
                                    <asp:BoundField DataField="DBA" HeaderText="DBA" ItemStyle-Width="300px" />
                                    <asp:BoundField DataField="Level" HeaderText="Level" />
                                    <asp:TemplateField HeaderText="Default Rate %" ItemStyle-HorizontalAlign="right"
                                        FooterStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <ig:WebPercentEditor ID="WebPercentEdit1" runat="server" BorderStyle="None" MinDecimalPlaces="2"
                                                Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right">
                                            </ig:WebPercentEditor>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payout Enabled" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="ckhEnabled" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UID" Visible="false" HeaderText="UID" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </fieldset>
                        <fieldset style="width: 800px;">
                            <legend>Settings</legend>
                            <table>
                                <tr>
                                    <td class="lblRight">Pymt Method:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="PaymentMethod" runat="server" Width="170px">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            <asp:ListItem Value="A">ACH</asp:ListItem>
                                            <asp:ListItem Value="D">Auto Debit</asp:ListItem>
                                            <asp:ListItem Value="C">Check</asp:ListItem>
                                            <asp:ListItem Value="W">Wire Transfer</asp:ListItem>
                                            <asp:ListItem Value="O">Other</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Paysafe Pays:</td>
                                    <td align="left">
                                        <asp:CheckBox ID="MeritusPays" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Residuals Suppress Details:</td>
                                    <td align="left">
                                        <asp:CheckBox ID="ReportSuppressDetails" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">ETF Waived:</td>
                                    <td align="left">
                                        <asp:CheckBox ID="ETFWaived" runat="server" Text="" />
                                    </td>
                                </tr>

                            </table>
                        </fieldset>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
