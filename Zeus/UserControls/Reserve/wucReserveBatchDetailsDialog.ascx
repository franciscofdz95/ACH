<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucReserveBatchDetailsDialog.ascx.cs" Inherits="ZeusWeb.UserControls.Reserve.wucReserveBatchDetailsDialog" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<ig:WebDialogWindow ID="dlgReserveBatchDetails" runat="server"
    Width="500px" Modal="True" 
    InitialLocation="Centered" WindowState="Hidden">
    <ContentPane>
        <Template>
            <asp:Panel runat="server" CssClass="dialog" ID="pnlDetails">
                
                
                        <b>ZID:</b> <asp:Label runat="server" ID="lblZID"></asp:Label><span style="padding:0px 15px;" >&nbsp;</span>
                        <b>MID:</b> <asp:Label runat="server" ID="lblMID"></asp:Label><span style="padding:0px 15px;" >&nbsp;</span>
                        <b>DBA:</b> <asp:Label runat="server" ID="lblDBA"></asp:Label>

                <asp:GridView runat="server" OnRowDataBound="GridView1_RowDataBound" ID="GridView1" AutoGenerateColumns="False"  ShowFooter="True"
                    CssClass="mGrid">
                
                    <Columns>
                        <asp:TemplateField HeaderText="Report Date">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                Total:
                            </FooterTemplate>
                        </asp:TemplateField>


                        <asp:BoundField DataField="BatchNumber" HeaderText="Batch Number" />
                        <asp:BoundField DataField="Card Type" HeaderText="Card Type" />


                          <asp:TemplateField HeaderText="Sales" ItemStyle-HorizontalAlign="Right" >
                            <ItemTemplate>
                                <asp:Label ID="lblSales" runat="server" Text='<%# Bind("Sales", "{0:C2}") %>'></asp:Label>
                            </ItemTemplate>    
                            <FooterTemplate>
                                <asp:Label ID="lblFootSalesTotal" runat="server"></asp:Label>
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Returns" ItemStyle-HorizontalAlign="Right" >
                            <ItemTemplate>
                                <asp:Label ID="lblReturns" runat="server" Text='<%# Bind("Returns", "{0:C2}") %>'></asp:Label>
                            </ItemTemplate>    
                            <FooterTemplate>
                                <asp:Label ID="lblFootReturnsTotal" runat="server"></asp:Label>
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Net" ItemStyle-HorizontalAlign="Right" >
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Amount", "{0:C2}") %>'></asp:Label>
                            </ItemTemplate>    
                            <FooterTemplate>
                                <asp:Label ID="lblFootAmountTotal" runat="server"></asp:Label>
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>

                
                </asp:GridView>

                
            </asp:Panel>
        </Template>
    </ContentPane>
    <Header CaptionText="Reserve Batch Details">
    </Header>
</ig:WebDialogWindow>
