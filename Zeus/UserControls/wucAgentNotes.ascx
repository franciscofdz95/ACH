<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucAgentNotes" CodeBehind="wucAgentNotes.ascx.cs" %>
<%@ Register Src="wucMessage.ascx" TagName="wucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<script language="javascript" type="text/javascript">

    function OpenClose(N1, N2, A1, A2, txt) {
        if (txt == 'Open') {
            document.getElementById(N1).style.display = 'none';
            document.getElementById(A1).style.display = 'none';
            document.getElementById(N2).style.display = 'inline';
            document.getElementById(A2).style.display = 'inline';
        }
        else {
            document.getElementById(N1).style.display = 'inline';
            document.getElementById(A1).style.display = 'inline';
            document.getElementById(N2).style.display = 'none';
            document.getElementById(A2).style.display = 'none';
        }
    }

    function OpenCloseHeader(A1, A2, txt, Grid) {
        if (txt == 'Open') {
            document.getElementById(A1).style.display = 'none';
            document.getElementById(A2).style.display = 'inline';
            HideEvenValueRows('none', Grid);
        }
        else {
            document.getElementById(A1).style.display = 'inline';
            document.getElementById(A2).style.display = 'none';
            HideEvenValueRows('inline', Grid);
        }
    }

    function HideEvenValueRows(txt, Grid) {
        var tGrid = document.getElementById(Grid);
        for (var i = 1; i < tGrid.rows.length; ++i) {
            var inputs = tGrid.rows[i].getElementsByTagName("a");
            var labels = tGrid.rows[i].getElementsByTagName("span");

            for (var j = 0; j < inputs.length; ++j) {
                if (inputs[j] != null) {
                    var txt2 = inputs[j].id;
                    if (txt2.indexOf('lnkmore') != -1)
                        inputs[j].style.display = (txt == 'none') ? 'none' : 'inline';
                    if (txt2.indexOf('lnkless') != -1)
                        inputs[j].style.display = (txt == 'none') ? 'inline' : 'none';
                }
            }

            for (var j = 0; j < labels.length; ++j) {
                if (labels[j] != null) {
                    var txt1 = labels[j].id;
                    if (txt1.indexOf('NotesMore') != -1)
                        labels[j].style.display = (txt == 'none') ? 'none' : 'inline';
                    if (txt1.indexOf('NotesLess') != -1)
                        labels[j].style.display = (txt == 'none') ? 'inline' : 'none';
                }
            }
        }
    }

</script>
<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <contenttemplate>
        <fieldset>
            <legend>Notes List</legend>

            <asp:Panel runat="server" ID="pnlDetails">
                <table width="100%">
                    <tr>
                        <td class="lblLeft">Page Size:
                                    <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem Selected="True">10</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>100</asp:ListItem>
                                        <asp:ListItem>250</asp:ListItem>
                                        <asp:ListItem>500</asp:ListItem>
                                    </asp:DropDownList>
                        </td>
                        <td class="lblRight">
                            <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grdAgentNotes" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                                Font-Names="Verdana" Font-Size="X-Small" OnRowDataBound="grdNotes_RowDataBound" DataSourceID="odsTransactions"
                                AllowSorting="true" OnSorting="grdNotes_Sorting" OnRowCommand="grdNotes_RowCommand" PageSize="5"
                                DataKeyNames="UID,Notes,NoteCode,ID,View_Agent,View_Merchant" AllowPaging="true" OnPageIndexChanging="grdNotes_PageIndexChanging">
                                <PagerStyle CssClass="pgr" />
                                <FooterStyle CssClass="footer" />
                                <AlternatingRowStyle CssClass="alt" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                    LastPageText="&raquo;" />
                                <Columns>
                                    <asp:BoundField Visible="False" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="center" SortExpression="ID" HeaderText="Note ID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnNotesID" runat="server" CausesValidation="false" CommandName="View"
                                                Text='<%#Eval("ID") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ticket ID" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <a class="fakea" onclick="OpenTicket('<%#Eval("TicketUID") %>')">
                                                <%#Eval("TicketID") %></a>
                                        </ItemTemplate>
                                        <ItemStyle Width="60px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NoteCode" HeaderText="Note Code" ItemStyle-Width="320px"
                                        SortExpression="NoteCode"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Notes">
                                        <HeaderTemplate>
                                            Notes&nbsp;<a id='lnkMore' runat="server" style="display: inline; font-weight: normal; color: #0a94d6; text-decoration: underline; cursor: pointer;">More</a><a id='lnkless'
                                                runat="server" style="font-weight: normal; display: none; text-decoration: underline; color: #0a94d6; cursor: pointer;">Less</a>
                                        </HeaderTemplate>
                                        <ItemStyle Width="200px" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="NotesMore"></asp:Label><a id='lnkmore' runat="server" style="display: none; cursor: pointer;">More</a>
                                            <asp:Label runat="server" ID="NotesLess" Style="display: none;"></asp:Label>
                                            <a id='lnkless' runat="server" style="display: none; cursor: pointer;">Less</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="CreatedOn" DataFormatString="{0:MM/dd/yyyy hh:mm tt}"
                                        HeaderText="Date Created" SortExpression="CreatedOn"></asp:BoundField>
                                    <asp:BoundField DataField="CreatedBy" HeaderText="User Created" SortExpression="CreatedBy"></asp:BoundField>
                                    <asp:BoundField DataField="UID" Visible="false" HeaderText="UID"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsTransactions" runat="server" OnSelecting="odsTransactions_Selecting"
                                EnablePaging="True" MaximumRowsParameterName="PageSize" StartRowIndexParameterName="CurrentPage"
                                TypeName="DataMerchantAppPaging" SelectMethod="GetAgentNotesPaging" SelectCountMethod="GetAgentNotesPagingRowCount">
                                <SelectParameters>
                                    <asp:Parameter Name="prms" Type="Object" />
                                    <asp:Parameter Name="PageSize" Type="Int32" />
                                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                                    <asp:Parameter Name="ControlID" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:Panel runat="server" Style="text-align: right;" Visible="false" ID="pnlMore">
                                <asp:HyperLink runat="server" ID="hypMore">More</asp:HyperLink>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label runat="server" ID="lblNotes" Visible="false" Text="no data..."></asp:Label>
        </fieldset>
        <fieldset>
            <legend>Add Notes</legend>
            <div>
                <uc1:wucmessage id="WucMessage1" runat="server" />
                <br />
                Note Codes:
                        <asp:DropDownList ID="cboNoteCode" runat="server">
                        </asp:DropDownList>
            </div>
            <div>
                <asp:TextBox ID="Notes" runat="server" Height="65px" TextMode="MultiLine" Width="95%"></asp:TextBox>
                <br />
                <asp:Button ID="btnAddNotes" runat="server" OnClick="btnAddNotes_Click1" Text="Add Notes" />&nbsp;<asp:Button
                    ID="btnClearNotes" runat="server" OnClick="btnClearNotes_Click1" Text="Clear Notes"
                    CausesValidation="false" />
            </div>
        </fieldset>
           <ig:webdialogwindow id="WebDialogWindow2" runat="server" height="400px" width="600px"
            modal="True" initiallocation="Centered" windowstate="Hidden">
                        <ContentPane>
                            <Template>
                                <table>
                                    <tr>
                                        <td class="lblRight">
                                            Note Code:
                                                                  </td>
                                        <td>
                                            <asp:TextBox ID="Subject" runat="server" Width="500px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight" valign="top">
                                            Notes:
                                                                  </td>
                                        <td>
                                            <asp:TextBox ID="txtNotes" runat="server" Height="300px" TextMode="MultiLine" Width="500px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table width="100%">
                        <tr>
                            <td align ="Center" valign="top">
                                <asp:CheckBox ID="chkAgent" runat="server" Enabled="false" Style="vertical-align: text-top;" Text="Access To Partner" /></td>
                            <td align="Center" valign="top">
                                <asp:CheckBox ID="chkMerchant" runat="server" Enabled="false" Style="vertical-align: text-top;" Text="Access To Merchant" /></td>
                        </tr>
                    </table
                            </Template>
                        </ContentPane>
                        <Header CaptionText="Notes">
                        </Header>
                    </ig:webdialogwindow>
    </contenttemplate>
</asp:UpdatePanel>
