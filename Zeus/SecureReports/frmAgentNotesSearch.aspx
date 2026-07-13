<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="frmAgentNotesSearch" Title="Agent Notes Search" Codebehind="frmAgentNotesSearch.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        var IsPostBack = '<%=IsPostBack.ToString() %>';
        window.onload = function () {
            var strCook = document.cookie;
            if (strCook.indexOf("!~") != 0) {
                var intS = strCook.indexOf("!~");
                var intE = strCook.indexOf("~!");
                var strPos = strCook.substring(intS + 2, intE);
                if (IsPostBack == 'True') {
                    if (document.getElementById("<%=pnlGrid.ClientID %>") != null) {
                        document.getElementById("<%=pnlGrid.ClientID %>").scrollTop = strPos;
                    }
                }
                else {
                    document.cookie = "yPos=!~0~!";
                }
            }
        }
        function SetDivPosition() {
            var intY = 0;

            if (document.getElementById("<%=pnlGrid.ClientID %>") != null) {
                intY = document.getElementById("<%=pnlGrid.ClientID %>").scrollTop;
            }

            document.cookie = "yPos=!~" + intY + "~!";
        }


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
            var tGrid = document.getElementById('<%= grd1.ClientID%>');
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

    <table width="100%">
        <tr>
            <td>
                <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                    <fieldset>
                        <legend>Agent Notes Search</legend>
                        <table cellspacing="2" width="100%">
                            <tr>
                                <td class="lblRight">
                                    Begin Date:</td>
                                <td>
                                    <ig:WebDatePicker ID="SearchBeginDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                                        BorderWidth="1px" EnableAppStyling="False" NullDateLabel="" Width="155px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                </td>
                                <td class="lblRight">
                                    End Date:</td>
                                <td>
                                    <ig:WebDatePicker ID="SearchEndDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                                        BorderWidth="1px" EnableAppStyling="False" NullDateLabel="" Width="155px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                </td>
                                <td class="lblRight">
                                    ID:</td>
                                <td>
                                    <asp:TextBox ID="AgentUID" runat="server" EnableViewState="False" Width="150px"></asp:TextBox></td>
                                <td class="lblRight">
                                    DBA:</td>
                                <td>
                                    <asp:TextBox ID="AgentDBA" runat="server" EnableViewState="False" Width="150px"></asp:TextBox></td>
                                <td class="lblRight">
                                    Status:</td>
                                <td>
                                    <asp:DropDownList ID="StatusUID" runat="server" Width="155px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td class="lblRight">
                                    First Name:</td>
                                <td>
                                    <asp:TextBox ID="AgentFirstName" runat="server" EnableViewState="False" Width="150px"></asp:TextBox></td>
                                <td class="lblRight">
                                    Last Name:</td>
                                <td>
                                    <asp:TextBox ID="AgentLastName" runat="server" Width="150px" EnableViewState="False"></asp:TextBox></td>
                                <td class="lblRight">
                                    Note Codes:</td>
                                <td>
                                    <asp:DropDownList ID="NoteCode" runat="server" Width="155px">
                                    </asp:DropDownList>
                                </td>                                
                                <td class="lblRight">
                                    Phone:</td>
                                <td>
                                    <asp:TextBox ID="AgentPhone" runat="server" Width="150px" EnableViewState="False"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="lblRight" valign="top">
                                    Notes:</td>
                                <td colspan="3">
                                    <asp:TextBox ID="Notes" runat="server" Width="510px" TextMode="MultiLine" Height="40px" Rows="3" EnableViewState="False"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <div style="text-align: center;">
                            <br />
                            <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                CausesValidation="False" AccessKey="h">
                                <Appearance>
                                    <Image Url="~/Images/Check.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                            &nbsp;
                            <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                                CausesValidation="False" AccessKey="l">
                                <Appearance>
                                    <Image Url="~/Images/delete.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </div>
                    </fieldset>
                </asp:Panel>
                <br />
                <fieldset>
                    <legend>
                        <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></legend>
                    <asp:Panel runat="server" ID="pnl1">
                        <div style="height: 5px">
                        </div>
                        <div class="buckethdright">
                            <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label>&nbsp;
                        </div>
                        <asp:Panel ID="pnlGrid" runat="server" Height="400px" Width="100%" ScrollBars="vertical"
                            onscroll="SetDivPosition()">
                            <asp:GridView ID="grd1" runat="server" OnRowCommand="grd_RowCommand" OnRowDataBound="grd_RowDataBound"
                                CssClass="mGrid" DataKeyNames="AgentUID,UID,Notes,NoteCode" AutoGenerateColumns="false" Width="99.8%">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink NavigateUrl='<%#  "~/SecureAgentManagementForms/frmAgentNotes.aspx?AgentUID=" + Eval("AgentUID") + "&Adding=false"  %>'
                                                            runat="server" ID="hypView" Text='View'></asp:HyperLink>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Note ID" ItemStyle-Width="40px" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnNotesID" runat="server" CausesValidation="false" CommandName="View" Text='<%#Eval("ID") %>'
                                               CommandArgument='<%#Eval("ID") %>'  ></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Dba" HeaderText="DBA"></asp:BoundField>
                                    <asp:BoundField DataField="NoteCode" HeaderText="Note Code"></asp:BoundField>
                                    <%--<asp:BoundField DataField="Notes" HeaderText="Notes"></asp:BoundField>--%>
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
                                    <asp:BoundField DataField="DateCreated" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UserCreated" HeaderText="User Created"></asp:BoundField>
                                    <asp:BoundField DataField="AgentUID" Visible="false" HeaderText="AgentUID"></asp:BoundField>
                                    <asp:BoundField DataField="UID" Visible="false" HeaderText="UID"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <div class="bucketfooter">
                            <div class="bucketfooterleft">
                                <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnExport_Click">
                                    <span style="height: 25px; vertical-align: middle;">
                                        <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                            Excel</span></asp:LinkButton>&nbsp;&nbsp;
                               <%-- <asp:LinkButton ID="btnPDF" runat="server" OnClick="btnExportPDF_Click">
                                    <span style="height: 25px; vertical-align: middle;">
                                        <asp:Image ID="Image2" runat="server" SkinID="SavePDF" /></span><span style="margin-left: 5px;">Save
                                            PDF</span></asp:LinkButton>--%>
                            </div>
                        </div>
                        <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="400px" Width="600px"
                        Modal="True" InitialLocation="Centered" WindowState="Hidden">
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
                            </Template>
                        </ContentPane>
                        <Header CaptionText="Notes">
                        </Header>
                    </ig:WebDialogWindow>
                    </asp:Panel>
                    <asp:Label runat="server" ID="NoData" Text="NoData..." Visible="false"></asp:Label>
                </fieldset>
            </td>
        </tr>
    </table>
</asp:Content>
