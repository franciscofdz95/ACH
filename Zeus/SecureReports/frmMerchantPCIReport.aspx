<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true"
    Inherits="frmMerchantPCIReport" Title="Merchant PCI Report" CodeBehind="frmMerchantPCIReport.aspx.cs" %>

<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <div class="title">
            &nbsp;&nbsp; Merchant PCI Search
            <hr class="line" />
        </div>
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
            <table width="100%">
                <tr>
                    <td colspan="6">
                        <asp:Label runat="server" ID="lblError" SkinID="Error"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" valign="top">
                        <fieldset style="width: 93%;">
                            <legend><font size='1'><i>Select Date</i></font></legend>
                            <table width="100%">
                                <tr>
                                    <td class="lblRight">
                                        Start Date:
                                    </td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                            Width="105px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                        </ig:WebDatePicker>
                                    </td>
                                    <td class="lblRight">
                                        End Date:
                                    </td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                            Width="105px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                        </ig:WebDatePicker>
                                    </td>
                                    <td colspan="2">
                                        <asp:RadioButtonList runat="server" ID="DateType" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Sent Date" Value="0" Selected="true">
                                            </asp:ListItem>
                                            <asp:ListItem Text="Completed Date" Value="1">
                                            </asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        DBA Name:
                    </td>
                    <td>
                        <asp:TextBox ID="BusinessDBAName" runat="server" Width="145px" EnableViewState="False"></asp:TextBox>
                    </td>
                    <td class="lblRight">
                        MLE:
                    </td>
                    <td>
                        <asp:TextBox ID="BusinessLegalName" runat="server" EnableViewState="False" Width="145px"></asp:TextBox>
                    </td>
                    <td class="lblRight">
                        Email:
                    </td>
                    <td>
                        <asp:TextBox ID="BusinessEmailAddress" runat="server" Width="145px" EnableViewState="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Status:
                    </td>
                    <td>
                        <asp:DropDownList ID="StatusUID" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">
                        Contact Name:
                    </td>
                    <td>
                        <asp:TextBox ID="BusinessContact" runat="server" Width="145px" EnableViewState="False"></asp:TextBox>
                    </td>
                    <td class="lblRight">
                        Phone:
                    </td>
                    <td>
                        <asp:TextBox ID="BusinessPhone" runat="server" Width="145px" EnableViewState="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Vendor:
                    </td>
                    <td>
                        <asp:DropDownList ID="VendorID" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">
                        ZID:
                    </td>
                    <td>
                        <asp:TextBox ID="MerchantID" runat="server" EnableViewState="False" Width="145px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <%-- <td class="lblRight">
                        Agent:</td>
                    <td>
                        <asp:DropDownList ID="AgentUID" runat="server" Width="150px">
                        </asp:DropDownList></td>--%>
                    <td colspan="6">
                        <asp:Panel runat="server" ID="AgentSelect">
                            <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="horizontal"
                                IDWidth="145px" DBAWidth="145px" lblDBAWidth="90px" lblIDWidth="195px" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
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
            &nbsp; &nbsp;
        </div>
        <br />
        <div class="title">
            &nbsp;&nbsp;Search Results
            <hr class="line" />
        </div>
        <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
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
                        <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                            AlternatingRowStyle-CssClass="alt" DataKeyNames="MerchantAppUID" OnRowDataBound="grd_RowDataBound"
                            OnRowCommand="grd_RowCommand" OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True"
                            OnSorting="grd_Sorting" DataSourceID="odsLeads">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <Columns>
                                <asp:TemplateField HeaderText="ZID">
                                    <ItemTemplate>
                                        <%--<asp:LinkButton ID="lbtnZID" runat="server" CommandName="MerchantID"></asp:LinkButton>--%>
                                        <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantPCI.aspx?Adding=false&MerchantAppUID=" + Eval("MerchantAppUID")  %>'
                                            runat="server" ID="hypZID" Text='<%# Eval("MerchantID") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name" SortExpression="BusinessDBAName" />
                                <asp:BoundField DataField="BusinessLegalName" HeaderText="MLE" SortExpression="BusinessLegalName" />
                                <asp:BoundField DataField="AgentDBAName" HeaderText="Agent Name" SortExpression="AgentDBAName" />
                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                                <asp:BoundField DataField="PCIStatus" HeaderText="PCI Status" SortExpression="PCIStatus" />
                                <asp:BoundField DataField="RequestDate" HeaderText="Sent Date" SortExpression="RequestDate"
                                    DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="CompletedDate" HeaderText="Completed Date" SortExpression="CompletedDate"
                                    DataFormatString="{0:MM/dd/yyyy}" />
                                <asp:BoundField DataField="NCDays" HeaderText="NC Days" SortExpression="NCDays" />
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsLeads" runat="server" SelectMethod="GetMerchantPCIPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetMerchantPCIPagingRowCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsLeads_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="bucketfooter">
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 33%;">
                                        <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                            <span style="height: 25px; vertical-align: middle;">
                                                <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                    Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                    </td>
                                    <td align="right">
                                        Export:&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                            <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                            <asp:ListItem Value="1">All Pages</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="right" style="width: 33%;">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
            No data...
        </asp:Panel>
        <br />
    </div>
</asp:Content>
