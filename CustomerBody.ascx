<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerBody.ascx.cs" Inherits="MGM_Transformer.CustomerBody" %>

<table border="0" cellpadding="0" cellspacing="5" width="100%">
    <tr align="center">
        <td colspan="4">
            <h1>
                <asp:Label ID="lblCustomers" runat="server" Text="Customers"></asp:Label></h1>
        </td>
    </tr>
    <tr align="center">
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblCustomerSearch" runat="server" Text="Find Company:"></asp:Label>
                    <asp:TextBox ID="tbSearchCompany" runat="server"></asp:TextBox>
                    <asp:Button ID="btnGo" runat="server" Text="GO" OnClick="btnGo_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr align="center">
        <td colspan="4">
            <hr />
        </td>
    </tr>
</table>
   <table align="center">
    <tr>
        <td colspan="4">
            <asp:UpdatePanel ID="upgvCustomerList" runat="server">
            <ContentTemplate>
                <asp:Gridview ID="gvCustomerList" runat="server" AllowSorting="True" DataKeyNames="CustomerContactID,Company" 
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Vertical" 
                    onrowcommand="gvCustomerList_RowCommand" AllowPaging="True" 
                    DataSourceID="CustomerList" 
                    onpageindexchanged="gvCustomerList_PageIndexChanged" PageSize="15" 
                    onrowdatabound="gvCustomerList_RowDataBound">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                         <asp:BoundField DataField="CustomerContactID" HeaderText="CustomerContactID" InsertVisible="False" 
                            ReadOnly="True" SortExpression="CustomerContactID" Visible="False" />
                        <asp:TemplateField ShowHeader="true" HeaderText="Company" SortExpression="Company" ConvertEmptyStringToNull="False">
                            <ItemTemplate>
                            <asp:LinkButton ID="Company" runat="server" CausesValidation="false" 
                                CommandName="Company" CommandArgument="<%# gvCustomerList.DataKeys[((GridViewRow)Container).RowIndex].Value %>"
                                Text="<%# gvCustomerList.DataKeys[((GridViewRow)Container).RowIndex].Values[1] %>"></asp:LinkButton>
                            </ItemTemplate>
                            <ControlStyle Font-Underline="true" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="City" HeaderText="City" 
                            SortExpression="City" />
                        <asp:BoundField DataField="ContactName" HeaderText="Contact Name" 
                            SortExpression="ContactName" />
                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                        <asp:BoundField DataField="Obsolete" HeaderText="Obsolete" SortExpression="Obsolete" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#336699" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#336699" Font-Bold="True" ForeColor="White" />
                    <PagerSettings PageButtonCount="20" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:Gridview>
            </ContentTemplate>
            </asp:UpdatePanel>    
        </td>
    </tr>
</table>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <asp:UpdatePanel ID="uppnlRow" runat="server">
            <ContentTemplate>
                <asp:Panel id="pnlRow" visible="false" runat="server">
                    <hr />
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvCustomerList" EventName = "RowCommand" />
            </Triggers>
            </asp:UpdatePanel>    
        </td>
    </tr>
    <tr align="center">
        <td>
            <asp:UpdatePanel ID="uptblEditCustomer" runat="server">
            <ContentTemplate>
                <asp:Panel id="pnlEditCust" visible="false" runat="server">
                <table border="0" cellpadding="4" cellspacing="1" width="50%">
                    <tr align="left" valign="top">
                        <td rowspan="4">
                            <h3>Edit&nbsp;Customer&nbsp;&nbsp;</h3>
                        </td>
                        <td>
                            Company
                        </td>
                        <td>
                            <asp:UpdatePanel ID="uptxtCompany" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCompany" Columns="30" MaxLength="100" runat="server" 
                                    AutoPostBack="true" ontextchanged="txtCompany_TextChanged"></asp:TextBox>
                                <asp:Label ID="lblCompany" runat="server" visible="false"></asp:Label>
                                <asp:TextBox ID="txtCustomerID" visible="false" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtRepID" visible="false" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtRepName" visible="false" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtRepDistributorID" visible="false" runat="server"></asp:TextBox>
                                <asp:Label ID="lblInvalidCompany" runat="server" visible="false" ForeColor="Red"> * System defined.  Contact MGM if no longer in use.</asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvCustomerList" EventName = "RowCommand" />
                                <asp:AsyncPostBackTrigger ControlID="chkObsolete" EventName = "CheckedChanged" />
                            </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td rowspan="5">
                            <asp:Button ID="btnSave" runat="server" Text="Save" Width="100" Height="60" 
                                onclick="btnSave_Click" /><br />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100" Height="30" 
                                onclick="btnCancel_Click" />
                        </td>
                    </tr>
                    <tr align="left">
                        <td colspan="2">
                            <asp:CheckBox ID="chkChgAllContacts" runat="server" Text="Company name changes for ALL CONTACTS." />
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            City
                        </td>
                        <td>
                            <asp:UpdatePanel ID="uptxtCity" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCity" Columns="30" MaxLength="100" runat="server"></asp:TextBox>
                                <asp:TextBox ID="txtCustomerContactID" visible="false" runat="server"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvCustomerList" EventName = "RowCommand" />
                                <asp:AsyncPostBackTrigger ControlID="chkObsolete" EventName = "CheckedChanged" />
                            </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            Contact&nbsp;Name
                        </td>
                        <td>
                            <asp:UpdatePanel ID="uptxtContactName" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtContactName" Columns="30" MaxLength="100" runat="server"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvCustomerList" EventName = "RowCommand" />
                                <asp:AsyncPostBackTrigger ControlID="chkObsolete" EventName = "CheckedChanged" />
                            </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr align="left">
                        <td>
                            <asp:UpdatePanel ID="upchkObsolete" runat="server">
                            <ContentTemplate>
                                <asp:CheckBox ID="chkObsolete" runat="server" Text="Obsolete" 
                                    oncheckedchanged="chkObsolete_CheckedChanged" AutoPostBack="true" />
                                <asp:Label ID="lblInvalidObsolete" runat="server" visible="false" ForeColor="Red"> * System defined.  Contact MGM if no longer in use.</asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="chkObsolete" EventName = "CheckedChanged" />
                            </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            Email
                        </td>
                        <td>
                            <asp:UpdatePanel ID="uptxtEmail" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEmail" Columns="30" MaxLength="100" runat="server" 
                                    ontextchanged="txtEmail_TextChanged" Autopostback="true"></asp:TextBox>
                                <asp:Label ID="lblEmailInvalid" runat="server" visible="false" ForeColor="Red">* Invalid email.</asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtEmail" EventName = "TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="gvCustomerList" EventName = "RowCommand" />
                                <asp:AsyncPostBackTrigger ControlID="chkObsolete" EventName = "CheckedChanged" />
                            </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                </asp:Panel>  <!-- pnlEditCust -->
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvCustomerList" EventName = "RowCommand" />
            </Triggers>
            </asp:UpdatePanel>    
        </td>
    </tr>
</table>


<div class="col100" align="center">
    <asp:UpdatePanel ID="uplblErrors" runat="server">
    <ContentTemplate>
        <asp:Label ID="lblErrors" runat="server" visible="false" ForeColor="Red">* See above error(s).</asp:Label>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="txtEmail" EventName = "TextChanged" />
        <asp:AsyncPostBackTrigger ControlID="txtCompany" EventName = "TextChanged" />
        <asp:AsyncPostBackTrigger ControlID="chkObsolete" EventName = "CheckedChanged" />
    </Triggers>
    </asp:UpdatePanel>

</div>

<asp:SqlDataSource ID="CustomerList" runat="server" 
    ConnectionString="<%$ ConnectionStrings:mgmdb %>" 
    SelectCommand="usp_Customer_List" SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:SessionParameter Name="rep_id" SessionField="RepDistributorID" Type="Int32" 
            DefaultValue="2" />
    </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="mgmuser" runat="server"></asp:SqlDataSource>

