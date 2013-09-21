//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Mvc.Infrastructure;
using Oxite.Mvc.ViewModels;

namespace Oxite.Mvc.ActionFilters
{
    public class AntiForgeryActionFilter : IActionFilter
    {
        private readonly Site site;

        public AntiForgeryActionFilter(Site site)
        {
            this.site = site;
        }

        #region IActionFilter Members

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            OxiteModel model = filterContext.Controller.ViewData.Model as OxiteModel;

            if (model != null)
            {
                model.AntiForgeryToken = new AntiForgeryToken(site.ID.ToString());
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        #endregion
    }
}
