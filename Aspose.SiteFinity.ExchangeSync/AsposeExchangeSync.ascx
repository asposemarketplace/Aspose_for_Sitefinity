<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="AsposeExchangeSync.ascx.cs" Inherits="Aspose.ExchangeSync.AsposeExchangeSync" %>

<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css">
<link rel="stylesheet" type="text/css" media="all" href="<%= ResolveUrl("~/Addons/AsposeExchangeSync/css/AsposeExchangeSync.css") %>" />

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $('.selectAllCheckBox input[type="checkbox"]').click(function (event) {  //on click
            if (this.checked) { // check select status
                $('.selectableCheckBox input[type="checkbox"]').each(function () { //loop through each checkbox
                    this.checked = true;  //select all checkboxes with class "checkbox1"              
                });
            } else {
                $('.selectableCheckBox input[type="checkbox"]').each(function () { //loop through each checkbox
                    this.checked = false; //deselect all checkboxes with class "checkbox1"                      
                });
            }
        });

        $('#ExchangeToSitefinitySyncSettigns input[type="radio"]').click(function () {
            if ($(this).val() == "UserRadioButton") {
                $('#usertTypeDiv').show();
                $('#memberTypeDiv').hide();
            }
            else {
                $('#memberTypeDiv').show();
                $('#usertTypeDiv').hide();
            }
        });
    });
</script>

<table style="width: auto; width: 560px; margin-left: auto; margin-right: auto;">
    <tr>
        <td>
            <br />
            <div class="aspsoeExchangeSync">
                <h2>Aspose .NET Exchange Sync for Sitefinity</h2>
                <div class="FormMessage FormInfo" runat="server" visible="false" id="LoggedInErrorDiv">You must be logged-in to use this module</div>

                <div id="moduleMainDiv" runat="server">
                    <div class="aspsoeExchangeSyncHome">
                        <table>
                            <tr>
                                <td>

                                    <asp:LinkButton ID="ExchangeToSitefinityHyperLink" runat="server" OnClick="ExchangeToSitefinityHyperLink_Click" ValidationGroup="DoNotCheck4">
                            <img width="32" height="32" src="<%= ResolveUrl("~/Addons/AsposeExchangeSync/Images/ExchangeSync.png") %>" style="border-width: 0px;">
                                <br />Exchange to Sitefinity<br /> Sync
                                    </asp:LinkButton></td>
                                <td>
                                    <asp:LinkButton ID="SitefinityToExchangeHyperLink" runat="server" OnClick="SitefinityToExchangeHyperLink_Click" ValidationGroup="DoNotCheck5">                            
                            
                                <img width="32" height="32" src="<%= ResolveUrl("~/Addons/AsposeExchangeSync/Images/ExchangeSync.png") %>" style="border-width: 0px;">
                <br />Sitefinity to Exchange<br /> Sync                                

                                    </asp:LinkButton></td>
                                <td>
                                    <asp:LinkButton ID="ExchangeSettingsHyperLink" runat="server" OnClick="ExchangeSettingsHyperLink_Click" ValidationGroup="DoNotCheck6">
                                <img width="32" height="32" src="<%= ResolveUrl("~/Addons/AsposeExchangeSync/Images/ExchangeSettings.png") %>" style="border-width: 0px;">
                                <br />Exchange Settings
                                    </asp:LinkButton></td>
                            </tr>
                        </table>
                    </div>

                    <div runat="server" id="ExchangeToSitefinitySync" visible="false">
                        <hr />

                        <h2>Exchange to Sitefinity Sync</h2>
                        <div id="Into_Div" runat="server" class="into-text">
                            Exchange to Sitefinity Sync allows you to Sync contacts between Exchange Server and Sitefinity. Please click on 'Fetch Exchange Contacts' button below to get started
                <br />
                            <br />
                            <asp:Button ID="GetExchangeContactsButton" CssClass="btn" runat="server" ValidationGroup="DoNotCheck2" Text="Fetch Exchange Contacts" OnClick="GetExchangeContactsButton_Click" />
                            <br />
                        </div>
                        <div class="FormMessage FormInfo" runat="server" visible="false" id="NoRowSelectedErrorDiv">
                            Please select one or more contacts to continue
                        </div>
                        <div id="ExchangeToSitefinity_MainDiv" runat="server" visible="false">
                            <div class="stepHeading">
                                Step 1: Select one or more contacts to Import them to Sitefinity
                            </div>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 60%; vertical-align: top;">
                                        <div style="max-height: 300px; overflow: auto;">
                                            <asp:GridView ID="ExchangeContactsGridView" EmptyDataText="There are no contacts." Width="100%" EmptyDataRowStyle-CssClass="emptyClass"
                                                GridLines="None" BorderWidth="0" AutoGenerateColumns="false" HeaderStyle-CssClass="rgHeader"
                                                CssClass="rgMasterTable" DataKeyNames="Email" ClientIDMode="Static" runat="server">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-CssClass="rgHeader" HeaderStyle-Width="20px">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="SelectAllCheckBox" CssClass="selectAllCheckBox" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="SelectedCheckBox" CssClass="selectableCheckBox" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DisplayName" HeaderStyle-CssClass="rgHeader" HeaderText="Display Name" />
                                                    <asp:BoundField DataField="Email" HeaderStyle-CssClass="rgHeader" HeaderText="Email" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                    <td style="width: 30px;"></td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <div class="stepHeading">
                                Step 2: Select Role(s) and click 'Exchange To Sitefinity Sync' button below
                            </div>
                            <div id="ExchangeToSitefinitySyncSettigns">
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>

                                            <asp:GridView ID="RolesGridView" EmptyDataText="There are no roles." EmptyDataRowStyle-CssClass="emptyClass"
                                                GridLines="None" BorderWidth="0" AutoGenerateColumns="false" ShowHeader="false" CssClass="rgMasterTable"
                                                DataKeyNames="id" ClientIDMode="Static" runat="server">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="SelectAllCheckBox" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="SelectedCheckBox" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Name" HeaderText="RoleName" />
                                                </Columns>
                                            </asp:GridView>


                                        </td>
                                        <td style="width: 40px;"></td>
                                        <td>
                                            <asp:Button ID="ExchangeToSitefinitySyncButton" CssClass="btn btn-primary" ValidationGroup="DoNotCheck3" OnClick="ExchangeToSitefinitySyncButton_Click" runat="server" Text="Exchange To Sitefinity Sync"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div id="ProcessSummaryDiv" runat="server" visible="false">
                            <asp:Literal ID="ImportedLiteral" runat="server"></asp:Literal>
                            <br />
                            <br />
                            <asp:Literal ID="AlreadyExistingLiteral" runat="server"></asp:Literal>
                        </div>
                    </div>

                    <div runat="server" id="SitefinityToExchangeSync" visible="false">
                        <hr />
                        <h2>Sitefinity to Exchange Sync</h2>
                        <div id="Div1" runat="server" class="into-text">
                            Sitefinity to Exchange Sync allows you to Sync contacts between Sitefinity and Exchange Server. Please click on 'Fetch Sitefinity Users' button below to get started
                            <br />
                            <br />
                            <asp:Button ID="GetSitefinityUsersButton" CssClass="btn" runat="server" ValidationGroup="DoNotCheck2" Text="Fetch Sitefinity Users" OnClick="GetSitefinityUsersButton_Click" />            
                            <br />
                            <br />
                        </div>

                        <div class="FormMessage FormInfo" runat="server" visible="false" id="Div3">
                            Please select one or more users to continue
                        </div>

                        <div id="SitefinityToExchange_MainDiv" runat="server" visible="false">
                            <div class="stepHeading">
                                Step 1: Select one or more users to Import them to Exchange server
                            </div>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 60%; vertical-align: top;">
                                        <div style="max-height: 300px; overflow: auto;">
                                            <asp:GridView ID="SitefinityUsersGridView" EmptyDataText="There are no users." Width="100%" EmptyDataRowStyle-CssClass="emptyClass"
                                                GridLines="None" BorderWidth="0" AutoGenerateColumns="false" HeaderStyle-CssClass="rgHeader"
                                                CssClass="rgMasterTable" DataKeyNames="Email" ClientIDMode="Static" runat="server">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-CssClass="rgHeader" HeaderStyle-Width="20px">
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="SelectAllCheckBox" CssClass="selectAllCheckBox" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="SelectedCheckBox" CssClass="selectableCheckBox" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Username" HeaderStyle-CssClass="rgHeader" HeaderText="Username" />

                                                    <asp:TemplateField HeaderStyle-CssClass="rgHeader">
                                                        <HeaderTemplate>
                                                            User
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "FirstName" ) %>&nbsp;
                                                            <%# DataBinder.Eval(Container.DataItem, "LastName" ) %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="Email" HeaderStyle-CssClass="rgHeader" HeaderText="Email" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                    <td style="width: 30px;"></td>
                                    <td>&nbsp;</td>
                                    <td style="vertical-align: top;">&nbsp;</td>
                                </tr>
                            </table>

                            <br />
                            <br />

                            <div class="stepHeading">
                                Step 2: Click 'Sitefinity To Exchange Sync' button below
                            </div>
                            <table>
                                <tr>
                                    <td></td>
                                    <td style="width: 40px;"></td>
                                    <td>
                                        <asp:Button ID="SitefinityToExchangeSyncButton" CssClass="btn btn-primary" ValidationGroup="DoNotCheck3" OnClick="SitefinityToExchangeSyncButton_Click" runat="server" Text="Sitefinity To Exchange Sync"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="SitefinityToExchange_ProcessSummaryDiv" runat="server" visible="false">
                            <asp:Literal ID="SitefinityToExchange_ImportedLiteral" runat="server"></asp:Literal>
                            <br />
                            <br />
                            <asp:Literal ID="SitefinityToExchange_AlreadyExistingLiteral" runat="server"></asp:Literal>
                        </div>
                    </div>

                    <div id="ExchangeSettings" runat="server" visible="false">
                        <hr />
                        <div class="Form ManageUsers Clear ui-tabs ui-widget ui-widget-content ui-corner-all">
                            <div class="settingDetails Clear" id="settingDetails">
                                <div class="udContent Clear">
                                    <fieldset>
                                        <div class="FormItem">
                                            <h2>Exchange Server details</h2>
                                        </div>
                                        <div class="FormMessage FormInfo errorMessage" runat="server" visible="false" id="ExchangeCredsErrorDiv">
                                            <strong>Oops!</strong> We are unable to connect to mail server using the information you have provided. Please check the information below and try again.
                                        </div>
                                        <table class="exchangeSyncSettingsTable" >
                                            <tr>
                                                <td>
                                                    <label><span class=" FormRequired">Server URL:</span></label></td>
                                                <td>
                                                    <asp:TextBox ID="ServerURLTextBox" CssClass="form-control" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:RequiredFieldValidator ControlToValidate="ServerURLTextBox" CssClass="FormMessage FormError" ID="RequiredFieldValidator1" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label><span class=" FormRequired">Username:</span></label></td>
                                                <td>
                                                    <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="form-control"></asp:TextBox>

                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator CssClass="FormMessage FormError" ControlToValidate="UsernameTextBox" ID="RequiredFieldValidator2" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label><span class=" FormRequired">Password:</span></label></td>
                                                <td>
                                                    <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox></td>
                                                <td>
                                                    <asp:RequiredFieldValidator CssClass="FormMessage FormError" ControlToValidate="PasswordTextBox" ID="RequiredFieldValidator3" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label><span class=" FormRequired" id="Span1">Domain:</span></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="DomainTextBox" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:Button ID="SaveButton" CssClass="btn btn-primary" runat="server" Text="Save" OnClick="SaveButton_Click"></asp:Button>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="CancelButton" OnClick="CancelButton_Click" ValidationGroup="DoNotCheck" CssClass="btn" runat="server" Text="Cancel"></asp:Button></td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:HiddenField ID="ExchangeToSitefinityClickedHiddenField" Value="false" runat="server" />
                <asp:HiddenField ID="SitefinityToExchangeClickedHiddenField" Value="false" runat="server" />

            </div>

        </td>
    </tr>

</table>


