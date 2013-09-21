//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web;
using System.Web.Routing;

namespace Oxite.Routing
{
    public class IsLong : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection) {
            long tryLong;
            return long.TryParse(values[parameterName].ToString(), out tryLong);
        }
    }
}