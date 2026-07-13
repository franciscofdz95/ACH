<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/MasterPageSales.master" Inherits="frmLeadActivityReport" Title="Lead Activity Report" Codebehind="frmLeadActivityReport.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dialog">
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
            <div class="title">
                &nbsp;&nbsp;Leads Activity Report
                <hr class="line" />
            </div>
            <asp:ValidationSummary runat="server" ID="ValidSummary" ShowSummary="true" DisplayMode="BulletList"
                Visible="true" />
            <asp:BulletedList runat="server" ID="lblError" CssClass="errorlist">
            </asp:BulletedList>
            <table border="0">
                <%--<tr>
                    <td class="lblRight">
                        Date of Last Contact :</td>
                    <td align="left">
                        <ig:WebDatePicker ID="SearchBeginDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                            BorderWidth="1px" EnableAppStyling="False" NullDateLabel="" Width="150px">
                        </ig:WebDatePicker>
                    </td>
                </tr>
                 <tr>
                    <td class="lblRight">
                        End Date:</td>
                    <td align="left">
                        <ig:WebDatePicker ID="SearchEndDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                            BorderWidth="1px" EnableAppStyling="False" NullDateLabel="" Width="150px">
                        </ig:WebDatePicker>
                    </td>
                </tr>--%>
                <tr>
                    <%-- <td class="lblRight" style="width: 13%;">
                        Agent:</td>
                    <td align="left">
                        <asp:DropDownList ID="AgentUID" runat="server" Width="300px">
                        </asp:DropDownList><cc1:ListSearchExtender ID="ListSearchExtender1" runat="server"
                            TargetControlID="AgentUID" PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt"
                            PromptPosition="Top" IsSorted="true" QueryPattern="Contains">
                        </cc1:ListSearchExtender>
                    </td>--%>
                    <td colspan="2">
                        <asp:Panel runat="server" ID="AgentSelect">
                            <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="vertical" IDWidth="80"
                                DBAWidth="120" lblDBAWidth="130" lblIDWidth="130" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        <asp:Label runat="server" Width="130px" ID="lblagent" Text="Include Sub-Agents:"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:CheckBox ID="chkSubAgent" runat="server" /></td>
                </tr>
                <tr>
                    <td class="lblRight">
                        <asp:Label runat="server" Width="130px" ID="Label1" Text=" Include Inactive Agents:"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:CheckBox runat="server" ID="chkActive" /></td>
                </tr>
                <tr>
                    <td class="lblRight">
                    </td>
                    <td align="left">
                        <div>
                            <br />
                            <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                AccessKey="h">
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
                    </td>
                </tr>
                <%-- <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lbl" Text="* Date of Last Contact is the date of when these events occurred: last note added, appointment created, or status change."
                            Font-Size="X-Small" Font-Italic="true"></asp:Label>
                        <br />
                    </td>
                </tr>--%>
            </table>
            <asp:Label runat="server" ID="lbl" Text="* Date of Last Contact is the date of when these events occurred: last note added, appointment created, or status change."
                Font-Size="X-Small" Font-Italic="true"></asp:Label>
        </asp:Panel>
        <br />
        <div class="title">
            &nbsp;&nbsp;Search Results
            <hr class="line" />
        </div>
        <asp:Label runat="server" ID="lblData" Text="No Data" Visible="true"></asp:Label>
        <asp:Panel runat="server" ID="pnl1" Width="99%" ScrollBars="Horizontal" Visible="false">
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
                        <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView runat="server" ID="grdLeadsActivity" AutoGenerateColumns="false" CssClass="mGrid"
                            OnSorting="grd_Sorting" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                            AllowSorting="true" Width="100%" OnRowCommand="grdLeadsActivity_RowCommand">
                            <RowStyle VerticalAlign="Top" />
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <FooterStyle CssClass="footer" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <Columns>
                                <asp:BoundField DataField="Agent" HeaderText="Agent Name" SortExpression="Agent"
                                    ItemStyle-Width="80px" />
                                <asp:TemplateField SortExpression="LeadID" HeaderText="LID" ItemStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="LeadID" ID="lblUID" Text='<%#Eval("LeadID") %>'
                                            CommandArgument='<%#Eval("LeadsUID") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="LeadName" HeaderText="Lead Name" SortExpression="LeadName"
                                    ItemStyle-Width="80px" HeaderStyle-Width="80px" />
                                <asp:BoundField DataField="Processing Volume" HeaderText="Processing Volume" SortExpression="Processing Volume"
                                    ItemStyle-Width="40px" DataFormatString="{0:0.00}" />
                                <asp:BoundField DataField="Contact Name" HeaderText="Contact Name" SortExpression="Contact Name"
                                    ItemStyle-Width="80px" HeaderStyle-Width="80px" />
                                <asp:BoundField DataField="Contact #" HeaderText="Contact #" SortExpression="Contact #"
                                    ItemStyle-Width="40px" HeaderStyle-Width="40px" />
                                <asp:BoundField DataField="Current Status" HeaderText="Current Status" SortExpression="Current Status"
                                    ItemStyle-Width="40px" HeaderStyle-Width="40px" />
                                <asp:BoundField DataField="Date of Last contact" HeaderText="Date of Last Contact"
                                    ItemStyle-Width="40px" HeaderStyle-Width="40px" SortExpression="Date of Last contact"
                                    DataFormatString="{0:MM/dd/yyyy hh:mm:ss tt}" />
                                <asp:BoundField DataField="Days Since Last Contact" HeaderText="Days Since Last Contact"
                                    ItemStyle-Width="40px" SortExpression="Days since last Contact" HeaderStyle-Width="40px" />
                                <asp:BoundField DataField="Source Description" HeaderText="Source Description" SortExpression="Source Description"
                                    ItemStyle-Width="60px" />
                            </Columns>
                        </asp:GridView>
                        <br />
                        <div style="width: 100%" class="bucketfooter">
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
    </div>
    <br />
</asp:Content>
