//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Routing;
using Oxite.Routing;

namespace Oxite.Model.Extensions
{
    public static class AreaExtensions
    {
        public static string GetUrl(this Area area, RequestContext context, RouteCollection routes)
        {
            return routes.GetUrl(context, "PostsByArea", new { areaName = area.Name });
        }
    }
}
