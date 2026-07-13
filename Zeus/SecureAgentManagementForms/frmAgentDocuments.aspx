<%@ Page Title="Agent Documents" Language="C#" MasterPageFile="~/MasterPageAgent.master" AutoEventWireup="true" Inherits="frmAgentDocuments" Codebehind="frmAgentDocuments.aspx.cs" %>

<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc2" %>

<%@ MasterType VirtualPath="~/MasterPageAgent.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--PXP-10767 >> RThakur Start--%>
    <script type="text/javascript">

        function confirmAction(chk) {

            var checked = chk.checked;

            if (checked) {
                if (confirm("Are you sure, you want to make the document private?")) {

                    $('#<%=hdnDocuments.ClientID%>').val('True');
                    __doPostBack(chk.id, '');
                    return true;
                }
                else {
                    chk.checked = !checked;
                    $('#<%=hdnDocuments.ClientID%>').val('');
                    return false;
                }
            }
            else {
                if (confirm("Are you sure, you want to remove the document as private?")) {

                    $('#<%=hdnDocuments.ClientID%>').val('False');
                    __doPostBack(chk.id, '');
                    return true;
                }
                else {
                    chk.checked = !checked;
                    $('#<%=hdnDocuments.ClientID%>').val('');
                    return false;
                }
            }

        }

    </script>
    <%--PXP-10767 >> RThakur End--%>
    <div id="contentpage">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
        <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
            <uc2:wucMessage ID="WucMessage1" runat="server" />
            &nbsp;&nbsp;
            <table width="100%">
                <tr>
                    <!-- 
                    <td valign="top">
                        <asp:Panel ID="pnlBarcode" runat="server">
                            <fieldset style="height: 230px">
                                <legend>Print Barcodes</legend>
                                <asp:Panel ID="Panel1" runat="server" Height="140px" Width="100%" ScrollBars="vertical">
                                    <asp:CheckBoxList ID="lstDocumentTypes2" runat="server">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <br />
                                <asp:Button ID="btnPrint" runat="server" Text="Print Barcode" 
                                    Visible="False" />
                                <asp:Button ID="btnPreview" runat="server" Text="Print Selected Doc Barcode" 
                                    Width="200px" />
                                <asp:Button ID="btnPreviewZID" runat="server" Text="Print Merchant Barcode" 
                                    Width="200px" />
                            </fieldset>
                        </asp:Panel>
                    </td>
                    -->
                    <td valign="top">
                        <asp:Panel ID="pnlUpload" runat="server">
                            <fieldset style="height: 230px">
                                <legend>Upload Document</legend>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            Document Type:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="lstDocumentTypes" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Select file:
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="fupDocument" runat="server" Width="500px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Description:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Description" runat="server" MaxLength="255" TextMode="MultiLine"
                                                Width="500px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <br />
                                            <asp:Button ID="btnSubmit" runat="server" Width="90px" Text="Upload" OnClick="btnSubmit_Click"
                                                CausesValidation="false" />
                                                <asp:HiddenField runat="server" ID="AgentUID" />
                                                <asp:HiddenField runat="server" ID="AgentID" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <br />
            <fieldset>
                <legend>Documents</legend>
                <table width="100%">
                    <tr>
                        <td>
                            <div>
                                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
                                <!--<asp:Button ID="btnPrintCUWebites" runat="server" Text="Retrieve CU Websites"  />-->
                                Source:<asp:DropDownList ID="lstDocumentSources" AutoPostBack="true" OnSelectedIndexChanged="lstDocumentSources_SelectedIndexChanged"
                                    runat="server" />
                            </div>
                            <table width="100%">
                                <tr>
                                    <td style="text-align: left">
                                        Page Size:
                                        <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlPageSize" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="50">50</asp:ListItem>
                                            <asp:ListItem Value="100">100</asp:ListItem>
                                            <asp:ListItem Value="250">250</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <span>Total Records Found:
                                            <asp:Literal ID="litRecordCount" runat="server">0</asp:Literal>
                                        </span>
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField runat="server" ID="hdnDocuments" />
                            <asp:GridView ID="grdDocuments" OnPageIndexChanging="grd_PageIndexChanging" DataSourceID="ods"
                                runat="server" AutoGenerateColumns="False" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                PagerStyle-CssClass="pgr" FooterStyle-CssClass="footer" Font-Names="verdana"
                                Font-Size="X-Small" OnRowDataBound="grdDocuments_RowDataBound" OnRowCancelingEdit="grdDocuments_RowCancelingEdit"
                                OnRowEditing="grdDocuments_RowEditing" OnRowUpdating="grdDocuments_RowUpdating"
                                DataKeyNames="DocID" AllowPaging="true">
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Private">
                                      <ItemTemplate>
                                         <asp:CheckBox runat="server" ID="IsPrivate" onclick="return confirmAction(this);" OnCheckedChanged="IsPrivate_CheckedChanged" />
                                      </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DocID" HeaderText="Doc ID" Visible="False" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/pdf.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Document Name">
                                        <ItemTemplate>
                                            <%--<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="#" Target="_blank" Text='<%# Eval("DocNameDisplay") %>'></asp:HyperLink>--%>
                                            <asp:HyperLink ID="hypOrigName" runat="server" Target="_blank" Text='<%# Eval("OrigName") %>'></asp:HyperLink>
                                            <asp:Label ID="lblOrigName" ToolTip="Private Document" runat="server" Text='<%# Eval("OrigName") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Group Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGroupType" Text='<%# Eval("DocTypeGroupName") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="Type" Text='<%# Eval("DocTypeName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddpType">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDescription" Text='<%#Eval("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="txtDescription" Text='<%#Eval("Description") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Source" HeaderText="Source" ReadOnly="True" />
                                    <asp:BoundField DataField="ContentSize" HeaderText="Size" ReadOnly="True" />
                                    <asp:BoundField DataField="DocDate" HeaderText="Document Date" DataFormatString="{0:MM/dd/yyy hh:mm:ss tt}"
                                        ReadOnly="True" />
                                    <asp:BoundField DataField="UserCreated" HeaderText="User Created" ReadOnly="True" ItemStyle-Width="80px" />
                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Width="10px" Style="text-align: center;"
                                                CausesValidation="false">
                                                <asp:Image ID="Img6" runat="server" ImageUrl="../Images/edit.png" ToolTip="Edit"
                                                    ImageAlign="middle" /></asp:LinkButton>
                                            <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update"  Style="text-align: center;"
                                                Width="10px" CausesValidation="true">
                                                <asp:Image ID="Img7" runat="server" ImageUrl="~/Images/save.gif" ToolTip="Update"
                                                    ImageAlign="middle" /></asp:LinkButton>
                                            <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Style="text-align: center;"
                                                CausesValidation="false" Width="10px">
                                                <asp:Image ID="Img8" runat="server" ImageUrl="~/Images/undo.png" ToolTip="Cancel"
                                                    ImageAlign="middle" /></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" Visible="false" runat="server" CommandArgument='<%# Eval("DocID") %>' CommandName="Delete" Style="text-align: center;"
                                                        CausesValidation="false" Width="10px" OnClick="LinkButton1_Click">
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/delete2.png" ToolTip="Delete"
                                                            ImageAlign="middle" />
                                                    </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle CssClass="footer" />
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="ods"  TypeName="DataMerchantAppPaging" runat="server" 
                                SelectMethod="GetDocumentPaging" 
                                SelectCountMethod="GetDocumentPagingCount" 
                                OnSelecting="ods_Selecting"
                                EnablePaging="True"
                                MaximumRowsParameterName="PageSize" 
                                StartRowIndexParameterName="CurrentPage" 
                                OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:Parameter Name="prms" Type="Object" />
                                    <asp:Parameter Name="PageSize" Type="Int32" />
                                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
    </div>
</asp:Content>
