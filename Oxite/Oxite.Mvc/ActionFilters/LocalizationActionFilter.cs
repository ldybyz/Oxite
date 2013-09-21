//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Oxite.Services;
using Oxite.Mvc.ViewModels;

namespace Oxite.Mvc.ActionFilters
{
    public class LocalizationActionFilter : IActionFilter
    {
        private readonly ILocalizationService locService;
        public LocalizationActionFilter(ILocalizationService locService)
        {
            this.locService = locService;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewResult result = filterContext.Result as ViewResult;
            if (result != null)
            {
                OxiteModel model = result.ViewData.Model as OxiteModel;
                if (model != null)
                {
                    model.AddModelItem(locService.GetTranslations());
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }
}
