using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.GoogleSync.Data;
using Aspose.GoogleSync.Components;
using Aspose.Email.Google;
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
using Aspose.Email.Exchange;
using Aspose.Email.Services.Google;

namespace Aspose.GoogleSync
{
    public partial class AsposeGoogleSync : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private bool GoogleSettingsExist
        {
            get
            {
                Aspose_GoogleSync_ServerDetails GoogleDetailsList = DatabaseHelper.CheckGoogleDetails(SitefinityAPIHelper.CurrentUserId);
                if (GoogleDetailsList != null)
                {
                    ViewState["GoogleDetails"] = GoogleDetailsList;
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
            GoogleToSitefinitySync.Visible = false;
            SitefinityToGoogleSync.Visible = false;
            GoogleSettings.Visible = false;
        }

        protected void GoogleToSitefinityHyperLink_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (GoogleSettingsExist)
            {
                GoogleToSitefinitySyncResetControls();
                GoogleToSitefinitySync.Visible = true;
            }
            else
            {
                GoogleSettings.Visible = true;
                GoogleToSitefinityClickedHiddenField.Value = "true";
            }
        }

        protected void SitefinityToGoogleHyperLink_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (GoogleSettingsExist)
            {
                SitefinityToGoogleSyncResetControls();
                SitefinityToGoogleSync.Visible = true;
            }
            else
            {
                GoogleSettings.Visible = true;
                SitefinityToGoogleClickedHiddenField.Value = "true";
            }
        }

        protected void GoogleSettingsHyperLink_Click(object sender, EventArgs e)
        {
            Aspose_GoogleSync_ServerDetails googleDetailsList = DatabaseHelper.CheckGoogleDetails(SitefinityAPIHelper.CurrentUserId);
            if (googleDetailsList != null)
            {
                EmailAddressTextBox.Text = googleDetailsList.Email;
                ClientIDTextBox.Text = googleDetailsList.ClientID;
                PasswordTextBox.Text = googleDetailsList.Password;
                ClientSecretTextBox.Text = googleDetailsList.ClientSecret.ToString();
            }

            ResetControls();
            GoogleSettings.Visible = true;
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            GoogleCredsErrorDiv.Visible = false;

            Aspose_GoogleSync_ServerDetails serverDetails = new Aspose_GoogleSync_ServerDetails();

            serverDetails.Email = EmailAddressTextBox.Text.Trim();

            if (serverDetails.Email.Contains("@"))
            {
                serverDetails.Username = serverDetails.Email.Split('@')[0];
            }

            serverDetails.Password = PasswordTextBox.Text.Trim();
            serverDetails.ClientID = ClientIDTextBox.Text.Trim();
            serverDetails.ClientSecret = ClientSecretTextBox.Text.Trim();
            serverDetails.UserID = SitefinityAPIHelper.CurrentUserId;

            try
            {
                string refresh_token = string.Empty;

                //Code segment - START
                //This segment of code is used to get the refresh_token. In general, you do not have to refresh refresh_token every time, you need to do it once, and then use it to retrieve access-token.
                //Thus, use it once to retrieve the refresh_token and then use the refresh_token value each time.
                string access_token; string token_type; int expires_in;
                GoogleTestUser user = new GoogleTestUser(serverDetails.Username, serverDetails.Email, serverDetails.Password, serverDetails.ClientID, serverDetails.ClientSecret);
                GoogleOAuthHelper.GetAccessToken(user, out access_token, out refresh_token, out token_type, out expires_in);
                serverDetails.RefreshToken = refresh_token;
                //Code segment - END

                using (IGmailClient client = Aspose.Email.Google.GmailClient.GetInstance(serverDetails.ClientID, serverDetails.ClientSecret, serverDetails.RefreshToken))
                {
                    FeedEntryCollection groups = client.FetchAllGroups();
                }

            }
            catch (Exception)
            {
                GoogleCredsErrorDiv.Visible = true;
                return;
            }

            serverDetails.Password = Crypto.Encrypt(serverDetails.Password);
            serverDetails.ClientID = Crypto.Encrypt(serverDetails.ClientID);
            serverDetails.ClientSecret = Crypto.Encrypt(serverDetails.ClientSecret);
            serverDetails.RefreshToken = Crypto.Encrypt(serverDetails.RefreshToken);

            DatabaseHelper.AddUpdateServerDetails(serverDetails);

            ResetControls();

            if (GoogleToSitefinityClickedHiddenField.Value.Equals("true"))
            {
                if (GoogleSettingsExist)
                {
                    GoogleToSitefinitySync.Visible = true;
                    GoogleToSitefinityClickedHiddenField.Value = "false";
                }
            }
            else if (SitefinityToGoogleClickedHiddenField.Value.Equals("true"))
            {
                SitefinityToGoogleSync.Visible = true;
                SitefinityToGoogleClickedHiddenField.Value = "false";
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        #region Google To Sitefinity

        public void GoogleToSitefinitySyncResetControls()
        {
            ProcessSummaryDiv.Visible = GoogleToSitefinity_MainDiv.Visible = NoRowSelectedErrorDiv.Visible = false;
            Into_Div.Visible = true;
        }

        protected void GoogleToSitefinitySyncButton_Click(object sender, EventArgs e)
        {
            NoRowSelectedErrorDiv.Visible = false;

            if (ViewState["GoogleDetails"] != null)
            {
                List<string> alreadyExistingList = new List<string>();

                List<string> contactsList = new List<string>();

                foreach (GridViewRow row in GoogleContactsGridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("SelectedCheckBox") as CheckBox);
                        if (chkRow.Checked)
                        {
                            string email = GoogleContactsGridView.DataKeys[row.RowIndex].Value.ToString();
                            contactsList.Add(email);
                        }
                    }
                }

                if (contactsList.Count() > 0)
                {
                    Aspose_GoogleSync_ServerDetails serverDetails = (Aspose_GoogleSync_ServerDetails)ViewState["GoogleDetails"];
                    List<GoogleContact> googleContactsList = new List<GoogleContact>();

                    using (IGmailClient client = Aspose.Email.Google.GmailClient.GetInstance(serverDetails.ClientID, serverDetails.ClientSecret, serverDetails.RefreshToken))
                    {
                        Contact[] contacts = client.GetAllContacts();

                        foreach (Contact contact in contacts)
                        {
                            if (contactsList.Contains(contact.EmailAddresses[0].Address))
                            {
                                var userMan = UserManager.GetManager();
                                User user = userMan.GetUserByEmail(contact.EmailAddresses[0].Address);
                                if (user == null)
                                    CreateSitefinityUser(contact);
                                else
                                    alreadyExistingList.Add(contact.EmailAddresses[0].Address);
                            }
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
                GoogleToSitefinity_MainDiv.Visible = false;
            }
        }

        private void CreateSitefinityUser(Contact contact)
        {
            string email = contact.EmailAddresses[0].Address;
            string firstName = contact.GivenName; string lastName = contact.Surname;
            
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName) && !string.IsNullOrEmpty(contact.DisplayName))
            {
                if (contact.DisplayName.Contains(" "))
                {
                    string[] names = contact.DisplayName.Split(' ');
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

        protected void GetGoogleContactsButton_Click(object sender, EventArgs e)
        {
            Into_Div.Visible = false;
            GoogleToSitefinity_MainDiv.Visible = true;

            RenderRoles();

            if (ViewState["GoogleDetails"] != null)
            {
                Aspose_GoogleSync_ServerDetails serverDetails = (Aspose_GoogleSync_ServerDetails)ViewState["GoogleDetails"];
                List<GoogleContact> googleContactsList = new List<GoogleContact>();

                using (IGmailClient client = Aspose.Email.Google.GmailClient.GetInstance(serverDetails.ClientID, serverDetails.ClientSecret, serverDetails.RefreshToken))
                {
                    Contact[] contacts = client.GetAllContacts();

                    foreach (Contact c in contacts)
                    {
                        if (c.EmailAddresses.Count > 0)
                            googleContactsList.Add(new GoogleContact(c.DisplayName, c.EmailAddresses[0].Address));
                    }

                    GoogleContactsGridView.DataSource = googleContactsList;
                    GoogleContactsGridView.DataBind();
                }
            }
        }

        #endregion Google To Sitefinity

        #region Sitefinity To Google

        public void SitefinityToGoogleSyncResetControls()
        {
            ProcessSummaryDiv.Visible = SitefinityToGoogle_MainDiv.Visible = NoRowSelectedErrorDiv.Visible = false;
            Into_Div.Visible = true;
        }

        protected void SitefinityToGoogleSyncButton_Click(object sender, EventArgs e)
        {
            NoRowSelectedErrorDiv.Visible = false;

            if (ViewState["GoogleDetails"] != null)
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
                    Aspose_GoogleSync_ServerDetails serverDetails = (Aspose_GoogleSync_ServerDetails)ViewState["GoogleDetails"];

                    using (IGmailClient client = Aspose.Email.Google.GmailClient.GetInstance(serverDetails.ClientID, serverDetails.ClientSecret, serverDetails.RefreshToken))
                    {
                        Contact[] contacts = client.GetAllContacts();
                        Contact[] validContacts = (from contactsList in contacts where contactsList.EmailAddresses.Count > 0 select contactsList).ToArray<Contact>();

                        foreach (User user in selectedUsersList)
                        {
                            if (validContacts.FirstOrDefault(x => x.EmailAddresses[0].Address.Equals(user.Email)) == null)
                            {
                                Contact contact = BuildNewGmailContact(user);
                                client.CreateContact(contact, serverDetails.Email);
                            }
                            else
                            {
                                alreadyExistingList.Add(user.Email);
                            }
                        }
                    }
                }
                else
                {
                    NoRowSelectedErrorDiv.Visible = true;
                    return;
                }

                SitefinityToGoogle_ImportedLiteral.Text = string.Format("{0} contact(s) have been imported to Google Server successfully.", (selectedUsersList.Count - alreadyExistingList.Count));

                if (alreadyExistingList.Count > 0)
                {
                    SitefinityToGoogle_AlreadyExistingLiteral.Text = "The following contacts already exists in Google Server and therefore not imported";
                    foreach (string email in alreadyExistingList)
                        SitefinityToGoogle_AlreadyExistingLiteral.Text += "<br>" + email;
                }

                SitefinityToGoogle_ProcessSummaryDiv.Visible = true;
                Into_Div.Visible = false;
                SitefinityToGoogle_MainDiv.Visible = false;
            }
        }

        private Contact BuildNewGmailContact(User user)
        {
            Contact contact = new Contact();
            EmailAddress ea = new EmailAddress();
            ea.Address = user.Email;
            contact.EmailAddresses.Work = ea;

            SitefinityProfile sitefinityProfile = GetUserProfileByUserId(user.Id);
            contact.DisplayName = sitefinityProfile.FirstName + " " + sitefinityProfile.LastName;
            return contact;
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

        private MapiContact BuildNewGoogleContact(User user)
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
            SitefinityToGoogle_MainDiv.Visible = true;

            if (ViewState["GoogleDetails"] != null)
            {
                UserManager userManager = UserManager.GetManager();
                List<User> users = userManager.GetUsers().ToList();
                SitefinityUsersGridView.DataSource = users;
                SitefinityUsersGridView.DataBind();
            }
        }

        #endregion Sitefinity To Google

    }
}