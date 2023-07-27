//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace sitecorepro3.Content
//{
//    using System.Web.Profile;

//    public class ProfileCommon : ProfileBase
//    {
//        // Define properties for profile data
//        public string FullName
//        {
//            get { return (string)GetPropertyValue("FullName"); }
//            set { SetPropertyValue("FullName", value); }
//        }

//        public string Email
//        {
//            get { return (string)GetPropertyValue("Email"); }
//            set { SetPropertyValue("Email", value); }
//        }

//        public string Address
//        {
//            get { return (string)GetPropertyValue("Address"); }
//            set { SetPropertyValue("Address", value); }
//        }

//        // Define a method to create a new profile for a user
//        public static ProfileCommon Create(string username)
//        {
//            return (ProfileCommon)Create(username, true);
//        }
//    }
//}