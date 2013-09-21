//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;

namespace Oxite.Mvc.Results
{
    public class NotFoundResult : ViewResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            ViewData = context.Controller.ViewData;
            TempData = context.Controller.TempData;
            ViewName = "NotFound";

            base.ExecuteResult(context);

            context.HttpContext.Response.StatusDescription = "File Not Found";
            context.HttpContext.Response.StatusCode = 404;
        }
    }
}