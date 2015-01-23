using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data.Entity;
using Aspose.GoogleSync.Data;

using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data.Configuration;

namespace Aspose.GoogleSync.Components
{
    public class DatabaseHelper
    {
        static GoogleSyncEntities GoogleSyncEntities;

        private static GoogleSyncEntities CurrentDBEntities
        {
            get
            {
                if (GoogleSyncEntities == null)
                {
                    var dataConfig = Config.Get<DataConfig>();

                    if (dataConfig.ConnectionStrings != null && dataConfig.ConnectionStrings.Count > 0)
                    {                       
                        string providerName = dataConfig.ConnectionStrings["Sitefinity"].ProviderName;
                        SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
                        sqlBuilder.ConnectionString = dataConfig.ConnectionStrings["Sitefinity"].ConnectionString;
                        EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                        entityBuilder.Provider = providerName;
                        entityBuilder.ProviderConnectionString = sqlBuilder.ToString();

                        entityBuilder.Metadata = "res://*/Data.GoogleSyncModel.csdl|res://*/Data.GoogleSyncModel.ssdl|res://*/Data.GoogleSyncModel.msl";
                        EntityConnection entityConnection = new EntityConnection(entityBuilder.ToString());

                        GoogleSyncEntities = new GoogleSyncEntities(entityBuilder.ToString());

                        // Check if table exists and create it if it does not
                        try
                        {
                            string sql = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Aspose_GoogleSync_ServerDetails' AND TABLE_TYPE = 'BASE TABLE' )
	                                        BEGIN
		                                        CREATE TABLE [dbo].[Aspose_GoogleSync_ServerDetails](
	                                                [ID] [int] IDENTITY(1,1) NOT NULL,
	                                                [UserID] [nvarchar](250) NULL,
	                                                [Username] [nvarchar](250) NULL,
	                                                [Email] [nvarchar](250) NULL,
	                                                [Password] [nvarchar](250) NULL,
	                                                [ClientID] [nvarchar](250) NULL,
	                                                [ClientSecret] [nvarchar](250) NULL,
	                                                [RefreshToken] [nvarchar](500) NULL,
                                                    CONSTRAINT [PK_Aspose_GoogleSync_ServerDetails] PRIMARY KEY CLUSTERED 
                                                (
	                                                [ID] ASC
                                                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                                ) ON [PRIMARY]
	                                        END";

                            using (SqlConnection objConnection = new SqlConnection(sqlBuilder.ConnectionString))
                            {
                                objConnection.Open();
                                var command = new SqlCommand(sql, objConnection);
                                command.ExecuteNonQuery();
                                objConnection.Close();
                            }
                        }
                        catch (Exception)
                        { }
                    }
                }
                return GoogleSyncEntities;
            }
        }

        private static string GoogleDetailsSessionName(string userId)
        {
            return "GoogleDetailsSession-" + userId.ToString();
        }

        public static Aspose_GoogleSync_ServerDetails CheckGoogleDetails(string userID)
        {
            Aspose_GoogleSync_ServerDetails detailsToReturn = null;
            try
            {
                Aspose_GoogleSync_ServerDetails serverDetails = CurrentDBEntities.Aspose_GoogleSync_ServerDetails.FirstOrDefault(x => x.UserID == userID);
                if (serverDetails != null)
                {
                    detailsToReturn = new Aspose_GoogleSync_ServerDetails();
                    detailsToReturn.UserID = serverDetails.UserID;
                    detailsToReturn.Username = serverDetails.Username;
                    detailsToReturn.Email = serverDetails.Email;
                    detailsToReturn.Password = Crypto.Decrypt(serverDetails.Password);
                    detailsToReturn.ClientID = Crypto.Decrypt(serverDetails.ClientID);
                    detailsToReturn.ClientSecret = Crypto.Decrypt(serverDetails.ClientSecret);
                    detailsToReturn.RefreshToken = Crypto.Decrypt(serverDetails.RefreshToken);
                }
            }
            catch (Exception)
            {
                // Fall back to session approach if database fails
                if (HttpContext.Current.Session[GoogleDetailsSessionName(userID)] != null)
                {
                    detailsToReturn = (Aspose_GoogleSync_ServerDetails)HttpContext.Current.Session[GoogleDetailsSessionName(userID)];
                }
            }
            return detailsToReturn;
        }

        public static void AddUpdateServerDetails(Aspose_GoogleSync_ServerDetails details)
        {
            try
            {
                Aspose_GoogleSync_ServerDetails serverDetails = CurrentDBEntities.Aspose_GoogleSync_ServerDetails.FirstOrDefault(x => x.UserID == details.UserID);

                if (serverDetails != null)
                {
                    serverDetails.UserID = details.UserID;
                    serverDetails.Username = details.Username;
                    serverDetails.Email = details.Email;
                    serverDetails.Password = details.Password;
                    serverDetails.ClientID = details.ClientID;
                    serverDetails.ClientSecret = details.ClientSecret;
                    serverDetails.RefreshToken = details.RefreshToken;
                    CurrentDBEntities.SaveChanges();
                }
                else
                {
                    CurrentDBEntities.AddToAspose_GoogleSync_ServerDetails(details);
                    CurrentDBEntities.SaveChanges();
                }
            }
            catch (Exception)
            {
                // Fall back to session approach if database fails
                details.Password = Crypto.Decrypt(details.Password);
                HttpContext.Current.Session[GoogleDetailsSessionName(details.UserID)] = details;
            }
        }
    }
}