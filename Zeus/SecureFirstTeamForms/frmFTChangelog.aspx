<%@ Page Title="Rules Change Log" Language="C#" MasterPageFile="~/MasterPageFirstTeam.Master"
    AutoEventWireup="true" CodeBehind="frmFTChangelog.aspx.cs" Inherits="ZeusWeb.SecureFirstTeamForms.frmFTChangelog" %>

<%@ Register Src="../UserControls/FirstTeam/wucFTGridChangelog.ascx" TagName="wucFTGridChangelog"
    TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%--<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contentpage" style="padding-left: 20px; padding-bottom: 20px;">
        <div class="titleline">
            Change Log 
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
                    PS Rep: 
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlReps"></asp:DropDownList>
                </td>
            </tr>
             <tr>
                <td>
                    DBA: 
                </td>
                <td>
                   <asp:TextBox runat="server" ID="tbDBA"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td>
                   ZID: 
                </td>
                <td>
                   <asp:TextBox runat="server" ID="tbZID"></asp:TextBox>
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
        <uc1:wucFTGridChangelog ID="wucFTGridChangelog1" runat="server" />
    </div>
</asp:Content>
