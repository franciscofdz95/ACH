<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true" Inherits="frmMerchantPCI" Title="Merchant PCI" Codebehind="frmMerchantPCI.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
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
        <table width="100%">
            <tr>
                <td>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
                    <asp:Panel ID="pnlTools" runat="server">
                        <div class="tbrtools">
                            <div class="tbrtoolsleft">
                            
<table>
<tr>
    <td><igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                    AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/edit.png" />
                                    </Appearance>
                                </igtxt:WebImageButton></td>
    <td><igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add"
                                    AccessKey="a" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/add2.png" />
                                    </Appearance>
                                </igtxt:WebImageButton></td>
    <td><igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false"
                                    AccessKey="s" CommandName="Save" OnClick="tbrTools_ButtonClicked">
                                    <Appearance>
                                        <Image Url="~/Images/disk_blue.png" />
                                    </Appearance>
                                </igtxt:WebImageButton></td>
    <td><igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                    AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/disk_blue_error.png" />
                                    </Appearance>
                                </igtxt:WebImageButton></td>
    <td><igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                    AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/refresh.png" />
                                    </Appearance>
                                </igtxt:WebImageButton></td>
</tr>
</table>
                             
                            </div>
                        </div>
                    </asp:Panel>
                    <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                    <br />
                    <br />
                    <div class="title">
                        &nbsp;&nbsp;Merchant PCI History
                        <hr class="line" />
                    </div>
                    <div class="indentedcontent20">
                        <table width="100%">
                            <tr>
                                <td class="lblRight">
                                    <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                                        <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                            Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                            AlternatingRowStyle-CssClass="alt" DataKeyNames="MerchantAppUID" OnRowDataBound="grd_RowDataBound"
                                            OnRowCommand="grd_RowCommand" OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True"
                                            OnSorting="grd_Sorting" DataSourceID="odsTransactions">
                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                            <SelectedRowStyle BackColor="#fffacd" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnZID" runat="server" Text="Select" CommandName="MerchantID"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor Name" SortExpression="VendorName" />
                                                <asp:BoundField DataField="PCIStatus" HeaderText="PCI Status" SortExpression="PCIStatus" />
                                                <asp:BoundField DataField="RequestDate" HeaderText="Sent Date" SortExpression="RequestDate"
                                                    DataFormatString="{0:MM/dd/yyyy}" />
                                                <asp:BoundField DataField="CompletedDate" HeaderText="Completed Date" SortExpression="CompletedDate"
                                                    DataFormatString="{0:MM/dd/yyyy}" />
                                                <asp:BoundField DataField="NCDays" HeaderText="NC Days" SortExpression="NCDays" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="odsTransactions" runat="server" SelectMethod="GetMerchantPCIPaging"
                                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                            SelectCountMethod="GetMerchantPCIPagingRowCount" StartRowIndexParameterName="CurrentPage"
                                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsTransactions_Selecting">
                                            <SelectParameters>
                                                <asp:Parameter Name="prms" Type="Object" />
                                                <asp:Parameter Name="PageSize" Type="Int32" />
                                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                                        No data...
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="pnlDetail" runat="server" Height="" Width="">
                        <br />
                        <div class="title">
                            &nbsp;&nbsp;PCI Information
                            <hr class="line" />
                        </div>
                        <div class="indentedcontent20">
                            <table cellspacing="10">
                                <tr>
                                    <td class="lblRight">
                                        Status:</td>
                                    <td>
                                        <asp:DropDownList ID="StatusUID" runat="server" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Vendor:</td>
                                    <td>
                                        <asp:DropDownList ID="VendorID" runat="server" Width="150px" Enabled="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Sent Date:
                                    </td>
                                    <td>
                                        <ig:WebDatePicker ID="RequestDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                            Width="150px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Completed Date:</td>
                                    <td>
                                        <ig:WebDatePicker ID="CompletedDate" runat="server" EnableAppStyling="False"
                                            NullDateLabel="" Width="150px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
