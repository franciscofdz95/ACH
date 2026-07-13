<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true"
    Inherits="frmApplicationStatus" Title="Application Status" CodeBehind="frmApplicationStatus.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <fieldset>
                    <legend>Merchant Status Report</legend>
                    <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                        <div style="width: 100%">
                            <table>
                                <tr>
                                    <td class="lblRight">
                                        &nbsp;Begin Date:
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
                                    <td class="lblRight">
                                        &nbsp; End Date:
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
                                    <%-- <td class="lblRight">
                                        Agent:</td>
                                    <td>
                                        <asp:DropDownList ID="AgentUID" runat="server" Width="275px">
                                        </asp:DropDownList><cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="AgentUID"
                                            PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                                            IsSorted="true" QueryPattern="Contains">
                                        </cc1:ListSearchExtender></td>--%>
                                    <td class="lblRight">
                                        &nbsp; Bank:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="4">
                                        <asp:Panel runat="server" ID="AgentSelect">
                                            <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="horizontal"
                                                IDWidth="100px" DBAWidth="100px" lblDBAWidth="70px" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <fieldset>
                                <legend>Status</legend>
                                <asp:CheckBoxList ID="lstStatus" runat="server" RepeatColumns="4">
                                </asp:CheckBoxList>
                            </fieldset>
                        </div>
                        <div style="text-align: center;">
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
                        </div>
                    </asp:Panel>
                </fieldset>
                <br />
                <fieldset>
                    <legend>Search Results</legend>
                    <asp:Panel runat="server" ID="pnlData">
                        <rsweb:ReportViewer ID="ApplicationStatus1" runat="server" Width="100%" Height="500px"
                            Font-Size="8pt" Style="padding: 5px 0px 30px 0px;" Font-Names="Verdana">
                            <LocalReport ReportPath="Reports\ApplicationStatus.rdlc">
                                <DataSources>
                                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1_sp_ReportMerchantStatus" />
                                </DataSources>
                            </LocalReport>
                        </rsweb:ReportViewer>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlRecords">
                        &nbsp; No Records Found....
                    </asp:Panel>
                </fieldset>
                <br />
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
                    TypeName="DataSet1TableAdapters.sp_ReportMerchantStatusTableAdapter"></asp:ObjectDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
