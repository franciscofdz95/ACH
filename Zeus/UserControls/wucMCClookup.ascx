<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMCClookup.ascx.cs" Inherits="wucMCClookup" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt"%>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<asp:UpdatePanel ID="UpdatePanel3" runat="server">
    <ContentTemplate>
        <div style="margin-bottom: 3px;">
            <asp:Label ID="lblSicCode" runat="server" Text="Non-Visa MCC:" CssClass="lblRight"></asp:Label>
            <asp:TextBox ID="txtSicCode" runat="server"></asp:TextBox>
            &nbsp;<asp:Button ID="btnLookup" runat="server" Text="Lookup MCC" Font-Size="Smaller" OnClick="btnLookup_Click" />
        </div>
        <div>
            <asp:Label ID="lblSicCodeDesc" runat="server" Text="MCC Description:" CssClass="lblRight"></asp:Label>
            <asp:TextBox ID="txtSicCodeDesc" runat="server" Width="275px" ReadOnly="true"></asp:TextBox>
            <asp:HiddenField ID="SicCodeDesc" runat="server" />
        </div >
         <div style="margin-bottom: 3px; margin-top:3px;" >
            <asp:Label ID="lblVisaSicCode" runat="server" Text="Visa MCC:" CssClass="lblRight"></asp:Label>
            <asp:TextBox ID="txtVisaSicCode" runat="server"></asp:TextBox>
            &nbsp;<asp:Button ID="btnVisaLookup" runat="server" Text="Lookup MCC" Font-Size="Smaller" OnClick="btnLookup_Click" />
              <asp:HiddenField ID="VisaMCC" runat="server" />
        </div>
        <div >
            <asp:Label ID="lblVisaSicCodeDesc" runat="server" Text="MCC Description:" CssClass="lblRight"></asp:Label>
            <asp:TextBox ID="txtVisaSicCodeDesc" runat="server" Width="275px" ReadOnly="true"></asp:TextBox>
            <asp:HiddenField ID="VisaSicCodeDesc" runat="server" />
        </div>
        <div runat="server" id="pnlNutra" visible="false" >
            <%--PXP-12436: Start by Rohit Thakur--%>
            <asp:Label ID="Label1" runat="server" Text="Nutra Trial:" CssClass="lblRight" Width="150px"></asp:Label>                        
            <%--PXP-12436: End by Rohit Thakur--%>
            <asp:CheckBox ClientIDMode="Static" ID="IsNutraMerchant" runat="server" />&nbsp;
        </div>
        <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="500px" Width="650px"
            InitialLocation="Centered" Modal="True" WindowState="Hidden">
            <ContentPane>
                <Template>
                    <div class="tabcontent">
                        <fieldset>
                            <legend>MCC Search</legend>
                            <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                                <table width="100%">
                                    <tr>
                                        <td class="lblRight">
                                             <asp:Label ID="lblMCC" runat="server" Text="MCC:" CssClass="lblRight"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMCC" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="lblRight">MCC Description:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMCCDesc" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <center>
                                <asp:Button ID="btnSearch" runat="server" CausesValidation="False" OnClick="btnSearch_Click" Text="Search" />
                                <asp:Button ID="btnReset" runat="server" CausesValidation="False" OnClick="btnReset_Click" Text="Reset" /></center>
                            </asp:Panel>
                        </fieldset>
                        <fieldset>
                            <legend>Search Results</legend>
                            <asp:GridView ID="grd" runat="server" OnRowCommand="grd_RowCommand" AutoGenerateColumns="False" CssClass="mGrid"
                                Font-Names="verdana" Font-Size="X-Small" OnRowDataBound="grd_RowDataBound">
                                <AlternatingRowStyle CssClass="alt" />
                                <PagerStyle CssClass="pgr" />
                                <FooterStyle CssClass="footer" />
                                <Columns>
                                    <asp:BoundField DataField="UID" Visible="false" HeaderText="UID"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" runat="server" CommandName="Select" Text="Select"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCode" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsRestricted">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRestrictedIndustry" runat="server" Text='<%#  Convert.ToBoolean(Eval("IsRestrictedIndustry")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Label ID="lblNoRecords" runat="server" Text="No data..."></asp:Label>
                        </fieldset>
                    </div>
                </Template>
            </ContentPane>
            <Header CaptionText="MCC Lookup">
            </Header>
        </ig:WebDialogWindow>
    </ContentTemplate>
</asp:UpdatePanel>
