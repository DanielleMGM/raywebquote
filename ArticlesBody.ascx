<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ArticlesBody.ascx.cs"
    Inherits="MGM_Transformer.ArticlesBody" %>
<table border="0" cellpadding="0" cellspacing="5" width="100%">
    <tr align="center">
        <td>
            <h2>
                <asp:Label ID="lblArticle" runat="server" Text="'Did You Know?' Articles"></asp:Label>
            </h2>
        </td>
    </tr>
    <tr align="center">
        <td>
            <asp:GridView ID="gvArticles" name="gvArticles" runat="server" CellPadding="4" GridLines="Vertical"
                AllowPaging="True" PageSize="20" AutoGenerateColumns="false" Width="35%" DataKeyNames="ArticleId,ArticleName,ArticleDate"
                OnRowCommand="gvArticles_RowCommand" 
                onpageindexchanging="gvArticles_PageIndexChanging">
                <AlternatingRowStyle BackColor="White" ForeColor="#31659C" />
                <Columns>
                    <asp:BoundField DataField="ArticleId" HeaderText="ArticleId" ReadOnly="True" Visible="false">
                        <HeaderStyle CssClass="GridHeaders" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemStyle Width="50px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ArticleNo" ReadOnly="True" HeaderText="Number" SortExpression="ArticleNo"
                        HtmlEncode="False">
                        <HeaderStyle CssClass="GridHeaders" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:TemplateField ShowHeader="true" HeaderText=" Article Name" SortExpression="ArticleName"
                        ConvertEmptyStringToNull="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="ViewArticle" runat="server" CausesValidation="false" CommandName="ViewArticle"
                                CommandArgument="<%#  gvArticles.DataKeys[((GridViewRow)Container).RowIndex].Value%>"
                                Text="<%# gvArticles.DataKeys[((GridViewRow)Container).RowIndex].Values[1] %>"></asp:LinkButton>
                        </ItemTemplate>
                        <ControlStyle Font-Underline="true" />
                        <HeaderStyle CssClass="GridHeaders" />
                        <ItemStyle Wrap="True" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="ArticleDate" ReadOnly="True" HeaderText="Date" SortExpression="ArticleDate"
                        HtmlEncode="False">
                        <HeaderStyle CssClass="GridHeaders" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemStyle Width="90px" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#31659C" Font-Bold="True" />
                <HeaderStyle Font-Bold="True" CssClass="GridHeaders" />
                <PagerStyle HorizontalAlign="Center" CssClass="GridHeaders" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
            <asp:SqlDataSource ID="dsArticles" runat="server" ConnectionString="<%$ ConnectionStrings:mgmdb %>">
            </asp:SqlDataSource>
        </td>
    </tr>
</table>
