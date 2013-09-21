using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Web.Mvc;
using Oxite.Mvc.Tests.Fakes;
using Oxite.Mvc.ActionFilters;
using System.Web.Routing;

namespace Oxite.Mvc.Tests.Filters
{
    public class AuthorizationFilterTests
    {
        [Fact]
        public void OnAuthorizationDoesNotTouchResultIfRequestIsAuthenticated()
        {
            AuthorizationContext context = new AuthorizationContext() { HttpContext = new FakeHttpContext("~/Secure") };
            (context.HttpContext.Request as FakeHttpRequest).RequestIsAuthenticated = true;

            AuthorizationFilter filter = new AuthorizationFilter(new RouteCollection());

            filter.OnAuthorization(context);

            Assert.Null(context.Result);
        }

        [Fact]
        public void OnAuthorizationRedirectsToSignInWithReturnUrlIfRequestIsNotAuthenticated()
        {
            AuthorizationContext context = new AuthorizationContext() { HttpContext = new FakeHttpContext(new Uri("http://foo.com/Secure"),"~/Secure") };

            RouteCollection routes = new RouteCollection();
            routes.Add("SignIn", new Route("SignIn", null));

            AuthorizationFilter filter = new AuthorizationFilter(routes);

            filter.OnAuthorization(context);

            RedirectResult result = context.Result as RedirectResult;
            Assert.NotNull(result);
            Assert.Equal("/SignIn?ReturnUrl=%2FSecure", result.Url);
        }
    }
}
