//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Oxite.Model;

namespace Oxite.Mvc.ModelBinders
{
    public class TagModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Tag tag = new Tag {
                Name = controllerContext.RouteData.Values["tagName"] as string
            };

            return tag;
        }
    }
}
