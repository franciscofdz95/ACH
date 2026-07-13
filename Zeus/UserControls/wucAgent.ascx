<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucAgent" CodeBehind="wucAgent.ascx.cs" %>
<div class="dialog">
    <fieldset>
        <legend>Agent List</legend>
        <asp:Panel runat="server" ID="pnlAgents" DefaultButton="btnSearch">
            <table width="100%">
                <tr>
                    <td class="lblRight">
                        Last Name:
                    </td>
                    <td>
                        <asp:TextBox ID="AgentLastName" runat="server" Width="100px" EnableViewState="False"
                            TabIndex="1"></asp:TextBox>
                    </td>
                    <td class="lblRight">
                        DBA:
                    </td>
                    <td>
                        <asp:TextBox ID="AgentDBA" runat="server" EnableViewState="False" Width="100px" TabIndex="2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        First Name:
                    </td>
                    <td>
                        <asp:TextBox ID="AgentFirstName" runat="server" EnableViewState="False" Width="100px"
                            TabIndex="3"></asp:TextBox>
                    </td>
                    <td class="lblRight">
                        ID:
                    </td>
                    <td>
                        <asp:TextBox ID="AgentID" runat="server" EnableViewState="False" Width="100px" MaxLength="7"
                            TabIndex="4"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        FMAID:
                    </td>
                    <td>
                        <asp:TextBox ID="FMAID" runat="server" Width="100px" EnableViewState="False" onKeyPress ="CheckNumeric()" MaxLength="15" ></asp:TextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="4" style="height: 10px;">
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button runat="server" ID="btnSearch" Text="Search" Width="70px" CausesValidation="false"
                            TabIndex="5" OnClick="btnSearch_Click"></asp:Button>
                        <asp:Button runat="server" ID="btnClose" Text="Close" Width="50px" CausesValidation="false"
                            TabIndex="5" OnClick="btnClose_Click"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                    <tr>
                        <td class="lblRight" colspan="4">
                            <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound"
                                Font-Names="Verdana" Font-Size="X-Small" OnRowCommand="grd_RowCommand" AllowSorting="True"
                                OnSorting="grd_Sorting">
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Agent ID" SortExpression="ID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAgentID" CausesValidation="false" runat="server" CommandName="AgentID"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="65px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AgentFullName" HeaderText="Agent Name" SortExpression="AgentFullName">
                                        <ItemStyle Width="225px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DBA" HeaderText="DBA Name" SortExpression="DBA">
                                        <ItemStyle Width="225px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AgentID" HeaderText="Agent ID" Visible="False" />
                                    
                                    <asp:BoundField DataField="AgentFMAID" HeaderText="FMAID" SortExpression="AgentFMAID">
                                        <ItemStyle Width="45px" />
                                    </asp:BoundField>

                                </Columns>
                                <PagerStyle CssClass="pgr" />
                                <FooterStyle CssClass="footer" />
                                <AlternatingRowStyle CssClass="alt" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsTransactions" runat="server" OnSelecting="odsTransactions_Selecting"
                                EnablePaging="True" MaximumRowsParameterName="PageSize" StartRowIndexParameterName="CurrentPage"
                                TypeName="DataMerchantAppPaging" SelectMethod="GetAgentsPaging" SelectCountMethod="GetAgentsPagingRowCount">
                                <SelectParameters>
                                    <asp:Parameter Name="prms" Type="Object" />
                                    <asp:Parameter Name="PageSize" Type="Int32" />
                                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                                    <asp:Parameter Name="ControlID" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                            No data...
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </fieldset>
</div>
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
