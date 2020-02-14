<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminRepsBody.ascx.cs" Inherits="MGM_Transformer.AdminRepsBody" %>

<table width="100%">
    <tr align="center">
        <td colspan="2">
            <hr />
            <h1>Reps</h1>
        </td>
    </tr>
    <tr align="center">
        <td colspan="2">
            <hr />
            <asp:Button ID="btnNew" runat="server" Text="NEW Rep" Width="150" Height="30" OnClick="btnNew_Click" />&nbsp;&nbsp;
            <hr />
        </td>
    </tr>
</table>

<table>
     <tr>
        <td>
            <div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="100%" border="0" cellpadding="0" cellspacing="5">
                            <!--  Single row table. -->
                            <tr valign="top">
                                <!-- Column #1 Agent List -->

                                <td width="400">
                                    <asp:GridView ID="gvRepList" runat="server" AutoGenerateColumns="False" DataKeyNames="RepID,Full_Name"
                                        CellPadding="4" DataSourceID="dsRep" ForeColor="#333333" AllowSorting="True"
                                        GridLines="Vertical" OnRowCommand="gvRepList_RowCommand"
                                        AllowPaging="True" PageSize="20" Font-Size="X-Small"
                                        OnPageIndexChanged="gvRepList_PageIndexChanged"
                                        OnRowDataBound="gvRepList_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="RepID" HeaderText="RepID" InsertVisible="False"
                                                ReadOnly="True" SortExpression="RepID" Visible="False" />
                                            <asp:TemplateField ShowHeader="true" HeaderText=" Rep Name" SortExpression="Full_Name"
                                                ConvertEmptyStringToNull="False">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="EditRep" runat="server" CausesValidation="false"
                                                        CommandName="EditRep" CommandArgument="<%# gvRepList.DataKeys[((GridViewRow)Container).RowIndex].Value %>"
                                                        Text="<%# gvRepList.DataKeys[((GridViewRow)Container).RowIndex].Values[1] %>"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ControlStyle Font-Underline="true" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Created_on" DataFormatString="{0:d}"
                                                HeaderText="Created" SortExpression="Created_on">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Last_activity" DataFormatString="{0:d}"
                                                HeaderText="Recent" SortExpression="Last_activity">
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SecurityLevel" HeaderText="Security Level"
                                                SortExpression="SecurityLevel">
                                                <ItemStyle HorizontalAlign="Left" />
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
                                </td>
                                <!-- Column #2 Agent Detail -->
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gvRepList" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="gvRepList" EventName="RowDataBound" />
                        <asp:AsyncPostBackTrigger ControlID="gvRepList" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="gvRepList" EventName="PageIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </td>
        <td>
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="udpAgentInfo" runat="server">
                            <ContentTemplate>
                                <div id="EditRegion" runat="server">

                                    <table width="100%" border="1" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" cellpadding="2" cellspacing="2">
                                                    <tr align="center">
                                                        <td colspan="2">

                                                            <h1>
                                                                <asp:Label ID="lblTitle" runat="server" Text="Label"></asp:Label></h1>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <hr />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="150px">
                                                            <asp:HiddenField ID="hidOption" runat="server" Value="General" Visible="false" />
                                                            <div id="adminbar">
                                                                <ul>
                                                                    <li>
                                                                        <asp:LinkButton ID="lnkbGeneral" runat="server" OnClick="lnkbGeneral_Click">General</asp:LinkButton>
                                                                        <asp:Label ID="lblGeneral" class="lnkselected" runat="server" Visible="false">General</asp:Label>
                                                                    </li>
                                                                    <li>
                                                                        <asp:LinkButton ID="lnkbAgentMembers" runat="server" OnClick="lnkbAgentMembers_Click">Agent Members</asp:LinkButton>
                                                                        <asp:Label ID="lblAgentMembers" class="lnkselected" runat="server" Visible="false">Agent Members</asp:Label>
                                                                    </li>
                                                                    <li>
                                                                        <asp:LinkButton ID="lnkbLogins" runat="server" OnClick="lnkbLogins_Click">Logins</asp:LinkButton>
                                                                        <asp:Label ID="lblLogins" class="lnkselected" runat="server" Visible="False">Logins</asp:Label>
                                                                    </li>
                                                                    <li>
                                                                        <asp:LinkButton ID="lnkbIPAddresses" runat="server" OnClick="lnkbIPAddresses_Click">IP Addresses</asp:LinkButton>
                                                                        <asp:Label ID="lblIPAddresses" class="lnkselected" runat="server" Visible="False">IP Addresses</asp:Label>
                                                                    </li>
                                                                    <li>
                                                                        <asp:LinkButton ID="lnkbStockPrices" runat="server" OnClick="lnkbStockPrices_Click">Stock Prices</asp:LinkButton>
                                                                        <asp:Label ID="lblStockPrices" class="lnkselected" runat="server" Visible="False">Stock Prices</asp:Label>
                                                                    </li>
                                                                    <li>
                                                                        <asp:LinkButton ID="lnkbRepSetup" runat="server" OnClick="lnkbRepMaintenance_Click">Report Setup</asp:LinkButton>
                                                                        <asp:Label ID="lblRepSetup" class="lnkselected" runat="server" Visible="False">Report Setup</asp:Label>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <asp:Panel ID="pnlGeneral" Visible="false" runat="server">
                                                                    <!-- ==========================================
                                                                         GENERAL
                                                                         ==========================================  
                                                                    -->
                                                                    <tr align="center">
                                                                        <td colspan="2">
                                                                            <h2>General</h2>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Rep&nbsp;Name*:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtFull_Name" runat="server" AutoPostBack="true"
                                                                                OnTextChanged="txtFull_Name_TextChanged" Columns="40" MaxLength="40"></asp:TextBox>
                                                                            <asp:Label ID="lblFull_NameReqd" runat="server" Text="* Required." Visible="false" class="errorlabel"></asp:Label>
                                                                            <asp:Label ID="lblFull_NameInvalid" runat="server" Text="* Not Unique." Visible="false" class="errorlabel"></asp:Label>
                                                                            <asp:Label ID="lblFull_NameChange" runat="server" Text="* Must Change." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>MGM&nbsp;Agent&nbsp;#*:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtMGMAgentNo" runat="server" Columns="4" MaxLength="4"
                                                                                AutoPostBack="true" OnTextChanged="txtMGMAgentNo_TextChanged"></asp:TextBox>
                                                                            <asp:Label ID="lblMGMAgentNoReqd" runat="server" Text="* Required." Visible="false" class="errorlabel"></asp:Label>
                                                                            <asp:Label ID="lblMGMAgentNoInvalid" runat="server" Text="* Integer Only." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Security&nbsp;Level*:
                                                                        </td>
                                                                        <td>

                                                                            <asp:DropDownList ID="ddlSecurityLevel" runat="server" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="ddlSecurityLevel_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                            <asp:Label ID="lblSecurityLevelReqd" runat="server" Text="* Required." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">Price Multiplier*:
                                                                            <asp:TextBox ID="txtPriceMultiplier" runat="server" MaxLength="4"
                                                                                    OnTextChanged="txtPriceMultiplier_TextChanged" Columns="4" AutoPostBack="true"></asp:TextBox>
                                                                            <asp:Label ID="lblPriceMultiplierRules" runat="server">(+/- 20% max) (1.00 = No multiplier.)</asp:Label>
                                                                            <asp:Label ID="lblPriceMultiplierReqd" runat="server" Text="* Required." Visible="false" class="errorlabel"></asp:Label>
                                                                            <asp:Label ID="lblPriceMultiplierInvalid" runat="server" Text="* Does not meet price multiplier rules." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Lead times are
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtLeadTimes" runat="server" Columns="2" MaxLength="2"
                                                                                AutoPostBack="true" OnTextChanged="txtLeadTimes_TextChanged"></asp:TextBox>
                                                                            &nbsp;# of working days.  99=Standard.
                                                         
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Email:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtEmail" runat="server" Columns="30" MaxLength="100" AutoPostBack="true"
                                                                                OnTextChanged="txtEmail_TextChanged"></asp:TextBox>
                                                                            <asp:Label ID="lblEmailInvalid" runat="server" Text="* Invalid." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Phone:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtPhone" runat="server" Columns="21" MaxLength="21"
                                                                                OnTextChanged="txtPhone_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                            <asp:Label ID="lblPhoneInvalid" runat="server" Text="* Does not meet phone rules." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Rep ID:
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblRepID" runat="server" class="textlabel" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Created:
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblCreated_on" runat="server" class="textlabel"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Last Activity:
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblLast_activity" runat="server" class="textlabel"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" align="center">
                                                                            <asp:Button ID="btnGeneralUpdate" runat="server" Text="Update" Width="100" Height="30" OnClick="btnGeneralUpdate_Click" />&nbsp;&nbsp;
                                                                            <asp:Button ID="btnGeneralCancel" runat="server" Text="Cancel" Width="100" Height="30" OnClick="btnGeneralCancel_Click" />&nbsp;&nbsp;
                                                                            <asp:Button ID="btnGeneralDelete" runat="server" Text="Delete" Width="100" Height="30" OnClick="btnGeneralDelete_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>

                                                                            <asp:Label ID="lblVerifyDelete" runat="server" Text="Label" Visible="false" class="errorlabel">Are you sure?&nbsp;</asp:Label>

                                                                        </td>
                                                                        <td>

                                                                            <asp:RadioButtonList ID="rblVerifyDelete" runat="server" Visible="false" AutoPostBack="true"
                                                                                RepeatDirection="Horizontal"
                                                                                OnSelectedIndexChanged="rblVerifyDelete_SelectedIndexChanged">
                                                                                <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                                                                <asp:ListItem Value="No">No</asp:ListItem>
                                                                            </asp:RadioButtonList>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" align="center">

                                                                            <asp:Label ID="lblReqdField" runat="server">* Required field.</asp:Label>
                                                                            <asp:Label ID="lblErrors" runat="server" Text="* Please correct errors as shown." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                </asp:Panel>
                                                                <!-- General -->
                                                                <asp:Panel ID="pnlAgentMembers" Visible="false" runat="server">
                                                                    <!-- ==========================================
                                                                         AGENT MEMBERS
                                                                         ==========================================  
                                                                    -->
                                                                    <tr align="center">
                                                                        <td colspan="2">
                                                                            <h2>

                                                                                <asp:Label ID="lblAgentMembersTitle" runat="server" Text="Agent Members"></asp:Label>

                                                                            </h2>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">

                                                                            <asp:GridView ID="gvDistributors" runat="server" AutoGenerateColumns="False" DataKeyNames="RepID,Distributor"
                                                                                CellPadding="4" ForeColor="#333333" GridLines="Vertical" AllowSorting="True"
                                                                                OnRowCommand="gvDistributors_RowCommand" DataSourceID="dsDistributors" AllowPaging="True"
                                                                                Font-Size="X-Small" OnRowDataBound="gvDistributors_RowDataBound">
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="RepID" HeaderText="RepID" InsertVisible="False"
                                                                                        ReadOnly="True" SortExpression="RepID" Visible="False" />
                                                                                    <asp:BoundField DataField="Distributor" HeaderText="Distributor" InsertVisible="False"
                                                                                        ReadOnly="True" SortExpression="Distributor" Visible="True" />
                                                                                    <asp:BoundField DataField="Type" HeaderText="Type" ReadOnly="True" SortExpression="Type" />
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
                                                                            <asp:SqlDataSource ID="dsDistributors" runat="server"
                                                                                ConnectionString="<%$ ConnectionStrings:mgmdb %>"
                                                                                SelectCommand="usp_Distributor_List" SelectCommandType="StoredProcedure">
                                                                                <SelectParameters>
                                                                                    <asp:SessionParameter DefaultValue="2" Name="rep_id" SessionField="RepID"
                                                                                        Type="Int32" />
                                                                                </SelectParameters>
                                                                            </asp:SqlDataSource>

                                                                        </td>
                                                                    </tr>
                                                                </asp:Panel>
                                                                <!-- Agent Members -->
                                                                <asp:Panel ID="pnlLogins" Visible="false" runat="server">
                                                                    <!-- ==========================================
                                                                         LOGINS
                                                                         ==========================================  
                                                                    -->
                                                                    <tr align="center">
                                                                        <td colspan="2">
                                                                            <h2>Logins</h2>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">

                                                                            <asp:GridView ID="gvLogins" runat="server" AutoGenerateColumns="False"
                                                                                DataKeyNames="UserName,Password,FullName,Last_Activity,Created_on" CellPadding="4" ForeColor="#333333" GridLines="Vertical" AllowSorting="True"
                                                                                DataSourceID="dsLogins" AllowPaging="True" Font-Size="X-Small" OnRowCommand="gvLogins_RowCommand">
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <Columns>
                                                                                    <asp:TemplateField ShowHeader="true" HeaderText="Edit" SortExpression="IP_Address"
                                                                                        ConvertEmptyStringToNull="False">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="Edit" runat="server" CausesValidation="false"
                                                                                                CommandName="Select" CommandArgument="<%# gvLogins.DataKeys[((GridViewRow)Container).RowIndex].Value %>"
                                                                                                Text="Edit"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <ControlStyle Font-Underline="true" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ShowHeader="true" HeaderText="Delete" SortExpression="IP_Address"
                                                                                        ConvertEmptyStringToNull="False">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="Delete" runat="server" CausesValidation="false"
                                                                                                CommandName="Delete" CommandArgument="<%# gvLogins.DataKeys[((GridViewRow)Container).RowIndex].Value %>"
                                                                                                Text="Delete"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <ControlStyle Font-Underline="true" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" ReadOnly="true" />
                                                                                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" ReadOnly="true" />
                                                                                    <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" ReadOnly="true" />
                                                                                    <asp:BoundField DataField="PhoneType" HeaderText="Type" SortExpression="PhoneType" ReadOnly="true" />
                                                                                    <asp:BoundField DataField="PhoneProgressCode" HeaderText="Setup" SortExpression="PhoneProgressCode" ReadOnly="true" />
                                                                                    <asp:BoundField DataField="VPN_IP" HeaderText="VPN IP" SortExpression="VPN_IP" ReadOnly="true" />
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
                                                                            <asp:SqlDataSource ID="dsLogins" runat="server"
                                                                                ConnectionString="<%$ ConnectionStrings:mgmdb %>"
                                                                                SelectCommand="usp_Logins" SelectCommandType="StoredProcedure">
                                                                                <SelectParameters>
                                                                                    <asp:SessionParameter DefaultValue="2" Name="rep_id" SessionField="RepID"
                                                                                        Type="Int32" />
                                                                                </SelectParameters>
                                                                            </asp:SqlDataSource>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>User Name*:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtUserName" runat="server" Columns="20" MaxLength="20"
                                                                                OnTextChanged="txtUserName_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                            &nbsp;&nbsp;<asp:Label ID="Label1" runat="server">Length >= 3.</asp:Label>
                                                                            <asp:Label ID="lblUserNameReqd" runat="server" Text="* Required." Visible="false" class="errorlabel"></asp:Label>
                                                                            <asp:Label ID="lblUserNameInvalid" runat="server" Text="* Does not meet user name rules." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Full Name*:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtFullNameLogin" runat="server" Columns="20" MaxLength="20"
                                                                                OnTextChanged="txtFullNameLogin_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                            &nbsp;&nbsp;<asp:Label ID="Label2" runat="server">Length >= 3.</asp:Label>
                                                                            <asp:Label ID="lblFullNameLoginReqd" runat="server" Text="* Required." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Password*:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtPassword" runat="server" Columns="20" MaxLength="20"
                                                                                OnTextChanged="txtPassword_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                            &nbsp;&nbsp;<asp:Label ID="lblPasswordRules" runat="server">Length >= 5.</asp:Label>
                                                                            <asp:Label ID="lblPasswordReqd" runat="server" Text="* Required." Visible="false" class="errorlabel"></asp:Label>
                                                                            <asp:Label ID="lblPasswordInvalid" runat="server" Text="* Does not meet password rules." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Email:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtLoginEmail" runat="server" Columns="30" MaxLength="100" AutoPostBack="true"
                                                                                OnTextChanged="txtLoginEmail_TextChanged"></asp:TextBox>
                                                                            <asp:Label ID="lblLoginEmailInvalid" runat="server" Text="* Invalid." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Phone:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtLoginPhone" runat="server" Columns="30" MaxLength="100" AutoPostBack="true"
                                                                                OnTextChanged="txtLoginPhone_TextChanged"></asp:TextBox>
                                                                            <asp:Label ID="lblLoginPhoneInvalid" runat="server" Text="* Invalid." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Phone Type:
                                                                        </td>
                                                                        <td>

                                                                            <asp:Label ID="lblPhoneType" runat="server" Text="" class="textlabel" AutoPostBack="true"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Setup Status:
                                                                        </td>
                                                                        <td>

                                                                            <asp:Button runat="server" ID="btnGrantAccess" OnClick="btnGrantAccess_Click" Text="Grant Access" Visible="false"></asp:Button>
                                                                            <asp:Label runat="server" ID="lblPhoneStatus" Text="" class="textlabel" AutoPostBack="true"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>VPN IP Address:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtVPN_IP" runat="server" Columns="19" MaxLength="19" AutoPostBack="true"
                                                                                OnTextChanged="txtVPN_IP_TextChanged"></asp:TextBox>
                                                                            <asp:Label ID="lblVPN_IPInvalid" runat="server" Text="* Invalid." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Created:
                                                                        </td>
                                                                        <td>

                                                                            <asp:Label ID="lblLoginCreated" runat="server" class="textlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Last Activity:
                                                                        </td>
                                                                        <td>

                                                                            <asp:Label ID="lblLoginLastActivity" runat="server" class="textlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" align="center">

                                                                            <asp:Button ID="btnLoginAdd" runat="server" Text="Add Login" Width="100" Height="30" OnClick="btnLoginAdd_Click" Visible="true" />
                                                                            <asp:Button ID="btnLoginUpdate" runat="server" Text="Update" Width="80" Height="30" OnClick="btnLoginUpdate_Click" Visible="false" />
                                                                            <asp:Button ID="btnLoginCancel" runat="server" Text="Cancel" Width="80" Height="30" OnClick="btnLoginCancel_Click" Visible="false" />
                                                                            <asp:Button ID="btnLoginDelete" runat="server" Text="Delete" Width="80" Height="30" OnClick="btnLoginDelete_Click" Visible="false" />

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" align="center">

                                                                            <asp:Label ID="lblLoginReqd" runat="server">* Required field.</asp:Label>
                                                                            <asp:Label ID="lblLoginErrors" runat="server" Text="* Please correct errors as shown." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                </asp:Panel>
                                                                <!-- Logins -->
                                                                <asp:Panel ID="pnlIPAddresses" Visible="false" runat="server">
                                                                    <!-- ==========================================
                                                                         IP ADDRESSES
                                                                         ==========================================  
                                                                    -->
                                                                    <tr align="center">
                                                                        <td colspan="2">
                                                                            <h2>IP Addresses</h2>
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="center">
                                                                        <td colspan="2">(For VPN IP addresses, use the Logins tab instead.)
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">

                                                                            <asp:GridView ID="gvRepIPs" runat="server" AutoGenerateColumns="False" DataKeyNames="IP_Address"
                                                                                CellPadding="4" ForeColor="#333333" GridLines="Vertical" AllowSorting="true"
                                                                                OnRowCommand="gvRepIPs_RowCommand" DataSourceID="dsRepIPs" AllowPaging="True"
                                                                                Font-Size="X-Small" OnRowDataBound="gvRepIPs_RowDataBound">
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <Columns>
                                                                                    <asp:TemplateField ShowHeader="true" HeaderText="Edit" SortExpression="IP_Address"
                                                                                        ConvertEmptyStringToNull="False">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="Edit" runat="server" CausesValidation="false"
                                                                                                CommandName="Select" CommandArgument="<%# gvRepIPs.DataKeys[((GridViewRow)Container).RowIndex].Value %>"
                                                                                                Text="Edit"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <ControlStyle Font-Underline="true" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField ShowHeader="true" HeaderText="Delete" SortExpression="IP_Address"
                                                                                        ConvertEmptyStringToNull="False">
                                                                                        <ItemTemplate>
                                                                                            <asp:LinkButton ID="Delete" runat="server" CausesValidation="false"
                                                                                                CommandName="Delete" CommandArgument="<%# gvRepIPs.DataKeys[((GridViewRow)Container).RowIndex].Value %>"
                                                                                                Text="Delete"></asp:LinkButton>
                                                                                        </ItemTemplate>
                                                                                        <ControlStyle Font-Underline="true" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:BoundField DataField="IP_Address" HeaderText="IP Address"
                                                                                        SortExpression="IP_Address">
                                                                                        <ItemStyle Width="100px" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Description" HeaderText="Description"
                                                                                        SortExpression="Description">
                                                                                        <ItemStyle Width="200px" />
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
                                                                            <asp:SqlDataSource ID="dsRepIPs" runat="server"
                                                                                ConnectionString="<%$ ConnectionStrings:mgmdb %>"
                                                                                SelectCommand="usp_RepIP_Select" SelectCommandType="StoredProcedure">
                                                                                <SelectParameters>
                                                                                    <asp:SessionParameter DefaultValue="2" Name="RepID" SessionField="RepID" Type="Int32" />
                                                                                </SelectParameters>
                                                                            </asp:SqlDataSource>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>New&nbsp;IP&nbsp;Address:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtIPAddress" runat="server" Columns="15" MaxLength="15"
                                                                                OnTextChanged="txtIPAddress_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                            <asp:Label ID="lblIPAddressReqd" runat="server" Text="* Required." Visible="false" class="errorlabel"></asp:Label>
                                                                            <asp:Label ID="lblIPAddressInvalid" runat="server" Text="* Does not meet IP address rules." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Description:
                                                                        </td>
                                                                        <td>

                                                                            <asp:TextBox ID="txtIPDescription" runat="server" Columns="20" MaxLength="50" AutoPostBack="true"></asp:TextBox>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td>

                                                                            <asp:Button ID="btnAddIP" runat="server" Text="Add IP Address" Width="150" Height="30" OnClick="btnAddIP_Click" Visible="true" />
                                                                            <asp:Button ID="btnUpdateIP" runat="server" Text="Update" Width="80" Height="30" OnClick="btnUpdateIP_Click" Visible="false" />
                                                                            <asp:Button ID="btnCancelIP" runat="server" Text="Cancel" Width="80" Height="30" OnClick="btnCancelIP_Click" Visible="false" />
                                                                            <asp:Button ID="btnDeleteIP" runat="server" Text="Delete" Width="80" Height="30" OnClick="btnDeleteIP_Click" Visible="false" />
                                                                            <asp:Label ID="lblAddIPErrors" runat="server" Text="* Not performed." Visible="false" class="errorlabel"></asp:Label>
                                                                            <asp:Label ID="lblAddIPOkay" runat="server" Text="&nbsp;&nbsp;IP Added." Visible="false" class="errorlabel"></asp:Label>

                                                                        </td>
                                                                    </tr>
                                                                </asp:Panel>
                                                                <!-- IP Addresses -->
                                                                <!-- ==========================================
                                                                     STOCK PRICES
                                                                     ==========================================  
                                                                -->
                                                                <asp:Panel ID="pnlStockPrices" Visible="false" runat="server">
                                                                    <tr align="center">
                                                                        <td colspan="2">
                                                                            <h2>Stock Prices</h2>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" align="center">

                                                                            <asp:GridView ID="gvRepProductPrices" runat="server" AllowPaging="True" PageSize="15" DataKeyNames="StockRepID"
                                                                                AllowSorting="True" AutoGenerateColumns="False" AutoEditColumns="True"
                                                                                CellPadding="4" DataSourceID="dsRepProductPrices" ForeColor="#333333"
                                                                                GridLines="None" OnRowCancelingEdit="gvRepProductPrices_RowCancelingEdit" OnRowUpdating="gvRepProductPrices_RowUpdating"
                                                                                OnRowUpdated="gvRepProductPrices_RowUpdated"
                                                                                OnRowEditing="gvRepProductPrices_RowEditing" Font-Size="X-Small"
                                                                                OnRowDataBound="gvRepProductPrices_RowDataBound">
                                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                                <Columns>
                                                                                    <asp:CommandField ShowEditButton="True"></asp:CommandField>
                                                                                    <asp:TemplateField HeaderText="Price" SortExpression="Price">
                                                                                        <ItemTemplate>
                                                                                            <%#Eval("PriceFormatted") %>
                                                                                        </ItemTemplate>
                                                                                        <EditItemTemplate>
                                                                                            <asp:TextBox ID="txtPrice" runat="server" Text='<%#Eval("Price") %>'></asp:TextBox>
                                                                                        </EditItemTemplate>
                                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="StockRepID" Visible="false">
                                                                                        <ItemTemplate>
                                                                                            <%#Eval("StockRepID") %>
                                                                                        </ItemTemplate>
                                                                                        <EditItemTemplate>
                                                                                            <asp:Label ID="lblStockRepID" runat="server" Text='<%#Eval("StockRepID") %>'></asp:Label>
                                                                                        </EditItemTemplate>
                                                                                    </asp:TemplateField>

                                                                                    <asp:BoundField ApplyFormatInEditMode="True" DataField="CatalogNumber"
                                                                                        HeaderText="Catalog Number" ReadOnly="True" SortExpression="CatalogNumber" />
                                                                                    <asp:BoundField ApplyFormatInEditMode="True" DataField="AltNumber"
                                                                                        HeaderText="Alt Number" ReadOnly="True" SortExpression="AltNumber" />
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
                                                                <!-- Stock Prices -->

                                                                <%#Eval("PriceFormatted") %>
                                                            </table>

                                                            <asp:Panel ID="pnlRepMaint" runat="server">

                                                                <table>

                                                                    <tr align="center">
                                                                        <td>
                                                                            <h2>Report Setup</h2>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <hr />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="margin-right: 0px;">
                                                                            <div>
                                                                                <asp:Label ID="lblAgentName" runat="server" Text="Agent Name"></asp:Label>
                                                                            </div>
                                                                            <div>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="tbAgentPerson" runat="server" Width="300"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Button ID="btnUpdateReportPerson" runat="server" Text="Update" OnClick="UpdateReportEMailPerson_Click" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <div style="margin: 5px;">
                                                                                <asp:Label ID="lblEMailPersonError" runat="server" Text=""></asp:Label>
                                                                            </div>

                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="margin-right: 0px;">
                                                                            <div>
                                                                                <asp:Label ID="lblReportEMail" runat="server" Text="E-Mail Address for Reports"></asp:Label>
                                                                            </div>
                                                                            <div>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="tbAgentReportEMail" runat="server" Width="300"></asp:TextBox>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Button ID="btnUpdateReportEMail" runat="server" Text="Update" OnClick="UpdateReportEMail_Click" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <div style="margin: 5px;">

                                                                                <asp:Label ID="lblEMail" runat="server" Text=""></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="margin-right: 0px;">
                                                                            <fieldset>
                                                                                <legend>Goal Amount</legend>
                                                                                <div>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblDollarSign" runat="server" Text="$"></asp:Label><asp:TextBox ID="tbGoalAmount" runat="server" Width="300"></asp:TextBox>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Button ID="btnUpdateGoal" runat="server" Text="Update" OnClick="UpdateReportGoalAmount_Click" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <asp:Label ID="lblGoalYear" runat="server" Text="Goal Year" Width="100"></asp:Label>
                                                                                                <span style="padding-right: 10px;">
                                                                                                    <asp:DropDownList ID="ddGoalYear" runat="server" Width="200"></asp:DropDownList></span>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <asp:Label ID="lblGoalName" runat="server" Text="Goal Name" Width="100"></asp:Label>
                                                                                                <span style="padding-right: 10px;">
                                                                                                    <asp:DropDownList ID="ddGoalName" runat="server" Width="200"></asp:DropDownList></span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </fieldset>
                                                                            <div style="margin: 5px;">
                                                                                <asp:Label ID="lblErrorGoalAmount" runat="server" Text=""></asp:Label>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- outline table -->
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="gvRepList" EventName="RowCommand" />
                                <asp:AsyncPostBackTrigger ControlID="gvRepList" EventName="RowDataBound" />
                                <asp:AsyncPostBackTrigger ControlID="gvRepList" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="gvRepList" EventName="PageIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>

                        <!-- Edit Region -->
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<asp:SqlDataSource ID="dsRep" runat="server" 
    ConnectionString="<%$ ConnectionStrings:mgmdb %>" 
    SelectCommand="usp_Rep_List" SelectCommandType="StoredProcedure">
</asp:SqlDataSource>
<asp:sqldatasource ID="dsRepProductPrices" runat="server" 
    ConnectionString="<%$ ConnectionStrings:mgmdb %>" 
    SelectCommand="usp_Rep_ProductPrices" SelectCommandType="StoredProcedure"
    UpdateCommand="usp_StockRepXRef_Price_Update" UpdateCommandType="StoredProcedure">
    <SelectParameters>
        <asp:SessionParameter DefaultValue="0" Name="RepID" SessionField="RepID" 
            Type="Int32" />
    </SelectParameters>
    <UpdateParameters>
        <asp:SessionParameter DefaultValue="0" Name="StockRepID" SessionField="RepDistributorID" 
            Type="Int32" />
        <asp:SessionParameter DefaultValue="0" Name="Price" SessionField="Price" 
            Type="Int32" />
    </UpdateParameters>
</asp:sqldatasource>

