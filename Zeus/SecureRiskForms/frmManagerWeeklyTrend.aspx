<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true"
    Inherits="frmManagerWeeklyTrend" Title="Manager Weekly Trend" CodeBehind="~/SecureRiskForms/frmManagerWeeklyTrend.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <legend>Maanger Weekly Trend Report</legend>
        <rsweb:ReportViewer ID="ManagerWeeklyTrend1" runat="server" Height="500px" Font-Size="8pt"
            AsyncRendering="false" SizeToReportContent="true" Width="100%" Font-Names="Verdana"
            Style="padding: 5px 5px 30px 5px;">
            <LocalReport ReportPath="Reports\ManagerWeeklyTrend.rdlc">
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
