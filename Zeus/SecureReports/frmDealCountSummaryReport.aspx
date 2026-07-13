<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true"
    Inherits="frmDealCountSummaryReport" Title="Merchant Profile Search" CodeBehind="frmDealCountSummaryReport.aspx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div class="title">
                    &nbsp;&nbsp;Deal Count Summary Report
                    <hr class="line" />
                </div>
                <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                    <table>
                        <tr>
                            <td class="lblRight">
                                &nbsp;&nbsp; Begin Date:
                            </td>
                            <td>
                                <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                    Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                </ig:WebDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">
                                &nbsp;&nbsp; End Date:
                            </td>
                            <td>
                                <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                    Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                </ig:WebDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">
                            </td>
                            <td>
                                <table>
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
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <div class="title">
                    &nbsp;&nbsp;Search Results
                    <hr class="line" />
                </div>
                <fieldset>
                    <legend>Credit Card</legend>
                    <asp:Label ID="NoData" Text="No Data..." runat="server"></asp:Label>
                    <asp:Panel runat="server" ID="PnlCreditCard" Width="100%" Height="" Visible="false">
                        <div class="buckethdright">
                            <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text=""></asp:Label>
                        </div>
                        <br />
                        <asp:GridView runat="server" ID="grdCredit" AutoGenerateColumns="false" CssClass="mGrid"
                            AllowPaging="True" Font-Names="Verdana" Font-Size="X-Small" PagerStyle-CssClass="pgr"
                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="grd_PageIndexChanging"
                            Width="100%" AllowSorting="True" OnSorting="grd_Sorting">
                            <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                            <Columns>
                                <asp:BoundField DataField="AgentFullName" HeaderText="Agent" ItemStyle-Width="250px"
                                    SortExpression="AgentFullName" />
                                <asp:BoundField DataField="UserName" HeaderText="ARM" ItemStyle-Width="100px" SortExpression="UserName" />
                                <asp:BoundField DataField="Deal Count" HeaderText="# App Recvd" ItemStyle-Width="100px"
                                    SortExpression="Deal Count" />
                                <asp:BoundField DataField="Requested Volume" HeaderText="Volume" ItemStyle-Width="100px"
                                    SortExpression="Requested Volume" DataFormatString="{0:0.00}" />
                                <asp:BoundField DataField="Number Approved" HeaderText="# Approved" ItemStyle-Width="100px"
                                    SortExpression="Number Approved" />
                                <asp:BoundField DataField="Approved Volume" HeaderText="Appr. Volume" ItemStyle-Width="100px"
                                    SortExpression="Approved Volume" DataFormatString="{0:0.00}" />
                                <asp:BoundField DataField="Number Declined" HeaderText="# Declined" ItemStyle-Width="100px"
                                    SortExpression="Number Declined" />
                                <asp:BoundField DataField="Declined Volume" HeaderText="Declined Volume" ItemStyle-Width="100px"
                                    SortExpression="Declined Volume" DataFormatString="{0:0.00}" />
                                <asp:BoundField DataField="Number Pending" HeaderText="# Pending" ItemStyle-Width="100px"
                                    SortExpression="Number Pending" />
                                <asp:BoundField DataField="Prior Month Received" HeaderText="Prior Month Recvd" ItemStyle-Width="100px"
                                    SortExpression="Prior Month Received" />
                                <asp:BoundField DataField="Previous Number Approved" HeaderText="Prior Month Appr."
                                    ItemStyle-Width="100px" SortExpression="Previous Number Approved" />
                            </Columns>
                        </asp:GridView>
                        <hr class="line" />
                        <table style="width: 100%">
                            <tr>
                                <td align="left">
                                    <asp:Panel runat="server" ID="Panel1">
                                        <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                            <span style="height: 25px; vertical-align: middle;">
                                                <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span> <span style="margin-left: 5px;">
                                                    Save Excel</span></asp:LinkButton>
                                    </asp:Panel>
                                </td>
                                <td align="right">
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
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
                <br />
                <fieldset>
                    <legend>ACH</legend>
                    <asp:Label ID="lblNoData" Text="No Data..." runat="server"></asp:Label>
                    <asp:Panel runat="server" ID="PnlACH" Width="" Height="" Visible="false">
                        <div class="buckethdright">
                            <asp:Label ID="lblCount" SkinID="RecordCount" runat="server" Text=""></asp:Label>
                        </div>
                        <br />
                        <asp:GridView runat="server" ID="grdACH" AutoGenerateColumns="false" CssClass="mGrid"
                            AllowPaging="True" Font-Names="Verdana" Font-Size="X-Small" PagerStyle-CssClass="pgr"
                            AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="grd_PageIndexChanging"
                            Width="100%" AllowSorting="True" OnSorting="grd_Sorting">
                            <Columns>
                                <asp:BoundField DataField="AgentFullName" HeaderText="Agent" ItemStyle-Width="250px"
                                    SortExpression="AgentFullName" />
                                <asp:BoundField DataField="UserName" HeaderText="ARM" ItemStyle-Width="100px" SortExpression="UserName" />
                                <asp:BoundField DataField="Deal Count" HeaderText="# App Recvd" ItemStyle-Width="100px"
                                    SortExpression="Deal Count" />
                                <asp:BoundField DataField="Requested Volume" HeaderText="Volume" ItemStyle-Width="100px"
                                    SortExpression="Requested Volume" />
                                <asp:BoundField DataField="Number Approved" HeaderText="# Approved" ItemStyle-Width="100px"
                                    SortExpression="Number Approved" />
                                <asp:BoundField DataField="Approved Volume" HeaderText="Appr. Volume" ItemStyle-Width="100px"
                                    SortExpression="Approved Volume" />
                                <asp:BoundField DataField="Number Declined" HeaderText="# Declined" ItemStyle-Width="100px"
                                    SortExpression="Number Declined" />
                                <asp:BoundField DataField="Declined Volume" HeaderText="Declined Volume" ItemStyle-Width="100px"
                                    SortExpression="Declined Volume" />
                                <asp:BoundField DataField="Number Pending" HeaderText="# Pending" ItemStyle-Width="100px"
                                    SortExpression="Number Pending" />
                                <asp:BoundField DataField="" HeaderText="Prior Month Recvd" ItemStyle-Width="100px"
                                    SortExpression="" />
                                <asp:BoundField DataField="Previous Number Approved" HeaderText="Prior Month Appr."
                                    ItemStyle-Width="100px" SortExpression="Previous Number Approved" />
                            </Columns>
                        </asp:GridView>
                        <hr class="line" />
                        <table style="width: 100%">
                            <tr>
                                <td align="left">
                                    <asp:Panel runat="server" ID="Panel2">
                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnExport1_Click">
                                            <span style="height: 25px; vertical-align: middle;">
                                                <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                    Excel</span></asp:LinkButton>
                                    </asp:Panel>
                                </td>
                                <td align="right">
                                    Page Size:
                                    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize1_SelectedIndexChanged">
                                        <asp:ListItem Selected="True">10</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>100</asp:ListItem>
                                        <asp:ListItem>250</asp:ListItem>
                                        <asp:ListItem>500</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
                    TypeName="DataSet1TableAdapters.sp_ReportDealCountSummaryTableAdapter"></asp:ObjectDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
