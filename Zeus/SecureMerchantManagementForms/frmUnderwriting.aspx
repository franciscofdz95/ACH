<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true" Inherits="frmUnderwriting" Title="Underwriting" CodeBehind="frmUnderwriting.aspx.cs" ValidateRequest="false" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Src="../UserControls/wucServices.ascx" TagName="wucServices" TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/wucConditions.ascx" TagName="wucConditions" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/wucCashAdvance.ascx" TagName="wucCashAdvance" TagPrefix="uc4" %>
<%@ Register Src="../UserControls/wucOwnerUW.ascx" TagName="wucOwnerUW" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/wucCorporateBusinessUW.ascx" TagName="wucCorpBuzUW" TagPrefix="uc10" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc7" %>
<%@ Register Src="../UserControls/wucMCClookup.ascx" TagName="wucMCClookup" TagPrefix="uc9" %>
<%@ Register Src="~/UserControls/wucDescriptor.ascx" TagName="wucDescriptor" TagPrefix="uc3" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<%@ Register Src="../UserControls/wucServices.ascx" TagName="wucServices" TagPrefix="uc5" %>
<%@ Register Src="~/UserControls/wucUWMerchantScoreCards.ascx" TagName="wucUWMerchantScoreCards" TagPrefix="sc1" %>
<%@ Register Src="~/UserControls/wucUWFinancialScoreCardGrid.ascx" TagName="wucUWFinancialScoreCardGrid" TagPrefix="sc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner">
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <script src="../js/encoder.js" type="text/javascript"></script>
        <link rel="Stylesheet" type="text/css" href="../css/modalPopLite.css" />
        <script type="text/javascript" src="../js/modalPopLite.js"></script>
        <script language="javascript" type="text/javascript">

            <%--PXP-12066: Start by Rohit Thakur--%>
            function countChar(val) { 
                var len = val.value.length;
            if(val.id == 'ContentPlaceHolder1_RegisteredURLs'){
                $('#ContentPlaceHolder1_charLength').text(len);
            }else{
                $('#ContentPlaceHolder1_VIRPcharLength').text(len);
            }
                
            };
            <%--PXP-12066: End by Rohit Thakur--%>
            function DisplayProcessing() {
                document.getElementById('<%=Request3DE.ClientID%>').disabled = true;
                document.getElementById('<%=Response3DE.ClientID%>').disabled = true;
                $('#pnlBusy').css("display", "block");
            };


            function CheckNumeric() {
                var key;
                key = event.which ? event.which : event.keyCode;

                if ((key >= 48 && key <= 57) || key == 13) {
                    return true;
                }
                else {
                    return false;
                }
            }

            function CheckAll(CheckBox, chkChild) {

                TotalChkBx = parseInt('<%= this.grdChecklist.Rows.Count %>');
                var TargetBaseControl = document.getElementById('<%= this.grdChecklist.ClientID %>');
                var TargetChildControl = chkChild;
                var Inputs = TargetBaseControl.getElementsByTagName("input");
                for (var iCount = 0; iCount < Inputs.length; ++iCount) {
                    //Niranjan:- PXP-5469 In Zeus, Credit-Verification section : 'TransUnion' checkbox is getting checked when 'Select All' is checked.
                    if ((Inputs[iCount].type == 'checkbox' && Inputs[iCount].disabled == false) && (Inputs[iCount].id.indexOf(TargetChildControl, 0) >= 0))
                        Inputs[iCount].checked = CheckBox.checked;
                }
            }


            function EnableView(legalname, creditdate) {

                //The last credit report was pulled on [Last Credit Pull Date]. 
                var str = "";

                if (creditdate != null && creditdate != "")
                    str = "The last credit report was pulled on " + creditdate + ".";

                if (legalname == null || legalname.trim() == "") {
                    alert("Legal Name is missing.");
                    return false;
                }

                if (confirm("You are about to pull credit for " + legalname + ". " + str + " Press Ok to proceed")) {
                    return true;
                }
                else {
                    return false;
                }
            }

            function ConfirmClear() {

                return confirm("Do you want to clear this credit report?");
            }

            //Amit Patne : PXP-3818 - Zeus: Vertical Market and Billing Method
            $(document).ready(function () {

                //Commented code by anuj for PXP-9250
                //$('#ContentPlaceHolder1_VerticalMarket input[type=checkbox]').change(function () {
                //    if (this.checked) {
                //        $('#ContentPlaceHolder1_VerticalMarket input[type=checkbox]').not(
                //              $(this)).prop('checked', false);
                //    }
                //});
                // PXP-12436: Start - Rohit Thakur
                if ($('#ContentPlaceHolder1_WucBusinessInfo1_OfficeID').val() == "1") {
                    $('#ContentPlaceHolder1_MCClookup_Label1').text("Tangible Trial");
                }
                else {
                    $('#ContentPlaceHolder1_MCClookup_Label1').text("Nutra Trial");
                }
                // PXP-12436: End - Rohit Thakur
                //Added Code by Anuj for PXP-9250
                $('#ContentPlaceHolder1_MarketingMethods input[type=checkbox]').change(function () {

                    if (this.checked) {
                        $('#ContentPlaceHolder1_MarketingMethods input[type=checkbox]').not(
                              $(this)).prop('checked', false);
                    }
                });
            });

            function WebImageSave_Click(oButton, oEvent)
            {
                //Commented code by anuj for PXP-9250
                //if ($('#ContentPlaceHolder1_VerticalMarket :checkbox:checked').length > 1) {
                //    alert('Multiple Vertical Markets has been selected. Please select only one Vertical Market.');
                //    oEvent.cancel = true;
                //}

                // Added by Chandra for PXP-7898
                var MCCcode = $("#<%=MCClookup.FindControl("txtSicCode").ClientID %>");
                var isNutra = $("#<%=MCClookup.FindControl("IsNutraMerchant").ClientID %>");
                var oldStatus = $("#<%=hidStatus.ClientID %>");
                var newStatus = $("#<%=StatusUID.ClientID %>");

                if ('<%=ACHStatus.Visible%>' == "True")
                { newStatus = $("#<%=ACHStatusUID.ClientID %>"); }

                if (oldStatus.val().toUpperCase() != newStatus.val().toUpperCase() && newStatus.val().toUpperCase() == "73FC4B27-98D4-40EA-B9FC-1370C564CB12") {
                    if (MCCcode.val() == "5968" || MCCcode.val() == "5964" ) {
                        var conf = alert("Review Tangible Trial checkbox for Tangible merchants");
                    }
                }
                //PXP-9051 RThakur
                var chkIsNewVertical = document.getElementById('<%= this.IsNewVertical.ClientID %>');
                var chkHierarchyApprovalSignoff = document.getElementById('<%= this.HierarchyApprovalSignOff.ClientID %>');

                if ((oldStatus.val().toUpperCase() != "2FDDA5E4-E80A-4155-8CB2-D5200992FA81") && (newStatus.val().toUpperCase() == "2FDDA5E4-E80A-4155-8CB2-D5200992FA81"
                     || $(chkHierarchyApprovalSignoff).prop("checked"))) {
                    var isAnyVerticalMarketSelected = false;
                    if ($('#ContentPlaceHolder1_VerticalMarket :checkbox:checked').length > 0) {
                        isAnyVerticalMarketSelected = true;
                    }
                    var isIsNewVertical = false;
                    if ($(chkIsNewVertical).prop("checked")) {
                        isIsNewVertical = true;
                    }
                    if (!isAnyVerticalMarketSelected && isIsNewVertical) {
                        var confVerticals = confirm("<%=PaymentXP.BusinessObjects.Constant.NewVerticalandVerticalMarketsChecked%>");
                        if (!confVerticals) {
                            oEvent.cancel = true;
                        }
                    }
                }

                // PXP-9348 RThakur >> Start
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
                // PXP-9348 RThakur >> End




                //Added by koshlendra for information popup display for PXP-9514
                var masterMRP = $("#<%=MasterMRP.ClientID %>");
                var sendMRPRequest = $("#<%=SendMRPRequest.ClientID %>");
                if (!masterMRP.is(':checked') && sendMRPRequest.is(':checked')) {
                    alert("Master MRP checked box is not checked.");
                }
            }
            
            // Add code by Anuj for PXP-8725
            $(document).ready(function () {
                var officeVal = $("[id$=OfficeID]").val();
                var prevVal = $("[id$=UWIssues]").val();
                $('#ContentPlaceHolder1_BillingTypes input[type=checkbox],[id$=HRMonitoringFee]').change(function () {

                    var checkHRMonitoringFee = $('[id$=HRMonitoringFee]').is(':checked');
                    if (checkHRMonitoringFee) {
                        $("[id$=UWIssues]").val("General Set up Details: " + "HR MONITORING FEE\n")
                    }
                    else {
                        $("[id$=UWIssues]").val("General Set up Details:\n");
                    }
                    var output = "";
                    $("#ContentPlaceHolder1_BillingTypes input[type=checkbox]").each(function () {

                        if (this.checked && officeVal == 1) {
                            switch ($(this).next('label').text()) {
                                case "FT":
                                    output += " Free Trial,";
                                    break;
                                case "Continuity":
                                    output += " Continuity,";
                                    break;
                                case "Install":
                                    output += " Install,";
                                    break;
                                case "OTS":
                                    output += " One Time Sale,";
                                    break;
                                default:
                                    break;
                            }
                        }

                    });

                    if (output != '' && officeVal == 1) {
                        output = output.substring(0, output.length - 1);
                        $("[id$=UWIssues]").val($("[id$=UWIssues]").val() + "Billing type:" + output + "\n");
                    }
                    else if (officeVal == 1) {
                        $("[id$=UWIssues]").val($("[id$=UWIssues]").val() + "Billing type:" + "\n");
                    }

                    if (prevVal != '' || prevVal != undefined) {
                        if (prevVal.indexOf("General Set up Details:") != -1 && prevVal.indexOf("Billing type:") != -1) {
                            var lineContent = prevVal.split('\n');
                            lineContent.splice(0, 2);
                            prevVal = lineContent.join('\n');

                        }

                        if (prevVal.indexOf("General Set up Details:") != -1 && prevVal.indexOf("Billing type:") == -1) {
                            var lineContent = prevVal.split('\n');
                            lineContent.splice(0, 1);
                            prevVal = lineContent.join('\n');

                        }
                       
                        $("[id$=UWIssues]").val($("[id$=UWIssues]").val() + prevVal);
                    }

                });
            });
          
        </script>
        <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
            <asp:Panel ID="pnlTools" runat="server">
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
                                <%--Code added for PXP-9145[Zeus:Ability to generate and review High Risk Merchant registration request for Mastercardconnect] by koshalendra start--%>
                                <td>
                                    <igtxt:WebImageButton ID="btnMrpPDF" runat="server" Text="Review MRP Form" OnClick="btnMrpAction_Click"
                                        CausesValidation="False" AccessKey="P">
                                        <Appearance>
                                            <Image Url="~/Images/document_out.png" />
                                        </Appearance>
                                    </igtxt:WebImageButton>
                                </td>
                                <%--Code added for PXP-9145[Zeus:Ability to generate and review High Risk Merchant registration request for Mastercardconnect] by koshalendra end--%>
                                 <td>
                                    <igtxt:WebImageButton ID="btnVIRPPDF" runat="server" Text="Review VIRP Form" OnClick="btnMrpAction_Click" Visible="false" CausesValidation="False" AccessKey="P">
                                        <Appearance>
                                            <Image Url="~/Images/document_out.png" />
                                        </Appearance>
                                    </igtxt:WebImageButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
            &nbsp;<br />
            <uc7:wucMessage ID="WucMessage1" runat="server" />
            &nbsp;&nbsp;
            <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
            <asp:HiddenField runat="server" ID="hidLegalName" />
            <asp:HiddenField runat="server" ID="hidStatus" />
            <asp:HiddenField runat="server" ID="hiddenCrmStatus" />
            <asp:HiddenField runat="server" ID="hiddenAcceptTransaction" />

            <br />
            <div class="title">
                &nbsp;&nbsp;Approved Sales Profile
                <hr class="line" />
            </div>
            <div class="indentedcontent20">
                <asp:Panel runat="server" ID="pnlApproved">
                    <table cellspacing="1" width="100%">
                        <tr>
                            <td class="lblCenter" colspan="2">
                                <strong></strong>
                            </td>
                            <td class="lblCenter" colspan="1"></td>
                            <td class="lblCenter" colspan="1"></td>
                            <td class="lblCenter" colspan="1"></td>
                            <td class="lblCenter" colspan="1"></td>
                        </tr>
                        <tr>
                            <td class="lblRight">Approved Monthly Sales:
                            </td>
                            <td>
                                <ig:WebNumericEditor ID="TinfoAverageMonthlyVMCVolume" runat="server" ValueText="0" MaxLength="15"
                                    Width="125px" TabIndex="100">
                                </ig:WebNumericEditor>
                                &nbsp;
                            </td>
                            <td class="lblRight" valign="top">Hard Cap:
                            </td>
                            <td valign="top">
                                <asp:CheckBox runat="Server" ID="HardCap" TabIndex="110" />
                            </td>
                            <td colspan="2" rowspan="6">
                                <uc9:wucMCClookup ID="MCClookup" IsVisible="true" IsEnabled="true" runat="server" lblNutraWidth="120px" txtSicCodeWidth="120px" txtSicCodeDescWidth="275px" lblSicCodeWidth="120px" lblSicCodeDescWidth="120px" LookupButtonTabIndex="120" txtVisaSicCodeWidth="120px" txtVisaSicCodeDescWidth="275px" lblVisaSicCodeWidth="120px" lblVisaSicCodeDescWidth="120px"></uc9:wucMCClookup>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight" width="130px">Approved Average Ticket:
                            </td>
                            <td>
                                <ig:WebNumericEditor ID="TinfoAverageVMCTicket" runat="server" ValueText="0" Width="125px"
                                    TabIndex="101">
                                </ig:WebNumericEditor>
                            </td>
                            <td class="lblRight">PCI Level:
                            </td>
                            <td>
                                <asp:DropDownList ID="PCILevel" runat="server" Width="80px" TabIndex="112">
                                    <asp:ListItem Value="-1" Selected="true">--Select--</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="5">N/A</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">Approved High Ticket:
                            </td>
                            <td>
                                <ig:WebNumericEditor ID="TinfoHighestTicketAmount" runat="server" ValueText="0"
                                    TabIndex="102" Width="125px">
                                </ig:WebNumericEditor>
                            </td>
                            <td class="lblRight" width="120px" valign="top">Days Hold/Arrears:
                            </td>
                            <td>
                                <asp:TextBox ID="DelaysApproved" runat="server" Width="80px" TabIndex="114"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">Website:
                            </td>
                            <td>
                                <asp:TextBox ID="BusinessWebsite" runat="server" MaxLength="100" Width="125px"></asp:TextBox>
                                <% if (EditMode)
                                { %>
                                    <script type="text/javascript">
                                        RemoveSpaces("#<%=BusinessWebsite.ClientID%>");
                                    </script>
                                <% }%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>

                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>

                        </tr>
                        <tr>
                            <td colspan="4">&nbsp;
                            </td>

                        </tr>
                        <tr>
                            <td colspan="4"></td>
                            <td class="lblRight" valign="top">Merchant Tag:
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="MerchantTag" runat="server" EnableViewState="False" Width="275px"
                                    TabIndex="123"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td runat="server" id="tdEmpty" colspan="2"></td>
                            <td colspan="2">
                                <asp:Panel runat="server" ID="trDaysHoldType">
                                    <td class="lblRight">Days Hold/Arrears Type:

                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DaysHoldTypeID" Width="125px" runat="server" TabIndex="113">
                                            <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                            <asp:ListItem Value="1">Calendar Days</asp:ListItem>
                                            <asp:ListItem Value="2">Business Days</asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                </asp:Panel>

                            </td>
                            <td class="lblRight" valign="top" width="120px">Product Tag:
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="ProductTag" runat="server" EnableViewState="False" Width="275px"
                                    TabIndex="124"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" valign="top">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%" cellspacing="1" border="0">
                                            <tr>
                                                <td class="lblRight" width="128px" valign="top">Notify Risk Dept:
                                                </td>
                                                <td valign="top">
                                                    <asp:CheckBox ID="NotifyRiskDept" runat="server" TabIndex="105" />
                                                </td>
                                                <td class="lblRight" width="150px">ByPass Indicator:
                                                </td>
                                                <td valign="top" align="left">
                                                    <asp:RadioButtonList runat="server" ID="BuyPassIndicator" ToolTip="Select Yes to allow duplicate processing"
                                                        RepeatDirection="horizontal" TabIndex="115">
                                                        <asp:ListItem Text="No" Value="False" Selected="true"></asp:ListItem>
                                                        <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <asp:PlaceHolder runat="server" ID="phAgentlevel">
                                                    <td class="lblRight" valign="top">
                                                        <asp:Label runat="server" Text="Agent Level:" ID="Level"></asp:Label>
                                                    </td>
                                                    <td valign="top">
                                                        <asp:DropDownList runat="server" ID="AgentLevel" Width="125px" TabIndex="106">
                                                        </asp:DropDownList>
                                                    </td>
                                                </asp:PlaceHolder>

                                                <asp:PlaceHolder runat="server" ID="phAssociationNumber">
                                                    <td class="lblRight">Association Number:
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="None"
                                                                ErrorMessage="Association Number is required." ControlToValidate="AssociationNumber"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AssociationNumber" runat="server" Width="180px" MaxLength="6" Text="100001"></asp:TextBox>
                                                        <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "AssociationNumber" ID="RegularExpressionValidatorAssociationNumber" ValidationExpression = "\d+" runat="server" ErrorMessage="Only number allowed."></asp:RegularExpressionValidator>
                                                    </td>
                                                </asp:PlaceHolder>
                                                <td class="lblRight">Divert Upon Boarding:
                                                </td>
                                                <td valign="top">
                                                    <asp:RadioButtonList runat="server" ID="DivertUponBoarding" TabIndex="116" RepeatDirection="horizontal">
                                                        <asp:ListItem Text="No" Value="False" Selected="true"></asp:ListItem>
                                                        <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="lblRight" valign="top">Tier II:
                                                </td>
                                                <td valign="top">
                                                    <asp:CheckBox ID="HighRisk" runat="server" TabIndex="107"></asp:CheckBox>
                                                </td>
                                                <td class="lblRight">Flags Off Upon Boarding:
                                                </td>
                                                <td valign="top">
                                                    <asp:RadioButtonList runat="server" ID="FlagsOffUponBoarding" TabIndex="117" RepeatDirection="horizontal">
                                                        <asp:ListItem Text="No" Value="False" Selected="true"></asp:ListItem>
                                                        <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="lblRight" valign="top">Conditional Approval:
                                                </td>
                                                <td valign="top">
                                                    <asp:CheckBox ID="ConditionalApproval" runat="server" TabIndex="108"></asp:CheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" class="lblRight">&nbsp;<asp:Label Text="Due Date:" ID="date" runat="server" Enabled="true" Style="text-align: right;"></asp:Label>
                                                </td>
                                                <td valign="top">
                                                    <ig:WebDatePicker ID="ConditionalDueDate" runat="server" Width="80px" BackColor="#EFF3FF"
                                                        BorderStyle="Solid" BorderWidth="1px" TabIndex="109">
                                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                            SlideOpenDuration="1" />
                                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                            SlideOpenDuration="1" />
                                                    </ig:WebDatePicker>
                                                </td>

                                            </tr> 

                                            <%--PXP-12066: End by Rohit Thakur--%>
                                            <%--Code added for PXP-9310[Generate and save High Risk Merchant registration request in csv format] by koshalendra end--%>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <br />
                                <div class="title">
                                    &nbsp;&nbsp;HIGH RISK REGISTRATION
                                <hr class="line" />
                                </div>
                                <div class="indentedcontent20" style="height:160px;">
                                    <asp:Panel runat="server">
                                        <div class="leftcolumn">
                                            <table cellpadding="2" cellspacing="2">
                                                <tr>

                                                    <td class="lblRight" valign="top">MC HR Registered:
                                                    </td>
                                                    <td valign="top">
                                                        <asp:CheckBox ID="HighRiskRegistered" runat="server" TabIndex="118"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" valign="top">MC HR Registered Date:
                                                    </td>
                                                    <td valign="top">
                                                        <asp:Label ID="HighRiskRegisteredDate" runat="server" Width="150px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" valign="top">Master MRP:
                                                    </td>
                                                    <td valign="top">
                                                        <asp:CheckBox ID="MasterMRP" OnCheckedChanged="MasterMRP_CheckedChanged" AutoPostBack="true" runat="server" TabIndex="118"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" valign="top">Send MRP Request:
                                                    </td>
                                                    <td valign="top">
                                                        <asp:CheckBox ID="SendMRPRequest" runat="server" TabIndex="118"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td runat="server" id="tdlabelRegisteredURLs" visible="false" class="lblRight" valign="top">Registered URLs:
                                                    </td>
                                                    <td runat="server" id="tdtextbRegisteredURLs" visible="false" valign="top">
                                                        <asp:TextBox ID="RegisteredURLs" placeholder="Add multiple website separate with semi-colon." onkeyup="countChar(this)" TextMode="MultiLine" runat="server" OnTextChanged="btnText_Changed"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="CharactersCount" visible="false">
                                                    <td class="lblRight" valign="top"># of characters entered:
                                                    </td>
                                                    <td valign="top">
                                                        <label id="charLength" class="charCount" runat="server"></label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="rightcolumn">
                                            <table cellpadding="2" cellspacing="2">
                                                <tr>
                                                    <td class="lblRight" valign="top">V HR Registered:
                                                    </td>
                                                    <td valign="top">
                                                        <asp:CheckBox ID="VIRPHighRiskRegistered" runat="server" TabIndex="118"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" valign="top">V HR Registered Date:
                                                    </td>
                                                    <td valign="top">
                                                        <asp:Label ID="VIRPHighRiskRegisteredDate" runat="server" Width="150px"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" valign="top">Master VIRP:
                                                    </td>
                                                    <td valign="top">
                                                        <asp:CheckBox ID="MasterVIRP" OnCheckedChanged="MasterMRP_CheckedChanged" AutoPostBack="true" runat="server" TabIndex="118"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight" valign="top">Send VIRP Request:
                                                    </td>
                                                    <td valign="top">
                                                        <asp:CheckBox ID="SendVIRPRequest" runat="server" TabIndex="118"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="VIRPRegisteredURLsTR" visible="false">
                                                    <td class="lblRight" valign="top">Registered URLs:
                                                    </td>
                                                    <td valign="top">
                                                        <asp:TextBox ID="VIRPRegisteredURLs" placeholder="Add multiple website separate with semi-colon." onkeyup="countChar(this)" TextMode="MultiLine" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="VIRPCharactersCount" visible="false">
                                                    <td class="lblRight" valign="top"># of characters entered:
                                                    </td>
                                                    <td valign="top">
                                                        <label id="VIRPcharLength" class="charCount" runat="server"></label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>                                        
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="MasterMRP"  EventName="CheckedChanged" /> 
                                    </Triggers>
                                    </asp:Panel>
                                </div>
                                <br />
                                <div class="title">
                                    &nbsp;&nbsp;Operations Instructions
                                <hr class="line" />
                                </div>
                                <div class="indentedcontent20">
                                    <asp:TextBox ID="UWIssues" runat="server" Rows="8" TextMode="multiLine" Width="99%"></asp:TextBox>
                                </div>
                                <br />
                            </td>
                            <td colspan="2" valign="top">
                                <table width="100%" cellspacing="1" border="0">
                                    <tr>
                                        <td class="lblRight" valign="top" width="120px">Merchant Sells:
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox ID="MerchantSells" runat="server" MaxLength="50" Width="270px" TextMode="MultiLine"
                                                Rows="5" TabIndex="125"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <%--Start Code by Anuj for PXP-9250--%>
                                    <tr>
                                        <td class="lblRight" valign="top" style="padding-top: 10px;">Marketing Methods:
                                        </td>
                                        <td valign="top" style="padding-top: 10px;">
                                            <asp:CheckBoxList ID="MarketingMethods" RepeatColumns="5" runat="server" RepeatDirection="Horizontal">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <%--End Code by Anuj for PXP-9250--%>
                                    <tr>
                                        <td class="lblRight" valign="top" style="padding-top: 10px;">Vertical Market:
                                        </td>
                                        <td valign="top" style="padding-top: 10px;">
                                            <asp:CheckBoxList RepeatColumns="5" runat="server" ID="VerticalMarket" TabIndex="126">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight" valign="top" style="padding-top: 10px;">
                                            <span>Billing Type:</span>
                                        </td>
                                        <td style="padding-top: 10px;">
                                            <%-- Modify by Anuj for PXP-8725--%>
                                            <%--<asp:CheckBoxList ID="BillingTypes" runat="server" RepeatColumns="5" TabIndex="127" AutoPostBack="true" OnSelectedIndexChanged="OnBillingTypes_Changed"></asp:CheckBoxList>--%>
                                            <asp:CheckBoxList ID="BillingTypes" runat="server" RepeatColumns="5" TabIndex="127"></asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <%--Added code by koshlendra for PXP-4963[Add Monitoring Fee instructions automatically on Ops Form] start--%>
                                    <%-- DM-5364 raul: hide HR Monitoring Fee--%>
                                    <tr style="display:none;">
                                        <td class="lblRight" valign="top" style="padding-top: 10px;">
                                            <span>HR Monitoring Fee:</span>
                                        </td>
                                        <td style="padding-top: 10px;">
                                            <%-- Modify by Anuj for PXP-8725--%>
                                            <%-- <asp:CheckBox ID="HRMonitoringFee" runat="server"  TabIndex="128" OnCheckedChanged="HRMonitoringFee_CheckedChanged" AutoPostBack="true"></asp:CheckBox>--%>
                                            <asp:CheckBox ID="HRMonitoringFee" runat="server" TabIndex="128"></asp:CheckBox>
                                        </td>

                                    </tr>
                                    <%--Added code by koshlendra for PXP-4963[Add Monitoring Fee instructions automatically on Ops Form] start--%>
                                    <%-- PXP-9051 RThakur --%>
                                    <tr>
                                        <td class="lblRight" valign="top" style="padding-top: 10px;">
                                            <span>New Vertical:</span>
                                        </td>
                                        <td style="padding-top: 10px;">
                                            <asp:CheckBox ID="IsNewVertical" runat="server" TabIndex="129"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <%-- PXP-9051 RThakur--%>
                                </table>
                            </td>
                        </tr>

                    </table>
                </asp:Panel>
            </div>
            <br />
            <div class="title">
                &nbsp;&nbsp;Product Required
                <hr class="line" />
            </div>
            <div class="indentedcontent20">
                <uc5:wucServices ID="WucServices1" runat="server" />
            </div>
            <br />
            <ig:WebTab runat="server" ID="TabControl" Enabled="true" Width="970px">
                <PostBackOptions EnableAjax="true" EnableAsyncUpdateAllTabs="true" EnableLoadOnDemand="false" />
                <Tabs>
                    <ig:ContentTabItem Text="Business" EnableDynamicUpdatePanel="False">
                        <Template>
                            <br />
                            <div class="title">
                                &nbsp;&nbsp;Status
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <table cellspacing="2">
                                            <tr>
                                                <asp:Panel ID="CCStatus" runat="server">
                                                    <td>Application Status:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="StatusUID" AutoPostBack="true" runat="server" OnSelectedIndexChanged="StatusUID_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="StatusUID"
                                                            Display="dynamic" Text="*" ErrorMessage="Please select a Status." Operator="NotEqual"
                                                            ValueToCompare="-1"></asp:CompareValidator>
                                                        <asp:HiddenField runat="server" ID="IsStatusOP" />
                                                    </td>
                                                </asp:Panel>
                                                <asp:Panel ID="ACHStatus" runat="server" Visible="false">
                                                    <td class="lblRight">ACH Status
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ACHStatusUID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="StatusUID_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ACHStatusUID"
                                                            ValidationGroup="UWValid" Display="dynamic" Text="*" ErrorMessage="Please select a Status."
                                                            Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                                                    </td>
                                                </asp:Panel>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblddlDeclineReason"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlddlDeclineReason"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lbltbPrimaryReason"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="tbPrimaryReason" Width="500px" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <%--    <asp:Panel ID="pnlWithdrawn" runat="server">
                                <br />
                                <div class="title">
                                    &nbsp;&nbsp;Primary Reason for Withdrawn
                                    <hr class="line" />
                                </div>
                                <div class="indentedcontent20">
                                    <asp:TextBox ID="ReasonForWithdrawn" runat="server" TextMode="MultiLine" MaxLength="100" Width="99%" Height="50px"></asp:TextBox>
                                </div>
                            </asp:Panel>
                              <asp:Panel ID="pnlDeclineReason" runat="server">
                                <br />
                                <div class="title">
                                    &nbsp;&nbsp;Primary Reason for Decline
                                    <hr class="line" />
                                </div>
                                <div class="indentedcontent20">
                                    <asp:TextBox ID="ReasonForDecline" runat="server" TextMode="MultiLine" MaxLength="100" Width="99%" Height="50px"></asp:TextBox>
                                </div>
                            </asp:Panel>--%>
                            <br />
                            <div class="title">
                                &nbsp;&nbsp;Business License
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">&nbsp;&nbsp;
                                            <asp:Label runat="server" ID="lbl1" Text="Credit Report: " CssClass="lblRight"></asp:Label>
                                            <%--<asp:Button ID="btnGet" runat="server" Text="Get" OnClick="btnGet_Click" />&nbsp;--%><%-- DM-2524 --%>
                                            <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" Visible="false" />&nbsp;
                                            <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" OnClientClick="return ConfirmClear();" />&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 60%">
                                            <table width="100%">
                                                <tr>
                                                    <td colspan="4" valign="top">
                                                        <asp:Panel runat="server" ID="ExperianData1">
                                                            <fieldset>
                                                                <legend><b>Risk Scores:</b></legend>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td colspan="2"><b>Intelli</b></td>
                                                                        <td colspan="2"><b>Financial Stability</b></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="lblRight">Score:</td>
                                                                        <td>
                                                                            <asp:TextBox ID="ResIntelliScore" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td class="lblRight">Score:</td>
                                                                        <td>
                                                                            <asp:TextBox ID="ResFinancialStabilityScore" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="lblRight">Risk Level:</td>
                                                                        <td>
                                                                            <asp:TextBox ID="ResIntelliRiskLevel" runat="server"></asp:TextBox>
                                                                        </td>

                                                                        <td class="lblRight">Risk Level:</td>
                                                                        <td>
                                                                            <asp:TextBox ID="ResFinancialStabilityRiskLevel" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Business Name/Phone Verification Indicator:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="NamePhoneVerification" runat="server" Width="70px">
                                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="0">Waived</asp:ListItem>
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="lblRight">Bus. Start Date:
                                                    </td>
                                                    <td>
                                                        <ig:WebDatePicker ID="BusinessStartDate" runat="server" NullText="" Width="100px">
                                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                SlideOpenDuration="1" />
                                                        </ig:WebDatePicker>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Business Address/SSN Indicator:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="AddressSSNVerification" runat="server" Width="70px">
                                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="0">Waived</asp:ListItem>
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="lblRight">Physical Site Visit:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox runat="server" ID="PhysicalSiteVisit" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Business Name/Address/TIN Indicator:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="NameAddressTINVerification" runat="server" Width="70px">
                                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                            <asp:ListItem Value="0">Waived</asp:ListItem>
                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td valign="top" class="lblRight">&nbsp;<asp:Label Text="Physical Visit On:" ID="Visit" Enabled="true" runat="server"
                                                        Style="text-align: right;"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <ig:WebDatePicker ID="PhysicalVisitOn" runat="server" NullText="" Width="100px">
                                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                SlideOpenDuration="1" />
                                                        </ig:WebDatePicker>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:Panel runat="server" ID="ExperianData2">
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="2" style="height: 5px;">
                                                            <asp:HiddenField runat="server" ID="PremierProfileID" />
                                                            <asp:HiddenField runat="server" ID="CreditReportDate" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">DBT:</td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ResDBT" Width="125px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Current Status:</td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ResCurrentStatus" Width="125px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Years on file:</td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ResYearsOnFile" Width="125px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Date of Incorporation:</td>
                                                        <td>
                                                            <ig:WebDatePicker ID="ResIncorporationDate" runat="server" NullText="" Width="130px">
                                                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                    SlideOpenDuration="1" />
                                                            </ig:WebDatePicker>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">State of Incorporation:</td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ResIncorporationState" Width="125px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">OFAC Match:</td>
                                                        <td>
                                                            <asp:CheckBox runat="server" ID="ResOFACMatch" />
                                                        </td>
                                                    </tr>
                                                    <%--                                               <div id="OFAC" runat="server" style="display:none;">--%>
                                                    <tr>
                                                        <td class="lblRight" valign="top">&nbsp;<asp:Label Text="OFAC Description:" ID="OFACLabel" Enabled="true" runat="server"
                                                            Style="text-align: right; vertical-align: text-top;"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="ResOFACDescription" Width="125px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%-- </div>--%>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <div class="title">
                                &nbsp;&nbsp;Valid Merchant Descriptors&nbsp;&nbsp;
                                    <asp:Button runat="server" ID="btnRefreshDesc" Text="Refresh List" OnClick="btnRefresh_Click"
                                        Enabled="true" />
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <uc3:wucDescriptor runat="server" ID="ValidDescriptors" ShowValidators="true" />
                                <br />
                                <br />
                                <asp:CheckBox runat="server" ID="AllowDescriptorOverride" Text="Allow Descriptor Override" />
                            </div>
                            <br />
                            <asp:Panel runat="server" ID="pnlCredit">
                                <div class="title">
                                    &nbsp;&nbsp;Credit Profile
                                    <hr class="line" />
                                </div>
                                <div class="indentedcontent20">
                                    <br />
                                    <uc2:wucOwnerUW ID="WucOwnerUW0" runat="server" />
                                    <uc2:wucOwnerUW ID="WucOwnerUW1" runat="server" />
                                    <uc2:wucOwnerUW ID="WucOwnerUW2" runat="server" />
                                    <uc2:wucOwnerUW ID="WucOwnerUW3" runat="server" />
                                    <%--PXP-3118 Rohit Thakur--%>
                                    <uc2:wucOwnerUW ID="WucOwnerUW4" runat="server" />
                                    <uc2:wucOwnerUW ID="WucOwnerUW5" runat="server" />
                                    <uc10:wucCorpBuzUW ID="wucCorpBuzUW1" runat="server" />
                                </div>
                                <br />
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlHierarchy" Visible="false">
                                <div class="title">
                                    &nbsp;&nbsp;Hierarchy Approval Sign Off Level
                                    <hr class="line" />
                                </div>
                                <div class="indentedcontent20">
                                    <br />
                                    <asp:Button runat="Server" ID="btnRequest" Text="Request Approval" OnClick="btnRequest_Click"
                                        Enabled="false" />
                                    <br />
                                    <br />
                                    <asp:CheckBox runat="server" ID="HierarchyApprovalSignOff" Text="ApprovalSignoff"
                                        Enabled="false" />
                                </div>
                                <br />
                            </asp:Panel>
                            <div class="title">
                                &nbsp;&nbsp;Overrides
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox Text="UW Rate Override" ID="UWOverride" runat="server"></asp:CheckBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox Text="UW Mult Override" ID="UWMultOverride" runat="server"></asp:CheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <div class="title">
                                &nbsp;&nbsp;Underwriting Notes
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <asp:TextBox ID="NotesUW" runat="server" Height="100px" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </div>
                            <br />
                            <asp:Panel ID="pnlMultipleLinks" runat="server">
                                <div class="title">
                                    &nbsp;&nbsp;Multiple Account Link Info &nbsp;&nbsp;
                                    <asp:Button runat="server" ID="btnRefreshList" Text="Refresh List" OnClick="btnRefreshList_Click"
                                        Enabled="true" />
                                    <hr class="line" />
                                </div>
                                <div class="indentedcontent20">
                                    <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="false">
                                        No data...
                                    </asp:Panel>
                                    <asp:Panel ID="pnlRecords" runat="server" ScrollBars="Horizontal">
                                        <table width="100%">
                                            <tr>
                                                <td class="lblLeft">Page Size:
                                                    <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True">5</asp:ListItem>
                                                        <asp:ListItem>10</asp:ListItem>
                                                        <asp:ListItem>25</asp:ListItem>
                                                        <asp:ListItem>50</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="lblRight">
                                                    <!-- Added by chandra for PXP-2716: to show the aggregate volumes on UI-->
                                                    <asp:Label ID="lblCCVolume" runat="server" Text=""></asp:Label>
                                                    <asp:Label ID="lblACHVolume" runat="server" Text=""></asp:Label>
                                                    <asp:Label ID="lblYTDVolume" runat="server" Text=""></asp:Label>
                                                    <!----end---->
                                                    <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:UpdatePanel runat="server" ID="pnl1">
                                                        <ContentTemplate>
                                                            <asp:GridView ID="grdLinks" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                                AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="grd1_PageIndexChanging"
                                                                AllowSorting="true" PageSize="5" OnSorting="grd1_Sorting"
                                                                DataSourceID="ods">
                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="ZID" SortExpression="ID">
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("LinkUID") + "&Adding=false"  %>'
                                                                                runat="server" ID="hypZID" Text='<%# Eval("ID") %>'></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="MLE">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="LegalName" Text='<%# Eval("LegalName")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="DBA">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="DBAName" runat="server" Text='<%# Eval("DBA")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Contact #">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="ContactNo" Text='<%# Eval("ContactNo")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="DBA #">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="DBANo" Text='<%# Eval("DBANo")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="FAX #">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="FAXNo" Text='<%# Eval("BusinessFax")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Customer Service Phone #">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="CustPhone" Text='<%# Eval("CustPhone")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Mailing Address">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="MailingAddress" Text='<%# Eval("MailingAddress")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Business Address">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="BusinessAddress" Text='<%# Eval("BusinessAddress")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Website">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="Website" runat="server" Text='<%# Eval("Website")%>' ToolTip='<%# Eval("WebsiteURL")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Email Address">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="EmailAddress" Text='<%# Eval("EmailAddress")%>' ToolTip='<%# Eval("PrimaryContactEmailAddress")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Tax ID">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="TaxID" Text='<%# Eval("TaxID")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Owner Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="Name" Text='<%# Eval("OwnerName")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Owner SSN">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="SSN" Text='<%# Eval("OwnerSSN")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Owner Phone">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="Phone" Text='<%# Eval("OwnerPhone")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%-- Chandra: Added new field Owner Address for PXP-2758--%>
                                                                    <asp:TemplateField HeaderText="Owner Address">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="OwnerAddress" Text='<%# Eval("OwnerAddress")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <%--end of PXP-2758--%>
                                                                    <asp:TemplateField HeaderText="Contact Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="ContactName" Text='<%# Eval("Contact")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Bank Account #">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="BankContact" Text='<%# Eval("BankAccount")%>' ToolTip='<%# Eval("BankAccountNumber")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="YTD Volume" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="YTDVolume" Text='<%# Eval("YTDVolume")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Created Date" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="CreatedDate" Text='<%# Eval("CreatedDate")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Status">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="Status" Text='<%# Eval("Status")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Closure Code">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="ClosureCode" Text='<%# Eval("ClosureCode")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Cancellation Date" ItemStyle-HorizontalAlign="Right">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="CancellationDate" Text='<%# Eval("CancellationDate")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <FooterStyle CssClass="footer" />
                                                                <PagerStyle CssClass="pgr" />
                                                                <AlternatingRowStyle CssClass="alt" />
                                                            </asp:GridView>
                                                            <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetMultilinkPaging" EnablePaging="True"
                                                                MaximumRowsParameterName="PageSize" SelectCountMethod="GetMultilinkPagingCount"
                                                                StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
                                                                OnSelecting="ods_Selecting" TypeName="DatamerchantAppPaging">
                                                                <SelectParameters>
                                                                    <asp:Parameter Name="prms" Type="Object" />
                                                                    <asp:Parameter Name="PageSize" Type="Int32" />
                                                                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                                                                    <asp:Parameter Name="merchantMID" Type="string" />
                                                                    <asp:Parameter Name="achID" Type="string" />
                                                                </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:DropDownList ID="ddlRange" runat="server">
                                                                    <asp:ListItem Value="Current">Export Current Page</asp:ListItem>
                                                                    <asp:ListItem Value="All">Export All Pages</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="ddlSelectExportFormat" runat="server">
                                                                    <asp:ListItem Value="excel">in Excel Format</asp:ListItem>
                                                                    <asp:ListItem Value="pdf">in PDF Format</asp:ListItem>
                                                                    <asp:ListItem Value="csv">in CSV Format</asp:ListItem>
                                                                    <asp:ListItem Value="tab">in Tab Delimited Format</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:Button ID="btnExport1" runat="server" CssClass="button" OnClick="btnExport1_Click"
                                                                    Text="Begin Export" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="Panel1" runat="server" Height="" Width="" HorizontalAlign="Right">
                                        <asp:Label runat="server" ID="lblRefresh"></asp:Label>
                                    </asp:Panel>
                                </div>
                                <br />
                            </asp:Panel>
                            <br />
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <uc4:wucCashAdvance runat="server" ID="CashAdvance" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </Template>
                    </ig:ContentTabItem>
                    <ig:ContentTabItem runat="server" EnableDynamicUpdatePanel="False" Text="Verification">
                        <Template>
                            <br />
                            <div class="title">
                                &nbsp;&nbsp;Checklist
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <asp:Panel runat="server" ID="pnl3DE" Visible="false">
                                    <div class="bucketbdy" style="padding-bottom: 10px;">
                                        <div class="bucketfooterleft">
                                            <b>
                                                <asp:Label ID="lbl3DEStatus" runat="server"></asp:Label></b>
                                        </div>
                                        <div style="display: none; float: right" id="pnlBusy">
                                            <asp:Image runat="server" ID="imgBusy" Style="width: 30px;" ImageUrl="~/Images/loading.gif" />
                                        </div>
                                        <div class="bucketfooteright" id="btnRequest3DE">
                                            <asp:Button runat="server" ID="Response3DE" Enabled="false" UseSubmitBehavior="false" Text="Get Response" OnClick="Response3DE_Click" OnClientClick="DisplayProcessing()" />
                                            &nbsp; &nbsp;
                                            <asp:Button runat="server" ID="Request3DE" Enabled="false" UseSubmitBehavior="false" Text="Get From 3DE" OnClick="Request3DE_Click" OnClientClick="DisplayProcessing()" />
                                        </div>
                                    </div>
                                    <br />
                                </asp:Panel>
                                <asp:GridView ID="grdChecklist" runat="server" AutoGenerateColumns="false" CssClass="mGrid" DataKeyNames="VendorID"
                                    OnRowDataBound="grdCheck_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="CheckListItem" HeaderText="Service" ItemStyle-Width="30px" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="70px">
                                            <HeaderTemplate>
                                                Reviewed
                                                <center>
                                                    <asp:CheckBox ID="chkAll" runat="Server" onclick="CheckAll(this,'Checked')" />
                                                </center>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Checked" runat="Server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Waived" ItemStyle-HorizontalAlign="center" ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Exception" runat="Server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Underwriting Notes" ItemStyle-HorizontalAlign="left" ItemStyle-Width="750px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="Notes" runat="Server" Rows="2" TextMode="MultiLine" Width="99%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="3DE Response Log" ItemStyle-HorizontalAlign="left" ItemStyle-Width="500px">
                                            <ItemTemplate>
                                                <asp:Label ID="ResponseLog" runat="Server" Width="99%" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="100px">
                                            <HeaderTemplate>
                                                Select All
                                                <center>
                                                    <asp:CheckBox ID="selectAll" runat="Server" onclick="CheckAll(this,'chkSelect')" />
                                                </center>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="Server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CheckListUID" Visible="false" />
                                        <asp:BoundField DataField="VendorID" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </Template>
                    </ig:ContentTabItem>
                    <ig:ContentTabItem runat="server" EnableDynamicUpdatePanel="False" Text="Risk Evaluation">
                        <Template>
                            <asp:Panel ID="pnlUWFulfillment" runat="server">
                                <div class="title">
                                    &nbsp;&nbsp;Extended Delivery
                                    <hr class="line" />
                                </div>
                                <div class="indentedcontent20">
                                    <table>
                                        <tr>
                                            <th class="auto-style1"></th>
                                            <th class="auto-style1">Total Volume(%)</th>
                                            <th class="auto-style1">NDX Days</th>
                                        </tr>
                                        <tr>
                                            <td>FulFillment Period 1</td>
                                            <td class="periodvolume">
                                                <ig:WebPercentEditor MaxValue="100" ID="Period1Volume" runat="server" ValueText="0" MinValue="0" MaxLength="5"
                                                    MinDecimalPlaces="2" Width="85px" ClientIDMode="Static">
                                                </ig:WebPercentEditor>
                                            </td>

                                            <td class="periodndx">
                                                <asp:TextBox ID="Period1NDXDays" runat="server" MaxLength="50" Width="85px"></asp:TextBox></td>

                                            <td style="width: 50px"></td>
                                            <td>Refund Days</td>
                                            <td class="periodndx">
                                                <asp:TextBox ID="RefundDays" Style="text-align: right;" runat="server" MaxLength="50" Width="85px"></asp:TextBox></td>
                                            <td style="width: 50px"></td>
                                            <td>Chargeback Days</td>
                                            <td class="periodndx">
                                                <asp:TextBox ID="ChargebackDays" Style="text-align: right;" runat="server" MaxLength="50" Width="85px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td>FulFillment Period 2</td>
                                            <td class="periodvolume">
                                                <ig:WebPercentEditor MaxValue="100" ID="Period2Volume" runat="server" ValueText="0" MinValue="0" MaxLength="5"
                                                    MinDecimalPlaces="2" Width="85px" ClientIDMode="Static">
                                                </ig:WebPercentEditor>
                                            </td>
                                            <td class="periodndx">
                                                <asp:TextBox ID="Period2NDXDays" runat="server" MaxLength="50" Width="85px"></asp:TextBox></td>

                                            <td style="width: 50px"></td>
                                            <td>Refund Volume (%)</td>
                                            <td class="periodndx">
                                                <ig:WebPercentEditor MaxValue="100" ID="RefundVolume" runat="server" ValueText="0" MinValue="0" MaxLength="5"
                                                    MinDecimalPlaces="2" Width="85px" ClientIDMode="Static">
                                                </ig:WebPercentEditor>
                                                <td style="width: 50px"></td>
                                                <td>Chargeback Volume (%)</td>
                                                <td class="periodndx">
                                                    <ig:WebPercentEditor MaxValue="100" ID="ChargebackVolume" runat="server" ValueText="0" MinValue="0" MaxLength="5"
                                                        MinDecimalPlaces="2" Width="85px" ClientIDMode="Static">
                                                    </ig:WebPercentEditor>
                                        </tr>
                                        <tr>
                                            <td>FulFillment Period 3</td>
                                            <td class="periodvolume">
                                                <ig:WebPercentEditor MaxValue="100" ID="Period3Volume" runat="server" ValueText="0" MinValue="0" MaxLength="5"
                                                    MinDecimalPlaces="2" Width="85px" ClientIDMode="Static">
                                                </ig:WebPercentEditor>
                                            </td>
                                            <td class="periodndx">
                                                <asp:TextBox ID="Period3NDXDays" runat="server" MaxLength="50" Width="85px"></asp:TextBox></td>
                                            <td style="width: 50px"></td>
                                            <td>Risk Exposure</td>
                                            <td class="periodndx">
                                                <asp:TextBox ID="RiskExposure" Style="text-align: right;" runat="server" MaxLength="50" Width="85px"></asp:TextBox></td>
                                            <td style="width: 50px"></td>
                                        </tr>
                                        <tr>
                                            <td>Total</td>
                                            <td>
                                                <ig:WebPercentEditor ID="TotalPeriodVolume" runat="server" ValueText="0" MinValue="0" MaxLength="5"
                                                    MinDecimalPlaces="2" Width="85px" ClientIDMode="Static" ReadOnly="true">
                                                </ig:WebPercentEditor>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </Template>
                    </ig:ContentTabItem>
                    <ig:ContentTabItem runat="server" EnableDynamicUpdatePanel="False" Text="Financials">
                        <Template>
                            <asp:Panel ID="Panel2" runat="server">
                                <br />
                                <div class="title">
                                    &nbsp;&nbsp;Financial Statements
                                    <hr class="line" />
                                </div>
                                <div class="indentedcontent20">
                                    <asp:GridView ID="grdFinancial" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                        Font-Names="verdana" Font-Size="X-Small" OnRowCommand="grdFinance_RowCommand"
                                        OnRowDataBound="grdFinance_RowDataBound" ShowFooter="false" Width="99.5%">
                                        <FooterStyle CssClass="footer" />
                                        <PagerStyle CssClass="pgr" />
                                        <AlternatingRowStyle CssClass="alt" />
                                        <PagerSettings FirstPageText="«" LastPageText="»" Mode="NumericFirstLast" />
                                        <RowStyle VerticalAlign="Top" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="lnkAdd" runat="server" CausesValidation="false" CommandName="AddUW"
                                                        ImageUrl="~/Images/add2.png" ToolTip="Add" />
                                                    <asp:ImageButton ID="lnkCancel" runat="server" CausesValidation="false" CommandName="CancelUW"
                                                        ImageUrl="~/images/Cancel.jpg" ToolTip="Cancel" />
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkEdit" runat="server" CausesValidation="false" CommandName="EditUW"
                                                        ImageUrl="~/images/edit.png" ToolTip="Edit" />
                                                    <asp:ImageButton ID="lnkUpdate" runat="server" CausesValidation="true" CommandName="UpdateUW"
                                                        ImageUrl="~/images/disk_blue.png" ToolTip="Update" />
                                                    <asp:ImageButton ID="lnkCancel" runat="server" CausesValidation="false" CommandName="CancelUW"
                                                        ImageUrl="~/images/Cancel.jpg" ToolTip="Cancel" />
                                                </ItemTemplate>
                                                <ItemStyle Width="40px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period">
                                                <FooterTemplate>
                                                    <ig:WebDatePicker ID="Period" runat="server" DataMode="EditModeText" DisplayModeFormat="MM/yyyy"
                                                        EditModeFormat="MM/yyyy" NullText="" Width="50px">
                                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                            SlideOpenDuration="1" />
                                                    </ig:WebDatePicker>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPeriod" runat="server" Text='<% #Eval("Period")%>' Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblPeriod1" runat="server" Text='<% #Eval("Period")%>' Width="50px"></asp:Label>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Current Asset">
                                                <FooterTemplate>
                                                    <ig:WebNumericEditor ID="CurrentAsset" runat="server" MinDecimalPlaces="4" Value='<% #Eval("CurrentAsset")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAsset" runat="server" Width="70px"> </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <ig:WebNumericEditor ID="CurrentAsset1" runat="server" MinDecimalPlaces="4" Value='<% #Eval("CurrentAsset")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Current Liability">
                                                <FooterTemplate>
                                                    <ig:WebNumericEditor ID="CurrentLiability" runat="server" MinDecimalPlaces="4" Value='<% #Eval("CurrentLiability")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLiability" runat="server" Width="70px"> </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <ig:WebNumericEditor ID="CurrentLiability1" runat="server" MinDecimalPlaces="4"
                                                        Value='<% #Eval("CurrentLiability")%>' ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Current Ratio" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Working Capital" ReadOnly="true" />
                                            <asp:TemplateField HeaderText="Total Asset">
                                                <FooterTemplate>
                                                    <ig:WebNumericEditor ID="TotalAsset" runat="server" MinDecimalPlaces="4" Value='<% #Eval("TotalAsset")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalAsset" runat="server" Width="70px"> </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <ig:WebNumericEditor ID="TotalAsset1" runat="server" MinDecimalPlaces="4" Value='<% #Eval("TotalAsset")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Liability">
                                                <FooterTemplate>
                                                    <ig:WebNumericEditor ID="TotalLiability" runat="server" MinDecimalPlaces="4" Value='<% #Eval("TotalLiability")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalLiability" runat="server" Width="70px"> </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <ig:WebNumericEditor ID="TotalLiability1" runat="server" MinDecimalPlaces="4" Value='<% #Eval("TotalLiability")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Debt Ratio" ReadOnly="true" />
                                            <asp:BoundField HeaderText="Networth" ReadOnly="true" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblNoFinance" runat="server" Text="No Data..."></asp:Label>
                                    <br />
                                    <div id="div3" class="bucketfooter">
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:LinkButton ID="lnkFinance" runat="server" CommandName="Finance" OnClick="btnExport_Click">
                                                        <span style="height: 25px; vertical-align: middle;">
                                                            <asp:Image ID="Image4" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span>
                                                    </asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkAddFinance" runat="server" CommandName="Finance" OnClick="lnkAdd_Click">Add New Row</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                                <div class="title">
                                    &nbsp;&nbsp;Income Statements
                                    <hr class="line" />
                                </div>
                                <div class="indentedcontent20">
                                    <asp:GridView ID="grdIncome" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                        Font-Names="verdana" Font-Size="X-Small" OnRowCommand="grdIncome_RowCommand"
                                        OnRowDataBound="grdIncome_RowDataBound" ShowFooter="false" Width="99.5%">
                                        <FooterStyle CssClass="footer" />
                                        <PagerStyle CssClass="pgr" />
                                        <AlternatingRowStyle CssClass="alt" />
                                        <PagerSettings FirstPageText="«" LastPageText="»" Mode="NumericFirstLast" />
                                        <RowStyle VerticalAlign="Top" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="lnkAdd" runat="server" CausesValidation="false" CommandName="AddUW"
                                                        ImageUrl="~/Images/add2.png" ToolTip="Add" />
                                                    <asp:ImageButton ID="lnkCancel" runat="server" CausesValidation="false" CommandName="CancelUW"
                                                        ImageUrl="~/images/Cancel.jpg" ToolTip="Cancel" />
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkEdit" runat="server" CausesValidation="false" CommandName="EditUW"
                                                        ImageUrl="~/images/edit.png" ToolTip="Edit" />
                                                    <asp:ImageButton ID="lnkUpdate" runat="server" CausesValidation="true" CommandName="UpdateUW"
                                                        ImageUrl="~/images/disk_blue.png" ToolTip="Update" />
                                                    <asp:ImageButton ID="lnkCancel" runat="server" CausesValidation="false" CommandName="CancelUW"
                                                        ImageUrl="~/images/Cancel.jpg" ToolTip="Cancel" />
                                                </ItemTemplate>
                                                <ItemStyle Width="40px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period">
                                                <FooterTemplate>
                                                    <ig:WebDatePicker ID="Period" runat="server" DataMode="EditModeText" DisplayModeFormat="MM/yyyy"
                                                        EditModeFormat="MM/yyyy" NullText="" Width="50px">
                                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                            SlideOpenDuration="1" />
                                                    </ig:WebDatePicker>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPeriod" runat="server" Text='<% #Eval("Period")%>' Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblPeriod1" runat="server" Text='<% #Eval("Period")%>' Width="50px"></asp:Label>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Revenue/ Gross Sales">
                                                <FooterTemplate>
                                                    <ig:WebNumericEditor ID="GrossSales" runat="server" MinDecimalPlaces="4" Value='<% #Eval("GrossSales")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGrossSales" runat="server" Width="70px"> </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <ig:WebNumericEditor ID="GrossSales1" runat="server" MinDecimalPlaces="4" Value='<% #Eval("GrossSales")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Net Income">
                                                <FooterTemplate>
                                                    <ig:WebNumericEditor ID="NetIncome" runat="server" MinDecimalPlaces="4" Value='<% #Eval("NetIncome")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNetIncome" runat="server" Width="70px"> </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <ig:WebNumericEditor ID="NetIncome1" runat="server" MinDecimalPlaces="4" Value='<% #Eval("NetIncome")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblNoIncome" runat="server" Text="No Data..."></asp:Label>
                                    <br />
                                    <div id="div1" class="bucketfooter">
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:LinkButton ID="lnkIncome" runat="server" CommandName="Income" OnClick="btnExport_Click">
                                                        <span style="height: 25px; vertical-align: middle;">
                                                            <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span>
                                                    </asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkAddIncome" runat="server" CommandName="Income" OnClick="lnkAdd_Click">Add New Row</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <br />
                                <div class="title">
                                    &nbsp;&nbsp;Bank Statements
                                    <hr class="line" />
                                </div>
                                <div class="indentedcontent20">
                                    <asp:GridView ID="grdBank" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                        Font-Names="verdana" Font-Size="X-Small" OnRowCommand="grdBank_RowCommand" OnRowDataBound="grdBank_RowDataBound"
                                        ShowFooter="false" Width="99.5%">
                                        <FooterStyle CssClass="footer" />
                                        <PagerStyle CssClass="pgr" />
                                        <AlternatingRowStyle CssClass="alt" />
                                        <PagerSettings FirstPageText="«" LastPageText="»" Mode="NumericFirstLast" />
                                        <RowStyle VerticalAlign="Top" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="lnkAdd" runat="server" CausesValidation="false" CommandName="AddUW"
                                                        ImageUrl="~/Images/add2.png" ToolTip="Add" />
                                                    <asp:ImageButton ID="lnkCancel" runat="server" CausesValidation="false" CommandName="CancelUW"
                                                        ImageUrl="~/images/Cancel.jpg" ToolTip="Cancel" />
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkEdit" runat="server" CausesValidation="false" CommandName="EditUW"
                                                        ImageUrl="~/images/edit.png" ToolTip="Edit" />
                                                    <asp:ImageButton ID="lnkUpdate" runat="server" CausesValidation="true" CommandName="UpdateUW"
                                                        ImageUrl="~/images/disk_blue.png" ToolTip="Update" />
                                                    <asp:ImageButton ID="lnkCancel" runat="server" CausesValidation="false" CommandName="CancelUW"
                                                        ImageUrl="~/images/Cancel.jpg" ToolTip="Cancel" />
                                                </ItemTemplate>
                                                <ItemStyle Width="40px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Period">
                                                <FooterTemplate>
                                                    <ig:WebDatePicker ID="Period" runat="server" DataMode="EditModeText" DisplayModeFormat="MM/yyyy"
                                                        EditModeFormat="MM/yyyy" NullText="" Width="50px">
                                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                            SlideOpenDuration="1" />
                                                    </ig:WebDatePicker>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPeriod" runat="server" Text='<% #Eval("Period")%>' Width="50px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblPeriod1" runat="server" Text='<% #Eval("Period")%>' Width="50px"></asp:Label>
                                                </EditItemTemplate>
                                                <ControlStyle Width="50px" />
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Checking Balance">
                                                <FooterTemplate>
                                                    <ig:WebNumericEditor ID="CheckingBalance" runat="server" MinDecimalPlaces="4" Value='<% #Eval("CheckingBalance")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCheckingBalance" runat="server" Width="70px"> </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <ig:WebNumericEditor ID="CheckingBalance1" runat="server" MinDecimalPlaces="4" Value='<% #Eval("CheckingBalance")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Savings Balance">
                                                <FooterTemplate>
                                                    <ig:WebNumericEditor ID="SavingsBalance" runat="server" MinDecimalPlaces="4" Value='<% #Eval("SavingsBalance")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSavingsBalance" runat="server" Width="70px"> </asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <ig:WebNumericEditor ID="SavingsBalance1" runat="server" MinDecimalPlaces="4" Value='<% #Eval("SavingsBalance")%>'
                                                        ValueText="0" Width="70px">
                                                    </ig:WebNumericEditor>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <asp:Label ID="lblNoBank" runat="server" Text="No Data..."></asp:Label>
                                <br />
                                <div id="div2" class="bucketfooter">
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <asp:LinkButton ID="lnkBank" runat="server" CommandName="Bank" OnClick="btnExport_Click">
                                                    <span style="height: 25px; vertical-align: middle;">
                                                        <asp:Image ID="Image3" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span>
                                                </asp:LinkButton>
                                                &nbsp;&nbsp;
                                                <asp:LinkButton ID="lnkAddBank" runat="server" CommandName="Bank" OnClick="lnkAdd_Click">Add New Row</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                            <br />
                        </Template>
                    </ig:ContentTabItem>
                    <ig:ContentTabItem runat="server" EnableDynamicUpdatePanel="False" Text="Financial Scorecard">
                        <Template>
                            <asp:Panel ID="pnlSCoreCards" runat="server">
                                <br />
                                &nbsp;<asp:Button runat="server" ID="btnNew" Text="New Scorecard" OnClick="btnNew_Click" />
                                &nbsp;<asp:Button runat="server" ID="btnRefresh1" Text="Refresh" OnClick="btnRefresh_Click1" />
                                <div style="height: 10px;">
                                    <!-- -->
                                </div>
                                <div class="title">
                                    &nbsp;&nbsp;Financial Scorecard
                                    <hr class="line" />
                                </div>
                                <div class="indentedcontent20">
                                    <sc1:wucUWMerchantScoreCards ID="MerchantScoreCards" runat="server" />
                                </div>
                            </asp:Panel>
                        </Template>
                    </ig:ContentTabItem>
                </Tabs>
            </ig:WebTab>
        </asp:Panel>
        <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="700px" Width="900px"
            Modal="true" InitialLocation="Centered" UseBodyAsParent="true" WindowState="Hidden" MaintainLocationOnScroll="true">
            <ContentPane>
                <Template>
                    <sc2:wucUWFinancialScoreCardGrid ID="FinancialScoreCardGrid" runat="server" />
                    <br />
                </Template>
            </ContentPane>
            <Header CaptionText="Financial Scorecard">
            </Header>
        </ig:WebDialogWindow>
        <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="180px" Width="330px"
            Modal="True" InitialLocation="Centered" UseBodyAsParent="True" WindowState="Hidden">
            <ContentPane>
                <Template>
                    <table width="100%">
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="padding-left: 3px; padding-right: 3px;">
                                <asp:Label runat="server" ID="lbl"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button runat="server" ID="Button1" Text="Ok" OnClick="btnGet1_Click" Visible="false" />
                                <asp:Button runat="server" ID="btnOk" Text="Ok" OnClick="btnGet1_Click" />
                                <asp:Button runat="server" ID="btnNo" Text="Cancel" OnClick="btnNo_Click" />
                                <asp:Button runat="server" ID="btnGetOld" Text="Get existing report" OnClick="btnGetOld_Click" />

                                <script type="text/javascript">
                                    // this prevents the submit button from being clicked multiple times.
                                    $('#<%= btnOk.ClientID %>').on("click", function (event) {
                                        $(this).attr('value', 'Processing...');
                                        $(this).unbind(event);

                                        <%= GetSubmitPostBack() %>;
                                    });
                                </script>
                            </td>
                        </tr>
                    </table>
                </Template>
            </ContentPane>
        </ig:WebDialogWindow>
    </div>
    <div id="popup-wrapper" style="width: 600px; height: 500px; padding: 0px; display: none">
        <div class="ig_Header igdw_HeaderArea" style="overflow: hidden;">
            <table cellpadding="0" cellspacing="0" style="width: 100%; height: 100%; table-layout: fixed;">
                <tr>
                    <td class="igdw_HeaderCornerLeft">&nbsp;</td>
                    <td class="igdw_HeaderContent" style="white-space: nowrap; overflow: hidden;"><span class="igdw_HeaderCaption">MCC Search</span></td>
                    <td class="igdw_HeaderContent igdw_HeaderButtonArea" style="width: 15px; text-align: right; white-space: nowrap;">
                        <img alt="Close" src="../ig_res/Default/images/igdw_Close.gif" onclick="closeModal()" /></td>
                    <td class="igdw_HeaderCornerRight">&nbsp;</td>
                </tr>
            </table>
        </div>
        <div class="dialog" style="padding-top: 10px; padding-bottom: 5px; display: none">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server" Visible="false">
                <ContentTemplate>
                    <fieldset>
                        <legend>MCC Search</legend>
                        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                            <table width="100%">
                                <tr>
                                    <td class="lblRight">MCC:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMCC" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">MCC Description:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMCCDesc" runat="server"></asp:TextBox>
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
                        <legend>Results</legend>
                        <div style="width: 100%; height: 300px; overflow: scroll">
                            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                Font-Names="verdana" Font-Size="X-Small" OnRowDataBound="grd_RowDataBound">
                                <AlternatingRowStyle CssClass="alt" />
                                <PagerStyle CssClass="pgr" />
                                <FooterStyle CssClass="footer" />
                                <Columns>
                                    <asp:BoundField DataField="UID" Visible="false" HeaderText="UID"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href="javascript:selectMCCCode('<%# DataBinder.Eval(Container.DataItem, "Name").ToString().Replace("'", "\\'") %>','<%# DataBinder.Eval(Container.DataItem, "Description").ToString().Replace("'", "\\'") %>')">Select</a>
                                            <%-- <asp:LinkButton ID="btnSelect" runat="server" CommandName="Select" Text="Select" OnClientClick="javascript:closeModal()"></asp:LinkButton>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Label ID="lblNoRecords" runat="server" Text="No data..."></asp:Label>
                        </div>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <script type="text/javascript">
        function ChangeDate() {
            if (document.getElementById('<% =ConditionalApproval.ClientID %>').checked) {
                document.getElementById('<% =date.ClientID %>').style.display = 'inline';
                document.getElementById('<% =ConditionalDueDate.ClientID %>').style.display = 'block';
            } else {
                document.getElementById('<% =date.ClientID %>').style.display = 'none';
                document.getElementById('<% =ConditionalDueDate.ClientID %>').style.display = 'none';
            }
        }

        function ChangePhysicalVisit() {
            if (document.getElementById('<% =PhysicalSiteVisit.ClientID %>').checked) {
                document.getElementById('<% =Visit.ClientID %>').style.display = 'inline';
                document.getElementById('<% =PhysicalVisitOn.ClientID %>').style.display = 'block';
            } else {
                document.getElementById('<% =Visit.ClientID %>').style.display = 'none';
                document.getElementById('<% =PhysicalVisitOn.ClientID %>').style.display = 'none';
            }
        }

        function ChangeOFAC() {
            var ofac = document.getElementById('<% =ResOFACMatch.ClientID %>');

            if (ofac.checked) {
                document.getElementById('<% =OFACLabel.ClientID %>').style.display = 'inline';
                document.getElementById('<% =ResOFACDescription.ClientID %>').style.display = 'inline';
            } else {
                document.getElementById('<% =OFACLabel.ClientID %>').style.display = 'none';
                document.getElementById('<% =ResOFACDescription.ClientID %>').style.display = 'none';
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="HeadPlaceHolder">
    <style type="text/css">
        .auto-style1 {
            height: 20px;
        }
    </style>
</asp:Content>

