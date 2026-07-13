<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAlertContacts.ascx.cs"
    Inherits="wucAlertContacts" %>
<div class="dialog">
    <asp:Panel ID="pnlAddContact" runat="server" ScrollBars="None">
        
        <div>
            <div>
                <fieldset>
                    <legend> &nbsp;<span>Add a New Contact</span></legend>
                    <table>
                        <tr>
                            <td align="right">
                                First Name:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFirstName" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Last Name:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtLastName" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                Email:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtAddEmail" Width="250px"></asp:TextBox>&nbsp;<asp:Button
                                    runat="server" ID="btnAddContact" Text="Add" OnClick="btnAddContact_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblMessage" EnableViewState="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </div>
        </div>
    </asp:Panel>
    <div style="text-align:right;">
    <asp:Label runat="server" ID="lblCount" EnableViewState="false" Font-Size="X-Small"></asp:Label>&nbsp;&nbsp; &nbsp;&nbsp;
        </div>
    <asp:Panel ID="pnlContacts" runat="server" ScrollBars="Auto" Height="200px">
        <div>
            <fieldset>
                <legend>&nbsp;<asp:Label runat="server" ID="lblContacts" Text="Contacts"></asp:Label></legend>
                <span>Please select a contact to assign to the alert. If no contacts exist, please add a new contact to assign to the alert.</span>
                <asp:GridView ID="grdContacts" AutoGenerateColumns="false" runat="server" AllowSorting="true"
                    AlternatingRowStyle-CssClass="alt" HorizontalAlign="left" OnSorting="grdContacts_Sorting"
                    DataKeyNames="ID, ContactID" CssClass="mGrid" ShowHeader="true" EmptyDataText="No Contacts..."
                    BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="100%"
                    ShowFooter="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Enabled" HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkEnabled" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ContactName" SortExpression="ContactName" HeaderText="Name" HeaderStyle-Width="225px" />
                        <asp:BoundField DataField="EmailAddress" SortExpression="EmailAddress" HeaderText="Email" />
                    </Columns>
                    <PagerStyle CssClass="pgr" />
                    <FooterStyle CssClass="footer" />
                    <PagerSettings Mode="NumericFirstLast" />
                </asp:GridView>
                <asp:ObjectDataSource ID="odsContacts" runat="server" OnSelecting="odsContacts_Selecting"
                    TypeName="DataMerchantAppPaging">
                    <SelectParameters>
                        <asp:Parameter Name="prms" Type="Object" />
                        <asp:Parameter Name="PageSize" Type="Int32" />
                        <asp:Parameter Name="CurrentPage" Type="Int32" />
                        <asp:Parameter Name="ControlID" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </fieldset>
        </div>
    </asp:Panel>
    <br />
    <center>
    <asp:Button ID="lnkOk" CssClass="button" runat="server" Text="OK" OnClick="lnkOk_Click" />
    <asp:Button ID="btnApplyAll" CssClass="button" runat="server" 
            Text="Apply to All Alerts" onclick="btnApplyAll_Click" />
    <input type="button" value="Cancel" onclick="CloseMCC()" />
    </center>
    
</div>


