<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucMerchantGeneralInfo" Codebehind="wucMerchantGeneralInfo.ascx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Panel ID="pnlGeneralInfo" runat="server" Height="" Width="">
    <div class="tabcontent">
        <div class="twocolumn">
            <div class="leftcolumn">
                <div class="bucket">
                    <div class="buckethdr">
                        &nbsp;Business Information</div>
                    <div class="bucketbdy">
                        <table cellspacing="5">
                            <tr>
                                <td style="width: 127px">
                                    Type:</td>
                                <td>
                                    <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="155px">
                                    </asp:DropDownList></td>
                                <td style="width: 115px">
                                    Status:</td>
                                <td>
                                    <asp:DropDownList ID="StatusUID" runat="server" Width="155px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    Merchant ID:</td>
                                <td>
                                    <asp:TextBox ID="ID" ReadOnly="true" Enabled="false" runat="server" MaxLength="50"
                                        Width="150px"></asp:TextBox></td>
                                <td>
                                    DBA:</td>
                                <td>
                                    <asp:TextBox ID="BusinessDBAName" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Legal Business Name:</td>
                                <td>
                                    <asp:TextBox ID="BusinessLegalName" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                                <td>
                                    Business Type:</td>
                                <td>
                                    <asp:DropDownList ID="BusinessStructureUID" runat="server" Width="155px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    Tax ID:</td>
                                <td>
                                    <asp:TextBox ID="BusinessTaxID" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                                <td>
                                    Business License #:</td>
                                <td>
                                    <asp:TextBox ID="BusinessLicense" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Business Start Date:</td>
                                <td>
                                    <ig:WebDatePicker ID="BusinessStartDate" runat="server" NullDateLabel="" NullValueRepresentation="Null"
                                        Width="155px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                </td>
                                <td>
                                    State Filed:</td>
                                <td>
                                    <asp:TextBox ID="BusinessStateFiled" runat="server" MaxLength="2" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Website:</td>
                                <td>
                                    <asp:TextBox ID="BusinessWebsite" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                                <td>
                                    Reason for change:</td>
                                <td>
                                    <asp:DropDownList ID="ReasonChangesUID" runat="server" Width="155px">
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <br />
                <div class="bucket" style="height: 180px">
                    <div class="buckethdr">
                        &nbsp;Contact</div>
                    <div class="bucketbdy">
                        <table cellspacing="5">
                            <tr>
                                <td style="width: 127px">
                                    Contact Name:</td>
                                <td>
                                    <asp:TextBox ID="BusinessContact" runat="server" MaxLength="50" Height="23px"></asp:TextBox></td>
                                <td style="width: 115px">
                                    Title:</td>
                                <td>
                                    <asp:TextBox ID="BusinessContactTitle" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Email:</td>
                                <td>
                                    <asp:TextBox ID="BusinessEmailAddress" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                                <td>
                                    Company Phone:</td>
                                <td>
                                    <asp:TextBox ID="BusinessPhone" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    DBA Phone:</td>
                                <td>
                                    <asp:TextBox ID="BusinessDBAPhone" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                                <td>
                                    Fax:</td>
                                <td>
                                    <asp:TextBox ID="BusinessFax" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="rightcolumn">
                <div class="bucket" style="height: 208px">
                    <div class="buckethdr">
                        &nbsp;Address</div>
                    <div class="bucketbdy">
                        <table cellspacing="5">
                            <tr>
                                <td style="width: 127px">
                                    Business Address:</td>
                                <td>
                                    <asp:TextBox ID="BusinessAddress" runat="server" MaxLength="50"></asp:TextBox></td>
                                <td style="width: 115px">
                                    Mailing Address:</td>
                                <td>
                                    <asp:TextBox ID="BusinessMailingAddress" runat="server" MaxLength="50"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    County:</td>
                                <td>
                                    <asp:TextBox ID="BusinessCounty" runat="server" MaxLength="50"></asp:TextBox></td>
                                <td>
                                    County:</td>
                                <td>
                                    <asp:TextBox ID="BusinessMailingCounty" runat="server" MaxLength="50"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    City:</td>
                                <td>
                                    <asp:TextBox ID="BusinessCity" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                                <td>
                                    City:</td>
                                <td>
                                    <asp:TextBox ID="BusinessMailingCity" runat="server" MaxLength="50" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    State:</td>
                                <td>
                                    <asp:TextBox ID="BusinessState" runat="server" MaxLength="2" Width="50px"></asp:TextBox></td>
                                <td>
                                    State:</td>
                                <td>
                                    <asp:TextBox ID="BusinessMailingState" runat="server" MaxLength="2" Width="50px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Zip:</td>
                                <td>
                                    <asp:TextBox ID="BusinessZip" runat="server" MaxLength="50" Width="50px"></asp:TextBox></td>
                                <td>
                                    Zip:</td>
                                <td>
                                    <asp:TextBox ID="BusinessMailingZip" runat="server" MaxLength="50" Width="50px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <br />
                <div class="bucket">
                    <div class="buckethdr">
                        &nbsp;Additional Information</div>
                    <div class="bucketbdy">
                        <table border="0" cellspacing="5">
                            <tr>
                                <td style="width: 127px">
                                    Zone:</td>
                                <td>
                                    <asp:DropDownList ID="AreaZoneUID" runat="server" Width="155px">
                                    </asp:DropDownList></td>
                                <td style="width: 115px">
                                    Approx size:</td>
                                <td>
                                    <asp:DropDownList ID="SquareFootageUID" runat="server" Width="155px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    Merchant property:</td>
                                <td>
                                    <asp:DropDownList ID="BusinessPremisesOwnershipUID" runat="server" Width="155px">
                                    </asp:DropDownList></td>
                                <td>
                                    Time Zone:</td>
                                <td>
                                    <asp:DropDownList ID="TimeZone" runat="server" Width="155px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    Merchant Location:</td>
                                <td>
                                    <asp:DropDownList ID="LocationTypeUID" runat="server" Width="155px">
                                    </asp:DropDownList></td>
                                <td>
                                    Application Type:</td>
                                <td>
                                    <asp:DropDownList ID="ApplicationTypeUID" runat="server" Width="155px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    Landlord Name:</td>
                                <td>
                                    <asp:TextBox ID="LandlordName" runat="server" Width="150px" MaxLength="50"></asp:TextBox></td>
                                <td>
                                    Landlord Phone:</td>
                                <td>
                                    <asp:TextBox ID="LandlordTelephone" runat="server" Width="150px" MaxLength="20"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Merchant sells:</td>
                                <td>
                                    <asp:TextBox ID="MerchantSells" runat="server" Width="150px" MaxLength="50"></asp:TextBox></td>
                                <td>
                                    Return Policy:</td>
                                <td>
                                    <asp:DropDownList ID="ReturnPoliciesUID" runat="server" Width="150px">
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <br />
    </div>
</asp:Panel>
