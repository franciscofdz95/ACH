<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucNotes" CodeBehind="wucNotes.ascx.cs" %>
<%@ Register Src="wucEmail.ascx" TagName="wucEmail" TagPrefix="uc4" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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

    function OpenCloseHeader(A1, A2, txt) {
        if (txt == 'Open') {
            document.getElementById(A1).style.display = 'none';
            document.getElementById(A2).style.display = 'inline';
            HideEvenValueRows('none');
        }
        else {
            document.getElementById(A1).style.display = 'inline';
            document.getElementById(A2).style.display = 'none';
            HideEvenValueRows('inline');
        }
    }

    function HideEvenValueRows(txt) {
        var tGrid = document.getElementById('<%= grdNotes.ClientID%>');
        for (var i = 1; i < tGrid.rows.length; ++i) {
            var inputs = tGrid.rows[i].getElementsByTagName("a");
            var labels = tGrid.rows[i].getElementsByTagName("span");

            for (var j = 0; j < inputs.length; ++j) {
                if (inputs[j] != null) {
                    var txt2 = inputs[j].id;
                    if (txt2.indexOf('lnk1') != -1)
                        inputs[j].style.display = (txt == 'none') ? 'none' : 'inline';
                    if (txt2.indexOf('lnk2') != -1)
                        inputs[j].style.display = (txt == 'none') ? 'inline' : 'none';
                }
            }

            for (var j = 0; j < labels.length; ++j) {
                if (labels[j] != null) {
                    var txt1 = labels[j].id;
                    if (txt1.indexOf('Notes1') != -1)
                        labels[j].style.display = (txt == 'none') ? 'none' : 'inline';
                    if (txt1.indexOf('Notes2') != -1)
                        labels[j].style.display = (txt == 'none') ? 'inline' : 'none';
                }
            }
        }
    }



</script>
<asp:Panel ID="UpdatePanel1" runat="server">
    <%-- <contenttemplate>--%>


    <div class="title">
        &nbsp;&nbsp;Notes List
        <hr class="line" />
    </div>
    <div class="indentedcontent20">
        <br />
       <asp:Label ID="Label1" runat="server" Text="Filter by Department: " Width="130px"
            Style="text-align: right;"></asp:Label>
        <asp:DropDownList ID="ddpNoteType" runat="server" AutoPostBack="True" Width="150px"
            OnSelectedIndexChanged="ddpNoteType_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Subject/Notes: " Width="130px" Style="text-align: right;"></asp:Label>
        <asp:TextBox ID="Search" runat="server" Width="145px"></asp:TextBox>
        &nbsp;
        <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
        &nbsp;<br />
        <asp:Panel runat="server" ID="pnlRecords">
            <asp:Label ID="Label3" runat="server" Text="Page Size: " Width="130px" Style="text-align: right;"></asp:Label>
            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                <asp:ListItem Selected="True">5</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>100</asp:ListItem>
                <asp:ListItem>250</asp:ListItem>
                <asp:ListItem>500</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            <div class="buckethdright">
                <asp:Label runat="server" ID="recordCnt" Width="90%"></asp:Label>&nbsp;
                <br />
            </div>
            <div style="clear:both;"></div>
            <asp:GridView ID="grdNotes" AllowPaging="true" runat="server" OnRowDataBound="grdNotes_RowDataBound"
                OnRowCommand="grdNotes_RowCommand" AutoGenerateColumns="False" Font-Names="Verdana"
                AllowSorting="true" PageSize="10" DataSourceID="odsNotes" Font-Size="X-Small"
                CssClass="mGrid" OnPageIndexChanging="grdNotes_PageIndexChanging" PagerStyle-CssClass="pgr"
                AlternatingRowStyle-CssClass="alt" Width="100%" Height="50%" OnSorting="grdNotes_Sorting"
                DataKeyNames="Subject,Notes,BusinessDBAName,MerchantAppUID,UID,View_MPSAll,View_Agent,View_Bank,View_Merchant">
                <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                <Columns>
                    <asp:TemplateField HeaderText="NoteID">
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
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEmail" runat="server" CausesValidation="false" CommandName="Email"
                                Text="Email"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="30px" />
                        <ControlStyle Width="30px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject">
                        <ItemStyle Width="200px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Notes">
                        <HeaderTemplate>
                            Notes&nbsp;<a id='lnk1' runat="server" style="display: inline; font-weight: normal; color: #0a94d6; text-decoration: underline; cursor: pointer;">More</a><a id='lnk2'
                                runat="server" style="font-weight: normal; display: none; text-decoration: underline; color: #0a94d6; cursor: pointer;">Less</a>
                        </HeaderTemplate>
                        <ItemStyle Width="200px" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="Notes1"></asp:Label><a id='lnk1' runat="server" style="display: none; cursor: pointer;">More</a>
                            <asp:Label runat="server" ID="Notes2" Style="display: none;"></asp:Label>
                            <a id='lnk2' runat="server" style="display: none; cursor: pointer;">Less</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Callback?" Visible="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkCallback" runat="server" AutoPostBack="true" OnCheckedChanged="chkCallback_CheckedChanged" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="70px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="IsPrivate" Visible="False">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIsPrivate" runat="server" AutoPostBack="true" OnCheckedChanged="chkCallback_CheckedChanged" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" Width="70px" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="UID" DataField="UID" Visible="False"></asp:BoundField>
                    <asp:BoundField HeaderText="RecordMode" DataField="RecordMode" Visible="False"></asp:BoundField>
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
            <asp:HiddenField ID="hdnAgent" runat="server" />
            <asp:HiddenField ID="hdnMerchant" runat="server" />
            <div class="bucketfooter">
                <table width="100%">
                    <tr>
                        <td align="left" style="width: 33%;">
                            <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click" CommandArgument="grdNotes">
                                <span style="height: 25px; vertical-align: middle;">
                                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                        Excel</span>
                            </asp:LinkButton>&nbsp;&nbsp;
                        </td>
                        <td align="right">Export:&nbsp;
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                <asp:ListItem Value="1">All Pages</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td align="right" style="width: 33%;"></td>
                    </tr>
                </table>
            </div>
            <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="650px" Width="800px"
                Modal="True" InitialLocation="Centered" WindowState="Hidden"><ContentPane><Template><uc4:wucEmail ID="WucEmail1" runat="server" /></Template></ContentPane><Header CaptionText="Email"></Header></ig:WebDialogWindow>
            <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="400px" Width="650px"
                Modal="True" InitialLocation="Centered" WindowState="hidden"><ContentPane><Template><table><tr><td class="lblRight">Subject: </td><td><asp:TextBox ID="txtSubject" runat="server" Width="500px"></asp:TextBox></td></tr><tr><td class="lblRight" valign="top">Notes: </td><td style="font-size:small"><asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Wrap="true" Height="300px" Width="550px" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"><asp:Label ID="txtNotes" runat="server" TextMode="MultiLine" Height="280px" BackColor="White"></asp:Label></asp:Panel></td></tr><tr><td></td><td><asp:Panel runat="server" ID="pnlNotesCBRowInWindow"><table width="100%"><tr><td align="left" valign="top"><asp:CheckBox ID="chkInternal" runat="server" Enabled="false" Style="vertical-align: text-top;" Text="Access To Internal" /></td><td align="left" valign="top"><asp:CheckBox ID="chkAgent" runat="server" Enabled="false" Style="vertical-align: text-top;" Text="Access To Partner" /></td><td align="left" valign="top"><asp:CheckBox ID="chkMerchant" runat="server" Enabled="false" Style="vertical-align: text-top;" Text="Access To Merchant" /></td></tr></table></asp:Panel></td></tr></table></Template></ContentPane><Header CaptionText="Notes"></Header></ig:WebDialogWindow>
        </asp:Panel>
        <asp:Panel ID="NoRecords" runat="server">
            &nbsp;No Notes..
        </asp:Panel>
    </div>

    <%--  </contenttemplate>
      <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnAddNotes" EventName="Click" />
    </Triggers>--%>
</asp:Panel>
<%--<asp:Panel ID="pnlView" runat="server">
    <ContentTemplate>--%>
<asp:Panel ID="pnlAddNotes" runat="server">
    <br />
    <div class="title">
        &nbsp;&nbsp;Add Notes
        <hr class="line" />
    </div>
    <div class="indentedcontent20">
        <table cellspacing="2" width="100%">
            <tr>
                <td align="left" valign="top" colspan="2">
                    <asp:Label runat="server" ID="lblErr" Text="" SkinID="error"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" style="width: 121px" class="lblRight">Note Code:
                </td>
                <td align="left" valign="top">
                    <asp:DropDownList ID="cboNoteCode" runat="server" Width="400px" AutoPostBack="true"
                        OnSelectedIndexChanged="cboNoteCode_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp; &nbsp; &nbsp;
                    <asp:CheckBox ID="chkApplySameLegalName" runat="server" Text="Apply note to shared MLE accounts" />
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" style="width: 121px" class="lblRight">Subject:
                </td>
                <td align="left" valign="top">
                    <asp:TextBox ID="Subject" runat="server" Width="98%" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" style="width: 121px" class="lblRight">Notes:
                </td>
                <td align="left" valign="top">
                    <asp:TextBox ID="Notes" runat="server" Height="65px" TextMode="MultiLine" Width="98%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Panel runat="server" ID="pnlNotesCBRow">
                        <table>
                            <tr>
                                <%-- <td style="width: 121px" class="lblRight">
                                    Requires Callback:
                                </td>
                                <td align="left" style="width: 121px">
                                    <asp:CheckBox ID="RequiresCallback" runat="server" Width="50px" />
                                </td>--%>
                                <td class="lblRight" style="width: 121px">Access To Internal:
                                </td>
                                <td align="left" valign="top">
                                    <asp:CheckBox ID="View_MPSAll" runat="server" Width="50px" />
                                </td>
                                <td class="lblRight" style="width: 121px">Access To Agent:
                                </td>
                                <td align="left" valign="top">
                                    <asp:CheckBox ID="View_Agent" runat="server" Width="50px" />
                                </td>
                                <%--<td class="lblRight" style="width: 121px">
                                    Access To Bank:
                                </td>
                                <td align="left" valign="top">
                                    <asp:CheckBox ID="View_Bank" runat="server" Width="50px" />
                                </td>--%>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td style="width: 121px" class="lblRight"></td>
                            <td align="left" valign="top">
                                <asp:CheckBox ID="IsPrivate" runat="server" Checked="True" Width="50px" Visible="False" />
                            </td>
                            <td class="lblRight" style="width: 121px">
                                <asp:CheckBox ID="Email_Agent" runat="server" Width="50px" Visible="False" />
                            </td>
                            <td align="left" valign="top"></td>
                            <td class="lblRight" style="width: 121px"></td>
                            <td align="left" valign="top"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 121px">&nbsp;
                </td>
                <td>
                    <asp:Button ID="btnAddNotes" runat="server" Text="Add Notes" CausesValidation="false"
                        OnClick="btnAddNotes_Click" />
                    &nbsp;
                    <asp:Button ID="btnClearNotes" runat="server" Text="Clear Notes" CausesValidation="false"
                        OnClick="btnClearNotes_Click" />&nbsp;
                </td>
            </tr>
            <tr>
                <td align="left" valign="top" style="width: 121px"></td>
                <td align="left" valign="top">
                    <asp:CheckBox ID="Complaint" runat="server" Text="Complaint" Visible="False" />&nbsp;<asp:CheckBox
                        ID="RepeatIssue" runat="server" Text="Repeat Issue" Visible="False" />&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
<%--    </ContentTemplate>
</asp:Panel>--%>
<script type="text/javascript">

    function getValue(thing, viewA) {
        var notesSubject = document.getElementById('<% =Subject.ClientID %>');
        notesSubject.value = thing.options[thing.selectedIndex].innerHTML + ': ';
    }

    function CloseEmail() {
        clearFields();
        oWebDialogWindow1 = $find('<% =WebDialogWindow1.ClientID %>'); oWebDialogWindow1.set_windowState($IG.DialogWindowState.Hidden);
    }

</script>
