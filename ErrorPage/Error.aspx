<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="MGM_Transformer.ErrorPage.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error</title>
     <link href="~/css/Site.css" rel="stylesheet" type="text/css" />
</head>
<body class="bodybackground" >
    <form id="form1" runat="server">
    <div>
     <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr><td>&nbsp;</td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr align="center">
            <td><div id="loginform">
                <table class="loginboxbackground" border="2" cellpadding="10" cellspacing="0"><tr><td>
                <table border="0" cellpadding="0" cellspacing="5">
                    <tr>
                        <th><h3>MGM Transformer Company</h3>
                        </th>
                    </tr>
                    <tr>
                        <th><h1>Oops!</h1> 
                        </th>
                    </tr>
                    <tr>
                        <td align="center">An unexpected error occurred.
                        </td>
                    </tr>
                   <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnReturn" runat="server" Text="Return" />
                        </td>
                    </tr>
                                        
                </table>
                </td></tr></table></div>
            </td>
        </tr>
       
    </table>
    </div>
    </form>
</body>
</html>
