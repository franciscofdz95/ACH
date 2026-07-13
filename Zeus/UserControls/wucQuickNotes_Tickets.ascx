<%@ Control Language="C#" AutoEventWireup="True" Inherits="wucQuickNotes_Tickets"
    CodeBehind="wucQuickNotes_Tickets.ascx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<fieldset style="width: 320px;">
    <legend>Last Tickets</legend>
    <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlDetail" runat="server">
                <asp:Label ID="lblError" runat="server" CssClass="gen_error"></asp:Label>
                <table border="0" width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlRecords" runat="server" Height="" Width="">
                                <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                                    Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    DataKeyNames="TicketUID" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                                    OnRowDataBound="grd_RowDataBound" PageSize="4" DataSourceID="odsTickets">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" SortExpression="TicketID">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnTicket" runat="server" CausesValidation="false" CommandName="View"
                                                    Text=""></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="30px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                            <ItemStyle Width="40px" />
                                        </asp:BoundField>
                                        <%-- <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category">
                                            <ItemStyle Width="90px" />
                                        </asp:BoundField>--%>
                                        <asp:BoundField DataField="ParentCategory" HeaderText="Category" SortExpression="ParentCategory">
                                            <ItemStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                                            <ItemStyle Width="60px" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <asp:ObjectDataSource ID="odsTickets" runat="server" SelectMethod="GetTicketsPaging"
                                    TypeName="DataMerchantAppPaging" EnablePaging="true" MaximumRowsParameterName="PageSize"
                                    SelectCountMethod="GetTicketsPagingCount" StartRowIndexParameterName="CurrentPage"
                                    OldValuesParameterFormatString="original_{0}" OnSelecting="odsTickets_Selecting">
                                    <SelectParameters>
                                        <asp:Parameter Name="prms" Type="Object" />
                                        <asp:Parameter Name="PageSize" Type="Int32" />
                                        <asp:Parameter Name="CurrentPage" Type="Int32" />
                                        <asp:Parameter Name="ControlID" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                                No data...
                            </asp:Panel>
                            <asp:HyperLink ID="btnTicket" CssClass="fakea" runat="server">New Ticket</asp:HyperLink>&nbsp;
                            <asp:LinkButton ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" CausesValidation="false">Refresh</asp:LinkButton>
                            <asp:HiddenField ID="hdnMerchantUID" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
<fieldset style="width: 320px;">
    <legend>Last Notes</legend>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="Records">
                <asp:GridView ID="grdNotes" runat="server" OnRowDataBound="grdNotes_RowDataBound" OnRowCommand="grdNotes_RowCommand" AutoGenerateColumns="False" Font-Names="Verdana"
                    PageSize="4" DataSourceID="odsNotes" OnPageIndexChanging="grdNotes_PageIndexChanging"  OnSorting="grdNotes_Sorting" AllowSorting="true"
                    AllowPaging="true" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                    AlternatingRowStyle-CssClass="alt" DataKeyNames="Subject,Notes,BusinessDBAName,MerchantAppUID,UID,View_MPSAll,View_Agent,View_Bank">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnNotesID" runat="server" CausesValidation="false" CommandName="View"
                                    Text='<%#Eval("ID") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Notes" HeaderText="Notes">
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Date" DataField="DateCreated" SortExpression="DateCreated"
                            DataFormatString="{0:MM/dd/yy hh:mm tt}">
                            <ItemStyle Width="67px" Wrap="True" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="User Created" DataField="UserCreated" SortExpression="UserCreated">
                            <ItemStyle Width="60px" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" />
                    <PagerStyle CssClass="pgr" />
                    <AlternatingRowStyle CssClass="alt" />
                </asp:GridView>
                <asp:ObjectDataSource ID="odsNotes" runat="server" SelectMethod="GetMerchantNotesPaging"
                    TypeName="DataMerchantAppPaging" OldValuesParameterFormatString="original_{0}"
                    OnSelecting="odsNotes_Selecting" EnablePaging="true" SelectCountMethod="GetMerchantNotesPagingCount"
                    MaximumRowsParameterName="PageSize" StartRowIndexParameterName="CurrentPage">
                    <SelectParameters>
                        <asp:Parameter Name="prms" Type="Object" />
                        <asp:Parameter Name="CurrentPage" Type="Int32" />
                        <asp:Parameter Name="PageSize" Type="int32" />
                        <asp:Parameter Name="ControlID" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="400px" Width="600px"
                    Modal="True" InitialLocation="Centered" WindowState="hidden">
                    <ContentPane>
                        <Template>
                            <table>
                                <tr>
                                    <td class="lblRight">
                                        Subject:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSubject" runat="server" Width="500px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight" valign="top">
                                        Notes:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNotes" runat="server" Height="280px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                               <%-- <td align="left" valign="top">
                                                    <asp:CheckBox ID="chkCallback" Text="Requires Callback" Style="vertical-align: text-top;"
                                                        runat="server" />
                                                </td>--%>
                                                <td align="left" valign="top">
                                                    <asp:CheckBox ID="chkInternal" Text="Access To Internal" Style="vertical-align: text-top;"
                                                        runat="server" />
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:CheckBox ID="chkAgent" Text="Access To Partner" Style="vertical-align: text-top;"
                                                        runat="server" />
                                                </td>
                                              <%--  <td align="left" valign="top">
                                                    <asp:CheckBox ID="chkBank" Text="Access To Bank" Style="vertical-align: text-top;"
                                                        runat="server" />
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </Template>
                    </ContentPane>
                    <Header CaptionText="Notes">
                    </Header>
                </ig:WebDialogWindow>
            </asp:Panel>
            <asp:Panel ID="NoRecords" runat="server">
                &nbsp;No Notes..
            </asp:Panel>
            <asp:Panel ID="pnlAddNotes" runat="server">
                <table cellspacing="2" width="100%">
                    <tr>
                        <td align="left" valign="top">
                            <asp:Label runat="server" ID="lblErr" Text="" SkinID="error"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            Note Code<br />
                            <asp:DropDownList ID="cboNoteCode" runat="server" Width="310px" AutoPostBack="true"
                                OnSelectedIndexChanged="cboNoteCode_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp; &nbsp; &nbsp;
                            <asp:CheckBox ID="chkApplySameLegalName" runat="server" Text="Apply note to same legal business name" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            Notes<br />
                            <asp:TextBox ID="Notes" runat="server" Height="65px" TextMode="MultiLine" Width="310px"></asp:TextBox>
                            <asp:HiddenField ID="Subject" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 90px" class="lblRight">
                                        Callback:
                                    </td>
                                    <td align="left" style="width: 90px">
                                        <asp:CheckBox ID="RequiresCallback" runat="server" Width="30px" />
                                    </td>
                                    <td class="lblRight" style="width: 90px">
                                        Internal:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:CheckBox ID="View_MPSAll" runat="server" Width="30px" />
                                    </td>
                                    <td class="lblRight" style="width: 90px">
                                        Agent:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:CheckBox ID="View_Agent" runat="server" Width="30px" />
                                    </td>
                                    <td class="lblRight" style="width: 90px">
                                        Bank:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:CheckBox ID="View_Bank" runat="server" Width="30px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight" style="width: 121px">
                                        <asp:CheckBox ID="Email_Agent" runat="server" Width="50px" Visible="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnAddNotes" runat="server" Text="Add Notes" CausesValidation="false"
                                OnClick="btnAddNotes_Click" />
                            &nbsp;
                            <asp:LinkButton ID="btnClearNotes" runat="server" Text="Clear Notes" CausesValidation="false"
                                OnClick="btnClearNotes_Click" />&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
<br />
