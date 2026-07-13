<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageAgent.master"
    Inherits="frmAgentInfo" Title="Agent Background" Codebehind="frmAgentInfo.aspx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    <div class="tbrtools">
                        <div class="tbrtoolsleft">
                        <TABLE>
<TR>
    <TD><igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                <Appearance>
                                    <Image Url="~/Images/edit.png" />
                                </Appearance>
                            </igtxt:WebImageButton></TD>
    <TD><igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false"
                                AccessKey="s" CommandName="Save" OnClick="tbrTools_ButtonClicked">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue.png" />
                                </Appearance>
                            </igtxt:WebImageButton></TD>
    <TD><igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue_error.png" />
                                </Appearance>
                            </igtxt:WebImageButton></TD>
    
</TR>
</TABLE>
                            
                           
                        </div>
                    </div>
                    <asp:Panel ID="pnlDetail" runat="server" Height="" Width="">
                        <div class="title">
                            &nbsp;&nbsp;Agent Category
                            <hr class="line" />
                        </div>
                        <div class="indentedcontent20">
                            <asp:RadioButtonList runat="server" ID="AgentCategoryUID" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                        </div>
                        <br />
                        <div class="title">
                            &nbsp;&nbsp;Background Information
                            <hr class="line" />
                        </div>
                        <div class="indentedcontent20">
                            <table width="100%" border="0" cellpadding="0" cellspacing="5">
                                <tr>
                                    <td class="lblRight150">
                                        Agent Name:</td>
                                    <td>
                                        <asp:Label ID="AgentFullName" runat="server" Width="200px" MaxLength="50"></asp:Label>
                                    </td>
                                    <td class="lblRight150">
                                        DOB:</td>
                                    <td>
                                        <ig:WebDatePicker ID="DOB" runat="server" Width="205px" NullDateLabel="" NullValueRepresentation="Null"
                                            EnableAppStyling="False">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight150">
                                        No. of Agents:</td>
                                    <td>
                                        <asp:TextBox ID="NumberofAgents" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td class="lblRight150">
                                        No. of deals/month:</td>
                                    <td>
                                        <asp:TextBox ID="NumberofDeals" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight150">
                                        Other Processors:</td>
                                    <td>
                                        <asp:TextBox ID="OtherProcessors" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td class="lblRight150">
                                        Vertical markets:</td>
                                    <td>
                                        <asp:TextBox ID="VerticalMarkets" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight150">
                                        Experience level:</td>
                                    <td>
                                        <asp:TextBox ID="Experience" runat="server" Width="200px" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td class="lblRight150">
                                        Alma Mater:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AlmaMater" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div class="title">
                            &nbsp;&nbsp;Family Information
                            <hr class="line" />
                        </div>
                        <div class="indentedcontent20">
                            <table width="100%" border="0" cellpadding="0" cellspacing="5">
                                <tr>
                                    <td class="lblRight150">
                                        Anniversary:</td>
                                    <td>
                                        <ig:WebDatePicker ID="Anniversary" runat="server" NullDateLabel="" NullValueRepresentation="Null"
                                            Width="205px" EnableAppStyling="False">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                    </td>
                                    <td class="lblRight150">
                                        Spouse Name:</td>
                                    <td>
                                        <asp:TextBox ID="SpouseName" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight150">
                                        Child #1:</td>
                                    <td>
                                        <asp:TextBox ID="Child1" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td class="lblRight150">
                                        Child #1 DOB:</td>
                                    <td>
                                        <ig:WebDatePicker ID="Child1DOB" runat="server" NullDateLabel="" NullValueRepresentation="Null"
                                            Width="205px" EnableAppStyling="False">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight150">
                                        Child #2:</td>
                                    <td>
                                        <asp:TextBox ID="Child2" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td class="lblRight150">
                                        Child #2 DOB:</td>
                                    <td>
                                        <ig:WebDatePicker ID="Child2DOB" runat="server" NullDateLabel="" NullValueRepresentation="Null"
                                            Width="205px" EnableAppStyling="False">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight150">
                                        Child #3:</td>
                                    <td>
                                        <asp:TextBox ID="Child3" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td class="lblRight150">
                                        Child #3 DOB:</td>
                                    <td>
                                        <ig:WebDatePicker ID="Child3DOB" runat="server" NullDateLabel="" NullValueRepresentation="Null"
                                            Width="205px" EnableAppStyling="False">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <div class="title">
                            &nbsp;&nbsp;Misc Information
                            <hr class="line" />
                        </div>
                        <div class="indentedcontent20">
                            <table width="100%" border="0" cellpadding="0" cellspacing="5">
                                <tr>
                                    <td class="lblRight150" valign="top">
                                        Misc:
                                    </td>
                                    <td align="left" colspan="3">
                                        <asp:TextBox ID="Misc" runat="server" Width="658px" MaxLength="255" TextMode="multiline"
                                            Rows="4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight150">
                                        Favorite Sports Team:</td>
                                    <td>
                                        <asp:TextBox ID="FavoriteSportsTeam" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td class="lblRight150">
                                        Favorite Drink:</td>
                                    <td>
                                        <asp:TextBox ID="FavoriteDrink" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight150" valign="top">
                                        Pets:</td>
                                    <td valign="top">
                                        <asp:TextBox ID="Pets" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight150" valign="top">
                                    </td>
                                    <td valign="top">
                                    </td>
                                    <td class="lblRight150">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
