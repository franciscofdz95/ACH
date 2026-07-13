<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="frmCurrentPendingReport" MasterPageFile="~/MasterPageReports.master"
    Title="Current Pending Report" Codebehind="frmCurrentPendingReport.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:BulletedList runat="server" ID="blError" CssClass="errorlist">
    </asp:BulletedList>
            <asp:ValidationSummary runat="server" ID="ValidSummary" ShowSummary="true" DisplayMode="BulletList"
                Visible="true" />
    <div class="dialog">
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
            <div class="title">
                &nbsp;&nbsp;Current Pending Report
                <hr class="line" />
            </div>
            <table cellspacing="2">
                <tr>
                    <td class="lblRight">
                        <asp:Label Text="Time Frame:" ID="lbl" runat="server" Width="110px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlTimeFrame" OnSelectedIndexChanged="ddlTimeFrame_SelectedIndexChanged"
                            Width="155px">
                            <asp:ListItem Value="0">Select Time Frame</asp:ListItem>
                            <asp:ListItem Value="1">Last 30 days</asp:ListItem>
                            <asp:ListItem Value="2">Last 60 days</asp:ListItem>
                            <asp:ListItem Value="3">Last 90 days</asp:ListItem>
                            <asp:ListItem Value="4">Last Month</asp:ListItem>
                            <asp:ListItem Value="5">Month to Date</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Begin Date:
                    </td>
                    <td>
                        <ig:WebDatePicker ID="SearchBeginDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                            BorderWidth="1px" EnableAppStyling="False" NullDateLabel="" Width="155px">
                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                SlideOpenDuration="1" />
                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                SlideOpenDuration="1" />
                        </ig:WebDatePicker>
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
                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                SlideOpenDuration="1" />
                        </ig:WebDatePicker>
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
                    <td colspan="2">
                        <asp:Panel runat="server" ID="AgentSelect">
                            <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="vertical" IDWidth="110"
                                DBAWidth="150" lblDBAWidth="110" lblIDWidth="110" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Include Sub-Agents:
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
                        Agent Channel:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="PartnerChannel" runat="server" Width="155px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
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
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <div class="title">
            &nbsp;&nbsp;Search Results
            <hr class="line" />
        </div>
        <asp:Label runat="server" ID="lblData" Text="No Data" Visible="false"></asp:Label>
        <asp:Panel runat="server" ID="pnl1">
            <table>
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
                        <asp:GridView runat="server" ID="grdCurrentPending" AutoGenerateColumns="false" CssClass="mGrid"
                            OnSorting="grd_Sorting" OnRowDataBound="grdCurrentPending_RowDataBound" AllowPaging="true"
                            OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="true">
                            <RowStyle VerticalAlign="Top" />
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <Columns>
                                <asp:BoundField DataField="DBA" HeaderText="DBA" ItemStyle-Width="80px" SortExpression="DBA" />
                                <asp:BoundField DataField="Legal" HeaderText="Legal Name" ItemStyle-Width="80px"
                                    SortExpression="Legal" />
                                <asp:BoundField DataField="AgentID" HeaderText="Agent ID" ItemStyle-Width="40px" 
                                    SortExpression="AgentID" />
                                <asp:BoundField DataField="AgentDBA" HeaderText="Agent DBA" ItemStyle-Width="80px" 
                                    SortExpression="AgentDBA" />
                                <asp:BoundField DataField="Bank" HeaderText="Bank" ItemStyle-Width="50px" SortExpression="Bank" />
                                <asp:BoundField DataField="Volume" HeaderText="Volume" ItemStyle-Width="65px" SortExpression="Volume"
                                    DataFormatString="{0:N0}" />
                                <asp:BoundField DataField="DateSubmitted" HeaderText="Date Submitted" ItemStyle-Width="60px"
                                    SortExpression="DateSubmitted" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" />
                                <asp:BoundField DataField="DatePended" HeaderText="Date Pended" ItemStyle-Width="60px"
                                    SortExpression="DatePended" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" />
                                <asp:BoundField DataField="PendedDays" HeaderText="Pended Days" ItemStyle-Width="30px"
                                    SortExpression="PendedDays" DataFormatString="{0:N0}" />
                                <asp:BoundField DataField="Status" HeaderText="Pend Type" ItemStyle-Width="30px"
                                    SortExpression="Status" />
                                <asp:BoundField HeaderText="Notes" ItemStyle-Width="200px" DataField="Conditions"
                                    SortExpression="Conditions" />
                            </Columns>
                        </asp:GridView>
                        <br />
                        <div class="bucketfooter" id="div">
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 33%;">
                                        <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                            <span style="height: 25px; vertical-align: middle;">
                                                <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                    Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <br />
</asp:Content>
