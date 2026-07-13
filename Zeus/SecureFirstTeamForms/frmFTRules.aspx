<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFirstTeam.Master" AutoEventWireup="true"
    CodeBehind="frmFTRules.aspx.cs" Inherits="ZeusWeb.SecureFirstTeamForms.frmFTRules" %>

<%@ Register Src="../UserControls/wucMRule.ascx" TagName="wucMRule" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/FirstTeam/wucFTRuleFilter.ascx" TagName="wucFTRuleFilter"
    TagPrefix="uc2" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contentpage" style="padding-left: 20px; padding-bottom: 20px;">
        <div class="titleline">
            Global Rules for Premier Services
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <uc1:wucMRule ID="wucMRule1" runat="server" ShowGlobalEnable="False" ControlRuleType="Global" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <div class="titleline">
            Merchant Rules
        </div>


        <table>
            <tr>
                <td class="lblRight">Merchant Enabled:</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlMerchantEnabled">
                    <asp:ListItem Value="-1">All</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="lblRight">Rule Name:</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlRuleName"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="lblRight">ZID:</td>
                <td>
                    <asp:TextBox runat="server" ID="tbZID"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="lblRight">DBA Name:</td>
                <td><asp:TextBox runat="server" ID="tbBusinessDBAName"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="lblRight">MLE:</td>
                <td><asp:TextBox runat="server" ID="tbBusinessLegalName"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="lblRight">PS Rep:</td>
                <td>
                
                <asp:DropDownList runat="server" ID="ddlReps"></asp:DropDownList>
                
                </td>
            </tr>
         <%--   <tr>
                <td>Start Date:</td>
                <td> <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                        Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                            SlideOpenDuration="1" />
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                            SlideOpenDuration="1" />
                    </ig:WebDatePicker></td>
            </tr>
            <tr>
                <td>End Date:</td>
                <td> <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                        Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                            SlideOpenDuration="1" />
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                            SlideOpenDuration="1" />
                    </ig:WebDatePicker></td>
            </tr>--%>

            <tr>
                <td></td>
                <td> <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />

                 <asp:Button ID="btnClear" runat="server" Text="Clear" onclick="btnClear_Click"  />
                
                </td>
            </tr>

        </table>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <uc2:wucFTRuleFilter ID="wucFTRuleFilter1" ShowGlobalEnable="False"  runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
