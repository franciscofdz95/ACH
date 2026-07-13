<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true"
    Inherits="frmMerchantNotesSearch" Title="Merchant Notes Search" CodeBehind="frmMerchantNotesSearch.aspx.cs" %>

<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/wucEmail.ascx" TagName="wucEmail" TagPrefix="uc4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
            var tGrid = document.getElementById('<%= grd.ClientID%>');
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
                <asp:Panel ID="Panel1" runat="server" Height="" Width="">
                    <fieldset>
                        <legend>Merchant Notes Search</legend>
                        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                            <table cellspacing="2" width="100%">
                                <tr>
                                    <td class="lblRight">Begin Date:
                                    </td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                            Width="125px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                        </ig:WebDatePicker>
                                    </td>
                                    <td class="lblRight">End Date:
                                    </td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                            Width="125px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                        </ig:WebDatePicker>
                                    </td>
                                    <td class="lblRight">DBA:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessDBAName" runat="server" EnableViewState="False" Width="130px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Legal Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessLegalName" runat="server" Width="130px" EnableViewState="False"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">MID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SettlePlatformMid" runat="server" EnableViewState="False" Width="130px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Bank:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="125px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">Status:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="StatusUID" runat="server" Width="125px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">User Created:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="UserUID" runat="server" Width="135px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">ZID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="MerchantID" runat="server" EnableViewState="False" Width="130px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Note Codes:
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="NoteCodes" runat="server" Width="445px">
                                        </asp:DropDownList>
                                        <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="NoteCodes"
                                            PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                                            IsSorted="true" QueryPattern="Contains">
                                        </cc1:ListSearchExtender>
                                    </td>
                                    <td colspan="4">
                                        <asp:Panel runat="server" ID="AgentSelect">
                                            <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="horizontal"
                                                IDWidth="90" DBAWidth="130" lblDBAWidth="95" lblIDWidth="139" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight" valign="top">Note:
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Notes" runat="server" Width="440px" TextMode="MultiLine" Height="40px" Rows="3" EnableViewState="False"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center; text-align: -moz-center;">
                                <br />
                                <table align="center" cellspacing="5px">
                                    <tr>
                                        <td>
                                            <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                                AccessKey="h">
                                                <Appearance>
                                                    <Image Url="~/Images/Check.png" />
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                        <td>
                                            <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                                                AccessKey="l">
                                                <Appearance>
                                                    <Image Url="~/Images/delete.png" />
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </fieldset>
                    <br />
                    <fieldset>
                        <legend>
                            <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></legend>
                        <asp:Panel runat="server" ID="pnl1" Width="100%">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label3" runat="server" Text="Page Size: "></asp:Label>
                                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                            <asp:ListItem Selected="True">10</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                            <asp:ListItem>500</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text=""></asp:Label>&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <%-- <asp:Panel ID="pnlGrid" runat="server" Height="400px" Width="100%" ScrollBars="vertical"
                                            onscroll="SetDivPosition()">--%>
                                        <asp:GridView ID="grd" runat="server" OnRowCommand="grd_RowCommand" OnRowDataBound="grd_RowDataBound" AllowPaging="true"
                                            CssClass="mGrid" AutoGenerateColumns="false" DataSourceID="odsNotes"
                                            Width="99%" OnPageIndexChanging="grd_PageIndexChanging" DataKeyNames="Subject,Notes,BusinessDBAName,MerchantAppUID,UID,ACHID,View_MPSAll,View_Agent,View_Bank">
                                            <FooterStyle CssClass="footer" />
                                            <PagerStyle CssClass="pgr" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantNotes.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&Adding=false"  %>'
                                                            runat="server" ID="hypZID" Text="View"></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Left" Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="NoteID">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnNotesID" runat="server" CausesValidation="false" CommandName="View"
                                                            Text='<%#Eval("ID") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                    <ControlStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="TicketID">
                                                    <ItemTemplate>
                                                        <a class="fakea" onclick="OpenTicket('<%#Eval("TicketUID") %>')">
                                                            <%#Eval("TicketID") %></a>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                    <ControlStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA" ItemStyle-Width="120px"></asp:BoundField>
                                                <asp:BoundField DataField="Subject" HeaderText="Subject" ItemStyle-Width="120px"></asp:BoundField>
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
                                                <asp:BoundField DataField="AgentDBA" HeaderText="Agent" ItemStyle-Width="120px" />
                                                <asp:BoundField DataField="DateCreated" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date Created"
                                                    ItemStyle-Width="70px"></asp:BoundField>
                                                <asp:BoundField DataField="UserCreated" HeaderText="User Created" ItemStyle-Width="70px"></asp:BoundField>
                                                <asp:BoundField DataField="MerchantAppUID" Visible="false"></asp:BoundField>
                                                <asp:BoundField DataField="AchID" Visible="false" />
                                                <asp:BoundField DataField="UID" Visible="false"></asp:BoundField>
                                            </Columns>
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
                                        <%-- </asp:Panel>--%>
                                        <br />
                                        <div class="bucketfooter">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" style="width: 33%;">
                                                        <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnExport_Click">
                                                            <span style="height: 25px; vertical-align: middle;">
                                                                <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span>
                                                        </asp:LinkButton>&nbsp;&nbsp;
                                                       <%-- <asp:LinkButton ID="btnPDF" runat="server" OnClick="btnExportPDF_Click">
                                                            <span style="height: 25px; vertical-align: middle;">
                                                                <asp:Image ID="Image2" runat="server" SkinID="SavePDF" /></span><span style="margin-left: 5px;">Save
                                                                    PDF</span>
                                                        </asp:LinkButton>--%>
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
                                                                <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Wrap="true" Height="300px" Width="550px" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px">
                                                                    <asp:Label ID="txtNotes" runat="server" TextMode="MultiLine" Height="280px" BackColor="White"></asp:Label></asp:Panel>
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
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Label runat="server" ID="NoData" Text=" No Data..." Visible="false"></asp:Label>
                    </fieldset>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <%--<script language="javascript" type="text/javascript">
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
    </script>--%>
</asp:Content>
