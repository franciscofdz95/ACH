<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucSelectAgent" Codebehind="wucSelectAgent.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:UpdatePanel ID="WebAsyncRefreshPanel1" runat="server">
    <ContentTemplate>
        <div class="dialog">
            <fieldset>
                <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                    <table width="100%" align="center">
                        <tr>
                            <td class="lblRight">
                                ID:</td>
                            <td>
                                <asp:TextBox ID="ID" runat="server" EnableViewState="False" Width="150px"></asp:TextBox></td>
                            <td class="lblRight">
                                DBA:</td>
                            <td>
                                <asp:TextBox ID="DBA" runat="server" Width="150px" EnableViewState="False"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="lblRight">
                                Last Name:</td>
                            <td>
                                <asp:TextBox ID="LastName" runat="server" EnableViewState="False" Width="150px"></asp:TextBox></td>
                            <td class="lblRight">
                                First Name:</td>
                            <td>
                                <asp:TextBox ID="FirstName" runat="server" EnableViewState="False" Width="150px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <center>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                        CausesValidation="False" />&nbsp;<asp:Button ID="btnReset" runat="server" Text="Reset"
                                            OnClick="btnReset_Click" CausesValidation="False" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </fieldset>
            <br />
            <fieldset>
                <legend>Search Results</legend>
                <div class="lblRight">
                    <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label></div>
                <asp:GridView ID="grd" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                    DataKeyNames="AgentID" Font-Size="X-Small" Font-Names="verdana" OnRowDataBound="grd_RowDataBound">
                    <AlternatingRowStyle CssClass="alt" />
                    <PagerStyle CssClass="pgr" />
                    <FooterStyle CssClass="footer" />
                    <Columns>
                        <asp:BoundField DataField="AgentID" Visible="false" HeaderText="Partner ID"></asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnSelect" Text="Select" CommandName="Select" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="ZID"></asp:BoundField>
                        <asp:BoundField DataField="DBA" HeaderText="DBA"></asp:BoundField>
                        <asp:BoundField DataField="FirstName" HeaderText="First Name"></asp:BoundField>
                        <asp:BoundField DataField="LastName" HeaderText="Last Name"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </fieldset>
            <br />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">

    function Field2Str(fieldvalue)
    {
        if (fieldvalue == null)
        return '';
        else
        return fieldvalue;
    }

    function ShowHookTableSelectAgent(id,name,uid)
    {
         doc = document;
        
        if (doc.getElementById('<% =this.HookTableDBAClientID %>') != null)
            doc.getElementById('<% =this.HookTableDBAClientID %>').value = name;
        if (doc.getElementById('<% =this.HookTableIDClientID %>') != null)
            doc.getElementById('<% =this.HookTableIDClientID %>').value = id;            
        if (doc.getElementById('<% =this.HookTableUIDClientID %>') != null)
            doc.getElementById('<% =this.HookTableUIDClientID %>').value = uid;   

        CloseDialogMerchant('<% =this.WebDialogWindowClientID %>');
    }

//            
//        if (doc.getElementById('<% =this.HookTableDBAClientID %>') != null)
//            doc.getElementById('<% =this.HookTableDBAClientID %>').value = Field2Str(row.getCellFromKey('BusinessDBAName').getValue());

//        if (doc.getElementById('<% =this.HookTableIDClientID %>') != null)
//            doc.getElementById('<% =this.HookTableIDClientID %>').value = Field2Str(row.getCellFromKey('ID').getValue());
//            
//        if (doc.getElementById('<% =this.HookTableUIDClientID %>') != null)
//            doc.getElementById('<% =this.HookTableUIDClientID %>').value = Field2Str(row.getCellFromKey('UID').getValue());
          

    function CloseDialogAgent(dialog)
    {
        oWebDialogWindow1 = $find(dialog);oWebDialogWindow1.set_windowState($IG.DialogWindowState.Hidden); 
        
    }
</script>

