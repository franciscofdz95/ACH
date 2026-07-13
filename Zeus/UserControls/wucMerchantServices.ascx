<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="UserControls_wucMerchantServices" Codebehind="wucMerchantServices.ascx.cs" %>
<asp:Panel ID="pnlServices" runat="server" Height="" Width="">
    <div class="tabcontent">
        <div class="twocolumn">
            <div class="leftcolumn">
                <div class="bucket">
                    <div class="buckethdr">
                        &nbsp;Identification Numbers</div>
                    <div class="bucketbdy">
                        <table border="0" cellspacing="2">
                            <tr>
                                <td>
                                    American Express:</td>
                                <td>
                                    <asp:TextBox ID="AMEXMid" runat="server" MaxLength="16" Width="100px"></asp:TextBox></td>
                                <td>
                                    <asp:LinkButton ID="lnkAgentUID" runat="server">Agent:</asp:LinkButton></td>
                                <td>
                                    <asp:DropDownList ID="AgentUID" runat="server" Width="250px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    Discover:</td>
                                <td>
                                    <asp:TextBox ID="DiscoverMid" runat="server" MaxLength="16" Width="100px"></asp:TextBox></td>
                                <td>
                                    WEX #:</td>
                                <td>
                                    <asp:TextBox ID="PetroleumWEXMid" runat="server" MaxLength="16" Width="245px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    JCB:</td>
                                <td>
                                    <asp:TextBox ID="JCBMid" runat="server" MaxLength="30" Width="100px"></asp:TextBox></td>
                                <td>
                                    Voyager #:</td>
                                <td>
                                    <asp:TextBox ID="PetroleumVoyagerMid" runat="server" MaxLength="16" Width="245px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    FCS #:</td>
                                <td>
                                    <asp:TextBox ID="EBTFCSMid" runat="server" MaxLength="16" Width="100px"></asp:TextBox></td>
                                <td>
                                    Check service #:</td>
                                <td>
                                    <asp:TextBox ID="CheckServicesMid" runat="server" Width="245px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Front-End:</td>
                                <td>
                                    <asp:DropDownList ID="AuthPlatformUID" runat="server" Width="105px">
                                    </asp:DropDownList></td>
                                <td>
                                    Front-End MID:</td>
                                <td>
                                    <asp:TextBox ID="AuthPlatformMid" runat="server" MaxLength="15" Width="245px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Back-End:</td>
                                <td>
                                    <asp:DropDownList ID="SettlePlatformUID" runat="server" Width="105px">
                                    </asp:DropDownList></td>
                                <td>
                                    Back-End MID</td>
                                <td>
                                    <asp:TextBox ID="SettlePlatformMid" runat="server" MaxLength="15" Width="245px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="rightcolumn">
                <div class="bucket" style="height: 187px">
                    <div class="buckethdr">
                        &nbsp;Bank Account</div>
                    <div class="bucketbdy">
                        <table border="0" cellspacing="2">
                            <tr>
                                <td>
                                    <asp:Label ID="Label34" runat="server" Text="Bank Name:"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="BankName" runat="server" MaxLength="50" Width="400px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Account Name:"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="AccountName" runat="server" MaxLength="50" Width="400px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label35" runat="server" Text="Routing Number:"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="RoutingNumber" runat="server" MaxLength="9"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label36" runat="server" Text="Account Number:"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="AccountNumber" runat="server" MaxLength="18"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="bucket">
            <div class="buckethdr">
                &nbsp;Services</div>
            <div class="bucketbdy">
                <table border="0" cellspacing="2">
                    <tr>
                        <td>
                            <asp:CheckBox ID="VisaMasterCard" runat="server" Text="Visa/MasterCard" /></td>
                        <td>
                            <asp:CheckBox ID="Diners" runat="server" Text="Diners Club" /></td>
                        <td>
                            <asp:CheckBox ID="WEX" runat="server" Text="WEX" /></td>
                        <td>
                            <asp:CheckBox ID="Ebt" runat="server" Text="EBT" /></td>
                        <td>
                            <asp:CheckBox ID="AdditionalLocation" runat="server" Text="Add Location" /></td>
                        <td>
                            <asp:CheckBox ID="ACH_EFT" runat="server" Text="ACH/Elec Fund Transfer" /></td>
                        <td>
                            <asp:CheckBox ID="Reprogram" runat="server" Text="Reprogram?" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="AtmDebit" runat="server" Text="Atm/Debit Card" /></td>
                        <td>
                            <asp:CheckBox ID="Others" runat="server" Text="Others" /></td>
                        <td>
                            <asp:CheckBox ID="Jcb" runat="server" Text="JCB" /></td>
                        <td>
                            <asp:CheckBox ID="EBTCashback" runat="server" Text="EBT Cashback" /></td>
                        <td>
                            <asp:CheckBox ID="AddTerminal" runat="server" Text="Add Terminal" /></td>
                        <td>
                            <asp:CheckBox ID="NMCMerClub" runat="server" Text="Merchant Club" /></td>
                        <td>
                            <asp:CheckBox ID="ChangeOfOwnership" runat="server" Text="Change of ownership?" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="AmericanExpress" runat="server" Text="American Express" /></td>
                        <td>
                            <asp:CheckBox ID="StoredValuePrograms" runat="server" Text="Stored Value Programs" /></td>
                        <td>
                            <asp:CheckBox ID="Voyager" runat="server" Text="Voyager" /></td>
                        <td>
                            <asp:CheckBox ID="GiftCards" runat="server" Text="Electronic Gift Card" /></td>
                        <td>
                            <asp:CheckBox ID="Checks" runat="server" Text="Check Services" /></td>
                        <td>
                            <asp:CheckBox ID="NMCGateway" runat="server" Text="Gateway" /></td>
                        <td>
                            <asp:CheckBox ID="ACH" runat="server" Text="ACH" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="DiscoverNovus" runat="server" Text="Discover Novus" /></td>
                        <td>
                            <asp:CheckBox ID="Wireless" runat="server" Text="Wireless" /></td>
                        <td>
                            <asp:CheckBox ID="MediFAX" runat="server" Text="MediFAX" /></td>
                        <td>
                            <asp:CheckBox ID="PrePaidPhone" runat="server" Text="Pre-Paid Calling Card" /></td>
                        <td>
                            <asp:CheckBox ID="Internet" runat="server" Text="Internet Services" /></td>
                        <td>
                            <asp:CheckBox ID="AllowBlindCredits" runat="server" Text="Allow Blind Credits" />
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Panel>
