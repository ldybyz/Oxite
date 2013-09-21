//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Oxite.Mvc.Infrastructure
{
    public class OxiteControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer container;

        public OxiteControllerFactory(IUnityContainer container)
        {
            this.container = container;
        }

        protected override IController GetControllerInstance(Type controllerType)
        {
            IController icontroller = container.Resolve(controllerType) as IController;
            if (typeof(Controller).IsAssignableFrom(controllerType))
            {
                Controller controller = icontroller as Controller;

                if (controller != null)
                    controller.ActionInvoker = container.Resolve<IActionInvoker>();

                return icontroller;
            }

            return icontroller;
        }
    }
}
