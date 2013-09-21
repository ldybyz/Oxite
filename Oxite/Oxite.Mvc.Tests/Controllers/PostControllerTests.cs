//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Web.Routing;
using Xunit;
using Oxite.Model;
using Oxite.Mvc.Controllers;
using Oxite.Mvc.Tests.Fakes;
using Oxite.Mvc.ViewModels;

namespace Oxite.Mvc.Tests.Controllers
{
    public class PostControllerTests
    {
        #region List

        [Fact]
        public void ListReturnsPageOneOnNullPageNumber()
        {
            FakePostService postService = new FakePostService();
            Guid postID = Guid.NewGuid();

            postService.PostPages.Add(0, new List<Post>(new[] { new Post() { ID = postID } }));

            PostController controller = new PostController(postService, null, null, null);

            OxiteModelList<Post> result = controller.List(null, 10, null);

            Assert.Equal(1, result.List.Count);
            Assert.Equal(postID, result.List[0].ID);
            Assert.IsType<HomePageContainer>(result.Container);
            Assert.Equal(10, postService.LastRequestedPageSize);
        }

        [Fact]
        public void ListReturnsPageNOnRequestForPageN()
        {
            FakePostService postService = new FakePostService();
            Guid postID = Guid.NewGuid();

            postService.PostPages.Add(3, new List<Post>(new[] { new Post() { ID = postID } }));

            PostController controller = new PostController(postService, null, null, null);

            OxiteModelList<Post> result = controller.List(4, 10, null);

            Assert.Equal(1, result.List.Count);
            Assert.Equal(postID, result.List[0].ID);
        }

        [Fact]
        public void ListReturnsNullListOnRequestForNonExistantPage()
        {
            FakePostService postService = new FakePostService();
            Guid postID = Guid.NewGuid();

            PostController controller = new PostController(postService, null, null, null);

            OxiteModelList<Post> result = controller.List(2, 10, null);

            Assert.Null(result.List);
        }

        [Fact]
        public void ListUsesIfModifiedSince()
        {
            FakePostService postService = new FakePostService();
            Guid postID = Guid.NewGuid();

            postService.PostPages.Add(0, new List<Post>(new[] { new Post() { ID = postID } }));

            PostController controller = new PostController(postService, null, null, null);

            DateTime sinceDate = DateTime.Now;

            OxiteModelList<Post> result = controller.List(null, 10, sinceDate);

            Assert.Equal(sinceDate, postService.LastRequestedSinceDate);
        }

        #endregion

        #region ListByArea

        [Fact]
        public void ListByAreaSetsAreaOnModel()
        {
            FakePostService postService = new FakePostService();
            FakeAreaService areaService = new FakeAreaService();

            Area area = new Area() { Name = "test"};

            areaService.StoredAreas.Add("test", area);

            PostController controller = new PostController(postService, null, areaService, null);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(new FakeHttpContext("~/"), new RouteData(), controller);

            OxiteModelList<Post> result = controller.ListByArea(null, 10, area, null);

            Assert.Equal(area.Name, result.Container.Name);
        }

        [Fact]
        public void ListByAreaReturnsNullForInvalidArea()
        {
            FakePostService postService = new FakePostService();
            FakeAreaService areaService = new FakeAreaService();

            Area area = new Area() { Name = "test" };

            PostController controller = new PostController(postService, null, areaService, null);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(new FakeHttpContext("~/"), new RouteData(), controller);

            OxiteModelList<Post> result = controller.ListByArea(null, 10, area, null);

            Assert.Null(result);
        }

        #endregion

        #region ListByTag

        [Fact]
        public void ListByTagSetsTagOnModel()
        {
            FakePostService postService = new FakePostService();
            FakeTagService tagService = new FakeTagService();

            Tag tag = new Tag() { Name = "Test" };

            tagService.StoredTags.Add("Test", tag);

            PostController controller = new PostController(postService, tagService, null, null);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(new FakeHttpContext("~/"), new RouteData(), controller);

            OxiteModelList<Post> result = controller.ListByTag(null, 0, tag, null);

            Assert.Equal(tag.Name, result.Container.Name);
        }

        [Fact]
        public void ListByTagReturnsNullForInvalidArea()
        {
            FakePostService postService = new FakePostService();
            FakeTagService tagService = new FakeTagService();

            Tag tag = new Tag() { Name = "Test" };

            PostController controller = new PostController(postService, tagService, null, null);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(new FakeHttpContext("~/"), new RouteData(), controller);

            OxiteModelList<Post> result = controller.ListByTag(null, 0, tag, null);

            Assert.Null(result);
        }

        #endregion

        #region ListBySearch

        [Fact]
        public void ListBySearchSetsSearchContainerOnModel()
        {
            FakePostService postService = new FakePostService();

            PostController controller = new PostController(postService, null, null, null);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(new FakeHttpContext("~/"), new RouteData(), controller);

            OxiteModelList<Post> result = controller.ListBySearch(null, 0, new SearchCriteria() { Term = "test" }, null);

            Assert.IsType<SearchPageContainer>(result.Container);
        }

        [Fact]
        public void ListBySearchReturnsHomePageOnEmptySearch()
        {
            FakePostService postService = new FakePostService();

            PostController controller = new PostController(postService, null, null, null);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(new FakeHttpContext("~/"), new RouteData(), controller);

            OxiteModelList<Post> result = controller.ListBySearch(null, 0, new SearchCriteria(), null);

            Assert.IsType<HomePageContainer>(result.Container);
        }

        #endregion

        #region ListByArchive

        [Fact]
        public void ListByArchiveSetsHomePageContainerOnModel()
        {
            FakePostService postService = new FakePostService();

            PostController controller = new PostController(postService, null, null, null);
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(new FakeHttpContext("~/"), new RouteData(), controller);

            OxiteModelList<Post> result = controller.ListByArchive(0, new ArchiveData());

            Assert.IsType<ArchiveContainer>(result.Container);
            Assert.Equal(new ArchiveData(), ((ArchiveContainer)result.Container).ArchiveData);
        }

        #endregion

        #region Item

        [Fact]
        public void ItemReturnsNullOnNotFoundArea()
        {
            FakeAreaService areaService = new FakeAreaService();

            PostController controller = new PostController(null, null, areaService, null);

            OxiteModelItem<Post> result = controller.Item(new Area() { Name = "Test" }, null);

            Assert.Null(result);
        }

        [Fact]
        public void ItemReturnsNullOnNotFoundPost()
        {
            FakeAreaService areaService = new FakeAreaService();
            FakePostService postService = new FakePostService();

            areaService.StoredAreas.Add("test", new Area());

            PostController controller = new PostController(postService, null, areaService, null);

            OxiteModelItem<Post> result = controller.Item(new Area() { Name = "test" }, new Post());

            Assert.Null(result);
        }

        [Fact]
        public void ItemReturnsPost()
        {
            FakeAreaService areaService = new FakeAreaService();
            FakePostService postService = new FakePostService();

            Guid postID = Guid.NewGuid();
            Guid areaID = Guid.NewGuid();

            areaService.StoredAreas.Add("test", new Area() {ID = areaID});
            postService.AddedPosts.Add(new Post() { ID = postID, Slug = "Slug" });

            PostController controller = new PostController(postService, null, areaService, null);

            OxiteModelItem<Post> result = controller.Item(new Area() { Name = "test" }, new Post() { Slug = "Slug" });

            Assert.Equal(postID, result.Item.ID);
            Assert.IsType<Area>(result.Container);
            Assert.Equal(areaID, (result.Container as Area).ID);
        }

        #endregion
    }
}
