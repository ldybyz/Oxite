//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Oxite.Model;
using Oxite.Mvc.Controllers;
using Oxite.Mvc.Tests.Fakes;
using Oxite.Mvc.ViewModels;
using Xunit;

namespace Oxite.Mvc.Tests.Controllers
{
    public class TrackbackControllerTests
    {
        [Fact]
        public void AddFaultsIfPostIsNotFound()
        {
            FakePostService postService = new FakePostService();

            TrackbackController controller = new TrackbackController(postService);

            TrackbackViewModel result = controller.Add(Guid.NewGuid(), null);

            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorCode);
        }

        [Fact]
        public void AddSavesNewTrackbackIfExistingTrackbackNotFound()
        {
            FakePostService postService = new FakePostService();

            Guid postId = Guid.NewGuid();
            postService.AddedPosts.Add(new Post() { ID = postId, Trackbacks = new List<Trackback>() });

            FormCollection form = new FormCollection();

            form.Add("url", "http://test");
            form.Add("title", "Test");
            form.Add("blog_name", "Test Blog");
            form.Add("excerpt", "Test excerpt");

            TrackbackController controller = new TrackbackController(postService);

            controller.Add(postId, form);

            Assert.NotNull(postService.AddedTrackback);
            Assert.Equal("http://test", postService.AddedTrackback.Url);
            Assert.Equal("Test", postService.AddedTrackback.Title);
            Assert.Equal("Test Blog", postService.AddedTrackback.BlogName);
            Assert.Equal("Test excerpt", postService.AddedTrackback.Body);
            Assert.Equal("", postService.AddedTrackback.Source);
        }

        [Fact]
        public void AddUpdatesTrackbackIfExistingTrackbackFound()
        {
            FakePostService postService = new FakePostService();

            Guid postId = Guid.NewGuid();
            postService.AddedPosts.Add(new Post() { ID = postId, Trackbacks = new List<Trackback>(new[] { new Trackback() { Url = "http://test" } }) });

            FormCollection form = new FormCollection();

            form.Add("url", "http://test");
            form.Add("title", "Test");
            form.Add("blog_name", "Test Blog");
            form.Add("excerpt", "Test excerpt");

            TrackbackController controller = new TrackbackController(postService);

            controller.Add(postId, form);

            Assert.NotNull(postService.AddedTrackback);
            Assert.Equal("Test", postService.AddedTrackback.Title);
            Assert.Equal("Test Blog", postService.AddedTrackback.BlogName);
            Assert.Equal("Test excerpt", postService.AddedTrackback.Body);
            Assert.Null(postService.AddedTrackback.IsTargetInSource);
        }

        [Fact]
        public void AddReturnsSuccessResult()
        {
            FakePostService postService = new FakePostService();

            Guid postId = Guid.NewGuid();
            postService.AddedPosts.Add(new Post() { ID = postId, Trackbacks = new List<Trackback>(new[] { new Trackback() { Url = "http://test" } }) });

            FormCollection form = new FormCollection();

            form.Add("url", "http://test");
            form.Add("title", "Test");
            form.Add("blog_name", "Test Blog");
            form.Add("excerpt", "Test excerpt");

            TrackbackController controller = new TrackbackController(postService);

            TrackbackViewModel result = controller.Add(postId, form);

            Assert.Equal(0, result.ErrorCode);
            Assert.True(string.IsNullOrEmpty(result.ErrorMessage));
        }
    }
}
