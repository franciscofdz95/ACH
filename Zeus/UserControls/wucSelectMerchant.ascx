<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucSelectMerchant" CodeBehind="wucSelectMerchant.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
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

    function checkValue() {
        var zid = $("#<%= ZID.ClientID %>").val();

        if (zid > 2147483640) {
            alert("Please enter a valid ZID");
            event.returnValue = false;
            return false;
        }

        event.returnValue = true;
        return true;
    }

</script>
<asp:UpdatePanel ID="WebAsyncRefreshPanel1" runat="server">
    <contenttemplate>
        <div class="dialog">
            <fieldset>
                <legend>Merchant Search</legend>
                <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                    <table width="100%" align="center">
                        <tr>
                            <td class="lblRight">
                                ZID:</td>
                            <td>
                                <asp:TextBox ID="ZID" runat="server" EnableViewState="False" Width="150px"></asp:TextBox>
                            </td>
                            <td class="lblRight">
                                DBA:</td>
                            <td>
                                <asp:TextBox ID="DBA" runat="server" Width="150px" EnableViewState="False"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="lblRight">
                                MID:</td>
                            <td>
                                <asp:TextBox ID="MID" runat="server" EnableViewState="False" Width="150px"></asp:TextBox></td>
                            <td class="lblRight">
                                MLE:</td>
                            <td>
                                <asp:TextBox ID="LegalName" runat="server" EnableViewState="False" Width="150px"></asp:TextBox></td>
                        </tr>
                        <tr>                            
                         <td class="lblRight">
                                        Contact:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessContact" runat="server" EnableViewState="False" Width="150px"></asp:TextBox>
                                    </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="4">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="checkValue()"
                                    CausesValidation="False" />&nbsp;<asp:Button ID="btnReset" runat="server" Text="Reset"
                                        OnClick="btnReset_Click" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
           <fieldset>
                        <legend>Search Results</legend>
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
                                        <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                           <asp:GridView ID="grd" runat="server" AutoGenerateColumns="false" CssClass="mGrid" AllowPaging="true"  OnPageIndexChanging="grd_PageIndexChanging" 
                                            DataKeyNames="MerchantAppUID" Font-Size="X-Small" Font-Names="verdana" OnRowDataBound="grdMer_RowDataBound" AllowSorting="True" OnSorting="grd_Sorting" DataSourceID="odsMerchants">
                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;" LastPageText="&raquo;" />
                                            <AlternatingRowStyle CssClass="alt" />
                                            <PagerStyle CssClass="pgr" />
                                            <FooterStyle CssClass="footer" />
                                            <EmptyDataRowStyle BorderColor="White" BorderWidth="0" BorderStyle="None" Width="200px" />
                                            <EmptyDataTemplate>
                                                No Records Found...
                                            </EmptyDataTemplate> 
                                            <Columns>
                                                <asp:BoundField DataField="MerchantAppUID" Visible="false" HeaderText="MerchantAppUID">
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnSelect" Text="Select" CommandName="Select" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ID" HeaderText="ZID" SortExpression="ID"></asp:BoundField>
                                                <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name" SortExpression="BusinessDBAName"></asp:BoundField>
                                                <asp:BoundField DataField="Bank" HeaderText="Bank" SortExpression="Bank"></asp:BoundField>
                                                <asp:BoundField DataField="SettlePlatformMid" HeaderText="MID" Visible="false"></asp:BoundField>
                                                <asp:BoundField DataField="AgentFullName" HeaderText="Partner" SortExpression="AgentFullName" ></asp:BoundField>
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
                                    </td>
                                </tr>                              
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                            No data...
                        </asp:Panel>
                    </fieldset>
            <br />
        </div>
    </contenttemplate>
</asp:UpdatePanel>

<script type="text/javascript">

    function Field2Str(fieldvalue) {
        if (fieldvalue == null)
            return '';
        else
            return fieldvalue;
    }

    function CloseDialogMerchant(dialog) {
        oWebDialogWindow1 = $find(dialog); oWebDialogWindow1.set_windowState($IG.DialogWindowState.Hidden);
    }

    function ShowHookTableSelectedMerchant(id, name, uid) {
        doc = document;

        if (doc.getElementById('<% =this.HookTableDBAClientID %>') != null)
            doc.getElementById('<% =this.HookTableDBAClientID %>').value = name;

        if (doc.getElementById('<% =this.HookTableIDClientID %>') != null)
            doc.getElementById('<% =this.HookTableIDClientID %>').value = id;

    if (doc.getElementById('<% =this.HookTableUIDClientID %>') != null)
            doc.getElementById('<% =this.HookTableUIDClientID %>').value = uid;

    CloseDialogMerchant('<% =this.WebDialogWindowClientID %>');
    }

</script>

