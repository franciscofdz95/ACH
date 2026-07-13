<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucQIRlookup.ascx.cs" Inherits="ZeusWeb.UserControls.wucQIRlookup" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<asp:UpdatePanel ID="UpQIRlookup" runat="server">
    <ContentTemplate>
        <div style="margin-bottom: 3px;">
            <asp:Label ID="lblQIRID" runat="server" Text="QIRID:" CssClass="lblRight" Visible="false"></asp:Label>
            <asp:TextBox ID="txtQIRCompany" runat="server" Width="275px" ReadOnly="true"></asp:TextBox>
            
            <asp:TextBox ID="txtQIRVID" runat="server" Visible="false"></asp:TextBox>
            &nbsp;<asp:Button ID="btnLookup" runat="server" Text="Lookup QIR" Font-Size="Smaller" OnClick="btnLookup_Click" />
            <asp:HiddenField ID="QIRVendorID" runat="server" />
        </div>        
         <div>
            <asp:Label ID="lblQIRCompany" runat="server" Text=" Company:" CssClass="lblRight" Visible="false"></asp:Label>
            
        </div>
        <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="500px" Width="650px"
            InitialLocation="Centered" Modal="True" WindowState="Hidden">
            <ContentPane>
                <Template>
                    <div class="tabcontent">
                        <fieldset>
                            <legend>QIR Search</legend>
                            <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                                <table  style= "width: 100%;">
                                    <tr>
                                        <td align="right" style="padding-left: 20px;">QIR Vendor:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQIR" runat="server"></asp:TextBox>
                                        </td>                                        
                                    </tr>
                                </table>
                                <br />
                                <center>
                                                                            <asp:Button ID="btnSearch" runat="server" CausesValidation="False" OnClick="btnSearch_Click"
                                                                                Text="Search" />
                                                                            <asp:Button ID="btnReset" runat="server" CausesValidation="False" OnClick="btnReset_Click"
                                                                                Text="Reset" /></center>
                            </asp:Panel>
                        </fieldset>
                        <fieldset>
                            <legend>Search Results</legend>
                            <asp:GridView ID="grdQIR" runat="server" OnRowCommand="grdQIR_RowCommand" AutoGenerateColumns="False" CssClass="mGrid"
                                Font-Names="verdana" Font-Size="X-Small" DataKeyNames="QIRVendorID">                                 
                                <AlternatingRowStyle CssClass="alt" />
                                <PagerStyle CssClass="pgr" />
                                <FooterStyle CssClass="footer" />
                                <Columns>
                                    <asp:BoundField DataField="QIRVendorID" Visible="false" HeaderText="QIRVendorID"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" runat="server" CommandName="Select" Text="Select"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="QIR Vendor ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("QIRVendorID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Company">
                                        <ItemTemplate>
                                            <asp:Label ID="lblcompany" runat="server" Text='<%# Bind("Company") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstatus" runat="server" Text='<%#  Convert.ToBoolean(Eval("IsActive")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Label ID="lblNoRecords" runat="server" Text="No data..."></asp:Label>
                        </fieldset>
                    </div>
                </Template>
            </ContentPane>
            <Header CaptionText="QIR Lookup">
            </Header>
        </ig:WebDialogWindow>
    </ContentTemplate>
</asp:UpdatePanel>