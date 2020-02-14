<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeBody.ascx.cs" Inherits="MGM_Transformer.HomeBody" %>
<asp:Panel ID="ControlPanel" runat="server" DefaultButton="btnGo">
    <table border="0" cellpadding="0" cellspacing="5" width="100%">
        <tr>
            <td class="width-70">
                <asp:Label ID="lblLogin" runat="server" Text="Company:"></asp:Label>
            </td>
            <td class="width-100">
                <asp:UpdatePanel ID="upddlLogin" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlLogin" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlLogin_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlLogin" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            <td></td>
            <td style="width: 100px;">Current User:
            </td>
            <td class="width-100">
                <asp:Label ID="lblFullName" runat="server" Text="Label" CssClass="textlabel"></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td></td>
            <td class="width-100"></td>
            <td valign="top" align="center">
                <asp:UpdatePanel ID="uplblQuote" runat="server">
                    <ContentTemplate>
                        <h1>
                            <asp:Label ID="lblQuote" runat="server" Text="Quote History"></asp:Label>
                        </h1>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlLogin" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>


        <tr>
            <td class="width-170" colspan="2" align="center"></td>
            <td align="center" rowspan="2">
                <asp:UpdateProgress ID="upProgQuoteHistory" runat="server">
                    <ProgressTemplate>
                        <img alt="progress" src="images/ajax-loader.gif" />Processing...
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
            <td class="width-70"></td>
            <td class="width-100"></td>
        </tr>
        <tr>
            <td colspan="5" align="center">
                <table>
                    <tr align="center">
                        <td>
                            <h3>Type</h3>
                        </td>
                        <td>
                            <h3>Rep</h3>
                        </td>
                        <td>
                            <h3>
                                <%--  <asp:Label ID="lblCustomers" runat="server" Text="Customers"></asp:Label>--%>

                            </h3>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="border-solid-line" style="text-align: left">
                            <asp:UpdatePanel ID="uprblOrder" runat="server" style="width:200px; height:65px; vertical-align:middle">
                                <ContentTemplate>
                                    <div style="padding-top:5px;">
                                    <asp:RadioButtonList ID="rblOrder" runat="server" RepeatDirection="Vertical"
                                        OnSelectedIndexChanged="rblOrder_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="QUOTES" Value="Active" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="ORDERS" Value="Orders">
                                        </asp:ListItem>
                                    </asp:RadioButtonList>
                                    <div style="padding-left:4px;">
                                        <asp:CheckBox ID="cbPendingApproval" runat="server" Text="PENDING APPROVAL" Visible="False" AutoPostBack="True" OnCheckedChanged="cbPendingApproval_CheckedChanged" />
                                    </div>
                                        </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rblOrder" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="border-solid-line" style="text-align: left">
                            <asp:UpdatePanel ID="uprblLoginFilter" runat="server" style="width:200px; height:65px; vertical-align:middle">
                                <ContentTemplate>
                                     <div style="padding-top:5px;">
                                     <asp:RadioButtonList ID="rblLoginFilter" runat="server" RepeatDirection="Vertical"
                                        OnSelectedIndexChanged="rblLoginFilter_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="ALL" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="Mine" Value="Login" Selected="True"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    </div>
                                    <div style="padding-left: 5px;"> 
                                        <asp:CheckBox ID="cbPendingEmails" runat="server" Text="PENDING EMAIL(S)" OnCheckedChanged="cbPendingEmails_CheckedChanged" AutoPostBack="True" Visible="false" />
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rblLoginFilter" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="rblOrder" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td class="border-solid-line">
                            <asp:UpdatePanel ID="uprblDateFilter" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="5" width="100%">
                                        <tr>
                                            <td>
                                               
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width:450px; height:65px;">
                                                    <ContentTemplate>
                                                        <div style="padding-top:20px;">
                                                        <asp:Label ID="lblCustomerSearch" runat="server" Text="Find:"></asp:Label>
                                                        <asp:TextBox ID="tbSearchCompany" runat="server"></asp:TextBox>
                                                        <asp:Button ID="btnGo" runat="server" Text="GO" OnClick="btnGo_Click" />
                                                        <asp:Button ID="btnClearCustomer" runat="server" Text="Clear" OnClick="btnClearCustomer_Click" /><br />
                                                         <div style="padding-top:5px;">
                                                           <asp:Label ID="FindWhat" runat="server" Text="Quote #,Company,Project or City. Example: 30231" ForeColor="#0066FF" Height="10px"></asp:Label>
                                                        </div>
                                                        </div>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnClearCustomer" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rblOrder" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="rblLoginFilter" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>

                    </tr>
                </table>
            </td>
        </tr>
        <tr align="center">
            <td colspan="5">
                <hr />
            </td>
        </tr>

      
        <tr align="center">
            <td colspan="5">

                 <!-- ============= -->
                 <!-- PENDING EMAILS -->
                 <!-- ============= -->


                <asp:UpdatePanel ID="updPendingEmails" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlPendingEmails" runat="server">
                            <asp:GridView ID="gvPendingEmails" runat="server"
                                CellPadding="4"
                                GridLines="Vertical" AllowPaging="True" PageSize="20"
                                DataKeyNames="DateTimeAdded"
                                AutoGenerateColumns="false" Width="100%" OnPageIndexChanging="gvPendingEmails_PageIndexChanging" OnRowDataBound="gvPendingEmails_RowDataBound">
                                <AlternatingRowStyle BackColor="White" ForeColor="#31659C" />
                                <Columns>
                                    <asp:BoundField DataField="Attempts" HeaderText="Attempts" ReadOnly="True"
                                        SortExpression="Attempts" HtmlEncode="False">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateTimeAdded" ReadOnly="True"
                                        HeaderText="Date Added" SortExpression="DateTimeAdded" HtmlEncode="False">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateTimeVerified" HeaderText="Date Verified"
                                        SortExpression="DateTimeVerified" ReadOnly="True">
                                        <HeaderStyle CssClass="GridHeaders" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmailTypeCode" HeaderText="Type Code"
                                        SortExpression="EmailTypeCode" ReadOnly="True">
                                        <HeaderStyle CssClass="GridHeaders" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ErrorText" HeaderText="Error"
                                        SortExpression="ErrorText" ReadOnly="True">
                                        <HeaderStyle CssClass="GridHeaders" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QuoteEmailID" HeaderText="QuoteEmailID"
                                        SortExpression="QuoteEmailID" ReadOnly="True">
                                        <HeaderStyle CssClass="GridHeaders" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="EmailStatus" Text='<%# Convert.ToInt32(Eval("Status")) == 1 ? "Sent" : "Not Sent" %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="GridHeaders" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserEmail" HeaderText="User Email"
                                        SortExpression="UserEmail" ReadOnly="True">
                                        <HeaderStyle CssClass="GridHeaders" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UserName" HeaderText="User Name"
                                        SortExpression="UserName" ReadOnly="True">
                                        <HeaderStyle CssClass="GridHeaders" />
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
                            <asp:HiddenField ID="hfPreviousVisibleGrid" runat="server" />
                            <asp:HiddenField ID="hfCurrentVisibleGrid" runat="server" />
                            <asp:SqlDataSource ID="dsPendingEmails" runat="server" ConnectionString="<%$ ConnectionStrings:mgmdb %>"></asp:SqlDataSource>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlLogin" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="rblOrder" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cbPendingEmails" EventName="CheckedChanged" />
                    </Triggers>
                </asp:UpdatePanel>

        <!-- ============= -->
        <!-- ACTIVE QUOTES -->
        <!-- ============= -->

                <asp:UpdatePanel ID="upgvQuoteActive" runat="server">
                    <ContentTemplate>

                        <asp:Label ID="lblValid" runat="server" Text="" CssClass="errorlabel"></asp:Label>

                        <asp:Panel ID="ActivePanel" runat="server">
                            <asp:GridView ID="gvQuoteActive" runat="server"
                                CellPadding="4" AllowSorting="True"
                                GridLines="Vertical" AllowPaging="True" PageSize="20"
                                AutoGenerateColumns="False" DataKeyNames="QuoteID,CompanyProjectCity"
                                OnRowCommand="gvQuoteActive_RowCommand" OnRowDataBound="gvQuoteActive_RowDataBound"
                                OnRowEditing="gvQuoteActive_RowEditing" OnRowUpdating="gvQuoteActive_RowUpdating"
                                OnRowCancelingEdit="gvQuoteActive_RowCancelingEdit"
                                OnPageIndexChanging="gvQuoteActive_PageIndexChanging"
                                Style="margin-top: 0px" OnSorting="gvQuoteActive_Sorting" Width="100%">
                                <AlternatingRowStyle BackColor="White" ForeColor="#31659C" />
                                <Columns>
                                    <asp:TemplateField ShowHeader="false" HeaderText="QuoteID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="QuoteID" Text='<%# Eval("QuoteID") %>' runat="server" />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:BoundField DataField="QuoteNo" HeaderText="Quote #" ReadOnly="True"
                                        SortExpression="QuoteNo" HtmlEncode="False">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QuoteNoVer" ReadOnly="True"
                                        HeaderText="Ver" SortExpression="QuoteNoVer" HtmlEncode="False">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="true" HeaderText=" Select"
                                        SortExpression="CompanyProjectCity" ConvertEmptyStringToNull="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="CompanyProjectCity" runat="server" CausesValidation="false"
                                                CommandName="CompanyProjectCity" CommandArgument="<%#  gvQuoteActive.DataKeys[((GridViewRow)Container).RowIndex].Value%>"
                                                Text="<%# gvQuoteActive.DataKeys[((GridViewRow)Container).RowIndex].Values[1] %>"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ControlStyle Font-Underline="true" />
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle Wrap="True" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CurrentStatus" HeaderText="Status"
                                        SortExpression="CurrentStatus" ReadOnly="True">
                                        <HeaderStyle CssClass="GridHeaders" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QuoteDate" ReadOnly="True"
                                        HeaderText="Date" SortExpression="QuoteDate"
                                        DataFormatString="{0:M/d/yy}">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RepName" HeaderText="By" ReadOnly="True"
                                        SortExpression="RepName">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TotalPrice" HeaderText="Price" ReadOnly="True"
                                        SortExpression="TotalPrice" DataFormatString="{0:C2}">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Notes" HeaderText="Notes"
                                        SortExpression="Notes" ReadOnly="True">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle Wrap="True" />
                                    </asp:BoundField>

                                    <asp:CommandField ShowEditButton="True" EditText="Progress"></asp:CommandField>
                                    <asp:TemplateField ShowHeader="true" HeaderText="Progress" SortExpression="ProgressCode" Visible="false">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlProgress" runat="server"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="true" HeaderText="Lost Reason" SortExpression="LostReasonCode" Visible="false">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlLostReason" runat="server"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="true" HeaderText="Lost To" SortExpression="LostToCode" Visible="false">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlLostTo" runat="server"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblProgressCode" Text='<%# Eval("ProgressCode") %>' runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblLostReasonCode" Text='<%# Eval("LostReasonCode") %>' runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblLostToCode" Text='<%# Eval("LostToCode") %>' runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    
                                    
                                    <asp:TemplateField ShowHeader="true" HeaderText="Followup" SortExpression="FollowupDate" Visible="false">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblFollowupDate" Text='<%# Eval("FollowupDate") %>' runat="server" />
                                        </EditItemTemplate>
                                        <HeaderStyle CssClass="GridHeaders" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="true" HeaderText="F/U Date" SortExpression="FollowupDate" Visible="false">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFollowupDate" runat="server" Text='<%# Eval("FollowupDate") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                   
                                   
                                    <asp:BoundField DataField="Display_Name" HeaderText="Agent Name" SortExpression="Display_Name">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
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

                            <asp:HiddenField ID="QuotesSorted" runat="server" />
                            <asp:HiddenField ID="QuotesSortedDirection" runat="server" />
                            <asp:HiddenField ID="QuotesCompanySearch" runat="server" />
                            <asp:HiddenField ID="QuotesSortedColumn" runat="server" />

                            <asp:HiddenField ID="ShowPendingApprovals" runat="server" />
                            
                            <asp:SqlDataSource ID="dsQuoteActive" runat="server" ConnectionString="<%$ ConnectionStrings:mgmdb %>" SelectCommand="select * from Reps"></asp:SqlDataSource>

                        </asp:Panel>
                        <!-- ActivePanel -->
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlLogin" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="rblOrder" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>


        <!-- ====== -->
        <!-- ORDERS -->
        <!-- ====== -->
        <tr align="center">
            <td colspan="5">
                <asp:UpdatePanel ID="upgvOrderHistory" runat="server">
                    <ContentTemplate>

                        <asp:Panel ID="OrderPanel" runat="server">

                            <asp:HiddenField ID="OrdersSorted" runat="server" />
                            <asp:HiddenField ID="OrdersSortedDirection" runat="server" />
                            <asp:HiddenField ID="OrdersCompanySearch" runat="server" />
                            <asp:HiddenField ID="OrdersSortedColumn" runat="server" />

                            <asp:SqlDataSource ID="dsOrders" runat="server" ConnectionString="<%$ ConnectionStrings:mgmdb %>" SelectCommand="select * from Reps"></asp:SqlDataSource>

                            <asp:GridView ID="gvOrderHistory" runat="server"
                                CellPadding="4" AllowSorting="True"
                                GridLines="Vertical" AllowPaging="True" PageSize="20"
                                AutoGenerateColumns="False" DataKeyNames="QuoteID,CompanyProjectCity,Notes"
                                OnRowCommand="gvOrderHistory_RowCommand"
                                OnPageIndexChanging="gvOrderHistory_PageIndexChanging"
                                Style="margin-top: 0px" OnSorting="gvOrderHistory_Sorting" Width="100%">
                                <AlternatingRowStyle BackColor="White" ForeColor="#31659C" />
                                <Columns>
                                    <asp:TemplateField ShowHeader="false" HeaderText="QuoteID" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="QuoteID" Text='<%# Eval("QuoteID") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="QuoteNo" HeaderText="Quote<br />#" ReadOnly="True"
                                        SortExpression="QuoteNo" HtmlEncode="False">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="QuoteNoVer"
                                        HeaderText="Ver<br />#" SortExpression="QuoteNoVer" HtmlEncode="False">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="true" HeaderText=" Select"
                                        SortExpression="ProjectName" ConvertEmptyStringToNull="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="CompanyProjectCity" runat="server" CausesValidation="false"
                                                CommandName="CompanyProjectCity" CommandArgument="<%#  gvOrderHistory.DataKeys[((GridViewRow)Container).RowIndex].Value%>"
                                                Text="<%# gvOrderHistory.DataKeys[((GridViewRow)Container).RowIndex].Values[1] %>"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ControlStyle Font-Underline="true" />
                                        <HeaderStyle CssClass="GridHeaders" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Status" HeaderText="Status"
                                        SortExpression="Status">
                                        <HeaderStyle CssClass="GridHeaders" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SalesOrderNo" HeaderText="Sales Order #" ReadOnly="True"
                                        SortExpression="SalesOrderNo" HtmlEncode="False">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PurchaseOrderNo"
                                        HeaderText="Purchase Order #" ReadOnly="True"
                                        SortExpression="PurchaseOrderNo" HtmlEncode="False">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShippingInfo" HeaderText="Shipping Info" ReadOnly="True" SortExpression="ShippingInfo" HtmlEncode="False">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShipDate" DataFormatString="{0:M/dd/yy}"
                                        HeaderText="Ship<br />Date" ReadOnly="True" SortExpression="ShipDate"
                                        HtmlEncode="False">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TotalPrice" HeaderText="Price" ReadOnly="True"
                                        SortExpression="TotalPrice" DataFormatString="{0:C2}">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Notes" HeaderText="Notes"
                                        SortExpression="Notes" ReadOnly="True">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle Wrap="True" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Display_Name" HeaderText="Agent Name" SortExpression="Display_Name">
                                        <HeaderStyle CssClass="GridHeaders" />
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
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
                        </asp:Panel>
                        <!-- OrderPanel -->
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlLogin" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="rblOrder" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

</asp:Panel>
