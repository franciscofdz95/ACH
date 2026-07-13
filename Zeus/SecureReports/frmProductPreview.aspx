<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmProductPreview.aspx.cs"
    Inherits="frmProductPreview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product Preview</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin:20px 20px 20px 20px; width:940px;">
        <div style="margin-top: 20px; font-size: 16px; font-weight: bold; border-bottom: 1px solid black;">
            <asp:Label ID="ProductName" runat="server"></asp:Label></div>
        <div class="justified_text" style="margin-top: 20px;">
            <asp:Label ID="MarketingContent" runat="server"></asp:Label></div>
        <div>
            &nbsp;</div>
        <asp:Panel ID="FeePanel" runat="server">
            <div class="SubHeading">
                Fee Schedule</div>
            <div>
                <asp:GridView Width="400" ID="FeeGrid" CssClass="mGrid" AutoGenerateColumns="false"
                    Font-Size="X-Small" Font-Names="Verdana" runat="server">
                    <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:BoundField DataField="FeeName" HeaderText="Fee Type">
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MerchantCost" HeaderText="Cost" ItemStyle-Width="100" DataFormatString="{0:0.00}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>