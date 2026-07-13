<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="UserControls_wucDocumentGrid" Codebehind="wucDocumentGrid.ascx.cs" %>
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" FooterStyle-CssClass="footer"
    Font-Names="verdana" Font-Size="X-Small" OnRowDataBound="GridView1_RowDataBound"
    DataKeyNames="ConditionDetailID,DocID">
    <Columns>
        <asp:TemplateField HeaderText="OrigName">
            <ItemTemplate>
                <asp:HyperLink ID="hypFilename" Target="_blank" runat="server" Text='<%# Bind("OrigName") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Approval Status">
            <ItemTemplate>
                <asp:DropDownList runat="server" ID="ApprovalStatusID">
                    <asp:ListItem Value="0">Pending</asp:ListItem>
                    <asp:ListItem Value="1">Approved</asp:ListItem>
                    <asp:ListItem Value="2">Denied</asp:ListItem>
                </asp:DropDownList>
                <asp:HiddenField ID="hidDocID" runat="server" Value='<%# Bind("DocID") %>'></asp:HiddenField>
               <asp:HiddenField ID="hidConditionDetailID" runat="server" Value='<%# Bind("ConditionDetailID") %>'>
                </asp:HiddenField>
                <%-- <asp:HiddenField ID="hidConditionDocumentUID" runat="server" Value='<%# Bind("UID") %>'>
                </asp:HiddenField>--%>
            </ItemTemplate>
            <ItemStyle Width="101px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Comments To Partner">
            <ItemTemplate>
                <asp:TextBox ID="MessageZeus" Width="190px" runat="server" TextMode="MultiLine" Text='<%# Bind("MessageZeus") %>'></asp:TextBox>
            </ItemTemplate>
            <ItemStyle Width="200px" />
        </asp:TemplateField>
        <asp:TemplateField Visible="false" HeaderText="Date Updated">
            <ItemTemplate>
                <%--<asp:Label ID="Label5" runat="server" Text='<%# Bind("DateModified") %>'></asp:Label>--%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField Visible="false" HeaderText="DocTypeName">
            <ItemTemplate>
                <asp:Label ID="Label6" runat="server" Text='<%# Bind("DocTypeName") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date Uploaded">
            <ItemTemplate>
                <asp:Label ID="Label7" runat="server" Text='<%# Bind("DateUploaded") %>'></asp:Label>
            </ItemTemplate>
             <ItemStyle Width="80px" />
        </asp:TemplateField>
    
    </Columns>
    <EmptyDataTemplate>
        No documents have been uploaded for this pending condition.
    </EmptyDataTemplate>
    <FooterStyle CssClass="footer" />
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
</asp:GridView>
