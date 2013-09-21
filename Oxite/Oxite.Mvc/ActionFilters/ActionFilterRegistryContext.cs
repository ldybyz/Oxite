//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;

namespace Oxite.Mvc.ActionFilters
{
    public class ActionFilterRegistryContext
    {
        public ActionFilterRegistryContext(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            ActionDescriptor = actionDescriptor;
            ControllerContext = controllerContext;
        }

        public ActionDescriptor ActionDescriptor { get; set; }
        public ControllerContext ControllerContext { get; set; }
    }
}
