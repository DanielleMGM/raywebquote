<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminMaintenanceBody.ascx.cs" Inherits="MGM_Transformer.AdminMaintenanceBody" %>
<table width="100%" border="0" cellpadding="0" cellspacing="5">
    <tr>
        <td colspan="2" align="center">
            <h1>Maintenance</h1>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <hr />
        </td>
    </tr>
    <tr valign="top">
        <td width="150px">
            <asp:HiddenField ID="hidOption" runat="server" Value="CustomPriceList" Visible="false" />
            <div id="adminbar">
                <ul>
                    <li>
                        <asp:LinkButton ID="lnkbCustomPriceList" runat="server" onclick="lnkbCustomPriceList_Click">Custom Price List</asp:LinkButton>
                        <asp:Label ID="lblCustomPriceList" class="lnkselected" runat="server" Visible="false">Custom Price List</asp:Label>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbExpedite" runat="server" onclick="lnkbExpedite_Click">Expedite Fees</asp:LinkButton>
                        <asp:Label ID="lblExpedite" class="lnkselected" runat="server" Visible="False">Expedite Fees</asp:Label>
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbGiftCardPmts" runat="server" onclick="lnkbGiftCardPmts_Click">Gift Card Pmts</asp:LinkButton>
                        <asp:Label ID="lblGiftCardPmts" class="lnkselected" runat="server" Visible="False">Gift Card Pmts</asp:Label>
                    </li>
                </ul>
            </div>
        </td>
        <td>
            <table>
                <asp:Panel id="pnlCustomPriceList" visible="false" runat="server">
                <tr>
                    <td colspan="2">
                        <h2>Custom Price List</h2>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr align="left" valign="top">
                    <td colspan="2">
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
                    <td colspan="2">
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
                </asp:Panel>
                <asp:Panel id="pnlExpediteFees" visible="false" runat="server">
                <tr>
                    <td colspan="2">
                        <h2>Expedite Fees</h2>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                            <asp:GridView ID="gvExpedite" runat="server" AutoGenerateColumns="False" 
                                CellPadding="4" DataSourceID="dsExpedite" AutoGenerateEditButton="True"
                                ForeColor="#333333" GridLines="Vertical" AllowSorting="True" 
                                AllowPaging="True" PageSize="20" DataKeyNames="NoOfDays">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="NoOfDays" HeaderText="No of Days" 
                                        SortExpression="NoOfDays" ReadOnly="True">
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MarkupPct" HeaderText="Markup Pct" 
                                        SortExpression="MarkupPct">
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:CheckBoxField DataField="IsEnabled" HeaderText="Is Enabled" 
                                        SortExpression="IsEnabled">
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:CheckBoxField>
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
                    </td>
                </tr>
                </asp:Panel>
                <asp:Panel id="pnlGiftCardPmts" visible="false" runat="server">
                <tr>
                    <td colspan="2">
                        <h2>Gift Card Pmts</h2>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        Show Paid:                        
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upchkShowPaid" runat="server">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkShowPaid" runat="server" AutoPostBack="true" onCheckedChanged="chkShowPaid_CheckedChanged" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkShowPaid" EventName = "CheckedChanged" />
                        </Triggers>
                        </asp:UpdatePanel>    
                    </td>
                </tr>
                <tr>
                    <td>
                        Rep:                        
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upddlReps" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlReps" runat="server" AutoPostBack="true"  onselectedindexchanged="ddlReps_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlReps" EventName = "SelectedIndexChanged" />
                        </Triggers>
                        </asp:UpdatePanel>    
                    </td>
                </tr>
                <tr>
                    <td>
                        Month:                        
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upddlMonths" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlMonths" runat="server" AutoPostBack="true"  onselectedindexchanged="ddlMonths_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlMonths" EventName = "SelectedIndexChanged" />
                        </Triggers>
                        </asp:UpdatePanel>    
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="upchkAll" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblChkAll" runat="server" Text="Check All:" Visible="false"></asp:Label>
                            <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Visible="false" onCheckedChanged="chkAll_CheckedChanged" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkAll" EventName = "CheckedChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlReps" EventName = "SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="ddlMonths" EventName = "SelectedIndexChanged" />
                        </Triggers>
                        </asp:UpdatePanel>    
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="upgvGiftCardPmts" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvGiftCardPmts" runat="server" AutoGenerateColumns="False" 
                                CellPadding="4" DataSourceID="dsGiftCardPmts" AutoGenerateEditButton="True"
                                ForeColor="#333333" GridLines="Vertical" AllowSorting="True" 
                                AllowPaging="True" PageSize="20" DataKeyNames="SONumber">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:CheckBoxField DataField="IsPaid" HeaderText="Paid" 
                                        SortExpression="IsPaid">
                                     <ItemStyle HorizontalAlign="Center" />
                                    </asp:CheckBoxField>
                                    <asp:BoundField DataField="RepName" HeaderText="Rep Name" 
                                        SortExpression="RepName" ReadOnly="True">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QuoteNo" HeaderText="Quote No" 
                                        SortExpression="QuoteNo" ReadOnly="True">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SONumber" HeaderText="SO Number" 
                                        SortExpression="SONumber" ReadOnly="True">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SODate" HeaderText="SO Date" 
                                        SortExpression="SODate" ReadOnly="True" DataFormatString="{0:d}" >
                                    <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CustomUnitTotal" HeaderText="Amount" 
                                        SortExpression="CustomUnitTotal" ReadOnly="True" DataFormatString="{0:C0}" >
                                    <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DatePaid" HeaderText="Date Paid" 
                                        SortExpression="DatePaid" ReadOnly="True" DataFormatString="{0:d}">
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
                            <asp:AsyncPostBackTrigger ControlID="ddlReps" EventName = "SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="chkShowPaid" EventName = "CheckedChanged" />
                        </Triggers>
                        </asp:UpdatePanel>    
                    </td>
                </tr>
                </asp:Panel>
            </table>
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
<asp:sqldatasource runat="server" 
    ConnectionString="<%$ ConnectionStrings:mgmdb %>" 
    SelectCommand="SELECT * FROM ExpediteFees ORDER BY NoOfDays DESC" SelectCommandType="Text" 
    UpdateCommand="UPDATE ExpediteFees SET MarkupPct=@MarkupPct, IsEnabled=@IsEnabled WHERE NoOfDays=@NoOfDays" UpdateCommandType="Text"
    ID="dsExpedite">
</asp:sqldatasource>
<asp:sqldatasource runat="server" 
    ConnectionString="<%$ ConnectionStrings:mgmdb %>" 
    SelectCommand="usp_Quote_GiftCard_Pmt" SelectCommandType="StoredProcedure" 
    UpdateCommand="UPDATE OrderSummary SET IsPaid=@IsPaid, DatePaid=CASE @IsPaid WHEN 1 THEN GetDate() ELSE NULL END WHERE SONumber=@SONumber" UpdateCommandType="Text"
    ID="dsGiftCardPmts">
    <SelectParameters>
        <asp:ControlParameter Name="rep_id" ControlID="ddlReps" PropertyName="SelectedValue" Type="Int32" />
        <asp:ControlParameter Name="show_paid" ControlID="chkShowPaid" PropertyName="Checked" Type="Boolean" />
        <asp:ControlParameter Name="month" ControlID="ddlMonths" PropertyName="SelectedValue" Type="DateTime" />
        <asp:ControlParameter Name="check_all" ControlID="chkAll" PropertyName="Checked" Type="Boolean" />
    </SelectParameters>
</asp:sqldatasource>


