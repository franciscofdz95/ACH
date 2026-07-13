<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true"
    Inherits="frmMerchantSearch" Title="Merchant Profile Search" CodeBehind="frmMerchantSearch.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/wucAccountGroups.ascx" TagName="wucAccountGroups" TagPrefix="uc8" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <style type="text/css">
    input, select, textarea
    {
        box-sizing: border-box;
        -moz-box-sizing: border-box;
        -webkit-box-sizing: border-box;
    }
       
     </style>
    <script language="javascript" type="text/javascript">

        function confirmMerchant(first) {
            if (first == 'True')
                return confirm("This is a 'Premier Services' merchant.");
            else
                return true;
        }

    </script>

    <div id="contentpage">
        <asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        <table width="100%">
            <tr>
                <td>
                    <fieldset>
                        <legend>Merchant Search</legend>
                        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                            <table width="100%">
                                <tr>
                                    <td class="lblRight">Rscv Begin:
                                    </td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" Width="90px"
                                            BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                        </ig:WebDatePicker>
                                    </td>
                                    <td class="lblRight">Rscv End:
                                    </td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" Width="90px"
                                            BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                        </ig:WebDatePicker>
                                    </td>
                                    <td class="lblRight">DBA:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessDBAName" runat="server" EnableViewState="False" Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">MLE:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessLegalName" runat="server" Width="90px" EnableViewState="False"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">MID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SettlePlatformMid" runat="server" EnableViewState="False" Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">
                                        <asp:Label ID="lblBank" runat="server" Text="Acq Bank:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="90px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">SSN:
                                    </td>
                                    <td>
                                        <ig:WebMaskEditor ID="SSN" runat="server" InputMask="###-##-####" Width="90px">
                                        </ig:WebMaskEditor>
                                    </td>
                                    <td class="lblRight">Owner:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="OwnerName" runat="server" EnableViewState="False" Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Premier Services:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="FirstTeam" runat="server" Width="90px">
                                            <asp:ListItem>All</asp:ListItem>
                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                            <asp:ListItem Value="0">No</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">Gateway:</td>
                                    <td>
                                      
                                        <asp:DropDownList runat="server" Width="90px" ID="MerchantBrandID">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                            <asp:ListItem Value="1">3rd Party</asp:ListItem>
                                            <asp:ListItem Value="2">NetBanx</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        <asp:Label ID="lblACHID" Width="90px" runat="server" Text="ACH/DD ID:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AchID" runat="server" EnableViewState="False" Width="90px" onKeyPress ="CheckNumeric()"></asp:TextBox>
                                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="AchID"
                                            ErrorMessage="Please enter a valid AID." MaximumValue="100000" MinimumValue="1"
                                            Type="Integer" Display="None"></asp:RangeValidator>
                                    </td>
                                    <td class="lblRight">Phone #:
                                    </td>
                                    <td>
                                        <ig:WebMaskEditor ID="BusinessPhone" runat="server" InputMask="##############################" PromptChar=' ' Width="90px" ShowMaskOnFocus="False">
                                        </ig:WebMaskEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">State:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="BusinessState" runat="server" Width="90px">
                                            <asp:ListItem>All</asp:ListItem>
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
                                    <td class="lblRight">Contact:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessContact" runat="server" EnableViewState="False" Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">DBA Phone:
                                    </td>
                                    <td>
                                        <ig:WebMaskEditor ID="BusinessDBAPhone" runat="server" InputMask="##############################" PromptChar=' ' Width="90px" ShowMaskOnFocus="False">
                                        </ig:WebMaskEditor>
                                    </td>
                                    <td class="lblRight">
                                        <asp:Label ID="lblAssignTo" Width="90px" runat="server" Text="Assign To:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="AssignToUID" runat="server" Width="90px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">ZID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="MerchantID" runat="server" EnableViewState="False" Width="90px" onKeyPress ="CheckNumeric()"></asp:TextBox>
                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="MerchantID"
                                            ErrorMessage="Please enter a valid ZID." MaximumValue="10000000" MinimumValue="1"
                                            Type="Integer" Display="None"></asp:RangeValidator>
                                    </td>
                                    <td class="lblRight">Fax #:
                                    </td>
                                    <td>
                                        <ig:WebMaskEditor ID="BusinessFax" runat="server" InputMask="##############################" PromptChar=' ' Width="90px" ShowMaskOnFocus="False">
                                        </ig:WebMaskEditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight" >Postal Code:
                                    </td>
                                    <td >
                                        <asp:TextBox ID="BusinessZip" runat="server" EnableViewState="False" Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight"  ><asp:Label Text="Front End:" runat="server" ID="lblFrontEnd" Width="90px"></asp:Label>
                                    </td>
                                    <td >
                                        <asp:DropDownList ID="AuthPlatformUID" runat="server" Width="90px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight" >
                                        <asp:Label Text="Front MID:" runat="server" ID="lbl" Width="90px"></asp:Label>
                                    </td>
                                    <td >
                                        <asp:TextBox ID="AuthPlatformMid" runat="server" EnableViewState="False" Width="90px"></asp:TextBox>
                                    </td >
                                    <td class="lblRight"  >Back-End:
                                    </td>
                                    <td >
                                        <asp:DropDownList ID="SettlePlatformUID" runat="server" Width="90px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight"  >TID:
                                    </td>
                                    <td >
                                        <asp:TextBox ID="TID" runat="server" MaxLength="30" Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight"  >
                                        <asp:Label ID="lblMerchantTag" runat="server" Width="90px" Text="Merchant Tag:"></asp:Label>
                                    </td>
                                    <td >
                                        <asp:TextBox ID="MerchantTag" runat="server" EnableViewState="False" Width="90px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Email:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessEmailAddress" runat="server" Width="90px" EnableViewState="False"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">PS Rep:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="FTRep" runat="server" Width="90px">
                                        </asp:DropDownList>
                                    </td>
                                   <%-- <td class="lblRight">
                                        FMA Name:</td>
                                    <td>
                                        <asp:TextBox ID="FMAName" runat="server" MaxLength="30" Width="90px"></asp:TextBox>
                                    </td>  --%>                                 
                                    <td colspan="4">
                                      
                                        <asp:Panel runat="server" ID="AgentSelect">
                                            <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="horizontal"
                                                IDWidth="58" DBAWidth="90" lblDBAWidth="94px" lblIDWidth="101px" />
                                        </asp:Panel>
                                      
                                    </td>
                                    <td class="lblRight"><asp:Label ID="lblFMAID" runat="server" Width="90px" Text="FMA ID:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="FMAID" runat="server" MaxLength="15" Width="90px" onKeyPress ="CheckNumeric()"></asp:TextBox>
                                    </td> 
                                    <%--Code added by koshlendra for PXP-1621- User should able to search merchant by MCC code start--%> 
                                    <td class="lblRight"><asp:Label ID="lblMCC" runat="server" Width="90px" Text="Non-Visa MCC:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="MCC" runat="server" MaxLength="4" Width="90px" onKeyPress ="CheckNumeric()"></asp:TextBox>
                                    </td>  
                                    <%--Code added by koshlendra for PXP-1621- User should able to search merchant by MCC code end--%>                                  
                                </tr>
                                <tr>
                                    <td class="lblRight">Status:
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="StatusUID" runat="server" Width="99%">
                                        </asp:DropDownList>
                                    </td>                                    
                                      <asp:Panel ID ="pnlAccGroupSearch" runat="server">
                                   <td class="lblRight">Account Group:</td>
                                    <td colspan="5">
                                        <asp:UpdatePanel ID="UpdatePanelAccGroups" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc8:wucAccountGroups runat="server" ID="wucAccountGroups"  ShowValidators="false" ControlWidth="285px" ControlType="TextBox"/>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                        </asp:Panel>                                  
                                    <%--Code added by koshlendra for PXP-12945- User should able to search merchant by MCC code start--%> 
                                    <td class="lblRight"><asp:Label ID="lblVisaMCC" runat="server" Width="90px" Text="Visa MCC:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="VisaMCC" runat="server" MaxLength="4" Width="90px" onKeyPress ="CheckNumeric()"></asp:TextBox>
                                    </td>  
                                    <%--Code added by koshlendra for PXP-12946- User should able to search merchant by MCC code end--%>                                                                      
                                </tr>
                                <tr>
								    <asp:Panel runat="server" ID="pnlSpace" Visible="true">
                                            <td class="lblRight"></td>
                                            <td colspan="9"></td>
                                    
                                    </asp:Panel>
                                     <asp:Panel runat="server" ID="pnlAgent" Visible="false">
                                        <td class="lblRight">
                                            <asp:CheckBox runat="server" ID="SubAgent" />
                                        </td>
                                        <td colspan="9">
                                            <asp:Label runat="server" ID="lblSubAgent" Text="Include Sub-Agents" />
                                        </td>                                      
                                    </asp:Panel>
                                    <%--Niranjan: PXP-8060--%>                                     
                                    <td class="lblRight"><asp:Label ID="lblNMIUserName" runat="server" Width="90px" Text="NMI User Name:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="NMIUserName" runat="server" Width="90px" EnableViewState="False"></asp:TextBox>
                                    </td>
                                      <%--Niranjan: PXP-8060--%>
								</tr>
                                <tr>
                                    
                                     <asp:Panel runat="server" ID="pnlTilled">
                                        <td class="lblRight">
                                             <asp:CheckBox runat="server" ID="Tilled" />
                                        </td>
                                        <td colspan="9">
                                            <asp:Label runat="server" ID="lblTilled" Text="Tilled Only" />
                                        </td>                                      
                                    </asp:Panel>              
                                       <%--Code added by koshlendra for PXP-12946- User should able to search merchant by Descriptor code start--%> 
                                    <td class="lblRight"><asp:Label ID="lblDescriptor" runat="server" Width="90px" Text="Descriptor:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="Descriptor" runat="server" Width="90px" EnableViewState="False"></asp:TextBox>
                                    </td>
                                     <%--Code added by koshlendra for PXP-12946- User should able to search merchant by Descriptor code end--%> 
                                </tr>
                                
                            </table>
                            <div>
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                                    AccessKey="h">
                                                    <Appearance>
                                                        <Image Url="~/Images/Check.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                                                    CausesValidation="false" AccessKey="l">
                                                    <Appearance>
                                                        <Image Url="~/Images/delete.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add" CausesValidation="False"
                                                    AccessKey="a" OnClick="btnAdd_Click">
                                                    <Appearance>
                                                        <Image Url="~/Images/add2.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </asp:Panel>
                    </fieldset>
                    <br />
                    <fieldset>
                        <legend>Search Results</legend>
                        <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                            <table width="100%">
                                <tr>
                                    <td class="lblLeft">Page Size:
                                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                            <asp:ListItem Selected="True">10</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                            <asp:ListItem>500</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                            AlternatingRowStyle-CssClass="alt" DataKeyNames="MerchantAppUID" OnRowDataBound="grd_RowDataBound"
                                            OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True" OnSorting="grd_Sorting" DataSourceID="odsMerchants">
                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;" LastPageText="&raquo;" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="ZID" SortExpression="ID">
                                                    <ItemTemplate>
                                                        <asp:HyperLink NavigateUrl='<%#"~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&Adding=false"  %>'
                                                            runat="server" ID="hypZID" Text='<%# Eval("ID") %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="FMAID" HeaderText="FMA ID" SortExpression="FMAID" Visible="false"><ItemStyle Width="50px" /></asp:BoundField>
                                                <asp:TemplateField HeaderText="DBA" SortExpression="BusinessDBAName">
                                                    <ItemTemplate>
                                                        <asp:HyperLink runat="server" ID="hypDBAName" Text='<%# Eval("BusinessDBAName") %>' NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmUnderwritingPending.aspx?UID=" + Eval("MerchantAppUID") + "&MerchantAppUID=" + Eval("MerchantAppUID") + "&Adding=false"  %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="183px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BusinessLegalName" HeaderText="MLE" SortExpression="BusinessLegalName">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SettlePlatformMid" HeaderText="MID" SortExpression="SettlePlatformMid">
                                                    <ItemStyle Width="75px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Bank" HeaderText="Bank" SortExpression="Bank">
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AgentID" HeaderText="Agent ID" SortExpression="AgentID">
                                                    <ItemStyle Width="50px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="AgentFullName" HeaderText="Agent DBA" SortExpression="AgentFullName">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="DateCreated" HeaderText="Received Date" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                                                    SortExpression="DateCreated">
                                                    <ItemStyle Width="65px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="MerchantAppUID" HeaderText="MerchantAppUID" Visible="False" />
                                                <asp:BoundField DataField="StatusUID" HeaderText="StatusUID" Visible="False" />
                                                <asp:BoundField DataField="PrivateLabelUID" HeaderText="Private Label UID" Visible="False" />
                                                <asp:TemplateField HeaderText="Gateway" Visible="false">
                                                    <ItemTemplate><asp:Label ID="lblMerchantBrandID" runat="server" Text='<%# Bind("MerchantBrandID") %>'></asp:Label></ItemTemplate><ItemStyle Width="50px" />
                                                </asp:TemplateField>
                                               <asp:BoundField DataField="Currency" HeaderText="Currency" SortExpression="Currency" Visible="false"><ItemStyle Width="50px" /></asp:BoundField>
                                                
                                                <asp:BoundField DataField="ETF" HeaderText="ETF" SortExpression="ETF" DataFormatString="{0:C}" ><ItemStyle Width="50px" /></asp:BoundField>                      
                                                <asp:BoundField DataField="ETFAssessed" HeaderText="ETF Assessed" SortExpression="ETFAssessed" ><ItemStyle Width="50px" /></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsMerchants" runat="server" SelectMethod="GetMerchantAppsPaging"
                                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                            SelectCountMethod="GetMerchantAppsPagingRowCount" StartRowIndexParameterName="CurrentPage"
                                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsMerchants_Selecting">
                                            <SelectParameters>
                                                <asp:Parameter Name="prms" Type="Object" />
                                                <asp:Parameter Name="PageSize" Type="Int32" />
                                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                                                <asp:Parameter Name="ControlID" Type="String" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="bucketfooter">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" style="width: 33%;">
                                                        <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                            <span style="height: 25px; vertical-align: middle;">
                                                                <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                                    Excel</span>
                                                        </asp:LinkButton>&nbsp;&nbsp;
                                                        <asp:LinkButton ID="btnPDF" runat="server" OnClick="btnExportPDF_Click">
                                                            <span style="height: 25px; vertical-align: middle;">
                                                                <asp:Image ID="Image1" runat="server" SkinID="SavePDF" /></span><span style="margin-left: 5px;">Save
                                                                    PDF</span>
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td align="right">Export:&nbsp;
                                                    </td>
                                                    <td align="left">
                                                        <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                                            <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                                            <asp:ListItem Value="1">All Pages</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td align="right" style="width: 33%;"></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                            No data...
                        </asp:Panel>
                    </fieldset>
                    <br />
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
