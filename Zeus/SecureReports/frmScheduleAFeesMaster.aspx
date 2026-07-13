<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="frmScheduleAFeesMaster" Title="Schedule A Fees Master" CodeBehind="frmScheduleAFeesMaster.aspx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ MasterType VirtualPath="~/MasterPageReports.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
      <script type="text/javascript">
          function hourglass() {
              document.body.style.cursor = "wait";
          }
    </script>
    <div id="contentpage">
        <asp:Label ID="lblError" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>

        <div class="tbrtools">
            <div class="tbrtoolsleft">
                <table>
                    <tr>
                        <td class="lblRight" style="font-weight: bold">
                            Select Schedule A Type :
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlScheduleATypes" runat="server" OnSelectedIndexChanged="ddlScheduleATypes_SelectedIndexChanged" AutoPostBack ="true">
                              <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            </asp:DropDownList>
                        </td>
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
                                AccessKey="s" OnClick="tbrTools_ButtonClicked" CausesValidation="true" >
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
            <legend>Schedule A Fees Master</legend>
            <asp:Label ID="lblSelectedScheduleAType" runat="server" Text="Label"></asp:Label>
            <asp:GridView ID="grdResidualReportItems" AutoGenerateColumns="false" runat="server" Width="600px"
                Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" OnRowDataBound="grdResidualReportItems_RowDataBound"
                DataKeyNames="UID,ScheduleAFeesMasterID">
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
                    <asp:BoundField DataField="ScheduleAFeesMasterID" Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="ScheduleAFeeTypeID" Visible="false" HeaderText="ScheduleATypeID"></asp:BoundField>
                </Columns>
            </asp:GridView>

        </fieldset>

    </div>
</asp:Content>
