<%@ Page Language="C#" MasterPageFile="~/MasterPageSales.master" AutoEventWireup="true" Inherits="SecureLeadForms_frmCommunications"
    Title="Communications Search" Codebehind="frmCommunications.aspx.cs" %>

<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc3" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <div class="dialog" style="padding-right: 10px;">
            <fieldset>
                <legend>Email Search</legend>
                <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="DBA Name:" EnableViewState="False"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="BusinessDBAName" runat="server" Width="75px" EnableViewState="False"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Subject:" EnableViewState="False"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="Subject" runat="server" EnableViewState="False" Width="75px"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="Body:" EnableViewState="False" Visible="False"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="Body" runat="server" EnableViewState="False" Visible="False" Width="75px"></asp:TextBox>&nbsp;</td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="To:" EnableViewState="False"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="To" runat="server" Width="75px" EnableViewState="False"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="Label2" runat="server" EnableViewState="False" Text="Cc:"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="Cc" runat="server" EnableViewState="False" Width="75px"></asp:TextBox></td>
                            <%--<td>
                                <asp:Label ID="Label5" runat="server" Text="Agent" EnableViewState="False"></asp:Label></td>
                            <td>
                                <asp:DropDownList ID="AgentAgentID" runat="server">
                                </asp:DropDownList>
                            </td>--%>
                            <td colspan="2">
                                <asp:Panel runat="server" ID="AgentSelect">
                                    <uc3:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="horizontal" IDWidth="75"
                                        DBAWidth="75" lblDBAWidth="70" lblIDWidth="93" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <div style="text-align: center;">
                        <br />
                        <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                            AccessKey="h">
                            <Appearance>
                                <Image Url="~/Images/Check.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                        &nbsp;
                        <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                            AccessKey="l">
                            <Appearance>
                                <Image Url="~/Images/delete.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </div>
                </asp:Panel>
            </fieldset>
            <br />
            <fieldset>
                <legend>&nbsp;<b>Search Results</b></legend>
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grdCommunication" AutoGenerateColumns="false" runat="server" OnRowCommand="grdCommunication_RowCommand"
                                AllowPaging="true" PageSize="10" DataKeyNames="LeadID,CommunicationID" AllowSorting="true"
                                Font-Names="Verdana" Font-Size="X-Small" OnPageIndexChanging="grdCommunication_PageIndexChanging"
                                OnRowDataBound="grdCommunication_RowDataBound" OnSorting="grdCommunication_Sorting"
                                CssClass="mGrid">
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                                <FooterStyle CssClass="footer" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" SortExpression="ID">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnMerchantID" runat="server" CommandName="Edit"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="30px"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AgentFullName" SortExpression="AgentFullName" HeaderText="Agent" 
                                        ItemStyle-Width="60px"></asp:BoundField>
                                    <asp:BoundField DataField="DBAName" SortExpression="DBAName" HeaderText="DBA Name"
                                        ItemStyle-Width="100px"></asp:BoundField>
                                    <asp:BoundField DataField="TimeSent" SortExpression="TimeSent" HeaderText="Sent Date"
                                        ItemStyle-Width="40px" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
                                    <asp:BoundField DataField="To" SortExpression="To" HeaderText="To" ItemStyle-Width="100px">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Subject" SortExpression="Subject" HeaderText="Subject"
                                        ItemStyle-Width="150px"></asp:BoundField>
                                    <asp:BoundField DataField="CC" SortExpression="CC" Visible="false"></asp:BoundField>
                                    <asp:BoundField DataField="Body" SortExpression="Body" Visible="false"></asp:BoundField>
                                    <asp:BoundField DataField="DateCreated" SortExpression="DateCreated" HeaderText="Date Created"
                                        ItemStyle-Width="40px" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
                                    <asp:BoundField DataField="UserCreated" SortExpression="UserCreated" HeaderText="User Created"
                                        ItemStyle-Width="60px"></asp:BoundField>
                                    <asp:BoundField DataField="LeadID" Visible="false"></asp:BoundField>
                                    <asp:BoundField DataField="CommunicationID" Visible="false"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <asp:Label runat="server" ID="lblData" Text="No Data..." Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
</asp:Content>
