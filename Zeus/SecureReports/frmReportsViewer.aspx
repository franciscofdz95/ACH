<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="frmReportsViewer" Title="Reports" Codebehind="frmReportsViewer.aspx.cs" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <div class="bucket">
        <div class="buckethdr">
            <div class="buckethdrleft">
                &nbsp;<asp:Label ID="lblTitle" runat="server" Text="Label"></asp:Label></div>
            <div class="buckethdright">
                &nbsp;</div>
        </div>
        <div class="bucketbdy">
            <div class="twocolumn">
                <div class="leftcolumn">
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                </div>
                <div class="rightcolunm">
                    <igtxt:WebImageButton ID="btnViewReport" runat="server" OnClick="btnViewReport_Click"
                        Text="View Report" UseBrowserDefaults="False">
                        <RoundedCorners HeightOfBottomEdge="2" HoverImageUrl="ig_butCRM2.gif" ImageUrl="ig_butCRM1.gif"
                            MaxHeight="40" MaxWidth="400" PressedImageUrl="ig_butCRM2.gif" RenderingType="FileImages"
                            WidthOfRightEdge="2" />
                    </igtxt:WebImageButton>
                </div>
            </div>
            <br />
            <center>
                <rsweb:ReportViewer ID="LeadsByStatus1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                    Height="600px" Width="100%">
                    <LocalReport ReportPath="Reports\LeadsByStatus.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="SqlDataSource1" Name="DataSet1_sp_ReportLeadByStatus" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
            </center>
        </div>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PrimusPaymentsConnectionString %>"
        SelectCommand="sp_ReportLeadByStatus" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="AssignedStartDate" Type="DateTime" />
            <asp:Parameter Name="AssignedEndDate" Type="DateTime" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
