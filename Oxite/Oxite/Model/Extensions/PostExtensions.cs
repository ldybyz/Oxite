//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Routing;
using Oxite.Routing;

namespace Oxite.Model.Extensions
{
    public static class PostExtensions
    {
        public static string GetUrl(this Post post, RequestContext context, RouteCollection routes)
        {
            return routes.GetUrl(context, "Post", new { areaName = post.Area != null ? post.Area.Name : "", slug = post.Slug, dataFormat = "" });
        }
    }
}
