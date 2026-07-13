<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucBusinessInfo_Risk" CodeBehind="wucBusinessInfo_Risk.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <fieldset>
            <legend>Business Information</legend>
            <asp:Panel ID="pnlGeneralInfo" runat="server" Height="" Width="">
                <table border="0" cellspacing="2" width="100%">
                    <tr>
                        <td class="lblRight">
                        </td>
                        <td>
                        </td>
                        <td class="lblRight">
                        </td>
                        <td>
                        </td>
                        <td class="lblRight">
                        </td>
                        <td>
                        </td>
                        <td class="lblRight">
                        </td>
                        <td>
                        </td>
                        <td class="lblRight">
                        </td>
                        <td>
                        </td>
                        <td class="lblRight">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            DBA:</td>
                        <td>
                            <asp:TextBox ID="BusinessDBAName" runat="server" MaxLength="50" Width="145px"></asp:TextBox>
                        </td>
                        <td class="lblRight">
                            MLE:</td>
                        <td>
                            <asp:TextBox ID="BusinessLegalName" runat="server" MaxLength="50" Width="145px"></asp:TextBox></td>                        
                        <td class="lblRight">
                            <asp:Label ID="lblACHStatus" runat="server" Style="display: none;" Text="ACH Status:"></asp:Label>
                            <asp:Label ID="lblCCStatus" runat="server" Style="display: inline;" Text="Status:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ACHStatusUID" runat="server" Width="150px" style="display: none;">
                            </asp:DropDownList>
                            <asp:DropDownList ID="StatusUID" runat="server" AutoPostBack="true" Width="150px" style="display: inline;">
                            </asp:DropDownList>
                        </td>
                        <td class="lblRight">
                            Partner DBA:</td>
                        <td>
                            <asp:Label runat="server" ID="AgentDBA" Width="150px"></asp:Label>
                        </td>
                        <td class="lblRight">
                            Bank:</td>
                        <td>
                            <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td class="lblRight">
                            ZID:</td>
                        <td>
                            <asp:TextBox ID="ID" ReadOnly="true" Enabled="false" runat="server" MaxLength="50"
                                Width="65px"></asp:TextBox>
                        </td>
                        <td>
                            AID:</td>
                        <td>
                            <asp:TextBox ID="AchID" runat="server" Enabled="false" MaxLength="50" ReadOnly="true"
                                Width="50px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            Back MID</td>
                        <td>
                            <asp:TextBox ID="SettlePlatformMid" runat="server" MaxLength="30" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            Back-End:</td>
                        <td>
                            <asp:DropDownList ID="SettlePlatformUID" runat="server" Width="150px">
                            </asp:DropDownList></td>
                        <td class="lblRight">
                            Reserve %:</td>
                        <td>
                            <ig:WebPercentEditor ID="ReservePercent" runat="server" MaxValue="100" ValueText="0"
                                Width="145px">
                            </ig:WebPercentEditor>
                        </td>
                        <td class="lblRight">
                            Bus Addr:</td>
                        <td>
                            <asp:TextBox ID="BusinessAddress" runat="server" MaxLength="50" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            City:</td>
                        <td>
                            <asp:TextBox ID="BusinessCity" runat="server" MaxLength="50" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            State:</td>
                        <td>
                            <asp:DropDownList ID="BusinessState" runat="server" Width="70px">
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
                            </asp:DropDownList></td>
                        <td>
                            Zip:</td>
                        <td>
                            <asp:TextBox ID="BusinessZip" runat="server" MaxLength="50" Width="50px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            Front MID:</td>
                        <td>
                            <asp:TextBox ID="AuthPlatformMid" runat="server" MaxLength="30" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            Front-End:</td>
                        <td>
                            <asp:DropDownList ID="AuthPlatformUID" runat="server" Width="150px">
                            </asp:DropDownList></td>
                        <td class="lblRight">
                            Release Method:</td>
                        <td>
                            <asp:DropDownList ID="ReleaseMethodUID" runat="server" Width="150px">
                            </asp:DropDownList></td>
                        <td class="lblRight">
                            Mail Addr:</td>
                        <td>
                            <asp:TextBox ID="BusinessMailingAddress" runat="server" MaxLength="50" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            City:</td>
                        <td>
                            <asp:TextBox ID="BusinessMailingCity" runat="server" MaxLength="50" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            State:</td>
                        <td>
                            <asp:DropDownList ID="BusinessMailingState" runat="server" Width="70px">
                                <asp:ListItem>--Select--</asp:ListItem>
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
                            </asp:DropDownList></td>
                        <td>
                            Zip:</td>
                        <td>
                            <asp:TextBox ID="BusinessMailingZip" runat="server" MaxLength="50" Width="50px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            Contact Name:</td>
                        <td>
                            <asp:TextBox ID="BusinessContact" runat="server" MaxLength="30" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            Email:</td>
                        <td>
                            <asp:TextBox ID="BusinessEmailAddress" runat="server" MaxLength="30" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            Phone:</td>
                        <td>
                            <asp:TextBox ID="BusinessPhone" runat="server" MaxLength="30" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            DBA Phone:</td>
                        <td>
                            <asp:TextBox ID="BusinessDBAPhone" runat="server" MaxLength="50" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            Date Approved:</td>
                        <td>
                            <asp:TextBox ID="DateApproved" runat="server" MaxLength="50" Width="145px"></asp:TextBox></td>
                        <td colspan="2">
                            High Risk:</td>
                        <td>
                            <asp:CheckBox ID="HighRisk" runat="server" /></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            Approved Vol:</td>
                        <td>
                            <asp:TextBox ID="TinfoAverageMonthlyVMCVolume" runat="server" MaxLength="30" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            Approved Avg Tkt:</td>
                        <td>
                            <asp:TextBox ID="TinfoAverageVMCTicket" runat="server" MaxLength="30" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            Approved High Tkt:</td>
                        <td>
                            <asp:TextBox ID="TinfoHighestTicketAmount" runat="server" MaxLength="30" Width="145px"></asp:TextBox></td>
                        <td class="lblRight">
                            Approved Delays:</td>
                        <td>
                            <asp:DropDownList ID="DelaysApproved" runat="server" Width="150px">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                <asp:ListItem Value="0">Waived</asp:ListItem>
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                            </asp:DropDownList></td>
                        <td class="lblRight">
                            % Swiped:</td>
                        <td>
                            <asp:TextBox ID="TinfoStoreFrontSwipedPercent" runat="server" MaxLength="50" Width="145px"></asp:TextBox></td>
                        <td colspan="2">
                            Month End Approved:</td>
                        <td>
                            <asp:CheckBox ID="MonthendApproved" runat="server" /></td>
                        <td>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
    </contenttemplate>
</asp:UpdatePanel>
