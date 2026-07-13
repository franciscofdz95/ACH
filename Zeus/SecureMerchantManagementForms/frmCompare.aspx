<%@ Page Language="C#" AutoEventWireup="true" Inherits="frmCompare" Codebehind="frmCompare.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Compare Files</title>
    <style>
.MyStyle{ white-space:nowrap; }
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" Width="600px" ScrollBars="horizontal">
                            <asp:DataList ID="lstSource" runat="server" CellPadding="0" CellSpacing="0" BorderStyle="None"
                                BackColor="#e8e8e8" Width="100%" ItemStyle-BackColor="white" ItemStyle-ForeColor="black"
                                FooterStyle-Font-Size="9pt" FooterStyle-Font-Italic="True" OnItemDataBound="lst_ItemDataBound">
                                <HeaderTemplate>
                                    Approved Content
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblData" runat="server" CssClass="MyStyle" Text='<%# DataBinder.Eval(Container.DataItem, "line").ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "data").ToString())  %>'></asp:Label>
                                                <asp:Label ID="lblDiff" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DiffResult").ToString()  %>'
                                                    Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <FooterTemplate>
                                    © Paysafe Group plc
                                </FooterTemplate>
                                <FooterStyle Font-Italic="True" Font-Size="9pt" />
                                <HeaderStyle Font-Bold="True" Font-Names="Verdana" Font-Size="12pt" HorizontalAlign="Left" />
                            </asp:DataList></asp:Panel>
                        &nbsp;
                    </td>
                    <td>
                        <asp:Panel ID="Panel3" runat="server" Width="600px" ScrollBars="horizontal">
                            <asp:DataList ID="lstDestination" runat="server" CellPadding="0" CellSpacing="0"
                                BorderStyle="None" BackColor="#e8e8e8" Width="100%" ItemStyle-BackColor="white"
                                ItemStyle-ForeColor="black" FooterStyle-Font-Size="9pt" FooterStyle-Font-Italic="True"
                                OnItemDataBound="lst_ItemDataBound">
                                <HeaderTemplate>
                                    New Content
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblData" runat="server" CssClass="MyStyle" Text='<%# DataBinder.Eval(Container.DataItem, "line").ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;" + Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "data").ToString())  %>'></asp:Label></td>
                                            <asp:Label ID="lblDiff" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DiffResult").ToString()  %>'
                                                Visible="false"></asp:Label>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <FooterTemplate>
                                    © Paysafe Group plc
                                </FooterTemplate>
                                <FooterStyle Font-Italic="True" Font-Size="9pt" />
                                <HeaderStyle Font-Bold="True" Font-Names="Verdana" Font-Size="12pt" HorizontalAlign="Left" />
                            </asp:DataList></asp:Panel>
                        &nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
