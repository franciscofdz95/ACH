<%@ Page Language="C#" MasterPageFile="~/MasterPageSales.master" AutoEventWireup="true"
    ValidateRequest="false" Inherits="frmNotes" Title="Notes Search" CodeBehind="frmNotes.aspx.cs" %>

<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%--<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <div class="dialog" style="padding-right: 10px;">
            <fieldset>
                <legend>Notes Search</legend>
                <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                    <table width="80%">
                        <tr>
                            <td class="lblRight">
                                <asp:Label ID="Label1" runat="server" Text="DBA Name:" EnableViewState="False"></asp:Label>
                            </td>
                            <td class="lblLeft">
                                <asp:TextBox ID="DBAName" runat="server" Width="75px"></asp:TextBox>
                            </td>
                            <td class="lblRight">
                                <asp:Label ID="Label13" runat="server" Text="Notes:" EnableViewState="False"></asp:Label>
                            </td>
                            <td class="lblLeft">
                                <asp:TextBox ID="Notes" runat="server" Width="100px" EnableViewState="False"></asp:TextBox>
                            </td>
                            <td class="lblLeft">
                                <%--<asp:DropDownList ID="AgentAgentID" runat="server" Width="300px">
                                </asp:DropDownList>
                                <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="AgentAgentID"
                                    PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                                    IsSorted="true" QueryPattern="Contains">
                                </cc1:ListSearchExtender>--%>
                                <asp:Panel runat="server" ID="AgentSelect">
                                    <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="horizontal"
                                        IDWidth="80" DBAWidth="80" lblDBAWidth="72px" lblIDWidth="58px" />
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" align="center">
                                <br />
                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                    AccessKey="h"></asp:Button>
                                &nbsp;
                                <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" AccessKey="l">
                                </asp:Button>
                                <%--<igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
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
                            </igtxt:WebImageButton>--%>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
            <br />
            <fieldset>
                <legend>Search Results</legend>
                <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                    <table width="100%">
                        <tr>
                            <td>
                            </td>
                            <td class="lblRight">
                                <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server"></asp:Label>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="grdLeadNotes" AllowPaging="true" runat="server" OnRowDataBound="grd_RowDataBound"
                                    OnSorting="grd_Sorting" OnPageIndexChanging="grd_PageIndexChanging" OnRowCommand="grd_RowCommand"
                                    Width="100%" AllowSorting="true" DataSourceID="odsNotes" DataKeyNames="LeadID,DBAName"
                                    AutoGenerateColumns="false" CssClass="mGrid">
                                    <AlternatingRowStyle CssClass="alt" />
                                    <PagerStyle CssClass="pgr" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" SortExpression="ID">
                                            <ItemTemplate>
                                                <asp:HyperLink NavigateUrl='<%# String.Format("~/SecureLeadForms/frmLeadsDetail.aspx?Adding=false&LeadUID={0}", Eval("LeadID"))  %>'
                                                    runat="server" ID="hypLeadID" Text='<%# Eval("ID") %>'></asp:HyperLink>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AgentFullName" SortExpression="AgentFullName" HeaderText="Agent">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DBAName" SortExpression="DBAName" HeaderText="DBA Name">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Notes" SortExpression="Notes" HeaderText="Notes"></asp:BoundField>
                                        <asp:BoundField DataField="DateCreated" SortExpression="DateCreated" HeaderText="Date Created">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="UserCreated" SortExpression="UserCreated" HeaderText="User Created">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LeadID" Visible="false"></asp:BoundField>
                                        <asp:BoundField DataField="LeadNotesID" Visible="false"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <asp:ObjectDataSource ID="odsNotes" runat="server" SelectMethod="GetLeadNotesPaging"
                                    EnablePaging="true" MaximumRowsParameterName="PageSize" TypeName="PaymentXP.DataObjects.DataLeadNotes"
                                    OldValuesParameterFormatString="original_{0}" SelectCountMethod="GetLeadNotesPagingCount"
                                    StartRowIndexParameterName="CurrentPage" OnSelecting="odsNotes_Selecting">
                                    <SelectParameters>
                                        <asp:Parameter Name="prms" Type="Object" />
                                        <asp:Parameter Name="PageSize" Type="Int32" />
                                        <asp:Parameter Name="CurrentPage" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblLeft">
                            </td>
                            <td align="right">
                                Page Size:
                                <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                    <asp:ListItem Selected="True">10</asp:ListItem>
                                    <asp:ListItem>15</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>25</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                    No data...
                </asp:Panel>
            </fieldset>
        </div>
    </div>
</asp:Content>
