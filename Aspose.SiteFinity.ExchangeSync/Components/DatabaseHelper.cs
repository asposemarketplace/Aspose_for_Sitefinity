using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data.Entity;
using Aspose.ExchangeSync.Data;

using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data.Configuration;

namespace Aspose.ExchangeSync.Components
{
    public class DatabaseHelper
    {
        static ExchangeSyncEntities exchangeSyncEntities;

        private static ExchangeSyncEntities CurrentDBEntities
        {
            get
            {
                if (exchangeSyncEntities == null)
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

                        entityBuilder.Metadata = "res://*/Data.ExchangeSyncModel.csdl|res://*/Data.ExchangeSyncModel.ssdl|res://*/Data.ExchangeSyncModel.msl";
                        EntityConnection entityConnection = new EntityConnection(entityBuilder.ToString());

                        exchangeSyncEntities = new ExchangeSyncEntities(entityBuilder.ToString());

                        // Check if table exists and create it if it does not
                        try
                        {
                            string sql = @"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Aspose_ExchangeSync_ServerDetails' AND TABLE_TYPE = 'BASE TABLE' )
	                                        BEGIN
		                                        CREATE TABLE [Aspose_ExchangeSync_ServerDetails](
			                                        [ID] [int] IDENTITY(1,1) NOT NULL,
			                                        [UserID] [nvarchar](250) NULL,
			                                        [ServerURL] [nvarchar](250) NULL,
			                                        [Username] [nvarchar](250) NULL,
			                                        [Password] [nvarchar](250) NULL,
			                                        [Domain] [nvarchar](250) NULL,
		                                            CONSTRAINT [PK_Aspose_ExchangeSync_ServerDetails] PRIMARY KEY CLUSTERED 
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
                return exchangeSyncEntities;
            }
        }

        private static string ExchangeDetailsSessionName(string userId)
        {
            return "ExchangeDetailsSession-" + userId.ToString();
        }

        public static Aspose_ExchangeSync_ServerDetails CheckExchangeDetails(string userID)
        {
            Aspose_ExchangeSync_ServerDetails detailsToReturn = null;
            try
            {
                Aspose_ExchangeSync_ServerDetails serverDetails = CurrentDBEntities.Aspose_ExchangeSync_ServerDetails.FirstOrDefault(x => x.UserID == userID);
                if (serverDetails != null)
                {
                    detailsToReturn = new Aspose_ExchangeSync_ServerDetails();
                    detailsToReturn.UserID = serverDetails.UserID;
                    detailsToReturn.ServerURL = serverDetails.ServerURL;
                    detailsToReturn.Username = serverDetails.Username;
                    detailsToReturn.Password = Crypto.Decrypt(serverDetails.Password);
                    detailsToReturn.Domain = serverDetails.Domain;
                }
            }
            catch (Exception)
            {
                // Fall back to session approach if database fails
                if (HttpContext.Current.Session[ExchangeDetailsSessionName(userID)] != null)
                {
                    detailsToReturn = (Aspose_ExchangeSync_ServerDetails)HttpContext.Current.Session[ExchangeDetailsSessionName(userID)];
                }
            }
            return detailsToReturn;
        }

        public static void AddUpdateServerDetails(Aspose_ExchangeSync_ServerDetails details)
        {
            try
            {
                Aspose_ExchangeSync_ServerDetails serverDetails = CurrentDBEntities.Aspose_ExchangeSync_ServerDetails.FirstOrDefault(x => x.UserID == details.UserID);

                if (serverDetails != null)
                {
                    serverDetails.UserID = details.UserID;
                    serverDetails.ServerURL = details.ServerURL;
                    serverDetails.Username = details.Username;
                    serverDetails.Password = details.Password;
                    serverDetails.Domain = details.Domain;
                    CurrentDBEntities.SaveChanges();
                }
                else
                {
                    CurrentDBEntities.AddToAspose_ExchangeSync_ServerDetails(details);
                    CurrentDBEntities.SaveChanges();
                }
            }
            catch (Exception)
            {
                // Fall back to session approach if database fails
                details.Password = Crypto.Decrypt(details.Password);
                HttpContext.Current.Session[ExchangeDetailsSessionName(details.UserID)] = details;
            }
        }
    }
}