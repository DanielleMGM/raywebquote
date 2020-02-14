<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="MGM_Transformer.AccessDenied" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ipError" runat="server">
        This computer is not authorized to view this page. IP address is: <%=ip %>
    </div>
    <div id="userError" runat="server">
        This account is currently in use by one of your colleagues.  Please wait for user to log out in order to receive a quote. Thank You. 
    </div>
    </form>
</body>
</html>
