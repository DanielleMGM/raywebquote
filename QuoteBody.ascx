<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuoteBody.ascx.cs" Inherits="MGM_Transformer.QuoteBody" %>
<!-- Outside border. 1 column, solid. -->
<table border="1" style="border-style: solid" width="100%">
    <tr>
        <td>
            <!-- Contact Section -->
            <table border="0" width="100%">
                <!-- Quote. 9 columns. -->
                <tr align="left" valign="top">
                    <td rowspan="2">
                        <asp:UpdatePanel ID="uplblQuoteID" runat="server">
                            <ContentTemplate>
                                <h2>
                                    <asp:Label ID="lblQuoteIDPrefix" runat="server" Text="Quote#"></asp:Label></h2>
                                <asp:Label ID="lblQuoteID" runat="server" Text="0" Visible="false"></asp:Label>
                                <asp:Label ID="lblQuoteNo" runat="server" Text="0" Visible="false"></asp:Label>
                                <asp:Label ID="lblQuoteNoVer" runat="server" Text="0" Visible="false"></asp:Label>
                                <asp:Label ID="lblQuoteOriginCode" runat="server" Text="Q" Visible="false"></asp:Label>
                                <h2>
                                    <asp:Label ID="lblQuoteNoAndVer" runat="server" Visible="true"></asp:Label></h2>
                                <asp:Label ID="lblQuoteCopy" runat="server" CssClass="textlabelhuge" Visible="true"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="left">
                        Company:
                    </td>
                    <td align="left">
                        <asp:UpdatePanel ID="uptxtCompany" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCompany" runat="server" Columns="60" MaxLength="100" AutoPostBack="true"
                                    Visible="false" OnTextChanged="txtCompany_TextChanged" TabIndex="1"></asp:TextBox>
                                <asp:TextBox ID="txtCustomerContactID" runat="server" Visible="false"></asp:TextBox>
                                <asp:TextBox ID="txtCustomerID" runat="server" Visible="false"></asp:TextBox>
                                <asp:Label ID="lblCompany" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="lblCompanyID" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="lblCompanyReqd" runat="server" CssClass="errorlabel" Text="&nbsp;Required."></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtCompany" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        Contact:
                    </td>
                    <td class="width-250">
                        <!-- Setting width to avoid Shipping taking up all the space -->
                        <asp:UpdatePanel ID="uptxtContactName" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtContactName" runat="server" Columns="30" MaxLength="100" TabIndex="5"></asp:TextBox>
                                <asp:Label ID="lblContactName" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="right" valign="top">
                        <asp:UpdatePanel ID="uptblPrice" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="pnlTotalPrice" runat="server" Visible="false">
                                    <table cellpadding="1" width="120" style="background-color: Blue">
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: White">
                                                    <tr align="center">
                                                        <td>
                                                            <asp:Label ID="lblTotalPriceHdr" runat="server" CssClass="textunderline">Total Price</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr align="center">
                                                        <td>
                                                            <asp:Label ID="lblTotalPrice" CssClass="textlabelemphasis" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="txtCompany" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr align="left" valign="top">
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:UpdatePanel ID="uplblOrSelect" runat="server">
                            <ContentTemplate>
                                <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtCompany" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upddlCustomer" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCustomer" runat="server" Visible="false" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtCompany" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnFilter" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        Email:
                    </td>
                    <td>
                        <asp:UpdatePanel ID="uptxtEmail" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtEmail" runat="server" Columns="30" MaxLength="100" AutoPostBack="true"
                                    OnTextChanged="txtEmail_TextChanged" TabIndex="6"></asp:TextBox>
                                <asp:Label ID="lblEmail" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="lblEmailInvalid" runat="server" Text="* Invalid." CssClass="errorlabel"
                                    Visible="false"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtEmail" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td align="right" rowspan="5">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="upbtnShowQuote" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnPDF" runat="server" Text="Preview Quote" Width="120" Height="30"
                                                OnClick="btnPDF_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnPDF" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="upbtnFinalize" runat="server">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSubmittal" runat="server" Text="Submittal" Width="120" Height="30"
                                                            Visible="true" OnClick="btnSubmittal_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnRequestApproval" runat="server" Text="Request Approval" Width="120"
                                                            Height="30" Visible="false" OnClick="btnRequestApproval_Click" />
                                                        <asp:Label ID="lblbtnRequestApprovalClicked" runat="server" Visible="false" Text="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnFinalize" runat="server" CssClass="btn-bold" Text="Finalize" Width="120"
                                                            Height="30" Visible="false" OnClick="btnFinalize_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnEdit" runat="server" Text="Re-Open" Width="120" Height="30" Visible="false"
                                                            OnClick="btnEdit_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit for Approval" Width="120"
                                                            Height="60" Visible="false" CssClass="btn-wrap-80" OnClick="btnSubmit_Click" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFinalizeErrors" CssClass="errorlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <br />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCompany" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnRequestApproval" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="chkSameQuoteNo" EventName="CheckedChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:UpdatePanel ID="uplblApprovals" runat="server">
                                        <ContentTemplate>
                                            <b>
                                                <asp:Label ID="lblApprovalReqd" runat="server" Text="" Visible="false" CssClass="errorlabel"></asp:Label>
                                                <asp:Label ID="lblApprovalReqdExplanation" runat="server" Text="" Visible="false"
                                                    CssClass="textlabel">Enter Approval Request (below, optional) and click Submit.</asp:Label>
                                            </b>
                                            <br />
                                            <asp:Label ID="lblApprovalRequested" runat="server" Text="Approval Requested." CssClass="statuslabel"></asp:Label>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:UpdatePanel ID="upbtnApprove" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnApprove" runat="server" Text="Approve" Width="120" Height="30"
                                                OnClick="btnApprove_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:Label ID="lblApprovedBy" runat="server" CssClass="textlabel"></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:Label ID="lblApprovedDate" runat="server" CssClass="textlabel"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="upbtnQuoteDetailsRpt" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnQuoteDetailsRpt" runat="server" Text="Quote Det Rpt" Width="120"
                                                Height="30" Visible="false" OnClick="btnQuoteDetailsRpt_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="upbtnEmail" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnEmail" runat="server" Text="Email" Width="120" Height="30" OnClick="btnEmail_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="upbtnCopyQuote" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnCopyQuote" runat="server" Text="Copy Quote-New#" Width="120" Height="30"
                                                OnClick="btnCopyQuote_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="chkSameQuoteNo" EventName="CheckedChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="upchkSameQuoteNo" runat="server">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkSameQuoteNo" runat="server" Text="Same quote #." Checked="false"
                                                OnCheckedChanged="chkSameQuoteNo_CheckedChanged" AutoPostBack="true" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:CheckBox ID="chkNoDrawingsAttached" runat="server" Text="No drawings." Checked="false"
                                        OnCheckedChanged="chkNoDrawingsAttached_CheckedChanged" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="upchkOEM" runat="server">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkOEM" runat="server" Text="OEM - Word." Checked="false" OnCheckedChanged="chkOEM_CheckedChanged"
                                                AutoPostBack="true" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="chkNoDrawingsAttached" EventName="CheckedChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr align="left" valign="top">
                    <td rowspan="2">
                        <asp:UpdatePanel ID="uplblRepDistributor" runat="server">
                            <ContentTemplate>
                                <table border="0" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            Rep:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRep" runat="server" CssClass="textlabel" Text=""></asp:Label>
                                            <asp:Label ID="lblRepID" runat="server" CssClass="textlabel" Text="" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPricing" Visible="false" runat="server" Text="Pricing:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblRepDistributor" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblRepDistributorID" Visible="false" runat="server" Text=""></asp:Label>
                                            <asp:TextBox ID="txtDetailID" runat="server" Visible="false"></asp:TextBox>
                                            <asp:TextBox ID="txtQuoteDetailsID" runat="server" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblLatest" runat="server" Visible="true">Latest:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFullNameLatest" runat="server" Visible="true"></asp:Label>
                                            <asp:HiddenField ID="hidUserNameLatest" runat="server" />
                                            <asp:Label ID="lblQuotedByReqd" runat="server" CssClass="errorlabel" Visible="false">Name required.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Created:
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upddlRep" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlRep" runat="server" Visible="true" OnSelectedIndexChanged="ddlRep_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hidUserNameCreated" runat="server" />
                                                    <asp:Label ID="lblFullNameCreated" runat="server" Visible="false"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlRep" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblCreatedPhoneTitle" runat="server" Visible="false">Phone(s):</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblCreatedPhone" runat="server" Visible="false" CssClass="textlabel"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                    </td>
                    <td>
                        Project:
                    </td>
                    <td align="left">
                        <asp:UpdatePanel ID="upProject" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtProject" runat="server" Columns="60" MaxLength="100" TabIndex="3"></asp:TextBox>
                                <asp:Label ID="lblProject" runat="server" CssClass="textlabel" Text="" Visible="false"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upCreateFinalizeLbl" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblCreatedLabel" runat="server" Text="Created:"></asp:Label>
                                <asp:Label ID="lblFinalizedLabel" runat="server" Text="Finalized:"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upCreatFinalizeDate" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblCreatedOn" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lblFinalizedOn" CssClass="textlabel" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr align="left" valign="top">
                    <td>
                    </td>
                    <td>
                        City:
                    </td>
                    <td>
                        <asp:UpdatePanel ID="uptxtCity" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtCity" runat="server" Columns="30" MaxLength="100" TabIndex="4"></asp:TextBox>
                                <asp:Label ID="lblCity" runat="server" CssClass="textlabel" Text="" Visible="false"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                    </td>
                    <td colspan="2">
                        <asp:UpdatePanel ID="uptxtPDFURL" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblStatus" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                <!-- Show LostTo and LostReason on separate lines, if completed. -->
                                <table>
                                    <tr>
                                        <td>
                                            Status:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblProgress" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPDFURL" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnPDF" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr align="left" valign="top">
                    <td>
                        <asp:HyperLink ID="hlnkCalculator" runat="server" NavigateUrl="http://www.mgmtransformer.com/calculator/"
                            Target="_blank">
                            <table cellspacing="0" cellpadding="2" border="0">
                                <tr>
                                    <td rowspan="2">
                                        <asp:Image ID="imgCalculator" runat="server" ImageUrl="~/images/Calculator.png" AlternateText="Volts / Amps / kVA Calculator" />
                                    </td>
                                    <td>
                                        <a href="http://www.mgmtransformer.com/calculator/" target="_blank">Amps/Volts/KVA</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <a href="http://www.mgmtransformer.com/calculator/" target="_blank">Calculator</a>
                                    </td>
                                </tr>
                            </table>
                        </asp:HyperLink>
                        &nbsp;
                        <asp:UpdatePanel ID="uplblRepDistributorAlt" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblRepDistributorAlt" runat="server" CssClass="statuslabel" Text="Special Pricing"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td colspan="2" align="center">
                        <asp:UpdatePanel ID="updShowNotes" runat="server">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            <h3>
                                                <asp:LinkButton ID="lnkbShowNotes" runat="server" OnClick="lnkbShowNotes_Click">Show Notes</asp:LinkButton>
                                                <asp:LinkButton ID="lnkbHideNotes" runat="server" OnClick="lnkbHideNotes_Click" Visible="false">Hide Notes</asp:LinkButton>
                                                <asp:Label ID="lblShowNotes" runat="server" Visible="false"></asp:Label>
                                            </h3>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSaveNotes" runat="server" Text="Save Notes" OnClick="btnSaveNotes_Click"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMsgNotesSaved" runat="server" Text="Notes saved." CssClass="textlabel"
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lnkbShowNotes" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkbHideNotes" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:UpdatePanel ID="uplblShippingTitle" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblShippingTitle" runat="server" Text="Shipping:" Visible="false"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="uplblShipping" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblShipping" CssClass="textlabel" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="8" align="center">
                        <asp:UpdatePanel ID="uplblFinalized" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblApprovalReasonCalc" runat="server" CssClass="textlabel" Visible="false"></asp:Label>
                                <asp:Label ID="lblItemApprovalReason" runat="server" CssClass="textlabel" Visible="false"></asp:Label>
                                <asp:Label ID="lblApprovalReasonQuote" runat="server" CssClass="textlabel" Visible="false"></asp:Label>
                                <asp:Label ID="lblFinalized" CssClass="statuslabel" runat="server" Text="Finalized."
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblNotesSaved" CssClass="statuslabel" runat="server" Text="Saved."
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblEmailSent" CssClass="statuslabel" runat="server" Text="Email Sent."
                                    Visible="false"></asp:Label>
                                <asp:Label ID="lblQuoteCopied" CssClass="textlabel" runat="server" Text="" Visible="true"></asp:Label>
                                <asp:UpdateProgress ID="upProgFinalize" runat="server" AssociatedUpdatePanelID="upbtnFinalize">
                                    <ProgressTemplate>
                                        <img alt="progress" src="images/ajax-loader.gif" />Processing...
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <asp:UpdateProgress ID="upProgApprove" runat="server" AssociatedUpdatePanelID="upbtnApprove">
                                    <ProgressTemplate>
                                        <img alt="progress" src="images/ajax-loader.gif" />Processing...
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <asp:UpdateProgress ID="upProgShowQuote" runat="server" AssociatedUpdatePanelID="upbtnShowQuote">
                                    <ProgressTemplate>
                                        <img alt="progress" src="images/ajax-loader.gif" />Processing...
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="9" align="center">
                        <div id="divOrders" runat="server">
                            <table>
                                <tr>
                                    <td align="center">
                                        <h2>
                                            Orders</h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                            CellPadding="4" AllowPaging="True" PageSize="10" ForeColor="#333333" GridLines="Vertical"
                                            DataSourceID="dsOrders" Font-Size="X-Small">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" Font-Size="X-Small" />
                                            <Columns>
                                                <asp:BoundField DataField="QuoteID" ReadOnly="True" Visible="false" />
                                                <asp:BoundField DataField="SalesOrderNo" ReadOnly="True" SortExpression="SalesOrderNo"
                                                    HeaderText="SO #">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PurchaseOrderNo" ReadOnly="True" SortExpression="PurchaseOrderNo"
                                                    HeaderText="PO #">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="OrderDate" DataFormatString="{0:MM/dd/yy}" HeaderText="Order Date"
                                                    ReadOnly="True" SortExpression="OrderDate">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ShipDate" DataFormatString="{0:MM/dd/yy}" HeaderText="Ship Date"
                                                    ReadOnly="True" SortExpression="ShipDate">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ShippingInstructions" HeaderText="Pro # - Shipper" ReadOnly="True"
                                                    SortExpression="ShippingInstructions">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="True" SortExpression="Total"
                                                    DataFormatString="{0:C2}">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="StatusCode" ReadOnly="True" SortExpression="StatusCode"
                                                    HeaderText="Status">
                                                    <ItemStyle HorizontalAlign="Center" />
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
                                        <asp:SqlDataSource ID="dsOrders" runat="server" ConnectionString="<%$ ConnectionStrings:mgmdb %>"
                                            SelectCommand="usp_Orders_List" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="quote_id" SessionField="QuoteID" Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <!-- Orders Div -->
                        <asp:UpdatePanel ID="uplblStockQuote" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblStockQuote" CssClass="textlabel" runat="server" Text=""></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnFinalize" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <!-- Notes Section -->
            <asp:UpdatePanel ID="uppnlNotes" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlNotes" Visible="false" runat="server">
                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                            <tr align="center">
                                <td>
                                    <b>Notes</b>
                                </td>
                                <td>
                                    <asp:Label ID="lblMGMNotes" runat="server"><b>MGM Notes</b></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblInternalPrivate" runat="server"><b>Internal: PRIVATE</b></asp:Label>
                                    <asp:Label ID="lblPlusMinus" runat="server"> (sp. ch's: ± ° )</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblApprovalRequest" runat="server"><b>Approval Request</b></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    (On Home page. Not on Quote.)
                                </td>
                                <td>
                                    <asp:Label ID="lblOnQuote" runat="server" Text="Label">(On Quote.)</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblNotSeenOnPDF" runat="server" Text="Label">(Not on PDF or quote.)</asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblEmailedtoMGM" runat="server" Text="Label">(Emailed to MGM.)</asp:Label>
                                </td>
                            </tr>
                            <tr align="center" valign="top">
                                <td>
                                    <asp:TextBox ID="txtNotes" runat="server" Columns="50" Rows="6" TextMode="multiline"
                                        Style="font-family: Microsoft Sans Serif, Sans-Serif;" MaxLength="1000"></asp:TextBox>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNotesPDF" runat="server" Columns="70" Rows="6" TextMode="multiline"
                                                    Style="font-family: Microsoft Sans Serif, Sans-Serif;" MaxLength="1000"></asp:TextBox>
                                                <asp:Label ID="lblNotesPDF" runat="server" Visible="false" CssClass="textlabel"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtNotesInternal" runat="server" Columns="40" Rows="6" TextMode="multiline"
                                        Style="font-family: Microsoft Sans Serif, Sans-Serif;" MaxLength="1000" OnTextChanged="txtNotesInternal_TextChanged"
                                        AutoPostBack="true"></asp:TextBox>
                                </td>
                                <td valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNotesRequest" runat="server" Columns="40" Rows="6" TextMode="multiline"
                                                    Style="font-family: Microsoft Sans Serif, Sans-Serif;" MaxLength="1000" Visible="true"
                                                    OnTextChanged="txtNotesRequest_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblNotesRequestRequired" runat="server" Visible="false" CssClass="errorlabel">Please enter request.</asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <!-- pnlNotes -->
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSaveNotes" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lnkbShowNotes" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="lnkbHideNotes" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="txtNotes" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="txtNotesInternal" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="txtNotesPDF" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="txtNotesRequest" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chkHideKVA" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chkHideVoltPrimary" EventName="CheckedChanged" />
                    <asp:AsyncPostBackTrigger ControlID="chkHideVoltSecondary" EventName="CheckedChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
            <!-- Add item border. 1 column. 1 row.  -->
            <asp:UpdatePanel ID="uppnlAddItem" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlAddItem" Visible="false" runat="server">
                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                            <!-- Add item. 10 columns. -->
                            <tr valign="top">
                                <td colspan="10" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="2" width="100%">
                                        <!-- 4 columns. -->
                                        <tr align="left">
                                            <td valign="top" class="width-200" colspan="1">
                                                <h2>
                                                    <asp:Label ID="lblAddEdit" runat="server" Text="Add Item to Quote"></asp:Label></h2>
                                            </td>
                                            <%-- Rays Change--%>
                                            <td>
                                                <asp:UpdatePanel ID="upCatalogNo" runat="server">
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="lnkbShowCatalogNo" runat="server" OnClick="lnkbShowCatalogNo_Click"
                                                            Visible="true">Fill by Catalog Number</asp:LinkButton>
                                                        <asp:LinkButton ID="lnkbHideCatalogNo" runat="server" OnClick="lnkbHideCatalogNo_Click"
                                                            Visible="false">Hide Fill by Catalog Number</asp:LinkButton>
                                                        <asp:Panel ID="pnlCatalogNumber" runat="server" Visible="false">
                                                            <table style="margin-left: 220px;">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <b>Catalog Number</b>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="tbCatalogNo" runat="server" Width="200px"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnApply" runat="server" Text="Apply" OnClick="btnApply_Click" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnClearCatalogNo" runat="server" Text="Clear" OnClick="btnClearCatalogNo_Click" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnClearCatalogNo" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                            <%--End Rays Change --%>
                                            <td align="right" rowspan="2">
                                                &nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="btnClear" runat="server" Text="Clear" Width="120" Height="30" OnClick="btnClear_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle">
                                                <table border="0" cellpadding="2" cellspacing="2" width="250">
                                                    <tr>
                                                        <td>
                                                            <b>Item&nbsp;Type:</b>
                                                        </td>
                                                        <td>
                                                            <asp:UpdatePanel ID="uprblStandardOrCustom" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:RadioButtonList ID="rblStandardOrCustom" runat="server" AutoPostBack="true"
                                                                        RepeatDirection="Horizontal" OnSelectedIndexChanged="rblStandardOrCustom_SelectedIndexChanged"
                                                                        TabIndex="7">
                                                                        <asp:ListItem Value="Custom">Custom</asp:ListItem>
                                                                        <asp:ListItem Value="Standard">Stock</asp:ListItem>
                                                                    </asp:RadioButtonList>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:UpdatePanel ID="uplblGeneral" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:Label ID="lblGeneral" CssClass="errorlabel" runat="server" Text=""></asp:Label>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="rblWindings" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="rblPhase" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="rblSpecialTypes" EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="uplblIsMatch" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblMatch" runat="server" CssClass="errorlabel" Text="This configuration matches a current stock offering.  For possibly a lower price and immediate delivery, try re-entering this as stock."
                                                            Visible="true"></asp:Label>
                                                        <asp:HiddenField ID="hidIsMatch" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hidDetailNotesInternal" runat="server" Value="" />
                                                        <asp:Label ID="lblExcessAccessories" runat="server" CssClass="statuslabel" Text="NOTE: This order has more accessories than items."
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblItemCopied" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="rblWindings" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="rblPhase" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlConfiguration" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPrimaryVoltage" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="rblSecondaryDW" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSecondaryVoltage" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlKFactor" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlTempRise" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="rblElectrostaticShield" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <!-- main entry -->
                        <asp:UpdatePanel ID="uptblMainEntry" runat="server">
                            <ContentTemplate>
                                <table id="tblMainEntry" border="0" cellpadding="0" cellspacing="2" width="100%"
                                    visible="false">
                                    <!-- 4 columns. -->
                                    <tr>
                                        <td colspan="10">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr valign="bottom" align="left">
                                        <td>
                                            <b>&nbsp;&nbsp;Windings</b>
                                        </td>
                                        <td>
                                            <b>&nbsp;Phase</b>
                                        </td>
                                        <td>
                                            <b>&nbsp;&nbsp;&nbsp;kVA</b>
                                        </td>
                                        <td colspan="2" align="left">
                                            <asp:UpdatePanel ID="uplblPrimaryVoltage" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblPrimaryVoltageCentering" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Label>
                                                    <b>
                                                        <asp:Label ID="lblPrimaryVoltage" runat="server" Text="&nbsp;&nbsp;Primary Voltage"></asp:Label>
                                                        <asp:Label ID="lblConfiguration" runat="server" Text="Configuration" Visible="false"></asp:Label>
                                                    </b>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td colspan="2" align="center">
                                            <asp:UpdatePanel ID="uplblSecondaryVoltage" runat="server">
                                                <ContentTemplate>
                                                    <b>
                                                        <asp:Label ID="lblSecondaryVoltage" runat="server" Text="&nbsp;&nbsp;Secondary Voltage"></asp:Label></b>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="uplblKFactor" runat="server">
                                                <ContentTemplate>
                                                    <b>
                                                        <asp:Label ID="lblKFactor" runat="server">&nbsp;K-Factor</asp:Label></b>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="uplblTempRise" runat="server">
                                                <ContentTemplate>
                                                    <b>
                                                        <asp:Label ID="lblTempRise" runat="server">&nbsp;Temp Rise</asp:Label></b>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td rowspan="2" valign="top">
                                            <asp:UpdatePanel ID="uprblElectrostaticShield" runat="server">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <b>
                                                                    <asp:Label ID="lblElectrostaticShield" runat="server" Text="&nbsp;&nbsp;Elect.&nbsp;Shield"></asp:Label></b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RadioButtonList ID="rblElectrostaticShield" runat="server" AutoPostBack="true"
                                                                    RepeatDirection="Horizontal" OnSelectedIndexChanged="rblElectrostaticShield_SelectedIndexChanged"
                                                                    TabIndex="18">
                                                                    <asp:ListItem Value="None" Selected="True">0</asp:ListItem>
                                                                    <asp:ListItem Value="Shielded">1</asp:ListItem>
                                                                    <asp:ListItem Value="Two">2</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <b>Customer Tag</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtCustomerTagNo" runat="server" MaxLength="13" Width="90"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlKFactor" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblElectrostaticShield" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr valign="top" align="left">
                                        <td class="width-90">
                                            <asp:UpdatePanel ID="uprblWindings" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <asp:RadioButtonList ID="rblWindings" runat="server" AutoPostBack="true" RepeatDirection="Vertical"
                                                        OnSelectedIndexChanged="rblWindings_SelectedIndexChanged" TabIndex="8">
                                                        <asp:ListItem Value="Aluminum" Selected="True">Aluminum</asp:ListItem>
                                                        <asp:ListItem Value="Copper">Copper</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rblWindings" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblPhase" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="width-80">
                                            <asp:UpdatePanel ID="uprblPhase" runat="server">
                                                <ContentTemplate>
                                                    <asp:RadioButtonList ID="rblPhase" runat="server" AutoPostBack="true" RepeatDirection="Vertical"
                                                        OnSelectedIndexChanged="rblPhase_SelectedIndexChanged" TabIndex="9">
                                                        <asp:ListItem Value="Three" Selected="True">Three</asp:ListItem>
                                                        <asp:ListItem Value="Single">Single</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rblPhase" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblWindings" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td class="width-80">
                                            <asp:UpdatePanel ID="upddlKVA" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlKVA" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlKVA_SelectedIndexChanged"
                                                        TabIndex="10" AppendDataBoundItems="false">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblKVAOr" runat="server"><br />or enter:<br /></asp:Label>
                                                    <asp:TextBox ID="txtKVA" runat="server" AutoPostBack="true" MaxLength="5" Width="50"
                                                        OnTextChanged="txtKVA_TextChanged"></asp:TextBox>
                                                    <asp:Label ID="lblKVAInvalid" CssClass="errorlabel" runat="server"></asp:Label>
                                                    <asp:Label ID="lblKVAUsed" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="lblHideKVANewLine" runat="server"><br /></asp:Label>
                                                    <asp:CheckBox ID="chkHideKVA" runat="server" OnCheckedChanged="chkHideKVA_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:Label ID="lblHideKVA" runat="server" Text="Hide KVA"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="txtKVA" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblWindings" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblPhase" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlKFactor" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlTempRise" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="chkHideKVA" EventName="CheckedChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvQuoteItems" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="uprblPrimaryDW" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlConfiguration" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlConfiguration_SelectedIndexChanged"
                                                        TabIndex="11" Width="275">
                                                    </asp:DropDownList>
                                                    <asp:RadioButtonList ID="rblPrimaryDW" runat="server" AutoPostBack="true" RepeatDirection="Vertical"
                                                        OnSelectedIndexChanged="rblPrimaryDW_SelectedIndexChanged" TabIndex="12">
                                                        <asp:ListItem Value="D" Selected="True">Delta</asp:ListItem>
                                                        <asp:ListItem Value="W">Wye</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblPhase" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblPrimaryDW" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upddlPrimaryVoltage" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlPrimaryVoltage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPrimaryVoltage_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblPrimaryOr" runat="server" Visible="false"><br />or enter:<br /></asp:Label>
                                                    <asp:Label ID="lblPrimaryShow" runat="server" Visible="true" CssClass="textlabel"></asp:Label>
                                                    <asp:TextBox ID="txtPrimaryVoltage" runat="server" AutoPostBack="true" Visible="false"
                                                        OnTextChanged="txtPrimaryVoltage_TextChanged" Width="90"></asp:TextBox>
                                                    <asp:Label ID="lblPrimaryVoltageInvalid" CssClass="errorlabel" runat="server"></asp:Label>
                                                    <asp:Label ID="lblHideVoltPrimaryNewLine" runat="server"><br /></asp:Label>
                                                    <asp:CheckBox ID="chkHideVoltPrimary" runat="server" OnCheckedChanged="chkHideVoltPrimary_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:Label ID="lblHideVoltPrimary" runat="server" Text="Hide Primary"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rblPrimaryDW" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlPrimaryVoltage" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="txtPrimaryVoltage" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblSecondaryDW" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlSecondaryVoltage" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="txtSecondaryVoltage" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="chkHideVoltSecondary" EventName="CheckedChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblSpecialTypes" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="pnlInventory" runat="server" Visible="false">
                                                <ContentTemplate>
                                                    <div style="margin-top: -20px;">
                                                        <asp:Table ID="tblInventory" runat="server" Width="500px">
                                                            <asp:TableRow Height="20">
                                                                <asp:TableCell ColumnSpan="3" HorizontalAlign="Center" VerticalAlign="Top">
                                                                    <asp:Label ID="lblNearbyStockAvailability" runat="server" Text="Nearby Stock Availability"
                                                                        Font-Bold="True" Font-Size="Medium"></asp:Label>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell HorizontalAlign="Center" Width="140">
                                                                    <asp:Label ID="lblWarehouse1" runat="server" Text="" Font-Bold="True" Font-Underline="True"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell HorizontalAlign="Center" Width="140">
                                                                    <asp:Label ID="lblWarehouse2" runat="server" Text="" Font-Bold="True" Font-Underline="True"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell HorizontalAlign="Center" Width="140">
                                                                    <asp:Label ID="lblWarehouse3" runat="server" Text="" Font-Bold="True" Font-Underline="True"></asp:Label>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell HorizontalAlign="Center">
                                                                    <asp:Label ID="lblWarehouse1Qty" runat="server" Text=""></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell HorizontalAlign="Center">
                                                                    <asp:Label ID="lblWarehouse2Qty" runat="server" Text=""></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell HorizontalAlign="Center">
                                                                    <asp:Label ID="lblWarehouse3Qty" runat="server" Text=""></asp:Label>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell ColumnSpan="3">
                                                                    <asp:Label ID="lblContactFactory" runat="server" Text="*For additional inventory, please refer to our "></asp:Label>
                                                                    <asp:HyperLink ID="hlFullInventory" runat="server" NavigateUrl="~/InventoryPDF.aspx?Agent=-1&Name=''&All=true&KVA=0&VoltageCat=ALL&VoltageDisp=&Windings=&Searching=false"
                                                                        Target="_blank">Full Inventory Report</asp:HyperLink>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="uprblSecondaryDW" runat="server">
                                                <ContentTemplate>
                                                    <asp:RadioButtonList ID="rblSecondaryDW" runat="server" AutoPostBack="true" RepeatDirection="Vertical"
                                                        OnSelectedIndexChanged="rblSecondaryDW_SelectedIndexChanged" TabIndex="14">
                                                        <asp:ListItem Value="W" Selected="True">Wye</asp:ListItem>
                                                        <asp:ListItem Value="D">Delta</asp:ListItem>
                                                        <asp:ListItem Value="Z">Zig-zag</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rblSecondaryDW" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblSpecialTypes" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upddlSecondaryVoltage" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlSecondaryVoltage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSecondaryVoltage_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblSecondaryOr" runat="server" Visible="false"><br />or enter:<br /></asp:Label>
                                                    <asp:Label ID="lblSecondaryShow" runat="server" CssClass="textlabel" Visible="true"></asp:Label>
                                                    <asp:TextBox ID="txtSecondaryVoltage" runat="server" AutoPostBack="true" Visible="false"
                                                        OnTextChanged="txtSecondaryVoltage_TextChanged" Width="90"></asp:TextBox>
                                                    <asp:Label ID="lblSecondaryVoltageInvalid" CssClass="errorlabel" runat="server"></asp:Label>
                                                    <asp:Label ID="lblHideVoltSecondaryNewLine" runat="server"><br /></asp:Label>
                                                    <asp:CheckBox ID="chkHideVoltSecondary" runat="server" OnCheckedChanged="chkHideVoltSecondary_CheckedChanged"
                                                        AutoPostBack="true" />
                                                    <asp:Label ID="lblHideVoltSecondary" runat="server" Text="Hide Secondary"></asp:Label>
                                                    <asp:HiddenField ID="hidIsStepUp" runat="server" Value="0" />
                                                    <asp:Label ID="lblStepUp" runat="server" Visible="false" CssClass="textlabel">Step-Up</asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rblPrimaryDW" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlPrimaryVoltage" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="txtPrimaryVoltage" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblSecondaryDW" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlSecondaryVoltage" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="txtSecondaryVoltage" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="chkHideVoltSecondary" EventName="CheckedChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblSpecialTypes" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upddlKFactor" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlKFactor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlKFactor_SelectedIndexChanged"
                                                        TabIndex="16">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="rblPhase" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlKFactor" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="txtKVA" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="chkForExport" EventName="CheckedChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upddlTempRise" runat="server">
                                                <ContentTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlTempRise" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTempRise_SelectedIndexChanged"
                                                                    TabIndex="17">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblTempUsed" runat="server" Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlTempRise" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="txtKVA" EventName="TextChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <!-- Add item. 10 columns. -->
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <!-- Panel AddItem  Visible set True/False -->
                    <!-- More Options -->
                    <asp:Panel ID="pnlMoreOptions" Visible="true" runat="server">
                        <table border="0" cellpadding="2" cellspacing="8" width="100%">
                            <tr>
                                <td colspan="6">
                                    <hr />
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <table border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <b>Frequency</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upddlFrequency" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlFrequency" runat="server" OnSelectedIndexChanged="ddlFrequency_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlFrequency" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b>Sound Level</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upddlSoundReduct" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="ddlSoundReduct" runat="server" OnSelectedIndexChanged="ddlSoundReduct_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSoundReduct" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="uplblEffiency" runat="server" Visible="true">
                                                    <ContentTemplate>
                                                        <asp:Label ID="lblEfficiency" runat="server"><b>Efficiency</b></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlFrequency" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPrimaryVoltage" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSecondaryVoltage" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="rblSpecialTypes" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtPrimaryVoltage" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtSecondaryVoltage" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="uplblEfficiency" runat="server" Visible="true">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlEfficiency" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEfficiency_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblEfficiencyValue" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblEfficiencyExemptReason" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblExemptReason" runat="server" Visible="false">Exempt reason:</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtExemptReason" runat="server" Visible="false" Columns="30" OnTextChanged="txtExemptReason_TextChanged"
                                                                        AutoPostBack="true"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Label ID="lblEfficiencyIsSetByAdmin" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblEfficiencyCodeCalc" runat="server" Visible="false"></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkForExport" EventName="CheckedChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlEfficiency" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlFrequency" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlPrimaryVoltage" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlSecondaryVoltage" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="rblSpecialTypes" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtExemptReason" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtPrimaryVoltage" EventName="TextChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtSecondaryVoltage" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upEfficiencyExport" runat="server" Visible="true">
                                                    <ContentTemplate>
                                                        <asp:CheckBox ID="chkForExport" runat="server" Text="For export" AutoPostBack="true"
                                                            OnCheckedChanged="chkForExport_CheckedChanged" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upddlEnclosure" runat="server">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <b>Enclosure / NEMA Rating</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEnclosure" runat="server" OnSelectedIndexChanged="ddlEnclosure_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                        <asp:Label ID="lblRainHoodAvail" CssClass="textlabel" runat="server" Visible="false">Will add rain hood/louver if Outdoor selected.</asp:Label>
                                                        <asp:Label ID="lblRainHoodUsed" CssClass="textlabel" runat="server" Visible="false">Outdoor because rain hood/louver included.</asp:Label>
                                                        <asp:Label ID="lblDualRated" CssClass="textlabel" runat="server" Visible="false">Dual rated. Rain hood/louver not required or offered.</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblStainless" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
                                                            OnSelectedIndexChanged="rblStainless_SelectedIndexChanged" Visible="false">
                                                            <asp:ListItem Value="304 Stainless Steel" Selected="True">304 Stainless</asp:ListItem>
                                                            <asp:ListItem Value="316 Stainless Steel">316 Stainless</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>
                                                            <asp:Label ID="lblCaseColor" runat="server" Text="Case Color" Visible="false"></asp:Label></b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCaseColor" runat="server" OnSelectedIndexChanged="ddlCaseColor_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <img id="ANSI61" src="images/color_ansi_61_90x30.png" alt="Color ANSI 61" runat="server"
                                                            visible="true" />
                                                        <img id="ANSI49" src="images/color_ansi_49_90x30.png" alt="Color ANSI 49" runat="server"
                                                            visible="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <b>
                                                            <asp:Label ID="lblCaseColorOther" runat="server" Text="Custom Color" Visible="false"></asp:Label></b>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtCaseColorOther" runat="server" MaxLength="11" Width="100" Visible="false"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCaseColorOtherReqd" runat="server" CssClass="errorlabel" Text="Please enter custom color."
                                                                    Visible="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox ID="chkMarineDuty" runat="server" AutoPostBack="true" Text="Marine Duty"
                                                                    OnCheckedChanged="chkMarineDuty_CheckedChanged" Visible="true" />
                                                            </td>
                                                        </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="chkMarineDuty" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlCaseColor" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlEnclosure" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td valign="top">
                                    <asp:UpdatePanel ID="upchkLstMadeInUSA" runat="server" Visible="true">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <b>Made in U.S.A.</b><br />
                                                        <asp:Label ID="lblNAFTA" runat="server" CssClass="small" Text="(Includes NAFTA)"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblMadeInUSA" runat="server" OnSelectedIndexChanged="rblMadeInUSA_SelectedIndexChanged"
                                                            RepeatDirection="Vertical" AutoPostBack="true" CssClass="small">
                                                            <asp:ListItem Value="None" Selected="True">None</asp:ListItem>
                                                            <asp:ListItem Value="Made in USA">Made in USA</asp:ListItem>
                                                            <asp:ListItem Value="ARRA">ARRA (Am Recov&Reinv)</asp:ListItem>
                                                            <asp:ListItem Value="BAA">BAA (Buy America)</asp:ListItem>
                                                            <asp:ListItem Value="SPPA">SPPA (Steel Prods Proc)</asp:ListItem>
                                                            <asp:ListItem Value="TAA">TAA (Trade Agreement)</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <a href="http://www.mgmtransformer.com/industry-buzz/buy-u-s-statutes/" target="_blank">
                                                            Made in USA Information</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="rblMadeInUSA" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <b>Special Types</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="uprblSpecialTypes" runat="server">
                                                    <ContentTemplate>
                                                        <asp:RadioButtonList ID="rblSpecialTypes" runat="server" OnSelectedIndexChanged="rblSpecialTypes_SelectedIndexChanged"
                                                            RepeatDirection="Vertical" AutoPostBack="true">
                                                            <asp:ListItem Value="None">None</asp:ListItem>
                                                            <asp:ListItem Value="Auto Transformer">Auto Transformer</asp:ListItem>
                                                            <asp:ListItem Value="Drive Isolation">Drive Isolation</asp:ListItem>
                                                            <asp:ListItem Value="Harmonic Mitigating">Harm Mitig (2 wdgs)</asp:ListItem>
                                                            <asp:ListItem Value="Scott-T">Scott-T</asp:ListItem>
                                                            <asp:ListItem Value="Zig Zag">Zig Zag Grnd (1 wdg)</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="rblSecondaryDW" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="rblSpecialTypes" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr align="center">
                                            <td>
                                                <a href="http://www.mgmtransformer.com/glossary/" target="_blank">Find in our Glossary</a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <table>
                                        <tr>
                                            <td>
                                                <b>Special Features</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="upchkSpecialFeatures" runat="server">
                                                    <ContentTemplate>
                                                        <asp:CheckBoxList ID="chkLstSpecialFeatures" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkListSpecialFeatures_SelectedIndexChanged"
                                                            Visible="true">
                                                        </asp:CheckBoxList>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkLstSpecialFeatures" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="txtKVA" EventName="TextChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="center">
                                    <asp:UpdatePanel ID="updCustomerTagNo" runat="server">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr align="center">
                                                    <td>
                                                        <asp:Label ID="lblSpecialFeatureNotes" runat="server" Text="<b>Special Features Notes</b>"
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:TextBox ID="txtSpecialFeatureNotes" runat="server" MaxLength="1000" Width="150"
                                                            Height="50" TextMode="MultiLine" Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:Label ID="lblSpecialFeatureNotesReqd" runat="server" Visible="false" CssClass="errorlabel"
                                                            Text="Please explain."></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:Label ID="lblTapsOEM" runat="server" Visible="false"><b>Taps (OEM)</b></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:TextBox ID="txtTapsOEM" runat="server" MaxLength="50" Width="100" Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:Label ID="lblImpedanceOEM" runat="server" Visible="false"><b>Impedance (OEM)</b></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:TextBox ID="txtImpedanceOEM" runat="server" MaxLength="50" Width="100" Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtCustomerTagNo" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtSpecialFeatureNotes" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="chkLstSpecialFeatures" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="chkOEM" EventName="CheckedChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <!-- End More Options -->
                        </table>
                        <!-- Notes. 4 columns. -->
                    </asp:Panel>
                    <!-- Notes -->
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlConfiguration" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="txtCompany" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            <!-- Show/Hide detail_totals -->
            <hr />
            <asp:UpdatePanel ID="upPanelTotals" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlTotals" Visible="false" runat="server">
                        <!-- ===================================== -->
                        <!-- WAS ONE 9 COLUMN TABLE.  NOW 3 TABLES -->
                        <!-- ===================================== -->
                        <!-- TABLE 1: Enclosure, Catalog Number, wall bracket images -->
                        <!-- TABLE 2: Item, Quantity, Unit Price, Extended Price, Total -->
                        <!-- TABLE 3: Buttons, Messages -->
                        <!-- ===================================== -->
                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                            <tr>
                                <td>
                                    <!-- TABLE 1: Enclosure, Catalog Number, wall bracket images (3 columns, including spacer) -->
                                    <asp:UpdatePanel ID="uplblEnclosureData" runat="server">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                                <!-- Titles -->
                                                <tr>
                                                    <th align="left">
                                                        <asp:Label ID="lblEnclosure" runat="server" Visible="true">Enclosure</asp:Label>
                                                    </th>
                                                    <th align="left">
                                                        Catalog Number
                                                    </th>
                                                    <th>
                                                        &nbsp;&nbsp;&nbsp;
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCaseSizes" runat="server" Visible="false" OnSelectedIndexChanged="ddlCaseSizes_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblCaseSize" CssClass="textlabel" runat="server"></asp:Label>&nbsp;
                                                        <asp:Label ID="lblCaseSizeCalc" CssClass="textlabel" runat="server" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblCaseSizeCalcDisplay" CssClass="textlabel" runat="server" Visible="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblCatalogNo" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEnclosureData" CssClass="textlabel" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <!-- second line of catalog number.  It is split, to make it less horizontally intrusive. -->
                                                        <asp:Label ID="lblCatalogNoExt" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <!-- This is a multiline text box to enter a custom OEM catalog number -->
                                                        <asp:TextBox ID="txtCatalogNoOEM" runat="server" Columns="20" MaxLength="30" Width="140"
                                                            TextMode="MultiLine" Height="50" Visible="false"></asp:TextBox>
                                                        <asp:Label ID="lblStockID" runat="server" Text="" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblCustomID" runat="server" Text="" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <!-- Wall bracket illustration -->
                                                    <td>
                                                        <asp:Image ID="imgBracketSide" runat="server" ImageUrl="images/Side%20Mounted%20Bracket.png"
                                                            AlternateText="Side Mounted Bracket" />
                                                    </td>
                                                    <td>
                                                        <asp:Image ID="imgBracketBottom" runat="server" ImageUrl="images/Bottom%20Mounted%20Bracket.png"
                                                            AlternateText="Bottom Mounted Bracket" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlCaseSizes" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlConfiguration" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlKFactor" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlPrimaryVoltage" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlSecondaryVoltage" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlTempRise" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtCompany" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="rblElectrostaticShield" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="rblPhase" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="rblWindings" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <!-- TABLE 2: Item, Quantity, Unit Price, Extended Price, Total (5 columns) -->
                                    <asp:UpdatePanel ID="uplblStandardOrCustom" runat="server">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                                <!-- Titles -->
                                                <tr>
                                                    <th align="left">
                                                        Item
                                                    </th>
                                                    <th align="left">
                                                        Quantity
                                                    </th>
                                                    <th align="right">
                                                        Unit&nbsp;Price
                                                    </th>
                                                    <th align="right">
                                                        Ext&nbsp;Price
                                                    </th>
                                                    <th align="center">
                                                        Total
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <!-- Says either "Stock Transformer" or "Custom Transformer" as the item description -->
                                                        <asp:Label ID="lblStandardOrCustom" CssClass="textlabel" runat="server" Text="Stock Transformer"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <!-- Item Quantity -->
                                                        <asp:Button ID="btnReduceQty" runat="server" Text="-" Width="20" Height="20" OnClick="btnReduceQty_Click" />&nbsp;/
                                                        <asp:Button ID="btnAddQty" runat="server" Text="+" Width="20" Height="20" OnClick="btnAddQty_Click" />
                                                        &nbsp;
                                                        <asp:TextBox ID="txtQuantity" runat="server" Columns="3" MaxLength="3" Width="25"
                                                            AutoPostBack="true" OnTextChanged="txtQuantity_TextChanged"></asp:TextBox>
                                                        <asp:Label ID="lblQuantityReqd" CssClass="errorlabel" runat="server" Text="* Required"
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblQuantityInvalid" CssClass="errorlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <!-- Item Price -->
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblUnitPriceSign" runat="server">$</asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtUnitPrice" runat="server" Columns="10" MaxLength="10" Width="60"
                                                                        AutoPostBack="true" OnTextChanged="txtUnitPrice_TextChanged" Visible="false"></asp:TextBox>
                                                                    <asp:TextBox ID="txtUnitPriceChanged" runat="server" Columns="10" MaxLength="10"
                                                                        Width="60" AutoPostBack="true" Visible="false" CssClass="textalert" OnTextChanged="txtUnitPriceChanged_TextChanged"></asp:TextBox>
                                                                    <asp:Label ID="lblUnitPrice" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Label ID="lblUnitPriceInvalid" CssClass="errorlabel" runat="server" Text=""
                                                                        Visible="false"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="right">
                                                        <!-- Ext Price -->
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblItemExtPriceSign" runat="server" Text="$"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblItemExtPrice" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td align="center">
                                                        <!-- Total Price (first row only) -->
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblTotalExtPriceSign" runat="server" Text="$"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblTotalExtPrice" CssClass="textlabelemphasis" runat="server" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <div id="RBKitInfo" runat="server">
                                                    <tr>
                                                        <!-- Row 2: Rodent Bird Kit -->
                                                        <td>
                                                            <!-- Item name -->
                                                            <asp:Label ID="lblRBKitName" runat="server" CssClass="textlabel" Text=""></asp:Label>
                                                            <asp:TextBox ID="txtRBKitNumber" runat="server" Text="" Visible="false"></asp:TextBox>
                                                            <asp:Label ID="lblRBKitID" runat="server" Text="" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <!-- Item Quantity -->
                                                            <asp:Button ID="btnRBKitReduceQty" runat="server" Text="-" Width="20" Height="20"
                                                                OnClick="btnRBKitReduceQty_Click" />&nbsp;/
                                                            <asp:Button ID="btnRBKitAddQty" runat="server" Text="+" Width="20" Height="20" OnClick="btnRBKitAddQty_Click" />
                                                            &nbsp;
                                                            <asp:TextBox ID="txtRBKitQty" runat="server" Columns="3" MaxLength="3" Width="25"
                                                                AutoPostBack="true" OnTextChanged="txtRBKitQty_TextChanged"></asp:TextBox>
                                                            <asp:Label ID="lblRBKitQtyInvalid" CssClass="errorlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblRBKitQtyOrig" runat="server" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblRBKitNameOrig" runat="server" Text="" Visible="false"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Item Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        $
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRBKitPrice" runat="server" Columns="10" MaxLength="10" Width="60"
                                                                            AutoPostBack="true" OnTextChanged="txtRBKitPrice_TextChanged" Visible="false"></asp:TextBox>
                                                                        <asp:Label ID="lblRBKitPrice" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblRBKitPriceCalc" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblRBKitPriceInvalid" CssClass="errorlabel" runat="server" Text=""
                                                                            Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Ext Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblRBKitExtPriceSign" runat="server" Text="$"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblRBKitExtPrice" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </div>
                                                <div id="WBKitInfo" runat="server">
                                                    <tr>
                                                        <!-- Row 3: Wall bracket -->
                                                        <td>
                                                            <!-- Item name -->
                                                            <asp:Label ID="lblWBKitName" runat="server" CssClass="textlabel" Text=""></asp:Label>
                                                            <asp:TextBox ID="txtWBKitNumber" runat="server" Text="" Visible="false"></asp:TextBox>
                                                            <asp:Label ID="lblWBKitID" runat="server" Text="" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <!-- Item Quantity -->
                                                            <asp:Button ID="btnWBReduceQty" runat="server" Text="-" Width="20" Height="20" OnClick="btnWBReduceQty_Click" />&nbsp;/
                                                            <asp:Button ID="btnWBAddQty" runat="server" Text="+" Width="20" Height="20" OnClick="btnWBAddQty_Click" />
                                                            &nbsp;
                                                            <asp:TextBox ID="txtWBKitQty" runat="server" Columns="3" MaxLength="3" Width="25"
                                                                AutoPostBack="true" OnTextChanged="txtWBKitQty_TextChanged"></asp:TextBox>
                                                            <asp:Label ID="lblWBKitQtyInvalid" CssClass="errorlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblWBKitQtyOrig" runat="server" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblWBKitNameOrig" runat="server" Text="" Visible="false"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Item Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        $
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtWBKitPrice" runat="server" Columns="10" MaxLength="10" Width="60"
                                                                            AutoPostBack="true" OnTextChanged="txtWBKitPrice_TextChanged" Visible="false"></asp:TextBox>
                                                                        <asp:Label ID="lblWBKitPrice" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblWBKitPriceCalc" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblWBKitPriceInvalid" CssClass="errorlabel" runat="server" Text=""
                                                                            Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Ext Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblWBKitExtPriceSign" runat="server" Text="$"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblWBKitExtPrice" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </div>
                                                <div id="KitInfo" runat="server">
                                                    <tr>
                                                        <!-- Row 4: Kit (bottom bracket, louvers) -->
                                                        <td>
                                                            <!-- Item name -->
                                                            <asp:Label ID="lblKitName" runat="server" CssClass="textlabel" Text=""></asp:Label>
                                                            <asp:TextBox ID="txtKitNumber" runat="server" Text="" Visible="false"></asp:TextBox>
                                                            <asp:Label ID="lblKitID" runat="server" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblKitIDPrev" runat="server" Text="" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <!-- Item Quantity -->
                                                            <asp:Button ID="btnKitReduceQty" runat="server" Text="-" Width="20" Height="20" OnClick="btnKitReduceQty_Click" />&nbsp;/
                                                            <asp:Button ID="btnKitAddQty" runat="server" Text="+" Width="20" Height="20" OnClick="btnKitAddQty_Click" />
                                                            &nbsp;
                                                            <asp:TextBox ID="txtKitQty" runat="server" Columns="3" MaxLength="3" Width="25" AutoPostBack="true"
                                                                OnTextChanged="txtKitQty_TextChanged"></asp:TextBox>
                                                            <asp:Label ID="lblKitQtyInvalid" CssClass="errorlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblKitQtyOrig" runat="server" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblKitIDOrig" runat="server" Text="" Visible="false"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Item Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        $
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtKitPrice" runat="server" Columns="10" MaxLength="10" Width="60"
                                                                            AutoPostBack="true" OnTextChanged="txtKitPrice_TextChanged" Visible="false"></asp:TextBox>
                                                                        <asp:Label ID="lblKitPrice" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblKitPriceCalc" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblKitPriceInvalid" CssClass="errorlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Ext Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblKitExtPriceSign" runat="server" Text="$"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblKitExtPrice" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </div>
                                                <div id="OPKitInfo" runat="server">
                                                    <tr>
                                                        <!-- Row 5: OSHPD Kit -->
                                                        <td>
                                                            <!-- Item name -->
                                                            <asp:Label ID="lblOPKitName" runat="server" CssClass="textlabel" Text=""></asp:Label>
                                                            <asp:HyperLink ID="lnkOSHPD" runat="server" NavigateUrl="http://www.mgmtransformer.com/industry-buzz/oshpd-osp/"
                                                                Target="_blank">More Info</asp:HyperLink>
                                                            <asp:TextBox ID="txtOPKitNumber" runat="server" Text="" Visible="false"></asp:TextBox>
                                                            <asp:Label ID="lblOPKitID" runat="server" Text="" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <!-- Item Quantity -->
                                                            <asp:Button ID="btnOPKitReduceQty" runat="server" Text="-" Width="20" Height="20"
                                                                OnClick="btnOPKitReduceQty_Click" />&nbsp;/
                                                            <asp:Button ID="btnOPKitAddQty" runat="server" Text="+" Width="20" Height="20" OnClick="btnOPKitAddQty_Click" />
                                                            &nbsp;
                                                            <asp:TextBox ID="txtOPKitQty" runat="server" Columns="3" MaxLength="3" Width="25"
                                                                AutoPostBack="true" OnTextChanged="txtOPKitQty_TextChanged"></asp:TextBox>
                                                            <asp:Label ID="lblOPKitQtyInvalid" CssClass="errorlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblOPKitQtyOrig" runat="server" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblOPKitNameOrig" runat="server" Text="" Visible="false"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Item Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        $
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtOPKitPrice" runat="server" Columns="10" MaxLength="10" Width="60"
                                                                            AutoPostBack="true" OnTextChanged="txtOPKitPrice_TextChanged" Visible="false"></asp:TextBox>
                                                                        <asp:Label ID="lblOPKitPrice" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblOPKitPriceCalc" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblOPKitPriceInvalid" CssClass="errorlabel" runat="server" Text=""
                                                                            Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Ext Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblOPKitExtPriceSign" runat="server" Text="$"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblOPKitExtPrice" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </div>
                                                <div id="LugKitInfo" runat="server">
                                                    <tr>
                                                        <!-- Row 6: Lug Kit -->
                                                        <td>
                                                            <!-- Item name -->
                                                            <asp:Label ID="lblLugKitName" runat="server" CssClass="textlabel" Text="" Visible="true"></asp:Label>
                                                            <asp:TextBox ID="txtLugKitNumber" runat="server" Text="" Visible="false"></asp:TextBox>
                                                            <asp:Label ID="lblLugKitID" runat="server" Text="" Visible="false"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <!-- Item Quantity -->
                                                            <asp:Button ID="btnLugKitReduceQty" runat="server" Text="-" Width="20" Height="20"
                                                                OnClick="btnLugKitReduceQty_Click" />&nbsp;/
                                                            <asp:Button ID="btnLugKitAddQty" runat="server" Text="+" Width="20" Height="20" OnClick="btnLugKitAddQty_Click" />
                                                            &nbsp;
                                                            <asp:TextBox ID="txtLugKitQty" runat="server" Columns="3" MaxLength="3" Width="25"
                                                                AutoPostBack="true" OnTextChanged="txtLugKitQty_TextChanged"></asp:TextBox>
                                                            <asp:Label ID="lblLugKitQtyInvalid" CssClass="errorlabel" runat="server" Text=""
                                                                Visible="false"></asp:Label>
                                                            <asp:Label ID="lblLugKitQtyOrig" runat="server" Text="" Visible="false"></asp:Label>
                                                            <asp:Label ID="lblLugKitNameOrig" runat="server" Text="" Visible="false"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Item Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        $
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLugKitPrice" runat="server" Columns="10" MaxLength="10" Width="60"
                                                                            AutoPostBack="true" OnTextChanged="txtLugKitPrice_TextChanged" Visible="false"></asp:TextBox>
                                                                        <asp:Label ID="lblLugKitPrice" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblLugKitPriceCalc" CssClass="textlabel" runat="server" Text="" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblLugKitPriceInvalid" CssClass="errorlabel" runat="server" Text=""
                                                                            Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Ext Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblLugKitExtPriceSign" runat="server" Text="$"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblLugKitExtPrice" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </div>
                                                <div id="ExpediteKitInfo" runat="server">
                                                    <tr>
                                                        <!-- Row 6: Expedite -->
                                                        <td>
                                                            <!-- Item name -->
                                                            <asp:Label ID="lblRush" runat="server" CssClass="textlabel">Expedite (Mfg time, not shipping)</asp:Label>
                                                        </td>
                                                        <td>
                                                            <!-- Item Quantity -->
                                                            <asp:DropDownList runat="server" ID="ddlExpedite" Visible="true" OnSelectedIndexChanged="ddlExpedite_SelectedIndexChanged"
                                                                AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Item Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblExpeditePriceSign" runat="server" Text="$"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtExpeditePrice" runat="server" Columns="10" MaxLength="10" Width="60"
                                                                            AutoPostBack="true" OnTextChanged="txtExpeditePrice_TextChanged" Visible="false"></asp:TextBox>
                                                                        <asp:Label ID="lblExpeditePrice" runat="server" Text="" CssClass="textlabel" Visible="false"></asp:Label>
                                                                        <asp:Label ID="lblExpeditePriceCalc" runat="server" Text="" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblExpeditePriceInvalid" runat="server" Text="" CssClass="textlabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Ext Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblExpediteExtPriceSign" runat="server" Text="$"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblExpediteExtPrice" CssClass="textlabel" runat="server" Text=""></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </div>
                                                <div id="ShippingInfo" runat="server">
                                                    <tr>
                                                        <!-- Row 7: Expedite Limit (when applicable) -->
                                                        <td colspan="5">
                                                            <asp:Label ID="lblExpediteLimit" runat="server" CssClass="errorlabel" Text="Limited expedite options due to stainless steel enclosure."
                                                                Visible="false"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <!-- Row 8: Adjustment -->
                                                        <td>
                                                            <!-- Item name -->
                                                            <asp:Label ID="lblCustomShipping" runat="server" CssClass="textlabel">Adjustment Reason:</asp:Label>
                                                        </td>
                                                        <td>
                                                            <!-- Item Quantity -->
                                                            <asp:TextBox ID="txtShippingReason" runat="server" Columns="30" MaxLength="50" Width="150"></asp:TextBox>
                                                            <asp:Label ID="lblShippingReason" runat="server" CssClass="textlabel"></asp:Label>
                                                            <asp:Label ID="lblShippingReasonInvalid" runat="server" CssClass="errorlabel" Visible="false"><br />* Please enter reason.</asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Item Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        $
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtShippingAmount" runat="server" Columns="10" AutoPostBack="true"
                                                                            MaxLength="10" Width="60" OnTextChanged="txtShippingAmount_TextChanged"></asp:TextBox>
                                                                        <asp:Label ID="lblShippingAmount" runat="server" Text="" CssClass="textlabel" Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblShippingAmountInvalid" runat="server" Text="" CssClass="errorlabel"
                                                                            Visible="false"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="right">
                                                            <!-- Ext Price -->
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblShippingAmtExtSign" runat="server" Text="$"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblShippingAmtExt" runat="server" Text="" CssClass="textlabel"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <!-- No Free Shipping checkbox in Total column -->
                                                            <asp:CheckBox ID="chkNoFreeShipping" runat="server" Text="No Free Shipping&nbsp;&nbsp;" />
                                                        </td>
                                                    </tr>
                                                </div>
                                                <tr>
                                                    <!-- Row 9: Additional messages (when applicable) -->
                                                    <td colspan="5">
                                                        <asp:Label ID="lblAccessoryChange" runat="server" CssClass="statuslabel" Text="Accessory quantity removed."
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblSeeFactory" runat="server" CssClass="errorlabel" Text="Call factory for pricing."
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="chkMarineDuty" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlCaseColor" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlExpedite" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlKFactor" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlTempRise" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlPrimaryVoltage" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlSecondaryVoltage" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="rblPhase" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="rblStandardOrCustom" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="rblWindings" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtExpeditePrice" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtKitPrice" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtKitQty" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtOPKitPrice" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtOPKitQty" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtQuantity" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtRBKitPrice" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtRBKitQty" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtShippingAmount" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtShippingReason" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtUnitPrice" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtUnitPriceChanged" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtWBKitPrice" EventName="TextChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtWBKitQty" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <!-- TABLE 3: Buttons, Messages (1 column) -->
                                    <asp:UpdatePanel ID="upbtnSave" runat="server">
                                        <ContentTemplate>
                                            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                                <!-- Save Button -->
                                                <tr align="center">
                                                    <td>
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" Width="120" Height="50" Visible="false"
                                                            OnClick="btnSave_Click" TabIndex="20" />&nbsp;
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:Button ID="btnCopyItem" runat="server" Text="Copy Item" Width="120" Height="30"
                                                            Visible="false" OnClick="btnCopyItem_Click" TabIndex="21" />&nbsp;
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:Label ID="lblCaseSizeRequired" runat="server" Text="Please enter a case size.<br />"
                                                            CssClass="errorlabel" Visible="false"></asp:Label>
                                                        <asp:Label ID="lblNotSaved" runat="server" Text="Error.<br /><b>NOT<b> saved." CssClass="errorlabel"
                                                            Visible="false"></asp:Label>
                                                        <asp:Label ID="lblCaseSizeExceeded" runat="server" Text="Unit Sub case size<br /><b>NOT<b> supported."
                                                            CssClass="errorlabel" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:Label ID="lblLeadTimesTitle" runat="server" CssClass="textlabel"><b>Lead Times</b></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td>
                                                        <asp:UpdatePanel ID="uplblLeadTimes" runat="server">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblLeadTimes" runat="server" CssClass="textlabel">Three to four weeks.</asp:Label>
                                                                <asp:Label ID="lblShipDays" runat="server" Visible="false"></asp:Label>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="rblSpecialTypes" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="ddlCaseSizes" EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblUnitPriceCalcTitle" runat="server" Text="Calc Price" Visible="true"></asp:Label>
                                                        <asp:Label ID="lblUnitPriceList" runat="server" Text="Calc Price" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblUnitPriceCalcSign" runat="server" Text="$" Visible="true"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblUnitPriceCalc" runat="server" CssClass="textlabel" Text="" Visible="true"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCopyItem" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnApprove" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <!-- totals (3 tables) -->
                    </asp:Panel>
                    <!-- PanelTotals  Visible set True/False -->
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlConfiguration" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="txtCompany" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            <!-- Show/Hide detail_totals -->
        </td>
    </tr>
    <tr align="center">
        <td>
            <asp:UpdatePanel ID="uppnlQuoteItems" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlQuoteItems" Visible="false" runat="server">
                        <table border="0" width="100%">
                            <tr align="left">
                                <td>
                                    <asp:Label ID="lblDeleteItem" runat="server" Text="Delete this item?" Visible="false"
                                        CssClass="errorlabel"></asp:Label>
                                    &nbsp;
                                    <asp:Button ID="btnDeleteItemYes" runat="server" Text="Yes" Visible="false" OnClick="btnDeleteItemYes_Click" />
                                    &nbsp;
                                    <asp:Button ID="btnDeleteItemNo" runat="server" Text="No" Visible="false" OnClick="btnDeleteItemNo_Click" />
                                    &nbsp;<asp:HiddenField ID="hidDetailID" runat="server" />
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <h2>
                                        Quote Items</h2>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:UpdatePanel ID="upgvQuoteItems" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvQuoteItems" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                                CellPadding="4" AllowPaging="True" PageSize="50" ForeColor="#333333" GridLines="Vertical"
                                                DataKeyNames="QuoteDetailsID,QuoteID,DetailID,EditDisplay" OnRowCommand="gvQuoteItems_RowCommand"
                                                DataSourceID="dsQuote" OnRowUpdated="gvQuoteItems_RowUpdated" Font-Size="X-Small"
                                                OnSelectedIndexChanged="gvQuoteItems_SelectedIndexChanged" OnRowDataBound="gvQuoteItems_RowDataBound">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Font-Size="X-Small" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="false" ConvertEmptyStringToNull="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="Edit" runat="server" CausesValidation="false" CommandName="Select"
                                                                CommandArgument="<%# gvQuoteItems.DataKeys[((GridViewRow)Container).RowIndex].Values[0] %>"
                                                                Text="<%# gvQuoteItems.DataKeys[((GridViewRow)Container).RowIndex].Values[3] %>"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Underline="true" />
                                                        <ItemStyle ForeColor="Blue" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="false" ConvertEmptyStringToNull="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="Delete" runat="server" CausesValidation="false" CommandName="Delete"
                                                                CommandArgument="<%# gvQuoteItems.DataKeys[((GridViewRow)Container).RowIndex].Values[0] %>"
                                                                Text="Del"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ControlStyle Font-Underline="true" />
                                                        <ItemStyle ForeColor="Blue" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DetailID" ReadOnly="True" SortExpression="DetailID" Visible="false" />
                                                    <asp:BoundField DataField="DetailIDDisplay" ReadOnly="True" SortExpression="DetailIDDisplay"
                                                        HeaderText="#">
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
                                                    <asp:BoundField DataField="Quantity" HeaderText="Qty" SortExpression="Quantity">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Price" HeaderText="Unit Price" SortExpression="Price"
                                                        DataFormatString="{0:C2}">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ExtendedPrice" HeaderText="Ext Price" ReadOnly="True"
                                                        SortExpression="ExtendedPrice" DataFormatString="{0:C2}">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ShipText" HeaderText="Ship Days" ReadOnly="True" SortExpression="ShipText">
                                                        <ItemStyle HorizontalAlign="Center" />
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
                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:SqlDataSource ID="dsQuote" runat="server" ConnectionString="<%$ ConnectionStrings:mgmdb %>"
                                        SelectCommand="usp_QuoteItemList_20180914" SelectCommandType="StoredProcedure"
                                        DeleteCommand="DELETE QuoteDetails WHERE QuoteID=@quote_id AND DetailID=@detail_id">
                                        <DeleteParameters>
                                            <asp:SessionParameter Name="quote_id" Type="Int32" SessionField="QuoteID" />
                                            <asp:SessionParameter Name="detail_id" Type="Int32" SessionField="DetailID" />
                                        </DeleteParameters>
                                        <SelectParameters>
                                            <asp:SessionParameter Name="quote_id" SessionField="QuoteID" Type="Int32" />
                                            <asp:SessionParameter Name="user_name" SessionField="UserName" Type="String" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <!-- Backup fields, set by ClearEntries(), restored by RestoreEntries(), when changing major selections,
                            such as Windings, Phase, KVA, etc. -->
                                    <!-- Change KFactor and Temp Rise to defaults if KVA gets over certain amount.  -->
                                    <asp:UpdatePanel ID="uphidKFactorTempRise" runat="server">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="hidKFactor" runat="server" />
                                            <asp:HiddenField ID="hidTempRise" runat="server" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlKVA" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="txtKVA" EventName="TextChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:HiddenField ID="hidCatalogNo" runat="server" />
                                    <asp:HiddenField ID="hidEnclosureType" runat="server" />
                                    <asp:HiddenField ID="hidIsTapsNone" runat="server" />
                                    <asp:HiddenField ID="hidKVA" runat="server" />
                                    <asp:HiddenField ID="hidKVAUsed" runat="server" />
                                    <asp:HiddenField ID="hidNEMA" runat="server" />
                                    <asp:HiddenField ID="hidPhase" runat="server" />
                                    <asp:HiddenField ID="hidPrimaryVoltage" runat="server" />
                                    <asp:HiddenField ID="hidPrimaryVoltageDW" runat="server" />
                                    <asp:HiddenField ID="hidSecondaryVoltage" runat="server" />
                                    <asp:HiddenField ID="hidSecondaryVoltageDW" runat="server" />
                                    <asp:HiddenField ID="hidElectrostaticShield" runat="server" />
                                    <asp:HiddenField ID="hidFreq" runat="server" />
                                    <asp:HiddenField ID="hidSoundLevel" runat="server" />
                                    <asp:HiddenField ID="hidEfficiency" runat="server" />
                                    <asp:HiddenField ID="hidForExport" runat="server" />
                                    <asp:HiddenField ID="hidEnclosureMtl" runat="server" />
                                    <asp:HiddenField ID="hidShipWeight" runat="server" />
                                    <asp:HiddenField ID="hidKitHasRainHood" runat="server" />
                                    <asp:HiddenField ID="hidCaseColor" runat="server" />
                                    <asp:HiddenField ID="hidCaseColorOther" runat="server" />
                                    <asp:HiddenField ID="hidTENV" runat="server" />
                                    <asp:HiddenField ID="hidMarineDuty" runat="server" />
                                    <asp:HiddenField ID="hidMadeInUSA" runat="server" />
                                    <asp:HiddenField ID="hidSpecialTypes" runat="server" />
                                    <asp:HiddenField ID="hidSpecialFeatures" runat="server" />
                                    <asp:HiddenField ID="hidSpecialFeatureNotes" runat="server" />
                                    <asp:HiddenField ID="hidCustomerTag" runat="server" />
                                    <asp:HiddenField ID="hidQty" runat="server" />
                                    <asp:HiddenField ID="hidWBKitQty" runat="server" />
                                    <asp:HiddenField ID="hidKitQty" runat="server" />
                                    <asp:HiddenField ID="hidRBKitQty" runat="server" />
                                    <asp:HiddenField ID="hidOPKitQty" runat="server" />
                                    <asp:HiddenField ID="hidExpediteDays" runat="server" />
                                    <asp:HiddenField ID="hidAdjustAmt" runat="server" />
                                    <asp:HiddenField ID="hidAdjustReason" runat="server" />
                                    <asp:HiddenField ID="hidButtonPressed" runat="server" />
                                    <asp:HiddenField ID="hidEfficiencyExempt" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <!-- Panel QuoteItems -->
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="txtCompany" EventName="TextChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlCustomer" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
            <!-- Show/Hide detail_totals -->
        </td>
    </tr>
</table>
