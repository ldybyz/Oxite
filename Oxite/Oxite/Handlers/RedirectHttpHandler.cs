//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web;

namespace Oxite.Handlers
{
    public class RedirectHttpHandler : IHttpHandler
    {
        private readonly string url;

        public RedirectHttpHandler(string url)
        {
            this.url = url;
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        #endregion

        public void ProcessRequest(HttpContextBase context)
        {
            context.Response.Status = "301 Moved Permanently";
            context.Response.AddHeader("content-length", "0");
            context.Response.AddHeader("Location", url);
        }
    }
}