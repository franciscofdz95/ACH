<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucLeadNotes" Codebehind="wucLeadNotes.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<fieldset class="add-to-outlook">
    <legend>Notes</legend>
    <asp:Panel ID="pnlNotes" runat="server" Height="" Width="" CssClass="bucketbdy">
        <asp:Label runat="server" ID="ErrorMess" ForeColor="red" Font-Bold="true"></asp:Label>
        <table width="100%">
            <tr>
                <td valign="top">
                    Notes: (Required)
                </td>
                <td align="left" style="width: 90%;">
                    <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Height="75px" Width="100%"
                        Text=""></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnAddNotes" runat="server" Text="Add Notes" OnClick="btnAddNotes_Click"
                        Width="85px" CausesValidation="False" />
                    &nbsp;
                    <asp:Button ID="btnClear" runat="server" Text="Clear Notes" OnClick="btnClearNotes_Click"
                        Width="85px" CausesValidation="False" />
                </td>
            </tr>
        </table>
        <asp:Panel runat="server" ID="pnlApp1" Visible="false">
            <br />
            <table width="100%">
                <tr>
                    <td>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="pnlLeadNotes" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdLeadNotes" runat="server" OnRowCommand="grdLeadNotes_RowCommand"
                                    OnRowDataBound="grdLeadNotes_RowDataBound" AutoGenerateColumns="False" DataKeyNames="LeadNotesID"
                                    Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                    PageSize="5" AlternatingRowStyle-CssClass="alt" AllowPaging="true" OnPageIndexChanging="grdLeadNotes_PageIndexChanging">
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkID" runat="server" CausesValidation="false" CommandName="ID"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="45px" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Notes" DataField="Notes">
                                            <ItemStyle />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                                            <ItemStyle Width="85px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="User Created" DataField="UserCreated">
                                            <ItemStyle Width="85px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LeadNotesID" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="LeadID" Visible="false"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                                    Font-Size="X-Small" Width="100%">
                                    <HeaderStyle BackColor="#e5e5e5" Font-Bold="true" Font-Names="verdana" Font-Size="X-Small" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                    <Columns>
                                        <asp:BoundField HeaderText="ID">
                                            <ItemStyle Width="30px" />
                                            <HeaderStyle Width="30px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Notes">
                                            <ItemStyle Width="300px" />
                                            <HeaderStyle Width="200px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Date Created">
                                            <ItemStyle Width="65px" />
                                            <HeaderStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="User Created">
                                            <ItemStyle Width="70px" />
                                            <HeaderStyle Width="70px" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="200px" Width="500px"
                                    Modal="True" InitialLocation="Centered" WindowState="hidden">
                                    <ContentPane>
                                        <Template>
                                            <br />
                                            <table style="vertical-align: middle;" align="center">
                                                <tr>
                                                    <td valign="top" class="lblRight">
                                                        Notes:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLeadNotes" runat="server" TextMode="MultiLine" Style="width: 400px;
                                                            height: 75px;" ReadOnly="true">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <br />
                                                        <asp:Button ID="btnCloseNotes" runat="server" Text="Close" Width="50px" OnClick="btnCloseNotes_Click"
                                                            CausesValidation="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </Template>
                                    </ContentPane>
                                    <Header CaptionText="Notes" CloseBox-Visible="false">
                                    </Header>
                                </ig:WebDialogWindow>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td align="right">
                        Page Size:
                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                            <asp:ListItem Selected="True">5</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</fieldset>
