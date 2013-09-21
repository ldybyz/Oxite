//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web;
using System.Web.Mvc;
using Oxite.Extensions;
using Oxite.Model;
using Oxite.Mvc.Extensions;

namespace Oxite.Mvc.ModelBinders
{
    public class PageModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            Page post = new Page
            {
                Title = request.Form.Get("title"),
                Body = request.Form.Get("body"),
                BodyShort = request.Form.Get("bodyShort"),
                Slug = request.Form.Get("slug")
            };

            if (request.Form.IsTrue("isPublished"))
            {
                DateTime published;

                post.Published = DateTime.TryParse(request.Form.Get("published"), out published) ? published : DateTime.UtcNow;
            }

            if (!string.IsNullOrEmpty(request.Form.Get("parentID")))
            {
                Guid parentID;

                if (request.Form.Get("parentID").GuidTryParse(out parentID))
                {
                    post.Parent = new Page { ID = parentID };
                }
            }

            return post;
        }
    }
}
