//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using System.Web.Routing;
using Oxite.Mvc.Extensions;

namespace Oxite.Mvc.ActionFilters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly RouteCollection routes;
        public AuthorizationFilter(RouteCollection routes)
        {
            this.routes = routes;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                UrlHelper helper = new UrlHelper(filterContext.RequestContext, routes);
                filterContext.Result = new RedirectResult(helper.SignIn(filterContext.HttpContext.Request.Url.AbsolutePath));
            }
        }
    }
}
