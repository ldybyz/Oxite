//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Oxite.Model;
using Oxite.Mvc.Controllers;
using Oxite.Mvc.Tests.Fakes;
using Oxite.Mvc.ViewModels;
using Xunit;

namespace Oxite.Mvc.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public void SignInSetsContainerToSignIn()
        {
            UserController controller = new UserController(null, null);

            OxiteModel result = controller.SignIn();

            Assert.NotNull(result);
            Assert.IsType<SignInPageContainer>(result.Container);
        }

        [Fact]
        public void SignInSetsModelStateErrorOnNullUserName()
        {
            UserController controller = new UserController(null, null);

            controller.SignIn(null, null, false, null);

            Assert.False(controller.ModelState.IsValid);
            Assert.False(controller.ModelState.IsValidField("username"));
        }

        [Fact]
        public void SignInSetsModelStateErrorOnNullPassword()
        {
            UserController controller = new UserController(null, null);

            controller.SignIn(null, null, false, null);

            Assert.False(controller.ModelState.IsValid);
            Assert.False(controller.ModelState.IsValidField("password"));
        }

        [Fact]
        public void SignInSetsCookieForValidUser()
        {
            FakeUserService userService = new FakeUserService();
            FakeFormsAuthentication formsAuth = new FakeFormsAuthentication();

            RouteCollection routes = new RouteCollection();
            routes.Add("Posts", new Route("", new MvcRouteHandler()));

            UrlHelper helper = new UrlHelper(new RequestContext(new FakeHttpContext(new Uri("http://oxite.net/"), "~/"), new RouteData()), routes);

            UserController controller = new UserController(formsAuth, userService) { Url = helper };

            controller.SignIn("test", "test", true, null);

            Assert.Equal("test", formsAuth.LastUserName);
            Assert.True(formsAuth.LastPersistCookie);
        }

        [Fact]
        public void SignInRedirectsToHomePageForNullReturnUrl()
        {
            FakeUserService userService = new FakeUserService();
            FakeFormsAuthentication formsAuth = new FakeFormsAuthentication();

            RouteCollection routes = new RouteCollection();
            routes.Add("Posts", new Route("", new MvcRouteHandler()));

            UrlHelper helper = new UrlHelper(new RequestContext(new FakeHttpContext(new Uri("http://oxite.net/"), "~/"), new RouteData()), routes);

            UserController controller = new UserController(formsAuth, userService) { Url = helper };

            ActionResult result = controller.SignIn("test", "test", true, null) as ActionResult;

            Assert.IsType<RedirectResult>(result);
            RedirectResult redirectResult = result as RedirectResult;
            Assert.Equal("/", redirectResult.Url);
        }

        [Fact]
        public void SignInRedirectsToUrlForNonNullReturnUrl()
        {
            FakeUserService userService = new FakeUserService();
            FakeFormsAuthentication formsAuth = new FakeFormsAuthentication();

            UserController controller = new UserController(formsAuth, userService);

            ActionResult result = controller.SignIn("test", "test", true, "/test") as ActionResult;

            Assert.IsType<RedirectResult>(result);
            RedirectResult redirectResult = result as RedirectResult;
            Assert.Equal("/test", redirectResult.Url);
        }

        [Fact]
        public void SignInSetsModelStateErrorIfUserDoesNotExist()
        {
            FakeUserService userService = new FakeUserService();
            userService.Authenticate = false;

            UserController controller = new UserController(null, userService);

            controller.SignIn("test", "test", false, null);

            Assert.False(controller.ModelState.IsValid);
            Assert.False(controller.ModelState.IsValidField("_FORM"));
        }
    }
}
