//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Routing;

namespace Oxite.Routing
{
    public static class RouteCollectionExtensions
    {
        public static string GetUrl(this RouteCollection routes, RequestContext context, string route, object values)
        {
            VirtualPathData pathData = routes[route].GetVirtualPath(context, new RouteValueDictionary(values));
            string path = "";

            if (pathData != null)
            {
                path = pathData.VirtualPath;

                if (!path.StartsWith("/"))
                    path = "/" + path;

                string appPath = context.HttpContext.Request.ApplicationPath;

                if (!path.StartsWith(appPath))
                    path = appPath + path;
            }

            return path;
        }
    }
}