<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhoneRequest.aspx.cs" Inherits="MGM_Transformer.PhoneRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MGM Transformer - Phone Setup Request</title>
    <meta http-equiv="X-UA-Compatible" content="IE-edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <script src="~/Scripts/jquery-2.1.3.min.js" type="text/javascript"></script>
    <script src="~/Scripts/bootstrap.min.js" type="text/javascript"></script>
    
    <link href="~/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="~/css/Site.css" rel="stylesheet" type="text/css" />
</head>
<body class="bodybackground">
    <form class="form-horizontal" runat="server" action="PhoneRequest.aspx">
    <asp:scriptmanager ID="Scriptmanager1" runat="server"></asp:scriptmanager>

    <table cellpadding="20" cellspacing="20" border="0" style="background-color:White" class="center">
        <tr><td colspan="3">&nbsp;</td></tr>
        <tr>
            <td>&nbsp;&nbsp;&nbsp;</td>
            <td>
                <table cellpadding="2" cellspacing="2" border="0"  width="600" style="background-color:White">
                    <tr style="text-align:center;">
                        <td><img src="images/MGMeyeLOGO_Small.png" alt="eye logo" />
                        </td>
                        <td><h1>Phone Setup Instructions</h1>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr /></td>
                    </tr>
                    <tr>
                        <td colspan="2"><h4>Overview</h4></td>
                    </tr>
                    <tr>
                        <td colspan="2">The way security works in this application:</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0" cellpadding="5">
                                <tr>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    <td><ul><li>&nbsp;</li></ul></td>
                                    <td>Enter User Name and Password.
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    <td><ul><li>&nbsp;</li></ul></td>
                                    <td>If your internet IP address is found (i.e. work computer), you are in.
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    <td><ul><li>&nbsp;</li></ul></td>
                                    <td>If not, sends a message to your cell phone. (After you've set it up here, and we've approved it.)
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    <td><ul><li>&nbsp;</li></ul></td>
                                    <td>If you have the DuoSecurity phone app installed, you click a button, and you're in.
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    <td><ul><li>&nbsp;</li></ul></td>
                                    <td>Enter User Name and PasswordThe computer you work on then can be anything: iPad, home computer, etc.  The system only verifies with your phone.  It does not check the computer you're using.
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    <td><ul><li>&nbsp;</li></ul></td>
                                    <td><b>IMPORTANT</b>:  Don't give unauthorized persons access to this system!  Thanks.
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            Please choose your style of phone:
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:UpdatePanel ID="uprblPhoneType" runat="server">
                                <ContentTemplate>
                                    <asp:RadioButtonList ID="rblPhoneType" runat="server" AutoPostBack="true" 
                                        onselectedindexchanged="rblPhoneType_SelectedIndexChanged">
                                        <asp:ListItem Value="iPhone">&nbsp;iPhone</asp:ListItem>
                                        <asp:ListItem Value="Android">&nbsp;Android</asp:ListItem>
                                        <asp:ListItem Value="Blackberry">&nbsp;Blackberry</asp:ListItem>
                                        <asp:ListItem Value="Windows">&nbsp;Windows</asp:ListItem>
                                        <asp:ListItem Value="Other">&nbsp;Other</asp:ListItem>
                                    </asp:RadioButtonList>
                                        </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rblPhoneType" EventName = "SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel ID="uplblInstruct" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblCodeReaderInstructions" runat="server" Visible="false">
                                        If your phone recognizes 'QR Code' (3D) style barcodes from its camera, you can skip this step.  
                                        If you need to install an application to read these barcodes, navigate here from your cellphone:
                                    </asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rblPhoneType" EventName = "SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center;">
                            <asp:UpdatePanel ID="uplnkReader" runat="server">
                                <ContentTemplate>
                                    <asp:HyperLink ID="iphoneReader" runat="server" Visible="true"><a href="https://itunes.apple.com/us/app/qr-reader-for-iphone/id368494609?mt=8">Free
                                        QR Code Reader for Apple iPhones</a></asp:HyperLink>
                            
                                    <asp:HyperLink ID="androidReader" runat="server" Visible="true"><a href="https://play.google.com/store/apps/details?id=me.scan.android.client&hl=en">
                                        Free QR Code Reader for Android phones</a></asp:HyperLink>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rblPhoneType" EventName = "SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <h4>Request Mobile Access</h4>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            Please fill out this section to request mobile access to the MGM Transformer Web Quote system.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr /></td>
                    </tr>
                    <tr>
                        <td>Login ID:</td>
                        <td>
                            <asp:UpdatePanel ID="upddlRep" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlRep" runat="server" Visible="true" AutoPostBack="true" 
                                        onselectedindexchanged="ddlRep_SelectedIndexChanged"></asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlRep" EventName = "SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td>User Name:</td>
                        <td>
                            <asp:UpdatePanel ID="uplblUserName" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox runat="server" ID="txtUserName" Text="" AutoPostBack="true" 
                                        ontextchanged="txtUserName_TextChanged"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox runat="server" ID="chkNewUser" Text="&nbsp;&nbsp;New" 
                                        AutoPostBack="true" oncheckedchanged="chkNewUser_CheckedChanged"></asp:CheckBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlRep" EventName = "SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="txtUserName" EventName = "TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="chkNewUser" EventName = "CheckedChanged" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td>Full Name:</td>
                        <td>
                            <asp:UpdatePanel ID="uptxtFullName" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox runat="server" ID="txtFullName" Text="" AutoPostBack="true"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlRep" EventName = "SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="txtFullName" EventName = "TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td>Password:</td>
                        <td>
                            <asp:UpdatePanel ID="uptxtPassword" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox runat="server" ID="txtPassword" AutoPostBack="true" 
                                        ontextchanged="txtPassword_TextChanged"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlRep" EventName = "SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td>Email:</td>
                        <td>
                            <asp:UpdatePanel ID="uptxtEmail" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtEmail" runat="server" AutoPostBack="true" 
                                        ontextchanged="txtEmail_TextChanged" Columns="50"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtEmail" EventName = "TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td>Mobile Phone:</td>
                        <td>
                            <asp:UpdatePanel ID="uptxtPhone" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtPhone" runat="server" Columns="20" 
                                            ontextchanged="txtPhone_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtPhone" EventName = "TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr style="text-align:center;">
                        <td colspan="2">&nbsp;
                            <asp:UpdatePanel ID="uplblStatus" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblStatus" runat="server" CssClass="errorlabel" Text=""></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="txtPassword" EventName = "TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="txtEmail" EventName = "TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="txtPhone" EventName = "TextChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="btnRequestAccess" EventName = "Click" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center;">
                           <asp:UpdatePanel ID="upbtnRequestAccess" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnRequestAccess" runat="server" Text="Request Mobile Access" 
                                        CssClass="btn-warning" onclick="btnRequestAccess_Click" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlRep" EventName = "SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr /></td>
                    </tr>
                    <tr>
                        <td colspan="2"><h4>Install Duo Security Application</h4></td>
                    </tr>
                    <tr>
                        <td colspan="2">Once MGM has set you up, you'll be notified to come back here, and install the Duo Security Application.  Click on the link below.</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center;">
                            <asp:UpdatePanel ID="uplnkInstall" runat="server">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lblNotAvailable" CssClass="textlabel" Text="Not available until MGM has set you up for mobile access."></asp:Label>
                                    <asp:Label runat="server" ID="lblRequested"  CssClass="textlabel" Text="Your service has been requested, but MGM has not yet set you up for mobile access."></asp:Label>
                                    <asp:HyperLink ID="iphoneInstall" runat="server"><a href="https://itunes.apple.com/us/app/id422663827?mt=8">Free Duo Mobile application
                                        for Apple iPhones</a></asp:HyperLink>
                                    <asp:HyperLink ID="androidInstall" runat="server"><a href="https://play.google.com/store/apps/details?id=com.duosecurity.duomobile&hl=en">
                                        Free Duo Mobile for Android and most other phones</a></asp:HyperLink>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rblPhoneType" EventName = "SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>    
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><h4>Try it out!</h4>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            After setting up the Duo Mobile application, go to <a href="http://www.mgmtransformer.com">
                                www.mgmtransformer.com</a> from your iPad or other device outside the network.
                            Let us know if you need assistance.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            We hope you like the expanded capabilities of the MGM Web Quote system. If you see
                            ways we can improve, please let us know!
                        </td>
                    </tr>
                </table>
            </td>
            <td>&nbsp;&nbsp;&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">&nbsp;&nbsp;&nbsp;</td>
        </tr>
    </table>
</form>
</body>
</html>
