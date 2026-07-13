<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucDowngrade" Codebehind="wucDowngrade.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<table width="100%">
    <tr>
        <td>
            <div class="title">
                &nbsp;&nbsp;Downgrade Summary Report
                <hr class="line" />
            </div>
            <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                <div style="width: 100%">
                    <table>
                        <tr>
                            <td class="lblRight">
                                <asp:Label Text="From Date:" ID="lbl" runat="server" Width="70px"></asp:Label></td>
                            <td align="left">
                                <ig:WebDatePicker ID="SearchBeginDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                                    BorderWidth="1px" EnableAppStyling="False" NullDateLabel="" Width="150px">
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">
                                End Date:</td>
                            <td align="left">
                                <ig:WebDatePicker ID="SearchEndDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                                    BorderWidth="1px" EnableAppStyling="False" NullDateLabel="" Width="150px">
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">
                                MID:</td>
                            <td>
                                <asp:TextBox ID="MID" runat="server" Width="145px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="lblRight">
                                ZID:</td>
                            <td>
                                <asp:TextBox ID="ZID" runat="server" Width="145px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <%--<td class="lblRight">
                                Agent:</td>
                            <td align="left">
                                <asp:DropDownList ID="AgentUID" runat="server" Width="300px">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="AgentUID"
                                    PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                                    IsSorted="true" QueryPattern="Contains">
                                </cc1:ListSearchExtender>
                            </td>--%>
                            <td colspan="2">
                                <asp:Panel runat="server" ID="AgentSelect">
                                    <uc1:agentselector runat="server" id="wucAgentSelector" layoutstyle="vertical" idwidth="110"
                                        dbawidth="145" lbldbawidth="103" lblidwidth="103" />
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">
                                Include Sub-Agents:</td>
                            <td align="left">
                                <asp:CheckBox runat="server" ID="chkSubAgent" /></td>
                        </tr>
                    </table>
                </div>
                <div style="text-align: center;">
                    <br />
                    <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                        AccessKey="h">
                        <Appearance>
                            <Image Url="~/Images/Check.png" />
                        </Appearance>
                    </igtxt:WebImageButton>
                    &nbsp;
                    <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                        AccessKey="l">
                        <Appearance>
                            <Image Url="~/Images/delete.png" />
                        </Appearance>
                    </igtxt:WebImageButton>
                    &nbsp;&nbsp;
                </div>
            </asp:Panel>
            <br />
            <div class="title">
                &nbsp;&nbsp;Search Results
                <hr class="line" />
            </div>
            <asp:Label runat="server" ID="lblData" Text="No Data" Visible="true"></asp:Label>
            <asp:Panel runat="server" ID="pnl1" Visible="false">
                <table width="100%">
                    <tr>
                        <td class="lblLeft">
                            Page Size:
                            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                <asp:ListItem Selected="True">10</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                                <asp:ListItem>250</asp:ListItem>
                                <asp:ListItem>500</asp:ListItem>
                            </asp:DropDownList></td>
                        <td class="lblRight">
                            <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView runat="server" ID="grdDownGrade" AutoGenerateColumns="false" CssClass="mGrid"
                                OnSorting="grd_Sorting" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                                OnRowDataBound="grdDownGrade_RowDataBound" AllowSorting="true">
                                <RowStyle VerticalAlign="Top" />
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                                <FooterStyle CssClass="footer" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                <Columns>
                                    <asp:BoundField DataField="AgentID" HeaderText="Partner ID" SortExpression="AgentID" />
                                    <asp:BoundField DataField="AgentDBA" HeaderText="Partner DBA" SortExpression="AgentDBA"  />
                                    <asp:BoundField DataField="DBAName" HeaderText="DBAName" SortExpression="DBAName" />
                                    <asp:BoundField DataField="MID" HeaderText="MID" SortExpression="MID" />
                                    <asp:BoundField DataField="Downgrade Vol" HeaderText="Downgrade Vol" SortExpression="Downgrade Vol" />
                                    <%--<asp:BoundField DataField="Downgrade Cnt" />--%>
                                    <asp:TemplateField HeaderText="Downgrade Cnt" SortExpression="Downgrade Cnt">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="Server" ID="lnkCnt" Text='<%#Eval("Downgrade Cnt") %>' CommandArgument='<%#Eval("MID") %>'
                                                OnClick="btnCnt_Click"></asp:LinkButton>
                                            <asp:Label runat="Server" ID="lblCnt" Text='<%#Eval("Downgrade Cnt") %>' Style="display: none;"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Sales Vol" HeaderText="Sales Vol" SortExpression="Sales Vol" />
                                    <asp:BoundField DataField="Sales Cnt" HeaderText="Sales Cnt" SortExpression="Sales Cnt" />
                                    <asp:BoundField DataField="Downgrade Cnt %" HeaderText="Downgrade Cnt %" SortExpression="Downgrade Cnt %" />
                                    <asp:BoundField DataField="Downgrade Vol %" HeaderText="Downgrade Vol %" SortExpression="Downgrade Vol %" />
                                </Columns>
                            </asp:GridView>
                            <%-- <asp:GridView runat="server" ID="grd" AutoGenerateColumns="true" CssClass="mGrid">
                            </asp:GridView>--%>
                            <br />
                            <div style="width: 100%">
                                <div class="buckethdrleft">
                                    <asp:Panel runat="server" ID="Panel1">
                                        <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                            <span style="height: 25px; vertical-align: middle;">
                                                <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                    Excel</span></asp:LinkButton>
                                    </asp:Panel>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
