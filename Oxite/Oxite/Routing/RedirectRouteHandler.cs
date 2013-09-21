//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web;
using System.Web.Routing;
using Oxite.Handlers;

namespace Oxite.Routing
{
    public class RedirectRouteHandler : IRouteHandler
    {
        private readonly string url;

        public RedirectRouteHandler(string url)
        {
            this.url = url;
        }

        #region IRouteHandler Members

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            string targetUrl = url;

            if (requestContext.RouteData.Values.ContainsKey("path") &&
                requestContext.RouteData.Values.ContainsKey("rootPath"))
            {
                targetUrl = url + requestContext.RouteData.Values["rootPath"] + "/" +
                            requestContext.RouteData.Values["path"];
            }

            return new RedirectHttpHandler(targetUrl);
        }

        #endregion
    }
}