<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucMerchantScoreBoards"
    CodeBehind="wucUWMerchantScoreCards.ascx.cs" %>

<table style="width: 100%">
            <tr>
                <td class="lblLeft" style="vertical-align: bottom">Page Size:
                    <asp:DropDownList ID="cboPageSize2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize2_SelectedIndexChanged">
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem Selected="True">10</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                        <asp:ListItem>250</asp:ListItem>
                        <asp:ListItem>500</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="lblRight">Total Records Found:
                    <asp:Label ID="lblRowCount" runat="server" Text="">0</asp:Label>
                </td>
            </tr>
        </table>
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PageSize="10" 
    OnRowCommand="GridView1_RowCommand" AllowPaging="true" AllowSorting="true" OnSorting="GridView1_Sorting"
    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" FooterStyle-CssClass="footer" OnPageIndexChanging="GridView1_PageIndexChanging"
    Font-Names="verdana" Font-Size="X-Small" OnRowDataBound="GridView1_RowDataBound" DataKeyNames="ScoreCardID">
     <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
    <Columns>
        <asp:BoundField DataField="TimePeriod" HeaderText="TimePeriod" SortExpression="TimePeriod" />
        <asp:TemplateField HeaderText="Card Name" SortExpression="ScoreCardName">
            <ItemTemplate>
                <asp:Label ID="lblName" runat="server" Text='<%# Bind("ScoreCardName") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="UserCreated" HeaderText="Created By" SortExpression="UserCreated" />
        <asp:BoundField DataField="DateCreated" DataFormatString="{0:MM/dd/yyy}" HeaderText="DateCreated" />       
        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbView" CommandName="view" CommandArgument='<%# Eval("ScoreCardID")+"_"+Eval("ScoreCardName")+"_"+Eval("TimePeriod") %>'>View</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
       No Records Found..
    </EmptyDataTemplate>
    <FooterStyle CssClass="footer" />
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
</asp:GridView>
<asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetMerchantScoreCardPaging"
    EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetMerchantScoreCardPagingCount"
    StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
    OnSelecting="ods_Selecting" TypeName="DataMerchantAppPaging">
    <SelectParameters>
        <asp:Parameter Name="prms" Type="Object" />
        <asp:Parameter Name="PageSize" Type="Int32" />
        <asp:Parameter Name="CurrentPage" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>

