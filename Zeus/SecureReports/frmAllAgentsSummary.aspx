<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="frmAllAgentsSummary" MasterPageFile="~/MasterPageReports.master" Codebehind="frmAllAgentsSummary.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div class="dialog">
                    <fieldset>
                        <legend>Call Summary Report (Executive Team's View)</legend>
                        <br />
                        <table>
                            <tr>
                                <th class="lblRight">
                                    Start Time:</th>
                                <td>
                                    <ig:WebDatePicker ID="StartDateTime" runat="server" NullDateLabel="" EnableAppStyling="False"
                                        Width="110px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <th class="lblRight">
                                    End Time:</th>
                                <td>
                                    <ig:WebDatePicker ID="EndDateTime" runat="server" NullDateLabel="" EnableAppStyling="False"
                                        Width="110px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                </td>
                            </tr>
                            <tr>
                                <th class="lblRight">
                                    Dept:</th>
                                <td>
                                    <asp:DropDownList ID="Dept" runat="server" Width="110px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <br />
                                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                                    <asp:Button runat="server" ID="btnClear" Text="Clear" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Panel runat="server" ID="pnlgrd">
                            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="True" Font-Names="Verdana"
                                DataSourceID="odsReports" ShowFooter="true" Font-Size="X-Small" CssClass="mGrid"
                                PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grd_RowDataBound"
                                FooterStyle-CssClass="footer">
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsReports" runat="server" SelectMethod="GetExecutiveSummaryReport"
                                TypeName="PaymentXP.DataObjects.DataAgent" OnSelecting="odsReports_Selecting">
                                <SelectParameters>
                                    <asp:Parameter Name="prms" Type="Object" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <div class="bucketfooter">
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 33%;">
                                            <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                <span style="height: 25px; vertical-align: middle;">
                                                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                        Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <asp:Label runat="server" ID="lblData" Text="No Data..." Visible="false"></asp:Label>
                    </fieldset>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
