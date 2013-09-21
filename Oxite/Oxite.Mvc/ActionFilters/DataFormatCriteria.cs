//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;

namespace Oxite.Mvc.ActionFilters
{
    public class DataFormatCriteria : IActionFilterCriteria
    {
        private readonly string format;
        public DataFormatCriteria(string format)
        {
            this.format = format;
        }

        public bool Match(ActionFilterRegistryContext context)
        {
            return string.Equals(format, context.ControllerContext.RouteData.Values["dataFormat"] as string, StringComparison.OrdinalIgnoreCase);
        }
    }
}
