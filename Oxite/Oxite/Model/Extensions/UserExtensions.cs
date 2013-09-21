//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Routing;
using Oxite.Routing;

namespace Oxite.Model.Extensions
{
    public static class UserExtensions
    {
        public static string GetAdminUrl(this User user, RequestContext context, RouteCollection routes)
        {
            return routes.GetUrl(context, "AdminUsersEdit", new { userID = user.ID });
        }

        public static string GetAdminChangePasswordUrl(this User user, RequestContext context, RouteCollection routes)
        {
            return routes.GetUrl(context, "AdminUsersChangePassword", new { userID = user.ID });
        }
    }
}
