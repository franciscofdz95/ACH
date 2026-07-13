<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucIPBlocking" Codebehind="wucIPBlocking.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<div style="width: 350px">
    <asp:CheckBox ID="EnableIPBlocking" runat="server" Text="Enable IP Blocking" /><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        IP Address:</td>
                    <td>
                        <asp:TextBox ID="A1" runat="server" Width="35px" MaxLength="3"></asp:TextBox>.
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="A1" ErrorMessage="Octet1 must be between 0 to 255"
                            MaximumValue="255" MinimumValue="0" Type="Integer" Display="None"></asp:RangeValidator></td>
                    <td>
                        <asp:TextBox ID="A2" runat="server" Width="35px" MaxLength="3"></asp:TextBox>.
                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="A2" ErrorMessage="Octet2 must be between 0 to 255"
                            MaximumValue="255" MinimumValue="0" Type="Integer" Display="None"></asp:RangeValidator></td>
                    <td>
                        <asp:TextBox ID="A3" runat="server" Width="35px" MaxLength="3"></asp:TextBox>.
                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="A3" ErrorMessage="Octet3 must be between 0 to 255"
                            MaximumValue="255" MinimumValue="0" Type="Integer" Display="None"></asp:RangeValidator></td>
                    <td>
                        <asp:TextBox ID="A4" runat="server" Width="35px" MaxLength="3"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="A4" ErrorMessage="Octet4 must be between 0 to 255"
                            MaximumValue="255" MinimumValue="0" Type="Integer" Display="None"></asp:RangeValidator></td>
                    <td>
                        <asp:Button ID="btnAddIP" runat="server" Text="Add IP" OnClick="btnAddIP_Click" /></td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                Font-Size="X-Small" CssClass="mGrid" DataSourceID="odsIPBlocking" Width="100%"
                DataKeyNames="IPBlockID">
                <PagerStyle CssClass="pgr" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="IPAddress" HeaderText="IP Address" />
                    <asp:BoundField DataField="IPBlockID" Visible="False" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDelete" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div style="text-align: right;">
                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" /></div>
            <asp:ObjectDataSource ID="odsIPBlocking" runat="server" SelectMethod="GetMerchantIPBlocks"
                TypeName="PaymentXP.DataObjects.DataRisk" OldValuesParameterFormatString="original_{0}"
                OnSelecting="odsIPBlocking_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="prms" Type="Object" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
