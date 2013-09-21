//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web.Mvc;
using Oxite.Model;

namespace Oxite.Mvc.ModelBinders
{
    public class PostBaseModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            PostBase postBase = new PostBase
            {
                Slug = controllerContext.RouteData.Values["slug"] as string
            };

            return postBase;
        }
    }
}
