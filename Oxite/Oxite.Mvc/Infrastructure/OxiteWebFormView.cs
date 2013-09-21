//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.IO;
using System.Web.Compilation;
using System.Web.Mvc;
using Oxite.Mvc.Skinning;

namespace Oxite.Mvc.Infrastructure
{
    public class OxiteWebFormView : WebFormView
    {
        private string controllerName;

        public OxiteWebFormView(string viewPath)
            : base(viewPath) { }

        public OxiteWebFormView(string viewPath, string masterPath)
            : base(viewPath, masterPath) { }

        public ISkinEngine SkinEngine { get; set; }

        public override void Render(ViewContext viewContext, TextWriter writer)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            controllerName = viewContext.Controller.ControllerContext.RouteData.GetRequiredString("controller");

            object viewInstance = BuildManager.CreateInstanceFromVirtualPath(ViewPath, typeof(object));
            if (viewInstance == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "An instance of the view '{0}' could not be created.",
                        ViewPath
                        )
                    );
            }

            ViewPage viewPage = viewInstance as ViewPage;
            if (viewPage != null)
            {
                if (!string.IsNullOrEmpty(MasterPath) && !string.IsNullOrEmpty(viewPage.MasterLocation))
                {
                    viewPage.MasterLocation = MasterPath;
                }

                viewPage.ViewData = viewContext.ViewData;
                viewPage.PreInit += viewPage_PreInit;

                viewPage.RenderView(viewContext);

                return;
            }

            base.Render(viewContext, writer);
        }

        private void viewPage_PreInit(object sender, EventArgs e)
        {
            ViewPage viewPage = (ViewPage)sender;

            if (!string.IsNullOrEmpty(viewPage.MasterPageFile))
            {
                string[] locationsSearched;
                string masterName = Path.GetFileNameWithoutExtension(viewPage.MasterPageFile);

                viewPage.MasterPageFile = SkinEngine.FindMasterPath(masterName, controllerName, out locationsSearched);

                if (string.IsNullOrEmpty(viewPage.MasterPageFile))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "The master page '{0}' could not be found. The following locations were searched:\r\n{1}",
                            masterName,
                            string.Join("\r\n", locationsSearched)
                            )
                        );
                }
            }
        }
    }
}
