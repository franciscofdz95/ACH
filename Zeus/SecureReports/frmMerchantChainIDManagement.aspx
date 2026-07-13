<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMerchantChainIDManagement.aspx.cs" Inherits="frmMerchantChainIDManagement" MasterPageFile="~/MasterPageReports.master" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControls/wucSelectMerchant.ascx" TagName="wucSelectMerchant"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">    
    <div class="dialog">
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
            <div class="title">
                &nbsp;&nbsp;Chain ID Management
                <hr class="line" />
            </div>
            <asp:ValidationSummary runat="server" ID="ValidSummary"  CssClass="errorlist"/>
            &nbsp;<br />
            <uc7:wucMessage ID="WucMessage1" runat="server" />
            <table cellspacing="2">
                <tr>
                    <td class="lblRight">Chain ID:
                    </td>
                    <td colspan="3">
                        <asp:TextBox runat="server" ID="txtChainID"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtChainID"
                            Display="None" ErrorMessage="Invalid ChainID" MaximumValue="99999" MinimumValue="1"
                            Type="Integer"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">ZID:
                    </td>
                    <td colspan="3">
                        <asp:TextBox runat="server" ID="txtZID"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtZID"
                            Display="None" ErrorMessage="Invalid ZID" MaximumValue="99999" MinimumValue="1"
                            Type="Integer"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">Description:
                    </td>
                    <td colspan="3">
                        <asp:TextBox runat="server" ID="txtDesc"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                            AccessKey="h">
                            <Appearance>
                                <Image Url="~/Images/Check.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                    <td>
                        <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                            CausesValidation="False" AccessKey="l">
                            <Appearance>
                                <Image Url="~/Images/delete.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                    <td>
                        <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add New Chain" CommandName="Add" CausesValidation="False"
                            AccessKey="a" OnClick="btnAddChain_Click">
                            <Appearance>
                                <Image Url="~/Images/add2.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <div class="title">
            &nbsp;&nbsp;Search Results
            <hr class="line" />
        </div>
        <div runat="server" ID="lblData" >
            &nbsp;&nbsp;&nbsp;No Records Found...<br /><br />
        </div>
        <table width="70%">
            <asp:Panel runat="server" ID="pnlRecords" Visible="false">
                <tr>
                    <td class="lblLeft">Page Size:
                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                            <asp:ListItem Selected="True">5</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>40</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">
                        <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView runat="server" ID="grd" AutoGenerateColumns="false" CssClass="mGrid"
                            OnSorting="grd_Sorting" OnRowDataBound="grd_RowDataBound" AllowPaging="true" DataKeyNames="ChainID"
                            OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="true" OnRowCommand="grd_RowCommand">
                            <RowStyle VerticalAlign="Top" />
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <Columns>
                                <asp:TemplateField HeaderText="Chain ID" SortExpression="ChainID">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="hypZID" CausesValidation="false" Text='<%# Eval("ChainID") %>' CommandName="ChainID" CommandArgument='<%# Eval("ChainDescription") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="35px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="ChainDescription" HeaderText="Description" ItemStyle-Width="80px" SortExpression="ChainDescription" />
                                <asp:TemplateField HeaderText="Associations">
                                    <HeaderTemplate>
                                        Associations&nbsp;<a id='lnk1' runat="server" style="display: inline; font-weight: normal; color: #0a94d6; text-decoration: underline; cursor: pointer;">More</a>
                                        <a id='lnk2' runat="server" style="font-weight: normal; display: none; text-decoration: underline; color: #0a94d6; cursor: pointer;">Less</a>
                                    </HeaderTemplate>
                                    <ItemStyle Width="100px" />
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="Notes1" Text='<%# Eval("MerchantIDs") %>'></asp:Label>
                                        <a id='lnk1' runat="server" style="display: none; cursor: pointer;">More</a>
                                        <asp:Label runat="server" ID="Notes2" Style="display: none;"></asp:Label>
                                        <a id='lnk2' runat="server" style="display: none; cursor: pointer;">Less</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Actions" ItemStyle-Width="10px" ItemStyle-HorizontalAlign="left">
                                     <ItemTemplate>
                                         <asp:ImageButton ID="lnkEdit" runat="server" ToolTip="Edit" CommandName="EditID"
                                             CommandArgument='<%#Eval("ChainDescription")%>' ImageUrl="~/Images/edit.png" />
                                     </ItemTemplate>
                                 </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </asp:Panel>           
            <tr>
                <td>
                    <table width="100%">
                        <asp:Panel runat="server" ID="pnlDetails" Visible="false">
                            <tr>
                                <td colspan="4">
                                    <div class="title">
                                        &nbsp;&nbsp;Manage Chain ID 
                                        <hr class="line" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight">Chain ID:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="ChainID" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="lblRight">Chain Description:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="ChainDescription" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Panel ID="pnlAccounts" runat="server" Height="" Width="">
                                        <fieldset>
                                            <legend>Associated Merchants</legend>
                                            <table>
                                                <tr>
                                                    <td>Enter ZID:
                                                        <asp:TextBox ID="ZID" runat="server" MaxLength="5" Width="75px"></asp:TextBox>
                                                        <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="ZID"
                                                            Display="None" ErrorMessage="Invalid ZID" MaximumValue="99999" MinimumValue="1"
                                                            Type="Integer"></asp:RangeValidator>
                                                        <asp:HiddenField ID="HookTableKeyUID" runat="server"></asp:HiddenField>
                                                        <asp:LinkButton ID="btnAddZID" runat="server" OnClick="btnAddZID_Click">Add</asp:LinkButton>
                                                        |
                                                        <asp:LinkButton ID="btnRemove" runat="server" OnClick="btnRemove_Click" CausesValidation="False">Remove</asp:LinkButton>
                                                        |
                                                        <asp:LinkButton ID="btnAddCorporate" runat="server" OnClientClick="ShowHookTable();"
                                                                CausesValidation="False">Lookup Merchant</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lstAccounts" runat="server" Width="350px"></asp:ListBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </asp:Panel>
                                    <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="375px" Width="530px"
                                        Modal="True" InitialLocation="Centered" WindowState="Hidden">
                                        <ContentPane>
                                            <Template>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <uc1:wucSelectMerchant ID="WucSelectMerchant1" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </Template>
                                        </ContentPane>
                                        <Header CaptionText="Merchant Search">
                                        </Header>
                                    </ig:WebDialogWindow>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlNew" Visible="false">
                             <tr>
                                <td colspan="4">
                                    <div class="title">
                                        &nbsp;&nbsp;New Chain ID 
                                        <hr class="line" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight" width="100px">Chain Description:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="ChainDescription1"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="ChainID1" />
                                </td>
                                <td colspan="2"></td>
                            </tr>
                            <tr><td colspan="4">&nbsp</td></tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;&nbsp;<asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click">Save</asp:LinkButton>
                                    |
                                    <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="False">Cancel</asp:LinkButton>
                                </td>
                                <td colspan="2"></td>
                            </tr>
                        </asp:Panel>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <br />
      <script type="text/javascript">

          function ShowHookTable() {
              oWebDialogWindow2 = $find('<% =WebDialogWindow2.ClientID %>'); oWebDialogWindow2.set_windowState($IG.DialogWindowState.Normal);
          }

          function OpenClose(N1, N2, A1, A2, txt) {
              if (txt == 'Open') {
                  document.getElementById(N1).style.display = 'none';
                  document.getElementById(A1).style.display = 'none';
                  document.getElementById(N2).style.display = 'inline';
                  document.getElementById(A2).style.display = 'inline';
              }
              else {
                  document.getElementById(N1).style.display = 'inline';
                  document.getElementById(A1).style.display = 'inline';
                  document.getElementById(N2).style.display = 'none';
                  document.getElementById(A2).style.display = 'none';
              }
          }

          function OpenCloseHeader(A1, A2, txt) {
              if (txt == 'Open') {
                  document.getElementById(A1).style.display = 'none';
                  document.getElementById(A2).style.display = 'inline';
                  HideEvenValueRows('none');
              }
              else {
                  document.getElementById(A1).style.display = 'inline';
                  document.getElementById(A2).style.display = 'none';
                  HideEvenValueRows('inline');
              }
          }

          function HideEvenValueRows(txt) {
              var tGrid = document.getElementById('<%= grd.ClientID%>');
              for (var i = 1; i < tGrid.rows.length; ++i) {
                  var inputs = tGrid.rows[i].getElementsByTagName("a");
                  var labels = tGrid.rows[i].getElementsByTagName("span");

                  for (var j = 0; j < inputs.length; ++j) {
                      if (inputs[j] != null) {
                          var txt2 = inputs[j].id;
                          if (txt2.indexOf('lnk1') != -1)
                              inputs[j].style.display = (txt == 'none') ? 'none' : 'inline';
                          if (txt2.indexOf('lnk2') != -1)
                              inputs[j].style.display = (txt == 'none') ? 'inline' : 'none';
                      }
                  }

                  for (var j = 0; j < labels.length; ++j) {
                      if (labels[j] != null) {
                          var txt1 = labels[j].id;
                          if (txt1.indexOf('Notes1') != -1)
                              labels[j].style.display = (txt == 'none') ? 'none' : 'inline';
                          if (txt1.indexOf('Notes2') != -1)
                              labels[j].style.display = (txt == 'none') ? 'inline' : 'none';
                      }
                  }
              }
          }

      </script>
</asp:Content>
