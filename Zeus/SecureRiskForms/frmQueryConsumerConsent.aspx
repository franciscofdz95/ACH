<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true" ValidateRequest="false"
    Inherits="frmQueryConsumerConsent" Title="Query Consumer Consent" CodeBehind="frmQueryConsumerConsent.aspx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/CRM/wucCRMNotificationGrid.ascx" TagName="wucCRMNotificationGrid"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/CRM/wucCRMCustomerGrid.ascx" TagName="wucCRMCustomerGrid"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/CRM/wucCRMSubscription.ascx" TagName="wucCRMSubscription"
    TagPrefix="uc3" %>
<%@ MasterType VirtualPath="~/MasterPageRisk.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="JavaScript" type="text/javascript">
       
    </script>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <legend>Filters</legend>
        <table>
            <tr>
                <td class="lblRight">
                  NMI Transaction ID:
                </td>
                <td>
                   <asp:TextBox ID="NMITransactionID" runat="server" Width="65px"></asp:TextBox>
                   <%-- <asp:RangeValidator ID="RangeValidator1" runat="server" Display="None" ErrorMessage="Please enter a valid NMITransactionID"
                        MaximumValue="100000000000" MinimumValue="1" Type="Double" ControlToValidate="NMITransactionID"></asp:RangeValidator>--%>
                </td>
               
                <td>
                    CRM:
                </td>
                <td>
                   <asp:DropDownList ID="NMIVendorUID" runat="server" Width="150px"   AutoPostBack="true">
                                                        </asp:DropDownList>
                </td>
               
                <td>
                    <igtxt:WebImageButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit"
                        AccessKey="h">
                        <Appearance>
                            <Image Url="~/Images/Check.png" />
                        </Appearance>
                    </igtxt:WebImageButton>
                </td>
            </tr>
        </table>
        <div style="text-align: center;">
            <br />
        </div>
    </fieldset>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlMerchant" Visible="false">
                    <br />
                        <uc3:wucCRMSubscription ID="wucCRMSubscription1" runat="server" />
                    <ig:WebTab ID="tabReport" runat="server" Width="1338" SelectedIndex="0">
                        <Tabs>
                            <ig:ContentTabItem runat="server" Text="Notification">
                                <Template>
                                    <div class="simplepadding">
                                        <uc1:wucCRMNotificationGrid ID="wucCRMNotificationGrid1" runat="server" />
                                    </div>
                                </Template>
                            </ig:ContentTabItem>
                            <ig:ContentTabItem runat="server" Text="Action">
                                <Template>
                                    <div class="simplepadding">
                                        <uc2:wucCRMCustomerGrid ID="wucCRMCustomerGrid1" runat="server" />
                                    </div>
                                </Template>
                            </ig:ContentTabItem>
                        </Tabs>
                    </ig:WebTab>
                  
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    <fieldset runat="server" id="NoDataRecord">
        <asp:Label runat="server" ID="noData" Text="No Data..."></asp:Label>
    </fieldset>
    <br />
</asp:Content>
