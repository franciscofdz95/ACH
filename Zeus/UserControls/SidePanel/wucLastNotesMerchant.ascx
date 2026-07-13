<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucLastNotesMerchant.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucLastNotesMerchant" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<fieldset style="width: 320px;">
    <legend>Last Notes</legend>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="Records">
                <asp:GridView ID="grdNotes" runat="server" OnRowDataBound="grdNotes_RowDataBound"
                    OnRowCommand="grdNotes_RowCommand" AutoGenerateColumns="False" Font-Names="Verdana"
                    PageSize="4" DataSourceID="odsNotes" OnPageIndexChanging="grdNotes_PageIndexChanging"
                    OnSorting="grdNotes_Sorting" AllowSorting="true" AllowPaging="true" Font-Size="X-Small"
                    CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                    DataKeyNames="Subject,Notes,BusinessDBAName,MerchantAppUID,UID,View_MPSAll,View_Agent,View_Bank,View_Merchant">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnNotesID" runat="server" CausesValidation="false" CommandName="View"
                                    Text='<%#Eval("ID") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes">
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Date" DataField="DateCreated" SortExpression="DateCreated"
                            DataFormatString="{0:MM/dd/yy hh:mm tt}">
                            <ItemStyle Width="67px" Wrap="True" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="User Created" DataField="UserCreated" SortExpression="UserCreated">
                            <ItemStyle Width="60px" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" />
                    <PagerStyle CssClass="pgr" />
                    <AlternatingRowStyle CssClass="alt" />
                </asp:GridView>
                <asp:ObjectDataSource ID="odsNotes" runat="server" SelectMethod="GetRecentMerchantNotesPaging"
                    TypeName="DataMerchantAppPaging" OldValuesParameterFormatString="original_{0}"
                    OnSelecting="odsNotes_Selecting" EnablePaging="true" SelectCountMethod="GetRecentMerchantNotesPagingCount"
                    MaximumRowsParameterName="PageSize" StartRowIndexParameterName="CurrentPage">
                    <SelectParameters>
                        <asp:Parameter Name="prms" Type="Object" />
                        <asp:Parameter Name="CurrentPage" Type="Int32" />
                        <asp:Parameter Name="PageSize" Type="int32" />
                        <asp:Parameter Name="ControlID" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <div style="text-align:right;">
                <asp:HyperLink runat="server"   ID="hypMore">More</asp:HyperLink>
                    </div>
                <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="400px" Width="650px"
                    Modal="True" InitialLocation="Centered" WindowState="Hidden">
                    <ContentPane>
                        <Template>
                            <table>
                                <tr>
                                    <td class="lblRight">
                                        Subject:
                                      </td>
                                    <td>
                                        <asp:TextBox ID="txtSubject" runat="server" Width="500px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight" valign="top">
                                        Notes:
                                      </td>
                                    <td style="font-size:small">
                                       <asp:Panel runat="server" ScrollBars="Auto" Wrap="true" Height="300px" Width="550px" BorderColor="#999999"  BorderStyle="Solid" BorderWidth="1px">
                                            <asp:Label ID="txtNotes" runat="server" TextMode="MultiLine" Height="280px"  BackColor="White" ></asp:Label>
                                       </asp:Panel>
                                        <%--<asp:TextBox ID="txtNotes" runat="server" Height="280px" TextMode="MultiLine" Width="500px"></asp:TextBox>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlNotesCBRow">
                                            <table width="100%">
                                                <tr>
                                                  <%--  <td align="left" valign="top">
                                                        <asp:CheckBox ID="chkCallback" Text="Requires Callback" Style="vertical-align: text-top;"
                                                            runat="server" />
                                                    </td>--%>
                                                    <td align="left" valign="top">
                                                        <asp:CheckBox ID="chkInternal" Text="Access To Internal" Style="vertical-align: text-top;"
                                                            runat="server" />
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <asp:CheckBox ID="chkAgent" Text="Access To Partner" Style="vertical-align: text-top;"
                                                            runat="server" />
                                                    </td>
                                                     <td align="left" valign="top">
                                                        <asp:CheckBox ID="ChkMerchant" Text="Access To Merchant" Style="vertical-align: text-top;"
                                                            runat="server" Enabled="False" />
                                                    </td>
                                                 <%--   <td align="left" valign="top">
                                                        <asp:CheckBox ID="chkBank" Text="Access To Bank" Style="vertical-align: text-top;"
                                                            runat="server" />
                                                    </td>--%>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </Template>
                    </ContentPane>
                    <Header CaptionText="Notes">
                    </Header>
                </ig:WebDialogWindow>
            </asp:Panel>
            <asp:Panel ID="NoRecords" runat="server">
                &nbsp;No Notes..
            </asp:Panel>
            <asp:Panel ID="pnlAddNotes" runat="server">
                <table cellspacing="2" width="100%">
                    <tr>
                        <td align="left" valign="top">
                            <asp:Label runat="server" ID="lblErr" Text="" SkinID="error"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            Note Code<br />
                            <asp:DropDownList ID="cboNoteCode" runat="server" Width="310px" AutoPostBack="true"
                                OnSelectedIndexChanged="cboNoteCode_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp; &nbsp; &nbsp;
                            <asp:CheckBox ID="chkApplySameLegalName" runat="server" Text="Apply note to same legal business name" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            Notes<br />
                            <asp:TextBox ID="Notes" runat="server" Height="65px" TextMode="MultiLine" Width="310px"></asp:TextBox>
                            <asp:HiddenField ID="Subject" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 90px" class="lblRight">
                                        Callback:
                                    </td>
                                    <td align="left" style="width: 90px">
                                        <asp:CheckBox ID="RequiresCallback" runat="server" Width="30px" />
                                    </td>
                                    <td class="lblRight" style="width: 90px">
                                        Internal:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:CheckBox ID="View_MPSAll" runat="server" Width="30px" />
                                    </td>
                                    <td class="lblRight" style="width: 90px">
                                        Agent:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:CheckBox ID="View_Agent" runat="server" Width="30px" />
                                    </td>
                                    <td class="lblRight" style="width: 90px">
                                        Bank:
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:CheckBox ID="View_Bank" runat="server" Width="30px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight" style="width: 121px">
                                        <asp:CheckBox ID="Email_Agent" runat="server" Width="50px" Visible="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                                
                            <asp:LinkButton ID="btnAddNotes" runat="server" Text="Add Notes" CausesValidation="false"
                                OnClick="btnAddNotes_Click" />
                            &nbsp;
                            <asp:LinkButton ID="btnClearNotes" runat="server" Text="Clear Notes" CausesValidation="false"
                                OnClick="btnClearNotes_Click" />&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
