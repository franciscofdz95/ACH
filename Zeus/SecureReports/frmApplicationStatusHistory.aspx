<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true"
    Inherits="frmApplicationStatusHistory" Title="Application Status History" CodeBehind="frmApplicationStatusHistory.aspx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <div class="title">
                    &nbsp;&nbsp;Merchant Status History Report
                    <hr class="line" />
                </div>
                <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                    <div style="width: 100%">
                        <table>
                            <tr>
                                <td class="lblRight">
                                    <asp:Label Text="Begin Date:" ID="lbl" Width="70px" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                        Width="225px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
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
                                    <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                        Width="225px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                            SlideOpenDuration="1" />
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                            SlideOpenDuration="1" />
                                    </ig:WebDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight">
                                    Bank:
                                </td>
                                <td>
                                    <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="225px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight">
                                    Status:
                                </td>
                                <td>
                                    <asp:DropDownList ID="StatusUID" runat="server" Width="225px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <%--<td class="lblRight">
                                    Agent:</td>
                                <td>
                                    <asp:DropDownList ID="AgentUID" runat="server" Width="275px">
                                    
                                    </asp:DropDownList><cc1:ListSearchExtender ID="ListSearchExtender1" runat="server"
                                        TargetControlID="AgentUID" PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt"
                                        PromptPosition="Top" IsSorted="true" QueryPattern="Contains">
                                    </cc1:ListSearchExtender>
                                    </td>--%>
                                <td colspan="2">
                                    <asp:Panel runat="server" ID="AgentSelect">
                                        <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="vertical" IDWidth="180"
                                            DBAWidth="220" lblDBAWidth="70" lblIDWidth="70" />
                                    </asp:Panel>
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
                                                    CausesValidation="False" AccessKey="l">
                                                    <Appearance>
                                                        <Image Url="~/Images/delete.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight" colspan="2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SearchBeginDate"
                                        Display="None" ErrorMessage="Begin date required"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="SearchEndDate"
                                        Display="None" ErrorMessage="End date required"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="StatusUID"
                                        Display="None" ErrorMessage="Status required" Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <br />
                <div class="title">
                    &nbsp;&nbsp;Search Results
                    <hr class="line" />
                </div>
                <asp:Panel runat="server" ID="pnlRecords">
                    <table width="100%">
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <rsweb:ReportViewer ID="ApplicationStatus2" runat="server" Width="100%" Height="500px"
                        Font-Size="8pt" Style="padding: 5px 0px 30px 0px;" Font-Names="Verdana">
                        <LocalReport ReportPath="Reports\ApplicationStatus.rdlc">
                            <%--   <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1_sp_ReportMerchantStatus" />
                            </DataSources>--%>
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <br />
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
                        TypeName="DataSet1TableAdapters.sp_ReportMerchantStatusTableAdapter"></asp:ObjectDataSource>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlNoRecords">
                    &nbsp;No Records Found...</asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
