<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucDiversionDialog.ascx.cs"
    Inherits="ZeusWeb.UserControls.Reserve.wucDiversionDialog" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<ig:WebDialogWindow ID="dlgDivertedMethod" runat="server" Height="350px" Width="400px"
    Modal="true" InitialLocation="Centered" UseBodyAsParent="true" WindowState="Hidden">
    <ContentPane>
        <Template>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel runat="server" CssClass="dialog" ID="pnlDetail">
                        <asp:Label runat="server" ID="lblError" Style="color: Red"></asp:Label>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    Date Diverted
                                </td>
                                <td>
                                    <%--<asp:TextBox ID="DateDiverted" runat="server"></asp:TextBox>--%>
                                    <ig:WebDatePicker ID="DateDiverted" runat="server" EnableAppStyling="False" NullDateLabel=""
                                        DataMode="Date" Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                            SlideOpenDuration="1" />
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                            SlideOpenDuration="1" />
                                    </ig:WebDatePicker>

                                    <%--<asp:Label runat="server" ID="DateDiverted"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr id="trDateUndiverted" runat="server" visible="false">
                                <td >
                                    Date UnDiverted
                                </td>
                                <td>
                                    <%--<asp:TextBox ID="DateUnDiverted" runat="server"></asp:TextBox>--%>
                                    <ig:WebDatePicker ID="DateUndiverted" runat="server" EnableAppStyling="False" NullDateLabel=""
                                        DataMode="Date" Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                            SlideOpenDuration="1" />
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                            SlideOpenDuration="1" />
                                    </ig:WebDatePicker>

                                    
                                    <%--<asp:Label runat="server" ID="DateUndiverted"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Type
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="DiversionTypeID" AutoPostBack="True" OnSelectedIndexChanged="DivertedTypeID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trReserveRate" visible="false">
                                <td>
                                    Reserve Rate
                                </td>
                                <td>
                                    <ig:WebPercentEditor ID="ReserveRate" runat="server" MaxValue="100" ValueText="0"
                                        Width="111px">
                                    </ig:WebPercentEditor>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Reason
                                </td>
                                <td>
                               <%--     <asp:DropDownList ID="DiversionReasonID" runat="server">
                                    </asp:DropDownList>--%>

                                    <asp:TextBox runat="server" TextMode="MultiLine" Width="190px" ID="DivertedReason"></asp:TextBox>

                                </td>
                            </tr>
                            <tr runat="server" id="trResolution" visible="false">
                                <td>
                                    Resolution
                                </td>
                                <td>
                                <%--    <asp:DropDownList ID="DiversionResolutionID" runat="server">
                                    </asp:DropDownList>--%>

                                    <asp:TextBox runat="server" TextMode="MultiLine" Width="190px" ID="Resolution"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Diverted By
                                </td>
                                <td>
                                    <asp:Label ID="DivertedBy" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                       
                    </asp:Panel>
                     <center>
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />
                        </center>
                </ContentTemplate>
            </asp:UpdatePanel>
        </Template>
    </ContentPane>
    <Header CaptionText="Diverted Method">
    </Header>
</ig:WebDialogWindow>
