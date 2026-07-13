<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucLeads" CodeBehind="wucLeads.ascx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<script type="text/javascript" language="javascript">

    function CheckNumeric() {
        var key;
        key = event.which ? event.which : event.keyCode;
        if ((key >= 48 && key <= 57) || key == 13) {
            event.returnValue = true;
        }
        else {
            alert("Please enter Numeric only");
            event.returnValue = false;
        }
    }

</script>

<div class="dialog">
    <fieldset>
        <legend>Lead Search</legend>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true" ValidationGroup="SearchLeads"
            ShowSummary="false" />
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
            <table width="100%">                
                <tr>
                    <td class="lblRight">DBA Name:
                    </td>
                    <td>
                        <asp:TextBox ID="BusinessDBAName" runat="server" Width="85px" EnableViewState="False"></asp:TextBox>
                    </td>
                    <td class="lblRight">Lead ID:</td>
                    <td>
                        <asp:TextBox ID="LeadID" runat="server" Width="85px" EnableViewState="False"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="LeadID"
                            ErrorMessage="Please enter a valid Lead ID." MaximumValue="100000" ValidationGroup="SearchLeads"
                            MinimumValue="1" Type="Integer" Display="none"></asp:RangeValidator>
                    </td>
                    <td class="lblRight">ZID:</td>
                    <td>
                        <asp:TextBox ID="MerchantID" runat="server" Width="85px" EnableViewState="False"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="MerchantID"
                            ErrorMessage="Please enter a valid ZID." MaximumValue="10000000" MinimumValue="1" ValidationGroup="SearchLeads"
                            Type="Integer" Display="none"></asp:RangeValidator>
                    </td>
                    <td class="lblRight">Source:</td>
                    <td>
                        <asp:DropDownList ID="SourceID" runat="server" Width="85px">
                        </asp:DropDownList></td>                   
                </tr>
                <tr>
                    <td class="lblRight">Agent DBA:</td>
                    <td>
                        <asp:TextBox ID="AgentDBA" runat="server" Width="85px"></asp:TextBox>
                    </td>
                    <td class="lblRight">Agent ID:</td>
                    <td>
                        <asp:TextBox ID="AgentID" runat="server" Width="85px"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="AgentID"
                            ErrorMessage="Please enter a valid Agent ID." MaximumValue="100000" MinimumValue="1"
                            Type="Integer" Display="none" ValidationGroup="SearchLeads"></asp:RangeValidator>
                    </td>
                    <td class="lblRight">Assigned To:
                    </td>
                    <td>
                        <asp:DropDownList ID="AssignedUserID" runat="server" Width="85px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <div style="text-align: center;">
                <br />
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" ValidationGroup="SearchLeads"></asp:Button>&nbsp;
                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false" ValidationGroup="SearchLeads"></asp:Button>
                <br />
                <div style="clear: both;">
                </div>
            </div>
        </asp:Panel>
    </fieldset>
    <fieldset style="width: 98%;">
        <legend>Search Results</legend>
        <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
            &nbsp; No Leads...
        </asp:Panel>
        <asp:Panel ID="pnlRecords" runat="server" Height="" Width="">
            <table width="100%">
                <tr>
                    <td class="lblLeft">Page Size:
                                                            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                                <asp:ListItem Selected="True">10</asp:ListItem>
                                                                <asp:ListItem>15</asp:ListItem>
                                                                <asp:ListItem>20</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>50</asp:ListItem>
                                                            </asp:DropDownList></td>
                    <td class="lblRight">
                        <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblError" SkinID="error"></asp:Label>
                        <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                            AlternatingRowStyle-CssClass="alt" DataKeyNames="LeadID,ID"
                            OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True" OnRowCommand="grd_RowCommand"
                            OnSorting="grd_Sorting" DataSourceID="odsLeads">
                            <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                            <Columns>
                                <asp:TemplateField HeaderText="Lead ID" SortExpression="ID">
                                    <ItemTemplate>
                                        <asp:LinkButton CausesValidation="false" CommandArgument='<%# Eval("LeadID") %>' runat="server" ID="hypLeadID" Text='<%# Eval("ID") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="10px" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="DBA Name" SortExpression="DBAName" DataField="DBAName">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Agent ID" DataField="AgentID" SortExpression="AgentID">
                                    <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Agent" DataField="AgentDBA" SortExpression="AgentDBA">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Assigned to" SortExpression="AssignedUser" DataField="AssignedUser">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                            </Columns>
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsLeads" runat="server" SelectMethod="GetLeadsPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetLeadsPagingRowCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsLeads_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                                <asp:Parameter Name="ControlID" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <hr />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
    </fieldset>
</div>
