<%@ Control Language="C#" AutoEventWireup="true" Inherits="LeadCategories" CodeBehind="wucLeadCategories.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<asp:Panel runat="server" ID="Panel1">
    <fieldset>
        <legend>
            <asp:Label runat="server" ID="lblName" Text="Services"></asp:Label></legend>
        <asp:Panel ID="pnlGrd" runat="server" Height="180px" Width="100%">
            <table border="0" cellspacing="2" width="100%">
                <tr>
                    <td style="width: 5px;">
                    </td>
                    <td class="lblLeft" style="font-weight: bold;">
                        Merchant Details:
                    </td>
                    <td class="lblLeft" style="font-weight: bold;">
                        Additional Services/Reports:
                    </td>
                </tr>
                <tr>
                    <td style="width: 5px;">
                    </td>
                    <td valign="top">
                        <asp:GridView ID="grd1" runat="server" OnRowDataBound="grd_RowDataBound" ShowHeader="false"
                            ShowFooter="false" AllowSorting="true" DataSourceID="odsLeads1" AutoGenerateColumns="false"
                            GridLines="None" DataKeyNames="LeadServiceID">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkChecked" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        &nbsp;<asp:TextBox runat="server" ID="text" Width="250px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="LeadServiceID" Visible="false"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsLeads1" runat="server" SelectMethod="GetLeadServices"
                            OnSelecting="odsLeads1_Selecting" TypeName="PaymentXP.DataObjects.DataLeadServices"
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                    <td valign="top">
                        <asp:GridView ID="grd2" runat="server" OnRowDataBound="grd_RowDataBound" ShowHeader="false"
                            ShowFooter="false" AllowSorting="true" DataSourceID="odsLeads2" AutoGenerateColumns="false"
                            GridLines="None" DataKeyNames="LeadServiceID,Name">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkChecked" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        &nbsp;<asp:TextBox runat="server" ID="text" Width="250px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="LeadServiceID" Visible="false"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsLeads2" runat="server" SelectMethod="GetLeadServices"
                            OnSelecting="odsLeads2_Selecting" TypeName="PaymentXP.DataObjects.DataLeadServices"
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </fieldset>
</asp:Panel>
<asp:Panel runat="server" ID="pnlCategories">
    <fieldset>
        <legend>
            <asp:Label runat="server" ID="Label1" Text="Business Relationships"></asp:Label></legend>
        <asp:UpdatePanel ID="pnlBusiness" runat="server">
            <ContentTemplate>
                <table border="0" cellspacing="2" width="100%">
                    <tr>
                        <td style="width: 5px;">
                        </td>
                        <td valign="top">
                            <asp:GridView ID="grdBusiness" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                CssClass="mGrid" DataKeyNames="ServiceID,LeadsServicesUID" OnRowCommand="grdBusiness_RowCommand"
                                OnRowDataBound="grdBusiness_RowDataBound">
                                <AlternatingRowStyle CssClass="alt" />
                                <PagerStyle CssClass="pgr" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                <Columns>
                                    <asp:BoundField HeaderText="Category" DataField="Service" ReadOnly="true" ItemStyle-Width="200px">
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Details" SortExpression="ContactName">
                                        <ItemStyle Width="200px" />
                                        <ItemTemplate>
                                            <asp:Literal runat="server" Text='<%#Eval("Description") %>' ID="litContactName"></asp:Literal>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" Text='<%#Eval("Description") %>' ID="txtContactName"
                                                Width="230px"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Created On" DataField="DateCreated" ReadOnly="true" ItemStyle-Width="60px"
                                        DataFormatString="{0:MM/dd/yy hh:mm tt}"></asp:BoundField>
                                    <asp:BoundField HeaderText="Created By" DataField="UserCreated" ReadOnly="true" ItemStyle-Width="80px">
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Updated On" DataField="DateUpdated" ReadOnly="true" ItemStyle-Width="60px"
                                        DataFormatString="{0:MM/dd/yy hh:mm tt}"></asp:BoundField>
                                    <asp:BoundField HeaderText="Updated By" DataField="UserUpdated" ReadOnly="true" ItemStyle-Width="80px">
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lnkEdit" ImageUrl="~/images/edit.png" runat="server" CommandName="EditLead"
                                                ToolTip="Edit" CausesValidation="false"></asp:ImageButton>
                                            <asp:ImageButton ID="lnkUpdate" ImageUrl="~/images/disk_blue.png" runat="server"
                                                CommandName="UpdateLead" ToolTip="Update" CausesValidation="false"></asp:ImageButton>
                                            <asp:ImageButton ID="lnkCancel" ImageUrl="~/images/Cancel.jpg" runat="server" CommandName="CancelLead"
                                                ToolTip="Cancel" CausesValidation="false"></asp:ImageButton>
                                            <asp:ImageButton ID="lnkDelete" ImageUrl="~/images/delete.png" runat="server" CommandName="DeleteLead"
                                                ToolTip="Delete" CausesValidation="false"></asp:ImageButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                CssClass="mGrid">
                                <AlternatingRowStyle CssClass="alt" />
                                <PagerStyle CssClass="pgr" />
                                <Columns>
                                    <asp:BoundField HeaderText="Category"></asp:BoundField>
                                    <asp:BoundField HeaderText="Details"></asp:BoundField>
                                    <asp:BoundField HeaderText="Created On"></asp:BoundField>
                                    <asp:BoundField HeaderText="Created By"></asp:BoundField>
                                    <asp:BoundField HeaderText="Updated On"></asp:BoundField>
                                    <asp:BoundField HeaderText="Updated By"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <div id="div" class="bucketfooter">
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="false" OnClick="lnkAdd_Click">Add More</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <ig:WebDialogWindow ID="WebDialogWindow4" runat="server" Height="230px" Width="500px"
                                Modal="True" InitialLocation="Centered" WindowState="hidden">
                                <ContentPane>
                                    <Template>
                                        <br />
                                        <table style="vertical-align: middle;" align="center">
                                            <tr>
                                                <td valign="top" class="lblRight">
                                                    Category:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="Category" runat="server" Width="300px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" class="lblRight">
                                                    Notes:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="Notes" runat="server" TextMode="MultiLine" Style="width: 300px;
                                                        height: 65px;">
                                                    </asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <br />
                                                    <asp:Button ID="btnSaveB" runat="server" Text="Save" Width="60px" CausesValidation="false"
                                                        OnClick="btnSaveB_Click" />&nbsp;
                                                    <asp:Button ID="btnCancelB" runat="server" Text="Cancel" Width="60px" CausesValidation="false"
                                                        OnClick="btnCancelB_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </Template>
                                </ContentPane>
                                <Header CaptionText="Category">
                                </Header>
                            </ig:WebDialogWindow>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Panel>
