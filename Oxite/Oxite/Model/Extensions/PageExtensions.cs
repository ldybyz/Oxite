//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Routing;
using Oxite.Routing;

namespace Oxite.Model.Extensions
{
    public static class PageExtensions
    {
        public static string GetUrl(this Page page, RequestContext context, RouteCollection routes)
        {
            return routes.GetUrl(context, "Page", new { pagePath = page.Path.Substring(1) });
        }
    }
}
