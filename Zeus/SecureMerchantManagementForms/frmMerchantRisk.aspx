<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="True"
    Inherits="frmMerchantRisk" Title="Risk" CodeBehind="frmMerchantRisk.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Src="../UserControls/wucServices.ascx" TagName="wucServices" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc4" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<%@ Register Src="~/UserControls/Reserve/wucDiversionGrid.ascx" TagName="wucDiversionGrid"
    TagPrefix="uc6" %>
<%@ Register Src="~/UserControls/Reserve/wucDiversionDialog.ascx" TagName="wucDiversionDialog"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
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
                            <uc4:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                            <ig:WebTab ID="tabReport" runat="server" Width="970px" SelectedIndex="2">
                                <Tabs>
                                    <ig:ContentTabItem runat="server" Text="Divert">
                                        <Template>
                                            <div class="indentedcontent20">
                                                <br />
                                                <asp:Panel runat="server" CssClass="errorlist" ID="pnlDiversionDates">
                                                    <asp:Label runat="server" ID="lblDiversionNotice"></asp:Label>
                                                    <asp:BulletedList runat="server" ID="blDiv">
                                                    </asp:BulletedList>
                                                    <br />
                                                </asp:Panel>
                                                <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Add" />
                                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Refresh" />
                                                <uc6:wucDiversionGrid ID="wucDiversionGrid1" runat="server" />
                                            </div>
                                        </Template>
                                    </ig:ContentTabItem>
                                    <ig:ContentTabItem runat="server" Text="Risk Parameters">
                                        <Template>
                                            <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
                                                <div>
                                                    <br />
                                                    <br />
                                                    <div class="title">
                                                        &nbsp;<a name="RiskParameters">Payment XP Risk Parameters</a>
                                                        <hr class="line" />
                                                    </div>
                                                    <div class="indentedcontent20">
                                                        <br />
                                                       <%-- DataKeyNames="UID"--%>
                                                        <asp:Panel ID="pnlGrd" runat="server" Enabled="False" Width="50%">
                                                            <asp:GridView ID="grdRisk" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                                 Font-Names="verdana" Font-Size="X-Small" OnRowDataBound="grdRisk_RowDataBound">
                                                                <PagerStyle CssClass="pgr" />
                                                                <AlternatingRowStyle CssClass="alt" />
                                                                <FooterStyle CssClass="footer" />
                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="RiskID" HeaderText="ID"></asp:BoundField>
                                                                    <asp:BoundField DataField="Exception" HeaderText="Exception"></asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Enable">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="chkEnabled_CheckedChanged" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Threshold">
                                                                        <ItemTemplate>
                                                                            <ig:WebNumericEditor ID="WebNumericEditor" runat="server" BorderStyle="None" DataMode="Decimal"
                                                                                MinDecimalPlaces="4" ValueText="0.0000" AutoPostBack="True" OnValueChange="WebNumericEdit_ValueChange">
                                                                            </ig:WebNumericEditor>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="UID"  HeaderText="UID" ItemStyle-CssClass="hideGridColumn"  HeaderStyle-CssClass="hideGridColumn"></asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <asp:Label runat="server" ID="lblFee" Text="no data.." Visible="False"></asp:Label>
                                                        </asp:Panel>
                                                    </div>
                                                    <div style="display: none">
                                                        <br />
                                                        <br />
                                                        <div class="title">
                                                            &nbsp;<a name="InternalDeclineRule">Internal Decline Rule</a>
                                                            <hr class="line" />
                                                        </div>
                                                        <div class="indentedcontent20">
                                                            <br />
                                                            <asp:CheckBox ID="MeritusDeclineRule" runat="server" Text="Enable Paysafe Internal Decline Rule" />
                                                            <br />
                                                            <asp:CheckBox ID="ShowMeritusDeclineRule" runat="server" Text="Show Paysafe Internal Decline Rule to Merchant" />
                                                            <br />
                                                            <br />
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <br />
                                                    <div class="title">
                                                        &nbsp;&nbsp;MISC
                                                        <hr class="line" />
                                                    </div>
                                                    <div class="indentedcontent20">
                                                        <asp:CheckBox Text="Placed on MATCH" ID="PlacedOnMATCH" runat="server"></asp:CheckBox>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </Template>
                                    </ig:ContentTabItem>
                                    <ig:ContentTabItem runat="server" Text="Association Programs">
                                        <Template>
                                            <div style="margin-left: 15px;">
                                                <div>
                                                    <br />
                                                    <asp:Panel runat="server" CssClass="errorlist" ID="pnlAssocProgramAlert" Width="98%" Visible="false">
                                                        <asp:Label runat="server" ID="lblAssocProgramMsg"></asp:Label>
                                                        <asp:BulletedList runat="server" ID="blAssocPrograms">
                                                    </asp:BulletedList>
                                                    <br />
                                                </asp:Panel>
                                                    <br />
                                                </div>
                                                <asp:Panel runat="server" ID="pnlAssocProgram">
                                                    <table>
                                                        <tr>
                                                            <td class="lblRight">Card Association:</td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ChargebackCardID" OnSelectedIndexChanged="ChargebackCardID_SelectedIndexChanged" AutoPostBack="true" Width="125px">
                                                                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                                    <asp:ListItem Value="1">Visa</asp:ListItem>
                                                                    <asp:ListItem Value="2">MasterCard</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">Program Name:</td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ChargebackCardProgramID" Width="125px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">Program Month:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ProgramMonth" Width="125px">
                                                                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                                    <asp:ListItem Value="EW">Early Warning</asp:ListItem>
                                                                    <asp:ListItem Value="N">N</asp:ListItem>
                                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                                    <asp:ListItem Value="2">2</asp:ListItem>
                                                                    <asp:ListItem Value="3">3</asp:ListItem>
                                                                    <asp:ListItem Value="4">4</asp:ListItem>
                                                                    <asp:ListItem Value="5">5</asp:ListItem>
                                                                    <asp:ListItem Value="6">6</asp:ListItem>
                                                                    <asp:ListItem Value="7">7</asp:ListItem>
                                                                    <asp:ListItem Value="8">8</asp:ListItem>
                                                                    <asp:ListItem Value="9">9</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">Calendar Month/Year:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="CalendarMonth" Width="125px">
                                                                    <asp:ListItem Value="1">Jan</asp:ListItem>
                                                                    <asp:ListItem Value="2">Feb</asp:ListItem>
                                                                    <asp:ListItem Value="3">Mar</asp:ListItem>
                                                                    <asp:ListItem Value="4">Apr</asp:ListItem>
                                                                    <asp:ListItem Value="5">May</asp:ListItem>
                                                                    <asp:ListItem Value="6">Jun</asp:ListItem>
                                                                    <asp:ListItem Value="7">Jul</asp:ListItem>
                                                                    <asp:ListItem Value="8">Aug</asp:ListItem>
                                                                    <asp:ListItem Value="9">Sep</asp:ListItem>
                                                                    <asp:ListItem Value="10">Oct</asp:ListItem>
                                                                    <asp:ListItem Value="11">Nov</asp:ListItem>
                                                                    <asp:ListItem Value="12">Dec</asp:ListItem>
                                                                </asp:DropDownList>
                                                                &nbsp;
                                                    <asp:TextBox runat="server" ID="CalendarYear" MaxLength="4" Width="75px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">Date Received:
                                                            </td>
                                                            <td>
                                                                <ig:WebDatePicker ID="DateReceived" runat="server" EnableAppStyling="False" Width="90px"
                                                                    BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                        SlideOpenDuration="1" />
                                                                </ig:WebDatePicker>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblErrorMsg" EnableViewState="false" ForeColor="Red"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:Button Text="Add" ID="btnAddProgram" OnClick="btnAddProgram_Click" runat="server" />
                                                                <br />
                                                                <br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <div>
                                                    <asp:GridView ID="grdAssociationProg" runat="server" AutoGenerateColumns="False" CssClass="mGrid" Font-Names="verdana" 
                                                        Font-Size="X-Small" OnRowDataBound="grdAssociationProg_RowDataBound" Width="95%">
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <Columns>
                                                            <asp:BoundField DataField="StartProgramDate" HeaderText="Calendar Date"></asp:BoundField>
                                                            <asp:BoundField DataField="CardAssociationName" HeaderText="Card Association"></asp:BoundField>
                                                            <asp:BoundField DataField="AssociationProgramName" HeaderText="Program"></asp:BoundField>
                                                            <asp:BoundField DataField="ProgramMonth" HeaderText="Program Month"></asp:BoundField>
                                                            <asp:BoundField DataField="DateReceived" HeaderText="Date Received"></asp:BoundField>
                                                            <asp:BoundField DataField="UserCreated" HeaderText="Added By"></asp:BoundField>
                                                            <asp:BoundField DataField="DateCreated" HeaderText="Date Added"></asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" ID="lnkDelete" Text="Delete" CommandArgument='<%# Bind("ChargebackProgramID") %>' OnClientClick="return confirm('Are you sure you want to delete this associate program record?');" CausesValidation="false" OnClick="lnkDelete_Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </Template>
                                    </ig:ContentTabItem>                                
                                     <ig:ContentTabItem runat="server" Text="Risk Actions">
                                       <Template>
                                            <div style="margin-left: 15px;">
                                                  <div>
                                                      <br />
                                            <asp:Panel runat="server" ID="pnlRiskFlag">
                                                <br>
                                                </br>
                                                  
                                                    <table>
                                                        <tr>
                                                            <td class="lblLeft">Sales Flags:</td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="SalesFlag" Width="125px">
                                                                   <asp:ListItem Value="False">OFF</asp:ListItem>
                                                                    <asp:ListItem Value="True">ON</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblLeft">Restrict PaymentXP Amount Processed:</td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="AllowAmount" Width="125px">
                                                                   <asp:ListItem Value="False">OFF</asp:ListItem>
                                                                    <asp:ListItem Value="True">ON</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        </table>
                                              
                                                <br>
                                              
                                                </br>
                                                 
                                        </asp:Panel>
                                                  </div>
                                                </div>
                                             
                                       </Template>
                                         </ig:ContentTabItem>    
                                  
                                        <ig:ContentTabItem runat="server" Text="Chargeback Management">
                                            <Template>
                                                    <div style="margin-left: 15px;">
                                                      <br />
                                                 <asp:Panel runat="server" ID="pnlChargebackManagement">
                                                       
                                                <table>
                                                    <tr>
                                                     <td class="lblLeft">Dispute via eResponse:</td>
                                                            
                                                        <td>
                                                              <asp:DropDownList runat="server" ID="DisputeViaEResponse" Width="125px">
                                                                   <asp:ListItem Value="False">OFF</asp:ListItem>
                                                                    <asp:ListItem Value="True">ON</asp:ListItem>
                                                                </asp:DropDownList>
                                                        </td>
                                                        </tr>
                                                </table>
                                                    <br />
                                                     </asp:Panel>
                                                        </div>
                                            </Template>
                                        </ig:ContentTabItem>
                                
                                 </Tabs>
                            </ig:WebTab>
                        </td>
                    </tr>
                </table>
            </div>
            <uc1:wucDiversionDialog ID="wucDiversionDialog1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
