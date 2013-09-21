//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;

namespace Oxite.Mvc.ActionFilters
{
    public class PageSizeActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext) { }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters.ContainsKey("pageSize"))
            {
                string dataFormat = filterContext.RouteData.Values["dataFormat"] as string;
                if (!string.IsNullOrEmpty(dataFormat) && 
                    (StringComparer.CurrentCultureIgnoreCase.Compare(dataFormat, "RSS") == 0 || StringComparer.CurrentCultureIgnoreCase.Compare(dataFormat, "Atom") == 0))
                    filterContext.ActionParameters["pageSize"] = 50;
                else
                    filterContext.ActionParameters["pageSize"] = 10;
            }
        }
    }
}
