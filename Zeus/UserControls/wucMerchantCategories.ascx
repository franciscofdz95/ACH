<%@ Control Language="C#" AutoEventWireup="true" Inherits="MerchantCategories" CodeBehind="wucMerchantCategories.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<asp:Panel runat="server" ID="Panel1">
    <fieldset>
        <legend>
            <asp:Label runat="server" ID="lblName" Text="Services"></asp:Label></legend>
        <asp:Panel ID="pnlGrd" runat="server" Height="180px" Width="100%">
            <table border="0" cellspacing="2" width="100%">
                <tr>
                    <td style="width: 5px;">
                    </td>
                    <td class="lblLeft" style="font-weight: bold;">
                        Merchant Details:
                    </td>
                    <td class="lblLeft" style="font-weight: bold;">
                        Additional Services/Reports:
                    </td>
                </tr>
                <tr>
                    <td style="width: 5px;">
                    </td>
                    <td valign="top">
                        <asp:GridView ID="grd1" runat="server" OnRowDataBound="grd_RowDataBound" ShowHeader="false"
                            ShowFooter="false" AllowSorting="true" DataSourceID="odsMerchants1" AutoGenerateColumns="false"
                            GridLines="None" DataKeyNames="MerchantServiceID">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkChecked" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        &nbsp;<asp:TextBox runat="server" ID="text" Width="250px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MerchantServiceID" Visible="false"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsMerchants1" runat="server" SelectMethod="GetMerchantServicesList"
                            OnSelecting="odsMerchants1_Selecting" TypeName="PaymentXP.DataObjects.DataMerchantServices"
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                    <td valign="top">
                        <asp:GridView ID="grd2" runat="server" OnRowDataBound="grd_RowDataBound" ShowHeader="false"
                            ShowFooter="false" AllowSorting="true" DataSourceID="odsMerchants2" AutoGenerateColumns="false"
                            GridLines="None" DataKeyNames="MerchantServiceID,Name">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkChecked" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        &nbsp;<asp:TextBox runat="server" ID="text" Width="250px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MerchantServiceID" Visible="false"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsMerchants2" runat="server" SelectMethod="GetMerchantServicesList"
                            OnSelecting="odsMerchants2_Selecting" TypeName="PaymentXP.DataObjects.DataMerchantServices"
                            OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </fieldset>
</asp:Panel>
<asp:Panel runat="server" ID="pnlCategories">
    <fieldset>
        <legend>
            <asp:Label runat="server" ID="Label1" Text="Business Relationships"></asp:Label></legend>
        <asp:UpdatePanel ID="pnlBusiness" runat="server">
            <ContentTemplate>
                <table border="0" cellspacing="2" width="100%">
                    <tr>
                        <td style="width: 5px;">
                        </td>
                        <td valign="top">
                            <asp:GridView ID="grdBusiness" runat="server" AutoGenerateColumns="false" ShowFooter="false"
                                CssClass="mGrid" DataKeyNames="ServiceID,MerchantAppSevicesUID,RelationShipRecordID" OnRowCommand="grdBusiness_RowCommand"
                                OnRowDataBound="grdBusiness_RowDataBound">
                                <AlternatingRowStyle CssClass="alt" />
                                <PagerStyle CssClass="pgr" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                <EmptyDataRowStyle CssClass="EmptyDataRowStyle" />
                                <EmptyDataTemplate>
                                    No Data...
                                </EmptyDataTemplate> 
                                <Columns>
                                    <asp:BoundField HeaderText="Relationship Record ID" DataField="RelationShipRecordID" ReadOnly="true" ItemStyle-Width="60px"/> <%--Row added for PXP-8431 by koshlendra--%>
                                    <asp:BoundField HeaderText="Category" DataField="Service" ReadOnly="true" ItemStyle-Width="200px">
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Details" SortExpression="ContactName">
                                        <ItemStyle Width="200px" />
                                        <ItemTemplate>
                                            <asp:Literal runat="server" Text='<%#Eval("Description") %>' ID="litContactName"></asp:Literal>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" Text='<%#Eval("Description") %>' ID="txtContactName"
                                                Width="230px"></asp:TextBox>
                                            <div style="position:relative;">
                                            <asp:TextBox runat="server" text='<%#Eval("Description") %>' visible="false" id="txtCrmName"
                                                width="230px"></asp:TextBox>
                                                </div>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                     <asp:BoundField DataField="TPPCertifiedFlag" HeaderText="TPP Certified Flag" ReadOnly="True" ItemStyle-Width="40px" />
                                     <asp:BoundField DataField="VendorUID" HeaderText="VendorUID" ReadOnly="True" ItemStyle-Width="40px" visible="false" />
                                     <asp:BoundField DataField="AcceptTransactions" HeaderText="Accept Transactions" ReadOnly="True" ItemStyle-Width="40px" />
                                    <asp:BoundField DataField="Source" HeaderText="Source" ReadOnly="True" ItemStyle-Width="40px" />
                                    <asp:BoundField HeaderText="Created On" DataField="DateCreated" ReadOnly="true" ItemStyle-Width="60px"
                                        DataFormatString="{0:MM/dd/yy hh:mm tt}"></asp:BoundField>
                                    <asp:BoundField HeaderText="Created By" DataField="UserCreated" ReadOnly="true" ItemStyle-Width="80px">
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Updated On" DataField="DateUpdated" ReadOnly="true" ItemStyle-Width="60px"
                                        DataFormatString="{0:MM/dd/yy hh:mm tt}"></asp:BoundField>
                                    <asp:BoundField HeaderText="Updated By" DataField="UserUpdated" ReadOnly="true" ItemStyle-Width="80px">
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>

                                            <asp:ImageButton ID="lnkEdit" ImageUrl="~/images/edit.png" runat="server" CommandName="EditMerchant" 
                                                ToolTip="Edit" CausesValidation="false"></asp:ImageButton>
                                            <asp:ImageButton ID="lnkUpdate" ImageUrl="~/images/disk_blue.png" runat="server"
                                                CommandName="UpdateMerchant" ToolTip="Update" CausesValidation="false"></asp:ImageButton>
                                            <asp:ImageButton ID="lnkCancel" ImageUrl="~/images/Cancel.jpg" runat="server" CommandName="CancelMerchant"
                                                ToolTip="Cancel" CausesValidation="false"></asp:ImageButton>
                                            <asp:ImageButton ID="lnkDelete" ImageUrl="~/images/delete.png" runat="server" CommandName="DeleteMerchant"
                                                ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete the record?');"></asp:ImageButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>                          
                            <br />
                            <div id="div" class="bucketfooter">
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="false" OnClick="lnkAdd_Click">Add More</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <ig:WebDialogWindow ID="WebDialogWindow4" runat="server" Height="230px" Width="500px"
                                Modal="True" InitialLocation="Centered" WindowState="hidden" OnStateChanged="WebDialogWindow1_StateChanged">
                                <AutoPostBackFlags WindowStateChange="On" /><ContentPane><Template><br />
                                    <div style="padding-left:30px">
                                    <asp:Label ID="lblError" runat="server" CssClass="gen_error"></asp:Label>
                                    </div>
                                    <table style="vertical-align: middle;" align="center">
                                        <tr>
                                            <td valign="top" class="lblRight">Category: </td>
                                            <td><asp:DropDownList ID="Category" runat="server" Width="300px" OnSelectedIndexChanged="onCategorylistChanged" AutoPostBack="True"></asp:DropDownList></td>
                                        </tr>
                                        <tr ID="CRMListRow" runat="server" visible="false">
                                            <td valign="top" class="lblRight">CRM List: </td>
                                            <td>
                                                <asp:DropDownList ID="CRMList" runat="server" Width="300px"  OnSelectedIndexChanged="onCRMListChanged" AppendDataBoundItems="true" AutoPostBack="true"><asp:ListItem Text="--Select--" Value="-1"></asp:ListItem></asp:DropDownList>

                                            </td>

                                        </tr>
                                        <tr runat="server" ID="NotesSection">
                                            <td valign="top" ID="CRMLabel" runat="server" class="lblRight">Notes: </td>
                                            <td valign="top" class="lblRight" ID="OtherCRMLabel" runat="server" visible="false">Other CRM/Notes:</td>
                                            <td><asp:TextBox ID="Notes" runat="server" TextMode="MultiLine" Style="width: 300px;
                                                        height: 65px;"> 
                                                </asp:TextBox>

                                            </td>

                                        </tr>
                                        <!--PXP-8638:Sanidhya --->
                                       <tr>                                          
                                           <td colspan="2">
                                               <table style=" margin: 0 auto;">
                                                <tr ID="TppRow" runat="server" visible="false"> 
                                            <td valign="top" class="lblRight">TPP Certified Flag:</td>
                                            <td valign="top" class="lblLeft"><asp:Label ID="tpp_flaglbl" runat="server"></asp:Label></td>
                                        </tr> 
                                        <tr ID="AcceptTransRow" runat="server" visible="false">
                                            <td valign="top" class="lblRight">Accept Transactions:</td>
                                            <td valign="top" class="lblLeft"><asp:Label ID="accept_translbl" runat="server"></asp:Label></td>
                                        </tr>
                                                   </table>
                                           </td>
                                       </tr>
                                        <tr>
                                            <td colspan="2" align="center"><br />
                                                <asp:Button OnClientClick="DisabledButton(this.id)" ID="btnSaveB" runat="server" Text="Save" Width="60px" CausesValidation="false"
                                                        OnClick="btnSaveB_Click" />&nbsp; <asp:Button ID="btnCancelB" runat="server" Text="Cancel" Width="60px" CausesValidation="false"
                                                        OnClick="btnCancelB_Click" />

                                            </td>

                                        </tr>

                                    </table>

                                               </Template>
                                   </ContentPane>
                                <Header CaptionText="Category">
                                </Header>
                            </ig:WebDialogWindow>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Panel>
<script type="text/javascript" src="../js/bootstrap.min.js"></script>
     <script type='text/javascript' src="../js/AutoComplete.js"></script>
     <script type="text/javascript">
         Sys.Application.add_init(application_init);
         function application_init() {
             Sys.Debug.trace("Application.Init");
             var prm = Sys.WebForms.PageRequestManager.getInstance();
             prm.add_pageLoaded(AutoCompleteGrid);
         }
         function AutoCompleteGrid() {
             var ParamArray = [[]];
             fnAutoComplete.init('#' + $("[name*='txtCrmName']").prop("id"), '../ajax/AjaxWebservice.asmx/GetCRMName', 'CRMNameFragment', ParamArray,true);
         }
         function DisabledButton(id) {
             $("#" + id).unbind(event);
             $("#" + id).css({ "background-color": "green","color": "white", 'width': '90px'}).attr('value', 'Processing...');
             $("#" + id).prop("disabled", true);
             <%= GetSubmitPostBack() %>;
         }

     </script>

