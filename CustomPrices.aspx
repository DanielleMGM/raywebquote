<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomPrices.aspx.cs" Inherits="MGM_Transformer.CustomPrices" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administration - Custom Prices</title>
    <meta http-equiv="X-UA-Compatible"  content="IE-Edge"/>
    <!-- <link rel="stylesheet" type="text/css" href="./Scripts/mgmtransformer/production.css" /> -->
</head>
<body>
    <form id="form1" runat="server">
    <asp:scriptmanager ID="Scriptmanager1" runat="server"></asp:scriptmanager>
    <table style="text-align:center;"><tr><td>
    <a href="home.aspx">Home</a>
    <table>
        <tr style="text-align:center;">
            <td>
                <h1>Custom Prices</h1>
            </td>
        </tr>
        <tr>
            <td>
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="uprblWinding" runat="server">
                <ContentTemplate>
                    <asp:RadioButtonList id="rblWinding" runat="server" Autopostback="true"
                        RepeatDirection="Horizontal" 
                        onselectedindexchanged="rblWinding_SelectedIndexChanged" 
                        style="width: 157px">
                        <asp:ListItem Selected="True">Aluminum</asp:ListItem>
                        <asp:ListItem>Copper</asp:ListItem>
                    </asp:RadioButtonList>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rblWinding" EventName = "SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>    
            </td>
        </tr>
        <tr>
            <td>
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="upgvCustomPrices" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvCustomPrices" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" DataKeyNames="CustomStockID" DataSourceID="dsCustomPrices" 
                        ForeColor="#333333" GridLines="None" 
                        onrowdatabound="gvCustomPrices_RowDataBound">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="Winding" HeaderText="Winding" 
                                SortExpression="Winding" />
                            <asp:BoundField DataField="KFactor" HeaderText="K-Factor" 
                                SortExpression="KFactor" />
                            <asp:BoundField DataField="KVA" HeaderText="KVA" SortExpression="KVA" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Temperature" HeaderText="Degrees Rise" 
                                SortExpression="Temperature" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Price" DataFormatString="{0:C0}" HeaderText="Price" 
                                SortExpression="Price" >
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#336699" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#336699" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rblWinding" EventName = "SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>    
            </td>
        </tr>
        <tr>
            <td style="text-align:center;">
                <a href="home.aspx">Home</a>
            </td>
        </tr>
    </table>
    </td></tr></table>
    
    <asp:sqldatasource runat="server" 
        ConnectionString="<%$ ConnectionStrings:mgmdb %>" 
        SelectCommand="usp_CustomPrices" SelectCommandType="StoredProcedure" 
        ID="dsCustomPrices">
        <SelectParameters>
            <asp:SessionParameter Name="Winding" 
                SessionField="Winding" Type="String" />
        </SelectParameters>
    </asp:sqldatasource>
    </form>
</body>
</html>

