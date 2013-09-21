//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web;

namespace Oxite.Mvc.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GenerateSiteHost(this HttpRequest request)
        {
            return string.Format("{0}://{1}{2}{3}", request.Url.Scheme, request.Url.Host, request.Url.Port != 80 ? ":" + request.Url.Port : "", request.ApplicationPath.Length > 1 ? request.ApplicationPath.Substring(1) : "");
        }
    }
}
