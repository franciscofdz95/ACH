<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true"
    Inherits="frmMerchantDocuments" Title="Merchant Documents" CodeBehind="frmMerchantDocuments.aspx.cs" %>

<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner">
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
        <asp:Panel ID="pnlTools" runat="server">
        </asp:Panel>
        <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
            <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
            &nbsp;<br />
            <uc2:wucMessage ID="WucMessage1" runat="server" />
            &nbsp;&nbsp;
            <table width="100%">
                <tr>
                    <td valign="top">
                        <asp:Panel ID="pnlBarcode" runat="server">
                            <fieldset style="height: 230px">
                                <legend>Print Barcodes</legend>
                                <asp:Panel ID="Panel1" runat="server" Height="140px" Width="100%" ScrollBars="vertical">
                                    <asp:CheckBoxList ID="lstDocumentTypes2" runat="server">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <br />
                                <asp:Button ID="btnPrint" runat="server" Text="Print Barcode" OnClick="btnPrint_Click"
                                    Visible="False" />
                                <asp:Button ID="btnPreview" runat="server" Text="Print Selected Doc Barcode" OnClick="btnPreview_Click"
                                    Width="200px" />
                                <asp:Button ID="btnPreviewZID" runat="server" Text="Print Merchant Barcode" OnClick="btnPreviewZID_Click"
                                    Width="200px" />
                            </fieldset>
                        </asp:Panel>
                    </td>
                    <td valign="top">
                        <asp:Panel ID="pnlUpload" runat="server">
                            <fieldset>
                                <legend>Upload Document</legend>
                              <%--  <p style="color: black; font-size: 8pt;">
                                    <b>Note: </b>Max file size cannot exceed 80MB.
                                </p>--%>
                                <table>
                                    <tr>
                                        <td>Document Type:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="lstDocumentTypes" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Select file:
                                        </td>
                                        <td>
                                            <div id="mydropzoneid" class="dropzone mydz"></div>
                                            <script type="text/javascript">

                                                Dropzone.autoDiscover = false;

                                                var myDropzone = new Dropzone('div#mydropzoneid', {
                                                    init: function () {
                                                        this.on("processing", function (file) {
                                                            this.options.url = '<%= String.Format("{0}ajax/ControlServer.aspx?control=uploadfile&MerchantAppID={1}&MerchantAppUID={2}&MDocSourceID={3}&Username={4}&DefaultDocTypeID={5}", WebUtil.GetBaseUrl(), UserSessions.CurrentMerchantApp.ID, UserSessions.CurrentMerchantApp.MerchantAppUID, 5, UserSessions.CurrentUser.UserName, 46) %>' + "&DocTypeID=" + $("#ContentPlaceHolder1_lstDocumentTypes").val() + "&Description=" + $("#ContentPlaceHolder1_Description").val() + "&IsPrivate=" + $('#IsPrivateFile').is(':checked');
                                                        });
                                                    },
                                                    autoProcessQueue: false,
                                                    clickable: true,
                                                    acceptedFiles: '<%= ConfigurationManager.AppSettings["AcceptableUploadMimeTypes"] %>',
                                                    error: function (file, errorMessage) {
                                                        //console.log('error', errorMessage, file);
                                                        alert(errorMessage);
                                                        this.removeFile(file);
                                                    },
                                                    maxFiles: 99, // keep this high, otherwise it will discard the file from the queue.
                                                    parallelUploads: 99,
                                                    paramName: "newlyuploadedfile",
                                                    url: '<%= String.Format("{0}ajax/ControlServer.aspx?control=uploadfile&MerchantAppID={1}&MerchantAppUID={2}&MDocSourceID={3}&Username={4}&DefaultDocTypeID={5}", WebUtil.GetBaseUrl(), UserSessions.CurrentMerchantApp.ID, UserSessions.CurrentMerchantApp.MerchantAppUID, 5, UserSessions.CurrentUser.UserName, 46) %>',
                                                    complete: function (file) {
                                                        //console.log('refreshing');
                                                        __doPostBack('ctl00$ContentPlaceHolder1$lbRefresh', '');
                                                    },
                                                    success: function (file) {
                                                        //console.log('just uploaded and removing file', file);
                                                        this.removeFile(file);
                                                    }

                                                });



                                            </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Description:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Description" runat="server" MaxLength="255" TextMode="MultiLine"
                                                Width="500px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr runat="server" ID="IsPrivateFileRow" ClientIDMode="Static" Visible="false" >
                                        <td class="lblRight">Private:
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="IsPrivateFile" ClientIDMode="Static" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <br />
                                            <asp:Button ID="btnSubmit" runat="server" Width="90px" Text="Upload" Visible="false" OnClick="btnSubmit_Click"
                                                CausesValidation="false" />

                                            <button type="button" id="multibegin">Begin Upload</button>

                                            <script type="text/javascript">

                                                $('#multibegin').on('click', function () {

                                                    myDropzone.processQueue();
                                                });
                                            
                                            </script>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <fieldset>
                        <legend>Documents</legend>
                        <table width="100%">
                            <tr>
                                <td>
                                    <div>
                                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" />
                                        <asp:LinkButton ID="lbRefresh" runat="server" Text="Refresh" Style="display: none" OnClick="btnRefresh_Click"></asp:LinkButton>
                                        <asp:Button ID="btnPrintCUWebites" runat="server" Text="Retrieve CU Websites" Style="display: none" OnClick="btnPrintCUWebites_Click" />
                                        Group Type:<asp:DropDownList ID="lstDocGroupTypes" AutoPostBack="true" OnSelectedIndexChanged="lstDocGroupTypes_SelectedIndexChanged"
                                            runat="server" />
                                    </div>
                                    <table width="100%">
                                        <tr>
                                            <td style="text-align: left">Page Size:
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
                                    <asp:GridView ID="grdDocuments" OnPageIndexChanging="grd_PageIndexChanging" DataSourceID="ObjectDataSource1"
                                        runat="server" AutoGenerateColumns="False" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                                        PagerStyle-CssClass="pgr" FooterStyle-CssClass="footer" Font-Names="verdana"
                                        Font-Size="X-Small" OnRowDataBound="grdDocuments_RowDataBound" OnRowCancelingEdit="grdDocuments_RowCancelingEdit"
                                        OnRowEditing="grdDocuments_RowEditing" OnRowUpdating="grdDocuments_RowUpdating"
                                        DataKeyNames="DocID" AllowPaging="True" AllowSorting="true" OnSorting="grdDocuments_Sorting">
                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                        <Columns>                                            
                                            <asp:TemplateField HeaderText="Private">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="IsPrivate" onclick="return confirmAction(this);" OnCheckedChanged="IsPrivate_CheckedChanged" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DocID" HeaderText="Doc ID" Visible="False" />
                                            <asp:TemplateField ItemStyle-Width="10px">
                                                <ItemTemplate>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/pdf.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Document Name" SortExpression="OrigName">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hypOrigName" runat="server" Target="_blank" Text='<%# Eval("OrigName") %>'></asp:HyperLink>
                                                    <asp:Label ID="lblOrigName" ToolTip="Private Document" runat="server" Text='<%# Eval("OrigName") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group Type" SortExpression="DocTypeGroupName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroupType" Text='<%# Eval("DocTypeGroupName") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type" SortExpression="DocTypeName">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="Type" Text='<%# Eval("DocTypeName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddpType" Width="150px">
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDescription" Text='<%#Eval("Description") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtDescription" Text='<%#Eval("Description") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Source" HeaderText="Source" ReadOnly="True" ItemStyle-Width="40px" SortExpression="Source" />
                                            <asp:BoundField DataField="ContentSize" HeaderText="Size" ReadOnly="True" ItemStyle-Width="40px" />
                                            <asp:BoundField DataField="DocDate" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyy hh:mm:ss tt}"
                                                ReadOnly="True" ItemStyle-Width="60px" />
                                            <asp:TemplateField HeaderText="User Created" ItemStyle-Width="65px" SortExpression="UserCreated">
                                                <EditItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("UserCreated") %>'></asp:Label>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("UserCreated") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Width="13px" Height="10px" Style="text-align: center; border: hidden; text-decoration: none;"
                                                        CausesValidation="false">
                                                        <asp:Image ID="Img6" runat="server" ImageUrl="../Images/edit.png" ToolTip="Edit"
                                                            ImageAlign="middle" />
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" Height="10px" Style="text-align: center;"
                                                        Width="13px" CausesValidation="true">
                                                        <asp:Image ID="Img7" runat="server" ImageUrl="~/Images/save.gif" ToolTip="Update"
                                                            ImageAlign="middle" />
                                                    </asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkCancel" runat="server" CommandName="Cancel" Height="10px" Style="text-align: center;"
                                                        CausesValidation="false" Width="13px">
                                                        <asp:Image ID="Img8" runat="server" ImageUrl="~/Images/undo.png" ToolTip="Cancel"
                                                            ImageAlign="middle" />
                                                    </asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkDelete" Visible="false" runat="server" CommandArgument='<%# Eval("DocID") %>' CommandName="Delete" Style="text-align: center;"
                                                        CausesValidation="false" Width="13px" OnClick="LinkButton1_Click">
                                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/delete2.png" ToolTip="Delete"
                                                            ImageAlign="middle" />
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="footer" />
                                        <PagerStyle CssClass="pgr" />
                                        <AlternatingRowStyle CssClass="alt" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetDocumentPaging" EnablePaging="True"
                                        MaximumRowsParameterName="PageSize" SelectCountMethod="GetDocumentPagingCount"
                                        StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
                                        OnSelecting="ods_Selecting" TypeName="DatamerchantAppPaging">
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
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                    <asp:PostBackTrigger ControlID="btnPrintCUWebites" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <hr />
</asp:Content>
