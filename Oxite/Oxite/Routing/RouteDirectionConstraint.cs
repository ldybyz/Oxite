//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Routing;

namespace Oxite.Routing
{
    class RouteDirectionConstraint : IRouteConstraint
    {
        private readonly RouteDirection routeDirection;

        public RouteDirectionConstraint(RouteDirection routeDirection)
        {
            this.routeDirection = routeDirection;
        }

        public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return this.routeDirection == routeDirection;
        }
    }
}
