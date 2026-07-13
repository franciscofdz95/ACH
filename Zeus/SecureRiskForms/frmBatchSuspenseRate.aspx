<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true" Inherits="frmBatchSuspenseRate"
    Title="Batch Suspense Rate" Codebehind="~/SecureRiskForms/frmBatchSuspenseRate.aspx.cs" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>Batch Suspense Rate Report</legend>
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
            <div style="width: 100%">
                <table>
                    <tr>
                        <td colspan="10">
                            <asp:Label runat="server" ID="lblError" Text="" SkinID="Required"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            Begin Date:</td>
                        <td>
                            <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False"
                                NullDateLabel="" Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                        </td>
                        <td class="lblRight">
                            End Date:</td>
                        <td>
                            <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False"
                                NullDateLabel="" Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                        </td>
                        <td class="lblRight">
                            Merchant Status:</td>
                        <td>
                            <asp:DropDownList ID="lstStatus" runat="server" Width="170px">
                            </asp:DropDownList></td>
                        <td class="lblRight">
                            Suspense Reason:</td>
                        <td>
                            <asp:DropDownList ID="lstReason" runat="server" Width="205px">
                            </asp:DropDownList></td>
                        <td align="left">
                            <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                AccessKey="h">
                                <Appearance>
                                    <Image Url="../Images/check.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td align="left">
                            <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                                AccessKey="l">
                                <Appearance>
                                    <Image Url="../Images/delete.png" />
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
        <rsweb:ReportViewer ID="BatchSuspenseRate1" runat="server" Style="padding: 5px 0px 30px 0px;"
            Height="500px" Width="100%">
            <LocalReport ReportPath="Reports\BatchSuspenseRate.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet2_sp_ReportBatchSuspenseRates" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:Label runat="server" ID="lbl" Text="no data.." Visible="false"></asp:Label>
    </fieldset>
    <br />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
        TypeName="DataSet2TableAdapters.sp_ReportBatchSuspenseRatesTableAdapter"></asp:ObjectDataSource>
</asp:Content>
