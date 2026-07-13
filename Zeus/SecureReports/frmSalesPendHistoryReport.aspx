<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="frmSalesPendHistoryReport" MasterPageFile="~/MasterPageReports.master" Codebehind="frmSalesPendHistoryReport.aspx.cs" %>

<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="dialog">
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
            <div class="title">
                &nbsp;&nbsp;Sales Office Pend History Report
                <hr class="line" />
            </div>
            <asp:ValidationSummary runat="server" ID="ValidSummary" ShowSummary="true" DisplayMode="BulletList"
                Visible="true" />
            <table cellspacing="2">
                <tr>
                    <td class="lblRight">
                        Begin Date:
                    </td>
                    <td>
                        <ig:WebDatePicker ID="SearchBeginDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                            BorderWidth="1px" EnableAppStyling="False" NullDateLabel="" Width="155px">
                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                SlideOpenDuration="1" />
                        </ig:WebDatePicker>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SearchBeginDate"
                            Display="None" ErrorMessage="Begin date required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        End Date:
                    </td>
                    <td>
                        <ig:WebDatePicker ID="SearchEndDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                            BorderWidth="1px" EnableAppStyling="False" NullDateLabel="" Width="155px">
                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                SlideOpenDuration="1" />
                        </ig:WebDatePicker>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="SearchEndDate"
                            Display="None" ErrorMessage="End date required"></asp:RequiredFieldValidator>
                        <asp:CompareValidator runat="server" ID="cvValidDate" ErrorMessage="End date should be greater than Start date"
                            Text="" ControlToCompare="SearchBeginDate" ControlToValidate="SearchEndDate"
                            Display="none" Operator="greaterthanequal" Type="date">
                        </asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Pend Type:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddpPend" runat="server" Width="155px">
                            <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="CU" Value="CU"></asp:ListItem>
                            <asp:ListItem Text="SS" Value="SS"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        SS Rep:
                    </td>
                    <td>
                        <asp:DropDownList ID="PrimaryContactUID" runat="server" Width="155px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <%-- <td class="lblRight">
                        Agent:</td>
                    <td>
                        <asp:DropDownList ID="AgentUID" runat="server" Width="300px">
                        </asp:DropDownList>
                        <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="AgentUID"
                            PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                            IsSorted="true" QueryPattern="Contains">
                        </cc1:ListSearchExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please select an Agent"
                            ControlToValidate="AgentUID" InitialValue="-1" Text="" Display="none">
                        </asp:RequiredFieldValidator>
                    </td>--%>
                    <td colspan="2">
                        <asp:Panel runat="server" ID="AgentSelect">
                            <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="vertical" IDWidth="110"
                                DBAWidth="150" lblDBAWidth="110" lblIDWidth="110" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        <asp:Label runat="server" Text="Include Sub-Agents:" ID="lbl" Width="110px"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkSubAgent" />
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Agent Category:
                    </td>
                    <td align="left">
                        <asp:CheckBoxList runat="server" ID="AgentCategoryUID" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                    </td>
                    <td>
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
            </table>
        </asp:Panel>
        <br />
        <div class="title">
            &nbsp;&nbsp;Search Results
            <hr class="line" />
        </div>
        <asp:Label runat="server" ID="lblData" Text=" No Data..." Visible="false"></asp:Label>
        <asp:Panel runat="server" ID="pnl1">
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
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">
                        <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView runat="server" ID="grdHistoryPending" AutoGenerateColumns="false" CssClass="mGrid"
                            OnSorting="grd_Sorting" OnRowDataBound="grdHistoryPending_RowDataBound" AllowPaging="true"
                            OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="true">
                            <RowStyle VerticalAlign="Top" />
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <Columns>
                                <asp:BoundField DataField="DBA" HeaderText="DBA" ItemStyle-Width="130px" SortExpression="DBA" />
                                <asp:BoundField DataField="Legal" HeaderText="MLE" ItemStyle-Width="100px"
                                    SortExpression="Legal" />
                                <asp:BoundField DataField="AgentID" HeaderText="Agent ID" ItemStyle-Width="30px" 
                                    SortExpression="AgentID" />
                                <asp:BoundField DataField="AgentDBA" HeaderText="Agent DBA" ItemStyle-Width="130px" 
                                    SortExpression="AgentDBA" />
                                <asp:BoundField DataField="Bank" HeaderText="Bank (Submitted to)" ItemStyle-Width="75px"
                                    SortExpression="Bank" />
                                <asp:BoundField DataField="DateSubmitted" HeaderText="Date Submitted" ItemStyle-Width="50px"
                                    SortExpression="DateSubmitted" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" />
                                <asp:BoundField DataField="PendStatus" HeaderText="Pend Type (Credit Or App)" ItemStyle-Width="50px"
                                    SortExpression="PendStatus" />
                                <asp:BoundField DataField="DatePended" HeaderText="Date Pended" ItemStyle-Width="50px"
                                    SortExpression="DatePended" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" />
                                <asp:BoundField DataField="Status" HeaderText="Current Status" ItemStyle-Width="70px"
                                    SortExpression="Status" />
                                <asp:BoundField DataField="CurrentStatusDate" HeaderText="Last Status Date" ItemStyle-Width="50px"
                                    SortExpression="CurrentStatusDate" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" />
                                <asp:BoundField DataField="Conditions" HeaderText="Notes" ItemStyle-Width="150px" />
                            </Columns>
                        </asp:GridView>
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
    </div>
    <br />
</asp:Content>
