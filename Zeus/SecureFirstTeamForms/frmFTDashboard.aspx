<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFirstTeam.Master" AutoEventWireup="True"
    CodeBehind="frmFTDashboard.aspx.cs" Inherits="ZeusWeb.SecureFirstTeamForms.frmFTDashboard" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/FirstTeam/wucFTGridManager.ascx" TagName="wucFTGridManager"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/FirstTeam/wucFTGridRep.ascx" TagName="wucFTGridRep"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/FirstTeam/wucFTGridTicket.ascx" TagName="wucFTGridTicket"
    TagPrefix="uc3" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contentpage" style="padding-left: 20px; padding-bottom: 40px;">
        <div class="titleline">
            Premier Services Dashboard
        </div>
        <asp:Panel runat="server" ID="pnlReps">
            Premier Services Reps:
            <asp:DropDownList runat="server" ID="ddlReps" AutoPostBack="true" OnSelectedIndexChanged="ddlReps_SelectedIndexChanged">
            </asp:DropDownList>
        </asp:Panel>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="mvNone" runat="server">
                None!
            </asp:View>
            <asp:View ID="mvManager" runat="server">
                <uc1:wucFTGridManager ID="wucFTGridManager1" runat="server" />
            </asp:View>
            <asp:View ID="mvRep" runat="server">
                <uc2:wucFTGridRep ID="wucFTGridRep1" runat="server" />
            </asp:View>
        </asp:MultiView>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <ig:WebTab ID="WebTab1" runat="server" Width="970px">
                    <Tabs>
                        <ig:ContentTabItem runat="server" Enabled="true">
                            <Template>
                                <div style="padding: 10px">
                                    <uc3:wucFTGridTicket ID="wucFTGridTicket1" runat="server" />
                                </div>
                            </Template>
                        </ig:ContentTabItem>
                        <ig:ContentTabItem runat="server" Enabled="true">
                            <Template>
                                <div style="padding: 10px">
                                    <uc3:wucFTGridTicket ID="wucFTGridTicket2" runat="server" />
                                </div>
                            </Template>
                        </ig:ContentTabItem>
                        <ig:ContentTabItem runat="server" Enabled="true">
                            <Template>
                                <div style="padding: 10px">
                                    <uc3:wucFTGridTicket ID="wucFTGridTicket3" runat="server" />
                                </div>
                            </Template>
                        </ig:ContentTabItem>
                        <ig:ContentTabItem runat="server" Enabled="true">
                            <Template>
                                <div style="padding: 10px">
                                    <uc3:wucFTGridTicket ID="wucFTGridTicket4" runat="server" />
                                </div>
                            </Template>
                        </ig:ContentTabItem>
                        <ig:ContentTabItem runat="server" Enabled="true">
                            <Template>
                                <div style="padding: 10px">
                                    <uc3:wucFTGridTicket ID="wucFTGridTicket5" runat="server" />
                                </div>
                            </Template>
                        </ig:ContentTabItem>
                    </Tabs>
                </ig:WebTab>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
