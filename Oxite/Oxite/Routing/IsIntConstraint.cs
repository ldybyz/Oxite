//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web;
using System.Web.Routing;

namespace Oxite.Routing
{
    public class IsInt : IRouteConstraint
    {
        public IsInt() { }

        public IsInt(int minValue, int maxValue)
            : this()
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public IsInt(bool isOptional)
            : this()
        {
            IsOptional = isOptional;
        }

        public IsInt(int minValue, int maxValue, bool isOptional)
            : this(minValue, maxValue)
        {
            IsOptional = isOptional;
        }

        protected int MinValue { get; set; }
        protected int MaxValue { get; set; }
        protected bool IsOptional { get; set; }

        #region IRouteConstraint Members

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            int tryInt;

            return (
                       values[parameterName] != null
                       && int.TryParse(values[parameterName].ToString(), out tryInt)
                       && ((MinValue < 1 && MaxValue < 1) || (MinValue <= tryInt && tryInt <= MaxValue))
                   )
                   || (IsOptional && int.Parse(values[parameterName].ToString()) == 0);
        }

        #endregion
    }
}