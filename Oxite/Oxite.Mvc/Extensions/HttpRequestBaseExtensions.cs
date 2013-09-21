//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Net;
using System.Web;
using Oxite.Extensions;

namespace Oxite.Mvc.Extensions
{
    public static class HttpRequestBaseExtensions
    {
        public static IPAddress GetUserIPAddress(this HttpRequestBase request)
        {
            IPAddress address;

            if (!IPAddress.TryParse(request.UserHostAddress, out address))
            {
                address = null;
            }

            return address;
        }

        public static string GenerateAntiForgeryToken(this HttpRequestBase request, string key, string salt)
        {
            return (key + salt + request.UserAgent).ComputeHash();
        }
    }
}