<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucFinancialScoreBoardGrid"
    CodeBehind="wucUWFinancialScoreCardGrid.ascx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:UpdatePanel ID="pnlCustomerInfo" runat="server">
    <contenttemplate>
         <script type="text/javascript">
             function CopyConfirmation_Click(oButton, oEvent) {
                 var x = confirm("Are you sure to delete the merchant scorecard?");
                 if (!x) {
                     oEvent.cancel = true;
                 }
             }
    </script>

<div Style="padding: 3px 3px 3px 3px;">
<asp:Label runat="server" ID="lblError" SkinID="Error"></asp:Label>
</div>
<asp:Panel ID="pnlEdit" runat="server" Style="padding: 3px 0px;" Height="">
    <table width="100%" cellspacing="5" cellpadding="3">
        <tr>
            <td>Scorecard Name:
           &nbsp;
                <asp:TextBox runat="server" ID="ScoreCardName" TabIndex="400"></asp:TextBox>
                <asp:HiddenField runat="server" ID="ScoreCardID" />
            </td>
            <td>Time Period:
           &nbsp;
                <asp:TextBox runat="server" ID="TimePeriod" TabIndex="402"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="title" style="width: 100%;">
                    <asp:Label Text="Liquidity Ratios" runat="server" ID="lblName" ForeColor="#0a94d6"
                        Font-Bold="true"></asp:Label>
                    <hr class="line" />
                </div>
                <asp:GridView ID="grd1" AutoGenerateColumns="false" runat="server" AllowSorting="true"
                    HorizontalAlign="left" Font-Names="Verdana" Font-Size="X-Small"
                    OnRowDataBound="gvCommon_RowDataBound" ShowHeader="false"
                    BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="100%">
                    <PagerStyle CssClass="pgr" />
                    <AlternatingRowStyle CssClass="alt" />
                    <HeaderStyle HorizontalAlign="center" />
                    <Columns>
                        <asp:BoundField DataField="ScoreCardItemDesc"
                            ItemStyle-Width="200px" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hidScoreCardItemID" Value='<%# Bind("ScoreCardItemID") %>' />
                                <asp:HiddenField runat="server" ID="hidIsRequired" Value='<%# Bind("IsRequired") %>' />
                                <asp:HiddenField runat="server" ID="hidAllowNegative" Value='<%# Bind("AllowNegative") %>' />
                                <asp:HiddenField runat="server" ID="hidEditable" Value='<%# Bind("IsEditable") %>' />
                                <asp:HiddenField runat="server" ID="hidName" Value='<%# Bind("ScoreCardItemDesc") %>' />
                                <asp:Label ID="lblValue" runat="server" Width="80px" Visible="false" style="text-align:right;border:solid 1px gray;background-color:gray;"></asp:Label>
                                <asp:TextBox ID="txtValue" runat="server" Width="80px" Visible="false" style="text-align:right;" TabIndex="404" ></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            </tr>
        <tr>
            <td colspan="2">
                <div class="title" style="width: 100%;">
                    <asp:Label Text="Profitability Ratios" runat="server" ID="Label1" ForeColor="#0a94d6"
                        Font-Bold="true"></asp:Label>
                    <hr class="line" />
                </div>
               <asp:GridView ID="grd2" AutoGenerateColumns="false" runat="server" AllowSorting="true" 
                    HorizontalAlign="left" Font-Names="Verdana" Font-Size="X-Small"
                    OnRowDataBound="gvCommon_RowDataBound" ShowHeader="false"
                    BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="100%">
                    <PagerStyle CssClass="pgr" />
                    <AlternatingRowStyle CssClass="alt" />
                    <HeaderStyle HorizontalAlign="center" />
                    <Columns>
                        <asp:BoundField DataField="ScoreCardItemDesc"
                            ItemStyle-Width="200px" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hidScoreCardItemID" Value='<%# Bind("ScoreCardItemID") %>' />
                                <asp:HiddenField runat="server" ID="hidIsRequired" Value='<%# Bind("IsRequired") %>' />
                                <asp:HiddenField runat="server" ID="hidAllowNegative" Value='<%# Bind("AllowNegative") %>' />
                                <asp:HiddenField runat="server" ID="hidEditable" Value='<%# Bind("IsEditable") %>' />
                                <asp:HiddenField runat="server" ID="hidName" Value='<%# Bind("ScoreCardItemDesc") %>' />
                                 <asp:Label ID="lblValue" runat="server" Width="80px" Visible="false" style="text-align:right;border:solid 1px gray;background-color:gray;"></asp:Label>
                                <asp:TextBox ID="txtValue" runat="server" Width="80px" Visible="false" style="text-align:right;" TabIndex="406" ></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="title" style="width: 100%;">
                    <asp:Label Text="Efficiency Ratios" runat="server" ID="Label2" ForeColor="#0a94d6"
                        Font-Bold="true"></asp:Label>
                    <hr class="line" />
                </div>
               <asp:GridView ID="grd3" AutoGenerateColumns="false" runat="server" AllowSorting="true" 
                    HorizontalAlign="left" Font-Names="Verdana" Font-Size="X-Small"
                    OnRowDataBound="gvCommon_RowDataBound" ShowHeader="false"
                    BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="100%">
                    <PagerStyle CssClass="pgr" />
                    <AlternatingRowStyle CssClass="alt" />
                    <HeaderStyle HorizontalAlign="center" />
                    <Columns>
                        <asp:BoundField DataField="ScoreCardItemDesc"
                            ItemStyle-Width="200px" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hidScoreCardItemID" Value='<%# Bind("ScoreCardItemID") %>' />
                                <asp:HiddenField runat="server" ID="hidIsRequired" Value='<%# Bind("IsRequired") %>' />
                                <asp:HiddenField runat="server" ID="hidAllowNegative" Value='<%# Bind("AllowNegative") %>' />
                                <asp:HiddenField runat="server" ID="hidEditable" Value='<%# Bind("IsEditable") %>' />
                                <asp:HiddenField runat="server" ID="hidName" Value='<%# Bind("ScoreCardItemDesc") %>' />
                                <asp:Label ID="lblValue" runat="server" Width="80px" Visible="false" style="text-align:right;border:solid 1px gray;background-color:gray;"></asp:Label>
                                <asp:TextBox ID="txtValue" runat="server" Width="80px" Visible="false" style="text-align:right;"  TabIndex="408"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
             </tr>
        <tr>
            <td colspan="2">
                <div class="title" style="width: 100%;">
                    <asp:Label Text="Cash Flows" runat="server" ID="Label3" ForeColor="#0a94d6"
                        Font-Bold="true"></asp:Label>
                    <hr class="line" />
                </div>
                <asp:GridView ID="grd4" AutoGenerateColumns="false" runat="server" AllowSorting="true"
                    HorizontalAlign="left" Font-Names="Verdana" Font-Size="X-Small"
                    OnRowDataBound="gvCommon_RowDataBound" ShowHeader="false"
                    BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="100%">
                    <PagerStyle CssClass="pgr" />
                    <AlternatingRowStyle CssClass="alt" />
                    <HeaderStyle HorizontalAlign="center" />
                    <Columns>
                        <asp:BoundField DataField="ScoreCardItemDesc"
                            ItemStyle-Width="200px" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hidScoreCardItemID" Value='<%# Bind("ScoreCardItemID") %>' />
                                <asp:HiddenField runat="server" ID="hidIsRequired" Value='<%# Bind("IsRequired") %>' />
                                <asp:HiddenField runat="server" ID="hidAllowNegative" Value='<%# Bind("AllowNegative") %>' />
                                <asp:HiddenField runat="server" ID="hidEditable" Value='<%# Bind("IsEditable") %>' />
                                <asp:HiddenField runat="server" ID="hidName" Value='<%# Bind("ScoreCardItemDesc") %>' />
                              <asp:Label ID="lblValue" runat="server" Width="80px" Visible="false" style="text-align:right;border:solid 1px gray;background-color:gray;"></asp:Label>
                                <asp:TextBox ID="txtValue" runat="server" Width="80px" Visible="false" style="text-align:right;" TabIndex="409" ></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlTools" runat="server">
    <div class="tbrtools">
        <div class="tbrtoolsleft">
            <table cellspacing="3" cellpadding="3">
                <tr>
                    <td>
                        <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" TabIndex="410"
                            AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                            <Appearance>
                                <Image Url="~/Images/edit.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                    <td>
                        <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add" AccessKey="a" TabIndex="411"
                            OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                            <Appearance>
                                <Image Url="~/Images/add2.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                    <td>
                        <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false" AccessKey="s" TabIndex="412"
                            CommandName="Save" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                            <Appearance>
                                <Image Url="~/Images/disk_blue.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                    <td>
                        <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false" TabIndex="413"
                            AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                            <Appearance>
                                <Image Url="~/Images/disk_blue_error.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                    <td>
                        <igtxt:WebImageButton ID="btnDelete" runat="server" Text="Delete" TabIndex="414"
                            CommandName="Submit" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                            <Appearance>
                                <Image Url="../Images/delete.png" />
                            </Appearance> <ClientSideEvents Click="CopyConfirmation_Click" />
                        </igtxt:WebImageButton>

                    </td>
                </tr>
            </table>
        </div>
    </div>  
</asp:Panel>  
</contenttemplate>
    <triggers>
          <asp:asyncpostbacktrigger controlid="btnSave" eventname="Click" />        
          <asp:postbacktrigger controlid="btnCancel" />
          <asp:postbacktrigger controlid="btnDelete" />
     </triggers>
</asp:UpdatePanel>