<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucCardNumberBlocking" Codebehind="wucCardNumberBlocking.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<div style="width: 350px">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        Card Number:</td>
                    <td>
                        <asp:TextBox ID="CardNumber" runat="server" MaxLength="16"></asp:TextBox><asp:CustomValidator
                            ID="CustomValidator1" runat="server" ErrorMessage="Please enter valid card number"
                            Display="None" ControlToValidate="CardNumber" ClientValidationFunction="card_check"></asp:CustomValidator></td>
                    <td>
                        <asp:Button ID="btnAddCardNo" runat="server" Text="Add Card" OnClick="btnAddCardNo_Click" /></td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                Font-Size="X-Small" CssClass="mGrid" DataSourceID="odsCardNumberBlocking" Width="100%"
                DataKeyNames="CardNumberBlockID">
                <PagerStyle CssClass="pgr" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="CardNumberMask" HeaderText="Card Number" />
                    <asp:BoundField DataField="CardNumberBlockID" Visible="False" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDelete" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div style="text-align: right;">
                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" /></div>
            <asp:ObjectDataSource ID="odsCardNumberBlocking" runat="server" SelectMethod="GetMerchantCardNumberBlocks"
                TypeName="PaymentXP.DataObjects.DataRisk" OldValuesParameterFormatString="original_{0}"
                OnSelecting="odsIPBlocking_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="prms" Type="Object" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<script type="text/javascript">
    
   

function card_check(sender, args)
 {
        /* based on http://www.beachnet.com/~hstiles/cardtype.html */
        var CardNumber = args.Value;
       
        if (! isNum(CardNumber)) 
        {
                args.IsValid =false;
                return;
        }
        
        var no_digit = CardNumber.length;
        var oddoeven = no_digit & 1;
        var sum = 0;
        
        for (var count = 0; count < no_digit; count++) 
        {
            var digit = parseInt(CardNumber.charAt(count));
            if (!((count & 1) ^ oddoeven)) 
            {
                digit *= 2;
                if (digit > 9)
                digit -= 9;
            }
            sum += digit;
        }
        
        if (sum % 10 != 0)
        {
             args.IsValid =false;
             return;
        }
        

         args.IsValid =true;
  
   }
   
   
   function isNum(argvalue) {
        argvalue = argvalue.toString();

        if (argvalue.length == 0)
            return false;

        for (var n = 0; n < argvalue.length; n++)
        if (argvalue.substring(n, n+1) < "0" || argvalue.substring(n, n+1) > "9")
            return false;

        return true;
 }
 
</script>

