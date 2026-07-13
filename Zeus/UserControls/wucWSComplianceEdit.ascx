<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_wucWSComplianceEdit"
    CodeBehind="wucWSComplianceEdit.ascx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="wucMessage.ascx" TagName="wucMessage" TagPrefix="uc2" %>
<asp:Panel ID="pnlTools" runat="server">
    <div class="tbrtools">
        <div class="tbrtoolsleft" style="width: 100%">
            <table width="100%">
                <tr>
                    <td>
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
                                    <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false" AccessKey="s"
                                        CommandName="Save" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
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
                                <td>
                                    <igtxt:WebImageButton ID="btnSubmit" runat="server" Text="Submit" Enabled="false"
                                        CommandName="Submit" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                        <Appearance>
                                            <Image Url="~/Images/refresh.png" />
                                        </Appearance>
                                    </igtxt:WebImageButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="text-align: right; width: 130px;">
                        <igtxt:WebImageButton ID="btnCancelReview" runat="server" Text="Cancel Review" Enabled="false"
                            CommandName="CancelReview" OnClick="tbrTools_ButtonClicked" CausesValidation="false">
                            <Appearance>
                                <Image Url="~/Images/disk_blue_error.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Panel>
<uc2:wucMessage ID="wucMessage2" runat="server" />
<asp:Panel runat="server" Visible="true" ID="pnlMore" Style="padding: 7px;">
    <asp:Button runat="server" ID="btnExportExcel" Text="Export To Excel" OnClick="btnExportExcel_Click" />
    <asp:Button runat="server" ID="btnExportPDF" Text="Export to PDF" OnClick="btnExportPDF_Click" />
    <%--<asp:Button runat="server" ID="btnPreview" Text="Preview Letter" OnClick="btnPreview_Click" />--%>

    <asp:Button runat="server" ID="btnInitiateEmail" Text="Initiate Email"
        OnClick="btnInitiateEmail_Click" />

</asp:Panel>
<asp:Panel runat="server" Style="padding: 10px;" ID="pnlDetails">
    <table cellpadding="5px" border="0">
        <tr>
            <td valign="top">
                <table class="wscomptable" border="0">
                    <tr>
                        <td>ZID:
                        </td>
                        <td>
                            <asp:Label ID="lblZID" ForeColor="Black" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Ticket ID:
                        </td>
                        <td>
                            <asp:Label ID="lblTicketID" ForeColor="Black" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>DBA:
                        </td>
                        <td>
                            <asp:Label ID="lblDBA" ForeColor="Black" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Date of Review:
                        </td>
                        <td>
                            <asp:Label ID="lblDateOfReview" ForeColor="Black" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Status:
                        </td>
                        <td>
                            <asp:Label ID="lblResult" ForeColor="Black" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Website URL:
                        </td>
                        <td>
                            <asp:Label ID="lblWebsiteURL" ForeColor="Black" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>MCC:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblMCC"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Date first CU Approved:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblDateCUApproved"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Date first MS Active:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblDateCSActive"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Link to CU Website Document:
                        </td>
                        <td>
                            <asp:PlaceHolder ID="phLink" runat="server"></asp:PlaceHolder>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table class="wscomptable">
                    <tr>
                        <td>Face to Face:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblFacetoFace"></asp:Label>%
                        </td>
                    </tr>
                    <tr>
                        <td>Internet:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblInternet"></asp:Label>%
                        </td>
                    </tr>
                    <tr>
                        <td>Mail Order:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblMailOrder"></asp:Label>%
                        </td>
                    </tr>
                    <tr>
                        <td>Telephone Order:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblTelephoneOrder"></asp:Label>%
                        </td>
                    </tr>
                    <tr>
                        <td>Underwriting Notes:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblUnderwritingNotes"></asp:Label>
                        </td>
                    </tr>

                    <tr>
                        <td>Compliance Rating:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblComplianceRating"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Earned / Possible:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblEarnedOverPossible"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Risk Level:
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblRiskIndex"></asp:Label>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblNoteMerch" Visible="false" runat="server">Note: This checklist is for a different merchant than the one selected</asp:Label>
    <br />
    <div class="title">
        Checklist &nbsp;<hr class="line" />
    </div>
    <asp:UpdatePanel runat="server" ID="pnlUpdate">
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" ShowFooter="True"
                AutoGenerateColumns="False" CssClass="mGrid wcchecklist" PagerStyle-CssClass="pgr"
                FooterStyle-CssClass="footer" Font-Names="verdana" Font-Size="X-Small">
                <Columns>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:HiddenField ID="hidWSComplianceItemID" Value='<%# Bind("WSComplianceItemID") %>'
                                runat="server" />
                            <asp:HiddenField ID="hidWSComplianceItemAnswerUID" Value='<%# Bind("WSComplianceItemAnswerUID") %>'
                                runat="server" />
                            <asp:HiddenField ID="hidWSComplianceAssocID" Value='<%# Bind("WSComplianceAssocID") %>'
                                runat="server" />
                            <asp:DropDownList ID="WSComplianceItemAnswerUID" CssClass="ddlComp" AutoPostBack="true"
                                runat="server" OnSelectedIndexChanged="WSComplianceItemAnswerUID_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Description">

                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("ComplianceDescription") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            Totals:
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                    </asp:TemplateField>


                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Points Possible">
                        <ItemTemplate>
                            <asp:Label ID="lblPointsPossible" runat="server"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblPointsPossibleFooter" runat="server"></asp:Label>
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterStyle HorizontalAlign="Right" />
                    </asp:TemplateField>


                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderText="Points Missed">
                        <ItemTemplate>
                            <asp:Label ID="lblPointsMissed" runat="server"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblPointsMissedFooter" runat="server"></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="Points Earned" ItemStyle-HorizontalAlign="Right"> 
                        <ItemTemplate>
                            <asp:Label ID="lblPointsEarned" runat="server"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblPointsEarnedFooter" runat="server"></asp:Label>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>



                    <asp:TemplateField ItemStyle-Width="155px" HeaderText="Comments">
                       <ItemTemplate>
                            <asp:TextBox ID="tbComment" runat="server" AutoPostBack="true" OnTextChanged="tbComment_TextChanged" Rows="3" Text='<%# Bind("Comment") %>' TextMode="MultiLine" Width="155px"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="155px" />
                    </asp:TemplateField>
                    
                    
                    <asp:TemplateField HeaderText="Internal Comments" ItemStyle-Width="155px">
                        <ItemTemplate>
                            <asp:TextBox ID="tbInternalComment" runat="server" AutoPostBack="true" OnTextChanged="tbInternalComment_TextChanged" Rows="3" Text='<%# Bind("InternalComment") %>' TextMode="MultiLine" Width="155px"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle Width="155px" />
                    </asp:TemplateField>


                </Columns>
                <FooterStyle CssClass="footer" />
                <PagerStyle CssClass="pgr" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="height: 20px;">
        <!-- -->
    </div>
    <div class="title">
        Upload Document
        <hr class="line" />
    </div>
    <table>
        <tr>
            <td valign="top">Select File:
            </td>
            <td>
                <asp:FileUpload ID="fuFile" runat="server" />
            </td>
        </tr>
        <tr>
            <td valign="top">Comments:
            </td>
            <td>
                <asp:TextBox runat="server" ID="Comment" Width="500px" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btnUpload" runat="server" Text="Upload File" OnClick="btnUpload_Click" />
            </td>
        </tr>
    </table>
    <div style="height: 20px;">
        <!-- -->
    </div>
    <div class="title">
        Document List
        <hr class="line" />
    </div>
</asp:Panel>
<div style="padding: 10px;">
    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
        AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
        FooterStyle-CssClass="footer" Font-Names="verdana" Font-Size="X-Small" EnableModelValidation="True" OnRowDataBound="GridView2_RowDataBound"> 
        <AlternatingRowStyle CssClass="alt" />
        <Columns>
            <asp:TemplateField HeaderText="Filename">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lbFilename" Text='<%# Bind("OrigName") %>' CommandName="ViewDoc"
                        CommandArgument='<%# Bind("DocID") %>' OnClick="lbFilename_Click"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Description" HeaderText="Comments" />
            <asp:BoundField DataField="ContentSize" HeaderText="Size" />
            <asp:BoundField DataField="DocDate" HeaderText="Date Uploaded" />
        </Columns>
        <FooterStyle CssClass="footer" />
        <PagerStyle CssClass="pgr" />
    </asp:GridView>
</div>
