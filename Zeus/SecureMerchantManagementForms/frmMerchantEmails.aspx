<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true"
    Inherits="frmMerchantEmails" Title="Merchant Emails" CodeBehind="frmMerchantEmails.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucEmail.ascx" TagName="wucEmail" TagPrefix="uc3" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
            <asp:Panel ID="pnlTools" runat="server">
    </asp:Panel>
        <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
            <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
            <br />
            <fieldset class="dialog">
                <legend>Email List</legend>
                <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                    <table width="100%">
                        <tr>
                            <td class="lblLeft">
                                Page Size:
                                <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                    <asp:ListItem Selected="True">10</asp:ListItem>
                                    <asp:ListItem>25</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                    <asp:ListItem>100</asp:ListItem>
                                    <asp:ListItem>250</asp:ListItem>
                                    <asp:ListItem>500</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="lblRight">
                                <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="grdEmail" runat="server" OnRowCommand="grdEmail_RowCommand" DataKeyNames="CommunicationID"
                                    AutoGenerateColumns="false" Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid"
                                    OnRowDataBound="grdEmail_RowDataBound" DataSourceID="odsEmails" AllowPaging="true"
                                    OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True" OnSorting="grd_Sorting">
                                    <PagerStyle CssClass="pgr" />
                                    <AlternatingRowStyle CssClass="alt" />
                                    <FooterStyle CssClass="footer" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnView" runat="server" CausesValidation="false" CommandName="View"
                                                    Text="View"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject"></asp:BoundField>
                                        <asp:BoundField DataField="Body" HeaderText="Body" HtmlEncode="false" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="CommunicationID" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="To" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="From" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="RecordMode" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="DateCreated" DataFormatString="{0:MM/dd/yy hh:mm tt}" SortExpression="DateCreated"
                                            HeaderText="Date Created"></asp:BoundField>
                                        <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <asp:ObjectDataSource ID="odsEmails" runat="server" SelectMethod="GetCommunicationsPaging"
                                    TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                    SelectCountMethod="GetCommunicationsPagingRowCount" StartRowIndexParameterName="CurrentPage"
                                    OldValuesParameterFormatString="original_{0}" OnSelecting="odsEmails_Selecting">
                                    <SelectParameters>
                                        <asp:Parameter Name="prms" Type="Object" />
                                        <asp:Parameter Name="PageSize" Type="Int32" />
                                        <asp:Parameter Name="CurrentPage" Type="Int32" />
                                        <asp:Parameter Name="ControlID" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="bucketfooter">
                                    <table width="100%">
                                        <tr>
                                            <td align="left" style="width: 33%;">
                                                <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                    <span style="height: 25px; vertical-align: middle;">
                                                        <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                            Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                            </td>
                                            <td align="right">
                                                Export:&nbsp;
                                            </td>
                                            <td align="left">
                                                <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                                    <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                                    <asp:ListItem Value="1">All Pages</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td align="right" style="width: 33%;">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Label runat="server" ID="lblEmail" Text="No Emails..." Visible="false"></asp:Label>
            </fieldset>
            <br />
            <center>
                <%--<asp:Button ID="btnEmail" runat="server" Text="New Email" />
                &nbsp;--%>
                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
            </center>
            <br />
             <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="650px" Width="850px"
                Modal="True" InitialLocation="Centered" WindowState="Hidden">
                <ContentPane>
                    <Template>
                        <uc3:wucEmail ID="WucEmail1" runat="server" />
                    </Template>
                </ContentPane>
                <Header CaptionText="Email">
                </Header>
            </ig:WebDialogWindow>
        </asp:Panel>
    </div>
    <script type="text/javascript">

        function ShowEmail() {
            clearFields();
            oWebDialogWindow2 = $find('<% =WebDialogWindow1.ClientID %>'); oWebDialogWindow2.set_windowState($IG.DialogWindowState.Normal);
            return false;
        }

        function CloseEmail() {
            oWebDialogWindow2 = $find('<% =WebDialogWindow1.ClientID %>'); oWebDialogWindow2.set_windowState($IG.DialogWindowState.Hidden);
        }
    </script>
</asp:Content>
