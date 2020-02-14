<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Followup.aspx.cs" Inherits="MGM_Transformer.followup.Followup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Followups</title>
</head>
<body>
    <form id="formMain" runat="server">
    <div>
        <asp:Panel ID="pnlMain" runat="server">
            <asp:Label ID="lblResponse" runat="server" Text="Please let us know the progress on your quotes."></asp:Label>
            <table>
                <tr>
                    <th>Quote<br />No</th><th><br />Progress</th><th>Lost<br />Reason</th><th>Lost<br />To</th>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="QuoteNo" runat="server" Text="1"></asp:Label>
                        <asp:HiddenField ID="QuoteID" runat="server" Value="1" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProgress" runat="server">
                        </asp:DropDownList>
                    </td>
                   <td>
                        <asp:DropDownList ID="ddlLostReason" runat="server">
                        </asp:DropDownList>
                    </td>
                   <td>
                        <asp:DropDownList ID="ddlLostTo" runat="server">
                        </asp:DropDownList>
                   </td>
                </tr>
            </table>
        </asp:Panel>
        
    </div>
    </form>
</body>
</html>
