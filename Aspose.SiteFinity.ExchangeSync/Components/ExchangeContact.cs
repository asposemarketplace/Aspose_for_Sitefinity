using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aspose.ExchangeSync.Components
{
    public class ExchangeContact
    {
        public ExchangeContact() { }
        public ExchangeContact(string displayName, string email) { DisplayName = displayName; Email = email; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}