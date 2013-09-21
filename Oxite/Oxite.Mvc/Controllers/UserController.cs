//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Globalization;
using System.Web.Mvc;
using Oxite.Infrastructure;
using Oxite.Model;
using Oxite.Mvc.Extensions;
using Oxite.Mvc.ViewModels;
using Oxite.Services;
using Oxite.Validation;

namespace Oxite.Mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IFormsAuthentication formsAuth;
        private readonly IUserService userService;

        public UserController(IFormsAuthentication formsAuth, IUserService userService)
        {
            this.formsAuth = formsAuth;
            this.userService = userService;
        }

        public virtual OxiteModel SignIn()
        {
            return new OxiteModel { Container = new SignInPageContainer() };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public object SignIn(string username, string password, bool rememberMe, string returnUrl)
        {
            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("username", "You must specify a username.");
            }

            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", "You must specify a password.");
            }

            if (ViewData.ModelState.IsValid)
            {
                User user = userService.GetUser(username, password);

                if (user != null)
                {
                    formsAuth.SetAuthCookie(user.Name, rememberMe);

                    if (!string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith("/"))
                    {
                        return Redirect(returnUrl);
                    }

                    return Redirect(Url.AppPath(Url.Home()));
                }
                
                ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");
            }

            return SignIn();
        }

        public virtual ActionResult SignOut()
        {
            formsAuth.SignOut();

            return Redirect(Url.AppPath(Url.Posts()));
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual OxiteModelItem<User> ChangePassword(User currentUser)
        {
            return new OxiteModelItem<User>();
        }

        [ActionName("ChangePassword"), AcceptVerbs(HttpVerbs.Post)]
        public virtual object ChangePasswordSave(User currentUser, FormCollection form)
        {
            string password = form["userPassword"];
            string confirmPassword = form["userPasswordConfirm"];
            ValidationStateDictionary validationState;

            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("userPassword", "New Password is not set.");
            }
            if (string.IsNullOrEmpty(confirmPassword))
            {
                ModelState.AddModelError("userPasswordConfirm", "New Password (Confirm) is not set.");
            }

            if (ModelState.IsValid)
            {
                if (password != confirmPassword)
                {
                    ModelState.AddModelError("userPasswordConfirm", "New Password (Confirm) does not match New Password");
                }
            }

            if (ModelState.IsValid)
            {
                currentUser.Password = password;

                userService.EditUser(currentUser, out validationState);

                if (!validationState.IsValid)
                {
                    ModelState.AddModelErrors(validationState);
                }
            }

            if (!ModelState.IsValid)
            {
                ModelState.SetModelValue("userPassword", new ValueProviderResult(password, password, CultureInfo.CurrentCulture));
                ModelState.SetModelValue("userPasswordConfirm", new ValueProviderResult(confirmPassword, confirmPassword, CultureInfo.CurrentCulture));
                return ChangePassword(currentUser);
            }

            return Redirect(Url.AppPath(Url.ManageUsers()));
        }
    }
}
