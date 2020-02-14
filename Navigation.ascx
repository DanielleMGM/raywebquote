<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navigation.ascx.cs" Inherits="MGM_Transformer.Navigation" %>
<table align="left" border="0" cellpadding="0" cellspacing="0" width="100%"> <!-- Screen size.  No border.  1 Column. -->
    <tr align="center">
        <td>
            <div id="navbar">
                <ul>
                    <li>
                        <asp:LinkButton ID="lnkbHome" runat="server" onclick="lnkbHome_Click" 
                            TabIndex="-1">Home</asp:LinkButton>
                        <asp:Label ID="lblHome" class="lnkselected" runat="server" Visible="False" 
                            Text="Home"></asp:Label>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbNewQuote" runat="server" onclick="lnkbNewQuote_Click" 
                            TabIndex="-1">New Quote</asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbQuote" runat="server" onclick="lnkbQuote_Click" 
                            TabIndex="-1">Quote</asp:LinkButton>
                        <asp:Label ID="lblQuote" class="lnkselected" runat="server" Visible="false" Text="Quote"></asp:Label>
                    </li>
                    <li>
                        <asp:Label ID="lblEmail" class="lnkselected" runat="server" Visible="false" Text="Email"></asp:Label>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbCustomer" runat="server" onclick="lnkbCustomer_Click" 
                            TabIndex="-1">Customers</asp:LinkButton>
                        <asp:Label ID="lblCustomer" class="lnkselected" runat="server" Visible="false" Text="Customers"></asp:Label>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbStockPrices" runat="server" 
                            onclick="lnkbStockPrices_Click" TabIndex="-1">Stock Prices</asp:LinkButton>
                        <asp:Label ID="lblStockPrices" class="lnkselected" runat="server" Visible="false" Text="Stock Prices"></asp:Label>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbReports" runat="server" 
                            onclick="lnkbReports_Click" TabIndex="-1">Reports</asp:LinkButton>
                        <asp:Label ID="lblReports" class="lnkselected" runat="server" Visible="false" Text="Reports"></asp:Label>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbDidYouKnow" runat="server" 
                            onclick="lnkbDidYouKnow_Click" TabIndex="-1">Did-You-Know</asp:LinkButton>
                        <asp:Label ID="lblDidYouKNow" class="lnkselected" runat="server" Visible="false" Text="Did-You-Know"></asp:Label>
                    </li>
                    <li>
                        <asp:HyperLink ID="hyp" NavigateUrl="~/UserGuide.pdf" runat="server" Text="Help" Target="_blank"></asp:HyperLink>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbReps" runat="server" onclick="lnkbReps_Click" 
                            TabIndex="-1">Reps</asp:LinkButton>
                        <asp:Label ID="lblReps" class="lnkselected" runat="server" Visible="false" Text="Reps"></asp:Label>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbMaintenance"  runat="server" 
                            onclick="lnkbMaintenance_Click" TabIndex="-1">Maintenance</asp:LinkButton>
                        <asp:Label ID="lblMaintenance" class="lnkselected" runat="server" Visible="false" Text="Maintenance"></asp:Label>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbLogout" runat="server" onclick="lnkbLogout_Click" 
                            TabIndex="-1">Log Out</asp:LinkButton>
                    </li>
                </ul>
            </div>
        </td>
    </tr>
    <tr align="center">
        <td><asp:Label ID="lblError" runat="server" CssClass="errorlabel" Text="Access denied." Visible=false></asp:Label></td>
    </tr>
</table>


