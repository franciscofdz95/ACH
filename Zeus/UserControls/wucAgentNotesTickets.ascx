<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucAgentNotesTickets"
    CodeBehind="wucAgentNotesTickets.ascx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="wucAgentNotes.ascx" TagName="wucAgentNotes" TagPrefix="uc1" %>
<div style="width: 320px;">
    <uc1:wucAgentNotes ID="WucAgentNotes1" runat="server">
    </uc1:wucAgentNotes>
</div>
