<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true"
    Inherits="frmUWMTDReport" Title="Underwriting MTD" CodeBehind="frmUWMTDReport.aspx.cs" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <div class="title">
        &nbsp;&nbsp;Underwriting MTD Report
        <hr class="line" />
    </div>
    <div class="indentedcontent20">
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
            <div>
                <table>
                    <tr>
                        <td class="lblRight">
                            <asp:Label runat="server" ID="lbl" Width="70px" Text="Period:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cboPeriod" runat="server" Width="170px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            Bank:
                        </td>
                        <td>
                            <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="170px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <%--<td class="lblRight">
                            Agent:</td>
                        <td>
                            <asp:DropDownList ID="AgentUID" runat="server" Width="170px">
                            </asp:DropDownList></td>--%>
                        <td colspan="2">
                            <asp:Panel runat="server" ID="AgentSelect">
                                <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="vertical" IDWidth="125"
                                    DBAWidth="165" lblDBAWidth="70" lblIDWidth="70" />
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
            </div>
        </asp:Panel>
    </div>
    <br />
    <div class="title">
        &nbsp;&nbsp;Search Results
        <hr class="line" />
    </div>
    <asp:Panel runat="Server" ID="Records">
        <div class="indentedcontent20">
            <rsweb:ReportViewer ID="rptViewer" runat="server" Width="100%" Height="500px" Font-Size="8pt"
                Font-Names="Verdana" Style="padding: 5px 0px 30px 0px;">
                <LocalReport ReportPath="Reports\UWMTD.rdlc">
                    <DataSources>
                        <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="DataSet1_sp_Report_UW_MTD" />
                    </DataSources>
                </LocalReport>
            </rsweb:ReportViewer>
            <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetData"
                TypeName="DataSet1TableAdapters.sp_Report_UW_MTDTableAdapter"></asp:ObjectDataSource>
        </div>
    </asp:Panel>
    <asp:Panel runat="Server" ID="NoRecords">
        &nbsp;No Records Found....
    </asp:Panel>
</asp:Content>
