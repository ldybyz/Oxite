//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;
using Oxite.LinqToSqlDataProvider;
using Oxite.Model;

namespace Oxite.Mvc.Tests.Repository
{
    // Note: All tests in this class actually hit a database and are therefore slow.
    // You need SQL Express to run these. Apologies to all those TDD people out there 
    // but I want to make sure the Linq queries actually work when turned into SQL.
    public class LinqToSqlRepositoryTests : IUseFixture<LinqToSqlFixture>
    {
        private LinqToSqlFixture fixture;

        [Fact(Skip = "Slow database test")]
        public void GetPostsReturnsPosts()
        {
            PostRepository repo = new PostRepository(fixture.Context, new Site() { ID = Guid.Empty }, null);

            IQueryable<Post> allPosts = repo.GetPosts(false);

            IList<Post> postList = allPosts.ToList();

            Assert.NotNull(postList);
            Assert.Equal(1, postList.Count);
        }

        [Fact(Skip = "Slow database test")]
        public void GetPostsFilteredByTagReturnsPosts()
        {
            PostRepository repo = new PostRepository(fixture.Context, new Site() { ID = Guid.Empty }, null);

            IQueryable<Post> byTag = repo.GetPosts(new Tag() { Name = "Oxite" });

            IList<Post> postList = byTag.ToList();

            Assert.NotNull(postList);
            Assert.Equal(1, postList.Count);
        }

        [Fact(Skip = "Slow database test")]
        public void GetPostsFilteredByAreaReturnsPosts()
        {
            PostRepository repo = new PostRepository(fixture.Context, new Site() { ID = Guid.Empty }, null);

            IQueryable<Post> byArea = repo.GetPosts(new Area() { Name = "Blog" });

            IList<Post> postList = byArea.ToList();

            Assert.NotNull(postList);
            Assert.Equal(1, postList.Count);
        }

        [Fact(Skip = "Slow database test")]
        [AutoRollback]
        public void SavePostWithNewPost()
        {
            PostRepository repo = new PostRepository(fixture.Context, new Site() { ID = Guid.Empty }, null);

            Post newPost = new Post()
            {
                Area = new Area() { Name = "Blog" },
                Body = "Body",
                BodyShort = "BodyShort",
                Creator = new User() { Name = "Admin" },
                Slug = "Slug",
                State = EntityState.Normal,
                Tags = new[] { new Tag() { Name = "Oxite" }, new Tag() { Name = "Test"}},
                Title = "Title",
            };

            repo.Save(newPost);

            Post savedPost = repo.GetPost(new Area() { Name = "Blog" }, "Slug");

            Assert.NotNull(savedPost);
            Assert.Equal("Blog", savedPost.Area.Name);
            Assert.Equal("Body", savedPost.Body);
            Assert.Equal("BodyShort", savedPost.BodyShort);
            Assert.Equal("Admin", savedPost.Creator.Name);
            Assert.Equal("Slug", savedPost.Slug);
            Assert.Equal(EntityState.Normal, savedPost.State);
            Assert.Equal(2, savedPost.Tags.Count);
            Assert.True(savedPost.Tags.Select(t => t.Name).Contains("Oxite"));
            Assert.True(savedPost.Tags.Select(t => t.Name).Contains("Test"));
            Assert.Equal("Title", savedPost.Title);
        }

        [Fact(Skip = "Slow database test")]
        [AutoRollback]
        public void SavePostUpdatePost()
        {
            PostRepository repo = new PostRepository(fixture.Context, new Site() { ID = Guid.Empty }, null);

            Post newPost = new Post()
            {
                ID = new Guid("0E1684CA-C627-4E87-9F21-843B074D9E4B"),
                Area = new Area() { Name = "Blog" },
                Body = "Body",
                BodyShort = "BodyShort",
                Creator = new User() { Name = "Admin" },
                Slug = "Slug",
                State = EntityState.Normal,
                Tags = new[] { new Tag() { Name = "Test" } },
                Title = "Title",
            };

            repo.Save(newPost);

            Post savedPost = repo.GetPost(new Area() { Name = "Blog" }, "Slug");

            Assert.NotNull(savedPost);
            Assert.Equal("Blog", savedPost.Area.Name);
            Assert.Equal("Body", savedPost.Body);
            Assert.Equal("BodyShort", savedPost.BodyShort);
            Assert.Equal("Admin", savedPost.Creator.Name);
            Assert.Equal("Slug", savedPost.Slug);
            Assert.Equal(EntityState.Normal, savedPost.State);
            Assert.Equal(1, savedPost.Tags.Count);
            Assert.True(savedPost.Tags.Select(t => t.Name).Contains("Test"));
            Assert.Equal("Title", savedPost.Title);
        }

        [Fact(Skip = "Slow database test")]
        [AutoRollback]
        public void SaveCommentNewAnonComment()
        {
            PostRepository repo = new PostRepository(fixture.Context, new Site() { ID = Guid.Empty }, null);

            Post existingPost = repo.GetPost(new Area() { Name = "Blog" }, "Hello-World");

            Comment newComment = new Comment()
            {
                Body = "Body",
                Creator = new UserBase() { Email = "test@microsoft.com", HashedEmail = "XXXXXX", Name = "Test", Url = "http://microsoft.com" },
                CreatorIP = 1L,
                CreatorUserAgent = "UA",
                Language = new Language() { Name = "en" },
                State = EntityState.Normal
            };

            repo.SaveComment(existingPost, newComment);

            Post editedPost = repo.GetPost(new Area() { Name = "Blog" }, "Hello-World");

            Comment addedComment = editedPost.Comments.Where(c => c.Body == "Body").FirstOrDefault();
            Assert.NotNull(addedComment);
            Assert.Equal("test@microsoft.com", addedComment.Creator.Email);
            Assert.Equal("XXXXXX", addedComment.Creator.HashedEmail);
            Assert.Equal("Test", addedComment.Creator.Name);
            Assert.Equal("http://microsoft.com", addedComment.Creator.Url);
            Assert.Equal(1L, addedComment.CreatorIP);
            Assert.Equal("UA", addedComment.CreatorUserAgent);
            Assert.Equal("en", addedComment.Language.Name);
            Assert.Equal(EntityState.Normal, addedComment.State);
        }

        [Fact(Skip = "Slow database test")]
        [AutoRollback]
        public void SaveCommentNewAuthedComment()
        {
            PostRepository repo = new PostRepository(fixture.Context, new Site() { ID = Guid.Empty }, null);

            Post existingPost = repo.GetPost(new Area() { Name = "Blog" }, "Hello-World");

            Comment newComment = new Comment()
            {
                Body = "Body",
                Creator = new User() { Name = "Admin" },
                CreatorIP = 1L,
                CreatorUserAgent = "UA",
                Language = new Language() { Name = "en" },
                State = EntityState.Normal
            };

            repo.SaveComment(existingPost, newComment);

            Post editedPost = repo.GetPost(new Area() { Name = "Blog" }, "Hello-World");

            Comment addedComment = editedPost.Comments.Where(c => c.Body == "Body").FirstOrDefault();
            Assert.NotNull(addedComment);
            Assert.Equal("Oxite Administrator", addedComment.Creator.Name);
            Assert.Equal(1L, addedComment.CreatorIP);
            Assert.Equal("UA", addedComment.CreatorUserAgent);
            Assert.Equal("en", addedComment.Language.Name);
            Assert.Equal(EntityState.Normal, addedComment.State);
        }

        #region IUseFixture<LinqToSqlFixture> Members

        public void SetFixture(LinqToSqlFixture data)
        {
            this.fixture = data;
        }

        #endregion
    }
}
