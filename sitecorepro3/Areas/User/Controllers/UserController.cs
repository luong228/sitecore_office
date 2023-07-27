using System;
using System.Web.Mvc;
using Sitecore.Security.Authentication;
using sitecorepro3.Project.sitecorepro3.Areas.User.Models;
using sitecorepro3.Project.sitecorepro3.Utilities;
using RegisterUser = sitecorepro3.Project.sitecorepro3.Areas.User.Models.RegisterUser;

namespace sitecorepro3.Project.sitecorepro3.Areas.User.Controllers
{
    public class UserController : Controller
    {
        // GET: User/User
        public ActionResult Index()
        {
            return View();
        }
        private bool isEmailRegister(string Email)

        {


            var authUtils = new AuthenticationUtility();

            string userName = authUtils.GetUserNameForSitePerConfiguration(Email);

            if (string.IsNullOrEmpty(userName))

            {

                return false;
            }
            return true;

        }
        private bool ProcessRegistrationUser(RegisterUser signUpData)
        {
            bool result = false;
            var userName = "";
            var roleName = "";
            var authUtils = new AuthenticationUtility();
            //Check if the site is configured to use default password or not. 
            //UseDefaultPassword == true means user is registered without a password on the form, default apssword in the config setting will be used
            string passwordToRegister = "";
            if (authUtils.IsSiteSharedLogin())
            {
                //getting default password from a common setting config file 
                passwordToRegister = "123123";
            }
            else
            {
                passwordToRegister = signUpData.Password;
            }
            if (!string.IsNullOrWhiteSpace(signUpData.Email))
            {
                roleName = authUtils.GetRoleName();
                if (!Sitecore.Security.Accounts.Role.Exists(roleName))
                {
                    System.Web.Security.Roles.CreateRole(roleName);
                    Sitecore.Diagnostics.Log.Info("Created Role:" + roleName, this);
                }
                var role = Sitecore.Security.Accounts.Role.FromName(roleName);
                Sitecore.Security.Accounts.User user = null;
                userName = authUtils.GetUserNameForSitePerConfiguration(signUpData.Email);
                if (!String.IsNullOrEmpty(userName))
                {
                    var isAuthenticated = System.Web.Security.Membership.ValidateUser(userName, passwordToRegister);
                    if (isAuthenticated)
                    {
                        var existingUser = Sitecore.Security.Accounts.User.FromName(userName, true);
                        if (!existingUser.IsInRole(roleName))
                        {
                            existingUser.Roles.Add(role);
                        }
                        result = true;
                        Sitecore.Diagnostics.Log.Info(" Web Registration FormSubmit - User-Exists. SiteName:" + Sitecore.Context.Site.Name + "| UserName:" + userName, this);
                    }
                }
                else
                {
                    //Create the user on primary domain
                    userName = authUtils.GetUserName(signUpData.Email, Sitecore.Context.Site.Domain.AccountPrefix);
                    //Create User
                    user = Sitecore.Security.Accounts.User.Create(userName, passwordToRegister);
                    Sitecore.Diagnostics.Log.Info(" Web Registration FormSubmit - SiteName:" + Sitecore.Context.Site.Name + "|UserName:" + userName + " Created for site.", this);
                    //Add User to the role
                    user.Roles.Add(role);
                    //Update profile of the user.
                    bool contactResult = UpdateProfile(signUpData, user, role);
                    if (contactResult)
                    {
                        result = true;
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Error(" Web Registration FormSubmit - UnabletoUpdateContact. Site Name:" + Sitecore.Context.Site.Name + "|Username:" + userName, this);
                        authUtils.DeleteUser(userName, roleName);
                        result = false;
                    }
                }
            }
            return result;
        }

        private bool UpdateProfile(RegisterUser registerData, Sitecore.Security.Accounts.User sitecoreUser,
            Sitecore.Security.Accounts.Role sitecoreRole)
        {
            bool retVal = false;
            try
            {
                var authUtils = new AuthenticationUtility();
                string profileId = Sitecore.Context.Site.Properties.Get(authUtils.Profile_Id);
                if (string.IsNullOrWhiteSpace(profileId))
                {
                    profileId = Constant.UserLoginProfileID;
                }

                if (string.IsNullOrWhiteSpace(profileId))
                {
                    Sitecore.Diagnostics.Log.Fatal("Profile Id missing in Constant File", this);
                    retVal = false;
                }

                sitecoreUser.Profile.ProfileItemId = profileId;
                sitecoreUser.Profile.Email = registerData.Email;
                sitecoreUser.Profile.SetCustomProperty("FirstName", registerData.Name);
                sitecoreUser.Profile.Save();
                sitecoreUser.Profile.Reload();
                retVal = true;
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]


        public ActionResult Register(RegisterUser signUpData)

        {

            bool userReg;

            try

            {

                //If email id registered to user profile or not

                bool emailRegister = isEmailRegister(signUpData.Email);

                if (!emailRegister)

                {

                    // Register and create sitecore user profile in core Db.

                    userReg = ProcessRegistrationUser(signUpData);

                    if (userReg)

                    {

                        // maintain a session if user is registered and give timeout 2 minutes ( you can increase as per  need)

                        Session["ValidUser"] = signUpData.Email;

                        Session.Timeout = 2;

                    }

                }

                else

                {

                    Session["AlreadyUser"] = signUpData.Email;

                }

            }

            catch (Exception ex)

            {

                Sitecore.Diagnostics.Log.Info(" SignUpFormSumbit - error: " + Sitecore.Context.Site.Name + ex.Message.ToString(), this);

            }

            string url = Request.UrlReferrer.Scheme + "://" + Request.UrlReferrer.Authority + "/profile";

            return Redirect(url);

        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginUser user)
        {
            try
            {
                if (user != null && !string.IsNullOrEmpty(user.Username) && !string.IsNullOrEmpty(user.Password))
                {
                    bool isAuthenticated = AuthenticationManager.Login(user.Username, user.Password);
                    if (isAuthenticated)
                    {
                        string url = Request.UrlReferrer.Scheme +"://" + Request.UrlReferrer.Authority + "/profile";
                        return Redirect(url);
                    }

                }
                TempData["error"] = "Login fail due to your username or password is invalid";
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Info(" LoginFormSumbit - error: " + Sitecore.Context.Site.Name + e.Message.ToString(), this);
            }
            return View();
        }

        public ActionResult Profile()
        {
            Sitecore.Security.Accounts.User user = Sitecore.Context.User;
            string userName = user.Name;
            string userEmail = user.Profile.Email;
            string userLanguage = Sitecore.Context.Language.Name;

            // Truyền thông tin đến view
            ViewBag.UserName = userName;
            ViewBag.UserEmail = userEmail;
            ViewBag.UserLanguage = userLanguage;
            return View();
        }
    }
   
}