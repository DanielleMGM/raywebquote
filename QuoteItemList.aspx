<asp:gridview ID="gvQuoteItemList" runat="server" AllowSorting="True" 
    AutoGenerateColumns="False" CellPadding="4" 
    ForeColor="#333333" GridLines="None" 
    onrowcommand="gvCustomerList_RowCommand" DataSourceID="dsQuoteItemList">
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    <Columns>
        <asp:BoundField DataField="Quantity" HeaderText="Qty" 
            SortExpression="Quantity" >
        <ItemStyle HorizontalAlign="Center" />
        </asp:BoundField>
        <asp:BoundField DataField="StockOrCustom" 
            ReadOnly="True" SortExpression="StockOrCustom" />
        <asp:BoundField DataField="KVA" HeaderText="KVA" 
            ReadOnly="True" SortExpression="KVA" />
        <asp:BoundField DataField="PrimaryVoltage" HeaderText="Prim Voltage" 
            SortExpression="PrimaryVoltage" ReadOnly="True" />
        <asp:BoundField DataField="SecondaryVoltage" HeaderText="Sec Voltage" 
            SortExpression="SecondaryVoltage" ReadOnly="True" />
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
</asp:gridview>
    <asp:SqlDataSource ID="dsQuoteItemList" runat="server" 
    ConnectionString="<%$ ConnectionStrings:mgmdb %>" 
    SelectCommand="usp_QuoteItemList" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter DefaultValue="" Name="quote_id" SessionField="QuoteId" 
                Type="Int32" />
        </SelectParameters>
</asp:SqlDataSource>
