using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Security.Claims;

namespace Aspose.ExchangeSync.Components
{
    public class SitefinityAPIHelper
    {
        public static string CurrentUserId
        {
            get
            {
                string currentUserGuid = ClaimsManager.GetCurrentIdentity().UserId.ToString();
                return currentUserGuid;
            }
        }
    }
}