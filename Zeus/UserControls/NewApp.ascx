<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_NewApp" CodeBehind="NewApp.ascx.cs" %>
<%@ Register Src="~/UserControls/wucTradeReference.ascx" TagName="wucTradeReference"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/wucOwner.ascx" TagName="wucOwner" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<script language="javascript" type="text/javascript">
    function CalTotalTransactionType_TextChanged(oEdit, newText, oEvent) {
        var edit1 = igedit_getById('<%=TinfoStoreFrontSwipedPercent.ClientID %>');
        var edit2 = igedit_getById('<%=TinfoInterntPercent.ClientID %>');
        var edit3 = igedit_getById('<%=TinfoMailOrderPercent.ClientID %>');
        var edit4 = igedit_getById('<%=TinfoTelephoneOrderPercent.ClientID %>');
        var edit5 = igedit_getById('<%=txtTotalSalesType.ClientID %>');

        var TinfoStoreFrontSwipedPercent;
        var TinfoInterntPercent;
        var TinfoMailOrderPercent;
        var TinfoTelephoneOrderPercent;

        if (edit1.getValue() == null)
            TinfoStoreFrontSwipedPercent = 0;
        else
            TinfoStoreFrontSwipedPercent = edit1.getValue();

        if (edit2.getValue() == null)
            TinfoInterntPercent = 0;
        else
            TinfoInterntPercent = edit2.getValue();

        if (edit3.getValue() == null)
            TinfoMailOrderPercent = 0;
        else
            TinfoMailOrderPercent = edit3.getValue();

        if (edit4.getValue() == null)
            TinfoTelephoneOrderPercent = 0;
        else
            TinfoTelephoneOrderPercent = edit4.getValue();

        edit5.setValue(TinfoStoreFrontSwipedPercent + TinfoInterntPercent + TinfoMailOrderPercent + TinfoTelephoneOrderPercent);
    }

    function CalTotalTransCompleted_TextChanged(oEdit, newText, oEvent) {
        var edit1 = igedit_getById('<% =TinfoElectronicDataCaptureSwipedPercent.ClientID %>');
        var edit2 = igedit_getById('<%=TinfoManualEntryWithImprintPercent.ClientID %>');
        var edit3 = igedit_getById('<%=TinfoManualEntryNoCardNoImprintPercent.ClientID %>');
        var edit4 = igedit_getById('<%=TinfoVoiceAuthPercent.ClientID %>');
        var edit5 = igedit_getById('<%=txtTotalTransCompleted.ClientID %>');

        var TinfoElectronicDataCaptureSwipedPercent;
        var TinfoManualEntryWithImprintPercent;
        var TinfoManualEntryNoCardNoImprintPercent;
        var TinfoVoiceAuthPercent;

        if (edit1.getValue() == null)
            TinfoElectronicDataCaptureSwipedPercent = 0;
        else
            TinfoElectronicDataCaptureSwipedPercent = edit1.getValue();

        if (edit2.getValue() == null)
            TinfoManualEntryWithImprintPercent = 0;
        else
            TinfoManualEntryWithImprintPercent = edit2.getValue();

        if (edit3.getValue() == null)
            TinfoManualEntryNoCardNoImprintPercent = 0;
        else
            TinfoManualEntryNoCardNoImprintPercent = edit3.getValue();

        if (edit4.getValue() == null)
            TinfoVoiceAuthPercent = 0;
        else
            TinfoVoiceAuthPercent = edit4.getValue();


        edit5.setValue(TinfoElectronicDataCaptureSwipedPercent + TinfoManualEntryWithImprintPercent + TinfoManualEntryNoCardNoImprintPercent + TinfoVoiceAuthPercent);
    }

    function ShowEquipment() {
        oWebDialogWindow2 = $find('<% =WebDialogWindow1.ClientID %>'); oWebDialogWindow2.set_windowState($IG.DialogWindowState.Normal);
        return false;
    }

    function ClientValidate(source, args) {
        var message = "";
        var perOwner1, perOwner, info, info1;
        var inter = false, percentFee = false;
        var txtVisaDiscountPercent = "", txtVisaTransFee = "", txtMCDiscountPercent = "";
        var txtMCTransFee = "", txtDiscoverDiscountRate = "", txtDiscoverTransFee = "";
        var txtInterchangePerItem = "", txtInterchangePlus = "";
        var txt = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner0_PercentOwnership');
        var txt1 = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner1_PercentOwnership');
        var txt2 = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner2_PercentOwnership');
        var txt3 = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner3_PercentOwnership');

        if (document.getElementById('<%=lblError.ClientID %>') != null) {
            document.getElementById('<%=lblError.ClientID %>').innerText = "";
            document.getElementById('<%=lblError.ClientID %>').visible = false;
        }
        if (txt.value != null && txt.value != "") {
            perOwner1 = (txt.value.indexOf('%') != -1) ? txt.value.replace('%', ' ').trim() : txt.value.trim();
            perOwner = parseFloat(perOwner1);
        }
        if (txt1.value != null && txt1.value != "") {
            perOwner1 = (txt1.value.indexOf('%') != -1) ? txt1.value.replace('%', ' ').trim() : txt1.value.trim();
            perOwner += parseFloat(perOwner1);
        }
        if (txt2.value != null && txt2.value != "") {
            perOwner1 = (txt2.value.indexOf('%') != -1) ? txt2.value.replace('%', ' ').trim() : txt2.value.trim();
            perOwner += parseFloat(perOwner1);
        }
        if (txt3.value != null && txt3.value != "") {
            perOwner1 = (txt3.value.indexOf('%') != -1) ? txt3.value.replace('%', ' ').trim() : txt3.value.trim();
            perOwner += parseFloat(perOwner1);
        }

        var InterchangePlus = document.getElementById('<%= InterchangePlus.ClientID %>');
        var InterchangePerItem = document.getElementById('<%= InterchangePerItem.ClientID %>');
        var VisaDiscountPercent = document.getElementById('<%= VisaTransFee.ClientID %>');
        var VisaTransFee = document.getElementById('<%= VisaTransFee.ClientID %>');
        var MCDiscountPercent = document.getElementById('<%= MCDiscountPercent.ClientID %>');
        var MCTransFee = document.getElementById('<%= MCTransFee.ClientID %>');
        var DiscoverDiscountRate = document.getElementById('<%= DiscoverDiscountRate.ClientID %>');
        var DiscoverTransFee = document.getElementById('<%= DiscoverTransFee.ClientID %>');
        var TinfoInterntPercent = document.getElementById('<%= TinfoInterntPercent.ClientID %>');
        var BusinessWebsite = document.getElementById('<%= BusinessWebsite.ClientID %>');
        var txtTotalTransCompleted = document.getElementById('<%= txtTotalTransCompleted.ClientID %>');
        var txtTotalSalesType = document.getElementById('<%= txtTotalSalesType.ClientID %>');
        var TinfoTelephoneOrderPercent = document.getElementById('<%= TinfoTelephoneOrderPercent.ClientID %>');
        var TinfoMailOrderPercent = document.getElementById('<%= TinfoMailOrderPercent.ClientID %>');
        var TinfoInterntPercent = document.getElementById('<%= TinfoInterntPercent.ClientID %>');
        var Descriptor = document.getElementById('<%= Descriptor.ClientID %>');

        if (InterchangePlus.value.trim() != "" && InterchangePerItem.value.trim() != "") {
            txtInterchangePerItem = (InterchangePerItem.value.indexOf('$') != -1) ? InterchangePerItem.value.replace('$', ' ').trim() : InterchangePerItem.value.trim();
            txtInterchangePlus = (InterchangePlus.value.indexOf('%') != -1) ? InterchangePlus.value.replace('%', ' ').trim() : InterchangePlus.value.trim();
            if (parseFloat(txtInterchangePlus) != parseFloat("0.0") && parseFloat(txtInterchangePerItem) != parseFloat("0.0"))
                inter = true;
        }

        if (VisaDiscountPercent.value.trim() != "" && VisaTransFee.value.trim() != "" && MCDiscountPercent.value.trim() != "" && MCTransFee.value.trim() != "" && DiscoverDiscountRate.value.trim() != "" && DiscoverTransFee.value.trim() != "") {
            txtVisaDiscountPercent = (VisaDiscountPercent.value.indexOf('%') != -1) ? VisaDiscountPercent.value.replace('%', ' ').trim() : VisaDiscountPercent.value;
            txtVisaTransFee = (VisaTransFee.value.indexOf('$') != -1) ? VisaTransFee.value.replace('$', ' ').trim() : VisaTransFee.value;
            txtMCDiscountPercent = (MCDiscountPercent.value.indexOf('%') != -1) ? MCDiscountPercent.value.replace('%', ' ').trim() : MCDiscountPercent.value;
            txtMCTransFee = (MCTransFee.value.indexOf('$') != -1) ? MCTransFee.value.replace('$', ' ').trim() : MCTransFee.value;
            txtDiscoverDiscountRate = (DiscoverDiscountRate.value.indexOf('%') != -1) ? DiscoverDiscountRate.value.replace('%', ' ').trim() : DiscoverDiscountRate.value;
            txtDiscoverTransFee = (DiscoverTransFee.value.indexOf('$') != -1) ? DiscoverTransFee.value.replace('$', ' ').trim() : DiscoverTransFee.value;
            if (parseFloat(txtVisaDiscountPercent) != parseFloat("0.0") || parseFloat(txtVisaTransFee) != parseFloat("0.0") || parseFloat(txtMCDiscountPercent) != parseFloat("0.0") || parseFloat(txtMCTransFee) != parseFloat("0.0") || parseFloat(txtDiscoverDiscountRate) != parseFloat("0.0") || parseFloat(txtDiscoverTransFee) != parseFloat("0.0"))
                percentFee = true;
        }

        if (inter == percentFee)
            message = "Please enter discount rates or Interchange pass through";

        if (txtTotalTransCompleted.value.trim() != "")
            if (((txtTotalTransCompleted.value.indexOf('%') != -1) ? parseFloat(txtTotalTransCompleted.value.replace('%', ' ').trim()) : parseFloat(txtTotalTransCompleted.value.trim())) != parseFloat("100.0")) {
                message += (message.length > 0) ? "<br />" : "";
                message += "Transaction completed % must be 100.";
            }

        if (txtTotalSalesType.value.trim() != "")
            if (((txtTotalSalesType.value.indexOf('%') != -1) ? parseFloat(txtTotalSalesType.value.replace('%', ' ').trim()) : parseFloat(txtTotalSalesType.value.trim())) != parseFloat("100.0")) {
                message += (message.length > 0) ? "<br />" : "";
                message += "Transaction type % must be 100.";
            }

        if (TinfoInterntPercent.value.trim() != "") {
            var percent1 = (TinfoInterntPercent.value.indexOf('%') != -1) ? TinfoInterntPercent.value.replace('%', ' ').trim() : TinfoInterntPercent.value.trim();
            if ((parseFloat(percent1) > parseFloat("0.0")) && BusinessWebsite.value == "") {
                message += (message.length > 0) ? "<br />" : "";
                message += "Website is required.";
                // BusinessWebsite.style.border = "solid 1px Red";
            }
            //else
            //BusinessWebsite.style = document.getElementById('<%= Descriptor.ClientID %>').style;
        }

        if (TinfoTelephoneOrderPercent.value.trim() != "") {
            info1 = (TinfoTelephoneOrderPercent.value.indexOf('%') != -1) ? TinfoTelephoneOrderPercent.value.replace('%', ' ').trim() : TinfoTelephoneOrderPercent.value.trim();
            info = parseFloat(info1);
        }

        if (TinfoMailOrderPercent.value.trim() != "") {
            info1 = (TinfoMailOrderPercent.value.indexOf('%') != -1) ? TinfoMailOrderPercent.value.replace('%', ' ').trim() : TinfoMailOrderPercent.value.trim();
            info += parseFloat(info1);
        }

        if (TinfoInterntPercent.value.trim() != "") {
            info1 = (TinfoInterntPercent.value.indexOf('%') != -1) ? TinfoInterntPercent.value.replace('%', ' ').trim() : TinfoInterntPercent.value.trim();
            info += parseFloat(info1);
        }

        if (info >= parseFloat("50.0") && Descriptor.value == "") {
            message += (message.length > 0) ? "<br />" : "";
            message += "Descriptor is required.";
        }

        var LastName = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner0_LastName');
        var FirstName = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner0_FirstName');
        var Address0 = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner0_Address1');
        var City = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner0_City');
        var Zip = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner0_Zip');
        var DOB = igdrp_getComboById('ctl00_ContentPlaceHolder1_newApp_wucOwner0_DOB');
        var HomePhone = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner0_HomePhone');
        var State = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner0_State');
        var Zip = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner0_Zip');
        var Address1 = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner1_Address1');
        var Address2 = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner2_Address1');
        var Address3 = document.getElementById('ctl00_ContentPlaceHolder1_newApp_wucOwner3_Address1');

        if (LastName.value == "" || FirstName.value == "" || DOB.getText() == "" || Address0.value == "" ||
                City.value == "" || Zip.value == "" || HomePhone.value == "" || State.selectedIndex == 0) {
            message += (message.length > 0) ? "<br />" : "";
            message += "Please enter at least one owner information.";
        }

        if (perOwner < parseFloat("52.0")) {
            message += (message.length > 0) ? "<br />" : "";
            message += "A total of 52% or more of the Ownership is required.";
        }

        var reg = /.*\s*[Pp]\.?\s?[Oo]\.?\s[Bb][Oo][Xx].*/;
        if ((Address0.value != "" && reg.test(Address0.value)) || (Address1.value != "" && reg.test(Address1.value)) || (Address2.value != "" && reg.test(Address2.value)) || (Address3.value != "" && reg.test(Address3.value))) {
            message += (message.length > 0) ? "<br />" : "";
            message += "No P.O box numbers for address.";
        }

        var grd = document.getElementById('<%=grdEquipments.ClientID %>');
        if (grd == null && (document.getElementById('<%=Type.ClientID %>').value == "" && document.getElementById('<%=Model.ClientID %>').value == "")) {
            message += (message.length > 0) ? "<br />" : "";
            message += "Please select at least one equipment.";
        }

        if (message != "") {
            source.errormessage = message;
            args.IsValid = false;
        }
    }
</script>
<div class="boxNoBottomBorder">
    <div class="popupHdr">
        <div class="title">
            &nbsp; Add/Edit Application</div>
    </div>
</div>
<div class="tbrtools">
    <div class="tbrtoolsleft">
        <table>
            <tr>
                <td>
                    <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                        CausesValidation="false" AccessKey="S">
                        <Appearance>
                            <Image Url="../Images/disk_blue.png" />
                        </Appearance>
                    </igtxt:WebImageButton>
                </td>
                <td>
                    <igtxt:WebImageButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                        AccessKey="u">
                        <Appearance>
                            <Image Url="~/Images/add2.png" />
                        </Appearance>
                    </igtxt:WebImageButton>
                </td>
                <td>
                    <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                        CausesValidation="false" AccessKey="C">
                        <Appearance>
                            <Image Url="~/Images/disk_blue_error.png" />
                        </Appearance>
                    </igtxt:WebImageButton>
                </td>
                <td>
                    <igtxt:WebImageButton ID="btnPDF" runat="server" Text="PDF" OnClick="btnPDF_Click"
                        CausesValidation="False" AccessKey="P">
                        <Appearance>
                            <Image Url="~/Images/document_out.png" />
                        </Appearance>
                    </igtxt:WebImageButton>
                </td>
            </tr>
        </table>
    </div>
</div>
<div class="dialog">
    <asp:Label ID="lblError" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List">
    </asp:ValidationSummary>
    <asp:Panel runat="server" ID="pnlDetail">
        <asp:Panel ID="pnlProfile" runat="server">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="vertical-align: top;" colspan="2" align="center">
                        <fieldset class="dialog">
                            <legend>Business Information</legend>
                            <table border="0" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="8">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="BusinessDBAName"
                                            Display="None" ErrorMessage="DBA is required."></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="BusinessLegalName"
                                            Display="None" ErrorMessage="Legal name is required."></asp:RequiredFieldValidator><asp:CompareValidator
                                                ID="CompareValidator2" runat="server" ControlToValidate="MerchantAppTypeUID"
                                                Display="None" ErrorMessage="Please select a Bank." Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="BusinessAddress"
                                            Display="None" ErrorMessage="Address is required."></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="BusinessCity"
                                            Display="None" ErrorMessage="City is required."></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="BusinessState"
                                            Display="None" ErrorMessage="State is required." Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="BusinessZip"
                                            Display="None" ErrorMessage="Zipcode is required."></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="BusinessZip"
                                            Display="None" ErrorMessage="Invalid zip code" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="BusinessMailingState"
                                            Display="None" ErrorMessage="Mailing state is required." Operator="NotEqual"
                                            ValueToCompare="-1"></asp:CompareValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="BusinessMailingZip"
                                            Display="None" ErrorMessage="Mailing zipcode is required."></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="BusinessMailingZip"
                                            Display="None" ErrorMessage="Invalid mailing zip code" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="BusinessDBAPhone"
                                            Display="None" ErrorMessage="Phone is required."></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="BusinessTaxID"
                                            Display="None" ErrorMessage="Tax ID is required."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight" style="width: 10%">
                                        DBA:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="BusinessDBAName" runat="server" MaxLength="50" Width="130px"></asp:TextBox>
                                        <asp:Label runat="server" ID="lblErr" SkinID="Required"></asp:Label>
                                    </td>
                                    <td class="lblRight">
                                        Legal Name:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="BusinessLegalName" runat="server" MaxLength="50" Width="130px"></asp:TextBox>
                                        <asp:Label runat="server" ID="Label1" SkinID="Required"></asp:Label>
                                    </td>
                                    <td class="lblRight">
                                        Bank:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="135px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        Status:
                                    </td>
                                    <td align="left">
                                        <asp:HiddenField Value="bf168eda-b741-40fc-9213-7fd83d35491e" ID="StatusUID" runat="server" />
                                        <asp:Label ID="ReportStatusGroup" runat="server" Width="130px" Text="New"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Front-End:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="AuthPlatformUID" runat="server" Width="135px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        Bus Addr:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="BusinessAddress" runat="server" MaxLength="50" Width="130px"></asp:TextBox>
                                        <asp:Label runat="server" ID="Label2" SkinID="Required"></asp:Label>
                                    </td>
                                    <td class="lblRight">
                                        City:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="BusinessCity" runat="server" MaxLength="50" Width="130px"></asp:TextBox>
                                        <asp:Label runat="server" ID="Label3" SkinID="Required"></asp:Label>
                                    </td>
                                    <td class="lblRight">
                                        State:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="BusinessState" runat="server" Width="45px">
                                            <asp:ListItem Value="-1">Select</asp:ListItem>
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
                                        <asp:Label runat="server" ID="Label5" SkinID="Required"></asp:Label>
                                        <span class="lblRight">Zip:</span>
                                        <asp:TextBox ID="BusinessZip" runat="server" MaxLength="50" Width="45px"></asp:TextBox>
                                        <asp:Label runat="server" ID="Label4" SkinID="Required"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        <asp:Label runat="server" ID="lbltext" Width="85px" Text="Back-End:" Enabled="true"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="SettlePlatformUID" runat="server" Width="135px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        Mail Addr:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="BusinessMailingAddress" runat="server" MaxLength="50" Width="130px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">
                                        City:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="BusinessMailingCity" runat="server" MaxLength="50" Width="130px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">
                                        State:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="BusinessMailingState" runat="server" Width="45px">
                                            <asp:ListItem Value="-1">Select</asp:ListItem>
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
                                        <asp:Label runat="server" ID="Label21" SkinID="Required"></asp:Label>
                                        <span class="lblRight">Zip:</span>
                                        <asp:TextBox ID="BusinessMailingZip" runat="server" MaxLength="50" Width="45px"></asp:TextBox>
                                        <asp:Label runat="server" ID="Label6" SkinID="Required"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        DBA Phone:
                                    </td>
                                    <td align="left">
                                        <ig:WebMaskEditor ID="BusinessDBAPhone" runat="server" InputMask="##############################" PromptChar=' '  Width="130px" ShowMaskOnFocus="False">
                                        </ig:WebMaskEditor>
                                        <asp:Label runat="server" ID="Label7" SkinID="Required"></asp:Label>
                                    </td>
                                    <td class="lblRight">
                                        TaxID:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="BusinessTaxID" runat="server" MaxLength="10" Width="130px"></asp:TextBox>
                                        <asp:Label runat="server" ID="Label8" SkinID="Required"></asp:Label>
                                    </td>
                                    <td class="lblRight">
                                        Descriptor:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Descriptor" runat="server" MaxLength="21" Width="130px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">
                                        Reserve %:
                                    </td>
                                    <td align="left">
                                        <ig:WebPercentEditor ID="ReservePercent" runat="server" MaxValue="100" ValueText="0"
                                            Width="130px">
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <%-- <td class="lblRight">
                                        Agent:</td>
                                    <td align="left">
                                        <asp:DropDownList ID="AgentUID" runat="server" Width="165px">
                                        </asp:DropDownList>
                                    </td>--%>
                                    <td colspan="4" align="left">
                                        <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="horizontal"
                                            IDWidth="80" DBAWidth="130" lblDBAWidth="90px" lblIDWidth="88px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        <asp:HiddenField ID="LeadID" runat="server" />
                                    </td>
                                    <td>
                                        <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="BusinessStructureUID"
                                            Display="None" ErrorMessage="Business type is required." Operator="NotEqual"
                                            ValueToCompare="-1"></asp:CompareValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="MerchantSells"
                                            Display="None" ErrorMessage="Merchant sells is required."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;" colspan="2" align="center">
                        <fieldset class="dialog">
                            <legend>Additional Information</legend>
                            <table border="0" cellspacing="0" width="100%">
                                <tr valign="top">
                                    <td class="lblRight" style="width: 10%;">
                                        Rtn Policy:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ReturnPoliciesUID" runat="server" Width="155px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        Application Type:
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ApplicationTypeUID" runat="server" Width="135px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        Business Type:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="BusinessStructureUID" runat="server" Width="155px">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="Label10" SkinID="Required"></asp:Label>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="lblRight">
                                        Source:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Source" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">
                                        Merchant Sells:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="MerchantSells" runat="server" Width="130px" Height="20px" MaxLength="50"></asp:TextBox>
                                        <asp:Label runat="server" ID="Label9" SkinID="Required"></asp:Label>
                                    </td>
                                    <td class="lblRight">
                                        Bus. Start Date:
                                    </td>
                                    <td align="left">
                                        <ig:WebDatePicker ID="BusinessStartDate" runat="server" NullDateLabel="" NullValueRepresentation="Null"
                                            Width="155px" EnableAppStyling="False">
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="lblRight">
                                        Website:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="BusinessWebsite" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">
                                        MCC:
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="SicCode" runat="server" Width="150px"></asp:Label>
                                    </td>
                                    <td class="lblRight">
                                        MCC Description:
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="SicCodeDesc" runat="server" Width="150px"></asp:Label>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="lblRight">
                                        Referral:
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="Referral" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <fieldset class="dialog">
                            <legend>Contact</legend>
                            <table cellspacing="2" width="100%" align="center">
                                <tr>
                                    <td class="lblRight" style="width: 15%">
                                        Contact Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessContact" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                        <asp:Label runat="server" ID="Label11" SkinID="Required"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="BusinessContact"
                                            Display="None" ErrorMessage="Contact name is required."></asp:RequiredFieldValidator>
                                    </td>
                                    <td class="lblRight" style="width: 15%;">
                                        Company Phone:
                                    </td>
                                    <td>
                                        <ig:WebMaskEditor ID="BusinessPhone" runat="server" InputMask="##############################" PromptChar=' '  Width="200px" ShowMaskOnFocus="False">
                                        </ig:WebMaskEditor>
                                        <asp:Label runat="server" ID="Label12" SkinID="Required"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="BusinessPhone"
                                            Display="None" ErrorMessage="Company phone is required."></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight" style="width: 15%">
                                        Title:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="BusinessContactTitle" runat="server" Width="205px">
                                            <asp:ListItem Value=" ">--Select--</asp:ListItem>
                                            <asp:ListItem>Owner</asp:ListItem>
                                            <asp:ListItem>President</asp:ListItem>
                                            <asp:ListItem>V.President</asp:ListItem>
                                            <asp:ListItem>Treasurer</asp:ListItem>
                                            <asp:ListItem>Secretary</asp:ListItem>
                                            <asp:ListItem>Partner</asp:ListItem>
                                            <asp:ListItem>Member</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        Fax:
                                    </td>
                                    <td>
                                        <ig:WebMaskEditor ID="BusinessFax" runat="server" InputMask="##############################" PromptChar=' '  Width="200px" ShowMaskOnFocus="False">
                                        </ig:WebMaskEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Access Level:
                                    </td>                             
                                    <td class="right">
                                        <asp:DropDownList ID="BusinessAccessLevel" runat="server" Width="205px">
                                            <asp:ListItem Value="0">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">All Access</asp:ListItem>
                                            <asp:ListItem Value="2">Information Only Access</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        Email:
                                    </td>
                                    <td colspan="1">
                                        <asp:TextBox ID="BusinessEmailAddress" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                    </td>       
                                    
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <asp:Panel runat="server" ID="pnlBank">
                            <fieldset class="dialog">
                                <legend>Bank Account</legend>
                                <table border="0" cellspacing="2" width="100%" align="center">
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight" style="width: 15%">
                                            Bank Name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="BankName" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                        </td>
                                        <td class="lblRight" style="width: 15%">
                                            Account Name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="AccountName" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">
                                            Routing Number:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="RoutingNumber" runat="server" MaxLength="9" Width="200px"></asp:TextBox>
                                            <asp:Label runat="server" ID="Label22" SkinID="Required"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="RoutingNumber"
                                                Display="None" ErrorMessage="Routing Number is required."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="RoutingNumber"
                                                Display="None" ErrorMessage="Invalid Routing Number" ValidationExpression="^(0[1-9]|1[0-2]|2[1-9]|3[0-2]|6[4-7])(\d{7})$"></asp:RegularExpressionValidator>
                                        </td>
                                        <td class="lblRight">
                                            Account Number:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="AccountNumber" runat="server" MaxLength="18" Width="200px"></asp:TextBox>
                                            <asp:Label runat="server" ID="Label23" SkinID="Required"></asp:Label>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="AccountNumber"
                                                Display="None" ErrorMessage="Account Number is required."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="AccountNumber"
                                                Display="None" ErrorMessage="Invalid Account Number" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlFees" runat="server" Height="" Width="">
            <table width="100%" border="0" cellpadding="0" cellspacing="2">
                <tr>
                    <td style="width: 50%; vertical-align: top">
                        <fieldset class="dialog">
                            <legend>Discount Rates</legend>
                            <table border="0" cellspacing="2" width="100%">
                                <tr>
                                    <td>
                                    </td>
                                    <td class="lblRight">
                                        <strong>Discount</strong>
                                    </td>
                                    <td class="lblRight">
                                        <strong>Trans Fee</strong>
                                    </td>
                                    <td>
                                    </td>
                                    <td class="lblRight">
                                        <strong>Discount</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Visa:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="VisaDiscountPercent" runat="server" ValueText="0"
                                            Width="60px">
                                        </ig:WebPercentEditor>
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="VisaTransFee" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                        Mid-Qual %:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="MidQualifyingRatePercent" runat="server"
                                            ValueText=".75" Width="60px">
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        MasterCard:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="MCDiscountPercent" runat="server" ValueText="0"
                                            Width="60px">
                                        </ig:WebPercentEditor>
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="MCTransFee" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                        Non-Qual %:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="NonQualifyingRatePercent" runat="server"
                                            ValueText="1.5" Width="60px">
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Discover:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="DiscoverDiscountRate" runat="server" ValueText="0"
                                            Width="60px">
                                        </ig:WebPercentEditor>
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="DiscoverTransFee" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Check Card:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="CheckCardDiscountRate" runat="server" MinDecimalPlaces="2"
                                            Width="60px">
                                        </ig:WebPercentEditor>
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="CheckCardTransFee" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td>
                                    </td>
                                    <td colspan="1" rowspan="1">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        AMEX:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="AMEXDiscountRate" runat="server" MinDecimalPlaces="2"
                                            ValueText="0" Width="60px">
                                        </ig:WebPercentEditor>
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="AMEXTransFee" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td>
                                    </td>
                                    <td colspan="1" rowspan="1">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        JCB:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="JCBDiscountRate" runat="server" ValueText="0"
                                            Width="60px">
                                        </ig:WebPercentEditor>
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="JCBTransFee" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="dialog">
                            <legend>Interchange Pass Through</legend>
                            <table border="0" cellspacing="2" width="100%">
                                <tr>
                                    <td class="lblRight">
                                        Interchange Plus:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="InterchangePlus" runat="server" ValueText="0"
                                            Width="60px">
                                        </ig:WebPercentEditor>
                                    </td>
                                    <td class="lblRight">
                                        Per Item:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="InterchangePerItem" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="dialog">
                            <legend>Gateway Fees</legend>
                            <table width="100%">
                                <tr>
                                    <td class="lblRight">
                                        Trans Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="GatewayTransFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                        Monthly Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="GatewayMonthlyFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                        Setup Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="GatewaySetupFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="dialog">
                            <legend>EBT</legend>
                            <table border="0" cellspacing="2" width="100%">
                                <tr>
                                    <td class="lblRight">
                                        Cash Back Max:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="EBTCashBackMax" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                        Trans. Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="EBTTransFee" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                    <td style="vertical-align: top">
                        <fieldset class="dialog">
                            <legend>Fixed Charges</legend>
                            <table cellspacing="2" width="100%">
                                <tr>
                                    <td class="lblRight">
                                        <strong>Processor Fees</strong>
                                    </td>
                                    <td class="lblRight">
                                        <strong>Amount</strong>
                                    </td>
                                    <td class="lblRight">
                                        <strong>Processor Fees</strong>
                                    </td>
                                    <td class="lblRight">
                                        <strong>Amount</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Chargeback Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="ChargeBackFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                        <asp:Label runat="server" ID="Label13" SkinID="Required"></asp:Label>
                                        <asp:CompareValidator ID="CompareValidator18" runat="server" ControlToValidate="ChargeBackFee"
                                            Display="None" ErrorMessage="Chargeback Fee is required." Operator="NotEqual"
                                            ValueToCompare="0"></asp:CompareValidator>
                                    </td>
                                    <td class="lblRight">
                                        ACH/Batch Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="ACHBatchFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Electronic AVS Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="AvsFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                        Statement Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="StatementFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Retrieval Request Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="RetrievalRequestFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                        <asp:Label runat="server" ID="Label14" SkinID="Required"></asp:Label>
                                        <asp:CompareValidator ID="CompareValidator17" runat="server" ControlToValidate="RetrievalRequestFee"
                                            Display="None" ErrorMessage="Retrieval Request Fee is required." Operator="NotEqual"
                                            ValueToCompare="0"></asp:CompareValidator>
                                    </td>
                                    <td class="lblRight">
                                        Merchant Club Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="MerchantClub" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Monthly Min Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="MonthlyMinimumFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                        Wireless Trans. Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="WirelessPerTrans" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Wireless Data Monthly Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="WirelessServiceFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                        Application Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="ApplicationFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Annual Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="AnnualFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                        Voice Auth Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="VoiceAuthFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                        <asp:Label runat="server" ID="Label15" SkinID="Required"></asp:Label>
                                        <asp:CompareValidator ID="CompareValidator16" runat="server" ControlToValidate="VoiceAuthFee"
                                            Display="None" ErrorMessage="Voice Auth Fee is required." Operator="NotEqual"
                                            ValueToCompare="0"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Return Item Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="ReturnItemFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                        <asp:Label runat="server" ID="Label16" SkinID="Required"></asp:Label>
                                        <asp:CompareValidator ID="CompareValidator15" runat="server" ControlToValidate="ReturnItemFee"
                                            Display="None" ErrorMessage="Return Item Fee is required." Operator="NotEqual"
                                            ValueToCompare="0"></asp:CompareValidator>
                                    </td>
                                    <td class="lblRight">
                                        AVS Voice Auth Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="AvsVoiceAuthFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                        <asp:Label runat="server" ID="Label17" SkinID="Required"></asp:Label>
                                        <asp:CompareValidator ID="CompareValidator14" runat="server" ControlToValidate="AvsVoiceAuthFee"
                                            Display="None" ErrorMessage="AVS Voice Auth Fee is required." Operator="NotEqual"
                                            ValueToCompare="0"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        PCI Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="PCIFee" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="lblRight">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        1. Other Fee:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="OtherFeeName1" runat="server" MaxLength="50" Width="145px"></asp:TextBox>&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="OtherFeeAmount1" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        2. Other Fee:
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="OtherFeeName2" runat="server" MaxLength="50" Width="145px"></asp:TextBox>&nbsp;&nbsp;
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="OtherFeeAmount2" runat="server" ValueText="0" Width="50px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset class="dialog">
                            <legend>Debit</legend>
                            <table border="0" cellspacing="2" width="100%">
                                <tr>
                                    <td class="lblRight">
                                        Monthly Access Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="DebitMonthlyFee" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                        Cash Back:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="DebitCashBackMax" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Authorization Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="DebitTransFee" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                    <td class="lblRight">
                                        Access Fee:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="DebitAccessFee" runat="server" ValueText="0" Width="60px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <fieldset class="dialog">
                <legend>Transaction Information</legend>
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="lblCenter">
                            <b>Sales Information</b>
                        </td>
                        <td class="lblCenter">
                            <b>Transaction Type Percentage</b>
                        </td>
                        <td class="lblCenter">
                            <b>How is transaction completed?</b>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <table cellpadding="0" cellspacing="2" width="100%">
                                <tr>
                                    <td class="lblRight">
                                        Average Monthly VISA & MC Volume:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="TinfoAverageMonthlyVMCVolume" runat="server" ValueText="0"
                                            Width="70px">
                                        </ig:WebNumericEditor>
                                        <asp:Label runat="server" ID="Label18" SkinID="Required"></asp:Label>
                                        <asp:CompareValidator ID="CompareValidator13" runat="server" ControlToValidate="TinfoAverageMonthlyVMCVolume"
                                            Display="None" ErrorMessage="Average Monthly VISA & MC Volume is required." Operator="NotEqual"
                                            ValueToCompare="0"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Average VISA & MC Ticket:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="TinfoAverageVMCTicket" runat="server" ValueText="0" Width="70px">
                                        </ig:WebNumericEditor>
                                        <asp:Label runat="server" ID="Label19" SkinID="Required"></asp:Label>
                                        <asp:CompareValidator ID="CompareValidator12" runat="server" ControlToValidate="TinfoAverageVMCTicket"
                                            Display="None" ErrorMessage="Average VISA & MC Ticket is required." Operator="NotEqual"
                                            ValueToCompare="0"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Highest Ticket Amount:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="TinfoHighestTicketAmount" runat="server" ValueText="0"
                                            Width="70px">
                                        </ig:WebNumericEditor>
                                        <asp:Label runat="server" ID="Label20" SkinID="Required"></asp:Label>
                                        <asp:CompareValidator ID="CompareValidator11" runat="server" ControlToValidate="TinfoHighestTicketAmount"
                                            Display="None" ErrorMessage="Highest Ticket Amount is required." Operator="NotEqual"
                                            ValueToCompare="0"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Seasonal Highest Monthly Volume:
                                    </td>
                                    <td>
                                        <ig:WebNumericEditor ID="TinfoSesonalHighestVolume" runat="server" ValueText="0"
                                            Width="70px">
                                        </ig:WebNumericEditor>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left" valign="top">
                            <table cellpadding="0" cellspacing="2" width="100%">
                                <tr>
                                    <td class="lblRight">
                                        Store Front/Swiped:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoStoreFrontSwipedPercent" runat="server"
                                            ValueText="0.0000" Width="70px">
                                            <ClientEvents TextChanged="CalTotalTransactionType_TextChanged" />
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Internet:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoInterntPercent" runat="server" ValueText="0"
                                            Width="70px">
                                            <ClientEvents TextChanged="CalTotalTransactionType_TextChanged" />
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Mail Order:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoMailOrderPercent" runat="server" ValueText="0"
                                            Width="70px">
                                            <ClientEvents TextChanged="CalTotalTransactionType_TextChanged" />
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Telephone Order:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoTelephoneOrderPercent" runat="server"
                                            ValueText="0" Width="70px">
                                            <ClientEvents TextChanged="CalTotalTransactionType_TextChanged" />
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        <b>Total:</b>
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor ID="txtTotalSalesType" runat="server" Enabled="False" ValueText="0"
                                            Width="70px">
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left" valign="top">
                            <table cellpadding="0" cellspacing="2" width="100%">
                                <tr>
                                    <td class="lblRight">
                                        Electronic data capture (swiped):
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoElectronicDataCaptureSwipedPercent"
                                            runat="server" ValueText="0" Width="70px">
                                            <ClientEvents TextChanged="CalTotalTransCompleted_TextChanged" />
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Manual entry with Impr:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoManualEntryWithImprintPercent" runat="server"
                                            ValueText="0" Width="70px">
                                            <ClientEvents TextChanged="CalTotalTransCompleted_TextChanged" />
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Manual entry, no card present, no imprint:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoManualEntryNoCardNoImprintPercent" runat="server"
                                            ValueText="0" Width="70px">
                                            <ClientEvents TextChanged="CalTotalTransCompleted_TextChanged" />
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Voice Authorization and Capture:
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoVoiceAuthPercent" runat="server" ValueText="0"
                                            Width="70px">
                                            <ClientEvents TextChanged="CalTotalTransCompleted_TextChanged" />
                                        </ig:WebPercentEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        <b>Total:</b>
                                    </td>
                                    <td>
                                        <ig:WebPercentEditor ID="txtTotalTransCompleted" runat="server" Enabled="False" ValueText="0"
                                            Width="70px">
                                        </ig:WebPercentEditor>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" Display="none" ErrorMessage=""
                                            ClientValidationFunction="ClientValidate"></asp:CustomValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
        <asp:Panel ID="pnlOwners" runat="server" Height="" Width="">
            <table>
                <tr>
                    <td align="left">
                        <h6 style="color: Red; vertical-align: text-bottom;">
                            NOTE: 52% of Ownership is required. No P.O. boxes for address.</h6>
                        <uc2:wucOwner ID="wucOwner0" runat="server" SetTitle="Owner1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc2:wucOwner ID="wucOwner1" runat="server" SetTitle="Owner2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc2:wucOwner ID="wucOwner2" runat="server" SetTitle="Owner3" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc2:wucOwner ID="wucOwner3" runat="server" SetTitle="Owner4" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc3:wucTradeReference ID="WucTradeReference0" runat="server" SetTitle="Trade Reference1" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc3:wucTradeReference ID="WucTradeReference1" runat="server" SetTitle="Trade Reference2" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc3:wucTradeReference ID="WucTradeReference2" runat="server" SetTitle="Trade Reference3" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlEquip" runat="server" Height="" Width="">
            <fieldset class="dialog">
                <legend>Equipments</legend>
                <table>
                    <tr>
                        <td class="lblRight">
                            Type:
                        </td>
                        <td>
                            <asp:TextBox ID="Type" ReadOnly="true" runat="server"></asp:TextBox>
                            <asp:HiddenField ID="ItemUID" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnLookup" runat="server" Text="Lookup Equipment" CausesValidation="false"
                                Width="130px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            Maker:
                        </td>
                        <td>
                            <asp:TextBox ID="Maker" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            Model:
                        </td>
                        <td>
                            <asp:TextBox ID="Model" ReadOnly="true" runat="server"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="pnlEquipment" runat="server">
                    <asp:GridView ID="grdEquipments" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                        Font-Names="Verdana" Font-Size="X-Small">
                        <PagerStyle CssClass="pgr" />
                        <AlternatingRowStyle CssClass="alt" />
                        <FooterStyle CssClass="footer" />
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                        <Columns>
                            <asp:BoundField DataField="TerminalNumber" HeaderText="Terminal Number" Visible="false" />
                            <asp:BoundField DataField="EquipmentType" HeaderText="Type" />
                            <asp:BoundField DataField="EquipmentMaker" HeaderText="Maker" />
                            <asp:BoundField DataField="Model" HeaderText="Model" />
                            <asp:BoundField DataField="TerminalStatusName" HeaderText="TerminalStatusName" />
                            <asp:BoundField DataField="DateShipped" HeaderText="Date Shipped" />
                            <asp:BoundField DataField="ShippingTrackingNumber" HeaderText="Tracking Number" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="pnlNoEquipment" runat="server">
                    &nbsp;<asp:Label runat="server" Text="no data.." ID="lblEuipment" Visible="true"
                        Font-Size="X-small"></asp:Label>
                </asp:Panel>
            </fieldset>
        </asp:Panel>
        <asp:Panel ID="special" runat="server">
            <fieldset>
                <legend>Special Requests</legend>
                <asp:TextBox ID="Notes" runat="server" TextMode="multiLine" Rows="4" Width="300px"></asp:TextBox>
            </fieldset>
        </asp:Panel>
        <br />
    </asp:Panel>
    <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="500px" Width="600px"
        InitialLocation="Centered" Modal="True" WindowState="Hidden" MaintainLocationOnScroll="True">
        <ContentPane>
            <Template>
                <fieldset>
                    <legend>Equipment Search</legend>
                    <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                        <table>
                            <tr>
                                <td class="lblRight">
                                    Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cboType" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td class="lblRight">
                                    Maker:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cboMaker" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td class="lblRight">
                                    Model:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtModel" runat="server" EnableViewState="False" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <asp:Button ID="btnSearch" runat="server" CausesValidation="False" OnClick="btnSearch_Click"
                                Text="Search" />
                            <asp:Button ID="btnReset" runat="server" CausesValidation="False" OnClick="btnReset_Click"
                                Text="Reset" /></center>
                    </asp:Panel>
                </fieldset>
                <fieldset>
                    <legend>Search Results</legend>
                    <asp:Panel ID="pnlgrdSearch" runat="server" Height="" Width="">
                        <asp:GridView ID="grdSearch" runat="server" CssClass="mGrid" AutoGenerateColumns="false"
                            Font-Size="X-Small" Font-Names="verdana" OnRowCommand="grdSearch_Rowcommand"
                            DataKeyNames="UID">
                            <FooterStyle CssClass="footer" />
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <Columns>
                                <asp:BoundField DataField="UID" Visible="false"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnSelect" Text="Select" runat="server" CommandName="Select"
                                            CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="EquipmentType" HeaderText="Type"></asp:BoundField>
                                <asp:BoundField DataField="EquipmentMaker" HeaderText="Maker"></asp:BoundField>
                                <asp:BoundField DataField="Model" HeaderText="Model"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="">
                        No data...
                    </asp:Panel>
                </fieldset>
            </Template>
        </ContentPane>
        <Header CaptionText="Equipment Lookup">
        </Header>
    </ig:WebDialogWindow>
</div>
