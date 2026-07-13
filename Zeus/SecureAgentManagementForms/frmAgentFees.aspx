<%@ Page Language="C#" MasterPageFile="~/MasterPageAgent.master" AutoEventWireup="true" Inherits="frmAgentFees" Title="Agent Fee" CodeBehind="frmAgentFees.aspx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ MasterType VirtualPath="~/MasterPageAgent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <asp:Label ID="lblError" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>

        <div class="tbrtools">
            <div class="tbrtoolsleft">
                <table>
                    <tr>
                        <td>
                            <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                <Appearance>
                                    <Image Url="~/Images/edit.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false" CommandName="Save"
                                AccessKey="s" OnClick="tbrTools_ButtonClicked">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue_error.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                    </tr>
                </table>
            </div>

        </div>
        <fieldset>
            <legend>Residual Report items</legend>

            <asp:GridView ID="grdResidualReportItems" AutoGenerateColumns="false" runat="server" Width="600px"
                Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" OnRowDataBound="grdResidualReportItems_RowDataBound"
                DataKeyNames="UID,AgentUID">
                <PagerStyle CssClass="pgr" />
                <AlternatingRowStyle CssClass="alt" />
                <FooterStyle CssClass="footer" />
                <HeaderStyle HorizontalAlign="center" />

                <Columns>
                    <asp:BoundField DataField="Description" HeaderText="Name" ItemStyle-Width="250px"></asp:BoundField>
                    <asp:TemplateField HeaderText="Agent Cost" ItemStyle-HorizontalAlign="right" ItemStyle-Width="140px">
                        <ItemTemplate>
                            <ig:WebNumericEditor ID="WebNumericEdit1" runat="server" HorizontalAlign="Right"
                                BorderStyle="None" DataMode="Decimal" MinDecimalPlaces="5">
                            </ig:WebNumericEditor>
                              <ig:WebPercentEditor ID="WebPercentEdit1" runat="server" HorizontalAlign="Right" MaxValue="100"
                                BorderStyle="None" DataMode="Decimal" MinDecimalPlaces="2" MaxDecimalPlaces="2">
                            </ig:WebPercentEditor>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Merchant Cost" ItemStyle-HorizontalAlign="right" ItemStyle-Width="140px">
                        <ItemTemplate>
                            <ig:WebNumericEditor ID="WebNumericEdit2" runat="server" HorizontalAlign="Right"
                                BorderStyle="None" DataMode="Decimal" MinDecimalPlaces="5">
                            </ig:WebNumericEditor>
                             <ig:WebPercentEditor ID="WebPercentEdit2" runat="server" HorizontalAlign="Right" MaxValue="100"
                                BorderStyle="None" DataMode="Decimal" MinDecimalPlaces="2" MaxDecimalPlaces="2">
                            </ig:WebPercentEditor>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chkEnable" />
                        </ItemTemplate>
                        <ItemStyle Width="70px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="UID" Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="AgentUID" Visible="false" HeaderText="AgentUID"></asp:BoundField>
                </Columns>
            </asp:GridView>

        </fieldset>

    </div>
</asp:Content>
