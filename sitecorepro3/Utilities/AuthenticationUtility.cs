using System;

namespace sitecorepro3.Project.sitecorepro3.Utilities
{
    public class AuthenticationUtility
    {
        public string Profile_Id = "profileId";
        public string Use_Default_Password = "useDefaultPassword";
        public string SharedLogin = "sharedLogin";
        public string SharedDomain = "sharedDomain";
        public string Extranet_Account_Prefix = "extranet\\";
        //public string Role_Name = @"{0}\User";
        public string Role_Name = @"training\User";


        public string GetRoleName()
        {
            //return string.Format(Role_Name, Sitecore.Context.Site.Name);
            return Role_Name;
        }

        public string GetUserName(string emailAddress, string domain)
        {
            //make sure there is a slash at the very end all the time
            if (!domain.EndsWith("\\"))
            {
                domain += "\\";
            }

            return string.Format("{0}{1}", domain, emailAddress);
        }

        /// <summary>
        /// This method also verifies if the User Exists or not
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public string GetUserNameForSitePerConfiguration(string emailAddress)
        {
            var retUserName = "";

            var siteDomain = Sitecore.Context.Site.Domain.AccountPrefix;
            if (!string.IsNullOrEmpty(emailAddress) && !string.IsNullOrEmpty(siteDomain))
            {
                var _userName = GetUserName(emailAddress, siteDomain);

                //If the domain name exists as per domain attribute in SiteDefinition us it
                if (Sitecore.Security.Accounts.User.Exists(_userName))
                {
                    retUserName = _userName;
                }
                else
                {
                    //Else if the site is configured to have Shared Login, then check if user exists in all configured domains
                    var sharedLoginBool = IsSiteSharedLogin();
                    if (sharedLoginBool)
                    {
                        var sharedDomain = Sitecore.Context.Site.Properties.Get(SharedDomain);
                        if (!string.IsNullOrEmpty(sharedDomain))
                        {
                            _userName = GetUserName(emailAddress, sharedDomain);//string.Format("{0}{1}", GetAccountPrefix(), emailAddress);
                            if (Sitecore.Security.Accounts.User.Exists(_userName))
                            {
                                retUserName = _userName;
                            }
                        }
                    }
                }

            }

            return retUserName;

        }


        public bool IsSiteSharedLogin()
        {
            var sharedLoginString = Sitecore.Context.Site.Properties.Get(SharedLogin);

            var sharedLoginBool = (!String.IsNullOrEmpty(sharedLoginString) && sharedLoginString.ToLower() == "true") ? true : false;

            return sharedLoginBool;
        }


        public bool IsDefaultPassword()
        {
            //Check if the site is configured to use default password or not for registration/login. 
            //UseDefaultPassword == true means user is registered without a password on the form, default apssword in the config setting will be used
            var isDefaultPassword = false;
            var passwordSetting = Sitecore.Context.Site.Properties.Get(Use_Default_Password);
            if (!string.IsNullOrWhiteSpace(passwordSetting))
            {
                isDefaultPassword = System.Convert.ToBoolean(passwordSetting);
            }

            return isDefaultPassword;
        }

        /// <summary>
        /// This method is used to revert user registration process in case of any errors
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userRoleName"></param>
        public void DeleteUser(string userName, string userRoleName)
        {
            try
            {
                if (!String.IsNullOrEmpty(userName) && Sitecore.Security.Accounts.User.Exists(userName))
                {
                    var user = Sitecore.Security.Accounts.User.FromName(userName, true);
                    //delete created User
                    user.Delete();

                    //Log this into Sitecore
                    Sitecore.Diagnostics.Log.Info("Utilities.AuthenticationUtils DeleteUser - SiteName:" + Sitecore.Context.Site.Name + "|UserName:" + userName + "|Role:" + userRoleName, this);


                }
            }
            catch (Exception ex)
            {
                //Log ex
                Sitecore.Diagnostics.Log.Error("Utilities.AuthenticationUtils DeleteUser - SiteName:" + Sitecore.Context.Site.Name + "|UserName:" + userName + "|Role:" + userRoleName + "|Exception:" + ex.Message, this);
            }
        }

        public bool ShouldIdentifyContact()
        {
            var shouldIdentifyContact = Sitecore.Configuration.Settings.GetSetting(" CommonSetting.Web.ShouldIdentifyContact ", " false");

            //SHouldIdentify COntact should be tru
            return (!String.IsNullOrEmpty(shouldIdentifyContact) && shouldIdentifyContact.ToLower().CompareTo("true") == 0);
        }
    }
}