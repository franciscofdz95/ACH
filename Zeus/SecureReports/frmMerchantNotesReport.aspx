<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="frmMerchantNotesReport" Title="Merchant Notes Report" Codebehind="frmMerchantNotesReport.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <fieldset>
                    <legend>Search Criteria - Merchant Notes Report</legend>
                    <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                        <div style="width: 100%">
                            <table cellspacing="5">
                                <tr>
                                    <td class="lblRight">
                                        Begin Date:</td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False"
                                            NullDateLabel="" Width="150px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                    </td>
                                    <td class="lblRight">
                                        <asp:Label Text="End Date:" ID="lbl" runat="server" Width="63px"></asp:Label></td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False"
                                            NullDateLabel="" Width="150px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                    </td>
                                    <td class="lblRight">
                                    <asp:Label Text="Bank:" ID="Label1" runat="server" Width="62px"></asp:Label>    </td>
                                    <td>
                                        <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        DBA:</td>
                                    <td>
                                        <asp:TextBox ID="BusinessDBAName" runat="server" EnableViewState="False" Width="145px"></asp:TextBox></td>
                                    <%--<td class="lblRight">
                                        Agent:</td>
                                    <td>
                                        <asp:DropDownList ID="AgentUID" runat="server" Width="280px">
                                        </asp:DropDownList><cc1:ListSearchExtender ID="ListSearchExtender1" runat="server"
                                            TargetControlID="AgentUID" PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt"
                                            PromptPosition="Top" IsSorted="true" QueryPattern="Contains">
                                        </cc1:ListSearchExtender>
                                    </td>--%>
                                    <td colspan="4">
                                        <asp:Panel runat="server" ID="AgentSelect">
                                            <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="horizontal"
                                                IDWidth="105px" DBAWidth="145px" lblDBAWidth="65px" lblIDWidth="72px" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <fieldset>
                                <legend>Status</legend>
                                <asp:CheckBoxList ID="lstStatus" runat="server" RepeatColumns="5" CellSpacing="5"
                                    RepeatDirection="horizontal" Width="100%">
                                </asp:CheckBoxList>
                            </fieldset>
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
                </fieldset>
                <br />
                <fieldset>
                    <legend>Search Results</legend>
                    <asp:Panel ID="pnlRecords" runat="server" Visible="false">
                        <div class="buckethdright">
                            <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label>&nbsp;</div>
                        <asp:GridView ID="grd" runat="server" OnRowCommand="grd_RowCommand" OnRowDataBound="grd_RowDataBound"
                            DataSourceID="odsMerchants" CssClass="mGrid" OnSorting="grd_Sorting" AllowPaging="true"
                            AllowSorting="true" OnPageIndexChanging="grd_PageIndexChanging" AutoGenerateColumns="false">
                            <FooterStyle CssClass="footer" />
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <Columns>
                                <asp:BoundField DataField="ID" Visible="False"></asp:BoundField>
                                <asp:BoundField DataField="AchID" Visible="False"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnSelect" runat="server" CommandName="Select" Text='Select'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name"></asp:BoundField>
                                <asp:BoundField DataField="Bank" HeaderText="Bank"></asp:BoundField>
                                <asp:BoundField DataField="AgentFullName" HeaderText="Agent" ></asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                                <asp:BoundField DataField="MerchantAppUID" Visible="False"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsMerchants" runat="server" SelectMethod="GetMerchantAppsPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetMerchantAppsPagingRowCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsMerchants_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                                <asp:Parameter Name="ControlID" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <div class="bucketfooter">
                            <div class="bucketfooterleft">
                                <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnExport_Click">
                                    <span style="height: 25px; vertical-align: middle;">
                                        <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                            Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="btnPDF" runat="server" OnClick="btnExportPDF_Click">
                                    <span style="height: 25px; vertical-align: middle;">
                                        <asp:Image ID="Image2" runat="server" SkinID="SavePDF" /></span><span style="margin-left: 5px;">Save
                                            PDF</span></asp:LinkButton>&nbsp;&nbsp;
                                <asp:LinkButton ID="btnSelectAll" runat="server" OnClick="btnSelectAll_Click">Select All</asp:LinkButton>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                        No data...
                    </asp:Panel>
                </fieldset>
                <fieldset>
                    <legend>Select Merchants</legend>
                    <br />
                    <asp:ListBox ID="lstSelected" runat="server" Width="100%"></asp:ListBox>
                    <br />
                    <asp:Button ID="btnViewReport" runat="server" Text="View Report" OnClick="btnViewReport_Click" />
                    <asp:Button ID="btnClearList" runat="server" Text="Clear List" OnClick="btnClearList_Click" />
                </fieldset>
                <br />
                <fieldset>
                    <legend>Merchant Notes Report</legend>
                    <br />
                    <rsweb:ReportViewer ID="MerchantNotes1" runat="server" Width="100%" Height="500px" Font-Size="8pt"
                        Font-Names="Verdana">
                        <LocalReport ReportPath="Reports\MerchantNotes.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="DataSet1_sp_ReportMerchantNotes" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetData"
                        TypeName="DataSet1TableAdapters.sp_ReportMerchantNotesTableAdapter"></asp:ObjectDataSource>
                    &nbsp;&nbsp;
                </fieldset>
            </td>
        </tr>
    </table>
</asp:Content>
