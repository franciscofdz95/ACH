<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRMConsentEmail.aspx.cs" Inherits="ZeusWeb.CRMConsentEmail" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onload="init()">
    <form runat="server">
    <script type="text/javascript">
        var dataFromParent;
        var newdataFromParent;
        var replacepat1;
        var replacepat2;
        var replacepat3;
       
        function init()
        {
            var newerstr;
            var newstr;
            newerstr = dataFromParent.replace(/[\r\n]+/gm, "");
            newdataFromParent = decodeURIComponent(newerstr);
            document.getElementById("newTabDiv").innerHTML = newdataFromParent.split('+').join(' ');
        }
    </script>
    <div id="newTabDiv"></div>
    </form>
</body>
</html>