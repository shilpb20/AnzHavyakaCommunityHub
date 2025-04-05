using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityHub.Core.Models
{
    public class UserContact
    {
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string ContactNumber { get; set; }

        public UserContact(string email, string countryCode, string contactNumber)
        {
            Email = email;
            CountryCode = countryCode;
            ContactNumber = contactNumber;
        }
    }
}
