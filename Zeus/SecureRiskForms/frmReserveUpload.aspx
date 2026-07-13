<%@ Page Title="Reserve Upload" Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true" CodeBehind="frmReserveUpload.aspx.cs" Inherits="ZeusWeb.SecureRiskForms.frmReserveUpload" %>

<%@ Register Src="../UserControls/Reserve/wucUploadADR060.ascx" TagName="wucUploadADR060" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/Reserve/wucUploadReject.ascx" TagName="wucUploadReject" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/Reserve/wucUploadRelease.ascx" TagName="wucUploadRelease" TagPrefix="uc3" %>
<%@ Register Src="../UserControls/Reserve/wucUploadReserve.ascx" TagName="wucUploadReserve" TagPrefix="uc4" %>
<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/jscript">

        $(document).ready(function () {
            //init date pickers
            $("#<%=ReportDate.ClientID %> ").datepicker();
        });

    </script>
    <uc5:wucMessage ID="wucMessage1" runat="server" />
    <br />
    <fieldset>
        <legend>Import Summary</legend>
        <div>
            <div>
                <div>
                    Report Date Summary for
            <asp:TextBox ID="ReportDate" Width="90px" runat="server"></asp:TextBox><asp:Button ID="ApplyReportDate" Text="Apply" runat="server" OnClick="ApplyReportDate_Click" />
                </div>
                <div>
                    <asp:GridView ID="UploadSummary" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                        ShowHeaderWhenEmpty="True"
                        Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        SelectedRowStyle-BackColor="#fffacd">
                        <Columns>
                            <asp:BoundField DataField="UploadName" HeaderText="Report" />
                            <asp:BoundField DataField="RowsImported" HeaderText="Rows Imported" />
                            <asp:BoundField DataField="Filename" HeaderText="File Name" />
                            <asp:BoundField DataField="ReportDates" HeaderText="Report Date(s) Contained" />
                            <asp:BoundField DataField="UploadedBy" HeaderText="Uploaded By" />
                            <asp:BoundField DataField="DateCompleted" HeaderText="Date/Time Complete" />
                        </Columns>
                        <EmptyDataTemplate>
                            No reports uploaded/imported.
                        </EmptyDataTemplate>
                        <AlternatingRowStyle CssClass="alt" />
                        <PagerStyle CssClass="pgr"></PagerStyle>
                        <SelectedRowStyle BackColor="LemonChiffon" />
                        <RowStyle CssClass="realrow" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </fieldset>
    <br />
    <uc3:wucUploadRelease ID="wucUploadRelease1" runat="server" />
    <br />
    <uc4:wucUploadReserve ID="wucUploadReserve1" runat="server" />
    <br />
    <uc2:wucUploadReject ID="wucUploadReject1" runat="server" />
    <br />
    <uc1:wucUploadADR060 ID="wucUploadADR0601" runat="server" />
    <br />
</asp:Content>
