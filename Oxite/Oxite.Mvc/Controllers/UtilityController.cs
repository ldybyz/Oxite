//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Oxite.Extensions;
using Oxite.Model;
using Oxite.Mvc.ViewModels;
using Oxite.Services;

namespace Oxite.Mvc.Controllers
{
    public class UtilityController : Controller
    {
        private readonly IPostService postService;

        public UtilityController(IPostService postService)
        {
            this.postService = postService;
        }

        public virtual ViewResult OpenSearch()
        {
            return View(new OxiteModel());
        }

        public virtual ViewResult OpenSearchOSDX()
        {
            return View(new OxiteModel());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ContentResult ComputeHash(string value)
        {
            return Content(value.ComputeHash(), "text/plain");
        }

        public virtual OxiteModel RobotsTxt()
        {
            return new OxiteModel();
        }

        public virtual OxiteModelList<DateTime> SiteMapIndex()
        {
            IList<DateTime> postDateGroups = postService.GetPostDateGroups();

            return new OxiteModelList<DateTime> { List = postDateGroups };
        }

        public virtual OxiteModelList<Post> SiteMap(int year, int month)
        {
            DateTime startDate = new DateTime(year, month, 1);
            IList<Post> posts = postService.GetPosts(startDate, startDate.AddMonths(1));

            return new OxiteModelList<Post> { List = posts };
        }
    }
}
