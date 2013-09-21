//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web;
using System.Web.Routing;
using Oxite.Extensions;

namespace Oxite.Routing
{
    public class IsGuid : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            Guid result;
            return values[parameterName].ToString().GuidTryParse(out result);
        }
    }
}
