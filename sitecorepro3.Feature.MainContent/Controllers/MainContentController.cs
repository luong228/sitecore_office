using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.DependencyInjection;
using System.Web.Security;
using Sitecore.Security.Authentication;
using Sitecore.SecurityModel;
using Sitecore.Shell.Framework.Commands.UserManager;
using sitecorepro3.Feature.MainContent.Models;
using sitecorepro3.Feature.MainContent.Utilities;
using Sitecore.Diagnostics;
using System.Net.Mail;
using System.Net;
using AuthenticationManager = Sitecore.Security.Authentication.AuthenticationManager;
using Sitecore.Data;
using Sitecore.Links;
using System.Xml.Serialization;
using Sitecore;
using Sitecore.ContentTesting.Intelligence.Math;
using Sitecore.Data.Items;
using Sitecore.Services.Core.ComponentModel;
using System.Text.RegularExpressions;

namespace sitecorepro3.Feature.MainContent.Controllers
{
    public class MainContentController : Controller
    {
        // GET: MainContent

        public ActionResult LoginBox()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginBox(LoginUser user)
        {

            try
            {
                if (user != null && !string.IsNullOrEmpty(user.Username) && !string.IsNullOrEmpty(user.Password))
                {
                    bool isAuthenticated = AuthenticationManager.Login(user.Username, user.Password);
                    if (isAuthenticated)
                    {
                        string url = Request.UrlReferrer.Scheme + "://" + Request.UrlReferrer.Authority + "/office/news";
                        return Redirect(url);
                    }

                }
                TempData["error"] = "Login fail due to your username or password is invalid";
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Info(" LoginFormSumbit - error: " + Sitecore.Context.Site.Name + e.Message.ToString(), this);
            }
            string urlHome = Request.UrlReferrer.Scheme + "://" + Request.UrlReferrer.Authority + "/office";
            return Redirect(urlHome);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.Logout();
            return View();
        }

        public ActionResult ForgotAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotAccount(ForgotAccount inputAccount)
        {
            var authUtils = new AuthenticationUtility();
            var userName = authUtils.GetUserNameForSitePerConfiguration(inputAccount.Email);
            string urlForgot = Request.UrlReferrer.Scheme + "://" + Request.UrlReferrer.Authority + "/office/forgotaccount";

            if (!String.IsNullOrEmpty(userName))
            {
                var user = System.Web.Security.Membership.GetUser("training\\" + inputAccount.Email);
                var newPassword = Membership.GeneratePassword(12, 3);
                //Sitecore.Security.Accounts.User user = null;
                //user = Sitecore.Security.Accounts.User.FromName("training\\" + inputAccount.Email, true);
                using (new SecurityDisabler())
                {
                    var oldPassword = user.ResetPassword();
                    user.ChangePassword(oldPassword, newPassword);
                }
                string from = "anhluong059@gmail.com";
                string mailServer = "smtp.gmail.com";
                string subject = "Forgot account office";
                string message = $"Your new password for {inputAccount.Email} is {newPassword}";


                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(from);
                mailMessage.Subject = subject;
                mailMessage.Body = message;


                mailMessage.To.Add(new MailAddress(inputAccount.Email));


                SmtpClient client = new SmtpClient(mailServer);

                try
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("anhluong059@gmail.com", "bqtwyujuwqdfyzdv");
                    client.EnableSsl = true;
                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    Log.Error("EmailExAction Threw An Exception", ex, this);
                }
                mailMessage.Dispose();
                client.Dispose();




                TempData["error"] = "Recovery email sent successfully, please check your mailbox";
                
                return Redirect(urlForgot);
            }
            TempData["error"] = "Your email is not correct";
            return Redirect(urlForgot);
        }
        public ActionResult Register()
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
                sitecoreUser.Profile.FullName = registerData.Name;
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
                    string url1 = Request.UrlReferrer.Scheme + "://" + Request.UrlReferrer.Authority + "/office/register";

                    return Redirect(url1);

                }

            }

            catch (Exception ex)

            {

                Sitecore.Diagnostics.Log.Info(" SignUpFormSumbit - error: " + Sitecore.Context.Site.Name + ex.Message.ToString(), this);

            }

            string url = Request.UrlReferrer.Scheme + "://" + Request.UrlReferrer.Authority + "/office";

            return Redirect(url);

        }

        public ActionResult NotFoundPage()
        {
            return View();
        }

        public ActionResult RobotPage()
        {
            Item contextItem = Sitecore.Context.Item;

            var robot = new Robot()
            {
                Item = contextItem
            };
            return View(robot);
        }
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        public ActionResult Newsletter()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Newsletter(Newsletter newsletter)
        {
            string url = Request.UrlReferrer.Scheme + "://" + Request.UrlReferrer.Authority + "/office/newsletter";

          
    
            if (newsletter.Email == null || !IsValidEmail(newsletter.Email))
            {
                TempData["InvalidEmail"] = true;
                return Redirect(url);
            }
            Database database = Sitecore.Data.Database.GetDatabase("master");
            Item newsletterItem = database.GetItem(new ID("{8B63B108-4CCA-4286-8EC5-2C5C849110AA}"));
            if (newsletterItem != null)
            {
                var emailList = newsletterItem.Fields["Text"].Value.Split(';').ToList();
                // Check if the email already exists in the item
                bool emailExists = emailList.Any(x => String.Equals(x, newsletter.Email, StringComparison.InvariantCultureIgnoreCase));

                if (!emailExists)
                {
                    // Create a new child item to store the email
                    using (new SecurityDisabler())
                    {
                        emailList.Add(newsletter.Email);
                        var emailToWrite = string.Join(";", emailList);

                        // Set the field values for the new item
                        newsletterItem.Editing.BeginEdit();
                        newsletterItem["Text"] = emailToWrite;
                        newsletterItem.Editing.EndEdit();
                        TempData["Success"] = true;
                        return Redirect(url);
                    }
                }

                TempData["DuplicateEmail"] = true;
                return Redirect(url);
            }
           

            TempData["InvalidEmail"] = true;
           
            return Redirect(url);
        }

    }

    }