//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Oxite.Mvc.Results;

namespace Oxite.Mvc.ActionFilters
{
    public class AtomResultActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Result = new FeedResult("Atom", false);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }
}
