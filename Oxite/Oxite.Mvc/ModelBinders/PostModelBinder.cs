//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Mvc.Extensions;

namespace Oxite.Mvc.ModelBinders
{
    public class PostModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            Post post = new Post
            {
                Title = request.Form.Get("title"),
                Body = request.Form.Get("body"),
                BodyShort = request.Form.Get("bodyShort"),
                Slug = request.Form.Get("slug"),
                CommentingDisabled = request.Form.IsTrue("commentingDisabled"),
                State = EntityState.Normal
            };

            DateTime published;
            if (request.Form.IsTrue("isPublished"))
                post.Published = DateTime.TryParse(request.Form.Get("published"), out published) ? published : DateTime.UtcNow;

            post.Tags = new List<Tag>();
            if (!string.IsNullOrEmpty(request.Form.Get("tags")))
            {
                foreach (string tagName in request.Form.Get("tags").Split(','))
                {
                    if (tagName.Length > 0)
                        post.Tags.Add(new Tag { Name = tagName.Trim() });
                }
            }

            return post;
        }
    }
}
