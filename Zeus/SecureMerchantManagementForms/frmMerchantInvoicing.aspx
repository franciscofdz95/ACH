<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true" Inherits="frmMerchantInvoicing" Title="Invoicing" Codebehind="frmMerchantInvoicing.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucACHGrid2.ascx" TagName="wucACHGrid2" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlTools" runat="server">
                        <div class="tbrtools">
                            <div class="tbrtoolsleft">
                                <table>
                                    <tr>
                                        <td>
                                            <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add Invoice" CommandName="Add"
                                                AccessKey="a" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                                <Appearance>
                                                    <Image Url="~/Images/add2.png" />
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                        <td>
                                            <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false" AccessKey="s"
                                                CommandName="Save" OnClick="tbrTools_ButtonClicked">
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
                                            <igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                                AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                                <Appearance>
                                                    <Image Url="~/Images/refresh.png" />
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Size="10pt" ForeColor="Red" />
                    <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                    <fieldset>
                        <legend>Invoice History</legend>
                        <asp:Panel ID="pnlGrid" runat="server" Height="" Width="">
                            <uc2:wucACHGrid2 ID="grdACH" runat="server"></uc2:wucACHGrid2>
                            &nbsp;<asp:Label runat="server" ID="lblACH" Text="no data.." Visible="false"></asp:Label>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
                            <div class="title">
                                &nbsp;&nbsp;Post Invoice (ACH)
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <table cellpadding="0" cellspacing="5">
                                    <tr>
                                        <td class="lblRight">
                                            Category:</td>
                                        <td>
                                            <asp:DropDownList ID="cboCategory" runat="server" Width="200px">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="cboCategory"
                                                ErrorMessage="Please select a category." Operator="NotEqual" ValueToCompare="-1"
                                                Display="None"></asp:CompareValidator></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">
                                            Type:</td>
                                        <td>
                                            <asp:DropDownList ID="cboType" runat="server" Width="200px">
                                                <asp:ListItem Value="27">ACH Charge</asp:ListItem>
                                                <asp:ListItem Value="22">ACH Refund</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">
                                            Post Date:</td>
                                        <td>
                                            <ig:WebDatePicker ID="txtPostDate" runat="server" NullDateLabel="" EnableAppStyling="False"
                                                Width="200px" AllowNull="False">
                                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                    SlideOpenDuration="1" />
                                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                    SlideOpenDuration="1" />
                                            </ig:WebDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">
                                            Amount:</td>
                                        <td>
                                            <ig:WebNumericEditor ID="txtAmount" runat="server" DataMode="Decimal" ValueText="0"
                                                Width="195px">
                                            </ig:WebNumericEditor>
                                            <asp:RequiredFieldValidator ID="rfvRequiredAmount" runat="server" ControlToValidate="txtAmount"
                                                Display="None" ErrorMessage="Please enter an amount"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtAmount"
                                                Display="None" ErrorMessage="Amount must be between $.01 and $10000" MaximumValue="10000"
                                                MinimumValue=".01" Type="Currency"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">
                                            Ref No:</td>
                                        <td>
                                            <asp:TextBox ID="txtRefID" runat="server" MaxLength="50" Width="195px"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
