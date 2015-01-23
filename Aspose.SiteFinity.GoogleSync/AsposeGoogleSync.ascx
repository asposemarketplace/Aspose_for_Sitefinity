<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="AsposeGoogleSync.ascx.cs" Inherits="Aspose.GoogleSync.AsposeGoogleSync" %>

<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.1/css/bootstrap.min.css">
<link rel="stylesheet" type="text/css" media="all" href="<%= ResolveUrl("~/Addons/AsposeGoogleSync/css/AsposeGoogleSync.css") %>" />

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

        $('#GoogleToSitefinitySyncSettigns input[type="radio"]').click(function () {
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

<table style="width: auto; width: 580px; margin-left: auto; margin-right: auto;">
    <tr>
        <td>
            <br />
            <div class="aspsoeGoogleSync">
                <h2>Aspose .NET Google Sync for Sitefinity</h2>
                <div class="FormMessage FormInfo" runat="server" visible="false" id="LoggedInErrorDiv">You must be logged-in to use this module</div>

                <div id="moduleMainDiv" runat="server">
                    <div class="aspsoeGoogleSyncHome">
                        <table>
                            <tr>
                                <td>

                                    <asp:LinkButton ID="GoogleToSitefinityHyperLink" runat="server" OnClick="GoogleToSitefinityHyperLink_Click" ValidationGroup="DoNotCheck4">
                            <img width="32" height="32" src="<%= ResolveUrl("~/Addons/AsposeGoogleSync/Images/GoogleSync.png") %>" style="border-width: 0px;">
                                <br />Google to Sitefinity<br /> Sync
                                    </asp:LinkButton></td>
                                <td>
                                    <asp:LinkButton ID="SitefinityToGoogleHyperLink" runat="server" OnClick="SitefinityToGoogleHyperLink_Click" ValidationGroup="DoNotCheck5">                            
                            
                                <img width="32" height="32" src="<%= ResolveUrl("~/Addons/AsposeGoogleSync/Images/GoogleSync.png") %>" style="border-width: 0px;">
                <br />Sitefinity to Google<br /> Sync                                

                                    </asp:LinkButton></td>
                                <td>
                                    <asp:LinkButton ID="GoogleSettingsHyperLink" runat="server" OnClick="GoogleSettingsHyperLink_Click" ValidationGroup="DoNotCheck6">
                                <img width="32" height="32" src="<%= ResolveUrl("~/Addons/AsposeGoogleSync/Images/GoogleSettings.png") %>" style="border-width: 0px;">
                                <br />Google Settings
                                    </asp:LinkButton></td>
                            </tr>
                        </table>
                    </div>

                    <div runat="server" id="GoogleToSitefinitySync" visible="false">
                        <hr />

                        <h2>Google to Sitefinity Sync</h2>
                        <div id="Into_Div" runat="server" class="into-text">
                            Google to Sitefinity Sync allows you to Sync contacts between Google Server and Sitefinity. Please click on 'Fetch Google Contacts' button below to get started
                <br />
                            <br />
                            <asp:Button ID="GetGoogleContactsButton" CssClass="btn" runat="server" ValidationGroup="DoNotCheck2" Text="Fetch Google Contacts" OnClick="GetGoogleContactsButton_Click" />
                            <br />
                        </div>
                        <div class="FormMessage FormInfo" runat="server" visible="false" id="NoRowSelectedErrorDiv">
                            Please select one or more contacts to continue
                        </div>
                        <div id="GoogleToSitefinity_MainDiv" runat="server" visible="false">
                            <div class="stepHeading">
                                Step 1: Select one or more contacts to Import them to Sitefinity
                            </div>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 60%; vertical-align: top;">
                                        <div style="max-height: 300px; overflow: auto;">
                                            <asp:GridView ID="GoogleContactsGridView" EmptyDataText="There are no contacts." Width="100%" EmptyDataRowStyle-CssClass="emptyClass"
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
                                Step 2: Select Role(s) and click 'Google To Sitefinity Sync' button below
                            </div>
                            <div id="GoogleToSitefinitySyncSettigns">
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
                                            <asp:Button ID="GoogleToSitefinitySyncButton" CssClass="btn btn-primary" ValidationGroup="DoNotCheck3" OnClick="GoogleToSitefinitySyncButton_Click" runat="server" Text="Google To Sitefinity Sync"></asp:Button>
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

                    <div runat="server" id="SitefinityToGoogleSync" visible="false">
                        <hr />
                        <h2>Sitefinity to Google Sync</h2>
                        <div id="Div1" runat="server" class="into-text">
                            Sitefinity to Google Sync allows you to Sync contacts between Sitefinity and Google Server. Please click on 'Fetch Sitefinity Users' button below to get started
                            <br />
                            <br />
                            <asp:Button ID="GetSitefinityUsersButton" CssClass="btn" runat="server" ValidationGroup="DoNotCheck2" Text="Fetch Sitefinity Users" OnClick="GetSitefinityUsersButton_Click" />
                            <br />
                            <br />
                        </div>

                        <div class="FormMessage FormInfo" runat="server" visible="false" id="Div3">
                            Please select one or more users to continue
                        </div>

                        <div id="SitefinityToGoogle_MainDiv" runat="server" visible="false">
                            <div class="stepHeading">
                                Step 1: Select one or more users to Import them to Google server
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
                                Step 2: Click 'Sitefinity To Google Sync' button below
                            </div>
                            <table>
                                <tr>
                                    <td></td>
                                    <td style="width: 40px;"></td>
                                    <td>
                                        <asp:Button ID="SitefinityToGoogleSyncButton" CssClass="btn btn-primary" ValidationGroup="DoNotCheck3" OnClick="SitefinityToGoogleSyncButton_Click" runat="server" Text="Sitefinity To Google Sync"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="SitefinityToGoogle_ProcessSummaryDiv" runat="server" visible="false">
                            <asp:Literal ID="SitefinityToGoogle_ImportedLiteral" runat="server"></asp:Literal>
                            <br />
                            <br />
                            <asp:Literal ID="SitefinityToGoogle_AlreadyExistingLiteral" runat="server"></asp:Literal>
                        </div>
                    </div>

                    <div id="GoogleSettings" runat="server" visible="false">
                        <hr />
                        <div class="Form ManageUsers Clear ui-tabs ui-widget ui-widget-content ui-corner-all">
                            <div class="settingDetails Clear" id="settingDetails">
                                <div class="udContent Clear">
                                    <fieldset>
                                        <div class="FormItem">
                                            <h2>Google Server details</h2>
                                        </div>
                                        <b>Note: </b>Please make sure that you have obtained Client ID and Client Secret as explained on <a href="http://www.aspose.com/docs/display/emailnet/Create+project+in+Google+Developer+Console">http://www.aspose.com/docs/display/emailnet/Create+project+in+Google+Developer+Console</a>
                                        <br /><br />
                                        <div class="FormMessage FormInfo errorMessage" runat="server" visible="false" id="GoogleCredsErrorDiv">
                                            <strong>Oops!</strong> We are unable to connect to mail server using the information you have provided. Please check the information below and try again.
                                        </div>
                                        <table class="GoogleSyncSettingsTable">
                                            <tr>
                                                <td>
                                                    <label><span class=" FormRequired">Google Email Address:</span></label></td>
                                                <td>
                                                    <asp:TextBox ID="EmailAddressTextBox" CssClass="form-control" runat="server"></asp:TextBox></td>
                                                <td>
                                                    <asp:RequiredFieldValidator ControlToValidate="EmailAddressTextBox" CssClass="FormMessage FormError" ID="RequiredFieldValidator1" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator></td>
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
                                                    <label><span class=" FormRequired">Client ID:</span></label></td>
                                                <td>
                                                    <asp:TextBox ID="ClientIDTextBox" runat="server" CssClass="form-control"></asp:TextBox>

                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator CssClass="FormMessage FormError" ControlToValidate="ClientIDTextBox" ID="RequiredFieldValidator2" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator></td>
                                            </tr>

                                            <tr>
                                                <td>
                                                    <label><span class=" FormRequired" id="Span1">Client secret:</span></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="ClientSecretTextBox" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                <td>
                                                    <asp:RequiredFieldValidator CssClass="FormMessage FormError" ControlToValidate="ClientSecretTextBox" ID="RequiredFieldValidator5" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator></td>
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

                <asp:HiddenField ID="GoogleToSitefinityClickedHiddenField" Value="false" runat="server" />
                <asp:HiddenField ID="SitefinityToGoogleClickedHiddenField" Value="false" runat="server" />

            </div>

        </td>
    </tr>

</table>


