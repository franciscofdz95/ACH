<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true" Inherits="frmDeclineReasons"
    Title="Decline Reasons" Codebehind="~/SecureRiskForms/frmDeclineReasons.aspx.cs" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>Decline Reasons Report</legend>
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
            <div style="width: 100%">
                <table>
                    <tr>
                        <td colspan="14">
                            <asp:Label runat="server" ID="lblError" Text="" SkinID="Required"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            Decline Source:</td>
                        <td>
                            <asp:DropDownList ID="lstSource" runat="server" Width="100px">
                                <asp:ListItem Selected="true" Text="All" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Provider" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Internal" Value="1"></asp:ListItem>
                            </asp:DropDownList></td>
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
                            ZID:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtZID" Width="90px"></asp:TextBox>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtZID"
                                ErrorMessage="Please enter a valid ZID." MaximumValue="100000" MinimumValue="1"
                                Type="Integer" Display="Dynamic"></asp:RangeValidator>
                        </td>
                        <td class="lblRight">
                            MID:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtMID" Width="90px"></asp:TextBox>
                        </td>
                        <td class="lblRight">
                            DBA Name:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtDBAName" Width="90px"></asp:TextBox>
                        </td>
                        <td class="lblRight">
                            MLE:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtLegalName" Width="90px"></asp:TextBox>
                        </td>
                        <td align="left">
                            <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                AccessKey="h">
                                <Appearance>
                                    <Image Url="~/Images/Check.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td align="left">
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
        <rsweb:ReportViewer ID="DeclineReasons2" runat="server" Height="500px" Font-Size="8pt"
            Font-Names="Verdana" Style="padding: 5px 5px 30px 5px;" AsyncRendering="false"
            SizeToReportContent="true" Width="100%">
            <LocalReport ReportPath="Reports\DeclineReasons.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1_sp_ReportMerchantStatus" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:Label runat="server" ID="lbl" Text="no data.." Visible="false"></asp:Label>
    </fieldset>
    <br />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
        TypeName="DataSet1TableAdapters.sp_ReportMerchantStatusTableAdapter"></asp:ObjectDataSource>
</asp:Content>
