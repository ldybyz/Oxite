//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Mvc.ViewModels;
using Oxite.Services;

namespace Oxite.Mvc.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService tagService;

        public TagController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        public virtual OxiteModelList<KeyValuePair<Tag, int>> Cloud()
        {
            return getTagList(new TagCloudPageContainer(), () => tagService.GetTagsWithPostCount());
        }

        private static OxiteModelList<KeyValuePair<Tag, int>> getTagList(INamedEntity container, Func<IList<KeyValuePair<Tag, int>>> serviceCall)
        {
            var result = new OxiteModelList<KeyValuePair<Tag, int>>
            {
                Container = container,
                List = serviceCall()
            };

            return result;
        }
    }
}