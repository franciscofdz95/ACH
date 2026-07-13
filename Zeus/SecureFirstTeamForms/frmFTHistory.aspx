<%@ Page Title="Rules Run History" Language="C#" MasterPageFile="~/MasterPageFirstTeam.Master"
    AutoEventWireup="true" CodeBehind="frmFTHistory.aspx.cs" Inherits="ZeusWeb.SecureFirstTeamForms.frmFTHistory" %>

<%@ Register Src="../UserControls/FirstTeam/wucFTGridHistory.ascx" TagName="wucFTGridHistory"
    TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%--<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contentpage" style="padding-left: 20px; padding-bottom: 20px;">
        <div class="titleline">
            Run History

        </div>
        <table>
            <tr>                
                <td>
                    Date Begin: 
                </td>
                <td>
                    <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                        Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                            SlideOpenDuration="1" />
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                            SlideOpenDuration="1" />
                    </ig:WebDatePicker>
                </td>
            </tr>
            <tr>
                <td>
                    Date End: 
                </td>
                <td>
                    <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                        Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                            SlideOpenDuration="1" />
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                            SlideOpenDuration="1" />
                    </ig:WebDatePicker>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                </td>
            </tr>
        </table>
        <hr />
        <uc1:wucFTGridHistory ID="wucFTGridHistory1" runat="server" />
    </div>
</asp:Content>
