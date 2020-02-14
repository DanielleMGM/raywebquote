<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminCustomPricesBody.ascx.cs" Inherits="MGM_Transformer.AdminCustomPricesBody" %>
<table width="100%" border="0" cellpadding="0" cellspacing="5">
    <tr align="center">
        <td>
            <h1>Custom Prices</h1>
        </td>
    </tr>
    <tr>
        <td>
            <hr />
        </td>
    </tr>
    <tr align="center">
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
    <tr align="center">
        <td>
            <asp:UpdatePanel ID="upgvCustomPrices" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvCustomPrices" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" DataKeyNames="CustomStockID" DataSourceID="dsCustomPrices" 
                    ForeColor="#333333" GridLines="Vertical" AllowSorting="true" 
                    AllowPaging="True" PageSize="20" 
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
        <td align="center">
            <a href="home.aspx">Home</a>
        </td>
    </tr>
</table>
    
<asp:sqldatasource runat="server" 
    ConnectionString="<%$ ConnectionStrings:mgmdb %>" 
    SelectCommand="usp_CustomPrices" SelectCommandType="StoredProcedure" 
    ID="dsCustomPrices">
    <SelectParameters>
        <asp:SessionParameter Name="Winding" 
            SessionField="Winding" Type="String" />
    </SelectParameters>
</asp:sqldatasource>

