<%@ Page Language="C#" MasterPageFile="~/MasterPageAgent.master" AutoEventWireup="true"
    Inherits="frmAgent" Title="Agent Profile" CodeBehind="frmAgent.aspx.cs" %>

<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/wucCreateUser.ascx" TagName="wucCreateUser" TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucContact.ascx" TagName="wucContact" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .agentcontacts .left
        {
            width: 100px;
        }
    </style>
    <script id="igClientScript1" type="text/javascript">
<!--

    function uwtMain_Initialize(sender, eventArgs) {
        ///<summary>
        ///
        ///</summary>
        ///<param name="sender" type="Infragistics.Web.UI.ControlMain"></param>
        ///<param name="eventArgs" type="Infragistics.Web.UI.EventArgs"></param>


        //    var tree = $find('<%= uwtMain.ClientID %>');
            //    if (tree.get_selectedNodes()[0] != null)
            //        tree.get_selectedNodes()[0].get_element().scrollIntoView();

        }// -->

        function isDMAllocation_onclick() {
            var a = document.getElementById("<%=IsDMAllocation.ClientID%>");
            var i = document.getElementById("<%=IsInternalAllocation.ClientID%>");
            if (i.checked) {
                a.checked = true;
                return;
            }
        }

        function isInternalAllocation_onclick(evt) {
            var a = document.getElementById("<%=IsDMAllocation.ClientID%>");
            var i = document.getElementById("<%=IsInternalAllocation.ClientID%>");
            if (!a.checked) {
                i.checked = false;
                return;
            }
        }

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
    <script type="text/javascript" language="javascript">
        function RequestedByChange() {
            var e = document.getElementById("<%=RequestedBy.ClientID%>");
            var txt = $('#<%=OtherUser.ClientID %>');
            var selVal = e.options[e.selectedIndex].value;
            if (selVal == 0)
                txt.show();
            else
                txt.hide();
        }
    </script>

    <div id="contentpage1">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 325px; vertical-align: top;">
                    <fieldset>
                        <legend>Agents</legend>
                        <ig:WebDataTree ID="uwtMain" runat="server" Height="600px" Width="100%" SelectionType="Single"
                            OnNodePopulate="uwtMain_NodePopulate" OnNodeClick="uwtMain_NodeClick">
                            <AutoPostBackFlags NodeClick="On" />
                            <ClientEvents Initialize="uwtMain_Initialize" />
                        </ig:WebDataTree>
                    </fieldset>
                </td>
                <td style="vertical-align: top; width: 700px">
                    <asp:Panel runat="server" ID="pnlID">
                        <uc1:wucMessage ID="WucMessage1" runat="server" />
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
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
                                                    <igtxt:WebImageButton ID="btnLogin" runat="server" Text="Agent Login" AccessKey="L"
                                                        CausesValidation="False" OnClick="btnLogin_Click">
                                                        <Appearance>
                                                            <Image Url="../Images/document_view.png" />
                                                        </Appearance>
                                                    </igtxt:WebImageButton>
                                                    <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="320px" Width="500px"
                                                        Modal="True" InitialLocation="Centered" WindowState="Hidden">
                                                        <ContentPane>
                                                            <Template>
                                                                <uc1:wucCreateUser ID="WucCreateUser1" runat="server" />
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
                        <asp:Panel ID="pnlDetail" runat="server" Height="" Width="" HorizontalAlign="left">
                            <fieldset>
                                <legend>Agent Memo</legend>
                                <asp:TextBox ID="AgentMemo" runat="server" Height="50px" MaxLength="255" TextMode="MultiLine"
                                    Width="96%"></asp:TextBox>
                            </fieldset>

                            <fieldset style="vertical-align: top;">
                                <legend>General Information</legend>
                                <table cellspacing="2" width="100%">
                                    <tr>
                                        <td class="lblRight">Gateway:</td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="Brand" Enabled="false" Width="180px" TabIndex="5" onchange="Showgateway(this)">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">3rd Party</asp:ListItem>
                                                <asp:ListItem Value="2">Netbanx</asp:ListItem>
                                            </asp:DropDownList></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Agent ID:
                                        </td>
                                        <td>
                                            <asp:Label ID="AgentID" runat="server" Font-Bold="True" Width="75px"></asp:Label>
                                        </td>
                                        <td class="lblRight">Parent Agent:
                                        </td>
                                        <td>
                                            <asp:Label ID="ParentAgentFullName" runat="server" Font-Bold="True" Width="180px"></asp:Label>
                                        </td>
                                    </tr>
            </tr>

            <tr>
                <td class="lblRight" id="tdAgentEdgeLabel" runat="server" visible="false">Edge Id:</td>
                <td align="left" id="tdAgentEdgeText" runat="server" visible="false">
                    <asp:TextBox ID="AgentEdgeId" runat="server" MaxLength="25"/>
                </td>
                <td class="lblRight"> Schedule A Type :
                </td>
                <td>
                    <asp:DropDownList ID="ddlScheduleATypes" runat="server" Width="185px">
                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="lblRight" colspan="2">
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="AgentTypeUID"
                        Display="None" ErrorMessage="Type is required." Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                </td>
                <td class="lblRight">
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="StatusUID"
                        Display="None" ErrorMessage="Agent status is required." Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                </td>
                <td class="lblRight">
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="ddlScheduleATypes" Enabled="false"
                        Display="None" ErrorMessage="Schedule A Type is required." Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td class="lblRight">FMA ID:</td>
                <td>
                    <asp:TextBox ID="AgentFMAID" runat="server" MaxLength="15" Width="180px" onKeyPress="CheckNumeric()"></asp:TextBox>
                </td>
                <td class="lblRight">Office Location:
                </td>
                <td>
                    <asp:DropDownList ID="OfficeID" runat="server" Width="185px">
                    </asp:DropDownList>
                    <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="OfficeID" Display="None" ErrorMessage="Please select a Office." Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td class="lblRight">OLE:
                </td>
                <td colspan="3">
                    <asp:DropDownList runat="server" Width="185px" ID="LegalEntityID">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="lblRight">Team:</td>
                <td align="left">
                    <asp:Label ID="TeamName" runat="server" Width="180px" Font-Bold="True"></asp:Label>
                </td>
                <td class="lblRight">Channel:</td>
                <td align="left">
                    <asp:DropDownList runat="server" ID="AgentGroupID" Width="185px"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="lblRight">Type As:
                </td>
                <td>
                    <asp:DropDownList ID="AgentTypeUID" runat="server" Width="185px">
                    </asp:DropDownList>
                </td>
                <td class="lblRight">Agent Status:
                </td>
                <td>
                    <asp:DropDownList ID="StatusUID" runat="server" DataTextField="name" DataValueField="status_id"
                        Width="185px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="lblRight" colspan="4">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                        ErrorMessage="DBA is required." ControlToValidate="AgentDBA"></asp:RequiredFieldValidator>
                </td>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td class="lblRight">DBA Name:
                </td>
                <td>
                    <asp:TextBox ID="AgentDBA" runat="server" Width="180px"></asp:TextBox>
                </td>
                <td class="lblRight">Agent LE:
                </td>
                <td>
                    <asp:TextBox ID="LegalName" runat="server" Width="180px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="lblRight">First Name:
                </td>
                <td>
                    <asp:TextBox ID="FirstName" runat="server" Width="180px"></asp:TextBox>
                </td>
                <td class="lblRight">Last Name:
                </td>
                <td>
                    <asp:TextBox ID="LastName" runat="server" Width="180px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="lblRight">Email:
                </td>
                <td>
                    <asp:TextBox ID="Email" runat="server" Width="180px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="Email"
                        Display="Dynamic" Text="*" ErrorMessage="Please enter a valid email address"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="None"
                        ErrorMessage="Email is required." ControlToValidate="Email"></asp:RequiredFieldValidator>
                </td>
                <td class="lblRight">Password:
                </td>
                <td>
                    <asp:TextBox ID="Password" runat="server" Width="180px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="Password"
                        Display="none" ErrorMessage="Password must be 8 characters long, contains letters and numbers, and at least 1 uppercase letter."
                        ValidationExpression="^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="lblRight">Tax Reg #:
                </td>
                <td>
                    <asp:TextBox ID="TaxID" runat="server" Width="180px" MaxLength="10"></asp:TextBox>
                </td>
                <td class="lblRight">SSN:
                </td>
                <td>
                    <ig:WebMaskEditor ID="SSNNumber" runat="server" InputMask="###-##-####" Width="180px">
                    </ig:WebMaskEditor>
                </td>
            </tr>
            <tr>
                <td class="lblRight">SS Rep:
                </td>
                <td>
                    <asp:DropDownList ID="PrimaryContactUID" runat="server" Width="185px">
                    </asp:DropDownList>
                </td>
                <td class="lblRight">Cell:
                </td>
                <td>
                    <ig:WebMaskEditor ID="CellPhone" runat="server" InputMask="##############################" PromptChar=' ' Width="180px" ShowMaskOnFocus="False">
                    </ig:WebMaskEditor>
                </td>
            </tr>
            <!-- by Asheesh-->

            <tr>
                <td class="lblRight">Channel Sales Manager:
                </td>
                <td>
                    <asp:DropDownList ID="TerritoryManager" runat="server" Width="185px">
                    </asp:DropDownList>
                </td>
                <td class="lblRight">Fax:
                </td>
                <td>
                    <asp:TextBox ID="Fax" runat="server" Width="180px"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td class="lblRight">Contact Phone #:
                </td>
                <td>
                    <asp:TextBox ID="Phone" runat="server" Width="180px"></asp:TextBox>
                </td>
                <td class="lblRight">
                </td>
                <td>
                    <asp:CheckBox ID="EnableOnlineApp" runat="server" Text="Online App" />
                </td>
            </tr>
            <tr>
                <td class="lblRight">Business Type:
                </td>
                <td>
                    <asp:DropDownList ID="BusinessStructureUID" runat="server" Width="185px">
                    </asp:DropDownList>
                </td>
                <td class="lblRight"></td>
                <td align="left">
                    <asp:CheckBox ID="EnableOnlineAppReview" runat="server" Text="Review App XP" />
                </td>
            </tr>
            <tr>
                <td class="lblRight">Requested By:
                </td>
                <td class="left">
                    <asp:DropDownList ID="RequestedBy" runat="server" Width="185px" onchange="RequestedByChange()">
                    </asp:DropDownList>
                </td>
                <td>&nbsp;</td>
                <td align="left">
                    <asp:CheckBox ID="EnableFixedCharges" runat="server" Text=" Unlock Fixed Fees" />
                </td>
            </tr>
            <tr>
                <td class="lblRight">Association Number:
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="None"
                 ErrorMessage="Association Number is required." ControlToValidate="AssociationNumber"></asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:TextBox ID="AssociationNumber" runat="server" Width="180px" MaxLength="6" Text="100001"></asp:TextBox>
                    <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "AssociationNumber" ID="RegularExpressionValidatorAssociationNumber" ValidationExpression = "\d+" runat="server" ErrorMessage="Only number allowed."></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="left">
                    <asp:TextBox ID="OtherUser" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
                <td align="left">
                    <asp:CheckBox ID="EnableAdditionalSpecialRequests" runat="server" Text=" Additional Special Requests" />
                </td>

            </tr>
            <tr>
                <td class="lblRight">Pricing Program:
                </td>
                <td class="left">
                    <asp:DropDownList ID="PricingProgram" runat="server" Width="185px">
                    </asp:DropDownList>
                </td>
                <td>&nbsp;</td>
                <td align="left">
                    <asp:CheckBox ID="IsDMAllocation" runat="server" Text=" Allocation" onclick="isDMAllocation_onclick()" />
                    <span>&nbsp;</span>
                    <asp:CheckBox ID="IsInternalAllocation" runat="server" Text=" Internal Only" onclick="isInternalAllocation_onclick()" />
                </td>
            </tr>
            <tr>
                <td class="lblRight">Onboarding Note:
                </td>
                <td class="left" colspan="3">
                    <asp:TextBox ID="OnboardingNote" runat="server" Height="50px" MaxLength="255" TextMode="MultiLine" Width="96%" onkeypress="return this.value.length < 255" ></asp:TextBox>
                    <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "OnboardingNote" ID="RegularExpressionValidator1" ValidationExpression = "^[\s\S]{0,255}$" runat="server" ErrorMessage="Maximum 255 characters allowed."></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
                <td class="lblRight"></td>
                <td align="left">
                </td>
            </tr>
        </table>
        </fieldset>
                            <table width="100%" style="vertical-align: top;">
                                <tr>
                                    <td valign="top">
                                        <fieldset style="height: 175px">
                                            <legend>Deal Count</legend>
                                            <table width="100%" class="tablecellborder">
                                                <tr>
                                                    <th></th>
                                                    <th>Submitted
                                                    </th>
                                                    <th>Approved
                                                    </th>
                                                    <th>Cancelled
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>WTD
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="WTDReceived" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="WTDApproved" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="WTDCancelled" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>MTD
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="MTDReceived" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="MTDApproved" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="MTDCancelled" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>YTD
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="YTDReceived" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="YTDApproved" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="YTDCancelled" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td># Active
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblActive" runat="server" Text=""></asp:Label>
                                                    </td>
                                                    <td align="center"></td>
                                                    <td align="center"></td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                    <td valign="top">
                                        <fieldset style="height: 175px">
                                            <legend>Top 5 Merchants</legend>
                                            <asp:Label ID="lblNoTopMerchants" runat="server" Text="   no data.." Visible="false"></asp:Label>
                                            <asp:GridView ID="grdMTDTopMerchants" runat="server" CssClass="mGrid" AllowPaging="True"
                                                Width="98%" Font-Names="Verdana" Font-Size="X-Small" AutoGenerateColumns="False">
                                                <PagerStyle CssClass="pgr" />
                                                <AlternatingRowStyle CssClass="alt" />
                                                <FooterStyle CssClass="footer" />
                                                <Columns>
                                                    <asp:BoundField DataField="MerchantAppUID" Visible="False" />
                                                    <asp:BoundField DataField="DBA" HeaderText="DBA" HtmlEncode="False">
                                                        <ItemStyle HorizontalAlign="left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TransCount" HeaderText="Sales Cnt" DataFormatString="{0:N0}"
                                                        HtmlEncode="False" SortExpression="TransCount">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="TransSales" DataFormatString="{0:0.00}" HeaderText="Sales Vol"
                                                        HtmlEncode="False" SortExpression="TransSales">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Net Income" DataFormatString="{0:0.00}" HeaderText="Net Income"
                                                        HtmlEncode="False" SortExpression="NetIncome" Visible="false">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <uc3:wucContact ID="wucContact1" runat="server" ControlContactType="Agent" />
                                    </td>
                                    <td>
                                        <fieldset style="height: 104px">
                                            <legend>SalesForce</legend>

                                            SalesForceID:
                                            <asp:TextBox ReadOnly="true" MaxLength="9" Width="60px" runat="server" Style="text-align: right" ID="SalesForceID"></asp:TextBox>

                                        </fieldset>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset style="height: 230px;">
                                            <legend>Business Address</legend>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" colspan="2"></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Country:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="BillingCountry" runat="server" Width="180px" OnSelectedIndexChanged="BillingCountry_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Name:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="BillingFullName" Width="180px" AutoPostBack="False" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Addr 1:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="BillingAddressLine1" Width="180px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Addr 2:</td>
                                                    <td>
                                                        <asp:TextBox ID="BillingAddressLine2" Width="180px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">City:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="BillingCity" Width="180px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">State/Region:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="BillingState" runat="server" Width="90px" Visible="true">
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
                                                        <asp:TextBox ID="BillingProvince" Width="80px" runat="server" Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Postal Code:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="BillingZipCode" Width="75px" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>

                                        </fieldset>
                                    </td>
                                    <td style="width: 50%;">
                                        <fieldset style="height: 230px;">
                                            <legend>Mailing Address</legend>
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:CheckBox ID="BillingAddressAsAbove" Text="Same As Bussiness Address" runat="server" OnCheckedChanged="BillingAddressAsAbove_CheckedChanged" AutoPostBack="true" Visible="false"></asp:CheckBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Mail Country:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="Country" runat="server" Width="180px" AutoPostBack="True" OnSelectedIndexChanged="Country_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Name:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="ShippingFullName" Width="180px" AutoPostBack="False" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Mail Addr 1:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="AddressLine1" runat="server" Width="180px"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Mail Addr 2:</td>
                                                    <td>
                                                        <asp:TextBox ID="AddressLine2" runat="server" Width="180px"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Mail City:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="City" runat="server" Width="180px"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">State/Region:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="State" runat="server" Width="90px">
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
                                                        <asp:TextBox ID="Province" Width="80px" runat="server" Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Mail Postal Code:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="ZipCode" runat="server" Width="75px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>

                            </table>
        </asp:Panel>
                    </asp:Panel>
                </td>
            </tr>
        </table>

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
    </div>
</asp:Content>
