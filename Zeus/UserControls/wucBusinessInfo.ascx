<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucBusinessInfo" CodeBehind="wucBusinessInfo.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>

<style type="text/css">
    input, select, textarea {
        box-sizing: border-box;
        -moz-box-sizing: border-box;
        -webkit-box-sizing: border-box;
    }

    /*Niranjan: PXP-3285 Error handler page is coming while adding ZID*/
    #dvloading {
        display: none;
        position: fixed;
        left: 0px;
        top: 0px;
        width: 100%;
        height: 100%;
        z-index: 9999;
        background: url('../Images/loading.gif') 50% 50% no-repeat rgb(0, 0, 0);
        opacity: .5;
        background-size: 50px 50px;
    }
</style>

<script type="text/javascript">
    $(document).ready(function () {
        Showgateway($('#ContentPlaceHolder1_WucBusinessInfo1_Brand'));

        /*Niranjan: PXP-3285 Error handler page is coming while adding ZID*/
        $('#ContentPlaceHolder1_WucBusinessInfo1_OfficeID').change(function () {
            $('#dvloading').show();
        });
        $('#ContentPlaceHolder1_WucBusinessInfo1_MerchantAppTypeUID').change(function () {
            $('#dvloading').show();
        });
    });

    function Showgateway(dropdown) {
        var element = $(dropdown);

        if (element.val() == 2) {

            $('#ContentPlaceHolder1_Gatewayonly').css("display", "inline");
            $('#ContentPlaceHolder1_lblGatewayonly').css("display", "inline");
            $('#ContentPlaceHolder1_BankIBAN').css("display", "inline");
            $('#ContentPlaceHolder1_BankSwiftID').css("display", "inline");
            $('#ContentPlaceHolder1_lblBankIBAN').css("display", "inline");
            $('#ContentPlaceHolder1_lblBankSwiftID').css("display", "inline");
            $('#ContentPlaceHolder1_lblWireAccount').css("display", "inline");
            $('#ContentPlaceHolder1_lblBankTransitNumber').css("display", "inline");
            $('#ContentPlaceHolder1_BankCode').css("display", "inline");
            $('#ContentPlaceHolder1_BankBranchCode').css("display", "inline");



        }

        else {
            $('#ContentPlaceHolder1_Gatewayonly').css("display", "none");
            $('#ContentPlaceHolder1_lblGatewayonly').css("display", "none");
            $('#ContentPlaceHolder1_BankIBAN').css("display", "none");
            $('#ContentPlaceHolder1_BankSwiftID').css("display", "none");
            $('#ContentPlaceHolder1_lblBankIBAN').css("display", "none");
            $('#ContentPlaceHolder1_lblBankSwiftID').css("display", "none");
            $('#ContentPlaceHolder1_lblWireAccount').css("display", "none");
            $('#ContentPlaceHolder1_lblBankTransitNumber').css("display", "none");
            $('#ContentPlaceHolder1_BankCode').css("display", "none");
            $('#ContentPlaceHolder1_BankBranchCode').css("display", "none");


        }
    }

    function ShowMerchantClosureCodes() {

        var status = '';

        if ($("#<%=StatusUID.ClientID %>").is(":visible"))
            status = $("#<%=StatusUID.ClientID %>").val().toUpperCase();
        else
            status = $("#<%=ACHStatusUID.ClientID %>").val().toUpperCase();

        var isdisplay = 'none'; isvisible = 'none';

        //if (status == '1DB379A0-8F4C-4B9D-ACFC-092E6711DB88' || status == 'DF56BE69-1C2D-4465-BCDB-BD2CE97566C4') //Cancelled or Pending Cancellation
        if (status == '1DB379A0-8F4C-4B9D-ACFC-092E6711DB88')  // ***PXP 1261 : remove closur code & ETF validation MS_RETENTION-PENDING-CANCELLATION status
        {
            isdisplay = 'block';

            //if (status == 'DF56BE69-1C2D-4465-BCDB-BD2CE97566C4') {
            //    isvisible = 'block';
            //}
        }

        $("#<%=lblMerchantClosureCodeUID.ClientID %>").css("display", isdisplay);
        $("#<%=MerchantClosureCodeUID.ClientID %>").css("display", isdisplay);
        $("#<%=ETF.ClientID %>").css("display", isdisplay);
        $("#<%=ETFAssessed.ClientID %>").css("display", isdisplay);
        $("#<%=lblETF.ClientID %>").css("display", isdisplay);
        $("#<%=CancelDate.ClientID %>").css("display", isvisible);
        $("#<%=lblCancel.ClientID %>").css("display", isvisible);
        $("#<%=CancellationDate.ClientID %>").css("display", isvisible);
    }

    $(document).ready(function () {            

        //Trigger this Jquery function only after you define the function. This should be below the change function.
        $("#<%=Brand.ClientID %>").trigger("change");

        $("#<%=BusinessCountry.ClientID %>").ready(function () {
            var Country = $("#<%=BusinessCountry.ClientID %>").val();
            if (Country != "US") {
                $("#<%=BusinessState.ClientID %>").css("display", "none");
                $("#<%=BusinessProvince.ClientID %>").css("display", "inline");
            }

            if (Country == "US" || Country == "") {
                $("#<%=BusinessState.ClientID %>").css("display", "inline");
                $("#<%=BusinessProvince.ClientID %>").css("display", "none");
            }

        });

        $("#<%=BusinessMailingCountry.ClientID %>").ready(function () {
            var MailingCountry = $("#<%=BusinessMailingCountry.ClientID %>").val();
            if (MailingCountry != "US") {
                $("#<%=BusinessMailingState.ClientID %>").css("display", "none");
                $("#<%=BusinessMailingProvince.ClientID %>").css("display", "inline");
            }

            if (MailingCountry == "US" || MailingCountry == "") {
                $("#<%=BusinessMailingState.ClientID %>").css("display", "inline");
                $("#<%=BusinessMailingProvince.ClientID %>").css("display", "none");
            }

        });
        $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessDBAPhoneCountryCode").change(function () {
            $("#ContentPlaceHolder1_WucBusinessInfo1_DBACountryCodeDisplay").val($(this).val());
        });

        $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessFaxCountryCode").change(function () {
            $("#ContentPlaceHolder1_WucBusinessInfo1_FaxCountryCodeDisplay").val($(this).val());
        });

       
    });
    $("#ContentPlaceHolder1_WucBusinessInfo1_MerchantAppTypeUID").ready(function () {
        var MerchantAppTypeUID = $("#ContentPlaceHolder1_WucBusinessInfo1_MerchantAppTypeUID").val();
        var OfficeID = $("#ContentPlaceHolder1_WucBusinessInfo1_OfficeID").val();
        //code changes doen for PXP-10225 by koshlendra
        //Bug Fix PXP-19281 'Advertising, Sales and Delivery Section' section on Profile page is not displayed for CFG Citzen on Zeus
        if ((MerchantAppTypeUID.toUpperCase() == "5AE0C790-F81B-4612-BC69-BB2DF753BC0F" || MerchantAppTypeUID.toUpperCase() == "3CAF93C7-D583-4401-99ED-46484A212C34" || MerchantAppTypeUID.toUpperCase() == "5779AFDF-CBED-40EE-B1C0-C0BF6B41B6F3") && OfficeID == "1") {
            $("#ContentPlaceHolder1_trRtnPolicy").css("display", "none");
            $("#ContentPlaceHolder1_pnlAdvertisingSalesDelivery").css("display", "inline");
        }
        else {
            $("#ContentPlaceHolder1_pnlAdvertisingSalesDelivery").css("display", "none");
        }

    });

</script>
<script type="text/javascript" language="javascript">
    function CheckNumeric() {
        var key;
        key = event.which ? event.which : event.keyCode;
        if ((key >= 48 && key <= 57) || key == 13) {
            event.returnValue = true;
        }
        else {
            alert("Please enter Numeric only");
            event.returnValue = false;
        }
    }
</script>
<%--Niranjan: PXP-3285 Error handler page is coming while adding ZID--%>
<div id="dvloading">
</div>
<contenttemplate>
        <table width="100%">
            <tr>
                <td>
                    <fieldset>
                        <legend>Business Information</legend>
                        <asp:Panel ID="pnlGeneralInfo" runat="server" Height="" Width="">
                            <table border="0">
                                <tr>
                                    <td class="lblRight">
                                        <asp:Label ID="lblACHStatus" runat="server" Style="display: none;" Text="ACH Status:"></asp:Label>
                                        <asp:Label ID="lblCCStatus" runat="server" Style="display: inline;" Text="Status:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ACHStatusUID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="StatusUID_SelectedIndexChanged" Width="125px" TabIndex="1" style="display: none;">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="StatusUID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="StatusUID_SelectedIndexChanged" Width="125px" TabIndex="1" style="display: inline;">
                                        </asp:DropDownList>
                                    </td>
                                    <div runat="server" id="ETF" style="display: none;">
                                        <td class="lblRight">
                                            <asp:Label ID="lblETF" runat="server" Style="display: none;" Text="ETF Assessed:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ETFAssessed" runat="server" Width="214px" Style="display: none;" TabIndex="100">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="W">ETF Waived</asp:ListItem>
                                                <asp:ListItem Value="A">ETF Assessed</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </div>
                                    <td class="lblRight">
                                        <asp:Label ID="lblMerchantClosureCodeUID" Width="125px" runat="server" Text="Closure Codes:" Style="display: none;"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="MerchantClosureCodeUID" runat="server" Width="125px" Style="display: none;" TabIndex="200" AutoPostBack="true" OnSelectedIndexChanged="MerchantClosureCodeUID_SelectedIndexChanged">
                                        </asp:DropDownList></td>
                                    <td class="lblRight">
                                        <asp:Label ID="lblRiskStatus" runat="server"  Text="Risk Status:" Width="125px" Style="display: none"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="RiskStatus" runat="server" Width="125px"  TabIndex="200" Style="display: none">
                                           <%-- <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">Eligible</asp:ListItem>
                                            <asp:ListItem Value="2">Not Eligible</asp:ListItem>--%>
                                        </asp:DropDownList></td>
                                    
                                </tr>
                                <tr>
                                    <td class="lblRight">Gateway:</td>
                                    <td>
                                <asp:DropDownList runat="server" ID="Brand" Enabled="false" Width="125px" TabIndex="5" onchange="Showgateway(this)">
                                            <asp:ListItem Value="3">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">3rd Party</asp:ListItem>
                                            <asp:ListItem Value="2">Netbanx</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                            <td class="lblRight">Office Location:</td>
                                    <td>
                                        <asp:DropDownList runat="server" Width="214px" TabIndex="105" AutoPostBack="true" ID="OfficeID" OnSelectedIndexChanged="OfficeID_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        OLE #:
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" Width="125px" TabIndex="205" ID="LegalEntityID">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight"><asp:Label ID="lblFMAID" runat="server" Text="FMA ID:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="FMAID" runat="server" Width="125px" ClientIDMode="Static" TabIndex="305" MaxLength="15" onKeyPress ="CheckNumeric()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">DBA:</td>
                                    <td>
                                        <asp:TextBox ID="BusinessDBAName" runat="server" MaxLength="75" Width="125px" tabindex="10"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Acq. Bank:</td>
                                    <td>    
                                        <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="214px" TabIndex="110" AutoPostBack="true" OnSelectedIndexChanged="MerchantAppTypeUID_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtBank" runat="server" MaxLength="50" Visible="false" Width="214px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Proc Currency:</td>
                                    <td>
                                        <asp:DropDownList ID="Currency" runat="server" Width="125px" TabIndex="210">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight"><span style="width: 125px; display: inline-block;">Days Hold/Arrears:</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="DelaysApproved" runat="server" Width="125px" Enabled="false" ReadOnly="true" TabIndex="114"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        <span style="width: 125px; display: inline-block;">MLE:</span>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessLegalName" runat="server" MaxLength="75" Width="125px" TabIndex="20"></asp:TextBox>
                                    </td>
                                    <td class="lblRight"><span style="width: 125px; display: inline-block;">DBA Phone:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="BusinessDBAPhoneCountryCode" runat="server" Width="45px">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="DBACountryCodeDisplay" runat="server" Width="36px"></asp:TextBox>                                                       
                                        <ig:WebMaskEditor ID="BusinessDBAPhone" runat="server" InputMask="############" CssClass="text igte_Edit" Width="78px" ShowMaskOnFocus="False"></ig:WebMaskEditor>
                                        <ig:WebMaskEditor ID="DBAPhoneExt" runat="server" InputMask="000000" CssClass="text igte_Edit" Width="42px" ShowMaskOnFocus="False"></ig:WebMaskEditor>                                        
                                    </td>
                                    <td class="lblRight">
                                        <span style="width: 125px; display: inline-block;">Back-End:</span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="SettlePlatformUID" runat="server" Width="125px" TabIndex="220">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        <span style="width: 125px; display: inline-block;">Reserve %:</span></td>
                                    <td>
                                        <ig:WebPercentEditor ID="ReservePercent" runat="server" Enabled="False" MaxValue="100" ValueText="0" Width="125px" TabIndex="310"></ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Tax Reg. #:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessTaxID" runat="server" MaxLength="50" Width="125px" 
                                           TabIndex="30"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Business Fax:</td>
                                    <td>
                                        <asp:DropDownList ID="BusinessFaxCountryCode" runat="server" Width="45px">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="FaxCountryCodeDisplay" runat="server" Width="36px"></asp:TextBox>                                                       
                                        <ig:WebMaskEditor ID="BusinessFax" runat="server" CssClass="text igte_Edit" Width="78px" ShowMaskOnFocus="False" InputMask="############" TabIndex="130"></ig:WebMaskEditor>
                                        <ig:WebMaskEditor ID="BusinessFaxExt" runat="server" InputMask="000000" CssClass="text igte_Edit" Width="42px" ShowMaskOnFocus="False"></ig:WebMaskEditor>                                        
                                    </td>
                                    <td class="lblRight">Back MID:</td>
                                    <td>
                                        <asp:TextBox ID="SettlePlatformMid" runat="server"  MaxLength="30"  Width="125px" TabIndex="230"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Upfront Reserve:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="UpfrontReserve" runat="server" Enabled="False" ValueText="0" Width="125px" TabIndex="320"></ig:WebNumericEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Addr 1:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessAddress" runat="server" MaxLength="50" Width="125px" TabIndex="40"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Mail Addr 1:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessMailingAddress" runat="server" MaxLength="50" Width="214px" TabIndex="140"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Front-End:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="AuthPlatformUID" runat="server" Width="125px" TabIndex="240">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">Release Method:</td>
                                    <td>
                                        <asp:DropDownList ID="ReleaseMethodUID" runat="server" Enabled="false" Width="125px" TabIndex="330">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Addr 2:</td>
                                    <td>
                                        <asp:TextBox ID="BusinessAddressLine2" runat="server" MaxLength="50" Width="125px" TabIndex="45"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Mail Addr 2: </td>
                                    <td>
                                        <asp:TextBox ID="BusinessMailingAddressLine2" runat="server" MaxLength="50" Width="214px" TabIndex="145"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Front MID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AuthPlatformMid" runat="server"  MaxLength="30"  Width="125px" TabIndex="250"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Discount Method:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DiscountMethod" runat="server" Enabled="false" Width="125px" TabIndex="340">
                                            <asp:ListItem Selected="true" Value="63b90eb6-4b2e-4b4f-a9f3-7bfd0a81963c">Monthly</asp:ListItem>
                                            <asp:ListItem Value="8398a86b-718e-4103-abd9-b60d2d4d14ce">Daily</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">City: </td>
                                    <td>
                                        <asp:TextBox ID="BusinessCity" runat="server" MaxLength="50" TabIndex="50" Width="125px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Mail City: </td>
                                    <td>
                                        <asp:TextBox ID="BusinessMailingCity" runat="server" MaxLength="50" TabIndex="150" Width="214px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">TID:</td>
                                    <td>
                                        <asp:TextBox ID="TID" runat="server" MaxLength="100" Width="125px" TabIndex="260"></asp:TextBox>
                                    </td>
                                     <td class="lblRight">Billing Method:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="BillingMethodUID" runat="server"  Width="125px" TabIndex="340">
                                        </asp:DropDownList>
                                    </td>
                                    </tr>
                                    <asp:Panel ID="Registered" runat="server" Visible="false">
                                        <tr>
                                        <td colspan="7" class="lblRight">MC High Risk Reg:
                                        </td>
                                        <td><asp:label ID="IsValHRReg" Text="Yes" runat="server"></asp:label>                                          
                                        </td>
                                        </tr>
                                    </asp:Panel>
                                     <asp:Panel ID="PnlMasterMRP" runat="server" Visible="false">
                                          <tr>                                         
                                            <td colspan="7" class="lblRight">Master MRP:
                                            </td>
                                            <td><asp:label ID="IsValMaster" Text="Yes" runat="server"></asp:label>                                          
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                <asp:Panel ID="VIRPRegistered" runat="server" Visible="false">
                                        <tr>
                                        <td colspan="7" class="lblRight">V High Risk Reg:
                                        </td>
                                        <td><asp:label ID="IsVIRPHRReg" Text="Yes" runat="server"></asp:label>                                          
                                        </td>
                                        </tr>
                                    </asp:Panel>
                                   <asp:Panel ID="PnlMasterVIRP" runat="server" Visible="false">
                                          <tr>                                         
                                            <td colspan="7" class="lblRight">Master VIRP:
                                            </td>
                                            <td><asp:label ID="IsValMasterVIRP" Text="Yes" runat="server"></asp:label>                                          
                                            </td>
                                        </tr>
                                    </asp:Panel>
                               
                                <tr>
                                    <td class="lblRight">Country:</td>
                                    <td>
                                        <asp:DropDownList ID="BusinessCountry" runat="server" AutoPostBack="False" TabIndex="60" Width="125px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">Mail Country:</td>
                                    <td >
                                        <asp:DropDownList ID="BusinessMailingCountry" runat="server" AutoPostBack="False" TabIndex="160" Width="214px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">PS Rep:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="FirstTeamRep" runat="server" Enabled="false" Width="125px" TabIndex="270"></asp:TextBox>
                                    </td>
                                    <asp:Panel ID="amexOptBlue" runat="server" Visible="false">
                                        <td class="lblRight">Amex OptBlue:
                                        </td>
                                        <td>Yes
                                    </td>
                                    </asp:Panel>
                                </tr>
                                <tr>
                                    <td class="lblRight">State/Region: </td>
                                    <td>
                                        <asp:DropDownList ID="BusinessState" runat="server" Height="16px" TabIndex="70" Width="125px">
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
                                        <asp:TextBox ID="BusinessProvince" runat="server" Style="display: none" Width="125px" TabIndex="75"></asp:TextBox>

                                    </td>
                                    <td class="lblRight">Mail State/Region:</td>
                                    <td>
                                        <asp:TextBox ID="BusinessMailingProvince" Width="214px" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:DropDownList ID="BusinessMailingState" runat="server" TabIndex="170" Width="214px">
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
                                        <asp:TextBox ID="TextBox1" Width="214px" runat="server" Style="display: none"></asp:TextBox>

                                    </td>
                                    <td class="lblRight">SS Rep:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="PrimaryContactUID" runat="server" Width="125px" TabIndex="280">
                                        </asp:DropDownList>
                                        </td>
                                    <asp:PlaceHolder runat="server" ID="phPaymentSchedule">
                                    <td class="lblRight">Payment Schedule:</td>
                                    <td>
                                        <asp:DropDownList runat="server" Width="125px"  ID="PaymentScheduleID"></asp:DropDownList>                                        
                                    </td>
                                    </asp:PlaceHolder>
                                </tr>
                                <tr>
                                    <td class="lblRight"></td>
                                    <td  Width="125px"></td>
                                    <td class="lblRight"></td>
                                    <td  Width="214px"></td>
                                    <td class="lblRight">SS QA Rep:</td>
                                    <td>
                                        <asp:DropDownList ID="SSQRep" runat="server" Width="125px" TabIndex="281">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Postal Code:</td>
                                    <td>
                                        <asp:TextBox ID="BusinessZip" runat="server" MaxLength="50" TabIndex="80" Width="125px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Mail Postal Code:</td>
                                    <td>
                                        <asp:TextBox ID="BusinessMailingZip" runat="server" MaxLength="50" TabIndex="180" Width="214px"></asp:TextBox>
                                    </td>
                                      <td class="lblRight" style="vertical-align:top;">
                                        Channel Sales Manager:
                                    </td>
                                    <td style="vertical-align:top;">
                                        <asp:TextBox ID="ChannelSalesManager" runat="server" Width="125px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    
                                    <asp:PlaceHolder runat="server" ID="phPaymentFrequency">
                                    <td class="lblRight">Payment Frequency</td>
                                    <td>
                                        <asp:DropDownList runat="server"  Width="125px"  ID="PaymentFrequencyID"></asp:DropDownList>

                                    </td>
                                    </asp:PlaceHolder>
                                </tr>
                                <tr>
                                   <asp:PlaceHolder runat="server" ID="phAssociationNumber">
                                       <td class="lblRight">Association Number:
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="None"
                                                ErrorMessage="Association Number is required." ControlToValidate="AssociationNumber"></asp:RequiredFieldValidator>
                                       </td>
                                       <td>
                                           <asp:TextBox ID="AssociationNumber" runat="server" Width="125px" MaxLength="6" Text="100001"></asp:TextBox>
                                           <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "AssociationNumber" ID="RegularExpressionValidatorAssociationNumber" ValidationExpression = "\d+" runat="server" ErrorMessage="Only number allowed."></asp:RegularExpressionValidator>
                                       </td>
                                       <td colspan="6">

                                       </td>
                                   </asp:PlaceHolder>
                                </tr>
                                <tr>
                                    <td colspan="4" style="vertical-align:top">
                                        <asp:Panel ID="AgentSelect" runat="server">
                                            <uc1:AgentSelector ID="wucAgentSelector" runat="server" DBAWidth="125" IDWidth="85" LayoutStyle="horizontal" lblDBAWidth="125" lblIDWidth="125" />
                                        </asp:Panel>
                                    </td>
                                    
                                    <div id="CancelDate" runat="server" style="display: none;">
                                        <td class="lblRight" style="vertical-align:top;">
                                            <asp:Label ID="lblCancel" runat="server" Style="display: none;" Text="Cancellation Date:" Width="125px"></asp:Label>
                                        </td>
                                        <td style="vertical-align:top;">
                                            <ig:WebDatePicker ID="CancellationDate" runat="server" EnableAppStyling="False" Width="125px" Style="display: none;"
                                                BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px" MaxValue="12/31/9999" TabIndex="290"><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                    SlideOpenDuration="1" /></ig:WebDatePicker>
                                        </td>
                                    </div>

                                    <td colspan="2">
                                        <div id ="DivOfficeAccess" runat ="Server" visible =" false">
                                         <fieldset style="height: 100px">
                                            <legend>Office Access</legend>
                                             <asp:Panel ID="pnlOfficeAccess" runat="server" Width="100%" Enabled="True">
                                            <asp:CheckBoxList ID="listOfficeAccess" runat="server" RepeatColumns="2" Width="100%">
                                          </asp:CheckBoxList>
                                      </asp:Panel>
                                 </fieldset>
                                            </div>
                                        
                                    </td>


                                </tr>
                            </table>
                        </asp:Panel>
                    </fieldset>
                </td>
            </tr>
        </table>    
    </contenttemplate>

