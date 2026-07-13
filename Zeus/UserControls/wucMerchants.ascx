<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucMerchants" Codebehind="wucMerchants.ascx.cs" %>

<script type="text/javascript" language="javascript">
 function validate(btn,txt1,txt2)
 {
    var btn1 = document.getElementById(btn);
    var txtA = document.getElementById(txt1);
    var txtB = document.getElementById(txt2);
    var s;
    if(txtA.value != " ")
    {
        s = parseInt(txtA.value);
        //PXP-15946
        //Fady Massoud
       if(s < 1)
       {
         alert('Enter valid ACHID');
         return false;
       }
    }
    if(txtB.value != " ")
    {
        s = parseInt(txtB.value);
        //PXP-15946
        //Fady Massoud
       if(s < 1)
       {
         alert('Enter valid MerchantID');
         return false;
       }  
    }
 }

 function CheckNumeric()
 {
    var key;
    key = event.which ? event.which : event.keyCode;
    if((key>=48 && key<=57)|| key == 13) 
    {
        event.returnValue= true;
    }
    else
    {
        alert("Please enter Numeric only");
        event.returnValue = false;
    }
 }
 
</script>


    

<div class="dialog">
    <fieldset>
        <legend>Merchant List</legend>
        <table width="100%">
            <tr>
                <td class="lblRight">
                    DBA:</td>
                <td>
                    <asp:TextBox runat="server" ID="txtDBA" Text="" Width="80px" EnableViewState="False"
                        TabIndex="1"></asp:TextBox></td>
                <td class="lblRight">
                    ZID:</td>
                <td>
                    <asp:TextBox ID="MerchantID" runat="server" EnableViewState="False" Width="70px"
                        MaxLength="7" TabIndex="2" ValidationGroup="grpMerchant"></asp:TextBox></td>
                <td class="lblRight">
                    MID:</td>
                <td>
                    <asp:TextBox ID="SettlePlatformMid" runat="server" EnableViewState="False" Width="90px"
                        TabIndex="7"></asp:TextBox></td>
                <td>
                    Partner DBA:</td>
                <td>
                    <asp:TextBox ID="AgentDBA" runat="server" EnableViewState="False" TabIndex="7" Width="90px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="lblRight">
                    Legal:</td>
                <td>
                    <asp:TextBox ID="BusinessLegalName" runat="server" Width="80px" TabIndex="5" EnableViewState="False"></asp:TextBox></td>
                <td class="lblRight">
                    AID:</td>
                <td>
                    <asp:TextBox ID="AchID" runat="server" ValidationGroup="grpMerchant" TabIndex="6"
                        EnableViewState="False" MaxLength="7" Width="70px"></asp:TextBox></td>
                <td class="lblRight">
                    Bank:</td>
                <td>
                    <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="95px" TabIndex="3">
                    </asp:DropDownList></td>
                <td>
                    Partner ID:</td>
                <td>
                    <asp:TextBox ID="AgentID" runat="server" EnableViewState="False" TabIndex="7" Width="90px"></asp:TextBox>
                   
                </td>
            </tr>
            <tr>
                <td>FMAID:</td>
                <td><asp:TextBox ID="FMAID" runat="server" EnableViewState="False"  Width="80px" MaxLength="15"></asp:TextBox></td>
                 <td>MLE:</td>
                <td><asp:TextBox ID="MLEName" runat="server" EnableViewState="False"  Width="70px"></asp:TextBox></td>
                <td colspan="4">&nbsp;</td>

            </tr>
            <tr>
                <td colspan="8" style="height: 10px;">
                </td>
            </tr>
            <tr>
                <td colspan="8" align="center">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" CausesValidation="false"
                        Width="70px" TabIndex="8" OnClick="btnSearch_Click"></asp:Button>
                    <asp:Button runat="server" ID="btnClose" Text="Close" Width="50px" CausesValidation="false"
                        TabIndex="5" OnClick="btnClose_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="lblRight" colspan="8">
                    <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                        <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            Font-Names="Verdana" Font-Size="X-Small" OnPageIndexChanging="grd_PageIndexChanging"
                            OnRowDataBound="grd_RowDataBound" DataSourceID="odsTransactions" OnRowCommand="grd_RowCommand"
                            AllowSorting="True" OnSorting="grd_Sorting">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
                            <Columns>
                                <asp:TemplateField HeaderText="ZID" SortExpression="ID">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkZID" runat="server" CommandName="ZID" CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="45px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name" SortExpression="BusinessDBAName">
                                    <ItemStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BusinessLegalName" HeaderText="MLE" SortExpression="BusinessLegalName">
                                    <ItemStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SettlePlatformMid" HeaderText="MID" SortExpression="SettlePlatformMid">
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AgentFirstLastName" HeaderText="Partner Name" SortExpression="AgentFirstLastName">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="FMAID" HeaderText="FMAID" SortExpression="FMAID">
                                    <ItemStyle Width="45px" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="Bank" HeaderText="Acq. Bank" SortExpression="Bank">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="ZID Status" SortExpression="Status">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ACHStatus" HeaderText="ACH/DD Status" SortExpression="ACHStatus">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                            </Columns>
                            <PagerStyle CssClass="pgr" />
                            <FooterStyle CssClass="footer" />
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsTransactions" runat="server" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            OnSelecting="odsTransactions_Selecting" StartRowIndexParameterName="CurrentPage"
                            TypeName="DataMerchantAppPaging">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                                <asp:Parameter Name="ControlID" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </asp:Panel>
                    <asp:Panel ID="pnlNoRecords" runat="server" Height="300px" Width="" Visible="true">
                        No data...
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </fieldset>
</div>