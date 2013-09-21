//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Linq;
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Mvc.ViewModels;
using Oxite.Services;

namespace Oxite.Mvc.Controllers
{
    public class CommentController : Controller
    {
        private readonly IPostService postService;
        private readonly ITagService tagService;
        private readonly IAreaService areaService;

        public CommentController(IPostService postService, ITagService tagService, IAreaService areaService)
        {
            this.postService = postService;
            this.tagService = tagService;
            this.areaService = areaService;
        }

        public virtual OxiteModelList<ParentAndChild<PostBase, Comment>> List()
        {
            return new OxiteModelList<ParentAndChild<PostBase, Comment>>
            {
                List = postService.GetComments(),
                Container = new HomePageContainer()
            };
        }

        [ActionName("ListForAdmin")]
        public virtual OxiteModelList<ParentAndChild<PostBase, Comment>> List(int? pageNumber, int pageSize)
        {
            int pageIndex = pageNumber.HasValue ? pageNumber.Value - 1 : 0;

            return getCommentList(new HomePageContainer(), () => postService.GetComments(pageIndex, pageSize, true, true));
        }

        public virtual OxiteModelList<ParentAndChild<PostBase, Comment>> ListByTag(Tag tagInput)
        {
            Tag tag = tagService.GetTag(tagInput.Name);

            if (tag == null)
                return null;

            return new OxiteModelList<ParentAndChild<PostBase, Comment>>
            {
                List = postService.GetComments(tag),
                Container = tag
            };
        }

        public virtual OxiteModelList<ParentAndChild<PostBase, Comment>> ListByArea(Area areaInput)
        {
            Area area = areaService.GetArea(areaInput.Name);

            if (area == null)
                return null;

            return new OxiteModelList<ParentAndChild<PostBase, Comment>>
            {
                List = postService.GetComments(area),
                Container = area
            };
        }

        public virtual OxiteModelList<ParentAndChild<PostBase, Comment>> ListByPost(Area areaInput, Post postInput)
        {
            Area area = areaService.GetArea(areaInput.Name);

            if (area == null)
                return null;

            Post post = postService.GetPost(area, postInput.Slug);

            if (post == null)
                return null;

            return new OxiteModelList<ParentAndChild<PostBase, Comment>>
            {
                List = postService.GetComments(post).Select(c => new ParentAndChild<PostBase, Comment> { Parent = post, Child = c }).ToList(),
                Container = post
            };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Approve(Area areaInput, PostBase postBaseInput, Comment commentInput, string returnUri)
        {
            Area area = areaService.GetArea(areaInput.Name);

            if (area == null)
                return null;

            Post post = postService.GetPost(area, postBaseInput.Slug);

            if (post == null)
                return null;

            try
            {
                postService.ApproveComment(post, commentInput.ID);

                if (!string.IsNullOrEmpty(returnUri))
                    return new RedirectResult(returnUri);

                return new JsonResult { Data = true };
            }
            catch
            {
                return new JsonResult { Data = false };
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Remove(Area areaInput, PostBase postBaseInput, Comment commentInput, string returnUri)
        {
            Area area = areaService.GetArea(areaInput.Name);

            if (area == null)
                return null;

            Post post = postService.GetPost(area, postBaseInput.Slug);

            if (post == null)
                return null;

            try
            {
                postService.RemoveComment(post, commentInput.ID);
            }
            catch
            {
                return new JsonResult { Data = false };
            }

            if (!string.IsNullOrEmpty(returnUri))
                return new RedirectResult(returnUri);

            return new JsonResult { Data = true };
        }

        private static OxiteModelList<ParentAndChild<PostBase, Comment>> getCommentList(INamedEntity container, Func<IPageOfList<ParentAndChild<PostBase, Comment>>> serviceCall)
        {
            OxiteModelList<ParentAndChild<PostBase, Comment>> result = new OxiteModelList<ParentAndChild<PostBase, Comment>>
            {
                Container = container,
                List = serviceCall()
            };

            return result;
        }
    }
}
