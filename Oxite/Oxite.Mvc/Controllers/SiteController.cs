//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;
using Oxite.Infrastructure;
using Oxite.Model;
using Oxite.Mvc.Extensions;
using Oxite.Mvc.ViewModels;
using Oxite.Services;
using Oxite.Validation;
using Microsoft.Practices.Unity;

namespace Oxite.Mvc.Controllers
{
    public class SiteController : Controller
    {
        private readonly AppSettingsHelper appSettings;
        private readonly ISiteService siteService;
        private readonly IUserService userService;
        private readonly IAreaService areaService;
        private readonly ILanguageService languageService;

        public SiteController(AppSettingsHelper appSettings, ISiteService siteService, IUserService userService, IAreaService areaService, ILanguageService languageService)
        {
            this.appSettings = appSettings;
            this.siteService = siteService;
            this.userService = userService;
            this.areaService = areaService;
            this.languageService = languageService;
        }

        public virtual OxiteModel Dashboard()
        {
            return new OxiteModel { Container = new AdminDashboardPageContainer() };
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual OxiteModelItem<Site> Item()
        {
            string siteName = appSettings.GetString("SiteName", "Oxite");
            Site site = siteService.GetSite(siteName)
                ?? new Site
                {
                    Name = siteName,
                    DisplayName = "My Oxite Site",
                    Description = "",
                    PageTitleSeparator = " - ",
                    TimeZoneOffset = -8,
                    ScriptsPath = "~/Skins/{0}/Scripts",
                    CssPath = "~/Skins/{0}/Styles",
                    CommentStateDefault = EntityState.PendingApproval.ToString(),
                    AuthorAutoSubscribe = true,
                    RouteUrlPrefix = "",
                    IncludeOpenSearch = true,
                    LanguageDefault = "en",
                    GravatarDefault = "http://mschnlnine.vo.llnwd.net/d1/oxite/gravatar.jpg",
                    PostEditTimeout = 24,
                    SkinDefault = "Default",
                    FavIconUrl = "~/Content/icons/flame.ico",
                    HasMultipleAreas = false,
                    CommentingDisabled = false
                };

            return new OxiteModelItem<Site> { Item = site };
        }

        [ActionName("Item"), AcceptVerbs(HttpVerbs.Post)]
        public virtual object SaveItem(Site siteInput, User userInput, FormCollection form)
        {
            ValidationStateDictionary validationState;

            if (siteInput.ID == Guid.Empty && userInput != null)
            {
                //TODO: (erikpo) This seems lame, but I need to do validation ahead of time.  Fix this.
                IUnityContainer container = (IUnityContainer)HttpContext.Application["container"];
                IValidator<Site> siteValidator = container.Resolve<IValidator<Site>>();
                IValidator<User> userValidator = container.Resolve<IValidator<User>>();

                validationState = new ValidationStateDictionary();

                validationState.Add(typeof(Site), siteValidator.Validate(siteInput));
                validationState.Add(typeof(User), userValidator.Validate(userInput));

                if (string.IsNullOrEmpty(form["userEmail"]))
                {
                    validationState[typeof(User)].Errors.Add("Email", form["userEmail"], "You must specify an Admin email address.");
                }

                if (string.IsNullOrEmpty(form["userPassword"]))
                {
                    validationState[typeof(User)].Errors.Add("Password", form["userPassword"], "You must specify an Admin password.");
                }

                if (string.IsNullOrEmpty(form["userPasswordConfirm"]))
                {
                    validationState[typeof(User)].Errors.Add("PasswordConfirm", form["userPasswordConfirm"], "You must confirm the Admin password.");
                }

                if (validationState.IsValid && form["userPassword"] != form["userPasswordConfirm"])
                {
                    validationState[typeof(User)].Errors.Add("PasswordMismatch", form["userPasswordConfirm"], "Admin passwords do not match.");
                }

                if (validationState.IsValid)
                {
                    Site site;

                    siteService.AddSite(siteInput, out validationState, out site);

                    Language language = new Language { Name = "en", DisplayName = "English" };
                    languageService.AddLanguage(language);

                    userService.EnsureAnonymousUser(language);

                    User user;

                    userInput.Status = (byte)EntityState.Normal;
                    userInput.LanguageDefault = language;

                    userService.AddUser(userInput, out validationState, out user);

                    Area area = new Area
                    {
                        CommentingDisabled = false,
                        Name = "Blog",
                        DisplayName = site.DisplayName,
                        Description = site.Description
                    };

                    areaService.AddArea(area, site, out validationState, out area);
                }
            }
            else
            {
                siteService.EditSite(siteInput, out validationState);
            }

            if (!validationState.IsValid)
            {
                ModelState.AddModelErrors(validationState);

                return Item();
            }

            ((OxiteApplication)HttpContext.ApplicationInstance).ReloadSite();

            return Redirect(Url.AppPath(Url.ManageSite()));
        }
    }
}
