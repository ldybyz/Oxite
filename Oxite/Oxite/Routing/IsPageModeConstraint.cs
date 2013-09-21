//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web;
using System.Web.Routing;

namespace Oxite.Routing
{
    public class IsPageMode : IRouteConstraint
    {
        private readonly PageMode mode;

        public IsPageMode(PageMode mode)
        {
            this.mode = mode;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values[parameterName] == null) return false;

            string pagePath = values[parameterName].ToString();

            if (mode == PageMode.Add && pagePath.EndsWith("/" + PageMode.Add, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            if (mode == PageMode.Edit && pagePath.EndsWith("/" + PageMode.Edit, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            if (mode == PageMode.Remove && pagePath.EndsWith("/" + PageMode.Remove, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
