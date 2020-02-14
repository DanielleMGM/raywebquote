<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubmittalBody.ascx.cs" Inherits="MGM_Transformer.SubmittalBody" %>
<script type="text/javascript">
    // Counts number of characters in a textbox.
    // Requires a control named lblTextBoxErrorMsg.
    // Used to keep SubmittalNotes text box the correct length, because MultiLine text box doesn't honor MaxLength.
    function TextBoxCount(text,long)
    {
        // NOTE: Because the UpdatePanels aren't fired by this JavaScript, the error message never shows up.
	    var maxlength = new Number(long); // Change number to your max length.
	    if (text.value.length > maxlength) {
	        text.value = text.value.substring(0, maxlength);
	        document.getElementById("lblTextBoxErrorMsg").innerHTML = "Only " + long + " characters permitted.";
	    } else {
	        document.getElementById("lblTextBoxErrorMsg").innerHTML = "";
	}
}
</script>

<table width="100%" border="0" cellpadding="0" cellspacing="5">
    <tr align="center">
        <td>
            <h2><asp:Label ID="lblTitle" runat="server" Text="Submittals"></asp:Label></h2>
        </td>
    </tr>
    <tr>
        <td>
            <hr />
        </td>
    </tr>
    <tr>
    <td>
    <!-- Table 3 columns, top portion of screen only -->
    <table border="0" cellpadding="2" cellspacing="2" align="center">
    <tr align="left" valign="top">
        <td>Quote:
        </td>
        <td><h2>
                <asp:Label ID="lblQuoteNo" runat="server" Text="WQ23456.2"></asp:Label>
            </h2>
        </td>
        <td rowspan="3">
            <asp:UpdatePanel ID="upRunSubmittal" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnPreview" runat="server" Text="Preview" width="100px" height="60px" 
                                    onclick="btnPreview_Click" TabIndex="8" style="font-size: medium" /><br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:LinkButton ID="btnReturn" runat="server" onclick="btnReturn_Click">Return</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPreview" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
        <td rowspan="3"><asp:Label ID="lblOEM" runat="server" Visible="false">(OEM)</asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblPurchaseOrderNo" runat="server">Purchase Order #:</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtPurchaseOrderNo" runat="server" 
                style="text-transform:uppercase;" maxlength="20" Text="" 
                ontextchanged="txtPurchaseOrderNo_TextChanged"></asp:TextBox>
            </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblSalesOrderNo" runat="server">Sales Order #:</asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtSalesOrderNo" runat="server" 
                style="text-transform:uppercase;" maxlength="9" Text="" 
                ontextchanged="txtSalesOrderNo_TextChanged"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblSubmittalType" runat="server">Submittal Type:</asp:Label>
        </td>
        <td>
            <asp:DropDownList ID="ddlSubmittalType" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Checkbox ID="chkWiringDiagram" runat="server" Text="Include Wiring Diagrams, if available"></asp:Checkbox>
        </td>
    </tr>

    <tr>
        <td colspan="2">
            <asp:UpdatePanel ID="upErrorMsg" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblErrorMsg" cssclass="errorlabel" runat="server"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnPreview" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
        <td colspan="2">
            <asp:UpdateProgress ID="upPreview" runat="server" AssociatedUpdatePanelID="upRunSubmittal">
                <ProgressTemplate>
                    <img alt="progress" src="images/ajax-loader.gif" />Processing...
                </ProgressTemplate>
            </asp:UpdateProgress>
        </td>
    </tr>
    </table>

    <!-- end of table -->
    <!-- panel when editing -->
    <asp:UpdatePanel ID="upEditing" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlEditing" runat="server" Visible="false">
            <!-- Outside border. 1 column, solid. -->
                <table border="1" style="border-style: solid" width="100%"><tr><td>
                    <!-- Inside table. -->
                    <table>
                        <!-- 11 columns -->
                        <tr valign="top">
                            <td rowspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            <h2>Item <asp:Label ID="lblItemNo" runat="server" Text="1"></asp:Label></h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upbtnSubmit" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="chkSubmit" runat="server" Text=" <b>Submit</b>" 
                                                        oncheckedchanged="chkSubmit_CheckedChanged" AutoPostBack="true" />
                                                    <br />
                                                    <br />
                                                    <asp:CheckBox ID="chkSubmitAll" runat="server" Text=" <b>Submit&nbsp;ALL</b>"
                                                     AutoPostBack="true" oncheckedchanged="chkSubmitAll_CheckedChanged" />

                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtSubmittalNote" EventName="TextChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <!-- Columns #2,3,4 -->
                            <td colspan="3" rowspan="2">
                                <table>
                                    <tr>
                                        <td colspan="2"><b>Submittal Notes:</b></td>
                                    </tr>
                                    <tr>
                                        <td><b>Quantity:</b></td>
                                        <td><asp:Label ID="lblQuantity" runat="server" class="textlabelemphasis" Text="1"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Type:</td>
                                        <td><asp:Label ID="lblTransformerType" runat="server" class="textlabel" Text="Custom"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Unit Price:</td>
                                        <td><asp:Label ID="lblPriceUnit" runat="server" class="textlabel" Text="$1,246"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>Ext Price:</td>
                                        <td><asp:Label ID="lblPriceExt" runat="server" class="textlabel" Text="$1,246"></asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                            <td colspan="5" rowspan="2">
                                <asp:UpdatePanel ID="upSubmittalNote" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtSubmittalNote" runat="server" Columns="55" Rows="8" TextMode="multiline"
                                                    Style="font-family: Microsoft Sans Serif, Sans-Serif;" MaxLength="450"
                                                    onKeyUp="TextBoxCount(this,350)" onChange="TextBoxCount(this,350)" 
                                            Height="105px" ontextchanged="txtSubmittalNote_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <br />
                                        <asp:Label ID="lblTextBoxErrorMsg" runat="server" class="textlabelemphasis" Text=""></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="chkSubmit" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="2" rowspan="2">
                                <asp:Button ID="btnSave" runat="server" Text="SAVE" Height="60px" Width="100px" 
                                    OnClientClick="this.disabled = true;" UseSubmitBehavior="false"
                                    onclick="btnSave_Click" />
                                <br />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Height="30px" 
                                    Width="100px" onclick="btnCancel_Click" />
                                <br />
                                <br />
                                <asp:Label ID="lblPlusMinus" runat="server">&nbsp; (sp. ch's: ± ° )</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11">
                                <asp:UpdateProgress ID="upProgSave" runat="server" AssociatedUpdatePanelID="upEditing">
                                    <ProgressTemplate>
                                        <img alt="progress" src="images/ajax-loader.gif" />Processing...
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                            <!-- Back to full 11 columns -->
                        <tr>
                            <td>Windings:</td>
                            <td><asp:Label ID="lblWindings" runat="server" class="textlabelemphasis" Text="Aluminum"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>Temp Rise:</td>
                            <td><asp:Label ID="lblTemperature" runat="server" class="textlabelemphasis" Text="150"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>Enclosure:</td>
                            <td><asp:Label ID="lblEnclosure" runat="server" class="textlabel" Text="Outdoor"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:Label ID="lblSpecialTypeCodeTitle" runat="server" class="statuslabel" Text="Special Type:"></asp:Label>
                                <asp:Label ID="lblSpecialTypeCodeNone" runat="server" Text="Special Type:"></asp:Label>
                            </td>
                            <td><asp:Label ID="lblSpecialTypeCode" runat="server" class="textlabelemphasis" Text="Auto Transformer"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Phase:</td>
                            <td><asp:Label ID="lblPhase" runat="server" class="textlabelemphasis" Text="Three"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>Elect. Shield:</td>
                            <td><asp:Label ID="lblElectrostaticShield" runat="server" class="textlabel" Text="One"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>Enclosure&nbsp;Mtl:</td>
                            <td><asp:Label ID="lblEnclosureMtl" runat="server" class="textlabel" Text="HRPO (STD)"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:Label ID="lblSpecialFeatureCodesTitle" runat="server" class="statuslabel" Text="Special Features:"></asp:Label>
                                <asp:Label ID="lblSpecialFeatureCodesNone" runat="server" Text="Special Features:"></asp:Label>
                            </td>
                            <td><asp:Label ID="lblSpecialFeatureCodes" runat="server" class="textlabel" Text="Features Selected"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>KVA:</td>
                            <td><asp:Label ID="lblKVA" runat="server" class="textlabelemphasis" Text="75"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>Efficiency:</td>
                            <td><asp:Label ID="lblEfficiency" runat="server" class="textlabelemphasis" Text="EXEMPT"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>Case Size:</td>
                            <td><asp:Label ID="lblCaseSize" runat="server" class="textlabelemphasis" Text="GPF"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:Label ID="lblSpecialFeatureNotesTitle" runat="server" class="statuslabel" Text="Feature Notes:"></asp:Label>
                                <asp:Label ID="lblSpecialFeatureNotesNone" runat="server" Text="Feature Notes:"></asp:Label>
                            </td>
                            <td><asp:Label ID="lblSpecialFeatureNotes" runat="server" class="textlabel" Text="Notes go here"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Primary Voltage:</td>
                            <td><asp:Label ID="lblPrimaryVoltage" runat="server" class="textlabelemphasis" Text="480Y/122"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:Label ID="lblEfficiencyExemptReasonTitle" runat="server" class="statuslabel" Text="Exempt&nbsp;Reason:"></asp:Label>
                                <asp:Label ID="lblEfficiencyExemptReasonNone" runat="server" Text="Exempt&nbsp;Reason:"></asp:Label>
                            </td>
                            <td><asp:Label ID="lblEfficiencyExemptReason" runat="server" class="textlabel" Text="For Export"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>Case Color:</td>
                            <td><asp:Label ID="lblCaseColorCode" runat="server" class="textlabel" Text="ANSI Gray"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:Label ID="lblTapsNotesTitle" runat="server" class="statuslabel" Text="Taps Notes:"></asp:Label>
                                <asp:Label ID="lblTapsNotesNone" runat="server" Text="Taps Notes:"></asp:Label>
                            </td>
                            <td><asp:Label ID="lblTapsNotes" runat="server" class="textlabel" Text="Tap Notes"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Secondary&nbsp;Volts:</td>
                            <td><asp:Label ID="lblSecondaryVoltage" runat="server" class="textlabelemphasis" Text="208 D"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>Frequency:</td>
                            <td><asp:Label ID="lblFrequency" runat="server" class="textlabel" Text="60 Hz"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:Label ID="lblMarineDutyTitle" runat="server" class="statuslabel" Text="Marine Duty:"></asp:Label>
                                <asp:Label ID="lblMarineDutyNone" runat="server" Text="Marine Duty:"></asp:Label>
                            </td>
                            <td><asp:Label ID="lblMarineDuty" runat="server" class="textlabel" Text="Marine Duty"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>
                                <asp:Label ID="lblImpedanceNotesTitle" runat="server" class="statuslabel" Text="Impedance Notes:"></asp:Label>
                                <asp:Label ID="lblImpedanceNotesNone" runat="server" Text="Impedance Notes:"></asp:Label>
                            </td>
                            <td><asp:Label ID="lblImpedanceNotes" runat="server" class="textlabel" Text="Impedance Notes"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>K-Factor:</td>
                            <td><asp:Label ID="lblKFactor" runat="server" class="textlabelemphasis" Text="K-1"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>Sound&nbsp;Level:</td>
                            <td><asp:Label ID="lblSoundReductCode" runat="server" class="textlabel" Text="45 dB"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td><asp:Label ID="lblMadeInUSACodesTitle" runat="server" class="statuslabel" Text="Made in USA:"></asp:Label>
                                <asp:Label ID="lblMadeInUSACodesNone" runat="server" Text="Made in USA:"></asp:Label></td>
                            <td><asp:Label ID="lblMadeInUSACodes" runat="server" class="textlabel" Text="Made in USA"></asp:Label></td>
                            <td>&nbsp;&nbsp;&nbsp;</td>
                            <td>Catalog Number:</td>
                            <td><asp:Label ID="lblCatalogNumber" runat="server" class="textlabel" Text="HT112A3B2SH-HK0160LN03R"></asp:Label></td>
                        </tr>
                    </table>
                 </td></tr></table>  <!-- Outside table -->
            </asp:Panel>    <!-- pnlEditing -->
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvQuoteItems" EventName="RowCommand" />
        </Triggers>
    </asp:UpdatePanel>
    <h3><asp:Label ID="lblItems" runat="server" Text="Items"></asp:Label></h3>
    Click 
        Select on each row in the list to create Submittal Notes.&nbsp; Click the Submit 
        checkbox.&nbsp; These are then marked by an "X" in the "Notes" column.
    <br />&nbsp;<br />
        <asp:UpdatePanel ID="upgvQuoteItems" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvQuoteItems" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                    CellPadding="4" AllowPaging="True" PageSize="50" ForeColor="#333333" GridLines="Vertical"
                    DataKeyNames="QuoteDetailsID,QuoteID,DetailID,EditDisplay" OnRowCommand="gvQuoteItems_RowCommand"
                    DataSourceID="dsQuote" Font-Size="X-Small" >
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" Font-Size="X-Small" />
                    <Columns>
                        <asp:BoundField DataField="DetailIDDisplay" ReadOnly="True" SortExpression="DetailIDDisplay"
                            HeaderText="#">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IsSubmittal" ReadOnly="True" SortExpression="IsSubmittal" HeaderText="Submit">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField ShowHeader="false" ConvertEmptyStringToNull="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="Note" runat="server" CausesValidation="false" CommandName="Edit"
                                    CommandArgument="<%# gvQuoteItems.DataKeys[((GridViewRow)Container).RowIndex].Values[0] %>"
                                    Text="Select"></asp:LinkButton>
                            </ItemTemplate>
                            <ControlStyle Font-Underline="true" />
                            <ItemStyle ForeColor="Blue" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="DetailID" ReadOnly="True" SortExpression="DetailID" Visible="false" />
                        <asp:BoundField DataField="HasSubmittalNote" ReadOnly="True" SortExpression="HasSubmittalNote" HeaderText="Note">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StandardOrCustom" ReadOnly="True" SortExpression="StandardOrCustom"
                            HeaderText="Type">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Windings" HeaderText="Windings" ReadOnly="True" SortExpression="Windings">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Phase" HeaderText="Phase" ReadOnly="True" SortExpression="Phase">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="KVA" HeaderText="KVA" ReadOnly="True" SortExpression="KVA">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PrimaryVoltage" HeaderText="Prim Voltage" ReadOnly="True"
                            SortExpression="PrimaryVoltage">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SecondaryVoltage" HeaderText="Sec Voltage" ReadOnly="True"
                            SortExpression="SecondaryVoltage">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="KFactor" HeaderText="KFactor" ReadOnly="True" SortExpression="KFactor">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Temperature" HeaderText="Temp" ReadOnly="True" SortExpression="Temperature">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Hz" HeaderText="Hz" ReadOnly="True" SortExpression="Hz">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ElectrostaticShield" HeaderText="ES Shield" ReadOnly="True"
                            SortExpression="ElectrostaticShield">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CatalogNumberKitNo" HeaderText="Catalog/Kit No" ReadOnly="True"
                            SortExpression="CatalogNumberKitNo">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Quantity" HeaderText="Qty" SortExpression="Quantity" ReadOnly="True">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Price" HeaderText="Unit Price" SortExpression="Price" ReadOnly="True"
                            DataFormatString="{0:C2}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ExtendedPrice" HeaderText="Ext Price" ReadOnly="True"
                            SortExpression="ExtendedPrice" DataFormatString="{0:C2}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" Font-Size="X-Small" />
                    <EmptyDataRowStyle Font-Size="X-Small" />
                    <FooterStyle BackColor="#336699" Font-Bold="True" ForeColor="White" Font-Size="X-Small" />
                    <HeaderStyle BackColor="#336699" Font-Bold="True" ForeColor="White" Font-Size="X-Small"
                        VerticalAlign="Bottom" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" Font-Size="X-Small" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="X-Small" />
                    <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Size="X-Small" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" Font-Size="X-Small" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" Font-Size="X-Small" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" Font-Size="X-Small" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" Font-Size="X-Small" />
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvQuoteItems" EventName="RowCommand" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:SqlDataSource ID="dsQuote" runat="server" ConnectionString="<%$ ConnectionStrings:mgmdb %>"
            SelectCommand="usp_QuoteItemList_Submittal" SelectCommandType="StoredProcedure">
            <DeleteParameters>
                <asp:SessionParameter Name="quote_id" Type="Int32" SessionField="QuoteID" />
                <asp:SessionParameter Name="detail_id" Type="Int32" SessionField="DetailID" />
            </DeleteParameters>
            <SelectParameters>
                <asp:SessionParameter Name="quote_id" SessionField="QuoteID" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
</td></tr></table>      <!-- Outer table -->

