<%@ Page Language="C#" MasterPageFile="~/MasterPageSales.master" AutoEventWireup="true" Inherits="frmAssignLeads" Title="Assign Leads" CodeBehind="frmAssignLeads.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%--<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>--%>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        function SelectAll(chk) {

            $('#grdLeads').find("input:checkbox").each(function () {
                if (this != chk) {
                    this.checked = chk.checked;
                }

            });
        }

        function CloseMCC() {
            oWebDialogWindowAccountgrps = $find('dlgLeadSourceRep');
            oWebDialogWindowAccountgrps.set_windowState($IG.DialogWindowState.Hidden);
        }

    </script>

    <div id="contentpage">
        <asp:UpdatePanel ID="pnl" runat="server">
            <ContentTemplate>
                <div class="dialog" style="padding-right: 10px;">
                    <br />
                    <fieldset>
                        <legend>Source Assignment</legend>
                        <asp:GridView ID="grdSourceAssignment" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                            Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnRowCommand="grdSourceAssignment_RowCommand">
                            <HeaderStyle HorizontalAlign="Center" />
                            <Columns>
                                <asp:BoundField DataField="SourceName" HeaderText="Source"></asp:BoundField>
                                <asp:BoundField DataField="SourceDescription" HeaderText="Description"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkActiveReps" Text='<%# Bind("ActiveRepsCount") %>' CommandArgument='<%# Bind("LeadSourceID") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </fieldset>
                    <br />
                    <fieldset>
                        <legend>Leads Assignment</legend>
                        <table id="TABLE1">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Size="8pt"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">Select a Source:</td>
                                <td>
                                    <asp:DropDownList ID="SourceID" runat="server" Width="250px" AutoPostBack="True"
                                        OnSelectedIndexChanged="SourceID_SelectedIndexChanged">
                                    </asp:DropDownList></td>
                            </tr>                            
                            <tr>
                                <td align="right">Include Assigned Leads:</td>
                                <td>
                                    <asp:CheckBox ID="IsAssigned" runat="server" AutoPostBack="True" OnCheckedChanged="IsAssigned_CheckedChanged" >
                                    </asp:CheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <fieldset>
                                        <legend>Assign To</legend>
                                        <table>
                                            <%--<tr>
                                                <td align="right" colspan="4">
                                                    <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="vertical" IDWidth="212"
                                                        DBAWidth="250" lblDBAWidth="90px" lblIDWidth="90px" />
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td align="right" width="90px">Assign to Rep:</td>
                                                <td>
                                                    <asp:DropDownList ID="AssignedUserID" runat="server" Width="228px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>&nbsp;
                                    <asp:Button ID="btnMassUpdate" runat="server" EnableViewState="False" OnClick="btnMassUpdate_Click"
                                        Text="Assign" Width="65px" AccessKey="u"></asp:Button>
                                                    &nbsp;
                                    <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" AccessKey="l"
                                        EnableViewState="False" Width="65px"></asp:Button>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <br />
                    <fieldset>
                        <legend>Search Results</legend>
                        <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                            <table width="100%">
                                <tr>
                                    <td class="lblLeft"></td>
                                    <td class="lblRight">
                                        <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="grdLeads" runat="server" AutoGenerateColumns="False" Font-Names="Verdana" ClientIDMode="Static"
                                Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                DataKeyNames="LeadID,ID" AllowSorting="true" OnSorting="grdLeads_Sorting" OnRowDataBound="grd_RowDataBound">
                                <HeaderStyle HorizontalAlign="Center" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" runat="server" onclick="javascript:SelectAll(this);" />
                                        </HeaderTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ID" HeaderText="Lead ID" SortExpression="ID">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DBAName" HeaderText="DBA Name" SortExpression="DBAName">
                                        <ItemStyle Width="180px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Agent" DataField="AgentDBA" SortExpression="AgentDBA">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Assigned to">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAssignedTo" Width="100px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Status" HeaderText="Lead Status" SortExpression="Status">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                  <%--  <asp:BoundField DataField="Source" HeaderText="Source" SortExpression="Source">
                                        <ItemStyle Width="60px" />
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="DateCreated" HeaderText="Date Created" SortExpression="DateCreated">
                                        <ItemStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LeadID" Visible="false"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                            No data...
                        </asp:Panel>

                    </fieldset>
                    <br />
                </div>

                <ig:WebDialogWindow ID="dlgLeadSourceRep" runat="server" Height="625px" InitialLocation="Centered" ClientIDMode="Static"
                    Modal="True" Width="800px" WindowState="Hidden">
                    <ContentPane>
                        <Template>
                            <div>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 50%">
                                            <fieldset style="height: 520px;">
                                                <legend>&nbsp;Unassigned Representatives</legend>
                                                <div style="height: 495px; overflow-y: scroll;">
                                                    <asp:GridView ID="grdUnassignedReps" AutoGenerateColumns="false" runat="server"
                                                        AlternatingRowStyle-CssClass="alt" HorizontalAlign="left"
                                                        CssClass="mGrid" ShowHeader="true" EmptyDataText="No Unassigned Representatives"
                                                        BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="325px"
                                                        Style="table-layout: fixed; word-wrap: break-word;">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="20px">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox runat="server" ID="chkEnabled" />
                                                                    <asp:HiddenField runat="server" ID="UserID" Value='<%#Eval("UserID") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="FullName" HeaderText="Name" HeaderStyle-Width="75px" />
                                                            <asp:BoundField DataField="UserName" HeaderText="Login" HeaderStyle-Width="75px" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Button ID="AssignReps" CssClass="button" runat="server" Text=">" OnClick="AssignReps_Click" />
                                            <br />
                                            <br />
                                            <asp:Button ID="UnassignReps" CssClass="button" runat="server" Text="<" OnClick="UnassignReps_Click" />
                                            <br />
                                        </td>
                                        <td style="width: 50%">
                                            <fieldset style="height: 520px;">
                                                <legend>&nbsp;Assigned Representatives</legend>
                                                <div style="height: 495px; overflow-y: scroll;">
                                                    <asp:GridView ID="grdAssignedReps" AutoGenerateColumns="false" runat="server"
                                                        AlternatingRowStyle-CssClass="alt" HorizontalAlign="left"
                                                        CssClass="mGrid" ShowHeader="true" EmptyDataText="No Assigned Representatives"
                                                        BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="325px"
                                                        Style="table-layout: fixed; word-wrap: break-word;">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="20px">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox runat="server" ID="chkEnabled" />
                                                                    <asp:HiddenField runat="server" ID="UserID" Value='<%#Eval("UserID") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="FullName" HeaderText="Name" HeaderStyle-Width="75px" />
                                                            <asp:BoundField DataField="UserName" HeaderText="Login" HeaderStyle-Width="75px" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>

                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <center>
                                <asp:Button runat="server" Text="Apply" ID="Apply" OnClick="Apply_Click"></asp:Button>
                                <input type="button" value="Close" onclick="CloseMCC()" />
                            </center>
                        </Template>
                    </ContentPane>
                </ig:WebDialogWindow>
                <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="150px" InitialLocation="Centered" ClientIDMode="Static"
                    Modal="True" Width="400px" WindowState="Hidden">
                    <ContentPane EnableRelativeLayout="true">
                        <Template>
                            <div style="align-content: center; align-items: center; vertical-align: central; margin-bottom: 25px; margin-top: 25px">
                                <table cellspacing="5" width="100%" align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:Label runat="server" ID="lblError1" Text="" Font-Names="Verdana" Font-Size="X-Small"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button runat="server" Text="Ok" ID="btnOk" OnClick="btnOk_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </Template>
                    </ContentPane>
                    <Header CaptionText="Alert" CloseBox-Visible="false"></Header>
                </ig:WebDialogWindow>



            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
