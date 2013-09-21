//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using System.Web.Mvc;

namespace Oxite.Mvc.Tests.Fakes
{
    public class FakeActionDescriptor : ActionDescriptor
    {
        public string Name { get; set; }

        public override string ActionName
        {
            get { return this.Name; }
        }

        public override ControllerDescriptor ControllerDescriptor
        {
            get { return null; }
        }

        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            return null;
        }

        public override ParameterDescriptor[] GetParameters()
        {
            return null;
        }
    }
}
