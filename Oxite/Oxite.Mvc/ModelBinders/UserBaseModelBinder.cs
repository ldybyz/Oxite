//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Web;
using System.Web.Mvc;
using Oxite.Model;

namespace Oxite.Mvc.ModelBinders
{
    public class UserBaseModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            UserBase userBase = new UserBase
            {
                Name = request.Form.Get("name"),
                Email = request.Form.Get("email"),
                HashedEmail = request.Form.Get("hashedEmail"),
                Url = request.Form.Get("url")
            };

            return userBase;
        }
    }
}
