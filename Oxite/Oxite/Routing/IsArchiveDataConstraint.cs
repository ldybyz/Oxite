//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web;
using System.Web.Routing;
using Oxite.Model;

namespace Oxite.Routing
{
    public class IsArchiveData : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return values[parameterName] != null && (new ArchiveData(values[parameterName].ToString())).Year > 0;
        }
    }
}