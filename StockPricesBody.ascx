<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StockPricesBody.ascx.cs"
    Inherits="MGM_Transformer.StockPricesBody" %>
<script type="text/javascript">
    function PriceList(winding, efficiency) {
        window.open("PriceListPDF.aspx?winding=" + winding + '&efficiency=' + efficiency, "_blank");
    }
</script>
<table width="100%" border="0" cellpadding="0" cellspacing="5">
    <tr align="center">
        <td>
            <h1>
                <asp:UpdatePanel ID="uplblStockPrices" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblStockPrices" runat="server" Text="Stock Prices" AutoPostback="true"></asp:Label>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDistributor" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </h1>
        </td>
    </tr>
    <tr>
        <td>
            <hr />
        </td>
    </tr>
    <tr align="center">
        <td>
            <table border="0" cellpadding="5" cellspacing="0">
                <tr>
                    <td align="left">
                        Distributors:
                    </td>
                    <td align="left">
                        <asp:UpdatePanel ID="upddlDistributor" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlDistributor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDistributor_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtRepID" runat="server" AutoPostBack="true" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtRepDistributorID" runat="server" AutoPostBack="true" Visible="false"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlDistributor" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td align="right">
                        <b>Price List PDF's</b>:
                    </td>
                    <td width="100px" align="left">
                        <asp:Button ID="btnAluminum" runat="server" Width="140" Height="30" Text="Aluminum DOE 2016"
                            BackColor="#ADB2BD" OnClientClick="return PriceList('Aluminum', 'DOE2016')" />
                        <asp:Button ID="btnCopper" runat="server" Width="140" Height="30" Text="Copper DOE 2016"
                            BackColor="#C87533" OnClientClick="return PriceList('Copper', 'DOE2016')" />
                    </td>
                    <td width="100px" align="left">
                        <div id="divTP1" runat="server">
                            <asp:Button ID="btnAluminumTP1" runat="server" Width="140" Height="30" Text="Aluminum TP-1"
                                BackColor="#ADB2BD" OnClientClick="return PriceList('Aluminum', 'TP-1')" />
                            <asp:Button ID="btnCopperTP1" runat="server" Width="140" Height="30" Text="Copper TP-1"
                                BackColor="#C87533" OnClientClick="return PriceList('Copper', 'TP-1')" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Configurations:&nbsp;
                        <td align="left" colspan="3">
                            <asp:UpdatePanel ID="upddlConfigs" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlConfigs" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlConfigs_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;<asp:Label ID="lblRepDistributorAlt" runat="server" Visible="false" class="errorlabel">SPECIAL PRICING IN EFFECT</asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlDistributor" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr align="center">
        <td>
            <asp:UpdatePanel ID="upgvStockPrices" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvStockPrices" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataSourceID="dsStockPrices" ForeColor="#333333" AllowSorting="True" GridLines="Vertical"
                        AllowPaging="True" PageSize="20">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="KVA" HeaderText="KVA" SortExpression="KVA">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CatalogNumber" HeaderText="Catalog #" SortExpression="CatalogNumber" />
                            <asp:BoundField DataField="AltNumber" HeaderText="Alt #" SortExpression="AltNumber">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" Font-Bold="True" />
                            </asp:BoundField>
                            <asp:BoundField DataField="KitNumber" HeaderText="Kit #" SortExpression="KitNumber">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="KitPrice" HeaderText="Kit Price" SortExpression="KitPrice"
                                DataFormatString="{0:C2}">
                                <ItemStyle HorizontalAlign="Right" Font-Bold="True" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Height" HeaderText="Height" SortExpression="Height">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Width" HeaderText="Width" SortExpression="Width">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Depth" HeaderText="Depth" SortExpression="Depth">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Weight" HeaderText="Weight" SortExpression="Weight">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitCase" HeaderText="Case" SortExpression="UnitCase" />
                            <asp:BoundField DataField="Enclosure" HeaderText="Enclosure" SortExpression="Enclosure" />
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#336699" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#336699" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#E6E7EB" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlDistributor" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlConfigs" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:SqlDataSource ID="dsStockPrices" runat="server" ConnectionString="<%$ ConnectionStrings:mgmdb %>"
                SelectCommand="usp_Stock_Prices" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue="2" Name="rep_id" SessionField="RepDistributorID"
                        Type="Int32" />
                    <asp:SessionParameter DefaultValue="Three" Name="phase" SessionField="Phase" Type="String" />
                    <asp:SessionParameter Name="winding" SessionField="Winding" Type="String" DefaultValue="Aluminum" />
                    <asp:SessionParameter Name="configuration" SessionField="Configuration" Type="String"
                        DefaultValue="240V DELTA PRIMARY - 208Y/120" />
                    <asp:SessionParameter Name="efficiency" SessionField="Efficiency" Type="String" DefaultValue="DOE2016" />
                </SelectParameters>
            </asp:SqlDataSource>
        </td>
    </tr>
    <tr align="center">
        <td>
            * Rain Hood kits (RH) are required for Outdoor (NEMA 3R) applications. 10KVA to
            150 KVA enclosures are dual rated.
        </td>
    </tr>
    <tr align="center">
        <td>
            Freight Allowance $1,500 net (within territory). Must add freight for out of territory
            shipments.
        </td>
    </tr>
</table>
