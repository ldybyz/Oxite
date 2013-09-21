//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;
using Oxite.Mvc.Skinning;

namespace Oxite.Mvc.Infrastructure
{
    public class OxiteViewEngine : IViewEngine
    {
        public OxiteViewEngine(ISkinEngine skinEngine)
        {
            SkinEngine = skinEngine;
        }

        public string SkinName { get; set; }
        public ISkinEngine SkinEngine { get; private set; }

        protected virtual IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new OxiteWebFormView(partialPath) { SkinEngine = SkinEngine };
        }

        protected virtual IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new OxiteWebFormView(viewPath, masterPath) { SkinEngine = SkinEngine };
        }

        public virtual ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentNullException("partialViewName");
            }

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string[] locationsSearched;
            string partialPath = SkinEngine.FindPartialViewPath(partialViewName, controllerName, out locationsSearched);

            return !string.IsNullOrEmpty(partialPath)
                       ? new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this)
                       : new ViewEngineResult(locationsSearched);
        }

        public virtual ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentNullException("viewName");
            }

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string[] viewLocationsSearched;
            string viewPath = SkinEngine.FindViewPath(viewName, controllerName, out viewLocationsSearched);

            if (string.IsNullOrEmpty(viewPath))
            {
                return new ViewEngineResult(viewLocationsSearched);
            }

            string[] masterLocationsSearched;
            string masterPath = SkinEngine.FindMasterPath(masterName, controllerName, out masterLocationsSearched);

            if (!string.IsNullOrEmpty(masterName) && string.IsNullOrEmpty(masterPath))
            {
                return new ViewEngineResult(masterLocationsSearched);
            }

            return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
        }

        public virtual void ReleaseView(ControllerContext controllerContext, IView view)
        {
            IDisposable disposable = view as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}