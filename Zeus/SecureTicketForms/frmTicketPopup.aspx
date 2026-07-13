<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" Inherits="frmTicketPopup" CodeBehind="frmTicketPopup.aspx.cs" %>

<%@ Register Src="../UserControls/wucTicket.ascx" TagName="wucTicket" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/wucCommonScripts.ascx" TagName="wucCommonScripts"
    TagPrefix="uc4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ticket</title>
    <uc4:wucCommonScripts ID="wucCommonScripts1" runat="server" />
</head>
<body style="background-color: White;">
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <uc1:wucTicket ID="ticket1" runat="server" TicketPopup="true" />
        </div>
    </form>
</body>
</html>
