<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true"
    Inherits="frmMerchantACH" Title="ACH/DD Profile" CodeBehind="frmMerchantACH.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<%@ Register Src="~/UserControls/wuConfirmDialog.ascx" TagName="wuConfirm" TagPrefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <script type="text/javascript" src="../js/autoNumeric.js"></script>
    <link href="../css/font-awesome-4.2.0/css/font-awesome.min.css" rel="stylesheet">
    <script type="text/javascript">

        $(document).ready(function () {

            $('#BankID').change(function () {
                if ($(this).val() == "13") {
                    $('.location').css('visibility', 'visible');
                }
                else {
                    $('.location').css('visibility', 'hidden');
                }
            });

            if ($('#BankID').val() == "13") {
                $('.location').css('visibility', 'visible');
            } else {
                $('.location').css('visibility', 'hidden');
            }
        });

        //PXP-9051 RThakur(bug 9503)
        function WebImageSave_Click(oButton, oEvent) {
            var oldStatus = $("#ContentPlaceHolder1_WucBusinessInfo1_ACHStatusUID");
            var newStatus = $("#<%=MerchantStatusUID.ClientID %>");
            var selectedNewStatus = $(newStatus).find('option:selected').val().toUpperCase();
            var isNewVerticalandMarketsValue = $("#<%=hdnIsNewVerticalandMarkets.ClientID %>").val();
            if ((isNewVerticalandMarketsValue == "false") && selectedNewStatus == "2FDDA5E4-E80A-4155-8CB2-D5200992FA81" && oldStatus.val().toUpperCase() != newStatus.val().toUpperCase()) {
                var confVerticals = confirm("<%=PaymentXP.BusinessObjects.Constant.NewVerticalandVerticalMarketsChecked%>");
                if (!confVerticals) {
                    oEvent.cancel = true;
                }
            }
        }

    </script>

    <!-- PXP-9348 RThakur start -->
     <script language="javascript" type="text/javascript">
         function WebImageSave_Click(oButton, oEvent) {

              
             var isNutraMerchant = $("#<%=hidIsNutraMerchant.ClientID %>");   //PXP-9348 RThakur
             var oldStatus = $("#<%=hidStatus.ClientID %>");
             var newStatus = $("#<%=MerchantStatusUID.ClientID %>");
             var hidCRMStatus = document.getElementById('<%= this.hiddenCrmStatus.ClientID %>').value;
             var isCRMAcceptTrans = document.getElementById('<%= this.hiddenAcceptTransaction.ClientID %>').value;
             if (oldStatus.val().toUpperCase() != newStatus.val().toUpperCase() && newStatus.val().toUpperCase() == "<%=PaymentXP.BusinessObjects.Constants.QUEUESTATUS_CU_RECEIVED.ToUpper()%>") {
                 if (isNutraMerchant.val().toUpperCase() == "TRUE") {
                     if (hidCRMStatus == "InActive") {
                         var conf = confirm("<%=PaymentXP.BusinessObjects.Constant.CrmTppInactive%>");
                         if (!conf) {
                             $("#<%=MerchantStatusUID.ClientID%>").val(oldStatus.val().toLowerCase());
                         }
                     }
                     if (hidCRMStatus.toUpperCase() == "NA" && isCRMAcceptTrans.toLowerCase() == "false") {
                         var conf = confirm("<%=PaymentXP.BusinessObjects.Constant.CrmTppNA%>");
                         if (!conf) {
                             $("#<%=MerchantStatusUID.ClientID%>").val(oldStatus.val().toLowerCase());
                         }
                     }
                 }
             }
         }
    </script>
    <!-- PXP-9348 RThakur end -->

    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner">
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
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
                        <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                        <asp:HiddenField runat="server" ID="hdnIsNewVerticalandMarkets" />
                        <div style="margin: 0px 0px 0px 8px">
                            <ig:WebTab ID="WebTab1" runat="server" Width="948px">
                                <Tabs>
                                    <ig:ContentTabItem runat="server" Text="Profile" Key="Profile">
                                        <Template>

                                            <asp:Panel runat="server" ID="pnlReqiredFields">
                                                <fieldset>
                                                    <legend>Required Fields</legend>
                                                    <table id="Table2" width="100%" cellspacing="2">
                                                        <tr>
                                                            <td class="lblRight">ZID
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="MerchantID" runat="server" Font-Bold="True"></asp:Label>
                                                                <asp:Label ID="lblMerchantUID" runat="server" Visible="False"></asp:Label>
                                                            </td>
                                                            <td class="lblRight">Active
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="Active" runat="server" TextAlign="Right"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">ACH/DD ID
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="AchID" runat="server" Font-Bold="True"></asp:Label>
                                                            </td>
                                                            <td class="lblRight">Batch File Upload
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="BatchUpload" runat="server" TextAlign="Right"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">DBA Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="AchCoName" runat="server" MaxLength="40" Width="200px"></asp:TextBox>
                                                            </td>
                                                            <td class="lblRight">Stop EFT?
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="StopEFTFrom" runat="server"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">ACH/DD Status
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="MerchantStatusUID" runat="server" Width="200px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="lblRight">ACH/DD Bank
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="BankID" runat="server" Width="200px" ClientIDMode="Static">
                                                                </asp:DropDownList>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">Descriptor
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="AchDescrp" runat="server" Width="200px" MaxLength="10" BorderColor="#adc3de"></asp:TextBox>
                                                            </td>
                                                            <td class="lblRight">Account Name
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="dfiAcctName" runat="server" Width="200px" MaxLength="25"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">Default SECC
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="Secc" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="Secc_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="lblRight">Deposit Trans Route
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="TransRoute" runat="server" Width="200px" MaxLength="9"></asp:TextBox>
                                                                <%--    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="TransRoute"
                                                                Display="None" ErrorMessage="Deposit Trans Routing Number is required."></asp:RequiredFieldValidator>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">Approved SECC
                                                            </td>
                                                            <td>
                                                                <asp:CheckBoxList ID="SeccList" runat="server" RepeatColumns="4">
                                                                </asp:CheckBoxList>
                                                            </td>
                                                            <td class="lblRight">Deposit Account #
                                                               
                                                            </td>
                                                            <td>
                                                                <% if (EditMode && HasBankAccountRole && PnlBankField.IsEnabled)
                                                                { %>
                                                                    <asp:TextBox ID="AccountNo" runat="server" Width="200px" MaxLength="17"></asp:TextBox>
                                                            <% }
                                                                else
                                                                { %>
                                                                    <asp:TextBox ID="AccountTmp" runat="server" Width="200px" MaxLength="17"></asp:TextBox>
                                                            <% } %>
                                                                
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">
                                                                <asp:Label ID="lblwebsite" runat="server" Text="Website" Visible="false"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="BusinessWebsiteACH" runat="server" MaxLength="100" Width="200px" Visible="false"></asp:TextBox>

                                                            </td>
                                                            <td class="lblRight">
                                                                <span>AccountType </span>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="AccountType" runat="server" Width="200px">
                                                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                                    <asp:ListItem Value="CK">Checking</asp:ListItem>
                                                                    <asp:ListItem Value="SA">Savings</asp:ListItem>
                                                                </asp:DropDownList>

                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trAchDiscrtn">
                                                            <td class="lblRight">ACH/DD Discretionary
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="AchDiscrtn" runat="server" Width="200px" MaxLength="20" BorderColor="#adc3de"></asp:TextBox>
                                                            </td>

                                                            <td class="lblRight" runat="server" id="tdTest">Test Flag
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="Test" runat="server" Width="200px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr runat="server" id="trMerchantTypeCode">
                                                            <td class="lblRight">Merchant Type
                                                            </td>

                                                            <td>
                                                                <asp:DropDownList ID="MerchantTypeCode" runat="server" Width="200px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="lblRight">NAICS:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="NAICS" runat="server" MaxLength="6" Width="200px"></asp:TextBox>
                                                                <%--<asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="NAICS"
                                                            ErrorMessage="Please enter a valid NAICS." MaximumValue="100000" MinimumValue="1"
                                                            Type="Integer" Display="None"></asp:RangeValidator>--%>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <%--      <td class="lblRight">
                                                            Confirm Trans Route
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TransRouteConfirm" runat="server" Width="200px" MaxLength="9"></asp:TextBox>
                                                        </td>
                                                        <td class="lblRight">
                                                            Confirm Account No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="AccountNoConfirm" runat="server" Width="200px" MaxLength="17"></asp:TextBox>
                                                            </mps:Field>
                                                        </td>
                                                    </tr>--%>
                                                            <tr runat="server" id="trNAICS">
                                                                <td class="lblRight">Return Email
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="Email" runat="server" MaxLength="200" Width="200px"></asp:TextBox>
                                                                </td>
                                                                <td class="lblRight">
                                                                    <asp:Label ID="lblLocationID" CssClass="location" runat="server" Text="Location ID"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="LocationID" CssClass="location" runat="server" Width="200px" MaxLength="11"></asp:TextBox>
                                                                </td>


                                                            </tr>
                                                    </table>
                                                </fieldset>
                                            </asp:Panel>
                                            <!-- PXP-9348 RThakur start -->
                                            <asp:HiddenField runat="server" ID="hidStatus" />
                                            <asp:HiddenField runat="server" ID="hidIsNutraMerchant" />
                                            <asp:HiddenField runat="server" ID="hiddenCrmStatus" />
                                            <asp:HiddenField runat="server" ID="hiddenAcceptTransaction" />
                                            <!-- PXP-9348 RThakur end -->

                                            <asp:Panel runat="server" ID="pnlFeeInformation">
                                                <fieldset>
                                                    <legend>Fee Information</legend>
                                                    <asp:Panel ID="pnlMeritus" runat="server">
                                                        <table id="Table3" cellspacing="2" width="100%">
                                                            <tr>
                                                                <td class="lblRight" width="17%">Reserve
                                                                </td>
                                                                <td width="16%">
                                                                    <ig:WebPercentEditor ID="Reservepct" runat="server" Width="152px" ToolTip="Range of values: from 0 to 100"
                                                                        MinValue="0" MaxValue="100" SpinWrap="true" DataMode="Decimal" MinDecimalPlaces="0">
                                                                    </ig:WebPercentEditor>
                                                                </td>
                                                                <td class="lblRight" width="17%">Reserve Period
                                                                </td>
                                                                <td width="16%">
                                                                    <ig:WebNumericEditor ID="ReservePeriod" runat="server" Width="152px" ValueText="0"
                                                                        DataMode="Int" MaxValue="1000" MinValue="0" ToolTip="Range of values: from 0 to 1000">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                                <td class="lblRight" width="18%">Hold Period
                                                                </td>
                                                                <td width="16%">
                                                                    <ig:WebNumericEditor ID="HoldPeriod" runat="server" Width="152px" ValueText="0" DataMode="Int"
                                                                        MaxValue="1000" MinValue="0" ToolTip="Range of values: from 0 to 1000">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="lblRight">Trans Fee
                                                                </td>
                                                                <td>
                                                                    <ig:WebNumericEditor ID="ItemFee" runat="server" Width="152px" ValueText="0" MaxValue="10000"
                                                                        MinValue="0" ToolTip="Range of values: from 0 to 10000" Font-Size="10pt">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                                <td class="lblRight">High Ticket
                                                                </td>
                                                                <td>
                                                                    <ig:WebNumericEditor ID="HighTicket" runat="server" Width="152px" ValueText="0"
                                                                        MaxValue="10000000" MinValue="0" ToolTip="Range of values: from 0 to 10000000"
                                                                        Font-Size="10pt">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                                <td class="lblRight">File Load Fee
                                                                </td>
                                                                <td>
                                                                    <ig:WebNumericEditor ID="FileLoadFee" runat="server" Width="152px" ValueText="0"
                                                                        MaxValue="100000" MinValue="0" ToolTip="Range of values: from 0 to 100000"
                                                                        Font-Size="10pt">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="lblRight">Monthly Min
                                                                </td>
                                                                <td>
                                                                    <ig:WebNumericEditor ID="MonthlyMin" runat="server" Width="152px" ValueText="0"
                                                                        MaxValue="10000000" MinValue="0" ToolTip="Range of values: from 0 to 10000000"
                                                                        Font-Size="10pt">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                                <td class="lblRight">Over Draft Fee
                                                                </td>
                                                                <td>
                                                                    <ig:WebNumericEditor ID="OverDraftFee" runat="server" Width="152px" ValueText="0"
                                                                        MaxValue="10000" MinValue="0" ToolTip="Range of values: from 0 to 10000"
                                                                        Font-Size="10pt">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                                <td class="lblRight">Return Fee
                                                                </td>
                                                                <td>
                                                                    <ig:WebNumericEditor ID="ReturnFee" runat="server" Width="152px" ValueText="0" MaxValue="10000"
                                                                        MinValue="0" ToolTip="Range of values: from 0 to 10000" Font-Size="10pt">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="lblRight">Discount Fee
                                                                </td>
                                                                <td>
                                                                    <ig:WebPercentEditor MaxValue="100" ID="ProcessFee" runat="server" ValueText="0"
                                                                        Width="152px">
                                                                    </ig:WebPercentEditor>
                                                                </td>
                                                                <td class="lblRight">Statement Fee
                                                                </td>
                                                                <td>
                                                                    <ig:WebNumericEditor ID="StatementFee" runat="server" Width="152px" ValueText="0"
                                                                        MaxValue="10000" MinValue="0" ToolTip="Range of values: from 0 to 10000"
                                                                        Font-Size="10pt">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                                <td class="lblRight">Inquiry Fee
                                                                </td>
                                                                <td>
                                                                    <ig:WebNumericEditor ID="InquiryFee" runat="server" Width="152px" ValueText="0"
                                                                        MaxValue="10000" MinValue="0" ToolTip="Range of values: from 0 to 10000"
                                                                        Font-Size="10pt">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="lblRight">Discount Fee $
                                                                </td>
                                                                <td>
                                                                    <ig:WebNumericEditor ID="ProcessFeeDollars" runat="server" ValueText="0" MaxValue="10000" ToolTip="Range of values: from 0 to 10000"
                                                                        Width="152px">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                                <td class="lblRight">Monthly Volume
                                                                </td>
                                                                <td>
                                                                    <ig:WebNumericEditor ID="MonthlyVolume" runat="server" Width="152px" ValueText="0"
                                                                        MaxValue="999999999" MinValue="0" ToolTip="Range of values: from 0 to 999999999"
                                                                        Font-Size="10pt">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                                <td class="lblRight">Average Ticket
                                                                </td>
                                                                <td>
                                                                    <ig:WebNumericEditor ID="AverageTicket" runat="server" Width="152px" ValueText="0"
                                                                        MaxValue="10000000" MinValue="0" ToolTip="Range of values: from 0 to 10000000"
                                                                        Font-Size="10pt">
                                                                    </ig:WebNumericEditor>
                                                                </td>

                                                            </tr>
                                                            
                                                        </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlWoodforestIrvine" runat="server">
                                                            <table id="Table4" cellspacing="2" width="100%">

                                                                <tr >
                                                                <td class="lblRight" width="17%">Estimated Monthly Transactions
                                                                </td>
                                                                
                                                                  
                                                                <td width="16%">
                                                                    <ig:WebNumericEditor ID="EstimatedMonthlyVolume" runat="server" ValueText="0" MinValue="0" MaxValue="9999999999" ToolTip="Range of values: from 0 to 9999999999"
                                                                        Width="152px" DataMode="Long" MaxLength="10">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                                    <td class="lblRight" width="17%">
                                                                        Estimated Monthly Returns
                                                                    </td>
                                                                    <td width="16%">
                                                                         <ig:WebNumericEditor ID="EstimatedMonthlyReturns" runat="server" Width="152px" ValueText="0"
                                                                        MaxValue="9999999999" MinValue="0" ToolTip="Range of values: from 0 to 9999999999"
                                                                        Font-Size="10pt" DataMode="Long" MaxLength="10">
                                                                    </ig:WebNumericEditor>
                                                                    </td>
                                                                    <td class="lblRight" width="18%">Estimated Monthly Credits</td>
                                                                    <td width="16%">
                                                                        <ig:WebNumericEditor ID="EstimatedMonthlyCredits" runat="server" Width="152px" ValueText="0"
                                                                        MaxValue="9999999999" MinValue="0" ToolTip="Range of values: from 0 to 9999999999"
                                                                        Font-Size="10pt" DataMode="Long" MaxLength="10">
                                                                    </ig:WebNumericEditor>
                                                                    </td>

                                                                      </tr>
                                                                <tr >
                                                                    <td colspan="6"></td>
                                                                </tr>
                                                                 <tr >
                                                                    <td colspan="6"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" class="lblCenter" width="33%">
                                                                        <b>ACH Transactions Methods</b>
                                                                    </td>
                                                                    <td colspan="2" class="lblCenter" width="33%">
                                                                        <b>Products sold to</b>
                                                                    </td>
                                                                    <td width="34%" colspan="2"  ></td>
                                                                </tr> 
                                                                 <tr> 
                                                                     <td class="lblRight" width="17%">
                                                                         Written Contract 
                                                                     </td>
                                                                     <td width="16%">
                                                                          <ig:WebPercentEditor MaxValue="100"  MinValue="0" ID="TinfoWrittenContractPercent" runat="server"
                                                                                    ValueText="0.00" Width="152px" MinDecimalPlaces="2">
                                                                                    <ClientEvents TextChanged="CalTotalACHTransCompleted_TextChanged" />
                                                                                </ig:WebPercentEditor>
                                                                     </td>
                                                                    <td class="lblRight" width="16%"> 
                                                                         Consumer
                                                                    </td>
                                                                     <td width="16%">
                                                                          <ig:WebPercentEditor MaxValue="100" ID="TinfoConsumerPercent"
                                                                                    MinDecimalPlaces="2" MinValue="0" runat="server" ValueText="0.00" Width="152px">
                                                                                    <ClientEvents TextChanged="CalTotalACHProductSold_TextChanged" />
                                                                                </ig:WebPercentEditor>
                                                                     </td>
                                                                     <td colspan="2" width="34%">
                                                                </td>
                                                               
                                                                </tr>
                                                                 <tr>                                                               
                                                                <td class="lblRight" width="17%">
                                                                   Internet Initiated
                                                                </td>
                                                                     <td width="16%">
                                                                          <ig:WebPercentEditor MaxValue="100" ID="TinfoInternetInitiatedPercent" runat="server" 
                                                                                    ValueText="0.00" Width="152px" MinDecimalPlaces="2" MinValue="0" AutoPostBackFlags-ValueChanged="On" OnValueChanged="TinfoInternetInitiatedPercent_ValueChanged">
                                                                                    <ClientEvents TextChanged="CalTotalACHTransCompleted_TextChanged"  />
                                                                                </ig:WebPercentEditor>
                                                                        
                                                                     </td>
                                                                    <td class="lblRight" width="17%">
                                                                        Business
                                                                    </td>
                                                                     <td width="16%">
                                                                         <ig:WebPercentEditor MaxValue="100"  MinValue="0" ID="TinfoBusinessPercent" runat="server"
                                                                                    MinDecimalPlaces="2" ValueText="0.00" Width="152px">
                                                                                    <ClientEvents TextChanged="CalTotalACHProductSold_TextChanged" />
                                                                                </ig:WebPercentEditor>
                                                                     </td>
                                                                     <td colspan="2">
                                                                         
                                                                </td>
                                                               
                                                            </tr>
                                                                <tr runat="server" id="trURLInternetPage" visible="false" >
                                                                <td class="lblRight">     
                                                                           Payment page url
                                                                </td>
                                                                    <td>
                                                                         <ig:WebTextEditor ID="TinfoPaymentPageURL" MaxLength="100" runat="server"  Width="152px">                                                                                                                                                                     
                                                                                </ig:WebTextEditor> 

                                                                      </td>
                                                                    <td colspan="4">

                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="lblRight" >
                                                                        Telephone Initiated</td>
                                                                    <td>
                                                                          <ig:WebPercentEditor MaxValue="100" MinValue="0" ID="TinfoTelephoneInitiatedPercent" runat="server"
                                                                                    MinDecimalPlaces="2" ValueText="0.00"  Width="152px">
                                                                                    <ClientEvents TextChanged="CalTotalACHTransCompleted_TextChanged" />
                                                                                </ig:WebPercentEditor>                                                                 
                                                                    </td> 
                                                                <td class="lblRight">
                                                                     <b>Total</b>
                                                                </td>
                                                                     <td >
                                                                         <ig:WebPercentEditor ID="txtTotalACHProductSold" MinValue="0" runat="server" Enabled="False" MinDecimalPlaces="2"
                                                                                    ValueText="0.00" Width="152px">
                                                                                </ig:WebPercentEditor>
                                                                </td>
                                                                <td colspan="2">
                                                                    
                                                                </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="lblRight"><b>Total</b></td>
                                                                     <td > <ig:WebPercentEditor ID="txtTotalACHTransCompleted" MinValue="0" runat="server" Enabled="False" ValueText="0.00"
                                                                                    MinDecimalPlaces="2" Width="152px">
                                                                                </ig:WebPercentEditor>
                                                                </td>
                                                                <td colspan="4">
                                                                    
                                                                </td>
                                                                     
                                                                
                                                                </tr>
                                                                
                                                            </table>                                                       
                                                            </asp:Panel>
                                                    
                                                    <asp:Panel ID="pnlOptimal" runat="server">
                                                        <table width="100%">

                                                            <tr>
                                                                <td valign="top">
                                                                    <table id="Table1" cellspacing="2" width="100%">
                                                                        <tr>
                                                                            <td colspan="2"><b>DISCOUNT FEES</b></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Completed Presentment
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebPercentEditor ID="DiscountFeeCompletedPresentment" runat="server" Width="152px" ToolTip="Range of values: from 0 to 100"
                                                                                    MinValue="0" MaxValue="100" SpinWrap="true" DataMode="Decimal" MinDecimalPlaces="0">
                                                                                </ig:WebPercentEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Completed Credit
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebPercentEditor ID="DiscountFeeCompletedCredit" runat="server" Width="152px" ToolTip="Range of values: from 0 to 100"
                                                                                    MinValue="0" MaxValue="100" SpinWrap="true" DataMode="Decimal" MinDecimalPlaces="0">
                                                                                </ig:WebPercentEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Declined Presentment
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebPercentEditor ID="DiscountFeeDeclinedPresentment" runat="server" Width="152px" ToolTip="Range of values: from 0 to 100"
                                                                                    MinValue="0" MaxValue="100" SpinWrap="true" DataMode="Decimal" MinDecimalPlaces="0">
                                                                                </ig:WebPercentEditor>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td class="lblRight">Rolling Reserve
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebPercentEditor ID="ReseveCompletedPresentment" runat="server" Width="152px" ToolTip="Range of values: from 0 to 100"
                                                                                    MinValue="0" MaxValue="100" SpinWrap="true" DataMode="Decimal" MinDecimalPlaces="0">
                                                                                </ig:WebPercentEditor>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan="2"><b>DISPUTE MANAGEMENT FEES</b></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Returned Presentment
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="ReturnedPresentment" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Returned Credit
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="ReturnedCredit" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Returned Mandate
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="ReturnedMandate" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td class="lblRight">Chargeback
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="Chargeback" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>


                                                                        <tr>
                                                                            <td colspan="2"><b>FEE INFORMATION</b></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Monthly Volume
                                                                            </td>
                                                                            <td>


                                                                                <ig:WebNumericEditor ID="optMonthlyVolume" runat="server" Width="152px" ValueText="0"
                                                                                    MaxValue="10000000" MinValue="0" ToolTip="Range of values: from 0 to 10000000"
                                                                                    Font-Size="10pt">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td class="lblRight">Average Ticket
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="optAverageTicket" runat="server" Width="152px" ValueText="0"
                                                                                    MaxValue="10000000" MinValue="0" ToolTip="Range of values: from 0 to 10000000"
                                                                                    Font-Size="10pt">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">High Ticket
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="optItemLimitAmount" runat="server" Width="152px" ValueText="0"
                                                                                    MaxValue="10000000" MinValue="0" ToolTip="Range of values: from 0 to 10000000"
                                                                                    Font-Size="10pt">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>



                                                                    </table>
                                                                </td>
                                                                <td>
                                                                    <table id="Table6" cellspacing="2" width="100%">
                                                                        <tr>
                                                                            <td colspan="2"><b>TRANSACTION FEES</b></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Completed Verify
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="CompletedVerify" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Completed Presentment
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="CompletedPresentment" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Completed Credit
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="CompletedCredit" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td class="lblRight">Completed Mandate
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="CompletedMandate" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td class="lblRight">Declined Verify
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="DeclinedVerify" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Declined Presentment
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="DeclinedPresentment" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Declined Credit
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="DeclinedCredit" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Declined Mandate
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="DeclinedMandate" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Completed Representment
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="CompletedRepresentment" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <td class="lblRight">Bacs File Submission Fee
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="BacsFileSubmissionFee" runat="server" Width="152px" ToolTip="Range of values: from 0 to 10000"
                                                                                    MinValue="0" MaxValue="10000" SpinWrap="true" DataMode="Decimal" ValueText="0">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2"><b>RESERVE FEE</b></td>

                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Reserve</td>
                                                                            <td>
                                                                                <ig:WebPercentEditor ID="optReservepct" runat="server" Width="152px" ToolTip="Range of values: from 0 to 100"
                                                                                    MinValue="0" MaxValue="100" SpinWrap="true" DataMode="Decimal" MinDecimalPlaces="0">
                                                                                </ig:WebPercentEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Reserve Period</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="optReservePeriod" runat="server" Width="152px" ValueText="0"
                                                                                    DataMode="Int" MaxValue="1000" MinValue="0" ToolTip="Range of values: from 0 to 1000">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="lblRight">Hold Period</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="optHoldPeriod" runat="server" Width="152px" ValueText="0" DataMode="Int"
                                                                                    MaxValue="1000" MinValue="0" ToolTip="Range of values: from 0 to 1000">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>

                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </fieldset>
                                            </asp:Panel>

                                            <asp:Panel runat="server" ID="pnlSalesForce">

                                                <fieldset>
                                                    <legend>Salesforce</legend>
                                                    <table>
                                                        <tr>
                                                            <td width="121px" style="text-align: right">SalesforceID: </td>
                                                            <td>S-<asp:TextBox ID="SalesForceID" ReadOnly="true" runat="server" Width="60px" Style="text-align: right" MaxLength="9"></asp:TextBox>
                                                            </td>

                                                        </tr>

                                                    </table>
                                                </fieldset>



                                            </asp:Panel>

                                        </Template>
                                    </ig:ContentTabItem>
                                    <ig:ContentTabItem runat="server" Text="Admin" Hidden="true" Key="Admin">
                                        <Template>
                                            <fieldset>
                                                <legend>Additional Information</legend>
                                                <table id="Table5" cellspacing="2" width="100%">
                                                    <tr>
                                                        <td class="lblRight">Withdraw Tran Route
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="WithdrawTR" runat="server" Width="150px" MaxLength="9"></asp:TextBox>
                                                        </td>
                                                        <td align="right">Create Response File?
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="ProduceRspFile" runat="server" Width="150px"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Withdraw Account No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="WithdrawAcctNo" runat="server" Width="150px" MaxLength="17"></asp:TextBox>
                                                        </td>
                                                        <td align="right">Create Return File?
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="ProduceRtnFile" runat="server"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Apply Fee To ACH/DD ID
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="WithdrawFees_AchID" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td align="right"></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Pull Funds From ACH/DD ID
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="WithdrawFunds_AchID" runat="server" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td align="right"></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Group Merchant
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="GroupID" runat="server" Width="154px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">Allow Blind Credits?
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="AllowBlindCredits" runat="server"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <%-- <div runat="server"id="divRouting" style="display:none;">--%>
                                                        <td class="lblRight">DSC Fee Trans Route
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="DSCFeeTransRoute" runat="server" MaxLength="9" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <%--</div>--%>
                                                        <td align="right">Create Hold?
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="CreateHold" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">DSC Fee Account No
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="DSCFeeAccountNo" runat="server" MaxLength="17" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td align="right">Override SECC?
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="OverrideSECC" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight"></td>
                                                        <td></td>
                                                        <td align="right">Perform Duplicate Trans Check?
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="CheckDuplicateTrans" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight"></td>
                                                        <td></td>
                                                        <td align="right">
                                                            <asp:Label ID="lblSuppressScrubReturnFee" runat="server" Text="Suppress Scrub Return Fee?"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="SuppressScrubReturnFee" runat="server" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight"></td>
                                                        <td></td>
                                                        <td align="right">Monthly Billing?
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="MonthlyBase" runat="server"></asp:CheckBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset></Template>
                                    </ig:ContentTabItem>
                                </Tabs>
                            </ig:WebTab>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
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
    </div>
<script language="javascript" type="text/javascript">
    function CalTotalACHTransCompleted_TextChanged(oEdit, newText, oEvent) {

        var edit1 = document.getElementById('<% =TinfoWrittenContractPercent.ClientID %>');           
        var edit2 = document.getElementById('<% =TinfoInternetInitiatedPercent.ClientID %>');
        var edit3 = document.getElementById('<% =TinfoTelephoneInitiatedPercent.ClientID %>');
        var edit4 = document.getElementById('<% =txtTotalACHTransCompleted.ClientID %>');
      
           var TinfoWrittenContractPercent;
           var TinfoInternetInitiatedPercent;
           var TinfoMailOrderPercent;
           var TinfoTelephoneInitiatedPercent;          
           if (edit1.value == null || edit1.value<0)
               TinfoWrittenContractPercent = 0.00;
           else
               TinfoWrittenContractPercent =parseFloat(edit1.value);

           if (edit2.value == null || edit2.value < 0)
               TinfoInternetInitiatedPercent = 0.00;
           else
               TinfoInternetInitiatedPercent = parseFloat(edit2.value);

           if (edit3.value == null || edit3.value < 0)
               TinfoTelephoneInitiatedPercent = 0.00;
           else
               TinfoTelephoneInitiatedPercent = parseFloat(edit3.value);

           edit4.value = parseFloat(TinfoWrittenContractPercent + TinfoInternetInitiatedPercent + TinfoTelephoneInitiatedPercent).toFixed(2);
         
       }

    function CalTotalACHProductSold_TextChanged(oEdit, newText, oEvent) {
           var edit1 = document.getElementById('<% =TinfoConsumerPercent.ClientID %>');
           var edit2 = document.getElementById('<%=TinfoBusinessPercent.ClientID %>');
           var edit3 = document.getElementById('<%=txtTotalACHProductSold.ClientID %>');

          var TinfoConsumerPercent;
          var TinfoBusinessPercent;

          if (edit1.value == null || edit1.value<0)
              TinfoConsumerPercent = 0.00;
          else
              TinfoConsumerPercent = parseFloat(edit1.value);

          if (edit2.value == null ||edit2.value < 0)
              TinfoBusinessPercent = 0.00;
          else
              TinfoBusinessPercent = parseFloat(edit2.value);


          edit3.value=parseFloat(TinfoConsumerPercent + TinfoBusinessPercent).toFixed(2);
      }
</script>


</asp:Content>
