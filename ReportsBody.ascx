<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportsBody.ascx.cs"
    Inherits="MGM_Transformer.ReportsBody" %>
<table border="0" cellpadding="0" cellspacing="5" width="100%">
    <tr align="center">
        <td>
            <h1>
                <asp:Label ID="lblReportsTitle" runat="server" Text="Reports"></asp:Label></h1>
        </td>
    </tr>
    <tr>
        <td>&nbsp;
        </td>
    </tr>
    <tr align="center">
        <td>
            <table border="0" cellpadding="0" cellspacing="5" width="70%" style="height: 200px;">
                <tr valign="top">
                    <td style="width: 230px">
                        <!-- Report Option -->
                        <asp:HiddenField ID="hidOption" runat="server" Value="Performance" Visible="false" />
                        <div id="adminbar" style="width: 230px;">
                            <asp:UpdatePanel ID="uplnkButtons" runat="server">
                                <ContentTemplate>
                                    <div id="performance" runat="server" style="padding: 10px;">
                                        <asp:LinkButton ID="lnkbPerformance" runat="server" OnClick="lnkbPerformance_Click"
                                            CssClass="lnknotselected" Width="200">Performance</asp:LinkButton>
                                        <asp:Label ID="lblPerformance" class="lnkselected" runat="server" Visible="false"
                                            Width="200">Performance</asp:Label>
                                    </div>
                                    <div id="giftcard" runat="server" style="padding: 10px; width: 100%;">
                                        <asp:LinkButton ID="lnkbGiftCard" runat="server" OnClick="lnkbGiftCard_Click" CssClass="lnknotselected"
                                            Width="200">Gift Card Promotion</asp:LinkButton>
                                        <asp:Label ID="lblGiftCard" class="lnkselected" runat="server" Visible="false" Width="200">Gift Card Promotion</asp:Label>
                                    </div>
                                    <div id="agentstockprices" runat="server" style="padding: 10px;">
                                        <asp:LinkButton ID="lnkbAgentStockPrices" runat="server" OnClick="lnkbAgentStockPrices_Click"
                                            CssClass="lnknotselected" Width="200">Agent Stock Prices</asp:LinkButton>
                                        <asp:Label ID="lblAgentStockPrices" class="lnkselected" runat="server" Visible="false"
                                            Width="200">Agent Stock Prices</asp:Label>
                                    </div>
                                    <div id="m1report" runat="server" style="padding: 10px;">
                                        <asp:LinkButton ID="lnkbM1CustSalesInfo" runat="server" OnClick="lnkbM1CustSalesInfo_Click"
                                            CssClass="lnknotselected" Width="200">M1 Customer Sales Info</asp:LinkButton>
                                        <asp:Label ID="lblM1CustSalesInfo" class="lnkselected" runat="server" Visible="false"
                                            Width="200">M1 Customer Sales Info</asp:Label>
                                    </div>
                                    <div id="pendingapprovals" runat="server" style="padding: 10px;">
                                        <asp:LinkButton ID="lnkbQuotesPendingApproval" runat="server" OnClick="lnkbQuotesPendingApproval_Click"
                                            Visible="false" CssClass="lnknotselected" Width="200">Pending Approvals</asp:LinkButton>
                                        <asp:Label ID="lblQuotesPendingApproval" class="lnkselected" runat="server" Visible="false"
                                            Width="200">Pending Approvals</asp:Label>
                                    </div>
                                    <div id="quotestatus" runat="server" style="padding: 10px;">
                                        <asp:LinkButton ID="lnkbQuoteStatus" runat="server" Visible="true" OnClick="lnkQuoteStatus_Click"
                                            CssClass="lnknotselected" Width="200">Quote Status</asp:LinkButton>
                                        <asp:Label ID="lblQuoteStatus" class="lnkselected" runat="server" Visible="false"
                                            Width="200">Quote Status</asp:Label>
                                    </div>
                                    <div id="inventory" runat="server" style="padding: 10px;">
                                        <asp:LinkButton ID="lnkbInventoryPerItem" runat="server" Visible="true" OnClick="lnkInventoryPerItem_Click"
                                            CssClass="lnknotselected" Width="200">Inventory</asp:LinkButton>
                                        <asp:Label ID="lblInventoryPerItem" class="lnkselected" runat="server" Visible="false"
                                            Width="200">Inventory</asp:Label>
                                    </div>
                                    <div id="distributordashboard" runat="server" style="padding: 10px;">
                                        <asp:LinkButton ID="lnkDistributorDashboard" runat="server"  
                                            CssClass="lnknotselected" Width="200" OnClick="lnkDistDashboard_Click">Agent Dashboard</asp:LinkButton>
                                        <asp:Label ID="lblDistributorDashboard" class="lnkselected" runat="server" Visible="False"
                                            Width="200px">Agent Dashboard</asp:Label>
                                    </div>

                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="lnkbPerformance" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lnkbGiftCard" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lnkbAgentStockPrices" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lnkbQuotesPendingApproval" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lnkbInventoryPerItem" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lnkbQuoteStatus" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lnkbInventoryPerItem" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td align="center" style="width: 450px;">
                        <!-- Conditions -->
                        <asp:UpdatePanel ID="uplblReportName" runat="server">
                            <ContentTemplate>
                                <h3>
                                    <asp:Label ID="lblReportName" runat="server" Text="Performance Report"></asp:Label></h3>
                                <asp:Panel ID="pnlCriteria" runat="server">
                                      <br />
                                      <div><asp:Label ID="lblDistDashFrom" runat="server" Text="From Year:" Visible="false"></asp:Label></div>
                                      <div><asp:DropDownList ID="ddlDistributorDashboardFromYear" runat="server" Visible="false" AutoPostBack="True" OnSelectedIndexChanged="ddlDistributorDashboardFromYear_SelectedIndexChanged"></asp:DropDownList></div><br />
                                      <div><asp:Label ID="lblDistDashTo" runat="server" Text="To Year:" Visible="false"></asp:Label></div>
                                      <div><asp:DropDownList ID="ddlDistributorDashboardToYear" runat="server" Visible="false" AutoPostBack="True" OnSelectedIndexChanged="ddlDistributorDashboardToYear_SelectedIndexChanged"></asp:DropDownList></div>
                                      <div><asp:Label ID="lblNotEnoughDataDashboard" runat="server" Text="" ForeColor="Red"></asp:Label></div>
                                    <table border="0" cellpadding="2" cellspacing="2">
                                        <tr align="left">
                                            <td width="30%">
                                                <asp:UpdatePanel ID="uptxtFromLbl" runat="server">
                                                  <ContentTemplate>
                                                     <asp:Label ID="lblFrom" runat="server" Visible="true" Text="From:"></asp:Label>
                                              </ContentTemplate>
                                             </asp:UpdatePanel>
                                            </td>
                                            <td width="70%">
                                                <asp:UpdatePanel ID="uptxtFrom" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtFrom" runat="server" Columns="10" OnTextChanged="txtFrom_TextChanged"></asp:TextBox>
                                                        <asp:Label ID="lblFromInvalid" runat="server" class="errorlabel" Visible="false"
                                                            Text="* Invalid Date."></asp:Label>
                                                        <asp:Label ID="lblFromReqd" runat="server" class="errorlabel" Visible="false" Text="* Required."></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="txtFrom" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtTo" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnPDF" EventName="Click" />
                                                        <asp:AsyncPostBackTrigger ControlID="cbDSRQuarters" EventName="CheckedChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr align="left">
                                            <td>
                                                 <asp:UpdatePanel ID="uptxtToLbl" runat="server">
                                                    <ContentTemplate>
                                                       <asp:Label ID="lblTo" runat="server" Visible="false" Text="To:"></asp:Label>
                                                  </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="uptxtTo" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtTo" runat="server" Columns="10" OnTextChanged="txtTo_TextChanged"></asp:TextBox>
                                                        <asp:Label ID="lblToInvalid" runat="server" class="errorlabel" Text="* Invalid Date."
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblToReqd" runat="server" class="errorlabel" Text="* Required." Visible="false"></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="txtFrom" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtTo" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnPDF" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:RadioButtonList ID="rblAll" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="0" Text="Current Rep"></asp:ListItem>
                                                    <asp:ListItem Value="1" Text="All Reps" Selected="True"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:RadioButtonList ID="rblProductCat" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="ALL" Text="All Products" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="AA" Text="AA"></asp:ListItem>
                                                    <asp:ListItem Value="AS" Text="AS"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <!-- Available for Internal Reps when M1 Report selected -->
                                                <div><asp:Label ID="lblInventoryExternal" runat="server" Visible="False" Font-Bold="True"></asp:Label></div>
                                                <div><asp:DropDownList ID="ddlAgents" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAgents_SelectedIndexChanged">
                                                </asp:DropDownList></div>
                                            </td>
                                        </tr>
                                          <tr>
                                            <td colspan ="2">
                                                <asp:Label ID="lblSelectAgent" runat="server" Text="" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <%-- Distributor Sales Report--%>
                                                <br />
                                                <br />
                                                <div id="DSR" runat="server">
                                                    <asp:UpdatePanel ID="upDSRReport" runat="server">
                                                        <ContentTemplate>
                                                            <fieldset>
                                                                <legend>Distributor Sales Report</legend>
                                                                <hr />
                                                                <div>
                                                                    <asp:CheckBox ID="cbUseDSR" Text="Get Distributor Sales Report" runat="server" OnCheckedChanged="cbUseDSR_CheckedChanged" AutoPostBack="True" />
                                                                    <div><asp:Label ID="lblDSRNoResults" runat="server" Text="" ForeColor="Red"></asp:Label></div>
                                                                </div>
                                                                <table style="width: 240px;">
                                                                    <tr>
                                                                        <td>
                                                                            <div>
                                                                                <asp:CheckBox ID="cbDSRQuarters" Text="Quarters" runat="server" AutoPostBack="True" OnCheckedChanged="cbDSRQuarters_CheckedChanged" Enabled="False" /></div>
                                                                            <div>
                                                                                <asp:DropDownList ID="ddlDSRYear" runat="server" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlDSRYear_SelectedIndexChanged" Enabled="False"></asp:DropDownList>
                                                                            </div>
                                                                           
                                                                        </td>
                                                                    </tr>
                                                                   
                                                                </table>
                                                            </fieldset>
                                                            <br />
                                                           
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnCSV" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>


                                                </div>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <table id="InventoryTable" runat="server" style="width: 300px">
                                                    <tr style="line-height: 20px;">
                                                        <td>
                                                            <asp:RadioButton ID="rbAllAgentInventory" runat="server" Text="All" GroupName="AgentInventory"
                                                                Checked="True" AutoPostBack="True" OnCheckedChanged="rbAllAgentInventory_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <tr style="line-height: 20px;">
                                                        <td>
                                                            <div>
                                                                <asp:RadioButton ID="rbSelectAgentInventory" runat="server" Text="Agent" GroupName="AgentInventory"
                                                                    AutoPostBack="True" OnCheckedChanged="rbSelectAgentInventory_CheckedChanged" />
                                                                <asp:DropDownList ID="ddlAgentsWithWarehouse" runat="server" Visible="True" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlAgentsWithWarehouse_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div style="text-align: center">
                                                                <asp:Label ID="lblAgentReqd" runat="server" class="errorlabel" Text="* Required."
                                                                    Visible="false"></asp:Label>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rbSearchAvailability" Text="Search" GroupName="AgentInventory"
                                                                runat="server" AutoPostBack="True" OnCheckedChanged="rbSearchAvailability_CheckedChanged" />
                                                        </td>
                                                    </tr>
                                                    <tr style="line-height: 20px;">
                                                        <td align="center">
                                                            <table id="InventorySelections" runat="server" visible="false">
                                                                <tr>
                                                                    <td>
                                                                        <fieldset>
                                                                            <legend>Windings</legend>
                                                                            <asp:RadioButtonList ID="rblWindings" runat="server" RepeatDirection="Horizontal"
                                                                                AutoPostBack="True" OnSelectedIndexChanged="rblWindings_SelectedIndexChanged">
                                                                            </asp:RadioButtonList>
                                                                        </fieldset>
                                                                        <br />
                                                                        <fieldset>
                                                                            <legend>Voltage</legend>
                                                                            <asp:DropDownList ID="ddVoltage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddVoltage_SelectedIndexChanged"
                                                                                Width="250">
                                                                            </asp:DropDownList>
                                                                        </fieldset>
                                                                        <br />
                                                                        <fieldset>
                                                                            <legend>KVA</legend>
                                                                            <asp:DropDownList ID="ddKVA" runat="server" Width="250" OnSelectedIndexChanged="ddKVA_SelectedIndexChanged">
                                                                            </asp:DropDownList>
                                                                        </fieldset>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblNoInventoryData" runat="server" Text="" ForeColor="Red"></asp:Label>

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <!-- pnlCriteria -->
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lnkbPerformance" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkbGiftCard" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkbAgentStockPrices" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkbM1CustSalesInfo" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkbQuotesPendingApproval" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkbQuoteStatus" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkbInventoryPerItem" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="cbUseDSR" EventName="CheckedChanged" />
                               <%-- <asp:AsyncPostBackTrigger ControlID="cbDSRQuarters" EventName="CheckedChanged" />--%>
                                <%--  <asp:PostBackTrigger ControlID="cbDSRQuarters" />--%>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <!-- PDF Button -->

                        <asp:UpdatePanel ID="upbtnPDF" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnPDF" runat="server" Text="PDF" Width="100" Height="50" OnClick="btnPDF_Click"
                                    AutoPostBack="true" />&nbsp;&nbsp;
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnPDF" EventName="Click" />
                             <asp:AsyncPostBackTrigger ControlID="cbUseDSR" EventName="CheckedChanged" />
                            </Triggers>
                        </asp:UpdatePanel>

                        <!-- CSV Button -->
                        <asp:UpdatePanel ID="UpdatePanelCSV" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnCSV" runat="server" Text="CSV" Width="100" Height="50" OnClick="btnCSV_Click"
                                    AutoPostBack="true" />
                            </ContentTemplate>
                            <Triggers>
                                 <asp:AsyncPostBackTrigger ControlID="hidOption" EventName="ValueChanged" />
                                <asp:PostBackTrigger ControlID="btnCSV" />
                                <asp:AsyncPostBackTrigger ControlID="cbUseDSR" EventName="CheckedChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        
                        
                         <asp:UpdateProgress ID="upProgShowQuote" runat="server" AssociatedUpdatePanelID="upbtnPDF">
                                    <ProgressTemplate>
                                        <img alt="progress" src="images/ajax-loader.gif" />Processing...
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
