<%@ Control Language="C#" AutoEventWireup="true" Inherits="pnlDates" Codebehind="pnlDates.ascx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<table>
    <tr>
        <td class="lblEdit">
            <asp:Label ID="Label1" runat="server" Text="Start Date:"></asp:Label>
        </td>
        <td>
            <ig:WebDatePicker id="StartDate" runat="server" nulldatelabel="">
                
                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
            </td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="StartDate"
                ErrorMessage="*" Font-Size="10pt"></asp:RequiredFieldValidator></td>
        <td class="lblEdit">
            <asp:Label ID="Label2" runat="server" Text="End Date:"></asp:Label>
        </td>
        <td>
            <ig:WebDatePicker id="EndDate" runat="server" nulldatelabel="">
              <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
            </td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="EndDate"
                ErrorMessage="*" Font-Size="10pt"></asp:RequiredFieldValidator></td>
    </tr>
</table>
