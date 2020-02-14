<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoryBody.ascx.cs" Inherits="MGM_Transformer.HistoryBody" %>
<table border="0" cellpadding="0" cellspacing="5" width="100%">
    <tr align="center">
        <td width="30px">
            <div class="font12">Distributor:</div>
        </td>
        <td width="100px">
            <asp:UpdatePanel ID="upddlDistributor" runat="server">
            <ContentTemplate>
                <asp:DropDownList ID="ddlDistributor" runat="server"  AutoPostback="true"
                    onselectedindexchanged="ddlDistributor_SelectedIndexChanged"></asp:DropDownList>
                <asp:TextBox ID="txtRepDistributorID" runat="server" AutoPostback="true" visible="false"></asp:TextBox>
            </ContentTemplate>
            </asp:UpdatePanel>    
        </td>
        <td>
            <asp:UpdatePanel ID="uplblQuote" runat="server">
            <ContentTemplate>
                <h1><asp:Label ID="lblQuote" runat="server" Text="Quote History for XXX"></asp:Label></h1>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlDistributor" EventName = "SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlCompanies" EventName = "SelectedIndexChanged" />
            </Triggers>
            </asp:UpdatePanel>    
            
        </td>
        <td width="30px">
           <div class="font12">Customer:</div>
        </td>
        <td width="100px">
            <asp:UpdatePanel ID="upddlCompanies" runat="server">
            <ContentTemplate>
                <asp:DropDownList ID="ddlCompanies" runat="server"  AutoPostback="true"
                    onselectedindexchanged="ddlCompanies_SelectedIndexChanged"></asp:DropDownList>
                <asp:TextBox ID="txtCustomerID" runat="server" AutoPostback="true" visible="false"></asp:TextBox>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlDistributor" EventName = "SelectedIndexChanged" />
            </Triggers>
            </asp:UpdatePanel>    
        </td>
    </tr>
    <tr align="center">
        <td colspan="5">
            <hr />
        </td>
    </tr>
    <tr align="center">
        <td colspan="5">
            <asp:UpdatePanel ID="upgvQuoteHistory" runat="server">
            <ContentTemplate>

            <asp:GridView ID="gvQuoteHistory" runat="server" DataKeyNames="QuoteID,ProjectName"
                CellPadding="4" ForeColor="#333333" AllowSorting="True" 
                GridLines="Vertical" AllowPaging="True" PageSize="20" 
                AutoGenerateColumns="False" DataSourceID="dsQuoteHistory" 
                    onrowcommand="gvQuoteHistory_RowCommand">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField ShowHeader="true" HeaderText=" Select" 
                        SortExpression="Company" ConvertEmptyStringToNull="False">
                        <ItemTemplate>
                        <asp:LinkButton ID="ProjectName" runat="server" CausesValidation="false" 
                            CommandName="ProjectName" CommandArgument="<%#  gvQuoteHistory.DataKeys[((GridViewRow)Container).RowIndex].Value%>"
                            Text="<%# gvQuoteHistory.DataKeys[((GridViewRow)Container).RowIndex].Values[1] %>"></asp:LinkButton>
                        </ItemTemplate>
                        <ControlStyle Font-Underline="true" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="QuoteID" HeaderText="Quote #" ReadOnly="True" 
                        SortExpression="QuoteID" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Created_on" DataFormatString="{0:d}" 
                        HeaderText="Date" ReadOnly="True" SortExpression="Created_on" >
                    <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status" ReadOnly="True" 
                        SortExpression="Status" />
                    <asp:BoundField DataField="Company" HeaderText="Company" ReadOnly="True" 
                        SortExpression="Company" />
                    <asp:BoundField DataField="City" HeaderText="City" ReadOnly="True" 
                        SortExpression="City" />
                    <asp:BoundField DataField="ContactName" HeaderText="Contact Name" 
                        ReadOnly="True" SortExpression="ContactName" />
                    <asp:BoundField DataField="Notes" HeaderText="Notes" ReadOnly="True" 
                        SortExpression="Notes" />
                    <asp:BoundField DataField="TotalPrice" DataFormatString="{0:C2}" HeaderText="Total" 
                        ReadOnly="True" SortExpression="TotalPrice" >
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
            <asp:SqlDataSource ID="dsQuoteHistory" runat="server" 
                ConnectionString="<%$ ConnectionStrings:mgmdb %>" 
                SelectCommand="usp_Quote_History" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter DefaultValue="2" Name="rep_id" 
                        SessionField="RepDistributorID" Type="Int32" />
                    <asp:SessionParameter DefaultValue="0" Name="customer_id" 
                        SessionField="CustomerID" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlDistributor" EventName = "SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlCompanies" EventName = "SelectedIndexChanged" />
            </Triggers>
            </asp:UpdatePanel>    
        </td>
    </tr>
</table>


