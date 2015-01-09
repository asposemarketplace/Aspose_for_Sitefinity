using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.ExchangeSync.Data;
using Aspose.ExchangeSync.Components;
using Aspose.Email.Exchange;
using Aspose.Email.Mail;
using System.IO;
using System.Net;
using System.Net.Security;
using Aspose.Email.Outlook.Pst;
using Aspose.Email.Outlook;
using System.Web.Security;
using System.Linq;
using System.Collections;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Model;
using Telerik.Sitefinity.Security.Claims;

namespace Aspose.ExchangeSync
{
    public partial class AsposeExchangeSync : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private bool ExchangeSettingsExist
        {
            get
            {
                Aspose_ExchangeSync_ServerDetails exchangeDetailsList = DatabaseHelper.CheckExchangeDetails(SitefinityAPIHelper.CurrentUserId);
                if (exchangeDetailsList != null)
                {
                    ViewState["ExchangeDetails"] = exchangeDetailsList;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void ResetControls()
        {
            ExchangeToSitefinitySync.Visible = false;
            SitefinityToExchangeSync.Visible = false;
            ExchangeSettings.Visible = false;
        }

        protected void ExchangeToSitefinityHyperLink_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (ExchangeSettingsExist)
            {
                ExchangeToSitefinitySyncResetControls();
                ExchangeToSitefinitySync.Visible = true;
            }
            else
            {
                ExchangeSettings.Visible = true;
                ExchangeToSitefinityClickedHiddenField.Value = "true";
            }
        }

        protected void SitefinityToExchangeHyperLink_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (ExchangeSettingsExist)
            {
                SitefinityToExchangeSyncResetControls();
                SitefinityToExchangeSync.Visible = true;
            }
            else
            {
                ExchangeSettings.Visible = true;
                SitefinityToExchangeClickedHiddenField.Value = "true";
            }
        }

        protected void ExchangeSettingsHyperLink_Click(object sender, EventArgs e)
        {
            Aspose_ExchangeSync_ServerDetails exchangeDetailsList = DatabaseHelper.CheckExchangeDetails(SitefinityAPIHelper.CurrentUserId);
            if (exchangeDetailsList != null)
            {
                ServerURLTextBox.Text = exchangeDetailsList.ServerURL;
                UsernameTextBox.Text = exchangeDetailsList.Username;
                PasswordTextBox.Text = exchangeDetailsList.Password;
                DomainTextBox.Text = exchangeDetailsList.Domain;
            }

            ResetControls();
            ExchangeSettings.Visible = true;
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            ExchangeCredsErrorDiv.Visible = false;

            Aspose_ExchangeSync_ServerDetails serverDetails = new Aspose_ExchangeSync_ServerDetails();

            serverDetails.ServerURL = ServerURLTextBox.Text.Trim();
            serverDetails.Username = UsernameTextBox.Text.Trim();
            serverDetails.Password = PasswordTextBox.Text.Trim();
            serverDetails.Domain = DomainTextBox.Text.Trim();

            serverDetails.UserID = SitefinityAPIHelper.CurrentUserId;

            try
            {
                NetworkCredential credentials = new NetworkCredential(serverDetails.Username, serverDetails.Password, serverDetails.Domain);
                IEWSClient client = EWSClient.GetEWSClient(serverDetails.ServerURL, credentials);
            }
            catch (Exception)
            {
                ExchangeCredsErrorDiv.Visible = true;
                return;
            }

            serverDetails.Password = Crypto.Encrypt(serverDetails.Password);

            DatabaseHelper.AddUpdateServerDetails(serverDetails);

            ResetControls();

            if (ExchangeToSitefinityClickedHiddenField.Value.Equals("true"))
            {
                ExchangeToSitefinitySync.Visible = true;
                ExchangeToSitefinityClickedHiddenField.Value = "false";
            }
            else if (SitefinityToExchangeClickedHiddenField.Value.Equals("true"))
            {
                SitefinityToExchangeSync.Visible = true;
                SitefinityToExchangeClickedHiddenField.Value = "false";
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        #region Exchange To Sitefinity

        public void ExchangeToSitefinitySyncResetControls()
        {
            ProcessSummaryDiv.Visible = ExchangeToSitefinity_MainDiv.Visible = NoRowSelectedErrorDiv.Visible = false;
            Into_Div.Visible = true;
        }

        protected void ExchangeToSitefinitySyncButton_Click(object sender, EventArgs e)
        {
            NoRowSelectedErrorDiv.Visible = false;

            if (ViewState["ExchangeDetails"] != null)
            {
                List<string> alreadyExistingList = new List<string>();

                List<string> contactsList = new List<string>();

                foreach (GridViewRow row in ExchangeContactsGridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("SelectedCheckBox") as CheckBox);
                        if (chkRow.Checked)
                        {
                            string email = ExchangeContactsGridView.DataKeys[row.RowIndex].Value.ToString();
                            contactsList.Add(email);
                        }
                    }
                }

                if (contactsList.Count() > 0)
                {
                    Aspose_ExchangeSync_ServerDetails serverDetails = (Aspose_ExchangeSync_ServerDetails)ViewState["ExchangeDetails"];

                    NetworkCredential credentials = new NetworkCredential(serverDetails.Username, serverDetails.Password, serverDetails.Domain);
                    IEWSClient client = EWSClient.GetEWSClient(serverDetails.ServerURL, credentials);

                    MapiContact[] contacts = client.ListContacts(client.MailboxInfo.ContactsUri);

                    foreach (MapiContact contact in contacts)
                    {
                        if (contactsList.Contains(contact.ElectronicAddresses.Email1.EmailAddress))
                        {
                            var userMan = UserManager.GetManager();
                            User user = userMan.GetUserByEmail(contact.ElectronicAddresses.Email1.EmailAddress);
                            if (user == null)
                                CreateSitefinityUser(contact);
                            else
                                alreadyExistingList.Add(contact.ElectronicAddresses.Email1.EmailAddress);
                        }
                    }
                }
                else
                {
                    NoRowSelectedErrorDiv.Visible = true;
                    return;
                }

                ImportedLiteral.Text = string.Format("{0} contact(s) have been imported to Sitefinity successfully.", (contactsList.Count - alreadyExistingList.Count));

                if (alreadyExistingList.Count > 0)
                {
                    AlreadyExistingLiteral.Text = "The following contacts already exists in Sitefinity and therefore not imported";
                    foreach (string email in alreadyExistingList)
                        AlreadyExistingLiteral.Text += "<br>" + email;
                }

                ProcessSummaryDiv.Visible = true;
                Into_Div.Visible = false;
                ExchangeToSitefinity_MainDiv.Visible = false;
            }
        }

        private void CreateSitefinityUser(MapiContact contact)
        {
            string email = GetUserEmailAddress(contact);
            string firstName = contact.NameInfo.GivenName; string lastName = contact.NameInfo.Surname;

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(contact.NameInfo.DisplayName))
            {
                if (contact.NameInfo.DisplayName.Contains(" "))
                {
                    string[] names = contact.NameInfo.DisplayName.Split(' ');
                    if (names != null && names.Count() > 1)
                    {
                        firstName = names[0];
                        lastName = names[1];
                    }
                }
            }

            CreateUser(email, email, firstName, lastName, email, string.Empty, string.Empty, true);
        }

        public void AddUserToRoles(string userName, List<string> rolesToAdd)
        {
            UserManager userManager = UserManager.GetManager();
            RoleManager roleManager = RoleManager.GetManager(SecurityManager.ApplicationRolesProviderName);
            roleManager.Provider.SuppressSecurityChecks = true;

            if (userManager.UserExists(userName))
            {
                User user = userManager.GetUser(userName);

                foreach (var roleName in rolesToAdd)
                {
                    if (roleManager.RoleExists(roleName))
                    {
                        Role role = roleManager.GetRole(roleName);
                        roleManager.AddUserToRole(user, role);
                    }
                }
            }

            roleManager.SaveChanges();
            roleManager.Provider.SuppressSecurityChecks = false;
        }

        public MembershipCreateStatus CreateUser(string username, string password, string firstName, string lastName, string mail, string secretQuestion, string secretAnswer, bool isApproved)
        {
            UserManager userManager = UserManager.GetManager();
            UserProfileManager profileManager = UserProfileManager.GetManager();

            MembershipCreateStatus status;

            User user = userManager.CreateUser(username, password, mail, secretQuestion, secretAnswer, isApproved, null, out status);

            if (status == MembershipCreateStatus.Success)
            {
                SitefinityProfile sfProfile = profileManager.CreateProfile(user, Guid.NewGuid(), typeof(SitefinityProfile)) as SitefinityProfile;

                if (sfProfile != null)
                {
                    sfProfile.FirstName = firstName;
                    sfProfile.LastName = lastName;
                }

                userManager.SaveChanges();
                profileManager.RecompileItemUrls(sfProfile);
                profileManager.SaveChanges();
            }

            List<string> rolesList = GetSelectedRoles();

            RoleManager roleManager = RoleManager.GetManager(SecurityManager.ApplicationRolesProviderName);
            roleManager.Provider.SuppressSecurityChecks = true;

            foreach (string roleID in rolesList)
            {
                roleManager.AddUserToRole(user, roleManager.GetRole(new Guid(roleID)));
            }

            roleManager.SaveChanges();
            roleManager.Provider.SuppressSecurityChecks = false;

            return status;
        }

        private string GetUserEmailAddress(MapiContact contact)
        {
            string emailAddress = string.Empty;

            emailAddress = contact.ElectronicAddresses.Email1.EmailAddress;
            if (!string.IsNullOrEmpty(emailAddress)) return emailAddress;

            emailAddress = contact.ElectronicAddresses.Email2.EmailAddress;
            if (!string.IsNullOrEmpty(emailAddress)) return emailAddress;

            emailAddress = contact.ElectronicAddresses.Email3.EmailAddress;
            if (!string.IsNullOrEmpty(emailAddress)) return emailAddress;

            return emailAddress;
        }

        private List<string> GetSelectedRoles()
        {
            List<string> rolesList = new List<string>();

            foreach (GridViewRow row in RolesGridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("SelectedCheckBox") as CheckBox);
                    if (chkRow.Checked)
                    {
                        string roleID = RolesGridView.DataKeys[row.RowIndex].Value.ToString();
                        rolesList.Add(roleID);
                    }
                }
            }

            return rolesList;
        }

        private void RenderRoles()
        {
            RoleManager roleManager = RoleManager.GetManager();
            List<Role> roles = roleManager.GetRoles().ToList();
            RolesGridView.DataSource = roles;
            RolesGridView.DataBind();
        }

        protected void GetExchangeContactsButton_Click(object sender, EventArgs e)
        {
            Into_Div.Visible = false;
            ExchangeToSitefinity_MainDiv.Visible = true;

            RenderRoles();

            if (ViewState["ExchangeDetails"] != null)
            {
                Aspose_ExchangeSync_ServerDetails serverDetails = (Aspose_ExchangeSync_ServerDetails)ViewState["ExchangeDetails"];

                NetworkCredential credentials = new NetworkCredential(serverDetails.Username, serverDetails.Password, serverDetails.Domain);
                IEWSClient client = EWSClient.GetEWSClient(serverDetails.ServerURL, credentials);

                MapiContact[] contacts = client.ListContacts(client.MailboxInfo.ContactsUri);

                List<ExchangeContact> exchangeContactsList = new System.Collections.Generic.List<ExchangeContact>();

                foreach (MapiContact contact in contacts)
                {
                    exchangeContactsList.Add(new ExchangeContact(contact.NameInfo.DisplayName, contact.ElectronicAddresses.Email1.EmailAddress));
                }

                ExchangeContactsGridView.DataSource = exchangeContactsList;
                ExchangeContactsGridView.DataBind();
            }
        }

        #endregion Exchange To Sitefinity

        #region Sitefinity To Exchange

        public void SitefinityToExchangeSyncResetControls()
        {
            ProcessSummaryDiv.Visible = SitefinityToExchange_MainDiv.Visible = NoRowSelectedErrorDiv.Visible = false;
            Into_Div.Visible = true;
        }

        protected void SitefinityToExchangeSyncButton_Click(object sender, EventArgs e)
        {
            NoRowSelectedErrorDiv.Visible = false;

            if (ViewState["ExchangeDetails"] != null)
            {
                List<string> alreadyExistingList = new System.Collections.Generic.List<string>();

                List<User> selectedUsersList = new List<User>();

                foreach (GridViewRow row in SitefinityUsersGridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("SelectedCheckBox") as CheckBox);
                        if (chkRow.Checked)
                        {
                            string email = SitefinityUsersGridView.DataKeys[row.RowIndex].Value.ToString();
                            var userMan = UserManager.GetManager();
                            User user = userMan.GetUserByEmail(email);
                            selectedUsersList.Add(user);
                        }
                    }
                }

                if (selectedUsersList.Count > 0)
                {
                    Aspose_ExchangeSync_ServerDetails serverDetails = (Aspose_ExchangeSync_ServerDetails)ViewState["ExchangeDetails"];
                    NetworkCredential credentials = new NetworkCredential(serverDetails.Username, serverDetails.Password, serverDetails.Domain);
                    IEWSClient client = EWSClient.GetEWSClient(serverDetails.ServerURL, credentials);

                    MapiContact[] contacts = client.ListContacts(client.MailboxInfo.ContactsUri);

                    foreach (User user in selectedUsersList)
                    {
                        if (contacts.FirstOrDefault(x => x.ElectronicAddresses.Email1.EmailAddress.Equals(user.Email)) == null)
                        {
                            MapiContact contact = BuildNewExchangeContact(user);
                            client.CreateContact(contact);
                        }
                        else
                        {
                            alreadyExistingList.Add(user.Email);
                        }
                    }
                }
                else
                {
                    NoRowSelectedErrorDiv.Visible = true;
                    return;
                }

                SitefinityToExchange_ImportedLiteral.Text = string.Format("{0} contact(s) have been imported to Exchange Server successfully.", (selectedUsersList.Count - alreadyExistingList.Count));

                if (alreadyExistingList.Count > 0)
                {
                    SitefinityToExchange_AlreadyExistingLiteral.Text = "The following contacts already exists in Exchange Server and therefore not imported";
                    foreach (string email in alreadyExistingList)
                        SitefinityToExchange_AlreadyExistingLiteral.Text += "<br>" + email;
                }

                SitefinityToExchange_ProcessSummaryDiv.Visible = true;
                Into_Div.Visible = false;
                SitefinityToExchange_MainDiv.Visible = false;
            }
        }

        public SitefinityProfile GetUserProfileByUserId(Guid userId)
        {
            UserProfileManager profileManager = UserProfileManager.GetManager();
            UserManager userManager = UserManager.GetManager();

            User user = userManager.GetUser(userId);

            SitefinityProfile profile = null;

            if (user != null)
            {
                profile = profileManager.GetUserProfile<SitefinityProfile>(user);
            }

            return profile;
        } 

        private MapiContact BuildNewExchangeContact(User user)
        {
            MapiContact contact = new MapiContact();
            contact.ElectronicAddresses.Email1.EmailAddress = user.Email;            
            SitefinityProfile sitefinityProfile = GetUserProfileByUserId(user.Id);
            contact.NameInfo.DisplayName = sitefinityProfile.FirstName + " " + sitefinityProfile.LastName;
            return contact;
        }

        protected void GetSitefinityUsersButton_Click(object sender, EventArgs e)
        {
            SitefinityUsersGridView.Visible = true;

            Into_Div.Visible = false;
            SitefinityToExchange_MainDiv.Visible = true;

            if (ViewState["ExchangeDetails"] != null)
            {
                UserManager userManager = UserManager.GetManager();
                List<User> users = userManager.GetUsers().ToList();
                SitefinityUsersGridView.DataSource = users;
                SitefinityUsersGridView.DataBind();
            }
        }

        #endregion Sitefinity To Exchange

    }
}