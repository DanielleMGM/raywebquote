<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaintDown.aspx.cs" Inherits="MGM_Transformer.MaintDown" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>MGM Transformer Company Quotes</title>
    <link href="~/css/Site.css" rel="stylesheet" type="text/css" />
</head>
<body class="bodybackground">
    <form id="form1" runat="server" autocomplete="off">
    <asp:scriptmanager ID="Scriptmanager1" runat="server" ></asp:scriptmanager>
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
                        <th colspan="2"><h3>MGM Transformer Company</h3>
                        </th>
                    </tr>
                    <tr>
                        <th colspan="2"><h2>Notification</h2> <%--(In Dev)--%>
                        </th>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <table class="bordersolid">
                                <tr align="center" class="errorlabel"><td><br />&nbsp;&nbsp;The system is <em>DOWN</em> now&nbsp;&nbsp;
                                    <br />for maintenance.<br />&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">It will be brief.  Please check back.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;
                        </td>
                    </tr>
                </table>
                </td></tr></table></div>
            </td>
        </tr>
     </table>
    </form>
</body>
</html>
