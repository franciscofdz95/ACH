<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true" Inherits="frmDeployment" Title="Deployment" CodeBehind="frmDeployment.aspx.cs" ValidateRequest="false" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucServices.ascx" TagName="wucServices" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/wucCreateUser.ascx" TagName="wucCreateUser" TagPrefix="uc3" %>
<%@ Register TagName="wucEquipment" TagPrefix="uc4" Src="~/UserControls/wucEquipment.ascx" %>
<%@ Register Src="../UserControls/wucMerchantCardCurrency.ascx" TagName="wucCardCurrency" TagPrefix="cc" %>
<%@ Register Src="../UserControls/wucCompass.ascx" TagName="wucCompass" TagPrefix="co" %>
<%@ Register Src="../UserControls/wucCBMSPlus.ascx" TagName="wucCBMSPlus" TagPrefix="cb" %>
<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc7" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript"> 
        // PXP-14570 - bug for PXP-14480
        function cTrig()
        {
            if (document.getElementById('ContentPlaceHolder1_chkPCCSSwitch').checked == true)
            {
                var box = confirm("Once checked, the box cannot be unchecked. Are you sure you want to check this box?");
                if (box == true)
                { return true; }
            }
            else
            {
                return false;
            }
            document.getElementById('ContentPlaceHolder1_chkPCCSSwitch').checked = false;

            return false;
        }

        // Added by Chandra for PXP-7898
        function WebImageSave_Click(oButton, oEvent) {
            var isNutra = $("#<%=IsNutra.ClientID %>");
            var oldStatus = $("#<%=hidStatus.ClientID %>");
            var newStatus = $("#<%=StatusUID.ClientID %>");
            var isNutraMerchant = $("#<%=hidIsNutraMerchant.ClientID %>");

            if ('<%=ACHStatus.Visible%>' == "True")
            { newStatus = $("#<%=ACHStatusUID.ClientID %>"); }

            if (oldStatus.val().toUpperCase() != newStatus.val().toUpperCase() && newStatus.val().toUpperCase() == "73FC4B27-98D4-40EA-B9FC-1370C564CB12") {
                if (MCCcode.val() == "5968" || MCCcode.val() == "5964") {
                    var conf = alert("Review Tangible Trial checkbox for Tangible merchants");
                }
            }
            //PXP-9051 RThakur
            var selectedNewStatus = $(newStatus).find('option:selected').val().toUpperCase();
            var isNewVerticalandMarketsValue = $("#<%=IsNewVerticalandMarkets.ClientID %>").val();
            var isNewVertical = $("#<%=IsNewVertical.ClientID %>").val();
            var isIsNewVertical = false;
            if (isNewVertical == "True") {
                isIsNewVertical = true;
            }
            if ((isNewVerticalandMarketsValue == "false") && isIsNewVertical && selectedNewStatus == "2FDDA5E4-E80A-4155-8CB2-D5200992FA81" && oldStatus.val().toUpperCase() != newStatus.val().toUpperCase())
            {
                var confVerticals = confirm("<%=PaymentXP.BusinessObjects.Constant.NewVerticalandVerticalMarketsChecked%>");
                if (!confVerticals) {
                    oEvent.cancel = true;
                }
            }


            // PXP-9348 RThakur >> Start 
            var hidCRMStatus = document.getElementById('<%= this.hiddenCrmStatus.ClientID %>').value;
            var isCRMAcceptTrans = document.getElementById('<%= this.hiddenAcceptTransaction.ClientID %>').value;
            if (oldStatus.val().toUpperCase() != newStatus.val().toUpperCase() && newStatus.val().toUpperCase() == "<%=PaymentXP.BusinessObjects.Constants.QUEUESTATUS_CU_RECEIVED.ToUpper()%>") {
                if (isNutraMerchant.val().toUpperCase() == "TRUE") {
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


        }
    </script>
    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <table width="100%">
            <tr>
                <td colspan="2">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
                    <uc7:wucMessage ID="WucMessage1" runat="server" />
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
                                            <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add Equipment" CommandName="Add"
                                                AccessKey="a" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
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
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <igtxt:WebImageButton ID="btnMerchantLogin" runat="server" Text="Merchant Login"
                                                        AccessKey="L" CausesValidation="False" OnClick="btnMerchantLogin_Click">
                                                        <%--<ClientSideEvents Click="ShowCreateUser" />--%>
                                                        <Appearance>
                                                            <Image Url="../Images/document_view.png" />
                                                        </Appearance>
                                                    </igtxt:WebImageButton>
                                                    <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="350px" Width="700px"
                                                        Modal="True" InitialLocation="Centered" WindowState="Hidden" Moveable="false" onstatechanged="WebDialogWindow2_StateChanged"  >
                                                        <AutoPostBackFlags WindowStateChange="On" />
                                                        <ContentPane>
                                                            <Template>
                                                                <uc3:wucCreateUser ID="WucCreateUser1" runat="server" />
                                                            </Template>
                                                        </ContentPane>
                                                        <Header CaptionText="Login Information">
                                                        </Header>
                                                    </ig:WebDialogWindow>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                    <asp:HiddenField runat="server" ID="hidStatus" />
                    <asp:HiddenField runat="server" ID="IsNutra" />
                    <asp:HiddenField runat="server" ID="IsNewVerticalandMarkets" />
                    <asp:HiddenField runat="server" ID="IsNewVertical" />
                    <asp:HiddenField runat="server" ID="hidIsNutraMerchant" />
                    <asp:HiddenField runat="server" ID="hiddenCrmStatus" />
                    <asp:HiddenField runat="server" ID="hiddenAcceptTransaction" />
                    <asp:HiddenField runat="server" ID="hidOldCRMID" />
                    <asp:Label ID="Label1" ForeColor="red" runat="server" Text=""></asp:Label>
                    <asp:Panel ID="pnlDetail" runat="server" Height="" Width="">
                        <fieldset>
                            <legend>Terminals</legend>
                            <asp:Panel runat="server" ID="pnlTerminals">
                                <table width="100%">
                                    <tr>
                                        <td align="right">
                                            <%--Code Removed for PXP-7621--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc4:wucEquipment ID="WucEquipment" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </fieldset>
                        <br />
                        <fieldset>
                            <legend>Deployment Notes</legend>
                            <asp:TextBox ID="DeploymentNotes" runat="server" Width="99%" TextMode="MultiLine" Rows="4"></asp:TextBox>
                            <br />
                        </fieldset>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td valign="top" style="width: 50%;">
                                    <asp:Panel ID="pnlMerchantAdditionalInfo" runat="server" Height="" Width="">
                                        <!--PXP-8982 by Sanidhya-->
                                        <asp:UpdatePanel ID="pnlNutraUpdate" runat="server">
                                            <ContentTemplate>
                                                <fieldset style="height: 60px;">
                                                    <legend>
                                                        <asp:Label runat="server" ID="lblStatus">Application Status</asp:Label>
                                                    </legend>
                                                    <asp:Panel ID="CCStatus" runat="server">
                                                        <br />
                                                        &nbsp;Status:&nbsp;<asp:DropDownList ID="StatusUID" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="StatusUID"
                                                            Display="None" ErrorMessage="Please select application status." Operator="NotEqual"
                                                            ValueToCompare="-1"></asp:CompareValidator>
                                                    </asp:Panel>
                                                    <asp:Panel ID="ACHStatus" runat="server" Visible="false">
                                                        <br />
                                                        &nbsp;ACH/DD Status: &nbsp;                                          
                                                <asp:DropDownList ID="ACHStatusUID" runat="server">
                                                </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ACHStatusUID"
                                                            ValidationGroup="UWValid" Display="dynamic" Text="*" ErrorMessage="Please select a Status."
                                                            Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                                                    </asp:Panel>
                                                </fieldset>
                                                <fieldset style="height: 380px;">
                                                    <legend>PaymentXP</legend>
                                                    <table width="65%">
                                                        <tr>
                                                            <td class="lblRight" style="width: 150px">Mode:
                                                            </td>
                                                            <td class="lblLeft">
                                                                <asp:DropDownList ID="GatewayModeUID" runat="server" Width="150px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight" style="width: 150px">Settlement Time:
                                                            </td>
                                                            <td class="lblLeft">
                                                                <asp:DropDownList ID="SettlementSchedule" runat="server" Width="150px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight" style="width: 150px">Classification:
                                                            </td>
                                                            <td class="lblLeft">
                                                                <asp:DropDownList ID="Classification" runat="server" Width="150px">
                                                                    <asp:ListItem Value="59" Selected="True">Ecommerce</asp:ListItem><%--Change the Default value of dropdown--%>
                                                                    <asp:ListItem Value="00">Retail</asp:ListItem>
                                                                    <asp:ListItem Value="08">MOTO</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <b>Payment Types</b>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="GatewayAllowCC" runat="server" Text="Gateway Credit Card" Width="150px" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="GatewayAllowACH" runat="server" Text="Gateway ACH" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="GatewayAllowPayPal" runat="server" Text="Gateway PayPal" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="GatewayAllowC21" runat="server" Text="Gateway Check21" Width="150px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">PayPal Email
                                                        <asp:TextBox ID="PayPalEmail" runat="server" Enabled="False" Width="300px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <b>Transaction Types</b>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="AllowSales" runat="server" Text="Allow Sales" ToolTip="Allows sales, auth only, capture, and void" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="AllowCredits" runat="server" Text="Allow Credits" Width="150px"
                                                                    ToolTip="Allows credits and blind credits" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="AllowBlindCredits" runat="server" Text="Allow Blind Credits" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="AllowTips" runat="server" Text="Allow Tips" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <asp:Panel ID="ReceiptsPanel" runat="server">
                                                            <tr>
                                                                <td>
                                                                    <b>Receipts</b>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="ReceiptEmailCustomer" runat="server" Text="Email Customer?" Width="150px" />
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="ReceiptEmailMerchant" runat="server" Text="Email Merchant?" />
                                                                </td>
                                                            </tr>
                                                        </asp:Panel>
                                                        <tr>
                                                            <td>
                                                                <b>Hosted Payment Page Fields</b>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:CheckBox runat="server" ID="chkEditableAmount" Text=" Trans Amount Editable" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <b>Additional Gateway Features</b>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox runat="server" ID="chkIsCOFEnabled" Text="Credential on File" Enabled="false" />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox runat="server" ID="chkIsPartialAuthEnabled" Text="Partial Authorization (Retail Only)" Enabled="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox runat="server" ID="chkIsOnlineRefundEnabled" Text="Online Refunds" Enabled="false" />
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <fieldset>
                                                    <legend>GETI / SecurePay / NMI Login</legend>
                                                    <table>
                                                        <tr>
                                                            <td class="lblRight" style="width: 150px">UserName:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="HostUserName" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight" style="width: 150px">Password:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="HostPassword" runat="server"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                                <asp:Panel ID="pnlNutra" runat="server">

                                                    <fieldset>
                                                        <legend>NMI Setup</legend>
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="lblRight" style="width: 150px">Vendor:
                                                                </td>
                                                                <td class="lblLeft">
                                                                    <asp:DropDownList ID="CRMID" runat="server" Width="150px"  OnSelectedIndexChanged="onNMIVendorChanged" AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="lblRight" style="width: 150px">Provider Name:
                                                                </td>
                                                                <td class="lblLeft">
                                                                    <asp:TextBox runat="server" ID="NMIProviderName" Width="150px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </fieldset>

                                                </asp:Panel>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:Panel ID="PanelNMI" runat="server" Height="" Width="">
                                            <table width="50%" border="1" class="left" style="float: left">
                                                <tr>
                                                    <td colspan="2" class="lblCenter">
                                                        <asp:Button ID="btnNMILockDownAPI" runat="server" Text="Board merchant onto NMI Lock-down" OnClick="btnNMILockDownAPI_Click" />
                                                        <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>--%>

                                                        <ig:WebDialogWindow ID="WebDialogWindow3" runat="server" Height="180px" Width="330px"
                                                            Modal="True" InitialLocation="Centered" UseBodyAsParent="True" WindowState="Hidden">
                                                            <ContentPane>
                                                                <Template>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding-left: 3px; padding-right: 3px;">
                                                                                <asp:Label runat="server" ID="IsUsernameExistError"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:Button runat="server" ID="btnUsernameExistError" Text="Ok" OnClick="btnUsernameExistError_Click" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </Template>
                                                            </ContentPane>
                                                        </ig:WebDialogWindow>
                                                        <%--</ContentTemplate>
                                                            <Triggers>

                                                                <asp:AsyncPostBackTrigger ControlID="btnNMILockDownAPI" EventName="Click" />

                                                            </Triggers>
                                                        </asp:UpdatePanel>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Plan ID : </td>
                                                    <td class="lblLeft">
                                                        <asp:Label ID="LockDownPlanID" runat="server" Text="380909"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Auto-Boarded at NMI :</td>
                                                    <td class="lblLeft">
                                                        <asp:CheckBox ID="chkNMILockDown" runat="server" Enabled="false"></asp:CheckBox></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Merchant Boarded Date at lockdown :</td>
                                                    <td class="lblLeft">
                                                        <asp:Label ID="LockdownDate" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="50%" border="1" class="right" style="float: right">
                                                <tr>
                                                    <td colspan="2" class="lblCenter">
                                                        <asp:Button ID="btnNMIAffiliateAPI" runat="server" Text="Board merchant onto NMI Paysafe Affiliate" OnClick="btnNMIAffiliateAPI_Click" />
                                                        <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="180px" Width="330px"
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
                                                                                <asp:Button runat="server" ID="Button1" Text="Ok"  Visible="false" />
                                                                                <asp:Button runat="server" ID="btnOk" Text="Continue" OnClick="btnOk_Click" />
                                                                                <asp:Button runat="server" ID="btnNo" Text="Cancel" OnClick="btnNo_Click" />

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
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Plan ID : </td>
                                                    <td class="lblLeft">
                                                        <asp:DropDownList ID="AffiliatePlanId" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Auto-Boarded at NMI :</td>
                                                    <td class="lblLeft">
                                                        <asp:CheckBox ID="chkNMIAffiliate" runat="server" Enabled="false"></asp:CheckBox></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Merchant Boarded Date at Affiliate :</td>
                                                    <td class="lblLeft">
                                                        <asp:Label ID="AffiliateDate" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <cc:wucCardCurrency ID="WucPlatformCardCurrency" runat="server" />
                                        <cb:wucCBMSPlus ID="wucCBMS" runat="server" Visible="false" />
                                    </asp:Panel>
                                </td>
                                <td valign="top">
                                    <asp:Panel ID="pnlMerchant" runat="server" Height="" Width="">
                                        <fieldset style="height: 58px;">
                                            <legend>Private Label</legend>
                                            <table>
                                                <tr>
                                                    <td class="lblLeft" style="width: 150px"><br />Company :
                                                    </td>
                                                    <td>
                                                        <br />
                                                        <asp:DropDownList ID="PrivateLabelUID" runat="server" Enabled="False"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <fieldset >
                                            <legend>Welcome Email Configuration</legend>
                                            <table>
                                                <tr>
                                                    <td class="lblLeft" style="width: 150px">Alternate Email :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AlternateEmail" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblLeft" style="width: 150px">Additional Recipients :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AdditionalRecipients" runat="server" Width="250px" TextMode="MultiLine" ></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>

                                        <fieldset>
                                            <legend>Other Options</legend>
                                            <uc2:wucServices ID="WucServices1" runat="server" />
                                            <asp:Label ID="lblVendorList" runat="server" Text="CB Vendor"></asp:Label>
                                            <asp:DropDownList ID="VendorList" runat="server" Width="150px"  AppendDataBoundItems="true" AutoPostBack="false">  <%--OnSelectedIndexChanged="onVendorListChanged"--%>
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </fieldset>
                                        <asp:Panel ID="pnlTSYS" runat="server">
                                            <fieldset>
                                                <legend>TSYS Parameters</legend>
                                                <table>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">MCC:
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="SicCode" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Merchant Number:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Tsys_MerchantNumber" runat="server" MaxLength="12"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Acquirer BIN:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Tsys_AcquirerBin" runat="server" MaxLength="6"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Store Number (PXP Only):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Tsys_StoreNumber" runat="server" MaxLength="4"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Terminal Number (PXP Only):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Tsys_TerminalNumber" runat="server" MaxLength="4"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Location Number:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Tsys_LocationNumber" runat="server" MaxLength="5"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">TID (PXP Only):
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Tsys_TID" runat="server" MaxLength="8"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Agent Bank:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Tsys_AgentBank" runat="server" MaxLength="6"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">Agent Chain:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="Tsys_AgentChain" runat="server" MaxLength="6"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </asp:Panel>
                                        <!--PXP-14480-->
                                        <fieldset>
                                            <legend>Consent Controls</legend>
                                            <asp:CheckBox ID="chkPCCSSwitch" runat="server" Text=" Paysafe Consumer Consent Service (PCCS)" Width="300px" onclick="cTrig()" />
                                        </fieldset>
                                        <!--DM-7160 --Ahmer Bashir -->   
                                        <asp:Panel ID="pnlPHQ" runat="server">
                                            <fieldset>
                                                <legend>PaymentXP to PayHQ</legend>
                                                <table>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">PayHQ Username:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPHQUserName" runat="server" MaxLength="20"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">PayHQ Password:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPHQPass" runat="server" TextMode="Password" AutoCompleteType="disabled" placeholder="Password" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight" style="width: 150px">PayHQ Device ID:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPHQDeviceID" runat="server" MaxLength="20"></asp:TextBox>
                                                        </td>
                                                    </tr>                                                    
                                                </table>
                                            </fieldset>
                                        </asp:Panel>
                                    </asp:Panel>
                                    <co:wucCompass ID="wucCompassParameters" runat="server" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    &nbsp;
                    
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">

        function Field2Str(fieldvalue) {
            if (fieldvalue == null)
                return '';
            else
                return fieldvalue;
        }

        function ShowCreateUser(oButton, oEvent) {
            oWebDialogWindow2 = $find('<% =WebDialogWindow2.ClientID %>'); oWebDialogWindow2.set_windowState($IG.DialogWindowState.Normal);
            oEvent.cancel = true;
        }

        function CloseCreateUser() {
            oWebDialogWindow2 = $find('<% =WebDialogWindow2.ClientID %>'); oWebDialogWindow2.set_windowState($IG.DialogWindowState.Hidden);
        }

    </script>

    <script type='text/javascript' src="../js/bootstrap.min.js"></script>
    <script type='text/javascript' src="../js/MPSEquipmentAutoComplete.js"></script>

    <script type="text/javascript">

        if ($("#ContentPlaceHolder1_WucEquipment_tbEquipmentSearch").attr("readonly") != "readonly") {

            MPSEquipmentAutoComplete.init("#ContentPlaceHolder1_WucEquipment_tbEquipmentSearch",
                "#ContentPlaceHolder1_WucEquipment_Type",
                "#ContentPlaceHolder1_WucEquipment_Maker",
                "#ContentPlaceHolder1_WucEquipment_Model",
                "#ContentPlaceHolder1_WucEquipment_ItemUID",
                "#ContentPlaceHolder1_WucEquipment_IsNewItem",
                "#ContentPlaceHolder1_WucEquipment_EMVCompliance",
                "#ContentPlaceHolder1_WucEquipment_EMVComplianceMerchant"
            );
        }
        //PXP-8407
        $(document).ready(function () {

            if ($("#ContentPlaceHolder1_WucEquipment_Type").val() == '') {
                $('#ContentPlaceHolder1_WucEquipment_ItemUID').attr('value', "");
            }
        });

    </script>
</asp:Content>
