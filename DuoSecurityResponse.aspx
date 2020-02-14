<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DuoSecurityResponse.aspx.cs"
    Inherits="MGM_Transformer.DuoSecurityResponse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MGM Transformer - User Authentication Response</title>
    <meta http-equiv="X-UA-Compatible" content="IE-edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="Scripts/Duo-Web-v1.bundled.js" type="text/javascript"></script>
</head>
<body>
    <form id="frmMain" action="_self" runat="server">
        <asp:Label ID="lblNotVerified" runat="server" Visible="false">
            I'm sorry, but we could not authenticate you.<br />
            If you believe you've received this notice in error,<br />
            please contact:<br />
            <br />
            <table>
                <tr>
                    <th colspan="3"><h3>MGM Transformer Contacts</h3></th>
                </tr>
                <tr>
                    <th>Contact</th><th>Phone</th><th>Role</th>
                </tr>
                <tr>
                    <td>Davis DeBard</td><td>(323) 726-0888 x282</td><td>Application Developer</td>
                </tr>
                <tr>
                    <td>Chris Kaveh</td><td>(323) 726-0888 x223</td><td>Chief Technology Officer</td>
                </tr>
            </table>
        </asp:Label>
    </form>
</body>
</html>
