<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MGM_Transformer.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"><title>MGM Transformer Company Quotes</title>
    <link href="~/css/Site.css" rel="stylesheet" type="text/css" />
</head>
<body class="bodybackground">
    <form id="form1" runat="server" autocomplete="off">
    
    <!-- To prevent autofill -->
    <input type="text" name="prevent_autofill" id="prevent_autofill" value="" style="display:none;" />
    <input type="password" name="password_fake" id="password_fake" value="" style="display:none;" />

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
                        <th colspan="2"><h2>Rep Portal</h2> 
                        </th>
                    </tr>
                    <tr>
                        <td align="left">User Name&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:UpdatePanel ID="uptxtUserName" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtUserName" Columns="15" MaxLength="50" runat="server" 
                                    ontextchanged="txtUserName_TextChanged" AutoCompleteType="Disabled"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtUserName" EventName = "TextChanged" />
                            </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Password
                        </td>
                        <td>
                            <asp:UpdatePanel ID="uptxtPassword" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtPassword" Columns="15" MaxLength="50" runat="server" 
                                    TextMode="Password" ontextchanged="txtPassword_TextChanged" AutoCompleteType="Disabled"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtPassword" EventName = "TextChanged" />
                            </Triggers>
                            </asp:UpdatePanel>    
                       </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:UpdatePanel ID="uplblMessages" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblInvalid" runat="server" visible="false" ForeColor="Red">* Invalid user name.</asp:Label>
                            </ContentTemplate>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td align="left">
                                <asp:Button ID="btnLogin" runat="server" Text="Log in" Width="100" Height="30" UseSubmitBehavior="true" 
                                    onclick="btnLogin_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                        </td>
                    </tr>
                </table>
                </td></tr></table></div>
            </td>
        </tr>
        <tr align="center">
            <td>
                <asp:UpdatePanel ID="upPanelDuo" runat="server" UpdateMode="Conditional" Visible="false">
                <ContentTemplate>
                    <iframe id="duo_iframe" width="620" height="330" frameborder="0"></iframe>
                </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
