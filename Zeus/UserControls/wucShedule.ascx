<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucShedule" Codebehind="wucShedule.ascx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="250px" Width="450px"
    InitialLocation="Centered" Modal="True" WindowState="Hidden" >
    <ContentPane>
        <Template>
            <div class="dialog">
                <table>
                    <tr>
                        <td class="lblEdit">
                            <b>
                                <asp:HiddenField ID="TCID" runat="server" />
                                <asp:Label ID="lblEnableSchedule" runat="server" Text="Enable Schedule:" ForeColor="Green" Visible="true"></asp:Label></b></td>
                        <td>
                            <asp:CheckBox ID="IsEnabled" runat="server" Checked="True" AutoPostBack="True" OnCheckedChanged="IsEnabled_CheckedChanged" Visible="true"/></td>
                    </tr>
                    <tr>
                        
                        <td class="lblEdit">
                            Select Occurrence:</td>
                        <td>
                            <asp:RadioButtonList ID="cboOccurenceOption" runat="server" BackColor="#EEEAF4" AutoPostBack="True"
                                OnSelectedIndexChanged="Occurrence_Selected">
                                <asp:ListItem Value="1">Daily</asp:ListItem>
                                <asp:ListItem Value="2">Monthly</asp:ListItem>
                            </asp:RadioButtonList></td>
                    </tr>
                    <tr>
                        <td class="lblEdit">
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnOptDayly" runat="server" Visible="true">
                    <table>
                        <tr class="cal">
                            <td align="left" class="lblEdit">
                                Set Option:</td>
                            <td align="left" class="normal">
                                Every
                                <asp:DropDownList ID="cboDayly" runat="server" BackColor="#EEEAF4">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="12">12</asp:ListItem>
                                    <asp:ListItem Value="13">13</asp:ListItem>
                                    <asp:ListItem Value="14">14</asp:ListItem>
                                    <asp:ListItem Value="15">15</asp:ListItem>
                                    <asp:ListItem Value="16">16</asp:ListItem>
                                    <asp:ListItem Value="17">17</asp:ListItem>
                                    <asp:ListItem Value="18">18</asp:ListItem>
                                    <asp:ListItem Value="19">19</asp:ListItem>
                                    <asp:ListItem Value="20">20</asp:ListItem>
                                    <asp:ListItem Value="21">21</asp:ListItem>
                                    <asp:ListItem Value="22">22</asp:ListItem>
                                    <asp:ListItem Value="23">23</asp:ListItem>
                                    <asp:ListItem Value="24">24</asp:ListItem>
                                    <asp:ListItem Value="25">25</asp:ListItem>
                                    <asp:ListItem Value="26">26</asp:ListItem>
                                    <asp:ListItem Value="27">27</asp:ListItem>
                                    <asp:ListItem Value="28">28</asp:ListItem>
                                    <asp:ListItem Value="29">29</asp:ListItem>
                                    <asp:ListItem Value="30">30</asp:ListItem>
                                    <asp:ListItem Value="31">31</asp:ListItem>
                                </asp:DropDownList>
                                day(s)</td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnOptMonthly" runat="server" Visible="false">
                    <table>
                        <tr class="cal">
                            <td align="left" class="lblEdit">
                                Set Option:</td>
                            <td align="left" class="normal" colspan="2">
                                <asp:RadioButton ID="rdOptMonthDay" runat="server" Text="Day" GroupName="optMonth"
                                    Checked="true" />
                                <asp:DropDownList ID="cboMontylyDay" runat="server" Width="47px" BackColor="#EEEAF4">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="12">12</asp:ListItem>
                                    <asp:ListItem Value="13">13</asp:ListItem>
                                    <asp:ListItem Value="14">14</asp:ListItem>
                                    <asp:ListItem Value="15">15</asp:ListItem>
                                    <asp:ListItem Value="16">16</asp:ListItem>
                                    <asp:ListItem Value="17">17</asp:ListItem>
                                    <asp:ListItem Value="18">18</asp:ListItem>
                                    <asp:ListItem Value="19">19</asp:ListItem>
                                    <asp:ListItem Value="20">20</asp:ListItem>
                                    <asp:ListItem Value="21">21</asp:ListItem>
                                    <asp:ListItem Value="22">22</asp:ListItem>
                                    <asp:ListItem Value="23">23</asp:ListItem>
                                    <asp:ListItem Value="24">24</asp:ListItem>
                                    <asp:ListItem Value="25">25</asp:ListItem>
                                    <asp:ListItem Value="26">26</asp:ListItem>
                                    <asp:ListItem Value="27">27</asp:ListItem>
                                    <asp:ListItem Value="28">28</asp:ListItem>
                                    <asp:ListItem Value="29">29</asp:ListItem>
                                    <asp:ListItem Value="30">30</asp:ListItem>
                                    <asp:ListItem Value="31">31</asp:ListItem>
                                </asp:DropDownList>
                                of every&nbsp;<asp:DropDownList ID="cboMontylyMonth" runat="server" BackColor="#EEEAF4">
                                    <asp:ListItem Value="1">1</asp:ListItem>
                                    <asp:ListItem Value="2">2</asp:ListItem>
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="12">12</asp:ListItem>
                                </asp:DropDownList>
                                month(s)
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                &nbsp;&nbsp;<!-- -->
                <hr class="line" />
                <center>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />&nbsp;<asp:Button ID="btnCancel"
                        runat="server" Text="Cancel" CausesValidation="False" OnClick="btnCancel_Click" /></center>
            </div>
        </Template>
    </ContentPane>
    <Header CaptionText="Recurring Schedule">
    </Header>
</ig:WebDialogWindow>
