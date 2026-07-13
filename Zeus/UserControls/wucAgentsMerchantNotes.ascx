<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAgentsMerchantNotes.ascx.cs" Inherits="ZeusWeb.UserControls.wucAgentsMerchantNotes" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script type="text/javascript" src="../js/autoNumeric.js"></script>
<script language="javascript" type="text/javascript">

    function pageLoad(sender, args) {
        $("#<%=ZID.ClientID %>").autoNumeric('init', { vMin: '0', vMax: '99999', aSep: '' });
    }

    function OpenCloseAMN(N1, N2, A1, A2, txt) {
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

    function OpenCloseHeaderAMN(A1, A2, txt, Grid) {
        if (txt == 'Open') {

            document.getElementById(A1).style.display = 'none';
            document.getElementById(A2).style.display = 'inline';

            HideEvenValueRowsAMN('none', Grid);
        }
        else {
            document.getElementById(A1).style.display = 'inline';
            document.getElementById(A2).style.display = 'none';
            HideEvenValueRowsAMN('inline', Grid);
        }
    }

    function HideEvenValueRowsAMN(txt, Grid) {

        var tGrid = document.getElementById(Grid);
        for (var i = 1; i < tGrid.rows.length; ++i) {
            var inputs = tGrid.rows[i].getElementsByTagName("a");
            var labels = tGrid.rows[i].getElementsByTagName("span");

            for (var j = 0; j < inputs.length; ++j) {
                if (inputs[j] != null) {
                    var txt2 = inputs[j].id;
                    if (txt2.indexOf('lnkmore1') != -1)
                        inputs[j].style.display = (txt == 'none') ? 'none' : 'inline';
                    if (txt2.indexOf('lnkless1') != -1)
                        inputs[j].style.display = (txt == 'none') ? 'inline' : 'none';
                }
            }

            for (var j = 0; j < labels.length; ++j) {
                if (labels[j] != null) {
                    var txt1 = labels[j].id;
                    if (txt1.indexOf('NotesMore1') != -1)
                        labels[j].style.display = (txt == 'none') ? 'none' : 'inline';
                    if (txt1.indexOf('NotesLess1') != -1)
                        labels[j].style.display = (txt == 'none') ? 'inline' : 'none';
                }
            }
        }
    }



</script>
<asp:Panel runat="server" ID="panelagentsmerchantnotes" DefaultButton="btnSearch">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <fieldset>
                <legend>Merchant Notes List</legend>
                <table width="100%">
                    <tr>
                        <td class="lblLeft">Merchant ID:
                            <asp:TextBox ID="ZID" runat="server" Width="100px" ClientIDMode="Static"></asp:TextBox>&nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
                    </tr>
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

                            <asp:GridView ID="grdNotes" AllowPaging="true" runat="server" OnRowDataBound="grdNotes_RowDataBound"
                                OnRowCommand="grdNotes_RowCommand" AutoGenerateColumns="False" Font-Names="Verdana"
                                AllowSorting="true" PageSize="10" DataSourceID="odsNotes" Font-Size="X-Small"
                                CssClass="mGrid" OnPageIndexChanging="grdNotes_PageIndexChanging" PagerStyle-CssClass="pgr"
                                AlternatingRowStyle-CssClass="alt" Width="100%" Height="50%" OnSorting="grdNotes_Sorting"
                                DataKeyNames="Subject,Notes,BusinessDBAName,MerchantAppUID,UID,View_MPSAll,View_Agent,View_Bank,View_Merchant" EmptyDataText="No records Found..">
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                <Columns>
                                    <asp:TemplateField HeaderText="NoteID" SortExpression="ID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnNotesID" runat="server" CausesValidation="false" CommandName="View"
                                                Text='<%#Eval("ID") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ticket ID">
                                        <ItemTemplate>
                                            <a class="fakea" onclick="OpenTicket('<%#Eval("TicketUID") %>')">
                                                <%#Eval("TicketID") %></a>
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" />
                                        <ControlStyle Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ZID" SortExpression="ID">
                                        <ItemTemplate>
                                            <asp:HyperLink NavigateUrl='<%#"~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&Adding=false"  %>'
                                                runat="server" ID="hypZID" Text='<%# Eval("ZID") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Width="35px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject">
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Notes">
                                        <HeaderTemplate>
                                            Notes&nbsp;<a id='lnkMore11' runat="server" style="display: inline; font-weight: normal; color: #0a94d6; text-decoration: underline; cursor: pointer;">More</a><a id='lnkless11'
                                                runat="server" style="font-weight: normal; display: none; text-decoration: underline; color: #0a94d6; cursor: pointer;">Less</a>
                                        </HeaderTemplate>
                                        <ItemStyle Width="200px" />
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="NotesMore1"></asp:Label><a id='lnkmore1' runat="server" style="display: none; cursor: pointer;">More</a>
                                            <asp:Label runat="server" ID="NotesLess1" Style="display: none;"></asp:Label>
                                            <a id='lnkless1' runat="server" style="display: none; cursor: pointer;">Less</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderText="Date Created" DataField="DateCreated" SortExpression="DateCreated"
                                        DataFormatString="{0:MM/dd/yy hh:mm tt}">
                                        <ItemStyle Width="60px" Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="User Created" DataField="UserCreated" SortExpression="UserCreated">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BusinessDBAName" Visible="False" />
                                    <asp:BoundField DataField="MerchantAppUID" Visible="False" />
                                </Columns>
                                <HeaderStyle HorizontalAlign="Center" />
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <asp:ObjectDataSource ID="odsNotes" runat="server" SelectMethod="GetMerchantNotesPaging"
                    TypeName="DataMerchantAppPaging" OldValuesParameterFormatString="original_{0}"
                    OnSelecting="odsNotes_Selecting" EnablePaging="true" MaximumRowsParameterName="PageSize"
                    StartRowIndexParameterName="CurrentPage" SelectCountMethod="GetMerchantNotesPagingCount">
                    <SelectParameters>
                        <asp:Parameter Name="prms" Type="Object" />
                        <asp:Parameter Name="CurrentPage" Type="Int32" />
                        <asp:Parameter Name="PageSize" Type="int32" />
                        <asp:Parameter Name="ControlID" Type="string" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="400px" Width="650px"
                    Modal="True" InitialLocation="Centered" WindowState="hidden">
                    <ContentPane>
                        <Template>
                            <table>
                                <tr>
                                    <td class="lblRight">Subject: </td>
                                    <td>
                                        <asp:TextBox ID="txtSubject" runat="server" Width="500px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="lblRight" valign="top">Notes: </td>
                                    <td style="font-size: small">
                                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Wrap="true" Height="300px" Width="550px" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px">
                                            <asp:Label ID="txtNotes" runat="server" TextMode="MultiLine" Height="280px" BackColor="White"></asp:Label>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlNotesCBRowInWindow">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <asp:CheckBox ID="chkInternal" runat="server" Enabled="false" Style="vertical-align: text-top;" Text="Access To Internal" /></td>
                                                    <td align="left" valign="top">
                                                        <asp:CheckBox ID="chkAgent" runat="server" Enabled="false" Style="vertical-align: text-top;" Text="Access To Partner" /></td>
                                                    <td align="left" valign="top">
                                                        <asp:CheckBox ID="chkMerchant" runat="server" Enabled="false" Style="vertical-align: text-top;" Text="Access To Merchant" /></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </Template>
                    </ContentPane>
                    <Header CaptionText="Notes"></Header>
                </ig:WebDialogWindow>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>


