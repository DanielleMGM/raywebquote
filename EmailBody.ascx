<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailBody.ascx.cs" Inherits="MGM_Transformer.EmailBody" %>
<table width="100%" border="0" cellpadding="0" cellspacing="5">
    <tr align="center">
        <td>
            <asp:UpdatePanel ID="upTitle" runat="server">
            <ContentTemplate>
                <h2><asp:Label ID="lblTitle" runat="server" Text="Email a Quote"></asp:Label></h2>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="rblQuoteOrSubmittal" EventName="SelectedIndexChanged" />
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
    <!-- Inner table 4 columns -->
    <table border="0" cellpadding="2" cellspacing="2" align="center">
        <tr>
            <td colspan="2" align="left">
                <asp:UpdatePanel ID="upQuoteOrSubmittal" runat="server">
                <ContentTemplate>
                    <asp:RadioButtonList ID="rblQuoteOrSubmittal" runat="server" AutoPostBack="true"
                        RepeatDirection="Horizontal" OnSelectedIndexChanged="rblQuoteOrSubmittal_SelectedIndexChanged">
                        <asp:ListItem Value="0" Selected="True">Quote</asp:ListItem>
                        <asp:ListItem Value="1">Submittal</asp:ListItem>
                    </asp:RadioButtonList>
                </ContentTemplate>
                <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="rblQuoteOrSubmittal" EventName="SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>
            </td>
            <td colspan="2">
                <asp:UpdatePanel ID="uplblSubmitInvalid" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblSubmittalInvalid" runat="server" cssclass="errorlabel" 
                        Text="Submittal not yet fully defined." Visible="false"></asp:Label>
                </ContentTemplate>
                <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="rblQuoteOrSubmittal" EventName="SelectedIndexChanged" />
                </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    <tr>
        <td colspan="4">
            <hr />
        </td>
    </tr>
    <tr align="left" valign="top">
        <td width="50px">Date:
        </td>
        <td>
            <asp:Label ID="lblDate" runat="server" cssclass="textlabel"></asp:Label>
        </td>
        <td>
         </td>
        <td rowspan="9">
            <table>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="upBtnSend" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSend" runat="server" Text="SEND" width="100px" height="60px" 
                                onclick="btnSend_Click" OnClientClick="this.disabled = true;"
                                UseSubmitBehavior="false" TabIndex="8" /><br />
                          <%--  <asp:Button ID="btnSendTest" runat="server" Text="Send Test" width="100px" height="20px" 
                                OnClientClick="this.disabled = true;"
                                UseSubmitBehavior="false" TabIndex="8" onclick="btnSendTest_Click" /><br />--%>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSend" EventName="Click" />
                        </Triggers>
                        </asp:UpdatePanel>    
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:LinkButton ID="btnReturn" runat="server" onclick="btnReturn_Click" 
                            PostBackUrl="~/Quote.aspx">Return</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdateProgress ID="upProgEmail" runat="server">
                            <ProgressTemplate>
                                <img alt="progress" src="images/ajax-loader.gif" />Processing...
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr align="left">
        <td></td>
        <td><u>Name</u></td>
        <td><u>Email</u></td>
    </tr>
    <tr align="left">
        <td>From:
        </td>
        <td>
            <asp:Label ID="lblFromName" runat="server" Text="Rep Name" cssclass="textlabel"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblFromEmail" runat="server" Text="Rep Email" cssclass="textlabel"></asp:Label>
        </td>
    </tr>
    <tr align="left">
        <td colspan="2"></td>
        <td>
            <asp:Label ID="Label1" runat="server" Text="A copy will be sent to the above email."></asp:Label>
        </td>
    </tr>
    <tr valign="top" align="left">
        <td>To:
        </td>
        <td>
            <asp:UpdatePanel ID="uptxtToName" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtToName" runat="server" MaxLength="200" Columns="30" 
                    AutoPostBack="true" ontextchanged="txtToName_TextChanged" TabIndex="1"></asp:TextBox><br />(name only)
                <asp:Label ID="lblToName" runat="server" Visible="false" cssclass="textlabel"></asp:Label>
                <asp:Label ID="lblToNameReqd" runat="server" Text="* Required." Visible="false" cssclass="errorlabel"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtToName" EventName = "TextChanged" />
            </Triggers>
            </asp:UpdatePanel>    
        </td>
        <td>
            <asp:UpdatePanel ID="uptxtToEmail" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtToEmail" runat="server" MaxLength="200" Columns="40" 
                    AutoPostBack="true" ontextchanged="txtToEmail_TextChanged" TabIndex="2"></asp:TextBox><br />(email address)
                <asp:Label ID="lblToEmail" runat="server" Visible="false" cssclass="textlabel"></asp:Label>
                <asp:Label ID="lblToEmailReqd" runat="server" Text="* Required." Visible="false" cssclass="errorlabel"></asp:Label>
                <asp:Label ID="lblToEmailInvalid" runat="server" Text="* Does not meet email rules." Visible="false" cssclass="errorlabel"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtToEmail" EventName = "TextChanged" />
            </Triggers>
            </asp:UpdatePanel>    
        </td>
    </tr>
    <tr align="left">
        <td>cc:
        </td>
        <td>
            <asp:UpdatePanel ID="uptxtCCName" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtCCName" runat="server" MaxLength="200" Columns="30"  
                    AutoPostBack="true" ontextchanged="txtCName_TextChanged" TabIndex="3"></asp:TextBox>
                <asp:Label ID="lblCCName" runat="server" Visible="false" cssclass="textlabel"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtCCName" EventName = "TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtCCEmail" EventName = "TextChanged" />
            </Triggers>
            </asp:UpdatePanel>    
        </td>
        <td>
            <asp:UpdatePanel ID="uptxtCCEmail" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtCCEmail" runat="server" MaxLength="200" Columns="40" 
                    AutoPostBack="true" ontextchanged="txtCCEmail_TextChanged" TabIndex="4"></asp:TextBox>
                <asp:Label ID="lblCCEmail" runat="server" Visible="false" cssclass="textlabel"></asp:Label>
                <asp:Label ID="lblCCEmailReqd" runat="server" Text="* Required if cc: name entered." Visible="false" cssclass="errorlabel"></asp:Label>
                <asp:Label ID="lblCCEmailInvalid" runat="server" Text="* Does not meet email rules." Visible="false" cssclass="errorlabel"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtCCName" EventName = "TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtCCEmail" EventName = "TextChanged" />
            </Triggers>
            </asp:UpdatePanel>    
        </td>
    </tr>
    <tr align="left">
        <td></td>
        <td>
            <asp:CheckBox ID="chkNoPrices" runat="server" Text="No Prices" />
         </td>
        <td><asp:Label ID="lblCopyAssociate" ForeColor="Blue" runat="server">(Okay to use semicolon(;) between emails for additional recipients.)</asp:Label></td>
    </tr>
    <tr align="left">
        <td>Subject:
        </td>
        <td colspan="2">
            <asp:UpdatePanel ID="uptxtSubject" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtSubject" runat="server" MaxLength="200" Columns="80" 
                    AutoPostBack="true" ontextchanged="txtSubject_TextChanged" TabIndex="6"></asp:TextBox>
                <asp:Label ID="lblSubject" runat="server" Text="" Visible="false"></asp:Label>
                <asp:Label ID="lblSubjectReqd" runat="server" Text="* Required." Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtSubject" EventName = "TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="rblQuoteOrSubmittal" EventName="SelectedIndexChanged" />
            </Triggers>
            </asp:UpdatePanel>    
        </td>
    </tr>
    <tr valign="top" align="left">
        <td>Body:
        </td>
        <td valign="top" colspan="2">
            <asp:UpdatePanel ID="uptxtBody" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtBody" runat="server" MaxLength="2000" Columns="80" 
                    Height="200" TextMode="MultiLine" AutoPostBack="true"
                    ontextchanged="txtBody_TextChanged" TabIndex="7"></asp:TextBox>
                <asp:Label ID="lblBody" runat="server" Visible="false" cssclass="textlabel" Height="100"></asp:Label>
                <asp:Label ID="lblBodyReqd" runat="server" Text="* Required." Visible="false" cssclass="errorlabel"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtBody" EventName = "TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtToEmail" EventName = "TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="rblQuoteOrSubmittal" EventName="SelectedIndexChanged" />
            </Triggers>
            </asp:UpdatePanel>    
        </td>
    </tr>
    <tr>
        <td colspan="3" align="center">
            <asp:UpdatePanel ID="uplblErrors" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblErrors" runat="server" Text="* Please correct error(s) above." Visible="false" cssclass="errorlabel"></asp:Label>
                <asp:Label ID="lblServerErrors" runat="server" Text="The server was unable to send this email.  Please try again later." Visible="false" cssclass="errorlabel"></asp:Label>
                <asp:Label ID="lblSent" runat="server" Text="The email was sent successfully." Visible="false" cssclass="statuslabel"></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtToName" EventName = "TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtToEmail" EventName = "TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtCCEmail" EventName = "TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtSubject" EventName = "TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtBody" EventName = "TextChanged" />
            </Triggers>
            </asp:UpdatePanel>    
        </td>
    </tr>
    <tr>
        <td colspan="3" align="center">
            <hr />
        </td>
    </tr>
    <tr>
        <td colspan="3" align="center">
            <asp:UpdatePanel ID="upEmailTitle" runat="server">
                <ContentTemplate>
                    <h3><asp:Label ID="lblEmailTitle" runat="server" Text="Email History"></asp:Label></h3>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rblQuoteOrSubmittal" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>    
        </td>
    </tr>
    <tr>
        <td colspan="3" align="center">
            <asp:UpdatePanel ID="upQuoteHistory" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvQuoteHistory" runat="server"
                    CellPadding="4" ForeColor="#333333" AllowSorting="True" 
                    GridLines="Vertical" AllowPaging="True" PageSize="20" 
                    AutoGenerateColumns="False" onrowdatabound="gvQuoteHistory_RowDataBound">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="SentDate" DataFormatString="{0:M/d/yy}" 
                                    HeaderText="Date Sent" SortExpression="SentDate" >
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EmailTo" HeaderText="Emailed To" 
                                    SortExpression="EmailTo" />
                                <asp:BoundField DataField="EmailToName" HeaderText="Emailed To Name" 
                                    SortExpression="EmailToName" />
                                <asp:BoundField DataField="EmailCC" HeaderText="Copied To" 
                                    SortExpression="EmailCC" />
                                <asp:BoundField DataField="EmailCCName" HeaderText="Copied To Name" 
                                    SortExpression="EmailCCName" />
                                <asp:CheckBoxField DataField="CopyAssociate" HeaderText="Rep Copied" 
                                    SortExpression="CopyAssociate">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CheckBoxField>
                                <asp:CheckBoxField DataField="IsNoPrice" HeaderText="No Price" 
                                    SortExpression="IsNoPrice">
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
                <asp:SqlDataSource ID="dsEmailHistory" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:mgmdb %>" 
                    SelectCommand="usp_Email_History_20180206" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:SessionParameter Name="quote_id" SessionField="QuoteID" Type="Int32" DefaultValue="0" />
                        <asp:SessionParameter Name="is_submittal" SessionField="IsSubmittal" Type="Int16" DefaultValue="0" />
                    </SelectParameters>
                </asp:SqlDataSource>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="rblQuoteOrSubmittal" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>    
        </td>
    </tr>
</table>                <!-- Inner table -->
</td></tr></table>      <!-- Outer table -->

