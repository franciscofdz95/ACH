<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="SecureReports_frmMeritusAlerts"
    Title="Paysafe Alerts" Codebehind="frmMeritusAlerts.aspx.cs" %>

<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/wucMeritusAlertsDetail.ascx" TagName="wucMeritusAlertsDetail"
    TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage" style="padding: 5px;">
        <asp:Panel runat="server" ID="pnlToolbar" Visible="false" CssClass="tbrtools">
            <div class="tbrtoolsleft">
                <table>
                    <tr>
                        <td>
                            <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                <Appearance>
                                    <Image Url="~/Images/edit.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false" AccessKey="s"
                                CausesValidation="False" CommandName="Save" OnClick="tbrTools_ButtonClicked"
                                ClickOnEnterKey="false" ClickOnSpaceKey="false" TabIndex="123">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue.png" />
                                </Appearance>
                                <ClientSideEvents Click="btnSave_Click" />
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False"
                                TabIndex="124">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue_error.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False" TabIndex="125">
                                <Appearance>
                                    <Image Url="~/Images/refresh.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <asp:HyperLink runat="server" Visible="false" Style="line-height: 32px; height: 32px;"
                                NavigateUrl="~/SecureReports/frmMeritusAlerts.aspx" ID="hypBack">Back To Search</asp:HyperLink></td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <div style="clear:both"><!-- --></div>
        <uc2:wucMessage ID="WucMessage1" runat="server"></uc2:wucMessage>
        <asp:Panel runat="server" ID="pnlEdit">
            <fieldset>
                <legend>Edit Alert</legend>
                <uc1:wucMeritusAlertsDetail ID="WucMeritusAlertsDetail1" runat="server" />
            </fieldset>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlSearch">
            <fieldset>
                <legend>Search Paysafe Alerts</legend>
                <div class="MAGrid">
                    <div class="myblock">
                        <div class="mylabel">
                            Portal:</div>
                        <div class="myinput">
                            <asp:DropDownList ID="PortalUID" runat="server">
                            </asp:DropDownList></div>
                    </div>
                    <div class="myblock">
                        <div class="mylabel">
                            AlertID:</div>
                        <div class="myinput">
                            <asp:TextBox runat="server" ID="AlertID"></asp:TextBox></div>
                    </div>
                    <div class="myblock">
                        <div class="mylabel">
                            Subject:</div>
                        <div class="myinput">
                            <asp:TextBox runat="server" ID="Subject"></asp:TextBox></div>
                    </div>
                    <div class="myblock">
                        <div class="mylabel">
                            Content:</div>
                        <div class="myinput">
                            <asp:TextBox runat="server" ID="HTMLContent"></asp:TextBox></div>
                    </div>
                    <div class="myblock">
                        <div class="mylabel">
                            Active:</div>
                        <div class="myinput">
                            <asp:DropDownList runat="server" ID="Active">
                                <asp:ListItem Value="">Show All</asp:ListItem>
                                <asp:ListItem Value="active">Show Active Only</asp:ListItem>
                                <asp:ListItem Value="inactive">Show InActive Only</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="clear: both;">
                        <!-- -->
                    </div>
                    <div style="text-align: center; padding-top: 5px;">
                        <br />
                        <table style="width:300px; margin:0px auto;">
                            <tr>
                                <td>
                                    <igtxt:WebImageButton ID="WebImageButton1" runat="server" OnClick="btnSearch_Click"
                                        Text="Search" AccessKey="h">
                                        <Appearance>
                                            <Image Url="~/Images/Check.png" />
                                        </Appearance>
                                    </igtxt:WebImageButton>
                                </td>
                                <td>
                                    <igtxt:WebImageButton ID="WebImageButton2" runat="server" OnClick="btnClear_Click"
                                        Text="Clear" CausesValidation="false" AccessKey="l">
                                        <Appearance>
                                            <Image Url="~/Images/delete.png" />
                                        </Appearance>
                                    </igtxt:WebImageButton>
                                </td>
                                <td>
                                    <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add" AccessKey="a"
                                        OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                        <Appearance>
                                            <Image Url="~/Images/add2.png" />
                                        </Appearance>
                                    </igtxt:WebImageButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </fieldset>
            <fieldset>
                <legend>Search Results</legend>
                <asp:Panel runat="server" ID="pnlRecords">
                    <table width="100%">
                        <tr>
                            <td class="lblLeft">
                                Page Size:
                                <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                    <asp:ListItem Selected="True">10</asp:ListItem>
                                    <asp:ListItem>15</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>25</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="lblRight">
                                Total Records Found:
                                <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="0"></asp:Label></td>
                        </tr>
                    </table>
                    <asp:GridView runat="server" ID="gvMeritusAlerts" OnRowCommand="gvMeritusAlerts_RowCommand"
                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnSorting="gvMeritusAlerts_Sorting"
                        CssClass="mGrid" AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True"
                        OnPageIndexChanging="GridView1_PageIndexChanging" DataSourceID="odsMertiusAlerts"
                        OnRowDataBound="gvMeritusAlerts_RowDataBound">
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Edit">Details</asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hidPortalUID" Value='<%# Bind("PortalUID") %>' />
                                    <asp:HiddenField runat="server" ID="hidAlertUID" Value='<%# Bind("UID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Portal" SortExpression="PortalName">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("PortalName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AlertID" SortExpression="AlertID">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("AlertID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject" SortExpression="Subject">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("Subject") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Content">
                                <ItemTemplate>
                                    <asp:Label ID="lblHtmlContent" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Alert Date" SortExpression="AlertDate">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("AlertDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Disabled Date" SortExpression="DisabledDate">
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("DisabledDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Exclude Private Labels" SortExpression="ExcludePrivateLabels">
                                <ItemTemplate>
                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("ExcludePrivateLabels") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource runat="server" ID="odsMertiusAlerts" EnablePaging="true" MaximumRowsParameterName="PageSize"
                        StartRowIndexParameterName="CurrentPage" OnSelecting="odsMertiusAlerts_Selecting"
                        SelectCountMethod="SelectMeritusAlerts_PagingCount" SelectMethod="SelectMeritusAlerts_Paging"
                        TypeName="DataMerchantAppPaging">
                        <SelectParameters>
                            <asp:Parameter Name="prms" Type="Object" />
                            <asp:Parameter Name="PageSize" Type="Int32" />
                            <asp:Parameter Name="CurrentPage" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlNoRecords">
                    No Alerts Found.
                </asp:Panel>
            </fieldset>
        </asp:Panel>
    </div>
</asp:Content>
