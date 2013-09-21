//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web;
using Oxite.Model;
using Oxite.Model.Extensions;

namespace Oxite.Mvc.Extensions
{
    public static class HttpCookieCollectionExtensions
    {
        private const string anonymousUserCookieName = "anon";

        public static void ClearAnonymousUser(this HttpCookieCollection cookies)
        {
            cookies.Add(new HttpCookie(anonymousUserCookieName) { Expires = DateTime.Now.AddDays(-1) });
        }

        public static UserBase GetAnonymousUser(this HttpCookieCollection cookies)
        {
            UserBase user = null;

            HttpCookie cookie = cookies[anonymousUserCookieName];
            if (cookie != null)
            {
                user = user.FillFromSerlializedString(cookie.Value);
            }

            return user;
        }

        public static void SetAnonymousUser(this HttpCookieCollection cookies, UserBase user)
        {
            HttpCookie cookie = new HttpCookie(anonymousUserCookieName, user.ToJson());
            cookie.Expires = DateTime.Now.AddDays(14);

            cookies.Add(cookie);
        }
    }
}
