<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true"
    Inherits="frmMerchantProfile" Title="Merchant Profile" CodeBehind="frmMerchantProfile.aspx.cs" ValidateRequest="false" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucOwner.ascx" TagName="wucOwner" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/wucAccountGroups.ascx" TagName="wucAccountGroups" TagPrefix="uc8" %>
<%@ Register Src="../UserControls/wucMCClookup.ascx" TagName="wucMCClookup" TagPrefix="uc9" %>
<%@ Register Src="~/UserControls/wuConfirmDialog.ascx" TagName="wuConfirm" TagPrefix="uc3" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControls/wucServices.ascx" TagName="wucServices" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/wucSeasonalMonths.ascx" TagName="wucSeasonalMonths"
    TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/wucDescriptor.ascx" TagName="wucDescriptor" TagPrefix="uc3" %>
<%@ Register TagName="wucEquipment" TagPrefix="uc7" Src="~/UserControls/wucEquipment.ascx" %>
<%@ Register Src="~/UserControls/wucMerchantCategories.ascx" TagName="wucMerchantCategories"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/wucContact.ascx" TagName="wucContact" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/wucLeads.ascx" TagName="wucLeads" TagPrefix="uc99" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        // PXP-12436: Start - Rohit Thakur
        $(document).ready(function () {
            if ($('#ContentPlaceHolder1_WucBusinessInfo1_OfficeID').val() == "1") {
                $('#ContentPlaceHolder1_MCClookup_Label1').text("Tangible Trial");
            }
            else {
                $('#ContentPlaceHolder1_MCClookup_Label1').text("Nutra Trial");
            }
        });
        // PXP-12436: End - Rohit Thakur
        function CopyConfirmation_Click(oButton, oEvent) {
            var x = confirm("Are you sure to copy the merchant info? This action cannot be reversed! Are you sure?");
            if (!x) {
                oEvent.cancel = true;
            }
        }
        // Added by Chandra for PXP-7898
        function WebImageSave_Click(oButton, oEvent) {
            var MCCcode = $("#<%=MCClookup.FindControl("txtSicCode").ClientID %>");
            var isNutra = $("#<%=MCClookup.FindControl("IsNutraMerchant").ClientID %>");
            var oldStatus = $("#<%=hidStatus.ClientID %>");
            var newStatus = $("#<%=WucBusinessInfo1.FindControl("StatusUID").ClientID %>");

            if ($("#<%=WucBusinessInfo1.FindControl("ACHStatusUID").ClientID %>").is(":visible"))
            {
                newStatus = $("#<%=WucBusinessInfo1.FindControl("ACHStatusUID").ClientID %>");
            }

            if (oldStatus.val().toUpperCase() != newStatus.val().toUpperCase() && newStatus.val().toUpperCase() == "73FC4B27-98D4-40EA-B9FC-1370C564CB12") {
                if (MCCcode.val() == "5968" || MCCcode.val() == "5964" )  {
                    var conf = alert("Review Tangible Trial checkbox for Tangible merchants");
                }
            }
            //PXP-9051 RThakur
            var selectedNewStatus = $(newStatus).find('option:selected').val().toUpperCase();
            var isNewVerticalandMarketsValue = $("#<%=IsNewVerticalandMarkets.ClientID %>").val();
            var chkIsNewVertical = document.getElementById('<%= this.IsNewVertical.ClientID %>');
            var isIsNewVertical = false;
            if ($(chkIsNewVertical).prop("checked")) {
                isIsNewVertical = true;
            }
            if ((isNewVerticalandMarketsValue == "false") && isIsNewVertical && selectedNewStatus == "2FDDA5E4-E80A-4155-8CB2-D5200992FA81" && oldStatus.val().toUpperCase() != newStatus.val().toUpperCase()) {
                var confVerticals = confirm("<%=PaymentXP.BusinessObjects.Constant.NewVerticalandVerticalMarketsChecked%>");
                if (!confVerticals) {
                    oEvent.cancel = true;
                }
            }
            // PXP-9348 RThakur
            var hidCRMStatus = document.getElementById('<%= this.hiddenCrmStatus.ClientID %>').value;
            var isCRMAcceptTrans = document.getElementById('<%= this.hiddenAcceptTransaction.ClientID %>').value;
            if (oldStatus.val().toUpperCase() != newStatus.val().toUpperCase() && newStatus.val().toUpperCase() == "<%=PaymentXP.BusinessObjects.Constants.QUEUESTATUS_CU_RECEIVED.ToUpper()%>") {
                if (MCCcode.val() == "5968" && isNutra.is(':checked')) {
                    if (hidCRMStatus == "InActive") {
                        var conf = confirm("<%=PaymentXP.BusinessObjects.Constant.CrmTppInactive%>");
                        if (!conf) {
                            newStatus.val(oldStatus.val().toLowerCase());
                        }
                    }
                    if (hidCRMStatus.toUpperCase() == "NA" && isCRMAcceptTrans.toLowerCase() == "false") {
                        var conf = confirm("<%=PaymentXP.BusinessObjects.Constant.CrmTppNA%>");
                        if (!conf) {
                            newStatus.val(oldStatus.val().toLowerCase());
                        }
                    }
                }
            }
        }
    </script>
    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner">
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:Panel ID="pnlRedBanner" runat="server">
            <span class="ftrightred">This account is in collections</span>
        </asp:Panel>
        <asp:Panel ID="pnlYellowBanner" runat="server">
            <span class="ftrightYellow">This account is with agency</span>
        </asp:Panel>
        <asp:Panel ID="pnlOrangeBanner" runat="server">
            <span class="ftrightdarkorange">This account is in divert via Collections</span>
        </asp:Panel>
        <asp:Panel ID="pnlRiskDivertStatusBanner" runat="server">
            <span class="ftrightYellow">This account is in divert via Risk</span>
        </asp:Panel>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
                        <div class="tbrtools">
                            <div class="tbrtoolsleft">
                                <table>
                                    <tr>
                                        <asp:Panel ID="pnlTools" runat="server">
                                            <td>
                                                <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                                    AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                                    <Appearance>
                                                        <Image Url="~/Images/edit.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add" AccessKey="a"
                                                    OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                                    <Appearance>
                                                        <Image Url="~/Images/add2.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false" AccessKey="s"
                                                    CommandName="Save" OnClick="tbrTools_ButtonClicked">
                                                    <Appearance>
                                                        <Image Url="~/Images/disk_blue.png" />
                                                    </Appearance>
                                                    <ClientSideEvents Click="WebImageSave_Click" />
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
                                            <td>
                                                <igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                                    AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                                    <Appearance>
                                                        <Image Url="~/Images/refresh.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnCopy" runat="server" Text="Copy" CommandName="Copy"
                                                    AccessKey="o" OnClick="tbrTools_ButtonClicked" CausesValidation="False" Visible="true">
                                                    <Appearance>
                                                        <Image Url="~/Images/copy.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnQA" runat="server" Text="QA Print" CommandName="QA"
                                                    OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                                    <Appearance>
                                                        <Image Url="../Images/document_view.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnAch" runat="server" Text="ACH/DD" CommandName="ACH" OnClick="tbrTools_ButtonClicked"
                                                    CausesValidation="False" Visible="False">
                                                    <Appearance>
                                                        <Image Url="~/Images/currency_dollar.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                        </asp:Panel>
                                        <td>
                                            <igtxt:WebImageButton ID="btnPDF" runat="server" Text="View CU Form" OnClick="btnAction_Click"
                                                CausesValidation="False" AccessKey="P">
                                                <Appearance>
                                                    <Image Url="~/Images/document_out.png" />
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                        <td>
                                            <igtxt:WebImageButton ID="btnOpsPDF" runat="server" Text="View Ops Form" OnClick="btnOpsAction_Click"
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
                        <uc4:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                        <asp:HiddenField runat="server" ID="hidStatus" />
                        <asp:HiddenField runat="server" ID="IsNewVerticalandMarkets" />
                        <asp:HiddenField runat="server" ID="hiddenCrmStatus" />
                        <asp:HiddenField runat="server" ID="hiddenAcceptTransaction" />
                        <asp:Panel ID="pnlServices" runat="server" Height="" Width="">
                            <div runat="server" id="divProductSummary" visible="false">
                                <fieldset class="dialog">
                                    <legend>Summary</legend>
                                    <div style="margin-left: 10px;">
                                        <div runat="server" id="divPaymentAcceptance">
                                            <span><b>Payment Acceptance</b></span>
                                            <asp:DataList runat="server" ID="dListPaymentAcceptance" RepeatDirection="Vertical" RepeatColumns="4" ItemStyle-Width="225px" ItemStyle-Height="20px">
                                                <ItemTemplate>
                                                    <li style="margin-left: 15px;">
                                                        <%# DataBinder.Eval(Container.DataItem, "ProductName") %>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                            <img src="../images/spacer.gif" border="0" width="5" height="8px" alt="" />
                                        </div>
                                        <div runat="server" id="divRiskManagement">
                                            <span><b>Risk Management</b></span>
                                            <asp:DataList runat="server" ID="dListRiskmanagement" RepeatDirection="Vertical" RepeatColumns="4" ItemStyle-Width="225px" ItemStyle-Height="20px">
                                                <ItemTemplate>
                                                    <li style="margin-left: 15px;">
                                                        <%# DataBinder.Eval(Container.DataItem, "ProductName") %>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                            <img src="../images/spacer.gif" border="0" width="5" height="8px" alt="" />
                                        </div>
                                        <div runat="server" id="divDeployment">
                                            <span><b>Deployment</b></span>
                                            <asp:DataList runat="server" ID="dListDeployment" RepeatDirection="Vertical" RepeatColumns="4" ItemStyle-Width="225px" ItemStyle-Height="20px">
                                                <ItemTemplate>
                                                    <li style="margin-left: 15px;">
                                                        <%# DataBinder.Eval(Container.DataItem, "ProductName") %>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                            <img src="../images/spacer.gif" border="0" width="5" height="8px" alt="" />
                                        </div>
                                        <div runat="server" id="divAlternativePayments">
                                            <span><b>Alternative Payments</b></span>
                                            <asp:DataList runat="server" ID="dListAlternativePayments" RepeatDirection="Vertical" RepeatColumns="4" ItemStyle-Width="225px" ItemStyle-Height="20px">
                                                <ItemTemplate>
                                                    <li style="margin-left: 15px;">
                                                        <%# DataBinder.Eval(Container.DataItem, "ProductName") %>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                            <img src="../images/spacer.gif" border="0" width="5" height="8px" alt="" />
                                        </div>
                                        <div runat="server" id="divRDR">
                                            <span><b>RDR</b></span>
                                            <asp:DataList runat="server" ID="rdrList" RepeatDirection="Vertical" RepeatColumns="4" ItemStyle-Width="225px" ItemStyle-Height="20px">
                                                <ItemTemplate>
                                                    <li style="margin-left: 15px;">
                                                        <%# DataBinder.Eval(Container.DataItem, "ProductName") %>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:DataList>
                                            <img src="../images/spacer.gif" border="0" width="5" height="8px" alt="" />
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 50%; vertical-align: top">
                                        <fieldset class="dialog">
                                            <legend>Identification Numbers</legend>
                                            <table border="0" width="100%">
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">Visa:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="Visa" runat="server" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">Visa Electron:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="VisaElectron" runat="server" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">MasterCard:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="MasterCard" runat="server" />
                                                    </td>
                                                    <td></td>
                                                </tr>

                                                <tr>
                                                    <td class="lblRight" style="width: 150px">Discover:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="DiscoverNovus" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="DiscoverMid" runat="server" MaxLength="16" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">American Express ESA:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="AmericanExpress" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AMEXMid" runat="server" MaxLength="16" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">Amex OnePoint:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="AmexOP" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AmexOPID" runat="server" MaxLength="16" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">Amex OptBlue:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="AmexOB" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AmexOBID" runat="server" MaxLength="16" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="lblRight" style="width: 150px">ACH/DD:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ACH" runat="server" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">Pin Debit:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="PinDebit" runat="server" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">EBT FCS #:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="EBTFCS" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="EBTFCSMid" runat="server" MaxLength="16" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">WEX #:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="WEX" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PetroleumWEXMid" runat="server" MaxLength="16" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Voyager #:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="Voyager" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="PetroleumVoyagerMid" runat="server" MaxLength="16" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">JCB:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="Jcb" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="JCBMid" runat="server" MaxLength="30" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">Diners Club:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="Diners" runat="server" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">Maestro:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="Maestro" runat="server" />
                                                    </td>
                                                    <td></td>
                                                </tr>

                                                <tr>
                                                    <td class="lblRight" style="width: 150px">TeleCheck #:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="Telecheck" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="CheckServicesMid" runat="server" MaxLength="16" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">GETI #:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="GETI" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="GiftCardMID" runat="server" MaxLength="16" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">Fuel Man #:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="FuelMan" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="FuelManMID" runat="server" MaxLength="16" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%--<td class="lblRight" style="width: 150px">Activation Code:
                                                    </td>
                                                    <td></td>--%>
                                                    <td>
                                                        <asp:TextBox ID="ActivationCode" runat="server" MaxLength="16" Width="200px" Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">
                                                        Dual Pricing
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="DualPricing" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <fieldset class="dialog">
                                            <legend>Additional Information</legend>
                                            <table border="0" cellspacing="2" width="100%">
                                                <tr>
                                                    <td class="lblRight">
                                                        <asp:Label ID="lblGatewayonly" runat="server" Text="Gateway Only:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="Gatewayonly" runat="server" Enabled="False" AutoPostBack="true" OnCheckedChanged="Gatewayonly_OnCheckedChanged" />
                                                    </td>
                                                </tr>
                                                <tr id="trRtnPolicy" runat="server">
                                                    <td class="lblRight">Rtn Policy:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ReturnPoliciesUID" runat="server" Width="205px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Application Type:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ApplicationTypeUID" runat="server" Width="205px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" style="width: 150px">Business Type:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="BusinessStructureUID" runat="server" Width="205px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Bus. Start Date:
                                                    </td>
                                                    <td>
                                                        <ig:WebDatePicker ID="BusinessStartDate" runat="server" NullText="" Width="205px">
                                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                SlideOpenDuration="1" />
                                                        </ig:WebDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Bus. License #:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="BusinessLicense" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Change Reason:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ReasonChangesUID" runat="server" Width="205px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Website:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="BusinessWebsite" runat="server" MaxLength="100" Width="200px"></asp:TextBox>
                                                        <% if (EditMode)
                                                            { %>
                                                        <script type="text/javascript">
                                                            RemoveSpaces("#<%=BusinessWebsite.ClientID%>");
                                                        </script>
                                                        <% }%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Merchant Sells:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="MerchantSells" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Source:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Source" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Reference #:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="Referral" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Lead ID:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="LeadsID" runat="server" Width="200px" MaxLength="10" Enabled="false"></asp:TextBox>
                                                        <asp:HiddenField ID="LeadsUID" runat="server" />
                                                        <asp:LinkButton ID="btnSelect" runat="server" Text="Select" CausesValidation="false" OnClick="btnSelect_Click" Style="vertical-align: bottom;" />
                                                        <asp:LinkButton runat="server" ID="lbRemoveMerchant" CausesValidation="false" Visible="false" OnClientClick="return confirm('Are you sure you want to delete the Lead ID?')" OnClick="lbRemoveMerchant_Click"><span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="LeadsID"
                                                            ErrorMessage="Please enter a valid Lead ID." MaximumValue="100000" MinimumValue="1"
                                                            Type="Integer" Display="None"></asp:RangeValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <uc9:wucMCClookup IsVisible="true" IsEnabled="false" ID="MCClookup" runat="server" lblNutraWidth="150px" txtSicCodeWidth="120px" txtSicCodeDescWidth="200px" lblSicCodeWidth="150px" lblSicCodeDescWidth="150px" txtVisaSicCodeWidth="120px" txtVisaSicCodeDescWidth="200px" lblVisaSicCodeWidth="150px" lblVisaSicCodeDescWidth="150px"></uc9:wucMCClookup>
                                                    </td>
                                                </tr>
                                                <%--PXP-8253:START: Add New Vertical Checkbox in profile page By Ali Khan--%>
                                                <tr>
                                                    <td class="lblRight">New Vertical:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="IsNewVertical" runat="server" />
                                                    </td>
                                                </tr>
                                                <%-- PXP-8253:END: Add New Vertical Checkbox in profile page By Ali Khan--%>
                                                <%--PXP-14480: by Satyajit for PCCS--%>
                                                <tr>
                                                    <td class="lblRight">Paysafe Consumer Consent Service (PCCS):
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkPCCSSwitch" runat="server" Enabled="false" ClientIDMode="Static" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight"></td>
                                                    <td>
                                                        <asp:Label ID="NAICS" runat="server" Visible="False"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" valign="top">Special Requests:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="SpecialRequest" TextMode="multiLine" runat="server" Width="98%"
                                                            Rows="4"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="rowWaiveOtherItemFee" runat="server">
                                                    <td class="lblRight">Waive Other Item Fee:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="WaiveOtherItemFee" runat="server" />
                                                    </td>
                                                </tr>
                                                <%-- DM-5082--%>
                                                <tr id="rowCBExcessiveFeeWaived" runat="server" visible="false">
                                                    <td class="lblRight">Waive Excessive Chargeback Fee:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="ChargebackExcessiveFeeWaived" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr id="rowSuppressProcessingStatements" runat="server">
                                                    <td class="lblRight">Suppress Processing Statements:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="SuppressProcessingStatements" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">SalesForceID:
                                                    </td>
                                                    <td>S-<asp:TextBox ID="SalesForceID" ReadOnly="true" runat="server" Width="60px" Style="text-align: right" MaxLength="9"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="rowInCollection" runat="server">
                                                    <td class="lblRight">In Collections:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="IsInCollection" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">SOS Divert:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="SOSDivertStatus" runat="server" Enabled="false" />
                                                    </td>
                                                </tr>
                                                <tr >
                                                    <td class="lblRight">Risk Divert:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="RiskDivertStatus" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr id="rowWithAgency" runat="server">
                                                    <td class="lblRight">With Agency:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="IsWithAgency" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Risk Closure:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="AccountClosureRisk" runat="server" Enabled="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Rollover Account:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="IsRolloverAccount" runat="server" Enabled="False" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Allocation:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="IsDMAllocation" runat="server" Enabled="False" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <asp:Panel ID="pnlAccGroups" runat="server">
                                            <fieldset>
                                                <legend>Account Groups</legend>
                                                <asp:UpdatePanel ID="UpdatePanelAccGroups" runat="server" UpdateMode="Conditional">

                                                    <ContentTemplate>
                                                        <uc8:wucAccountGroups runat="server" ID="AccountGroups" ShowValidators="false" ControlWidth="250px" ControlType="BulletList" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </fieldset>
                                        </asp:Panel>
                                    </td>
                                    <td style="vertical-align: top">
                                        <fieldset class="dialog">
                                            <legend>Customer Service Information</legend>
                                            <table border="0" cellspacing="2" width="100%">
                                                <tr>
                                                    <td class="auto-style1">Customer Service Phone #:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="CustomerServicePhoneCallingCode" runat="server" Width="45px">
                                                        </asp:DropDownList>
                                                        <asp:TextBox ID="CallingCodeDisplay" runat="server" Width="42px"></asp:TextBox>
                                                        <ig:WebMaskEditor ID="CustomerServicePhone" runat="server" InputMask="############" PromptChar=' ' Width="78px" ShowMaskOnFocus="False">
                                                        </ig:WebMaskEditor>
                                                        <ig:WebMaskEditor ID="CustomerServicePhoneExt" runat="server" InputMask="000000" CssClass="text igte_Edit" Width="45px" ShowMaskOnFocus="False"></ig:WebMaskEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="auto-style1">Customer Service Email:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="CustomerServiceEmail" runat="server" MaxLength="50" Width="230px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <uc1:wucContact ID="wucContact1" ControlContactType="Merchant" runat="server" />
                                        <asp:Panel ID="pnlAgentInfo" runat="server">
                                            <fieldset class="dialog">
                                                <legend>Agent Contact</legend>
                                                <table cellspacing="2" width="100%">
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Agent ID:
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="AgentID" runat="server" Font-Bold="True" Width="150px" OnClick="AgentID_Click" CausesValidation="false"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Agent DBA:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="AgentFirstLastName" runat="server" Font-Bold="True" Width="150px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">DBA Phone:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="AgentPhone" runat="server" Font-Bold="True" Width="150px"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px" valign="top">Email:
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="AgentEmail" runat="server" Font-Bold="True" Enabled="false" ReadOnly="true"
                                                                Rows="3" TextMode="multiLine" Style="border: none 0px transparent;" Width="250px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </asp:Panel>
                                        <fieldset class="dialog">
                                            <legend>Bank Account</legend>
                                            <asp:Panel runat="server" ID="pnlBank" Width="" Height="">
                                                <asp:Label ID="lblFeesAndDebits" runat="server" Visible="false" Text="Fees and Debits" ForeColor="Blue"></asp:Label>
                                                <br /><br />
                                                <table border="0" cellspacing="2" width="100%">
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Account Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="AccountName" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight ">Dstn Bank Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="BankName" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblLeft" colspan="2"><b>US Domestic Bank Account</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Dstn Bank Routing #:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="RoutingNumber" runat="server" MaxLength="11" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Dstn Bank Account #:
                                                        </td>
                                                        <td>

                                                            <% if (EditMode && HasBankAccountRole && PnlBankField.IsEnabled)
                                                                { %>
                                                            <asp:TextBox ID="AccountNumber" runat="server" MaxLength="18" Width="200px"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="AccountNumber"
                                                                Display="None" ErrorMessage="Invalid Account Number" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                                            <% }
                                                                else
                                                                { %>
                                                            <asp:TextBox ID="AccountTmp" runat="server" MaxLength="18" Width="200px"></asp:TextBox>
                                                            <% } %>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Dstn Bank Currency:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="BankCurrency" runat="server" Width="200px"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="trlblWireAccount" runat="server">
                                                        <td class="lblLeft" colspan="2"><b>
                                                            <asp:Label ID="lblWireAccount" runat="server" Text="Wire Account Information"></asp:Label></b></td>
                                                    </tr>
                                                    <tr runat="server" id="trBankIBAN">
                                                        <td class="lblRight">
                                                            <asp:Label ID="lblBankIBAN" runat="server" Text="Dstn IBAN:"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="BankIBAN" runat="server" MaxLength="30" Width="200px"></asp:TextBox></td>
                                                    </tr>
                                                    <tr runat="server" id="trBankSwiftID">
                                                        <td class="lblRight">
                                                            <asp:Label ID="lblBankSwiftID" runat="server" Text="Dstn Bank ID/SwiftCode:"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="BankSwiftID" runat="server" MaxLength="11" Width="200px"></asp:TextBox></td>
                                                    </tr>
                                                    <tr id="trBankTransitCode" runat="server">
                                                        <td class="lblRight" title="The CAD requirement for transit number consists of nine digits. (Bank Code (4 digits) + Branch Code (5 digits))">
                                                            <asp:Label ID="lblBankTransitNumber" runat="server" Text="Dstn Transit Number:"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="BankCode" runat="server" MaxLength="4" Width="75px" ToolTip="Bank Code (4 digits)"></asp:TextBox>
                                                            <asp:TextBox ID="BankBranchCode" runat="server" MaxLength="5" Width="120px" ToolTip="Branch Code (5 digits)"></asp:TextBox>

                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:CheckBox runat="server" ID="chkShowDeposits" AutoPostBack="true"
                                                    OnCheckedChanged="chkShowDeposits_CheckedChanged" Text="Merchant has separate DDAs for Fees & Debits and Deposits." />
                                                <br /><br />
                                                <asp:Label ID="lblDeposits" runat="server" Text="Deposits" Visible="false" ForeColor="Blue"></asp:Label>
                                                <br />
                                                <br />
                                                <table id="tblFeesAndDebits" border="0" cellspacing="2" width="100%" runat="server" visible="false">
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Account Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDepositAccountName" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight ">Dstn Bank Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDepositBankName" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblLeft" colspan="2"><b>US Domestic Bank Account</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Dstn Bank Routing #:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDepositRoutingNumber" runat="server" MaxLength="11" Width="200px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Dstn Bank Account #:
                                                        </td>
                                                        <td>
                                                            <% if (EditMode && HasBankAccountRole && PnlBankField.IsEnabled)
                                                                { %>
                                                            <asp:TextBox ID="txtDepositAccountNumber" runat="server" MaxLength="18" Width="200px"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtDepositAccountNumber"
                                                                Display="None" ErrorMessage="Invalid Account Number" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                                            <% }
                                                                else
                                                                { %>
                                                            <asp:TextBox ID="txtDepositAccountNumberMask" runat="server" MaxLength="18" Width="200px"></asp:TextBox>
                                                            <% } %>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Dstn Bank Currency:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlDepositBankCurrency" runat="server" Width="200px"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </fieldset>
                                        <fieldset class="dialog">
                                            <legend>Seasonal Months</legend>
                                            <uc5:wucServices ID="WucServices2" runat="server" />
                                            <uc6:wucSeasonalMonths ID="WucSeasonalMonths" runat="server" />
                                        </fieldset>
                                        <fieldset class="dialog">
                                            <legend>Valid Merchant Descriptors</legend>
                                            <asp:UpdatePanel runat="server" ID="pnl">
                                                <ContentTemplate>
                                                    <uc3:wucDescriptor runat="server" ID="ValidDescriptors" ShowValidators="false" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </fieldset>
                                        <fieldset class="dialog">
                                            <legend>Operation Instructions</legend>
                                            <asp:TextBox ID="UWIssues" runat="server" Rows="8" TextMode="multiLine" Width="99%"></asp:TextBox>


                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlTerminals" runat="server">
                                <fieldset class="dialog">
                                    <legend>Terminals</legend>
                                    <asp:CheckBox runat="server" Text="No Equipment Information" ID="NoEquipment" Visible="false" />
                                    <uc7:wucEquipment ID="WucEquipment" runat="server" EquipmentDocs="true" />
                                </fieldset>
                            </asp:Panel>
                            <asp:Panel ID="pnlAdvertisingSalesDelivery" runat="server" >
                                <fieldset class="dialog">
                                    <legend>Advertising Sales and Delivery</legend>
                                    <table border="0">
                                        <tr>
                                            <td class="lblRight" width="40%" >
                                                Do you have a refund policy for Visa/MC/Discover/Amex:
                                            </td>
                                            <td width="60%">
                                                <asp:DropDownList ID="WFReturnPoliciesUID" runat="server" Width="200px">
                                                </asp:DropDownList>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="lblRight" width="40%" >
                                                What is your return, cancellation or refund policy:
                                            </td>
                                            <td width="70%">
                                                <asp:TextBox ID="ReturnCancelPolicy" runat="server"  MaxLength="50" Width="200px"
                                                    CssClass="text igte_Edit">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="lblRight" width="40%">
                                                Is the refund policy in writing and obvious to the cardholder/customer:
                                            </td>
                                            <td width="60%"> <asp:DropDownList runat="server" CssClass="text igte_Edit"  ID="RefundPolicyAwareness"
                                                    Width="200px" AutoPostBack="true" OnSelectedIndexChanged="RefundPolicyAwareness_SelectedIndexChanged" >
                                                    <asp:ListItem Text="-- Select --" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                </asp:DropDownList></td>


                                        </tr>
                                        <tr id="trRefundPolicyAwarenessReason" runat="server" visible="false">

                                            <td align="right" width="40%">
                                                Explain, if No:
                                            </td>
                                            <td width="60%">
                                                <asp:TextBox ID="RefundPolicyAwarenessReason" runat="server"  MaxLength="50" Width="200px"
                                                                    CssClass="text igte_Edit" >
                                                </asp:TextBox>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="lblRight" width="40%">
                                                How does the customer purchase/order the product:
                             
                                            </td>
                                            <td width="60%">
                                                <asp:CheckBoxList runat="server"
                                                    ID="PurchaseOrOrderProduct" RepeatDirection="Horizontal" RepeatColumns="5">
                                                </asp:CheckBoxList>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="lblRight" width="40%">
                                                What is the delivery time frame of the product/service to the customer:                             
                                            </td>
                                            <td width="60%">
                                                <asp:DropDownList runat="server" CssClass="text igte_Edit" ID="DeliveryTime"
                                                    Width="200px">
                                                    <asp:ListItem Text="-- Select --" Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="0-7 days" Value="Delivery07"></asp:ListItem>
                                                    <asp:ListItem Text="8-14 days" Value="Delivery08"></asp:ListItem>
                                                    <asp:ListItem Text="15-30 days" Value="Delivery15"></asp:ListItem>
                                                    <asp:ListItem Text="30+ days" Value="Delivery30"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="lblRight" width="40%">
                                                <b> What percentage of your business is</b>
                                            </td>
                                            <td width="60%"></td>
                                        </tr>
                                        <tr>

                                            <td class="lblRight" width="40%"> Deposits / Future Services: </td>
                                            <td width="60%">
                                                <ig:WebPercentEditor ID="Deposit_FutureServices" MinDecimalPlaces="2" MaxValue="100" runat="server"
                                                    Width="200px" NullText="0.00" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="lblRight" width="40%">
                                                Cash & Carry:
                                            </td>
                                            <td width="60%">
                                                <ig:WebPercentEditor ID="Cash_Carry" MinDecimalPlaces="2" MaxValue="100" runat="server"
                                                    Width="200px"  NullText="0.00" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="lblRight" width="40%">
                                                In what geographic areas will the product(s) be marketed and sold:                             
                                            </td>
                                            <td width="60%">   <asp:TextBox ID="GeographicAreas" runat="server"  MaxLength="50" Width="200px"
                                                    CssClass="text igte_Edit"> 
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>

                                </fieldset>
                            </asp:Panel>
                            <uc2:wucMerchantCategories ID="wucMerchantCategories1" runat="server" />
                            <br />
                        </asp:Panel>
                    </asp:Panel>
                    <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="250px" Width="400px"
                        Modal="true" InitialLocation="centered" WindowState="Hidden" Moveable="false">
                        <ContentPane>
                            <Template>
                                <uc3:wuConfirm runat="server" ID="confirm" />
                            </Template>
                        </ContentPane>
                        <Header CaptionText="Confirm Values" CloseBox-Visible="false">
                        </Header>
                    </ig:WebDialogWindow>
                    <ig:WebDialogWindow ID="dlgcontrol" runat="server" Height="550px" Width="750px" Modal="false"
                        InitialLocation="Centered" WindowState="Hidden">
                        <ContentPane>
                            <Template>
                                <uc99:wucLeads ID="grdLeads" runat="server" />
                            </Template>
                        </ContentPane>
                        <Header CaptionText="Leads">
                        </Header>
                    </ig:WebDialogWindow>
                </td>
            </tr>
        </table>
    </div>

    <script type='text/javascript' src="../js/bootstrap.min.js"></script>
    <script type='text/javascript' src="../js/MPSAddressAutoComplete.js"></script>
    <script type='text/javascript' src="../js/MPSEquipmentAutoComplete.js"></script>

    <script type="text/javascript">

        if ($("#ContentPlaceHolder1_WucBusinessInfo1_BusinessAddress").attr("readonly") != "readonly") {

            MPSAddressAutoComplete.init("#ContentPlaceHolder1_WucBusinessInfo1_BusinessAddress",
                "#ContentPlaceHolder1_WucBusinessInfo1_BusinessCity",
                "#ContentPlaceHolder1_WucBusinessInfo1_BusinessState",
                "#ContentPlaceHolder1_WucBusinessInfo1_BusinessZip",
                "#ContentPlaceHolder1_WucBusinessInfo1_BusinessCountry",
                "#ContentPlaceHolder1_WucBusinessInfo1_BusinessProvince");
            if ($("#ContentPlaceHolder1_WucBusinessInfo1_BusinessCountry").val() != "US") {
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessProvince").css({ display: "inline" });
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessState").css({ display: "none" });
            }

        }

        if ($("#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingAddress").attr("readonly") != "readonly") {

            MPSAddressAutoComplete.init("#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingAddress",
                "#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingCity",
                "#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingState",
                "#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingZip",
                "#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingCountry",
                "#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingProvince");
            if ($("#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingCountry").val() != "US") {
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingProvince").css({ display: "inline" });
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingState").css({ display: "none" });
            }
        }

        if ($("#ContentPlaceHolder1_WucEquipment_tbEquipmentSearch").attr("readonly") != "readonly") {

            MPSEquipmentAutoComplete.init("#ContentPlaceHolder1_WucEquipment_tbEquipmentSearch",
                "#ContentPlaceHolder1_WucEquipment_Type",
                "#ContentPlaceHolder1_WucEquipment_Maker",
                "#ContentPlaceHolder1_WucEquipment_Model",
                "#ContentPlaceHolder1_WucEquipment_ItemUID",
                "#ContentPlaceHolder1_WucEquipment_IsNewItem",
                "#ContentPlaceHolder1_WucEquipment_EMVCompliance"
            );
        }


        $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessCountry").change(function () {

            if ($(this).val() != "US") {
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessState").css("display", "none");
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessProvince").css("display", "inline");
            }

            else if ($(this).val() == "US") {
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessState").css("display", "inline");
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessProvince").css("display", "none");
            }


        })



        $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingCountry").change(function () {

            if ($(this).val() != "US") {
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingState").css("display", "none");
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingProvince").css("display", "inline");
            }

            else if ($(this).val() == "US") {
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingState").css("display", "inline");
                $("#ContentPlaceHolder1_WucBusinessInfo1_BusinessMailingProvince").css("display", "none");
            }


        })
        $("#ContentPlaceHolder1_CustomerServicePhoneCallingCode").change(function () {
            $("#ContentPlaceHolder1_CallingCodeDisplay").val($(this).val());
        })


    </script>
    <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="180px" Width="420px"
        InitialLocation="Centered" ClientIDMode="Static" Modal="True" WindowState="Hidden">
        <ContentPane>
            <Template>
                <div class="tabcontent">
                    <table cellspacing="3" width="100%" align="center">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblErr" CssClass="gen_error"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblMessage" Visible="false" Text="Are you sure to copy the merchant info? This action cannot be reversed! Are you sure? Press OK to copy the selected items or Press Cancel to close." Font-Names="Verdana" Font-Size="X-Small"></asp:Label>
                                <asp:Label runat="server" ID="lblNoACH" Text="Are you sure to copy the merchant info? This action cannot be reversed! Are you sure? Press OK to copy or Press Cancel to close." Font-Names="Verdana" Font-Size="X-Small"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trAchProfile" runat="server" visible="false">
                            <td style="padding-left: 8px; padding-right: 5px;">

                                <asp:CheckBox runat="server" ID="chkCopyACHprofile" Text="ACH Profile" />
                            </td>
                        </tr>
                        <tr id="trMerchantDocuments" runat="server" visible="false">
                            <td style="padding-left: 5px; padding-right: 5px;">
                                <span class="gen_Succ">Merchant Documents:</span>
                                <br />
                                <asp:CheckBoxList ID="chkDocumentlist" runat="server" DataValueField="DocID" DataTextField="OrigName">
                                </asp:CheckBoxList>
                            </td>

                        </tr>
                        <td align="center">
                            <asp:Button runat="server" Text="OK" ID="btnOk" OnClick="btnOk_Click" CommandArgument="Yes" CausesValidation="false" />
                            &nbsp;
                            <asp:Button runat="server" Text="Cancel" ID="btnCan" OnClick="btnOk_Click" CommandArgument="No" CausesValidation="false" />
                        </td>

                    </table>
                </div>
            </Template>
        </ContentPane>
        <Header CaptionText="Merchants">
        </Header>
    </ig:WebDialogWindow>

</asp:Content>


<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="HeadPlaceHolder">
    <style type="text/css">
        .auto-style1 {
            width: 160px;
        }
    </style>

</asp:Content>



