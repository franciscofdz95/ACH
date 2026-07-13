<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucOwner" Codebehind="wucOwner.ascx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<style type="text/css">
    .auto-style1
    {
        width: 100px;
    }
</style>

 <script type="text/javascript">
     $(document).ready(function () {
         $("#<%=Country.ClientID %>").change(function () {
             if ($(this).val() != "US") {
                 $("#<%=State.ClientID %>").css("display", "none");
                 $("#<%=Province.ClientID %>").css("display", "inline");
             }

             else if ($(this).val() == "US") {
                 $("#<%=State.ClientID %>").css("display", "inline");
                 $("#<%=Province.ClientID %>").css("display", "none");
             }
         })

         $("#<%=Country.ClientID %>").ready(function () {
             var Country = $("#<%=Country.ClientID %>").val();
             if (Country != "US") {
                 $("#<%=State.ClientID %>").css("display", "none");
                 $("#<%=Province.ClientID %>").css("display", "inline");
             }

             if (Country == "US" || Country == "") {
                 $("#<%=State.ClientID %>").css("display", "inline");
                 $("#<%=Province.ClientID %>").css("display", "none");
             }

         });

         $("#ContentPlaceHolder1_wucOwner0_HomePhoneCountryCode").change(function () {
             $("#ContentPlaceHolder1_wucOwner0_HomeCountryCodeDisplay").val($(this).val());
         });
         $("#ContentPlaceHolder1_wucOwner1_HomePhoneCountryCode").change(function () {
             $("#ContentPlaceHolder1_wucOwner1_HomeCountryCodeDisplay").val($(this).val());
         });
         $("#ContentPlaceHolder1_wucOwner2_HomePhoneCountryCode").change(function () {
             $("#ContentPlaceHolder1_wucOwner2_HomeCountryCodeDisplay").val($(this).val());
         });
         $("#ContentPlaceHolder1_wucOwner3_HomePhoneCountryCode").change(function () {
             $("#ContentPlaceHolder1_wucOwner3_HomeCountryCodeDisplay").val($(this).val());
         });
         //PXP-2883
         $("#ContentPlaceHolder1_wucOwner4_HomePhoneCountryCode").change(function () {
             $("#ContentPlaceHolder1_wucOwner4_HomeCountryCodeDisplay").val($(this).val());
         });
         $("#ContentPlaceHolder1_wucOwner5_HomePhoneCountryCode").change(function () {
             $("#ContentPlaceHolder1_wucOwner5_HomeCountryCodeDisplay").val($(this).val());
         });
     });

</script>
<fieldset>
    <asp:HiddenField ID="hiddenOwnerId" runat="server"/>
    <asp:HiddenField ID="hiddenPanelID" runat="server"/>
    <legend>
        <asp:Label ID="lblTitle" runat="server" Text="" Enabled="true"></asp:Label></legend>
    <asp:Panel runat="server" ID="pnlOwn">
        <table width="100%">
            <tr>
                <td class="lblRight">
                    Prefix:</td>
                <td>
                    <asp:DropDownList ID="Prefix" runat="server" Width="100px">
                        <asp:ListItem>--Select--</asp:ListItem>
                        <asp:ListItem Value="Dr">Dr</asp:ListItem>
                        <asp:ListItem Value="Mr">Mr</asp:ListItem>
                        <asp:ListItem Value="Mrs">Mrs</asp:ListItem>
                        <asp:ListItem Value="Ms">Ms</asp:ListItem>             
                   </asp:DropDownList>
                </td>
                <td class="lblRight">
                    First:</td>
                <td>
                    <asp:TextBox ID="FirstName" runat="server" Width="214px" MaxLength="50"></asp:TextBox></td>
                <td class="lblRight">
                    Last:</td>
                <td>
                    <asp:TextBox ID="LastName" runat="server" Width="100px" MaxLength="50"></asp:TextBox><asp:TextBox
                        ID="OwnerID" runat="server" Visible="false"></asp:TextBox></td>
                <td class="lblRight">
                    Suffix:</td>
                  <td>
                    <asp:DropDownList ID="Suffix" runat="server" Width="100px">
                        <asp:ListItem>--Select--</asp:ListItem>
                        <asp:ListItem Value="Dc">Dc</asp:ListItem>
                        <asp:ListItem Value="Dds">Dds</asp:ListItem>
                        <asp:ListItem Value="Esq">Esq</asp:ListItem>
                        <asp:ListItem Value="II">II</asp:ListItem>
                        <asp:ListItem Value="III">III</asp:ListItem>
                        <asp:ListItem Value="IV">IV</asp:ListItem>
                        <asp:ListItem Value="Jr">Jr</asp:ListItem>
                        <asp:ListItem Value="Phd">Phd</asp:ListItem>  
                        <asp:ListItem Value="Sr">Sr</asp:ListItem>              
                   </asp:DropDownList>
                </td>
                
                <td class="lblRight">
                    Middle:</td>
                <td>
                    <asp:TextBox ID="MiddleName" runat="server" Width="100px" MaxLength="50"></asp:TextBox></td>
                <td class="lblRight">
                    Title:</td>
                <td class="auto-style1">
                    <asp:DropDownList ID="Title" runat="server" Width="100px">
                        <asp:ListItem>--Select--</asp:ListItem>
                        <asp:ListItem>Owner</asp:ListItem>
                        <asp:ListItem>President</asp:ListItem>
                        <asp:ListItem>V. President</asp:ListItem>
                        <asp:ListItem>Treasurer</asp:ListItem>
                        <asp:ListItem>Secretary</asp:ListItem>
                        <asp:ListItem>Partner</asp:ListItem>
                        <asp:ListItem>Member</asp:ListItem>
                        <%--PXP-2883--%>
                        <asp:ListItem>Director</asp:ListItem>
                        <asp:ListItem>CEO</asp:ListItem>
                        <asp:ListItem>COO</asp:ListItem>
                        <asp:ListItem>CFO</asp:ListItem>
                        <asp:ListItem>Controller</asp:ListItem>
                        <asp:ListItem>Shareholder</asp:ListItem>
                    </asp:DropDownList>
                </td>
               
            </tr>
            <tr>
                 <td class="lblRight">
                    Owner %:</td>
                <td>
                    <ig:WebPercentEditor MaxValue="100" ID="PercentOwnership" runat="server" Width="100px"
                        ValueText="0">
                    </ig:WebPercentEditor>
                </td>
                <td class="lblRight">
                    Phone:</td>
                <td>
                    <asp:DropDownList ID="HomePhoneCountryCode" runat="server" Width="45px">
                    </asp:DropDownList>
                    <asp:TextBox ID="HomeCountryCodeDisplay" runat="server" Width="37px"></asp:TextBox>
                    <ig:WebMaskEditor ID="HomePhone" runat="server" InputMask="############" PromptChar=" " ShowMaskOnFocus="False" Width="78px">
                    </ig:WebMaskEditor>
                    <ig:WebMaskEditor ID="HomePhoneExt" runat="server" InputMask="000000" CssClass="text igte_Edit" Width="42px" ShowMaskOnFocus="False"></ig:WebMaskEditor>                                        

                </td>
                <td class="lblRight">
                    Address:</td>
                <td>
                    <asp:TextBox ID="Address1" runat="server" Width="100px" MaxLength="100"></asp:TextBox></td>
                <td class="lblRight">Line2:</td>
                <td> <asp:TextBox ID="Address2" runat="server" Width="100px" MaxLength="100"></asp:TextBox></td>
                <td class="lblRight">
                    City:</td>
                <td>
                    <asp:TextBox ID="City" runat="server" Width="100px"></asp:TextBox></td>
                <td class="lblRight">
                    Country</td>
                <td class="auto-style1">
                    <asp:DropDownList ID="Country" runat="server" Width="100px">
                    </asp:DropDownList>
                </td>
               
            </tr>
            <tr>
                 <td class="lblRight">
                    State/Province:</td>
                <td>
                    <asp:DropDownList ID="State" runat="server" Width="100px">
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
                    </asp:DropDownList> <asp:TextBox ID="Province" Width="100px" runat="server"  Style="display:none"></asp:TextBox>

                </td>
                <td class="lblRight">
                    Postal Code:</td>
                <td>
                    <asp:TextBox ID="Zip" runat="server" MaxLength="50" Width="214px"></asp:TextBox>
                    
                </td>
                <td class="lblRight">
                    SSN:</td>
                <td>
                    <ig:WebMaskEditor ID="SSN" runat="server" InputMask="###-##-####" Width="100px" AutoPostBack="true"
                        OnValueChange="SSN_ValueChange">
                    </ig:WebMaskEditor>
                </td>
                <td class="lblRight">
                    DOB:</td>
                <td>
                    <ig:WebDatePicker ID="DOB" runat="server" EnableAppStyling="False" NullDateLabel="" NullValueRepresentation="Null" Width="100px">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" />
                    </ig:WebDatePicker>
                </td>
                <td class="lblRight">
                    DL #:</td>
                <td>
                    <asp:TextBox ID="DriversLicense" runat="server" Width="100px" MaxLength="50"></asp:TextBox></td>
                <td class="lblRight">
                    DL State:</td>
                <td>
                    <asp:DropDownList ID="DriversLicenseState" runat="server" Width="100px">
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
                    </asp:DropDownList>
                </td>
               
                
                <td class="lblRight">
                </td>
                <td>
                    &nbsp;</td>
            </tr>
                <tr>
                     <td class="lblRight">
                    Expiration:</td>
                    <td class="auto-style1">
                    <ig:WebDatePicker ID="DriversLicenseExp" runat="server" NullDateLabel="" NullValueRepresentation="Null"
                        Width="100px" EnableAppStyling="False">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                            SlideOpenDuration="1" />
                    </ig:WebDatePicker>
                </td>
                <td class="lblRight">
                    Passport #</td>
                <td>
                    <asp:TextBox ID="PassportNumber" runat="server" Width="214px" MaxLength="40"></asp:TextBox>
                </td>
                <td class="lblRight">
                    ID Card:</td>
                <td>
                    <asp:TextBox ID="IDCardNumber" runat="server" Width="100px" MaxLength="40"></asp:TextBox>
                </td>
                <td class="lblRight">
                    ID Nationality:</td>
                <td>
                    <asp:DropDownList ID="IDNationality" runat="server" Width="100px">
                    </asp:DropDownList>
                <%--<td>
                    </td>--%>
                <td colspan="2">        
                        <asp:CheckBox ID="BeneficialOwner" runat="server" Text="Equity/Ownership" Enabled="false" />
                <td colspan="2">       
                        <asp:CheckBox ID="AuthorizedSignature" runat="server" Text="Controller" Enabled="false" />
                </td>
                <%--<td>
                   </td>--%>
                <%--<td class="auto-style1" colspan="2">
                    <asp:CheckBox ID="AuthorizedSignature" runat="server" Text="Actual Authorized Signature" />
                </td>--%>   
                <td>
                    &nbsp;</td> 
            </tr>
              <tr>
                  <td class="lblRight">
                    Email:</td>
                <td>
                    <asp:TextBox ID="Email" runat="server" Width="100px" MaxLength="40"></asp:TextBox>
                </td>
                  <td colspan="2">
                      <asp:CheckBox ID="CBRWaived" Enabled="false" runat="server" Text="CBR Waived"/>
                  </td>
                  <td colspan="2">       
                        <asp:CheckBox ID="PersonalGuarantor" runat="server" Text="Personal Guarantor" Enabled="true"/>
                </td>
                  </tr>
        </table>
    </asp:Panel>
</fieldset>
