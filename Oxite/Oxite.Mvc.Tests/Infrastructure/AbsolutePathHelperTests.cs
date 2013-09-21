//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Web.Routing;
using Xunit;
using Oxite.Model;
using Oxite.Routing;

namespace Oxite.Mvc.Tests.Infrastructure
{
    public class AbsolutePathHelperTests
    {
        [Fact]
        public void GetAbsoluteUrlForPostFaultsForNullPost()
        {
            AbsolutePathHelper helper = new AbsolutePathHelper(null, null);

            Assert.Throws<ArgumentNullException>(() => helper.GetAbsolutePath(null));
        }

        [Fact]
        public void GetAbsoluteUrlForPostFaultsForNoSlug()
        {
            AbsolutePathHelper helper = new AbsolutePathHelper(null, null);

            Assert.Throws<ArgumentException>(() => helper.GetAbsolutePath(new Post()));
        }

        [Fact]
        public void GetAbsoluteUrlForPostFaultsForNullArea()
        {
            AbsolutePathHelper helper = new AbsolutePathHelper(null, null);

            Assert.Throws<ArgumentException>(() => helper.GetAbsolutePath(new Post() { Slug = "test" }));
        }

        [Fact]
        public void GetAbsoluteUrlForPostFaultsForNullAreaName()
        {
            AbsolutePathHelper helper = new AbsolutePathHelper(null, null);

            Assert.Throws<ArgumentException>(() => helper.GetAbsolutePath(new Post() { Slug = "test", Area = new Area() }));
        }

        [Fact]
        public void GetAbsoluteUrlReturnsUrlForPost()
        {
            Site site = new Site() { Host = new Uri("http://test.com/foo") };
            RouteCollection routes = new RouteCollection();

            routes.Add("Post", new Route("{slug}/{areaName}", null));

            AbsolutePathHelper helper = new AbsolutePathHelper(site, routes);

            string url = helper.GetAbsolutePath(new Post() { Slug = "test", Area = new Area() { Name = "area" } });

            Assert.Equal("http://test.com/foo/test/area", url);
        }

        [Fact]
        public void GetAbsoluteUrlReturnsUrlForComment()
        {
            Site site = new Site() { Host = new Uri("http://test.com/foo") };
            RouteCollection routes = new RouteCollection();

            routes.Add("PostCommentPermalink", new Route("{areaName}/{slug}#{comment}", null));

            AbsolutePathHelper helper = new AbsolutePathHelper(site, routes);

            string url = helper.GetAbsolutePath(new Post() { Area = new Area() { Name = "area" }, Slug = "test" }, new Comment() { Created = new DateTime(2009, 1, 1, 1, 24, 1, 100) }).Replace("%23", "#");

            Assert.Equal("http://test.com/foo/area/test#c-200901010124011", url);
        }

        [Fact]
        public void GetPostFromUriFaultsOnNullUri()
        {
            AbsolutePathHelper helper = new AbsolutePathHelper(null, null);
            Assert.Throws<ArgumentNullException>(() => helper.GetPostFromUri(null));
        }

        [Fact]
        public void GetPostReturnsPostWithSlugAndAreaWithName()
        {
            Site site = new Site() { Host = new Uri("http://test.com") };
            RouteCollection routes = new RouteCollection();

            routes.Add("Post", new Route("{slug}/{areaName}", null));

            AbsolutePathHelper helper = new AbsolutePathHelper(site, routes);

            Post post = helper.GetPostFromUri(new Uri("http://test.com/postSlug/area"));

            Assert.NotNull(post);
            Assert.Equal("postSlug", post.Slug);
            Assert.Equal("area", post.Area.Name);
        }

        [Fact]
        public void GetPostReturnsNullOnNonMatchingUrl()
        {
            Site site = new Site() { Host = new Uri("http://fail.com/foo") };
            RouteCollection routes = new RouteCollection();

            routes.Add("Post", new Route("{slug}/{areaName}", null));

            AbsolutePathHelper helper = new AbsolutePathHelper(site, routes);

            Post post = helper.GetPostFromUri(new Uri("http://test.com/foo/postSlug/area"));

            Assert.Null(post);
        }

        [Fact]
        public void GetPostReturnsNullOnNonPostUrl()
        {
            Site site = new Site() { Host = new Uri("http://test.com/foo") };
            RouteCollection routes = new RouteCollection();

            routes.Add("Post", new Route("{slug}/{areaName}", null));

            AbsolutePathHelper helper = new AbsolutePathHelper(site, routes);

            Post post = helper.GetPostFromUri(new Uri("http://test.com/foo/postSlug"));

            Assert.Null(post);
        }
    }
}
