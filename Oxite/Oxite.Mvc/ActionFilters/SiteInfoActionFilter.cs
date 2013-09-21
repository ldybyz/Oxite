//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Oxite.Infrastructure;
using Oxite.Model;
using Oxite.Mvc.ViewModels;

namespace Oxite.Mvc.ActionFilters
{
    public class SiteInfoActionFilter : IActionFilter
    {
        private readonly AppSettingsHelper appSettings;
        private readonly Site site;

        public SiteInfoActionFilter(AppSettingsHelper appSettings, Site site)
        {
            this.appSettings = appSettings;
            this.site = site;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            OxiteModel model = filterContext.Controller.ViewData.Model as OxiteModel;

            if (model != null)
            {
                model.Site = new SiteViewModel(site, appSettings.GetString("SiteName", "Oxite"));
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }
}
