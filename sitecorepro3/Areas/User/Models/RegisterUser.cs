using System.Collections.Generic;

namespace sitecorepro3.Project.sitecorepro3.Areas.User.Models
{
    public class RegisterUser
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string isEmailSend { get; set; }

        public List<string> SitesAccess { get; set; }

        public string URLReturn { get; set; }
    }
}