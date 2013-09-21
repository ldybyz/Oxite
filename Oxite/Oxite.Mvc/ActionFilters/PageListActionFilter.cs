//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Mvc.ViewModels;
using Oxite.Services;

namespace Oxite.Mvc.ActionFilters
{
    public class PageListActionFilter : IActionFilter
    {
        private readonly IPageService pageService;

        public PageListActionFilter(IPageService pageService)
        {
            this.pageService = pageService;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            OxiteModel model = filterContext.Controller.ViewData.Model as OxiteModel;

            if (model == null) return;

            IList<Page> pages = pageService.GetPages();
            model.AddModelItem(new PageListViewModel(pages));
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }
}
