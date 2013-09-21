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
    public class AreaListActionFilter : IActionFilter
    {
        private readonly IAreaService areaService;

        public AreaListActionFilter(IAreaService areaService)
        {
            this.areaService = areaService;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            OxiteModel model = filterContext.Controller.ViewData.Model as OxiteModel;

            if (model == null) return;

            IList<Area> areas = areaService.GetAreas();
            model.AddModelItem(new AreaListViewModel(areas));
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }
}
