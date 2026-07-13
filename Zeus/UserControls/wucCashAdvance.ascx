<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucCashAdvance" Codebehind="~/UserControls/wucCashAdvance.ascx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<fieldset>
    <legend>Cash Advance</legend>
    <asp:Panel ID="pnlCash" runat="server">
        <asp:Label ID="lblError" runat="server" SkinID="Error"></asp:Label>
        <table style="width: 100%;">
            <tr>
                <td class="lblRight">
                    Lender:</td>
                <td>
                    <asp:DropDownList ID="LenderID" runat="server" Width="155px">
                    </asp:DropDownList>
                </td>
                <td class="lblRight">
                    Collection Method:
                </td>
                <td>
                    <asp:DropDownList ID="CollectionMethodID" runat="server" Width="155px">
                    </asp:DropDownList>
                </td>
                <td class="lblRight">
                    Amount Borrowed:</td>
                <td>
                    <ig:WebNumericEditor ID="AmountBorrowed" runat="server" ValueText="0" Width="100px" MinDecimalPlaces="2">
                    </ig:WebNumericEditor>
                </td>
                <td class="lblRight">
                    Date Funded:</td>
                <td>
                    <ig:WebDatePicker ID="DateFunded" runat="server" NullDateLabel="" NullValueRepresentation="Null"
                        Width="105px" EnableAppStyling="False">
                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                </td>
            </tr>
            <tr>
                <td class="lblRight">
                    Status:</td>
                <td>
                    <asp:DropDownList ID="StatusID" runat="server" Width="155px">
                    </asp:DropDownList>
                </td>
                <td class="lblRight">
                    Pay Back Amount:</td>
                <td>
                    <ig:WebNumericEditor ID="PaybackAmount" runat="server" ValueText="0" Width="150px">
                    </ig:WebNumericEditor>
                </td>
                <td class="lblRight">
                    Hold Back %:</td>
                <td colspan="3">
                    <ig:WebPercentEditor ID="HoldbackPct" runat="server" MaxValue="100" MinDecimalPlaces="2"
                        Width="100px">
                    </ig:WebPercentEditor>
                </td>
            </tr>
            <tr>
                <td colspan="8" align="center">
                    <br />
                    <asp:Button ID="btnAdd" runat="server" Text="Add Cash Advance" OnClick="btnAdd_Click" />&nbsp;<asp:Button
                        ID="btnSave" Visible="false" runat="server" Text="Save Cash Advance" OnClick="btnSave_Click" />&nbsp;<asp:Button
                            ID="btnClearFields" runat="server" OnClick="btnClearFields_Click" Text="Clear" />
                    <asp:HiddenField runat="server" ID="hdnCashadvance" />
                </td>
            </tr>
        </table>
        <br /> <%--DataKeyNames="CashAdvanceID,LenderID,CollectionMethodID,StatusID"--%>
        <asp:Panel runat="server" ID="pnlAdvances" Height="200px" Width="100%" ScrollBars="vertical">
            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                Font-Size="X-Small" CssClass="mGrid" DataSourceID="odsCashAdvance" Width="100%"
                OnRowDataBound="grd_RowDataBound"
                OnRowCommand="grd_RowCommand">
                <PagerStyle CssClass="pgr" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:TemplateField HeaderText="ID" ItemStyle-Width="10px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkID" runat="server" CommandName="Select"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CashAdvanceID" ItemStyle-CssClass="hideGridColumn"  HeaderStyle-CssClass="hideGridColumn"/>
                    <asp:BoundField DataField="LenderName" HeaderText="Lender Name" ItemStyle-Width="50px" />
                    <asp:BoundField DataField="CollectionMethod" HeaderText="Collection Method" ItemStyle-Width="70px" />
                    <asp:BoundField DataField="AmountBorrowed" HeaderText="Amt Borrowed" DataFormatString="{0:0.00}"
                        ItemStyle-Width="50px" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="PaybackAmount" HeaderText="Payback Amt" DataFormatString="{0:0.00}"
                        ItemStyle-Width="50px" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="HoldbackPct" HeaderText="Hold back %" DataFormatString="{0:n}"
                        ItemStyle-Width="50px" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField DataField="StatusName" HeaderText="Status" ItemStyle-Width="30px" />
                    <asp:BoundField DataField="DateFunded" HeaderText="Date Funded" DataFormatString="{0:MM/dd/yyyy}"
                        ItemStyle-Width="30px" />
                    <asp:BoundField DataField="UserCreated" HeaderText="User Created" ItemStyle-Width="50px" />
                    <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyyy}"
                        ItemStyle-Width="30px" />
                    <asp:BoundField DataField="LenderID" ItemStyle-CssClass="hideGridColumn"  HeaderStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="CollectionMethodID" ItemStyle-CssClass="hideGridColumn"  HeaderStyle-CssClass="hideGridColumn" />
                    <asp:BoundField DataField="StatusID" ItemStyle-CssClass="hideGridColumn"  HeaderStyle-CssClass="hideGridColumn" />
                </Columns>
            </asp:GridView>
        </asp:Panel>
        <asp:ObjectDataSource ID="odsCashAdvance" runat="server" SelectMethod="GetCashAdvance"
            TypeName="PaymentXP.DataObjects.DataRisk" OldValuesParameterFormatString="original_{0}"
            OnSelecting="odsCashAdvance_Selecting">
            <SelectParameters>
                <asp:Parameter Name="prms" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
</fieldset>
