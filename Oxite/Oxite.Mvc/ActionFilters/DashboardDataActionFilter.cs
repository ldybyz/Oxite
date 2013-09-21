//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System.Collections.Generic;
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Mvc.ViewModels;
using Oxite.Services;

namespace Oxite.Mvc.ActionFilters
{
    public class DashboardDataActionFilter : IActionFilter
    {
        private readonly IPostService postService;
        private readonly IAreaService areaService;

        public DashboardDataActionFilter(IPostService postService, IAreaService areaService)
        {
            this.postService = postService;
            this.areaService = areaService;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            OxiteModel model = filterContext.Controller.ViewData.Model as OxiteModel;

            if (model == null) return;

            //recent posts - all up
            IList<Post> posts = postService.GetPostsWithDrafts(0, 5);

            //recent comments - all up
            IList<ParentAndChild<PostBase, Comment>> comments = postService.GetComments(0, 10, true, true);

            IList<Area> areas = areaService.GetAreas();

            model.AddModelItem(new AdminDataViewModel(posts, comments, areas));
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }
}
