<%@ Page Language="C#" MasterPageFile="~/MasterPageAgent.master" AutoEventWireup="true" Inherits="frmAgentBanks" Title="Agent Banks" Codebehind="frmAgentBanks.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%--<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <table width="100%">
            <tr>
                <td>
                    <div class="tbrtools" style="display: none;">
                        <div class="tbrtoolsleft">
                            <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                <Appearance>
                                    <Image Url="~/Images/edit.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                            &nbsp;<igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false"
                                AccessKey="s" CommandName="Save" OnClick="tbrTools_ButtonClicked">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                            &nbsp;<igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue_error.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </div>
                      
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Label runat="server" ID="lblError" CssClass="ErrorLabel"></asp:Label>
                            <fieldset style="width: 300px">
                                <legend>Banks</legend>
                                <asp:GridView ID="grdBanks" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                    Font-Names="verdana" DataKeyNames="RecordID,FileID,AgentBankFileID,MerchantAppTypeUID,IsDefault" Font-Size="X-Small" OnRowDataBound="grdBanks_RowDataBound">
                                    <PagerStyle CssClass="pgr" />
                                    <AlternatingRowStyle CssClass="alt" />
                                    <FooterStyle CssClass="footer" />
                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                    <Columns>                                        
                                        <asp:BoundField DataField="Bank" HeaderText="Bank"></asp:BoundField>
                                        <asp:BoundField DataField="FileName" HeaderText="Application"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Enable">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="chkEnabled_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
