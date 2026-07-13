<%@ Page Language="C#" MasterPageFile="~/MasterPageAgent.master" AutoEventWireup="true"
    Inherits="frmMerchantSplits" Title="Merchant Splits" CodeBehind="frmMerchantSplits.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2">
                    <div class="tbrtools">
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
                                        CommandName="Save" OnClick="tbrTools_ButtonClicked">
                                        <Appearance>
                                            <Image Url="~/Images/disk_blue.png" />
                                        </Appearance>
                                    </igtxt:WebImageButton>
                                </td>
                                <td>
                                    <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                        AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                        <Appearance>
                                            <Image Url="~/Images/disk_blue_error.png" />
                                        </Appearance>
                                    </igtxt:WebImageButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td valign="top" style="width: 55%;">
                    <fieldset>
                        <legend>Merchant List</legend>
                        <asp:Panel ID="pnlGrid" runat="server" Width="500px">
                            <table>
                                <tr>
                                    <td>ZID
                                    </td>
                                    <td>
                                        <asp:TextBox ID="MerchantID" runat="server" Width="90px"></asp:TextBox>
                                    </td>
                                    <td>DBA
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessDBAName" runat="server" EnableViewState="False" Width="90px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CausesValidation="true" OnClick="btnSearch_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" CausesValidation="false" OnClick="btnClear_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%">
                                <tr>
                                     <td class="lblLeft">Page Size:
                                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                                SelectedRowStyle-BackColor="#fffacd" Width="483px" Font-Names="Verdana" Font-Size="X-Small"
                                DataKeyNames="MerchantAppUID" OnRowCommand="grd_RowCommand" DataSourceID="odsMerchants"
                                AllowPaging="True" OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True" OnSorting="grd_Sorting">
                                <HeaderStyle HorizontalAlign="Center" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                    LastPageText="&raquo;" />
                                <FooterStyle CssClass="footer" />
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ZID" SortExpression="ID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnMerchantID" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container.DataItem,"ID")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false" HeaderText="ACHID" SortExpression="ACHID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnACHID" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ACHID")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA" SortExpression="BUSINESSDBANAME"></asp:BoundField>
                                    <asp:BoundField DataField="Bank" HeaderText="Bank" SortExpression="BANK"></asp:BoundField>
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="STATUS"></asp:BoundField>
                                    <asp:TemplateField HeaderText="HR" ItemStyle-HorizontalAlign="center" SortExpression="HIGHRISK">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkRisk" Enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem,"HighRisk")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MerchantAppUID" Visible="false" HeaderText="MerchantAppUID"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsMerchants" runat="server" SelectMethod="GetMerchantAppsPaging"
                                TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                SelectCountMethod="GetMerchantAppsPagingRowCount" StartRowIndexParameterName="CurrentPage"
                                OldValuesParameterFormatString="original_{0}" OnSelecting="odsMerchants_Selecting">
                                <SelectParameters>
                                    <asp:Parameter Name="prms" Type="Object" />
                                    <asp:Parameter Name="PageSize" Type="Int32" />
                                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                                    <asp:Parameter Name="ControlID" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:Label runat="server" Text="no data.." ID="lblRecords" Visible="false"></asp:Label>
                        </asp:Panel>
                    </fieldset>
                </td>
                <td valign="top" align="left">
                    <asp:UpdatePanel runat="server" ID="pnl">
                        <ContentTemplate>
                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                            <fieldset>
                                <legend>Splits</legend>
                                <br />
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" Style="text-align: left;"></asp:ValidationSummary>
                                DBA Name:
                                <asp:Label ID="lblDBA" runat="server" Text=""></asp:Label><br />
                                &nbsp;&nbsp;&nbsp;&nbsp; ZID:
                                <asp:Label ID="lblID" runat="server" Text=""></asp:Label><br />
                                <br />
                                <asp:GridView ID="grdContracts" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                                    ShowFooter="true"
                                    OnRowDataBound="grdContracts_RowDataBound"
                                    Font-Names="Verdana" Font-Size="X-Small" DataKeyNames="UID">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <FooterStyle CssClass="footer" />
                                    <PagerStyle CssClass="pgr" />
                                    <AlternatingRowStyle CssClass="alt" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                        LastPageText="&raquo;" />
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID" FooterText="Total:"></asp:BoundField>
                                        <asp:BoundField DataField="DBA" HeaderText="DBA" ItemStyle-Width="150px"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Splits %" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:TextBox Text='<%# Convert.ToDecimal(DataBinder.Eval(Container.DataItem,"Rate")).ToString("0.00")%>'
                                                    runat="server" ID="txtRate" Style="text-align: right;"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="regexpName" runat="server" ErrorMessage="Enter valid data"
                                                    ControlToValidate="txtRate" Display="Dynamic" Text="*" ValidationExpression="^[0-9]*[.]{0,1}[0-9]{0,4}$" />
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="UID" Visible="false" HeaderText="UID"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <script language="javascript" type="text/javascript">
            var IsPostBack = '<%=IsPostBack.ToString() %>';
            window.onload = function () {
                var strCook = document.cookie;
                if (strCook.indexOf("!~") != 0) {
                    var intS = strCook.indexOf("!~");
                    var intE = strCook.indexOf("~!");
                    var strPos = strCook.substring(intS + 2, intE);
                    if (IsPostBack == 'True') {
                        if (document.getElementById("<%=pnlGrid.ClientID %>") != null) {
                            document.getElementById("<%=pnlGrid.ClientID %>").scrollTop = strPos;
                        }
                    }
                    else {
                        document.cookie = "yPos=!~0~!";
                    }
                }
            }
            function SetDivPosition() {
                var intY = 0;

                if (document.getElementById("<%=pnlGrid.ClientID %>") != null) {
                    intY = document.getElementById("<%=pnlGrid.ClientID %>").scrollTop;
                }
                document.cookie = "yPos=!~" + intY + "~!";
            }
        </script>
    </div>
</asp:Content>
