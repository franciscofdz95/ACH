<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucLeadsQueue" Codebehind="wucLeadsQueue.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<fieldset>
    <legend>
        <asp:Label ID="lblTitle" runat="server" Text="Label"></asp:Label></legend>
    <asp:UpdatePanel runat="server" ID="pnlDate">
        <ContentTemplate>
            <asp:Panel ID="pnlRecords" runat="server" Height="" Width="">
                <table width="100%">
                    <tr>
                        <td class="lblLeft">
                            Page Size:
                            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                <asp:ListItem Selected="true">10</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                            </asp:DropDownList></td>
                        <td class="lblRight">
                            <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label></td>
                    </tr>
                </table>
                <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                    AlternatingRowStyle-CssClass="alt" DataKeyNames="LeadID" OnRowDataBound="grd_RowDataBound"
                    OnRowCommand="grd_RowCommand" OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True"
                    OnSorting="grd_Sorting" DataSourceID="odsLeads">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                    <Columns>
                        
                        <asp:TemplateField HeaderText="LID" SortExpression="ID">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnLeadID" runat="server" CommandName="ID"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="10px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DBA Name" SortExpression="DBAName">
                            <ItemStyle Width="80px" />
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("DBAName") %>' ID="lblDBA" Width="99%" Height="99%"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" Text='<%#Eval("DBAName") %>' ID="txtDBA" Width="80px">
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="First Name" SortExpression="ContactName">
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <asp:Literal runat="server" Text='<%#Eval("ContactName") %>' ID="litContactName"></asp:Literal>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" Text='<%#Eval("ContactName") %>' ID="txtContactName"
                                    Width="79px"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Name" SortExpression="ContactLastName">
                            <ItemStyle Width="120px" />
                            <ItemTemplate>
                                <asp:Literal runat="server" Text='<%#Eval("ContactLastName") %>' ID="litContactLastName"></asp:Literal>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" Text='<%#Eval("ContactLastName") %>' ID="txtContactLastName"
                                    Width="79px"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Agent DBA" SortExpression="AgentDBA">
                            <ItemStyle Width="80px" />
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("AgentDBA") %>' ID="lblAgentDBA"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phone" SortExpression="PhoneNumber">
                            <ItemStyle Width="80px" />
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("PhoneNumber") %>' ID="lblPhone" Width="80px"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox runat="server" Text='<%#Eval("PhoneNumber") %>' ID="txtPhone" Width="80px">
                                </asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="State" SortExpression="State">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("State") %>' ID="lblState"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Assigned to" SortExpression="AssignedUser">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%#Eval("AssignedUser") %>' ID="lblAgent" Width="100px"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="ddpAgent" Width="100px">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Source" HeaderText="Source" SortExpression="Source" ItemStyle-Width="90px"/>
                        <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:MM-dd-yy hh:mm tt}"
                            ReadOnly="True" SortExpression="DateCreated">
                            <ItemStyle Width="65px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LeadID" Visible="False" />
                        
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
            </asp:Panel>
            <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="">
                No data...
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
