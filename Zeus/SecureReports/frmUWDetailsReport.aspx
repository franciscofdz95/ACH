<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true"
    Inherits="frmUWDetailsReport" Title="Underwriting Details" CodeBehind="frmUWDetailsReport.aspx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <div class="title">
        &nbsp;&nbsp;Underwriting Details Report
        <hr class="line" />
    </div>
    <div class="indentedcontent20">
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
            <div>
                <table>
                    <tr>
                        <td class="lblRight">
                            Begin Date:
                        </td>
                        <td>
                            <ig:WebDatePicker ID="SearchBeginDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                                BorderWidth="1px" EnableAppStyling="False" NullDateLabel="" Width="100px">
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                    SlideOpenDuration="1" />
                            </ig:WebDatePicker>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SearchBeginDate"
                                Display="Dynamic" ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            End Date:
                        </td>
                        <td>
                            <ig:WebDatePicker ID="SearchEndDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                                BorderWidth="1px" EnableAppStyling="False" NullDateLabel="" Width="100px">
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                    SlideOpenDuration="1" />
                            </ig:WebDatePicker>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="SearchEndDate"
                                Display="Dynamic" ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2">
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
    <div class="indentedcontent20">
        <%-- <rsweb:ReportViewer ID="UWDetails1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="Reports\UWDetails.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
       

       
        <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="GetData"
            TypeName="DataSet1TableAdapters.sp_Report_UW_DetailsTableAdapter"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetData"
            TypeName="DataSet1TableAdapters.sp_Report_UW_MTDTableAdapter"></asp:ObjectDataSource>--%>
    </div>
    <rsweb:ReportViewer ID="UWDetails1" runat="server" Font-Names="Verdana" Font-Size="8pt"
        InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        <LocalReport ReportPath="Reports\UWDetails.rdlc">
            <%--    <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSet1_sp_Report_UW_Details" />
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                    Name="DataSet1_sp_Report_UW_Details_Declines" />
            </DataSources>--%>
        </LocalReport>
    </rsweb:ReportViewer>
    <br />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData"
        TypeName="DataSet1TableAdapters.sp_ReportMerchantStatusTableAdapter"></asp:ObjectDataSource>
</asp:Content>
