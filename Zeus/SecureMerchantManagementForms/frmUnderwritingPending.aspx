<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true"
    Inherits="frmUnderwritingPending" Title="Pendings" CodeBehind="frmUnderwritingPending.aspx.cs" %>
<%@ Register Src="~/UserControls/wucConditions.ascx" TagName="wucConditions" TagPrefix="uc5" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../js/encoder.js" type="text/javascript"></script>
    <!-- Added by Chandra for PXP-7898 -->
     <script language="javascript" type="text/javascript">
         function WebImageSave_Click(oButton, oEvent) {

             var isNutra = $("#<%=IsNutra.ClientID %>");
             var isNutraMerchant = $("#<%=hidIsNutraMerchant.ClientID %>");   //PXP-9348 RThakur
             var oldStatus = $("#<%=hidStatus.ClientID %>");
             var newStatus = $("#<%=StatusUID.ClientID %>");

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
             var isNewVertical = $("#<%=IsNewVerticalNew.ClientID %>").val();
             var isIsNewVertical = false;
             if (isNewVertical == "True") {
                 isIsNewVertical = true;
             }
             if ((isNewVerticalandMarketsValue == "false") && isIsNewVertical && selectedNewStatus == "2FDDA5E4-E80A-4155-8CB2-D5200992FA81" && oldStatus.val().toUpperCase() != newStatus.val().toUpperCase()) {
                var confVerticals = confirm("New Vertical checked but no Vertical Market selected for merchant");
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
        <asp:Panel runat="server" ID="pnlBanner">
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <table width="100%">
            <tr>
                <td>
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
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
                        <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                        <asp:HiddenField runat="server" ID="hidStatus" />
                        <asp:HiddenField runat="server" ID="IsNutra" />
                        <asp:HiddenField runat="server" ID="IsNewVerticalandMarkets" />
                        <asp:HiddenField runat="server" ID="IsNewVerticalNew" />
                        <asp:HiddenField runat="server" ID="hidIsNutraMerchant" />
                        <asp:HiddenField runat="server" ID="hiddenCrmStatus" />
                        <asp:HiddenField runat="server" ID="hiddenAcceptTransaction" />

                        <asp:Panel ID="pnlStatus" runat="server">
                            <br />
                            <div class="title">
                                &nbsp;&nbsp;Status
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <table cellspacing="2">
                                    <tr>
                                        <asp:Panel ID="CCStatus" runat="server">
                                            <td class="lblRight">Application Status
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="StatusUID" runat="server">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="StatusUID"
                                                    ValidationGroup="UWValid" Display="dynamic" Text="*" ErrorMessage="Please select a Status."
                                                    Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                                            </td>
                                        </asp:Panel>
                                        <asp:Panel ID="ACHStatus" runat="server" Visible="false">
                                            <td class="lblRight">ACH Status
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ACHStatusUID" runat="server">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="ACHStatusUID"
                                                    ValidationGroup="UWValid" Display="dynamic" Text="*" ErrorMessage="Please select a Status."
                                                    Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                                            </td>
                                        </asp:Panel>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <uc5:wucConditions runat="server" ID="Conditions" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
